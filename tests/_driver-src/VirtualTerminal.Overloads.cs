// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

// Typed Write/WriteLine/WriteFormat/WriteLineFormat overloads for char, char[], object, bool, double,
// float, decimal, int and long (without an explicit Style) have no logic of their own: they just route
// to the core Write(string?, Style, bool)/WriteLine(string?, Style, bool) using CurrentStyle, mirroring
// AnsiConsoleAdapter.cs (ConsolePlus/src/ConsoleAbstractions/AnsiConsoleAdapter.cs:358-995) verbatim,
// minus the locking/disposal concerns that do not apply to this in-memory driver.

using System.Globalization;

namespace ConsolePlusLibrary.Testing
{
    public sealed partial class VirtualTerminal
    {
        public void Write(char value) => Write(value.ToString(), CurrentStyle, false);
        public void Write(char value, Style style) => Write(value.ToString(), style, false);
        public void Write(char value, bool clearrestofline) => Write(value.ToString(), CurrentStyle, clearrestofline);
        public void Write(char value, Style style, bool clearrestofline) => Write(value.ToString(), style, clearrestofline);

        public void Write(char[]? value) { if (value != null) { Write(new string(value), CurrentStyle, false); } }
        public void Write(char[]? value, Style style) { if (value != null) { Write(new string(value), style, false); } }
        public void Write(char[]? value, bool clearrestofline) { if (value != null) { Write(new string(value), CurrentStyle, clearrestofline); } }
        public void Write(char[]? value, Style style, bool clearrestofline) { if (value != null) { Write(new string(value), style, clearrestofline); } }

        public void Write(object? value) { if (value != null) { Write(value.ToString(), CurrentStyle, false); } }
        public void Write(object? value, Style style) { if (value != null) { Write(value.ToString(), style, false); } }
        public void Write(object? value, bool clearrestofline) { if (value != null) { Write(value.ToString(), CurrentStyle, clearrestofline); } }
        public void Write(object? value, Style style, bool clearrestofline) { if (value != null) { Write(value.ToString(), style, clearrestofline); } }

        public void Write(bool value) => Write(value.ToString(CultureInfo.InvariantCulture), CurrentStyle, false);
        public void Write(bool value, Style style) => Write(value.ToString(CultureInfo.InvariantCulture), style, false);
        public void Write(bool value, bool clearrestofline) => Write(value.ToString(CultureInfo.InvariantCulture), CurrentStyle, clearrestofline);
        public void Write(bool value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.InvariantCulture), style, clearrestofline);

        public void Write(double value) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void Write(double value, Style style) => Write(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void Write(double value, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void Write(double value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void Write(float value) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void Write(float value, Style style) => Write(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void Write(float value, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void Write(float value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void Write(decimal value) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void Write(decimal value, Style style) => Write(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void Write(decimal value, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void Write(decimal value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void Write(int value) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void Write(int value, Style style) => Write(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void Write(int value, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void Write(int value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void Write(long value) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void Write(long value, Style style) => Write(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void Write(long value, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void Write(long value, Style style, bool clearrestofline) => Write(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void Write(string? value) => Write(value, CurrentStyle, false);
        public void Write(string? value, Style style) => Write(value, style, false);
        public void Write(string? value, bool clearrestofline) => Write(value, CurrentStyle, clearrestofline);

        public void WriteFormat(string format, params object?[] arg) => Write(string.Format(CultureInfo.CurrentCulture, format, arg), CurrentStyle, false);
        public void WriteFormat(string format, Style style, params object?[] arg) => Write(string.Format(CultureInfo.CurrentCulture, format, arg), style, false);
        public void WriteFormat(string format, bool clearrestofline, params object?[] arg) => Write(string.Format(CultureInfo.CurrentCulture, format, arg), CurrentStyle, clearrestofline);
        public void WriteFormat(string format, Style style, bool clearrestofline, params object?[] arg) => Write(string.Format(CultureInfo.CurrentCulture, format, arg), style, clearrestofline);

        public void WriteLine(char value) => WriteLine(value.ToString(), CurrentStyle, false);
        public void WriteLine(char value, Style style) => WriteLine(value.ToString(), style, false);
        public void WriteLine(char value, bool clearrestofline) => WriteLine(value.ToString(), CurrentStyle, clearrestofline);
        public void WriteLine(char value, Style style, bool clearrestofline) => WriteLine(value.ToString(), style, clearrestofline);

        public void WriteLine(char[]? value) { if (value != null) { WriteLine(new string(value), CurrentStyle, false); } }
        public void WriteLine(char[]? value, Style style) { if (value != null) { WriteLine(new string(value), style, false); } }
        public void WriteLine(char[]? value, bool clearrestofline) { if (value != null) { WriteLine(new string(value), CurrentStyle, clearrestofline); } }
        public void WriteLine(char[]? value, Style style, bool clearrestofline) { if (value != null) { WriteLine(new string(value), style, clearrestofline); } }

        public void WriteLine(bool value) => WriteLine(value.ToString(CultureInfo.InvariantCulture), CurrentStyle, false);
        public void WriteLine(bool value, Style style) => WriteLine(value.ToString(CultureInfo.InvariantCulture), style, false);
        public void WriteLine(bool value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.InvariantCulture), CurrentStyle, clearrestofline);
        public void WriteLine(bool value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.InvariantCulture), style, clearrestofline);

        public void WriteLine(double value) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void WriteLine(double value, Style style) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void WriteLine(double value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void WriteLine(double value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void WriteLine(float value) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void WriteLine(float value, Style style) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void WriteLine(float value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void WriteLine(float value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void WriteLine(decimal value) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void WriteLine(decimal value, Style style) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void WriteLine(decimal value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void WriteLine(decimal value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void WriteLine(int value) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void WriteLine(int value, Style style) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void WriteLine(int value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void WriteLine(int value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void WriteLine(long value) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, false);
        public void WriteLine(long value, Style style) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, false);
        public void WriteLine(long value, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), CurrentStyle, clearrestofline);
        public void WriteLine(long value, Style style, bool clearrestofline) => WriteLine(value.ToString(CultureInfo.CurrentCulture), style, clearrestofline);

        public void WriteLine(object? value) { if (value != null) { WriteLine(value.ToString(), CurrentStyle, false); } }
        public void WriteLine(object? value, Style style) { if (value != null) { WriteLine(value.ToString(), style, false); } }
        public void WriteLine(object? value, bool clearrestofline) { if (value != null) { WriteLine(value.ToString(), CurrentStyle, clearrestofline); } }
        public void WriteLine(object? value, Style style, bool clearrestofline) { if (value != null) { WriteLine(value.ToString(), style, clearrestofline); } }

        public void WriteLine(string? value) => WriteLine(value, CurrentStyle, false);
        public void WriteLine(string? value, Style style) => WriteLine(value, style, false);
        public void WriteLine(string? value, bool clearrestofline) => WriteLine(value, CurrentStyle, clearrestofline);

        public void WriteLineFormat(string format, params object?[] arg) => WriteLine(string.Format(CultureInfo.CurrentCulture, format, arg), CurrentStyle, false);
        public void WriteLineFormat(string format, Style style, params object?[] arg) => WriteLine(string.Format(CultureInfo.CurrentCulture, format, arg), style, false);
        public void WriteLineFormat(string format, bool clearrestofline, params object?[] arg) => WriteLine(string.Format(CultureInfo.CurrentCulture, format, arg), CurrentStyle, clearrestofline);
        public void WriteLineFormat(string format, Style style, bool clearrestofline, params object?[] arg) => WriteLine(string.Format(CultureInfo.CurrentCulture, format, arg), style, clearrestofline);
    }
}
