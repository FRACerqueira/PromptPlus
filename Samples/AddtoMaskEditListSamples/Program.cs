using System.Globalization;
using PPlus;

namespace AddtoMaskEditListSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:AddtoMaskEditList - minimal usage");
            PromptPlus.AddtoMaskEditList("Input value")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .DescriptionWithInputType()
                .Run();

            PromptPlus.DoubleDash("Control:AddtoMaskEditList - AllowDuplicate");
            PromptPlus.AddtoMaskEditList("Input value")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .DescriptionWithInputType()
                .AllowDuplicate()
                .Run();


            PromptPlus.DoubleDash("Control:AddtoMaskEditList - Range, Min. 2 items, Max. 5 items");
            PromptPlus.AddtoMaskEditList("Input value")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .DescriptionWithInputType()
                .Range(2, 5)
                .Run();


            PromptPlus.DoubleDash("Control:AddtoMaskEditList - initial values/Default value");
            PromptPlus.AddtoMaskEditList("Input value")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .DescriptionWithInputType()
                .AddItem("XYZ 232-aaa-AX-sdd")
                .AddItem("XYZ 232-aaa-AX-zzz", true)
                .Default("XYZ 232")
                .Run();

            PromptPlus.DoubleDash("[yellow]For other features to maskededit see - MaskEdit samples (same behaviour)[/]");
            PromptPlus.DoubleDash("For other features below see - input/Select samples (same behaviour)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]MaxLenght[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]InputToCase[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]SuggestionHandler[/] - InputWithSugestionSamples");
            PromptPlus.WriteLine(". [yellow]Interaction[/] - SelectBasicSamples");
            PromptPlus.WriteLine(". [yellow]PageSize[/] - SelectBasicSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}