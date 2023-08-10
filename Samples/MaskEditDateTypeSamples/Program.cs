using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace MaskEditDateTypeSamples
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


            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - minimal usage");

            var mask = PromptPlus.MaskEdit("input", "MaskEdit DateOnly input - value is requeried!")
                .Mask(MaskedType.DateOnly)
                .Run();

            if (!mask.IsAborted)
            {
                PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
                PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
            }

            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - accept empty value.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly)
                .AcceptEmptyValue()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - with tip for type input and custom color.");

            PromptPlus.MaskEdit("input", "MaskEdit [blue]DateOnly[/] [yellow]input[/]")
                .Mask(MaskedType.DateOnly)
                .AcceptEmptyValue()
                .DescriptionWithInputType(FormatWeek.Long)
                .TypeTipStyle(Style.Default.Foreground(Color.IndianRed))
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly - with overwrite culture:pt-br.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly)
                .DescriptionWithInputType(FormatWeek.Short)
                .Culture(new CultureInfo("pt-br"))
                .AcceptEmptyValue()
                .Run();


            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - overwrite prompt mask char.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly, '_')
                .AcceptEmptyValue()
                .DescriptionWithInputType(FormatWeek.Short)
                .Run();


            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - with format year.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly)
                .AcceptEmptyValue()
                .FormatYear(FormatYear.Short)
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - with FillZeros.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly)
                .FillZeros()
                .Run();

            PromptPlus.DoubleDash($"Control:MaskEdit DateOnly ({cult.Name}) - with FillZeros and AcceptEmptyValue.");

            PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
                .Mask(MaskedType.DateOnly)
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