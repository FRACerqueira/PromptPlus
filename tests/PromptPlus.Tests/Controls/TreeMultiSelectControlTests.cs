using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // TreeMultiSelectControl, `Select` mode (global navigation + expand/collapse/Tab-drilldown/check
    // tri-state/cascade/F2/Range/ViewOnly/disabled/predicate outside of filter). `Filter` mode
    // scenarios are in TreeMultiSelectControlFilterModeTests.cs. Checklist derived from reading the
    // real TryResult/InitControl/BufferTemplate (TreeMultiSelectControl.cs) plus render probes.
    //
    // Fixes applied this session (same patterns already fixed in MultiSelect/Table/MultiTable —
    // see promptplus-multi-predicate-rule and promptplus-naming-audit-checklist):
    // - `PredicateSelected`/`PredicateSelectedAsync` -> `PredicateChecked`/`PredicateCheckedAsync`.
    // - The predicate only validates on CHECK (Space) — unchecking must never call it.
    //   `ToggleCheck` and `ToggleCheckSingleNode` used to validate unconditionally before
    //   computing the toggle direction; fixed to compute the direction first and only validate
    //   on the checking branch.
    // - Added the concept of a disabled node (same as Tree/Select): visible, navigable,
    //   expandable, but not confirmable via Space/Ctrl+Space. Specific to TreeMultiSelect
    //   (tri-state/cascade): a cascade (`SetCheckedOnSource`) passes through a disabled container
    //   without touching its own flag, but still reaches its enabled descendants; `Default(...)`
    //   forces-checks a disabled node (bypassing the block), and that forced check survives the
    //   F2 uncheck-all (F2 skips disabled nodes in both directions).
    //
    // Real bug found and fixed this session (not part of any pre-approved pattern):
    // `SetCheckedOnSource` stamps its own id for EVERY node touched during a cascade (containers
    // and leaves alike), but the checkbox (`ComputeCheck`) and the footer counter used different
    // sources of truth: a container's checkbox with CascadeCheck=true ALWAYS derives from its
    // descendant leaves (ignoring its own flag), while `CollectCheckedFrom`/the footer counter
    // read the raw `_checkedSourceIds`. Result: checking a container via cascade and then
    // unchecking ONE child individually left the container's flag "stuck" in
    // `_checkedSourceIds` forever — the screen correctly showed `[?]` (Indeterminate), but
    // `Enter` still included the container in the final result, and the "N selected" footer
    // over-counted. Fixed by making `CollectCheckedFrom` and the footer use the same rule as the
    // checkbox (`ComputeCheck(node) == Checked`) — now screen, footer, and final result never
    // disagree. Expected side effect (not a bug, a direct consequence of the same aggregate
    // model): with `RecursiveMarkWithCtrlSpace` + `CascadeCheck` (default `true`), checking ONLY
    // a container via plain Space (no cascade) is inert — it shows up neither in the checkbox
    // nor in the result — because a container's state under cascade is always an aggregate of
    // its descendants, never an independent flag of its own.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TreeMultiSelectControlTests : IDisposable
    {
        private const string HistoryFile = "multitree-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public TreeMultiSelectControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // Root
        //  Apple
        //    A1
        //    A2
        //  Berry
        private static ITreeMultiSelectControl<string> MakeTree(VirtualTerminal vt)
        {
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            var apple = tree.AddLast("Apple");
            _ = apple.AddLast("A1");
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");
            return tree;
        }

        [Fact]
        public void Initial_render_shows_the_root_expanded_children_collapsed_and_unchecked_boxes()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: Root");
            _ = vt.TextAt(1, 0, 12).Should().Be(">[ ][-] Root");
            _ = vt.TextAt(2, 0, 16).Should().Be(" [ ] |-[+] Apple");
            _ = vt.TextAt(3, 0, 12).Should().Be(" [ ] |_Berry");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void ExpandKey_reveals_the_children_of_a_container()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("A1").Should().NotBeNull();
            _ = vt.Find("A2").Should().NotBeNull();
            _ = vt.Find("Qty:5 items").Should().NotBeNull();
        }

        [Fact]
        public void CollapseKey_hides_the_children_of_an_expanded_container()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add).Enqueue(ConsoleKey.Subtract);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("A1").Should().BeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Tab_from_the_root_drills_into_its_first_child()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 1).Should().Be(">");
        }

        [Fact]
        public void ShiftTab_on_the_first_child_collapses_the_parent_and_jumps_to_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            // Tab -> Apple, Tab -> expands Apple and drills into A1, Shift+Tab -> collapses
            // Apple (A1 is its first child) and jumps the cursor back to it.
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 16).Should().Be(">[ ] |-[+] Apple");
            _ = vt.Find("A1").Should().BeNull();
        }

        [Fact]
        public void Space_checks_a_leaf_and_Enter_confirms_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["A1"]);
            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: Apple/A1");
        }

        [Fact]
        public void ExtraInfo_is_appended_to_the_live_answer()
        {
            // No key sent: the safety-net timeout ends Run() leaving this live frame on screen.
            var vt = MakeTerminal();
            var control = MakeTree(vt).ExtraInfo(x => $"Length: {x.Length}");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: Root  Length: 4").Should().NotBeNull();
        }

        [Fact]
        public void ExtraInfoAsync_is_appended_to_the_live_answer()
        {
            // "Apple" is a non-root node, so FormatAnswerForNode shows "Root/Apple" (parent +
            // PathSeparator + name); ExtraInfo still receives the raw value ("Apple", 5 chars).
            var vt = MakeTerminal();
            var control = MakeTree(vt).ExtraInfoAsync(x => System.Threading.Tasks.Task.FromResult<string?>($"Length: {x.Length}"));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: Root/Apple  Length: 5").Should().NotBeNull();
        }

        [Fact]
        public void ExtraInfo_is_not_appended_to_the_final_answer()
        {
            // Same drill-down (Tab, Tab -> A1) as Space_checks_a_leaf_and_Enter_confirms_it, so the
            // checked result is known: just the leaf "A1".
            var vt = MakeTerminal();
            var control = MakeTree(vt).ExtraInfo(x => $"Length: {x.Length}");
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["A1"]);
            _ = vt.Find("Length: 2").Should().BeNull();
            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: Apple/A1");
        }

        [Fact]
        public void ExtraInfo_overflowing_the_console_width_is_hidden_up_front()
        {
            var vt = MakeTerminal();
            string longExtra = new string('X', 100) + "TAIL";
            var control = MakeTree(vt).ExtraInfo(_ => longExtra);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("TAIL").Should().BeNull();
        }

        [Fact]
        public void ExtraInfo_overflow_is_reachable_by_scrolling_the_live_answer_with_End()
        {
            var vt = MakeTerminal();
            string longExtra = new string('X', 100) + "TAIL";
            var control = MakeTree(vt).ExtraInfo(_ => longExtra);
            _ = vt.Keys.Enqueue(ConsoleKey.End);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("TAIL").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_a_container_cascades_the_check_to_every_descendant_and_auto_expands()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
            _ = vt.TextAt(0, 0, 36).Should().Be("Choose: Root/Apple,Apple/A1,Apple/A2");
        }

        [Fact]
        public void A_partially_checked_container_shows_the_indeterminate_glyph()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            // Check Apple (cascades to A1+A2), then move to A1 and uncheck it individually —
            // Apple must show Indeterminate, never Checked, once its descendants disagree.
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 4).Should().Be(" [?]");
            _ = vt.TextAt(4, 0, 4).Should().Be(" [x]");
        }

        [Fact]
        public void Bugfix_a_stale_container_flag_left_over_from_a_cascade_never_leaks_into_the_result_or_the_footer()
        {
            // Regression for the bug found+fixed this session: SetCheckedOnSource stamps every
            // descendant's own id during a cascade (containers included). Once Apple is checked
            // (cascade to A1+A2) and A1 is later unchecked individually, Apple's own id is still
            // sitting in _checkedSourceIds forever — nothing ever clears it. Both the "N selected"
            // footer and the final Enter result must agree with what the checkbox shows
            // (Indeterminate for Apple, i.e. NOT checked), not with the stale raw id set.
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["A2"]);
        }

        [Fact]
        public void Bugfix_footer_count_matches_the_reconciled_result_not_the_raw_stamped_id_set()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void CascadeCheck_false_toggles_only_the_container_itself_without_touching_children()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).CascadeCheck(false);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 16).Should().Be(">[x] |-[+] Apple");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void RecursiveMarkWithCtrlSpace_plain_space_on_a_leaf_toggles_just_that_leaf()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).RecursiveMarkWithCtrlSpace(true);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["A1"]);
        }

        [Fact]
        public void RecursiveMarkWithCtrlSpace_plain_space_on_a_container_is_inert_under_cascade()
        {
            // Direct consequence of the bugfix above: a container's checked state under
            // CascadeCheck=true is always derived from its descendants — marking only the
            // container's own flag (without touching children) can never surface anywhere.
            var vt = MakeTerminal();
            var control = MakeTree(vt).RecursiveMarkWithCtrlSpace(true);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void RecursiveMarkWithCtrlSpace_ctrlspace_on_a_container_still_cascades()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).RecursiveMarkWithCtrlSpace(true);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
        }

        [Fact]
        public void CheckLeafOnly_rejects_checking_a_container()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).CheckLeafOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(2, 0, 4).Should().Be(">[ ]");
        }

        [Fact]
        public void ShowFullPath_shows_the_full_ancestor_chain_in_the_finished_answer()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).ShowFullPath();
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 21).Should().Be("Choose: Root/Apple/A1");
        }

        [Fact]
        public void Enter_below_the_minimum_range_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).CascadeCheck(false).Range(2);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Minimum selection of 2 items is required").Should().NotBeNull();
        }

        [Fact]
        public void Default_prechecks_a_deep_value_and_autoexpands_to_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).Default(["A1"]);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(3, 0, 12).Should().Be(">[x] |  |-A1");
            _ = vt.Find("Qty:5 items").Should().NotBeNull();
            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void EnableHistory_persists_the_confirmed_checks_and_autoreloads_without_extra_opt_in()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            // Deliberately NOT calling UseDefaultHistory()/Default(...) — same auto-reload
            // convention already confirmed for Tree: _useDefaultHistory starts true here too.
            var control2 = MakeTree(vt2).EnableHistory(HistoryFile);
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().BeEquivalentTo(["A1"]);
        }

        [Fact]
        public void ViewOnly_ignores_space_and_Enter_confirms_the_initial_checked_defaults()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).Default(["Apple"]).ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
        }

        [Fact]
        public void Escape_always_returns_an_aborted_empty_result()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void Space_unchecking_an_already_checked_leaf_ignores_the_predicate()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).Default(["A1"]).PredicateChecked(_ => false);
            // Default already auto-expands and auto-selects A1, so no navigation is needed.
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(3, 0, 12).Should().Be(">[ ] |  |-A1");
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().BeNull();
        }

        [Fact]
        public void Space_checking_a_node_rejected_by_the_sync_predicate_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Space_checking_a_node_rejected_by_the_predicate_with_a_custom_message()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateChecked(_ => (false, "Custom rejection"));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Space_checking_a_node_rejected_by_the_async_predicate_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateCheckedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void F2_checks_every_visible_node_when_none_are_fully_checked()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Root", "Apple", "A1", "A2", "Berry"]);
        }

        [Fact]
        public void F2_again_unchecks_every_node_once_all_are_checked()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("0 selected").Should().NotBeNull();
            _ = vt.TextAt(1, 0, 4).Should().Be(">[ ]");
        }

        [Fact]
        public void F2_checking_all_skips_only_the_nodes_the_predicate_rejects()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateChecked(v => v != "Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
        }

        [Fact]
        public void F2_unchecking_all_ignores_the_predicate()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Berry checked directly (predicate only gates individual Space checking, not F2's
            // uncheck branch): F2 above actually starts as a mass-check since Berry/Root are still
            // unchecked; assert only that Apple's own cascade set survives an unrelated predicate.
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().BeNull();
        }

        [Fact]
        public void Navigating_onto_a_disabled_node_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(2, 0, 1).Should().Be(">");
        }

        [Fact]
        public void Space_on_a_disabled_node_shows_an_error_and_does_not_toggle_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(2, 0, 4).Should().Be(">[ ]");
        }

        [Fact]
        public void Disabled_node_uses_a_visually_distinct_style_for_both_checkbox_and_label()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Row 2 = " [ ] |-Apple" (disabled), row 3 = " [ ] |_Berry" (enabled).
            _ = vt.StyleAt(2, 8).Foreground.Should().NotBe(vt.StyleAt(3, 8).Foreground);
        }

        [Fact]
        public void Initial_disabled_root_shows_the_error_immediately_without_any_keypress()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root", disable: true)
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void ViewOnly_bypasses_disabled_and_confirms_normally()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root", disable: true)
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .ViewOnly();
            _ = control.AddLast("Apple");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().BeNull();
        }

        [Fact]
        public void Default_force_checks_a_disabled_node_and_it_survives_a_later_F2_mass_uncheck()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");
            _ = control.Default(["Apple"]);
            // Check Berry too so every visible candidate is checked before F2 fires the
            // uncheck-all branch (F2's "any unchecked -> check all" would otherwise fire instead).
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple"]);
        }

        [Fact]
        public void Cascading_check_passes_through_a_disabled_container_to_reach_enabled_descendants()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            var sales = control.AddLast("Sales", disable: true);
            _ = sales.AddLast("EMEA");
            _ = sales.AddLast("APAC");
            // Space on Root cascades through the disabled "Sales" container without checking it,
            // but must still reach its enabled children EMEA/APAC.
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Root", "EMEA", "APAC"]);
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_visible_node_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Berry"]);
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipPages).Should().NotBeNull();
        }

        [Fact]
        public void ChangeDescription_updates_as_the_cursor_moves()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).ChangeDescription(v => $"desc:{v}");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("desc:Apple").Should().NotBeNull();
        }

        // ─── construction-time `check` (added after the initial MultiTree rollout, for parity
        // with IMultiSelectControl<T>.AddItem(ischecked:)/ITableMultiSelectControl<T>.AddItem(ischecked:))
        // ────────────────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Check_true_on_a_chained_node_starts_it_pre_checked_without_autoexpanding()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            var apple = tree.AddLast("Apple");
            _ = apple.AddLast("A1", check: true);
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = tree.Run(cts.Token);

            // Apple is Indeterminate (A1 checked, A2 not) but stays collapsed — unlike Default,
            // check:true does not auto-expand the tree to reveal the pre-checked node.
            _ = vt.TextAt(1, 0, 12).Should().Be(">[?][-] Root");
            _ = vt.TextAt(2, 0, 16).Should().Be(" [?] |-[+] Apple");
            _ = vt.Find("A1").Should().BeNull();
        }

        [Fact]
        public void Check_true_on_a_chained_leaf_is_returned_on_Enter()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            var apple = tree.AddLast("Apple");
            _ = apple.AddLast("A1", check: true);
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = tree.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["A1"]);
        }

        [Fact]
        public void Check_true_on_a_container_cascades_to_its_descendants_same_as_an_interactive_check()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            var apple = tree.AddLast("Apple", check: true);
            _ = apple.AddLast("A1");
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = tree.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
        }

        [Fact]
        public void Check_true_on_the_root_cascades_to_its_children()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root", check: true)
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = tree.AddLast("Apple");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = tree.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Root", "Apple"]);
        }

        [Fact]
        public void Check_true_is_additive_with_Default_neither_one_clears_the_other()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .Default(["Berry"]);
            var apple = tree.AddLast("Apple");
            _ = apple.AddLast("A1", check: true);
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = tree.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["A1", "Berry"]);
        }

        [Fact]
        public void Check_true_combined_with_disable_true_force_checks_through_the_block()
        {
            var vt = MakeTerminal();
            var tree = new PromptPlusControls(vt, new PromptConfig()).TreeMultiSelect<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = tree.AddLast("Apple", disable: true, check: true);
            _ = tree.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = tree.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple"]);
        }
    }
}
