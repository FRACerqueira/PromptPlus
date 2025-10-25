// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ConsoleInputControlsSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Simples input control", extraLines: 1);

            var result = PromptPlus.Controls.Input("Name: ", "Enter your name")
                .MaxLength(20)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample input control", extraLines: 1);

            result = PromptPlus.Controls.Input("Name: ", "Enter your name")
                .Styles(InputStyles.Answer, Color.Green)
                .InputToCase(CaseOptions.Any)
                .AcceptInput((charinput) => true)
                .Default("John Doe")
                .DefaultIfEmpty("Name Empty")
                .ChangeDescription((input) => $"Input Length: {input.Length}")
                .MaxLength(150)
                .MaxWidth(20)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash("Sample input control with sugestion", extraLines: 1);

            result = PromptPlus.Controls.Input("Input: ", "Try sugestions")
                .MaxLength(20, 15)
                .SuggestionHandler((input) => ["Suggestion1", "Suggestion2", "Suggestion3"])
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample input control with History and sugestion", extraLines: 1);

            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("TestinputSample")
                .AddHistory("Test1")
                .AddHistory("Test2")
                .AddHistory("Test3")
                .AddHistory("Test4")
                .AddHistory("Test5")
                .Save();

            result = PromptPlus.Controls.Input("Input: ", "Try History")
                .Default("Tes", false)
                .MaxLength(20, 15)
                .EnabledHistory("TestinputSample", (opt) => opt.PageSize(3))
                .SuggestionHandler((input) => ["Suggestion1", "Suggestion2", "Suggestion3"])
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample input control with default by last input by History", extraLines: 1);

            result = PromptPlus.Controls.Input("Input: ", "Try History")
                    .Default("")
                    .MaxLength(20, 20)
                    .EnabledHistory("TestinputSample")
                    .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("TestinputSample")
                .Remove();

            PromptPlus.Widgets.DoubleDash("Sample input control with with input validation filter", extraLines: 1);

            result = PromptPlus.Controls.Input("Input numbers: ", "only number are accepted")
                    .AcceptInput((charinput) => char.IsDigit(charinput))
                    .MaxLength(5)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample input control with validator", extraLines: 1);

            result = PromptPlus.Controls.Input("Input numbers: ", "only number are accepted and minimum of 2 digits")
                    .AcceptInput((charinput) => char.IsDigit(charinput))
                    .PredicateSelected(x => 
                    {
                        if (x.Length < 2)
                        {
                            return false;
                        }
                        return true;
                    })
                    .MaxLength(5, 5)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample input control with secrect value", extraLines: 1);

            result = PromptPlus.Controls.Input("Password: ", "minimum of 8 char, one uppercase, one lowercase, one digit, one special character")
                    .IsSecret('#',true)
                    .PredicateSelected(x =>
                    {
                        var validate = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                        if (!validate.IsMatch(x))
                        {
                            return false;
                        }
                        return true;
                    })
                    .MaxLength(15, 15)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");
        }
    }
}
