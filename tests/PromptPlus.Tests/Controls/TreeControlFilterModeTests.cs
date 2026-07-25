using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — TreeControl, modo `Filter` (FilterMode != Disabled).
    // Globais e modo `Select` estão em TreeControlTests.cs.
    //
    // Achado confirmado por sonda: o filtro compara contra o CAMINHO COMPLETO de cada nó (incluindo
    // a Root), mas o match não exige que o termo bata com o INÍCIO da string inteira — bate se
    // QUALQUER segmento do caminho (separado por PathSeparator) começa com o termo digitado (em
    // FilterMode.StartsWith). Por isso digitar "app" acha "Root/Apple" mesmo sem digitar "root/".
    public class TreeControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ITreeControl<string> MakeFilterableTree(VirtualTerminal vt)
        {
            var tree = new PromptPlusControls(vt, new PromptConfig()).Tree<string>("Choose")
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
        public void Typing_a_segment_prefix_enters_filter_mode_and_flattens_the_tree()
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
        public void Filter_matches_a_descendants_own_segment_not_only_the_top_level()
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
            _ = vt.Keys.Type("a").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A1");
        }

        [Fact]
        public void Escape_while_filtering_still_aborts_with_a_null_result()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableTree(vt);
            _ = vt.Keys.Type("app").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }
    }
}
