using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Core;
using System.Globalization;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // BaseControlPrompt.GetEmacsTooltips (Controls/Common/BaseControlPrompt.cs) — used by
    // Input/MaskEdit/Select/MultiSelect/Table/MultiTable/Calendar to list the Emacs shortcuts in
    // the tooltip cycle (F1). Regression: _EmacsTooltips/_EmacsTooltipsReadonly were STATIC fields
    // with a `??=` cache — the first control to call the method, in any culture, froze the array
    // for the whole process; subsequent controls configured with a different culture read the
    // WRONG text (from the first culture), because the cache never saw the
    // PromptConfig.DefaultCulture change. Fixed by making the fields per-instance.
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

            // Second control, different culture, SAME closed generic type
            // (BaseControlPrompt<string>) — before the fix, it inherited the first one's cache.
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
            // Populates the "full" cache (isreadonly:false) first, then the "readonly" one — the two
            // fields are independent, so the fix needs to cover both.
            _ = ptBr.GetEmacsTooltips(false);
            string[] readonlyPtBr = ptBr.GetEmacsTooltips(true);

            var enUs = MakeControl(new CultureInfo("en-US"));
            string[] readonlyEnUs = enUs.GetEmacsTooltips(true);

            _ = readonlyPtBr[0].Should().NotBe(readonlyEnUs[0]);
        }
    }
}
