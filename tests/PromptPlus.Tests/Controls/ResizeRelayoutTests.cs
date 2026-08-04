using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.FileExec;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Controls.MultiFile;
using PromptPlusLibrary.Core;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // BaseControlPrompt's resize/relayout path (Run()'s _pendingResize branch,
    // Controls/Common/BaseControlPrompt.cs) — previously UNTESTABLE because
    // VirtualTerminal.RaiseResize only fired the SizeChanged event without actually resizing the
    // VirtualScreen. Now that RaiseResize resizes the screen for real (tests/_driver-src/
    // VirtualScreen.cs Resize), this exercises the relayout for the first time. Input is used as
    // the vehicle since its render path is simple and well understood by the existing suite; the
    // relayout machinery itself lives entirely in the base class.
    // Relies on fixed wall-clock waits (WaitUntilAllKeysConsumed, WaitUntilRenderSettles) to observe
    // the resize/relayout in flight deterministically — the same category of timing dependency
    // SerializedGlobalStateCollection's background-task-timing half was introduced for. Sharing that
    // collection serializes it against the other timing-sensitive classes instead of competing
    // with them for thread-pool slots.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class ResizeRelayoutTests : IDisposable
    {
        private readonly IFileSystem _originalHistoryFs = FileHistory.FileSystem;
        private readonly MockFileSystem _mockHistoryFs = new();
        private readonly IFileSystem _originalFileFs = FileControl.FileSystem;
        private readonly IFileSystem _originalMultiFileFs = MultiFileControl.FileSystem;

        public ResizeRelayoutTests() => FileHistory.FileSystem = _mockHistoryFs;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            FileHistory.FileSystem = _originalHistoryFs;
            FileControl.FileSystem = _originalFileFs;
            MultiFileControl.FileSystem = _originalMultiFileFs;
        }

        private static VirtualTerminal MakeTerminal(int width, int height)
            => VirtualTerminal.Create(o => { o.SupportsUnicode = false; o.Width = width; o.Height = height; });

        private static MockFileSystem MakeFileSystemWithLongName(out string root)
        {
            root = Path.Combine(Path.GetTempPath(), "resize-root");
            var fs = new MockFileSystem();
            fs.AddDirectory(root);
            fs.AddFile(Path.Combine(root, new string('x', 100) + ".txt"), new MockFileData(new byte[2048]));
            return fs;
        }

        private static IInputControl MakeInput(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Input("Name");

        [Fact]
        public void A_within_bounds_resize_preserves_the_in_progress_edit_cursor_position()
        {
            // Regression for a real bug: TryResult's inner loop set _updatePosAnswerBuffer = true
            // before waiting for a key, but only reset it to false on the "normal key processed"
            // path — the press.IsResize branch broke out of the loop with the flag still true. The
            // render pass that follows a resize then found the stale flag and reloaded _inputdata
            // from _lastinput + ToHome(), snapping the edit cursor back to the start of the text
            // even though nothing about the in-progress edit should change on a mere resize.
            var vt = MakeTerminal(100, 24);
            var control = MakeInput(vt);
            _ = vt.Keys.Type("hello");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => control.Run(cts.Token));

            WaitUntilAllKeysConsumed(vt);

            _ = vt.GetCursorPosition().Should().Be((11, 0), "cursor must sit right after 'hello' (prompt 'Name: ' is 6 columns) before the resize");

            vt.RaiseResize(85, 24);
            WaitUntilRenderSettles();

            _ = vt.Width.Should().Be(85, "RaiseResize must actually resize the underlying screen, not just fire the event");
            _ = vt.TextAt(0, 0, 11).Should().Be("Name: hello", "the resize must not touch the already-typed content");
            _ = vt.GetCursorPosition().Should().Be((11, 0), "the edit cursor must stay right after 'hello' — it must NOT snap back to Home because of a resize");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeTrue();
        }

        [Fact]
        public void A_within_bounds_resize_does_not_leave_duplicated_or_stale_content_on_screen()
        {
            var vt = MakeTerminal(100, 24);
            var control = MakeInput(vt);
            _ = vt.Keys.Type("hello");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => control.Run(cts.Token));

            WaitUntilAllKeysConsumed(vt);

            vt.RaiseResize(80, 12);
            WaitUntilRenderSettles();

            _ = vt.TextAt(0, 0, 11).Should().Be("Name: hello");
            // Only row 0 (answer) and the tooltip row below it belong to this control; every other
            // row must remain untouched blank space, not leftover fragments from the pre-resize frame.
            for (int row = 2; row < vt.Height; row++)
            {
                _ = vt.TextAt(row, 0, vt.Width).Trim().Should().BeEmpty($"row {row} must not contain stale content after the resize");
            }

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = runTask.GetAwaiter().GetResult();
        }

        [Fact]
        public void A_within_bounds_resize_preserves_a_scrolled_answer_preview_on_Select()
        {
            // Regression for the same bug pattern in SelectControl (also present in
            // MultiSelect/Table/MultiTable — same fix applied by direct analogy after confirming the
            // code shape is identical, not re-probed for each one). TreeSelect had this too until it was
            // refactored onto WriteAnswerViewport (see the TreeSelect-specific test below, which covers
            // that control's own mechanism for restoring the same guarantee): _updatePosAnswerBuffer
            // is force-set true at the top of every loop iteration and only narrowed back down by
            // specific keys (e.g. navigating the long answer preview with End). The press.IsResize
            // branch used to break out with the flag stuck at its force-set value, so a resize right
            // after scrolling the preview snapped it back to Home. Select additionally requires
            // restoring the PRE-iteration value (not just false) because, unlike Input, `true` is
            // the normal steady state here (it drives "reload the preview for a newly-selected item").
            var vt = MakeTerminal(100, 24);
            string longText = new('X', 90);
            var control = new PromptPlusControls(vt, new PromptConfig())
                .Select<string>("Choose")
                .AddItems([longText, "B", "C"]);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);

            _ = vt.Keys.Enqueue(ConsoleKey.End);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var (Left, Top) = vt.GetCursorPosition();

            vt.RaiseResize(90, 24);
            WaitUntilRenderSettles();

            _ = vt.GetCursorPosition().Left.Should().BeGreaterThan(50,
                "the resize must not snap the scrolled answer preview back to Home — it stayed near " +
                $"the end (col {Left} before resize)");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = runTask.GetAwaiter().GetResult();
        }

        [Fact]
        public async Task A_within_bounds_resize_preserves_a_scrolled_answer_preview_on_TreeSelect()
        {
            // TreeSelect used to carry the same _updatePosAnswerBuffer resize-preservation dance as
            // Select (by direct analogy, per the comment above) until it was replaced with the
            // shared WriteAnswerViewport helper to fix an unrelated bug: the answer line's own
            // scroll keys (Home/End/Left/Right) were never wired to the buffer WriteAnswer actually
            // rendered from, so scrolling did nothing at all. That fix used WriteAnswerViewport's
            // default behavior, which re-anchors to Home on any resize — silently dropping the
            // resize-preservation TreeSelect had before. WriteAnswerViewport now takes an opt-in
            // preservePositionOnResize flag; TreeSelect passes true to restore parity with Select.
            var vt = MakeTerminal(100, 24);
            string longExtra = new('X', 90);
            var tree = new PromptPlusControls(vt, new PromptConfig())
                .TreeSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .ExtraInfo(_ => longExtra);
            _ = tree.AddLast("Leaf");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => tree.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);

            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            _ = vt.Keys.Enqueue(ConsoleKey.End);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var (Left, Top) = vt.GetCursorPosition();

            vt.RaiseResize(90, 24);
            WaitUntilRenderSettles();

            _ = vt.GetCursorPosition().Left.Should().BeGreaterThan(50,
                "the resize must not snap the scrolled answer preview back to Home — it stayed near " +
                $"the end (col {Left} before resize)");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = await runTask;
        }

        [Fact]
        public async Task A_within_bounds_resize_preserves_a_scrolled_answer_preview_on_TreeMultiSelect()
        {
            // TreeMultiSelect never had the _updatePosAnswerBuffer dance (it always used
            // WriteAnswerViewport), so this is new coverage rather than a regression check — added
            // for parity with Select/MultiSelect/TableSelect/TableMultiSelect/TreeSelect, per the same
            // preservePositionOnResize opt-in used for TreeSelect above.
            var vt = MakeTerminal(100, 24);
            string longExtra = new('X', 90);
            var tree = new PromptPlusControls(vt, new PromptConfig())
                .TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .ExtraInfo(_ => longExtra);
            _ = tree.AddLast("Leaf");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => tree.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);

            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            _ = vt.Keys.Enqueue(ConsoleKey.End);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var (Left, Top) = vt.GetCursorPosition();

            vt.RaiseResize(90, 24);
            WaitUntilRenderSettles();

            _ = vt.GetCursorPosition().Left.Should().BeGreaterThan(50,
                "the resize must not snap the scrolled answer preview back to Home — it stayed near " +
                $"the end (col {Left} before resize)");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = await runTask;
        }

        [Fact]
        public async Task A_within_bounds_resize_preserves_a_scrolled_answer_preview_on_File()
        {
            // File/MultiFile always used WriteAnswerViewport's original re-anchor-on-resize
            // behavior — the only two controls left out of the Select/MultiSelect/Table/MultiTable/
            // TreeSelect/TreeMultiSelect resize-preservation guarantee. WriteAnswerViewport now preserves the
            // scroll position across resize unconditionally, so this is new coverage for File/
            // MultiFile rather than a regression check.
            var vt = MakeTerminal(100, 24);
            FileControl.FileSystem = MakeFileSystemWithLongName(out string root);
            var control = new PromptPlusControls(vt, new PromptConfig()).File("Choose").Root(root);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);

            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            _ = vt.Keys.Enqueue(ConsoleKey.End);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var (Left, Top) = vt.GetCursorPosition();

            vt.RaiseResize(90, 24);
            WaitUntilRenderSettles();

            _ = vt.GetCursorPosition().Left.Should().BeGreaterThan(50,
                "the resize must not snap the scrolled answer preview back to Home — it stayed near " +
                $"the end (col {Left} before resize)");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = await runTask;
        }

        [Fact]
        public async Task A_within_bounds_resize_preserves_a_scrolled_answer_preview_on_MultiFile()
        {
            var vt = MakeTerminal(100, 24);
            MultiFileControl.FileSystem = MakeFileSystemWithLongName(out string root);
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiFile("Choose").Root(root);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);

            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            _ = vt.Keys.Enqueue(ConsoleKey.End);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var (Left, Top) = vt.GetCursorPosition();

            vt.RaiseResize(90, 24);
            WaitUntilRenderSettles();

            _ = vt.GetCursorPosition().Left.Should().BeGreaterThan(50,
                "the resize must not snap the scrolled answer preview back to Home — it stayed near " +
                $"the end (col {Left} before resize)");

            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            _ = await runTask;
        }

        private static void WaitUntilAllKeysConsumed(VirtualTerminal vt)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (vt.Keys.HasNext && sw.ElapsedMilliseconds < 2000)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5);
            }
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
        }

        private static void WaitUntilRenderSettles()
        {
            // The resize is detected via console.SizeChanged -> _pendingResize, processed on the
            // main render loop's next 16ms poll tick. A short fixed margin covering a few ticks is
            // the same category of wait already used elsewhere in this suite for background/async
            // state (see SerializedGlobalStateCollection's background-task-timing half) — there is no
            // narrower signal to hook into from outside the control.
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(100);
        }
    }
}
