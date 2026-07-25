using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — MultiTreeControl, modo `Filter` (FilterMode !=
    // Disabled). Globais e modo `Select` estão em MultiTreeControlTests.cs.
    //
    // Achados confirmados por sonda (mesma família de comportamento do Tree, ver
    // TreeControlFilterModeTests.cs):
    // - `FilterMode.StartsWith` casa contra o NOME PRÓPRIO de cada nó (`TextSelector`), não o
    //   caminho completo — diferente de `Contains`, que casa contra o caminho completo cacheado
    //   em `_flatDisplayCache`.
    // - Marcar (Space) durante o filtro opera sobre o mesmo `TreeNode` fonte da árvore normal —
    //   uma cascata em modo filtro reflete de volta corretamente nos nós em modo Select ao limpar
    //   o filtro (ambas as projeções, `_nodes` e `_flatAll`, são recalculadas a partir do mesmo
    //   `_checkedSourceIds` via `RefreshNodeChecks`).
    public class MultiTreeControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IMultiTreeControl<string> MakeFilterableTree(VirtualTerminal vt)
        {
            var tree = new PromptPlusControls(vt, new PromptConfig()).MultiTree<string>("Choose")
                .Root("Root")
                .TextSelector(x => x)
                .DefaultMatchBy((a, b) => a == b)
                .Filter(FilterMode.StartsWith);
            var apple = tree.AddLast("Apple");
            _ = apple.AddLast("A1");
            _ = apple.AddLast("A2");
            _ = tree.AddLast("Berry");
            return tree;
        }

        [Fact]
        public void Typing_a_prefix_enters_filter_mode_and_flattens_the_tree_by_own_name()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("app");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: app (Filter)").Should().NotBeNull();
            _ = vt.Find("Root/Apple").Should().NotBeNull();
            _ = vt.Find("Berry").Should().BeNull();
            _ = vt.Find("Qty:1 items").Should().NotBeNull();
        }

        [Fact]
        public void Filter_matches_a_descendants_own_name_not_only_the_top_level()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("a1");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Root/Apple/A1").Should().NotBeNull();
            _ = vt.Find("Berry").Should().BeNull();
            _ = vt.Find("Qty:1 items").Should().NotBeNull();
        }

        [Fact]
        public void Backspacing_the_filter_to_empty_returns_to_the_lazy_tree_view()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Filter)").Should().BeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Arrow_navigation_works_while_the_filter_is_active()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("a").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            // "a" (StartsWith, own name) matches Apple, A1, A2 in tree order; DownArrow moves
            // from Apple to A1, Space checks it individually (A1 has no children to cascade to).
            _ = result.Content.Should().BeEquivalentTo(["A1"]);
        }

        [Fact]
        public void Escape_while_filtering_still_aborts_with_an_empty_result()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("app").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void Space_while_filtering_cascades_and_survives_clearing_the_filter()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            // "a" matches Apple (first), Space cascades to A1+A2; clearing the filter (2x
            // Backspace) must show that same checked state back in the tree view.
            _ = vt.Keys.Type("a").Enqueue(ConsoleKey.Spacebar)
              .Enqueue(ConsoleKey.Backspace).Enqueue(ConsoleKey.Backspace)
              .Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Apple", "A1", "A2"]);
        }
    }
}
