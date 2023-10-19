// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PPlus;
using PPlus.Controls;

internal class Program
{
    class Myclass
    {
        public int Qtd { get; set; }

        [MinLength(4)]
        [MaxLength(6)]
        public string? Text { get; set; }
    };

    static void Main()
    {
        PromptPlus.WriteLine("Hello, World!");
        //Ensure ValueResult Culture for all controls
        PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

        PromptPlus.DoubleDash("Control:Input - with MaxLength and validator");
        var in1 = PromptPlus
            .Input("Input sample1", "My MaxLength is 5 and MinLength is 2")
            .MaxLength(5)
            .AddValidators(PromptValidators.MinLength(2))
            .Run();

        if (!in1.IsAborted)
        {
            PromptPlus.WriteLine($"You input is {in1.Value}");
        }

        PromptPlus.DoubleDash("Control:Input - with ValidateOnDemand - Execute validators foreach input");
        PromptPlus
            .Input("Input sample1", "My MaxLength is 5 and MinLength is 2")
            .MaxLength(5)
            .AddValidators(PromptValidators.MinLength(2))
            .ValidateOnDemand()
            .Run();

        var instmyclass = new Myclass() { Qtd = 1, Text = "" };

        using (PromptPlus.EscapeColorTokens())
        {
            PromptPlus.DoubleDash("Control:Input - with import validator");
            PromptPlus.WriteLine("[MinLength(4)]");
            PromptPlus.WriteLine("[MaxLength(6)]");
            PromptPlus.WriteLine("public string? Text { get; set; }");
            PromptPlus.WriteLine("");
        }

        PromptPlus
            .Input("Input sample2", "import validator from decorate")
            .Default(instmyclass.Text)
            .AddValidators(PromptValidators.ImportValidators(instmyclass,x => x!.Text!))
            .Run();

        PromptPlus.WriteLines(2);
        PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
            .Run();

    }
}
