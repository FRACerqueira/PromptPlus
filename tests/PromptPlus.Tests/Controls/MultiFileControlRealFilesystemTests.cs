using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // MultiFileControl against a REAL temporary directory — not MockFileSystem. Same rationale as
    // FileControlRealFilesystemTests.cs (see that file's header): MockFileSystem.StringOperations.
    // Comparer is always OrdinalIgnoreCase and cannot reproduce Linux's case-sensitive real
    // filesystem. This is a thin real-disk safety net, not the primary coverage (that's
    // MultiFileControlTests.cs) — it exists to exercise the actual background-wildcard disk walk
    // (StartBackgroundWildcard -> EnumerateSubtree) against real I/O at least once, since Mock-based
    // tests never touch a real disk.
    //
    // [Collection(BackgroundTimingCollection.Name)]: the background-folder-check test spawns a real
    // background Task and waits on a fixed real-time margin for it to finish — observed flaky under
    // full-suite parallel load (see BackgroundTimingCollection.cs) until serialized this way.
    [Collection(BackgroundTimingCollection.Name)]
    public class MultiFileControlRealFilesystemTests : IDisposable
    {
        private readonly DirectoryInfo _tempDir = Directory.CreateTempSubdirectory("promptplus-multifilecontrol-");

        public void Dispose()
        {
            try { _tempDir.Delete(recursive: true); } catch { /* best-effort cleanup */ }
        }

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void Enter_confirms_individually_checked_real_files_under_a_temp_root()
        {
            File.WriteAllText(Path.Combine(_tempDir.FullName, "a.txt"), "1");
            File.WriteAllText(Path.Combine(_tempDir.FullName, "b.txt"), "22");

            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiFile("Pick").Root(_tempDir.FullName);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
                .Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
                .Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().HaveCount(2);
            _ = result.Content.Should().Contain(f => f.Name == "a.txt");
            _ = result.Content.Should().Contain(f => f.Name == "b.txt");
        }

        [Fact]
        public void Checking_a_folder_recursively_walks_the_real_disk_subtree_in_the_background()
        {
            _ = Directory.CreateDirectory(Path.Combine(_tempDir.FullName, "sub"));
            File.WriteAllText(Path.Combine(_tempDir.FullName, "sub", "a.txt"), "1");
            File.WriteAllText(Path.Combine(_tempDir.FullName, "sub", "b.txt"), "22");

            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiFile("Pick").Root(_tempDir.FullName);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            // Now that this class is serialized against the other background-task-heavy classes
            // (see BackgroundTimingCollection), a much smaller margin is safe: the background
            // wildcard op only has 2 tiny real files to enumerate and no longer competes with
            // hundreds of other parallel tests for thread-pool slots.
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(500);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().HaveCount(3);
            _ = result.Content.Should().Contain(f => f.Name == "sub" && f.IsDirectory);
            _ = result.Content.Should().Contain(f => f.Name == "a.txt");
            _ = result.Content.Should().Contain(f => f.Name == "b.txt");
        }

        [Fact]
        public void SearchPattern_matching_depends_on_the_running_OS_real_case_sensitivity()
        {
            // Same rationale as FileControlRealFilesystemTests's equivalent test: a Mock-based
            // equivalent would always match on Linux too, since MockFileSystem is always
            // case-insensitive there — only a real filesystem exposes the OS-dependent behavior.
            File.WriteAllText(Path.Combine(_tempDir.FullName, "Report.txt"), "content");

            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig())
                .MultiFile("Pick").Root(_tempDir.FullName).SearchPattern("report.*");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            bool expectCaseInsensitiveMatch = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS();
            if (expectCaseInsensitiveMatch)
            {
                _ = result.Content.Should().ContainSingle().Which.Name.Should().Be("Report.txt");
            }
            else
            {
                // Linux: "report.*" does not match "Report.txt" — DownArrow from the (childless)
                // root has nothing to move to and Space on the root is a no-op (root cannot be
                // checked), so nothing ends up checked.
                _ = result.Content.Should().BeEmpty();
            }
        }
    }
}
