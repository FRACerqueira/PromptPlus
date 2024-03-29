﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;

namespace AddToListSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:AddtoList - minimal usage");
            PromptPlus.AddtoList("Input value")
                .Run();

            PromptPlus.DoubleDash("Control:AddtoList - AllowDuplicate");
            PromptPlus.AddtoList("Input value")
                .AllowDuplicate()
                .Run();


            PromptPlus.DoubleDash("Control:AddtoList - Range");
            PromptPlus.AddtoList("Input value","Min. 2 items, Max. 5 items")
                .Range(2,5)
                .Run();


            PromptPlus.DoubleDash("Control:AddtoList - initial values/Default value");
            PromptPlus.AddtoList("Input value")
                .AddItem("item1")
                .AddItem("item2",true)
                .AddItem("item3")
                .Default("initial value")
                .Run();


            PromptPlus.DoubleDash("For other features below see - input/Select samples (same behavior)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]MaxLength[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]AcceptInput[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]InputToCase[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]SuggestionHandler[/] - InputWithSuggestionSamples");
            PromptPlus.WriteLine(". [yellow]Interaction[/] - SelectBasicSamples");
            PromptPlus.WriteLine(". [yellow]PageSize[/] - SelectBasicSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}