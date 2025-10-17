// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ConsoleAutoCompleteControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            var cult = Thread.CurrentThread.CurrentCulture;

            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Simples Autocomplete input control minimal usage", extraLines: 1);

            var result = PromptPlus.Controls.AutoComplete("Input value : ", "Sample autocomplete (minimal 3 chars)")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample Autocomplete input control", extraLines: 1);

            result = PromptPlus.Controls.AutoComplete("Input value : : ", "Sample autocomplete (minimal 2 chars)")
                .MinimumPrefixLength(2)
                .CompletionAsyncService(MYServiceCompleteAsync)
                .CompletionMaxCount(10)
                .CompletionWaitToStart(200)
                .Spinner(SpinnersType.DotsScrolling)
                .Styles(AutoComleteStyles.Answer, Color.Green)
                .InputToCase(CaseOptions.Uppercase)
                .AcceptInput((charinput) => true)
                .Default("John Doe")
                .DefaultIfEmpty("Input Empty")
                .ChangeDescription((input) => $"Input Length: {input.Length}")
                .MaxLength(150)
                .MaxWidth(20)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample Autocomplete input control with History", extraLines: 1);

            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("TestAutocompleteSample")
                .AddHistory("Test1")
                .Save();

            result = PromptPlus.Controls.AutoComplete("Input: ", "Try History")
                .Default("xx", true)
                .CompletionAsyncService(MYServiceCompleteAsync)
                .EnabledHistory("TestAutocompleteSample")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("TestAutocompleteSample")
                .Remove();


            PromptPlus.Widgets.DoubleDash("Sample Autocomplete input control with validator", extraLines: 1);

            result = PromptPlus.Controls.AutoComplete("Input numbers: ", "only Letter are accepted and minimum of 5 Letter")
                    .CompletionAsyncService(MYServiceCompleteAsync)
                    .AcceptInput((charinput) => char.IsLetter(charinput))
                    .PredicateSelected(x => 
                    {
                        if (x.Length < 5)
                        { 
                            return false;
                        }
                        return true;
                    })
                    .MaxLength(8, 5)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample Autocomplete input control with custom class", extraLines: 1);

            result = PromptPlus.Controls.AutoComplete("Input code city: ", "Press 'CNA','CEU' or 'CAS' ")
                    .CompletionAsyncService(MYServiceCityCompleteAsync)
                    .InputToCase(CaseOptions.Uppercase)
                    .TextSelector((item) => item[..6])
                    .PredicateSelected(x =>
                    {
                        if (x.Length < 6)
                        {
                            return false;
                        }
                        if (!MyCities().Any(c => c.code == x))
                        {
                            return false;
                        }
                        return true;
                    })
                    .MaxLength(6, 6)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");
        }

        private static async Task<string[]> MYServiceCityCompleteAsync(string prefixText, CancellationToken token)
        {
            return await Task.FromResult(MyCities()
                .Select(x => $"{x.code} - {x.City}, {x.other}")
                .Where(x => x.StartsWith(prefixText)).ToArray());
        }

        private static (string code, string City, string other)[] MyCities()
        {
            return
            [
                ("CNA001","Seattle","any1"),
                ("CNA002","New York","any4"),
                ("CEU001","London","any2"),
                ("CAS001","Tokyo","any3"),
                ("CAS002","Singapore","any5"),
                ("CAS003","Shanghai","any6"),
            ];
        }

        private static async Task<string[]> MYServiceCompleteAsync(string prefixText,  CancellationToken cancellationToken)
        {
            var random = new Random();
            var items = new List<string>();
            for (var i = 0; i < 200; i++)
            {
                var c1 = (char)random.Next(65, 90);
                var c2 = (char)random.Next(97, 122);
                var c3 = (char)random.Next(97, 122);

                items.Add(prefixText + c1 + c2 + c3);
            }
            //delay for sample purpose only
            cancellationToken.WaitHandle.WaitOne(2000);
            return await Task.FromResult(items.ToArray());
        }

    }
}
