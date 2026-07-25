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
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — TreeControl, modo `Select` (globais + expand/
    // collapse/Tab-drilldown/ViewOnly/SelectLeafOnly/predicado fora de filtro). Cenários do modo
    // `Filter` estão em TreeControlFilterModeTests.cs. Checklist levantado lendo TryResult/
    // BufferTemplate/InitControl reais (TreeControl.cs) + sondas de render.
    //
    // Diferenças confirmadas por sonda vs. Select/Table:
    // - Escape (real ou timeout) SEMPRE devolve Content=null/default, IsAborted=true — Tree nunca
    //   preserva a posição do cursor no cancelamento (mesma família do MultiSelect, diferente do
    //   Select/Table que preservam no Escape real).
    // - `EnableHistory` sozinho (sem `.UseDefaultHistory()`/`.Default(...)`) JÁ recarrega o valor
    //   salvo automaticamente — `_useDefaultHistory` começa `true` por padrão no Tree, diferente
    //   de Select/MultiSelect/Table (que começam `false` e exigem opt-in explícito). Não é bug —
    //   a própria doc de `EnableHistory` do Tree já promete esse comportamento — só uma
    //   divergência real entre controles, documentada aqui.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TreeControlTests : IDisposable
    {
        private const string HistoryFile = "tree-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public TreeControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // Root
        //  Apple
        //    A1
        //    A2
        //  Berry
        private static ITreeControl<string> MakeTree(VirtualTerminal vt)
        {
            var tree = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
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
        public void Initial_render_shows_the_root_expanded_with_children_collapsed()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: Root");
            _ = vt.TextAt(1, 0, 9).Should().Be(">[-] Root");
            _ = vt.TextAt(2, 0, 13).Should().Be("  |-[+] Apple");
            _ = vt.TextAt(3, 0, 9).Should().Be("  |_Berry");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
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
        public void Tab_on_an_expanded_container_drills_into_its_first_child()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt); // cursor starts on Root, already expanded
            _ = vt.Keys.Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: Root/Apple");
        }

        [Fact]
        public void Tab_on_a_collapsed_container_expands_it_then_drills_in()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("A1").Should().NotBeNull();
            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: Apple/A1");
        }

        [Fact]
        public void ShiftTab_on_the_first_child_collapses_the_parent_and_jumps_to_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("A1").Should().BeNull();
            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: Root/Apple");
        }

        [Fact]
        public void Enter_confirms_the_selected_node()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Apple");
        }

        [Fact]
        public void Escape_always_aborts_with_a_null_result_regardless_of_the_cursor()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void SelectLeafOnly_blocks_confirming_a_container_node()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).SelectLeafOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void SelectLeafOnly_allows_confirming_a_leaf()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).SelectLeafOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Berry");
        }

        [Fact]
        public void Navigating_onto_a_disabled_node_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: Root/Apple");
        }

        [Fact]
        public void Enter_on_a_disabled_node_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Disabled_root_shows_the_error_immediately_without_any_keypress()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root", disable: true)
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void ViewOnly_bypasses_Disabled_and_confirms_normally()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root", disable: true)
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .ViewOnly();
            _ = control.AddLast("Apple");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Root");
        }

        [Fact]
        public void Default_pointing_to_a_disabled_node_is_not_preselected()
        {
            // Mirrors Select/Table: a Default(...)/history target that resolves to a disabled node
            // is simply not honored — the cursor stays at its natural starting position (the root)
            // instead of expanding down to a node the user could never confirm anyway.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .Default("Apple");
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: Root");
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().BeNull();
        }

        [Fact]
        public void Disabled_node_uses_a_visually_distinct_style()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b);
            _ = control.AddLast("Apple", disable: true);
            _ = control.AddLast("Berry");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Row 2 = "  |-Apple" (disabled), row 3 = "  |_Berry" (enabled) — foreground must differ.
            _ = vt.StyleAt(2, 5).Foreground.Should().NotBe(vt.StyleAt(3, 5).Foreground);
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_the_default_error_and_does_not_confirm()
        {
            // Regression: the default message used to be SelectionDisabled ("Item disabled"), which
            // is wrong for a predicate rejection — Tree has no concept of disabled nodes at all.
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateSelected(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_the_custom_message()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateSelected(_ => (false, "Custom rejection"));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_async_predicate_shows_the_default_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).PredicateSelectedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void ViewOnly_Enter_without_a_default_returns_the_initially_selected_root()
        {
            // Regression: used to return null/default here even with the root visibly highlighted,
            // because the view-only fallback only ever checked an explicit Default(...) value.
            var vt = MakeTerminal();
            var control = MakeTree(vt).ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Root");
        }

        [Fact]
        public void ViewOnly_Enter_with_a_default_returns_the_resolved_target_regardless_of_navigation()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).Default("A1").ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("A1");
        }

        [Fact]
        public void Default_expands_the_ancestors_and_preselects_the_target()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).Default("A1");

            // Safety-net cancellation (no real key) — asserting against the live frame, before any
            // Enter/Escape, since FinishTemplate only renders the prompt+answer line afterwards.
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("A2").Should().NotBeNull();
            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: Apple/A1");
        }

        [Fact]
        public void ToggleFullPath_key_switches_between_the_immediate_parent_and_the_full_ancestor_chain()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add).Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: Apple/A1");

            var vt2 = MakeTerminal();
            var control2 = MakeTree(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Add).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.F3, shift: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.TextAt(0, 0, 21).Should().Be("Choose: Root/Apple/A1");
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_visible_node_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Berry");
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeTree(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("Enter:Finish").Should().BeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeTree(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void EnableHistory_persists_the_confirmed_node_and_autoreloads_it_without_extra_opt_in()
        {
            var vt = MakeTerminal();
            var control = MakeTree(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            // Deliberately NOT calling UseDefaultHistory()/Default(...) — Tree auto-reloads from
            // EnableHistory alone (see class remarks: _useDefaultHistory starts true here).
            var control2 = MakeTree(vt2).EnableHistory(HistoryFile);
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().Be("Apple");
        }
    }
}
