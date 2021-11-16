// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

using static PromptPlusControls.PromptPlus;


namespace PromptPlusObjects
{
    public struct ColorToken : IEquatable<ColorToken>
    {
        internal ColorToken(string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null, bool underline = false)
        {
            Text = text;
            Color = color ?? _consoleDriver.ForegroundColor;
            BackgroundColor = backgroundColor ?? _consoleDriver.BackgroundColor;
            AnsiColor = string.Format("\x1b[{0};{1}m", ToAnsiColor(Color), ToAnsiBgColor(BackgroundColor));
            Underline = underline;
        }

        public bool Underline { get; set; }

        public string AnsiColor { get; }

        public string Text { get; }

        public ConsoleColor Color { get; }

        public ConsoleColor BackgroundColor { get; }

        public static bool operator ==(ColorToken left, ColorToken right) => left.Equals(right);

        public static bool operator !=(ColorToken left, ColorToken right) => !left.Equals(right);

        public ColorToken Mask(ConsoleColor? defaultColor, ConsoleColor? defaultBackgroundColor)
        {
            if (Color != _consoleDriver.ForegroundColor || BackgroundColor != _consoleDriver.BackgroundColor)
            {
                return this;
            }
            return new(Text, Color == _consoleDriver.ForegroundColor ? defaultColor : Color, BackgroundColor == _consoleDriver.BackgroundColor ? defaultBackgroundColor : BackgroundColor, Underline);
        }

        public static implicit operator ColorToken(string text)
        {
            return new ColorToken(text);
        }

        public override string ToString() => Text;

        public override int GetHashCode() => Text == null ? 0 : Text.GetHashCode();

        public override bool Equals(object obj) => obj is ColorToken token && Equals(token);

        public bool Equals(ColorToken other) => Text == other.Text && Color == other.Color && BackgroundColor == other.BackgroundColor && Underline == other.Underline;

        private static string ToAnsiColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return "30";
                case ConsoleColor.Blue:
                    return "94";
                case ConsoleColor.Cyan:
                    return "96";
                case ConsoleColor.DarkBlue:
                    return "34";
                case ConsoleColor.DarkCyan:
                    return "36";
                case ConsoleColor.DarkGray:
                    return "1;30";
                case ConsoleColor.DarkGreen:
                    return "32";
                case ConsoleColor.DarkMagenta:
                    return "35";
                case ConsoleColor.DarkRed:
                    return "31";
                case ConsoleColor.DarkYellow:
                    return "33";
                case ConsoleColor.Gray:
                    return "37";
                case ConsoleColor.Green:
                    return "92";
                case ConsoleColor.Magenta:
                    return "95";
                case ConsoleColor.Red:
                    return "91";
                case ConsoleColor.White:
                    return "97";
                case ConsoleColor.Yellow:
                    return "93";
                default:
                    break;
            }
            return null;
        }

        private static string ToAnsiBgColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return "40";
                case ConsoleColor.Blue:
                    return "104";
                case ConsoleColor.Cyan:
                    return "106";
                case ConsoleColor.DarkBlue:
                    return "44";
                case ConsoleColor.DarkCyan:
                    return "46";
                case ConsoleColor.DarkGray:
                    return "100";
                case ConsoleColor.DarkGreen:
                    return "42";
                case ConsoleColor.DarkMagenta:
                    return "45";
                case ConsoleColor.DarkRed:
                    return "41";
                case ConsoleColor.DarkYellow:
                    return "43";
                case ConsoleColor.Gray:
                    return "107";
                case ConsoleColor.Green:
                    return "102";
                case ConsoleColor.Magenta:
                    return "105";
                case ConsoleColor.Red:
                    return "101";
                case ConsoleColor.White:
                    return "107";
                case ConsoleColor.Yellow:
                    return "103";
                default:
                    break;
            }
            return null;
        }
    }
}
