using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // FileControl (single-mode, tree-based browsing) against a REAL temporary directory — not
    // MockFileSystem. Purpose (see FASE2-CONTROLS-PLAN.md, Grupo 5): confirmed by reflection that
    // MockFileSystem.StringOperations.Comparer is always OrdinalIgnoreCase, with no supported way to
    // configure it — it can never reproduce Linux's case-sensitive real filesystem. These tests don't
    // swap FileControl.FileSystem at all (default = real disk via .Root(tempDir)), so:
    // - On Windows/macOS (case-insensitive real FS), they pass the same way Mock-based tests would —
    //   they don't prove anything new locally.
    // - On Linux (ci.yml's ubuntu-latest job, case-sensitive real FS), they exercise the actual OS
    //   behavior Mock can't replicate — that's the whole point of this file existing.
    // Bulk behavioral coverage (navigation, expand/collapse, tooltip, history) belongs in a future
    // Mock-based FileControlTests.cs when FileControl's turn comes up in the Grupo 5 plan; this file
    // is only the real-disk safety net, not the main suite.
    public class FileControlRealFilesystemTests : IDisposable
    {
        private readonly DirectoryInfo _tempDir = Directory.CreateTempSubdirectory("promptplus-filecontrol-");

        public void Dispose()
        {
            try { _tempDir.Delete(recursive: true); } catch { /* best-effort cleanup */ }
        }

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void Enter_confirms_a_real_file_under_a_temp_root()
        {
            File.WriteAllText(Path.Combine(_tempDir.FullName, "report.txt"), "content");

            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).File("Pick").Root(_tempDir.FullName);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().NotBeNull();
            _ = result.Content!.Name.Should().Be("report.txt");
            _ = result.Content.IsDirectory.Should().BeFalse();
        }

        [Fact]
        public void SearchPattern_matching_depends_on_the_running_OS_real_case_sensitivity()
        {
            // On the real filesystem of whichever OS runs this (Windows/macOS: case-insensitive;
            // Linux: case-sensitive), a differently-cased pattern either does or doesn't match
            // "Report.txt" — exactly the behavior a MockFileSystem-based equivalent could NOT
            // reproduce faithfully on Linux (it would always match, per the class remarks above).
            File.WriteAllText(Path.Combine(_tempDir.FullName, "Report.txt"), "content");

            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig())
                .File("Pick").Root(_tempDir.FullName).SearchPattern("report.*");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            bool expectCaseInsensitiveMatch = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS();
            if (expectCaseInsensitiveMatch)
            {
                _ = result.Content.Should().NotBeNull();
                _ = result.Content!.Name.Should().Be("Report.txt");
            }
            else
            {
                // Linux: "report.*" does not match "Report.txt" — DownArrow from the (childless)
                // root has nothing to move to, so Enter on the root itself selects the root folder.
                _ = result.Content.Should().NotBeNull();
                _ = result.Content!.IsDirectory.Should().BeTrue();
            }
        }
    }
}
