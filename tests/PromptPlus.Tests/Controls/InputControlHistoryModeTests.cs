using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // This collection serializes two unrelated-but-both-global-state concerns against each other,
    // because xUnit only allows a test class to belong to ONE collection:
    //
    // 1. FileHistory.FileSystem is a static field shared by the whole test assembly. Any two
    //    classes that swap this same static must run sequentially relative to each other —
    //    otherwise one class's constructor/Dispose swap races the other's mid-test.
    // 2. Classes that spawn a real background Task and rely on real wall-clock margins
    //    (ManualResetEventSlim handshakes, fixed TestContext.Current.CancellationToken.WaitHandle.
    //    WaitOne waits — ProgressBar/TaskExec/MultiTasks's caller-supplied handler, MultiFile's/
    //    MultiFileControlRealFilesystemTests' background wildcard folder check, or a resize
    //    relayout's real render tick) were confirmed flaky under the full suite's ~600-test
    //    parallel load: thread-pool contention from dozens of OTHER classes running at the same
    //    time occasionally pushed those margins past their limit. Serializing them here relative
    //    to EACH OTHER (not the whole suite — that's not needed) removes the contention.
    //
    // Concern 2 was originally a separate `BackgroundTimingCollection` (merged in here 2026-07-24
    // once MultiFileControlTests — already here for concern 1 — was found to share the same
    // background-Task-timing risk as concern 2's members, and a class can't join both).
    [CollectionDefinition(Name, DisableParallelization = true)]
    public class SerializedGlobalStateCollection
    {
        public const string Name = "FileHistory global state & background task timing";
    }

    // InputControl, `History` mode (F3, see ConfigPrompt.HotKeyInputHistoryView). Global behavior
    // and basic `Input` mode are in InputControlTests.cs; `Suggestions` mode is in
    // InputControlSuggestionsModeTests.cs.
    //
    // FileHistory.FileSystem is swapped for a MockFileSystem per test (same pattern as
    // FileHistoryTests.cs), so it never touches the real user profile and stays deterministic on
    // Windows/Linux. InitControl (called inside Run()) is what reads history from disk, so it's
    // enough to swap the FileSystem and populate the file BEFORE calling Run().
    [Collection(SerializedGlobalStateCollection.Name)]
    public class InputControlHistoryModeTests : IDisposable
    {
        private const string HistoryFile = "input-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public InputControlHistoryModeTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // FileHistory.LoadHistory re-sorts by TimeOutTicks descending (FileHistory.cs:55-57) — it
        // does NOT preserve the order items are saved in. Each value here needs an explicitly
        // distinct expiration so "newest" is unambiguous regardless of how many ticks elapse
        // between the CreateItemHistory calls (same-day timeouts raced and picked the wrong winner).
        private static void SeedHistory(params string[] valuesNewestFirst)
        {
            var items = new ItemHistory[valuesNewestFirst.Length];
            for (int i = 0; i < valuesNewestFirst.Length; i++)
            {
                items[i] = ItemHistory.CreateItemHistory(valuesNewestFirst[i], TimeSpan.FromDays(valuesNewestFirst.Length - i));
            }
            FileHistory.SaveHistory(HistoryFile, items);
        }

        private static IInputControl MakeInputWithHistory(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Input("Name").EnableHistory(HistoryFile);

        // These three confirm with a real Enter rather than letting the safety-net timeout end the
        // run: InputControl.TryResult's cancel branch explicitly restores the pre-history text and
        // ModeView.Input the moment a run ends while NOT in Input mode (mirrors Escape's own
        // restore, see the Escape test below) — so a "no more keys, wait for cancellation" snapshot
        // can never observe History-mode state, unlike Select's Filter mode (which does NOT revert
        // on cancel). Enter is the only way to observe what History mode actually loaded.

        [Fact]
        public void F3_opens_history_and_loads_the_most_recent_item()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("second");
        }

        [Fact]
        public void DownArrow_in_history_mode_cycles_to_the_next_item()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("first");
        }

        [Fact]
        public void UpArrow_in_history_mode_wraps_around_to_the_last_item()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("first");
        }

        [Fact]
        public void F3_again_closes_history_and_restores_the_text_that_was_there_before()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Type("draft").Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Name: draft");
        }

        [Fact]
        public void Editing_while_in_history_mode_exits_back_to_input_mode_keeping_the_edit()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Type("!").Enqueue(ConsoleKey.Enter);

            // 3s occasionally wasn't enough only under full-suite parallel load (CPU contention
            // across ~180 concurrently-running tests) — never flaked running this test alone.
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("second!");
        }

        [Fact]
        public void CtrlDelete_clears_the_saved_history_and_exits_history_mode()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.Delete, ctrl: true).Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            // History was cleared, so the second F3 (which only opens when there IS history) is a
            // no-op — the control is back in Input mode with nothing typed.
            _ = vt.TextAt(0, 0, 6).Should().Be("Name: ");
        }

        [Fact]
        public void F3_is_ignored_when_there_is_no_history()
        {
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Joe");
        }

        [Fact]
        public void Escape_while_in_history_mode_aborts_and_restores_the_text_typed_before_opening_it()
        {
            SeedHistory("second", "first");
            var vt = MakeTerminal();
            var control = MakeInputWithHistory(vt);
            _ = vt.Keys.Type("draft").Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("draft");
        }
    }
}
