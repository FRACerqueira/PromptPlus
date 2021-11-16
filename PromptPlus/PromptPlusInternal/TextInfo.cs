// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusInternal
{
    internal class TextInfo : IEquatable<TextInfo>
    {
        public TextInfo(string text, ConsoleColor color, ConsoleColor colorbg)
        {
            Text = text;
            Color = color;
            ColorBg = colorbg;
            Width = text.GetWidth();
            SaveCursor = false;
        }

        public string Text { get; private set; }

        public ConsoleColor Color { get; private set; }

        public int Width { get; private set; }

        public ConsoleColor ColorBg { get; private set; }

        public bool SaveCursor { get; internal set; }

        public bool Equals(TextInfo other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Text == other.Text && Color == other.Color && ColorBg == other.ColorBg;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Text.GetHashCode() * 397) ^ ((int)Color + (int)ColorBg);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TextInfo);
        }
    }
}
