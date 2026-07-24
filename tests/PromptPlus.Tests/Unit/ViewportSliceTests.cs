using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Core;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // BaseControlPrompt.ViewportSlice/ViewportSliceCore (Controls/Common/BaseControlPrompt.cs) —
    // motor de scroll+elipse compartilhado por Input/Select/MultiSelect/Table/MultiTable/Tree/
    // MultiTree/Calendar/File/MultiFile. Só entra em jogo quando o texto NÃO cabe no viewport
    // (FitsInWidth retorna false); com texto cabendo, o método sai por um early-return simples,
    // fora do escopo destes testes. Chamado direto via a instância de InputControl (qualquer
    // BaseControlPrompt<T> serve — ViewportSlice é público na base) para testar a matemática de
    // scroll isoladamente, sem depender do loop de render via VirtualTerminal.
    public class ViewportSliceTests
    {
        // fullText tem 30 chars (índices 0-9 = "0123456789", 10-29 = "A".."T"), bem maior que os
        // viewports pequenos usados abaixo — reproduz deliberadamente a condição "texto maior que
        // a tela" em que o bug relatado ocorria.
        private const string LongText = "0123456789ABCDEFGHIJKLMNOPQRST";

        private static InputControl MakeProbe()
        {
            var vt = VirtualTerminal.Create(o => { o.SupportsUnicode = false; o.Width = 200; o.Height = 24; });
            return (InputControl)new PromptPlusControls(vt, new PromptConfig()).Input("probe");
        }

        [Fact]
        public void Cursor_at_absolute_end_leaves_one_free_column_for_the_caret()
        {
            var probe = MakeProbe();

            // viewportWidth=13, cursor no fim absoluto do texto (posição 30 == Length).
            (string visibleLeft, string visibleRight) = probe.ViewportSlice(LongText, LongText.Length, 200 - 13);

            _ = visibleRight.Should().BeEmpty("nothing exists after the cursor when it sits at the true end");
            _ = visibleLeft.Should().Be("_JKLMNOPQRST");
            // Bug original: visibleLeft ocupava as 13 colunas inteiras (sem margem), forçando o
            // cursor (que é posicionado logo depois de visibleLeft) a cair na mesma coluna do
            // último caractere em vez de uma coluna além dele.
            _ = (visibleLeft.Length + visibleRight.Length).Should().Be(12, "one column must stay free for the caret");
        }

        [Fact]
        public void Stepping_one_position_back_from_the_end_reveals_the_hidden_last_character_with_an_ellipsis()
        {
            var probe = MakeProbe();

            // Mesmo texto/viewport do teste anterior, cursor uma posição antes do fim (29): o 'T'
            // passa a estar oculto à direita e precisa de elipse sinalizando isso.
            (string visibleLeft, string visibleRight) = probe.ViewportSlice(LongText, LongText.Length - 1, 200 - 13);

            // Bug original: 'T' era descartado por TrimToBudget para abrir espaço para a elipse
            // esquerda, e a elipse direita nunca era adicionada porque a flag hasHiddenRight tinha
            // sido calculada ANTES do corte (com base na janela original, onde nada estava oculto).
            _ = visibleRight.Should().NotBeEmpty("the 'T' character got pushed out of view and must be signaled");
            _ = visibleRight.Should().EndWith("_", "the ASCII ellipsis marks hidden content on the right");
            _ = (visibleLeft.Length + visibleRight.Length).Should().Be(13, "cursor is not at the end here, so the full viewport can be used");
        }

        [Fact]
        public void Text_that_fits_the_viewport_is_returned_verbatim_with_no_ellipsis()
        {
            var probe = MakeProbe();

            (string visibleLeft, string visibleRight) = probe.ViewportSlice("abc", 2, 200 - 10);

            _ = visibleLeft.Should().Be("ab");
            _ = visibleRight.Should().Be("c");
        }
    }
}
