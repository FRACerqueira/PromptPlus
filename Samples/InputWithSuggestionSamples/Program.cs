// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.WriteLine("Hello, World!");
//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

SuggestionOutput SuggestionInputSample(SuggestionInput arg)
{
    if (arg.Text.StartsWith("s", StringComparison.CurrentCultureIgnoreCase))
    {
        return SuggestionOutput
            .Create()
            .AddRange(
                new string[] { "Opc1", "Opc2", "Opc3" });
    }
    var result = new SuggestionOutput();
    result.Add("other suggestion 1");
    result.Add("other suggestion 2");
    result.Add("other suggestion 3");
    return result;
}

PromptPlus.DoubleDash("Control:Input - with suggestions.");
var in1 = PromptPlus
    .Input("Input sample", "input with suggestions.")
    .SuggestionHandler((input) => 
    {
        return SuggestionOutput
            .Create()
            .AddRange(
                new string[] { "Opc1", "Opc2", "Opc3" });
    })
    .Run();

if (!in1.IsAborted)
{
    PromptPlus.WriteLine($"You input is {in1.Value}");
}

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();