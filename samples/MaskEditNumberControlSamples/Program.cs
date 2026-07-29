// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MaskEditNumberControlSamples
{
    /// <summary>
    /// Demonstrates every method of <see cref="IMaskEditNumberControl{T}"/>.
    /// The number MaskEdit control edits whole numbers (int / long).
    ///
    /// Factory helpers used here:
    ///   PromptPlus.Controls.MaskInteger(...) -> IMaskEditNumberControl&lt;int&gt;
    ///   PromptPlus.Controls.MaskLong(...)    -> IMaskEditNumberControl&lt;long&gt;
    ///
    /// The mask itself is built by NumberFormat(...); you don't pass a mask string.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // ----------------------------------------------------------------------------
            // NumberFormat(integerpart, withsignal, withseparatorgroup)
            // Builds the numeric mask. Here: up to 6 digits, grouped by thousands.
            // ----------------------------------------------------------------------------
            ShowSection("1) NumberFormat - int, 6 digits, with group separator");
            var intResult = PromptPlus.Controls.MaskInteger("Quantity")
                .NumberFormat(6)
                .Run();
            PrintIntResult(intResult);

            ShowSection("2) NumberFormat - int, 5 digits, no group separator");
            intResult = PromptPlus.Controls.MaskInteger("PIN")
                .NumberFormat(5, withseparatorgroup: false)
                .Run();
            PrintIntResult(intResult);

            ShowSection("3) NumberFormat - int with signal (type + or - )");
            intResult = PromptPlus.Controls.MaskInteger("Temperature")
                .NumberFormat(3, withsignal: true)
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // PromptMask(value) - character shown on empty positions (default '_').
            // ----------------------------------------------------------------------------
            ShowSection("4) PromptMask - use '0' on empty positions");
            intResult = PromptPlus.Controls.MaskInteger("Counter")
                .NumberFormat(4, withseparatorgroup: false)
                .PromptMask('0')
                .Run();
            PrintIntResult(intResult);

            ShowSection("5) HideTipInputType - hide the input type hint");
            intResult = PromptPlus.Controls.MaskInteger("Value")
                .NumberFormat(5)
                .HideTipInputType()
                .Run();
            PrintIntResult(intResult);

            ShowSection("6) Default - pre-filled value 1234");
            intResult = PromptPlus.Controls.MaskInteger("Quantity")
                .NumberFormat(6)
                .Default(1234)
                .Run();
            PrintIntResult(intResult);

            ShowSection("7) DefaultIfEmpty - press ENTER empty to return 100");
            intResult = PromptPlus.Controls.MaskInteger("Quantity")
                .NumberFormat(6)
                .DefaultIfEmpty(100)
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // Culture(CultureInfo) - affects the group separator character.
            // pt-BR uses '.' as the thousands separator.
            // ----------------------------------------------------------------------------
            ShowSection("8) Culture(CultureInfo) - pt-BR grouping");
            intResult = PromptPlus.Controls.MaskInteger("Quantidade")
                .NumberFormat(7)
                .Culture(new CultureInfo("pt-BR"))
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // Culture(string) - same as above but using a culture name.
            // ----------------------------------------------------------------------------
            ShowSection("9) Culture(string) - de-DE grouping");
            intResult = PromptPlus.Controls.MaskInteger("Menge")
                .NumberFormat(7)
                .Culture("de-DE")
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,bool>) - simple validation.
            // ----------------------------------------------------------------------------
            ShowSection("10) PredicateSelected(bool) - must be even");
            intResult = PromptPlus.Controls.MaskInteger("Even number")
                .NumberFormat(4, withseparatorgroup: false)
                .PredicateSelected(value => value % 2 == 0)
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,(bool,string?)>) - validation + custom message.
            // ----------------------------------------------------------------------------
            ShowSection("11) PredicateSelected(tuple) - range 1..100 with message");
            intResult = PromptPlus.Controls.MaskInteger("Percentage")
                .NumberFormat(3, withseparatorgroup: false)
                .PredicateSelected(value =>
                    value is >= 1 and <= 100
                        ? (true, null)
                        : (false, "Enter a value between 1 and 100."))
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // Styles(styleType, style) - customize regions. Positive/Negative styles
            // apply when signal is enabled.
            // ----------------------------------------------------------------------------
            ShowSection("12) Styles - prompt, answer and positive/negative colors");
            intResult = PromptPlus.Controls.MaskInteger("Balance")
                .NumberFormat(5, withsignal: true)
                .Styles(MaskEditStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(MaskEditStyles.PositiveValue, new Style(Color.Green, Color.Black))
                .Styles(MaskEditStyles.NegativeValue, new Style(Color.Red, Color.Black))
                .Run();
            PrintIntResult(intResult);

            ShowSection("13) Options - description and abort key");
            intResult = PromptPlus.Controls.MaskInteger("Year")
                .NumberFormat(4, withseparatorgroup: false)
                .Options(opt =>
                {
                    opt.Description("Enter a 4-digit year");
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                })
                .Run();
            PrintIntResult(intResult);

            // ----------------------------------------------------------------------------
            // MaskLong - long variant (large ranges).
            // ----------------------------------------------------------------------------
            ShowSection("14) MaskLong - long value, 15 digits grouped");
            var longResult = PromptPlus.Controls.MaskLong("Card number")
                .NumberFormat(15)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {longResult.IsAborted}, Value: {longResult.Content}");
            PromptPlus.Console.WriteLine(string.Empty);

            ShowSection("15) Run(token) - auto-cancels after 4 seconds");
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4)))
            {
                intResult = PromptPlus.Controls.MaskInteger("Type before timeout")
                    .NumberFormat(4, withseparatorgroup: false)
                    .Run(cts.Token);
                PrintIntResult(intResult);
            }
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintIntResult(ResultPrompt<int> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
