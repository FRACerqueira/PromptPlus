// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MaskEditStringControlSamples
{
    /// <summary>
    /// Demonstrates every method of <see cref="IMaskEditStringControl{T}"/>.
    /// The string MaskEdit control edits text according to a mask pattern.
    ///
    /// Mask tokens:
    ///   9 = numeric (0-9)
    ///   L = lower letter, U = upper letter, A = any letter, X = letter or digit, C = custom only
    ///   \  = escape (next char is a literal/constant)
    ///   { } = group, [ ] = custom char list, ( ) = constant value
    ///   Any other char = literal.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // ----------------------------------------------------------------------------
            // Mask(mask, returnWithMask) - REQUIRED: defines the input pattern.
            // Here a US phone. returnWithMask=false returns only the typed characters.
            // ----------------------------------------------------------------------------
            ShowSection("1) Mask - phone (999) 999-9999, result without mask");
            var result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask(@"\(999\)\ 999\-9999")
                .Run();
            PrintResult(result);

            ShowSection("2) Mask - returnWithMask: true (literals kept in the result)");
            result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask("(999) 999-9999", returnWithMask: true)
                .Run();
            PrintResult(result);

            ShowSection("3) Mask - letters and digits UUU-9999 (license plate)");
            result = PromptPlus.Controls.MaskEdit("Plate")
                .Mask("UUU-9999")
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // Escaped literal, custom char list and constant value.
            //   \# -> literal '#'
            //   9[13579] -> a digit restricted to the odd digits
            //   constant upper letter 'A'
            // ----------------------------------------------------------------------------
            ShowSection(@"4) Mask - escaped literal + custom list + constant \#C[13579]\AT");
            result = PromptPlus.Controls.MaskEdit("Code")
                .Mask(@"\#C[13579]\AT")
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // PromptMask(value) - character shown on empty positions (default '_').
            // ----------------------------------------------------------------------------
            ShowSection("5) PromptMask - use '#' for empty positions");
            result = PromptPlus.Controls.MaskEdit("Serial")
                .Mask("AAAA-AAAA")
                .PromptMask('#')
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // InputMode(EditCursorFreely) - cursor moves freely instead of skipping
            // straight to the next input position.
            // ----------------------------------------------------------------------------
            ShowSection("6) InputMode - EditCursorFreely");
            result = PromptPlus.Controls.MaskEdit("Free cursor")
                .Mask("999.999.999-99")
                .InputMode(InputBehavior.EditCursorFreely)
                .Run();
            PrintResult(result);

            ShowSection("7) HideTipInputType - hide the input type hint");
            result = PromptPlus.Controls.MaskEdit("ZIP")
                .Mask("99999-999")
                .HideTipInputType()
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // Default(value) - pre-fills the control with an initial value.
            // The default length must match the mask (here without mask literals).
            // ----------------------------------------------------------------------------
            ShowSection("8) Default - pre-filled value");
            result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask("(999) 999-9999")
                .Default("1234567890")
                .Run();
            PrintResult(result);

            ShowSection("9) DefaultIfEmpty - press ENTER with empty input to use fallback");
            result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask("(999) 999-9999", returnWithMask: true)
                .DefaultIfEmpty("(000) 000-0000")
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,bool>) - simple validation (no custom message).
            // ----------------------------------------------------------------------------
            ShowSection("10) PredicateSelected(bool) - must not start with '0'");
            result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask("(999) 999-9999")
                .PredicateSelected(value => !value.StartsWith('0'))
                .Run();
            PrintResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,(bool,string?)>) - validation with a custom
            // error message shown to the user.
            // ----------------------------------------------------------------------------
            ShowSection("11) PredicateSelected(tuple) - custom error message:  must not start with '0' ");
            result = PromptPlus.Controls.MaskEdit("Phone")
                .Mask("(999) 999-9999")
                .PredicateSelected(value =>
                    value.StartsWith('0')
                        ? (false, "must not start with '0'")
                        : (true, null))
                .Run();
            PrintResult(result);

            ShowSection("12) Styles - custom prompt, answer and tagged-info regions");
            result = PromptPlus.Controls.MaskEdit("Styled")
                .Mask("AAAA-9999")
                .Styles(MaskEditStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(MaskEditStyles.Answer, new Style(Color.Green, Color.Black))
                .Styles(MaskEditStyles.TaggedInfo, new Style(Color.Cyan, Color.Black))
                .Run();
            PrintResult(result);

            ShowSection("13) Options - description, abort key and keep result visible");
            result = PromptPlus.Controls.MaskEdit("Product code")
                .Mask("UUU-9999")
                .Options(opt =>
                {
                    opt.Description("Format: 3 letters, dash, 4 digits");
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("14) Run(token) - auto-cancels after 4 seconds");
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4)))
            {
                result = PromptPlus.Controls.MaskEdit("Type before timeout")
                    .Mask("999-999")
                    .Run(cts.Token);
                PrintResult(result);
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

        private static void PrintResult(ResultPrompt<string> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: '{result.Content}'");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
