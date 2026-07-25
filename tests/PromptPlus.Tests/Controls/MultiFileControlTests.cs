using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Controls.MultiFile;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 5 (FASE2-CONTROLS-PLAN.md) — MultiFileControl (multi-check tree browsing a
    // mocked filesystem, with tri-state folder checkboxes and a background recursive-check path).
    //
    // `MultiFileControl.FileSystem` is a static, swappable `IFileSystem` (separate from both
    // `FileControl.FileSystem` and `FileHistory.FileSystem`) — every test swaps it for a
    // `MockFileSystem` and restores the real one in Dispose. `[Collection(FileHistoryCollection.
    // Name)]` is only needed for the tests that also touch `FileHistory.FileSystem` (Default/
    // EnabledHistory), but is applied to the whole class for simplicity, same as FileControlTests.
    //
    // Checking an UNCHECKED folder (default CascadeCheck=true) starts a real background Task that
    // recursively enumerates the folder's disk subtree off the UI thread (see StartBackgroundWildcard
    // in MultiFileControl.cs). Because `vt.Keys.Enqueue(...)` makes all queued keys immediately
    // available (no real timing model), pre-queuing Space followed by Enter in the same batch lets
    // Enter fire before the background task completes. The fix — validated via probe this session —
    // is to run `control.Run(cts.Token)` on a background `Task`, `TestContext.Current.CancellationToken.WaitHandle.WaitOne` on the test thread to
    // give the real background Task genuine wall-clock time, THEN enqueue the next key into the
    // still-live queue, then `.GetAwaiter().GetResult()`. This is only needed when a folder is
    // CHECKED via Space/Ctrl+Space with cascade enabled; unchecking an already-fully-checked folder
    // uses a synchronous fast path (no disk I/O), and individual file toggles are always synchronous.
    [Collection(FileHistoryCollection.Name)]
    public class MultiFileControlTests : IDisposable
    {
        private readonly IFileSystem _originalHistoryFs = FileHistory.FileSystem;
        private readonly MockFileSystem _mockHistoryFs = new();
        private readonly IFileSystem _originalMultiFileFs = MultiFileControl.FileSystem;

        public MultiFileControlTests() => FileHistory.FileSystem = _mockHistoryFs;

        public void Dispose()
        {
            FileHistory.FileSystem = _originalHistoryFs;
            MultiFileControl.FileSystem = _originalMultiFileFs;
        }

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // MockFileSystem uses the real host OS's path rules (no cross-platform simulation —
        // see TestableIO/System.IO.Abstractions#778), so a hardcoded Windows path like
        // `C:\root` is misparsed on Linux (`\` isn't a separator there). Rooting under
        // Path.GetTempPath() instead of a hardcoded drive letter derives a valid absolute
        // path from the real OS for whichever drive/root is actually available — nothing is
        // written to it, since MockFileSystem never touches the real disk.
        //
        // root
        //   sub\
        //     a.txt (1 B)
        //   top.txt (2048 B = "2 KB", no fractional digits)
        private static readonly string Root = Path.Combine(Path.GetTempPath(), "root");
        private static readonly string SubDir = Path.Combine(Root, "sub");
        private static readonly string ATxtPath = Path.Combine(SubDir, "a.txt");
        private static readonly string TopTxtPath = Path.Combine(Root, "top.txt");

        private static MockFileSystem MakeFs()
        {
            var fs = new MockFileSystem();
            fs.AddDirectory(Root);
            fs.AddDirectory(SubDir);
            fs.AddFile(ATxtPath, new MockFileData("a"));
            fs.AddFile(TopTxtPath, new MockFileData(new byte[2048]));
            return fs;
        }

        private IMultiFileControl MakeControl(VirtualTerminal vt)
        {
            MultiFileControl.FileSystem = MakeFs();
            return new PromptPlusControls(vt, new PromptConfig()).MultiFile("Choose").Root(Root);
        }

        [Fact]
        public void Initial_render_shows_tri_state_checkboxes_for_the_collapsed_root()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: root");
            _ = vt.TextAt(1, 0, 9).Should().Be(">[-] root");
            _ = vt.TextAt(2, 0, 15).Should().Be("  |-[ ] [+] sub");
            _ = vt.TextAt(3, 0, 21).Should().Be("  |_[ ] top.txt  2 KB");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Space_checks_an_individual_file_synchronously()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(3, 0, 21).Should().Be("> |_[x] top.txt  2 KB");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_an_unchecked_folder_recursively_checks_the_subtree_in_the_background()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(400);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().HaveCount(2);
            _ = result.Content.Should().Contain(f => f.Name == "sub" && f.IsDirectory);
            _ = result.Content.Should().Contain(f => f.Name == "a.txt" && !f.IsDirectory);
        }

        [Fact]
        public void Space_on_a_fully_checked_folder_unchecks_the_subtree_synchronously()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(400);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void CascadeCheck_false_makes_space_toggle_only_the_folder_itself()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).CascadeCheck(false);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().ContainSingle().Which.Name.Should().Be("sub");
        }

        [Fact]
        public void RecursiveMarkWithCtrlSpace_makes_plain_space_toggle_only_the_folder_itself()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).RecursiveMarkWithCtrlSpace();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().ContainSingle().Which.Name.Should().Be("sub");
        }

        [Fact]
        public void RecursiveMarkWithCtrlSpace_moves_the_recursive_action_to_ctrl_space()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).RecursiveMarkWithCtrlSpace();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(400);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.Content.Should().HaveCount(2);
        }

        [Fact]
        public void Re_pressing_the_check_key_on_a_running_background_op_cancels_it()
        {
            var vt = MakeTerminal();
            // An artificially slow predicate keeps the background op "running" long enough for a
            // deterministic cancel window (a real disk enumeration of one tiny mock file would
            // otherwise finish too fast/unpredictably for a reliable test).
            var control = MakeControl(vt).PredicateChecked(f => { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(1000); return true; });
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(200);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(300);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void Wait_glyph_and_running_background_tag_show_while_a_folder_check_is_pending()
        {
            var vt = MakeTerminal();
            // An artificially slow predicate keeps the background op "running" long enough to
            // deterministically capture the pending-state render (a real disk enumeration of one
            // tiny mock file would otherwise finish too fast/unpredictably for a reliable test).
            var control = MakeControl(vt).PredicateChecked(f => { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(1000); return true; });
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 15).Should().Be("> |-[*] [+] sub");
            _ = vt.Find(PromptPlusResources.MultiFileRunningBackground).Should().NotBeNull();
        }

        [Fact]
        public void F2_ToggleAllVisible_toggles_only_currently_visible_checkable_nodes()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().HaveCount(2);
            _ = result.Content.Should().Contain(f => f.Name == "sub");
            _ = result.Content.Should().Contain(f => f.Name == "top.txt");
            _ = result.Content.Should().NotContain(f => f.Name == "a.txt");
        }

        [Fact]
        public void F3_ToggleFilterOnlySelected_shows_a_flat_view_tagged_only_selected()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 18).Should().Be(">[x] top.txt  2 KB");
            _ = vt.Find("Qty:1 items").Should().NotBeNull();
            _ = vt.Find(PromptPlusResources.MultiFileOnlySelected).Should().NotBeNull();
        }

        [Fact]
        public void F3_is_a_noop_when_nothing_is_checked()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MultiFileOnlySelected).Should().BeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Range_min_selection_blocks_confirm_until_enough_items_are_checked()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Range(1);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(string.Format(System.Globalization.CultureInfo.InvariantCulture, PromptPlusResources.MultiSelectMinSelection, 1)).Should().NotBeNull();
        }

        [Fact]
        public void Range_max_selection_blocks_confirm_once_exceeded()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Range(0, 0);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(string.Format(System.Globalization.CultureInfo.InvariantCulture, PromptPlusResources.MultiSelectMaxSelection, 0)).Should().NotBeNull();
        }

        [Fact]
        public void SelectFilesOnly_blocks_checking_a_folder()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).SelectFilesOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void SelectFilesOnly_still_allows_checking_a_file()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).SelectFilesOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().ContainSingle().Which.Name.Should().Be("top.txt");
        }

        [Fact]
        public void PredicateChecked_bool_overload_rejects_with_the_default_message()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).PredicateChecked(f => false);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void PredicateChecked_tuple_overload_rejects_with_a_custom_message()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).PredicateChecked(f => (false, "nope"));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find("nope").Should().NotBeNull();
        }

        [Fact]
        public void PredicateCheckedAsync_governs_individual_file_toggles()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).PredicateCheckedAsync(f => Task.FromResult(f.Name == "top.txt"));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().ContainSingle().Which.Name.Should().Be("top.txt");
        }

        [Fact]
        public void ShowFullPath_hotkey_toggles_between_short_and_full_answer_text()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            var expected = $"Choose: {TopTxtPath}";
            _ = vt.TextAt(0, 0, expected.Length).Should().Be(expected);
        }

        [Fact]
        public void Escape_aborts_with_an_empty_result()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void Default_and_EnabledHistory_round_trip_the_checked_paths()
        {
            const string historyFile = "multifile-history-tests";
            var vt = MakeTerminal();
            var control = MakeControl(vt).Default([ATxtPath]).EnabledHistory(historyFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            MultiFileControl.FileSystem = MakeFs();
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).MultiFile("Choose").Root(Root)
                .EnabledHistory(historyFile);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            var expected = $"Choose: sub{Path.DirectorySeparatorChar}a.txt";
            _ = vt2.TextAt(0, 0, expected.Length).Should().Be(expected);
            _ = vt2.Find("a.txt").Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipExpandCollapse).Should().NotBeNull();
        }
    }
}
