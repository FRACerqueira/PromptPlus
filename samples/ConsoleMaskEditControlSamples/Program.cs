// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;

namespace ConsoleMaskEditControlSamples
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

            PromptPlus.Widgets.DoubleDash("MaskEdit('\\X\\#9\\:LUAX\\-C[AB1]\\#]\\X') and return widthout mask", extraLines: 1);

            var resultmaskstring = PromptPlus.Controls.MaskEdit("Masked : ")
                .Mask(@"\X\#9\:LUAX\-C[AB1]\#\X")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskstring.IsAborted}, Value: {resultmaskstring.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("MaskEdit('\\#9\\:LUAX\\-C[AB1]\\#') and return with mask", extraLines: 1);

            resultmaskstring = PromptPlus.Controls.MaskEdit("Masked : ")
                .Mask(@"\#9\:LUAX\-C[AB1]\#",true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskstring.IsAborted}, Value: {resultmaskstring.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("MaskEdit('\\#9\\:LUAX\\-C[AB1]\\#') and free navigation", extraLines: 1);

            resultmaskstring = PromptPlus.Controls.MaskEdit("Masked : ")
                .Mask(@"\#9\:LUAX\-C[AB1]\#")
                .InputMode(InputBehavior.EditCursorFreely)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskstring.IsAborted}, Value: {resultmaskstring.Content}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash($"MaskDateTime control culture({cult.Name})", extraLines: 1);

            var resultmaskdate = PromptPlus.Controls.MaskDateTime("Masked : ")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdate.IsAborted}, Value: {resultmaskdate.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDateTime control with fixed day and hour, culture({cult.Name})", extraLines: 1);

            resultmaskdate = PromptPlus.Controls.MaskDateTime("Masked : ")
                .FixedValues(DateTimePart.Month,10)
                .FixedValues(DateTimePart.Hour, -1) // -1 means datetime(now) current datepart value
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdate.IsAborted}, Value: {resultmaskdate.Content}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash($"MaskDate control culture({cult.Name})", extraLines: 1);

            resultmaskdate = PromptPlus.Controls.MaskDate("Masked : ")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdate.IsAborted}, Value: {resultmaskdate.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDate control with week info, culture({cult.Name})", extraLines: 1);

            resultmaskdate = PromptPlus.Controls.MaskDate("Masked : ")
                .Default(DateTime.Now)
                .WeekTypeMode(WeekType.WeekShort)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdate.IsAborted}, Value: {resultmaskdate.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDateOnly control, culture({cult.Name})", extraLines: 1);

            var resultmaskdateonly = PromptPlus.Controls.MaskDateOnly("Masked : ")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdateonly.IsAborted}, Value: {resultmaskdateonly.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskTime control culture({cult.Name})", extraLines: 1);

            resultmaskdate = PromptPlus.Controls.MaskTime("Masked : ")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmaskdate.IsAborted}, Value: {resultmaskdate.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskTimeOnly control culture({cult.Name})", extraLines: 1);

            var resultmasktimeonly = PromptPlus.Controls.MaskTimeOnly("Masked : ")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultmasktimeonly.IsAborted}, Value: {resultmasktimeonly.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDecimalCurrency(FixedCursor) control culture({cult.Name})", extraLines: 1);

            var resultdecimal = PromptPlus.Controls.MaskDecimalCurrency("Masked : ")
                .NumberFormat(28,28, withsignal: true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultdecimal.IsAborted}, Value: {resultdecimal.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDecimal control culture({cult.Name})", extraLines: 1);

            resultdecimal = PromptPlus.Controls.MaskDecimal("Masked : ")
                .NumberFormat(28,28, withsignal: true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultdecimal.IsAborted}, Value: {resultdecimal.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDoubleCurrency control culture({cult.Name})", extraLines: 1);

            var resultdouble = PromptPlus.Controls.MaskDoubleCurrency("Masked : ")
                .NumberFormat(15,15, withsignal: true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultdouble.IsAborted}, Value: {resultdouble.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskDouble control culture({cult.Name})", extraLines: 1);

            resultdouble = PromptPlus.Controls.MaskDouble("Masked : ")
                .NumberFormat(15,15, withsignal: true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resultdouble.IsAborted}, Value: {resultdouble.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskInteger control culture({cult.Name})", extraLines: 1);

            var resulint = PromptPlus.Controls.MaskInteger("Masked : ")
                .NumberFormat(10, withsignal: true)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {resulint.IsAborted}, Value: {resulint.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskLong control culture({cult.Name})", extraLines: 1);

            var resullong = PromptPlus.Controls.MaskLong("Masked : ")
                .NumberFormat(19, withsignal: true)
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {resullong.IsAborted}, Value: {resullong.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"MaskLong control with signal colors, culture({cult.Name})", extraLines: 1);

            resullong = PromptPlus.Controls.MaskLong("Masked : ")
                .NumberFormat(19, withsignal: true)
                .Styles(MaskEditStyles.PositiveValue, Color.LightGreen)
                .Styles(MaskEditStyles.NegativeValue, Color.Red1)
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {resullong.IsAborted}, Value: {resullong.Content}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash($"MaskLong control widthout signal, culture({cult.Name})", extraLines: 1);

            resullong = PromptPlus.Controls.MaskLong("Masked : ")
                .NumberFormat(19, withsignal: false)
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {resullong.IsAborted}, Value: {resullong.Content}");
            PromptPlus.Console.WriteLine("");
        }
    }
}
