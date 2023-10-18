// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace MaskEditTimeTypeSamples
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


            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - minimal usage");

            var mask = PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input - value is requeried!")
                .Mask(MaskedType.TimeOnly)
                .Run();

            if (!mask.IsAborted)
            {
                PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
                PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
            }

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - accept empty value.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly)
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - with tip for type input and custom color.");

            PromptPlus.MaskEdit("input", "MaskEdit [BLUE]TimeOnly[/] [YELLOW]input[/]")
                .Mask(MaskedType.TimeOnly)
                .AcceptEmptyValue()
                .ShowTipInputType()
                .TypeTipStyle(Style.Default.Foreground(Color.IndianRed))
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly - with overwrite culture:pt-br.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly)
                .ShowTipInputType()
                .Culture("pt-br")
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - overwrite prompt mask char.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly, '_')
                .AcceptEmptyValue()
                .ShowTipInputType()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - with format time.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly)
                .AcceptEmptyValue()
                .FormatTime(FormatTime.OnlyHM)
                .ShowTipInputType()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - with FillZeros.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly)
                .FillZeros()
                .ShowTipInputType()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit TimeOnly ({cult.Name}) - with FillZeros and AcceptEmptyValue.");

            PromptPlus.MaskEdit("input", "MaskEdit TimeOnly input")
                .Mask(MaskedType.TimeOnly)
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