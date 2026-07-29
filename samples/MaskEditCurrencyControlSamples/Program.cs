// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MaskEditCurrencyControlSamples
{
    /// <summary>
    /// Demonstrates every method of <see cref="IMaskEditCurrencyControl{T}"/>.
    /// The currency MaskEdit control edits decimal / double values with an integer
    /// part and a decimal part.
    ///
    /// Factory helpers used here:
    ///   MaskDecimal(...)          -> IMaskEditCurrencyControl&lt;decimal&gt; (no currency symbol)
    ///   MaskDecimalCurrency(...)  -> IMaskEditCurrencyControl&lt;decimal&gt; (with currency symbol)
    ///   MaskDouble(...)           -> IMaskEditCurrencyControl&lt;double&gt;
    ///   MaskDoubleCurrency(...)   -> IMaskEditCurrencyControl&lt;double&gt; (with currency symbol)
    ///
    /// The mask is built by NumberFormat(integerpart, decimalpart, ...).
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // ----------------------------------------------------------------------------
            // NumberFormat(integerpart, decimalpart, withsignal, withseparatorgroup)
            // MaskDecimal: no currency symbol. 6 integer digits + 2 decimals.
            // ----------------------------------------------------------------------------
            ShowSection("1) NumberFormat - decimal, 6 int + 2 dec, grouped");
            var result = PromptPlus.Controls.MaskDecimal("Amount")
                .NumberFormat(6, 2)
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // MaskDecimalCurrency: prepends the culture currency symbol ($, R$, ...).
            // ----------------------------------------------------------------------------
            ShowSection("2) MaskDecimalCurrency - with currency symbol");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(6, 2)
                .Run();
            PrintDecimalResult(result);

            ShowSection("3) NumberFormat - with signal (+/-)");
            result = PromptPlus.Controls.MaskDecimal("Balance")
                .NumberFormat(6, 2, withsignal: true)
                .Run();
            PrintDecimalResult(result);

            ShowSection("4) NumberFormat - 4 decimals, no group separator");
            result = PromptPlus.Controls.MaskDecimal("Rate")
                .NumberFormat(3, 4, withseparatorgroup: false)
                .Run();
            PrintDecimalResult(result);

            ShowSection("5) PromptMask - use '0' on empty positions");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(5, 2)
                .PromptMask('0')
                .Run();
            PrintDecimalResult(result);

            ShowSection("6) HideTipInputType - hide the input type hint");
            result = PromptPlus.Controls.MaskDecimal("Amount")
                .NumberFormat(6, 2)
                .HideTipInputType()
                .Run();
            PrintDecimalResult(result);

            ShowSection("7) Default - pre-filled value 1234.56");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(6, 2)
                .Default(1234.56m)
                .Run();
            PrintDecimalResult(result);

            ShowSection("8) DefaultIfEmpty - press ENTER empty to return 0.00");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(6, 2)
                .DefaultIfEmpty(0m)
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // Culture(CultureInfo) - drives the currency symbol and separators.
            // ----------------------------------------------------------------------------
            ShowSection("9) Culture(CultureInfo) - pt-BR (R$ and , as decimal)");
            result = PromptPlus.Controls.MaskDecimalCurrency("Preço")
                .NumberFormat(6, 2)
                .Culture(new CultureInfo("pt-BR"))
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // Culture(string) - same, using a culture name.
            // ----------------------------------------------------------------------------
            ShowSection("10) Culture(string) - de-DE currency");
            result = PromptPlus.Controls.MaskDecimalCurrency("Preis")
                .NumberFormat(6, 2)
                .Culture("de-DE")
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,bool>) - simple validation.
            // ----------------------------------------------------------------------------
            ShowSection("11) PredicateSelected(bool) - must be greater than zero");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(6, 2)
                .PredicateSelected(value => value > 0m)
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,(bool,string?)>) - validation + custom message.
            // ----------------------------------------------------------------------------
            ShowSection("12) PredicateSelected(tuple) - max 1000 with message");
            result = PromptPlus.Controls.MaskDecimalCurrency("Price")
                .NumberFormat(6, 2)
                .PredicateSelected(value =>
                    value <= 1000m
                        ? (true, null)
                        : (false, "The price cannot exceed 1000."))
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // Styles(styleType, style) - customize regions (incl. positive/negative).
            // ----------------------------------------------------------------------------
            ShowSection("13) Styles - prompt, answer and positive/negative colors");
            result = PromptPlus.Controls.MaskDecimal("Balance")
                .NumberFormat(6, 2, withsignal: true)
                .Styles(MaskEditStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(MaskEditStyles.PositiveValue, new Style(Color.Green, Color.Black))
                .Styles(MaskEditStyles.NegativeValue, new Style(Color.Red, Color.Black))
                .Run();
            PrintDecimalResult(result);

            ShowSection("14) Options - description and abort key");
            result = PromptPlus.Controls.MaskDecimalCurrency("Total")
                .NumberFormat(6, 2)
                .Options(opt =>
                {
                    opt.Description("Enter the total amount");
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                })
                .Run();
            PrintDecimalResult(result);

            // ----------------------------------------------------------------------------
            // MaskDouble / MaskDoubleCurrency - double variants.
            // ----------------------------------------------------------------------------
            ShowSection("15) MaskDoubleCurrency - double value with currency symbol");
            var dblResult = PromptPlus.Controls.MaskDoubleCurrency("Weight cost")
                .NumberFormat(5, 3)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {dblResult.IsAborted}, Value: {dblResult.Content}");
            PromptPlus.Console.WriteLine(string.Empty);

            ShowSection("16) Run(token) - auto-cancels after 4 seconds");
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4)))
            {
                result = PromptPlus.Controls.MaskDecimalCurrency("Type before timeout")
                    .NumberFormat(5, 2)
                    .Run(cts.Token);
                PrintDecimalResult(result);
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

        private static void PrintDecimalResult(ResultPrompt<decimal> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
