using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Core;
using System.Globalization;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // BaseControlPrompt.GetEmacsTooltips (Controls/Common/BaseControlPrompt.cs) — usado por
    // Input/MaskEdit/Select/MultiSelect/Table/MultiTable/Calendar para listar os atalhos Emacs no
    // ciclo de tooltip (F1). Regressão: _EmacsTooltips/_EmacsTooltipsReadonly eram campos STATIC
    // com cache `??=` — o primeiro controle a chamar o método, em qualquer cultura, congelava o
    // array para todo o processo; controles seguintes configurados numa cultura diferente liam o
    // texto ERRADO (da primeira cultura), porque a flag nunca via a mudança de
    // PromptConfig.DefaultCulture. Corrigido tornando os campos por instância.
    public class EmacsTooltipsCultureTests
    {
        private static InputControl MakeControl(CultureInfo culture)
        {
            var vt = VirtualTerminal.Create(o => { o.SupportsUnicode = false; });
            vt.EnabledEmacs = true;
            var config = new PromptConfig { DefaultCulture = culture };
            return (InputControl)new PromptPlusControls(vt, config).Input("probe");
        }

        [Fact]
        public void Each_control_instance_reflects_its_own_configured_culture()
        {
            var ptBr = MakeControl(new CultureInfo("pt-BR"));
            string[] tooltipsPtBr = ptBr.GetEmacsTooltips(false);

            // Segundo controle, cultura diferente, MESMO tipo genérico fechado
            // (BaseControlPrompt<string>) — antes do fix, herdava o cache do primeiro.
            var enUs = MakeControl(new CultureInfo("en-US"));
            string[] tooltipsEnUs = enUs.GetEmacsTooltips(false);

            _ = tooltipsPtBr[1].Should().Contain("início", "pt-BR text for Ctrl+A must be in Portuguese");
            _ = tooltipsEnUs[1].Should().Contain("start", "en-US text for Ctrl+A must be in English, not leaked from the pt-BR instance");
            _ = tooltipsPtBr[1].Should().NotBe(tooltipsEnUs[1]);
        }

        [Fact]
        public void Readonly_variant_also_reflects_its_own_culture_independently_of_the_full_variant()
        {
            var ptBr = MakeControl(new CultureInfo("pt-BR"));
            // Popula primeiro o cache "full" (isreadonly:false) e só depois o "readonly" — os dois
            // campos são independentes, então o fix precisa cobrir ambos.
            _ = ptBr.GetEmacsTooltips(false);
            string[] readonlyPtBr = ptBr.GetEmacsTooltips(true);

            var enUs = MakeControl(new CultureInfo("en-US"));
            string[] readonlyEnUs = enUs.GetEmacsTooltips(true);

            _ = readonlyPtBr[0].Should().NotBe(readonlyEnUs[0]);
        }
    }
}
