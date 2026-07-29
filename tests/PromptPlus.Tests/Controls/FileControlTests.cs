using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.FileExec;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // FileControl (single-select tree browsing a real or mocked filesystem). This is the
    // Mock-based main suite; `FileControlRealFilesystemTests.cs`
    // covers the case-sensitivity gap MockFileSystem can't reproduce on Linux (see that file's
    // header comment) and stays a thin real-disk safety net, not the primary coverage.
    //
    // `FileControl.FileSystem` is a static, swappable `IFileSystem` (mirrors `FileHistory.
    // FileSystem`) — every test here swaps it for a `MockFileSystem` in the constructor and
    // restores the real one in Dispose, same pattern as `SerializedGlobalStateCollection`. No dedicated
    // parallelization guard is added here beyond the existing `[Collection(SerializedGlobalStateCollection.
    // Name)]` (also needed because several tests touch `EnableHistory`) — `FileControl.FileSystem`
    // and `FileHistory.FileSystem` are separate static fields, but sharing the same collection is
    // simplest and keeps this suite from racing the History-swapping suites already using it.
    //
    // No `.Culture(...)` method exists on `IFileControl` — file-size formatting
    // (`FormatSize`/`FormatSize`) always uses the ambient `CultureInfo.CurrentCulture`, whose
    // decimal separator is machine-dependent. Tests that check size text use byte counts that
    // render with no fractional digits (exact multiples of 1024) to stay culture-independent.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class FileControlTests : IDisposable
    {
        private readonly IFileSystem _originalHistoryFs = FileHistory.FileSystem;
        private readonly MockFileSystem _mockHistoryFs = new();
        private readonly IFileSystem _originalFileFs = FileControl.FileSystem;

        public FileControlTests() => FileHistory.FileSystem = _mockHistoryFs;

        public void Dispose()
        {
            FileHistory.FileSystem = _originalHistoryFs;
            FileControl.FileSystem = _originalFileFs;
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

        private IFileControl MakeControl(VirtualTerminal vt)
        {
            FileControl.FileSystem = MakeFs();
            return new PromptPlusControls(vt, new PromptConfig()).File("Choose").Root(Root);
        }

        [Fact]
        public void Initial_render_shows_the_root_expanded_with_folders_and_files_sorted()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: root");
            _ = vt.TextAt(1, 0, 9).Should().Be(">[-] root");
            _ = vt.TextAt(2, 0, 11).Should().Be("  |-[+] sub");
            _ = vt.TextAt(3, 0, 17).Should().Be("  |_top.txt  2 KB");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Tab_moves_the_cursor_into_the_roots_first_child()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Name.Should().Be("sub");
            _ = result.Content.IsDirectory.Should().BeTrue();
        }

        [Fact]
        public void ExpandKey_reveals_a_folders_children()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("a.txt").Should().NotBeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void CollapseKey_hides_a_folders_children()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add).Enqueue(ConsoleKey.Subtract);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("a.txt").Should().BeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Enter_on_a_file_returns_a_FileItem_with_the_expected_metadata()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content!.Name.Should().Be("top.txt");
            _ = result.Content.IsDirectory.Should().BeFalse();
            _ = result.Content.Length.Should().Be(2048);
        }

        [Fact]
        public void SearchPattern_filters_files_but_never_folders()
        {
            var fs = MakeFs();
            fs.AddFile(Path.Combine(Root, "readme.md"), new MockFileData("x"));
            FileControl.FileSystem = fs;
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).File("Choose").Root(Root).SearchPattern("*.txt");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("readme.md").Should().BeNull();
            _ = vt.Find("sub").Should().NotBeNull();
            _ = vt.Find("top.txt").Should().NotBeNull();
        }

        [Fact]
        public void OnlyFolders_hides_every_file()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).OnlyFolders();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("top.txt").Should().BeNull();
            _ = vt.Find("Qty:2 items").Should().NotBeNull();
        }

        [Fact]
        public void SelectFilesOnly_blocks_confirming_a_folder()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).SelectFilesOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void SelectFilesOnly_still_allows_confirming_a_file()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).SelectFilesOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content!.Name.Should().Be("top.txt");
        }

        [Fact]
        public void HideSize_removes_the_size_suffix_from_files()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).HideSize();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(3, 0, 11).Should().Be("  |_top.txt");
        }

        [Fact]
        public void Default_expands_the_tree_down_to_the_target_and_selects_it()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Default(ATxtPath);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            var expected = $"Choose: sub{Path.DirectorySeparatorChar}a.txt";
            _ = vt.TextAt(0, 0, expected.Length).Should().Be(expected);
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void ShowFullPath_hotkey_toggles_between_short_and_full_answer_text()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            var expected = $"Choose: {Root}";
            _ = vt.TextAt(0, 0, expected.Length).Should().Be(expected);
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_entry_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Type("t").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Name.Should().Be("top.txt");
        }

        [Fact]
        public void Escape_aborts_with_a_null_result()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void EnableHistory_alone_autoreloads_without_needing_an_explicit_Default_call()
        {
            // Different from Select/Table/Calendar (which start _useDefaultHistory = false): File
            // follows the Tree/MultiTree convention where the field defaults to true, so
            // EnableHistory alone is already enough to restore the last confirmed path.
            const string historyFile = "file-history-tests";
            var vt = MakeTerminal();
            var control = MakeControl(vt).Default(ATxtPath).EnableHistory(historyFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            FileControl.FileSystem = MakeFs();
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).File("Choose").Root(Root)
                .EnableHistory(historyFile);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            var expected = $"Choose: sub{Path.DirectorySeparatorChar}a.txt";
            _ = vt2.TextAt(0, 0, expected.Length).Should().Be(expected);
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipExpandCollapse).Should().NotBeNull();
        }
    }
}
