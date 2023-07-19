using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace AutoCompleteSamples
{
    internal class Program
    {
        static void Main()
        {
            PromptPlus.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:AutoComplete - minimal usage");
            PromptPlus.AutoComplete("Input value", "Sample autocomplete (minimal 3 chars)")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .Run();

            PromptPlus.DoubleDash("Control:AutoComplete - Changed Spinner");
            PromptPlus.AutoComplete("Input value", "Sample autocomplete (minimal 3 chars)")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .Spinner(SpinnersType.Balloon)
                .Run();

            PromptPlus.DoubleDash("Control:AutoComplete - with CompletionMaxCount and CompletionWaitToStart");
            PromptPlus.AutoComplete("Input value", "Sample autocomplete (minimal 3 chars)")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .CompletionWaitToStart(500)
                .CompletionMaxCount(10)
                .Run();

            PromptPlus.DoubleDash("Control:AutoComplete - with MinimumPrefixLength");
            PromptPlus.AutoComplete("Input value", "Sample autocomplete (minimal 1 chars)")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .MinimumPrefixLength(1)
                .Run();

            PromptPlus.DoubleDash("For other features below see - input samples (same behaviour)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]ValidateOnDemand[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]AddValidators[/] - InputWithValidatorSamples");
            PromptPlus.WriteLine(". [yellow]MaxLenght[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]AcceptInput[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]InputToCase[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLine(". [yellow]Default[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]DefaultIfEmpty[/] - InputBasicSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }

        private static async Task<string[]> MYServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
        {
            if (count == 0)
            {
                count = 10;
            }
            var random = new Random();
            var items = new List<string>();
            for (var i = 0; i < count; i++)
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