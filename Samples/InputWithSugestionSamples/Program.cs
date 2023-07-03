// See https://aka.ms/new-console-template for more information
using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.WriteLine("Hello, World!");
//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

SugestionOutput SugestionInputSample(SugestionInput arg)
{
    var result = new SugestionOutput();
    if (arg.Text.StartsWith("s", StringComparison.CurrentCultureIgnoreCase))
    {
        result.Add("sugestion 1");
        result.Add("sugestion 2");
        result.Add("sugestion 3");
    }
    else
    {
        result.Add("other sugestion 1");
        result.Add("other sugestion 2");
        result.Add("other sugestion 3");
    }
    return result;
}

PromptPlus.DoubleDash("Control:Input - with sugestions.");
var in1 = PromptPlus
    .Input("Input sample", "input with sugestions.")
    .SuggestionHandler(SugestionInputSample)
    .Run();

if (!in1.IsAborted)
{
    PromptPlus.WriteLine($"You input is {in1.Value}");
}

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();