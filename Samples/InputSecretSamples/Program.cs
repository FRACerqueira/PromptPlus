// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.WriteLine("Hello, World!");
//Ensure default Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus.DoubleDash("Control:Input - secret");

var in1 = PromptPlus
    .Input("Input secret sample1")
    .IsSecret()
    .Run();

if (!in1.IsAborted)
{
    PromptPlus.WriteLine($"You input is {in1.Value}");
}

PromptPlus.DoubleDash("Control:Input - secret change mask to '*'");

PromptPlus.Config.PasswordViewPress = new HotKey(UserHotKey.F7);

var in2 = PromptPlus
    .Input("Input secret sample2")
    .IsSecret('*')
    .Run();

if (!in2.IsAborted)
{
    PromptPlus.WriteLine($"You input is {in2.Value}");
}

PromptPlus.DoubleDash("Control:Input - secret with change view");
PromptPlus
    .Input("Input secret sample2")
    .IsSecret()
    .EnabledViewSecret()
    .Run();

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();

