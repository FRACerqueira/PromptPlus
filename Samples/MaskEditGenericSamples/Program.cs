using PPlus;

namespace MaskEditGenericSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            PromptPlus.DoubleDash("Control:MaskEdit Generic - minimal usage");

            var mask = PromptPlus.MaskEdit("input", "MaskEdit Generic input")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .Run();

            if (!mask.IsAborted)
            {
                PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
                PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
            }

            PromptPlus.DoubleDash("Control:MaskEdit Generic - overwrite prompt mask char.");

            PromptPlus.MaskEdit("input", "MaskEdit Generic input")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}",'_')
                .Run();

            PromptPlus.DoubleDash("Control:MaskEdit Generic - with tip for type input.");

            PromptPlus.MaskEdit("input", "MaskEdit Generic input")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .DescriptionWithInputType()
                .TypeTipStyle(Style.Plain.Foreground(Color.Aqua))
                .Run();

            PromptPlus.DoubleDash("For other features below see - input samples (same behaviour)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]ValidateOnDemand[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]InputToCase[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLine(". [yellow]Default[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]DefaultIfEmpty[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]SuggestionHandler[/] - InputWithSugestionSamples");
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