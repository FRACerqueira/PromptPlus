// See https://aka.ms/new-console-template for more information
using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.WriteLine("Hello, World!");
//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus.DoubleDash("Control:Input - with history last 10 entries, min 1 char to activate history");
var end = false;
while (!end)
{
    var ctrlin = PromptPlus
    .Input("Input sample", "input a new value and try again (max. 1 min.) to view feature. [[ESC]] end sample")
    .HistoryEnabled("SampleHistInput")
    .HistoryMinimumPrefixLength(1)
    .HistoryMaxItems(10)
    .HistoryTimeout(TimeSpan.FromSeconds(60))
    .HistoryPageSize(5)
    .Config(cfg => cfg.HideAfterFinish(true))
    .Run();
    if (ctrlin.IsAborted)
    {
        end = true;
    }
}
