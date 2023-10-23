// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace MaskEditDateTimeTypeSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.DoubleDash($"Your default Culture is {cult.Name}", DashOptions.HeavyBorder, style: Style.Default.Foreground(Color.Yellow));


            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - minimal usage");

            var mask = PromptPlus.MaskEdit("input", "MaskEdit DateTime input - value is requeried!")
                .Mask(MaskedType.DateTime)
                .Run();

            if (!mask.IsAborted)
            {
                PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
                PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
            }

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - accept empty value.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - with tip for type input.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .AcceptEmptyValue()
                .ShowTipInputType(FormatWeek.Long)
                .Styles(MaskEditStyles.MaskTypeTip, Style.Default.Foreground(Color.Blue))
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime - with overwrite culture:pt-br.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .ShowTipInputType(FormatWeek.Long)
                .Culture("pt-br")
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - overwrite prompt mask char.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime, '_')
                .AcceptEmptyValue()
                .ShowTipInputType(FormatWeek.Short)
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - with format year/time.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .AcceptEmptyValue()
                .FormatYear(FormatYear.Short)
                .FormatTime(FormatTime.OnlyHM)
                .ShowTipInputType(FormatWeek.Short)
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - with FillZeros.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .FillZeros()
                .ShowTipInputType(FormatWeek.Short)
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateTime ({cult.Name}) - with FillZeros and AcceptEmptyValue.");

            PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
                .Mask(MaskedType.DateTime)
                .FillZeros()
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash("For other features below see - input samples (same behavior)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]ValidateOnDemand[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]InputToCase[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLine(". [yellow]Default[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]DefaultIfEmpty[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]SuggestionHandler[/] - InputWithSuggestionSamples");
            PromptPlus.WriteLine(". [yellow]HistoryEnabled[/] - InputWithHistorySamples");
            PromptPlus.WriteLine(". [yellow]HistoryMinimumPrefixLength[/] - InputWithHistorySamples");
            PromptPlus.WriteLine(". [yellow]HistoryTimeout[/] - InputWithHistorySamples");
            PromptPlus.WriteLine(". [yellow]HistoryMaxItems[/] - InputWithHistorySamples");
            PromptPlus.WriteLine(". [yellow]HistoryPageSize[/] - InputWithHistorySamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}