// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using System.Globalization;
using PPlus;

PromptPlus.WriteLine("Hello, World!");
//Ensure default Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");


PromptPlus.DoubleDash("Control:Input - Overwrite default start value with last result saved on history.");
PromptPlus
    .Input("Input sample","input last input overwrite. input a new value and re-run (max. 10 seg.) to view feature")
    .Default("foo")
    .OverwriteDefaultFrom("SampleInputOverwriteDefaultFrom", TimeSpan.FromSeconds(10))
    .Run();

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();