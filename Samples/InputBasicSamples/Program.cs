// See https://aka.ms/new-console-template for more information
using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.Clear();
PromptPlus.WriteLine("Hello, World!");

//Ensure default Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus.DoubleDash("Control:Input - minimal usage");

var in1 = PromptPlus
    .Input("Input sample1")
    .Run();

if (!in1.IsAborted)
{
    PromptPlus.WriteLine($"You input is {in1.Value}");
}

PromptPlus.DoubleDash("Control:Input - with color");
PromptPlus
    .Input("Input [blue]sample2[/]", "with [yellow]description[/]")
    .Run();

PromptPlus.DoubleDash("Control input - with MaxLenght");
PromptPlus
    .Input("Input sample3", "My MaxLenght is 5")
    .MaxLenght(5)
    .Run();

PromptPlus.DoubleDash("Control:Input - with CaseOptions");
PromptPlus
    .Input("Input sample4", "input to upper case")
    .InputToCase(CaseOptions.Uppercase)
    .Run();

PromptPlus.DoubleDash("Control:Input - with filter keypress");
PromptPlus
    .Input("Input sample5", "valid only number")
    .AcceptInput(char.IsNumber)
    .Run();

PromptPlus.DoubleDash("Control:Input - with default value");
PromptPlus
    .Input("Input sample6", "with initial value")
    .Default("initial value")
    .Run();

PromptPlus.DoubleDash("Control:Input - with default value when empty");
PromptPlus
    .Input("Input sample7","Please press enter with no input")
    .DefaultIfEmpty("empty value")
    .Run();

PromptPlus.DoubleDash("Control input - Dynamically change the description using a user role");
PromptPlus
    .Input("Input sample8", "Dynamically change the description using a user role")
    .ChangeDescription((inputvalue) =>
    {
        var aux = $"Input [yellow]lenght[/] is {(inputvalue ?? string.Empty).Length}";
        return aux;
    })
    .Run();

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key",cfg => cfg.ShowTooltip(false))
    .Run();
