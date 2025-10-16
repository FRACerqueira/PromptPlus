// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core.Colors;
using System;

namespace PromptPlusLibrary.Core.Ansi
{
    internal static class AnsiColorBuilder
    {
        public static byte[] GetAnsiCodes(ColorSystem system, Color color, bool foreground)
        {
            return system switch
            {
                ColorSystem.NoColors => [], // No colors
                ColorSystem.TrueColor => GetTrueColor(color, foreground), // 24-bit
                ColorSystem.EightBit => GetEightBit(color, foreground), // 8-bit
                ColorSystem.Standard => GetFourBit(color, foreground), // 4-bit
                ColorSystem.Legacy => GetThreeBit(color, foreground), // 3-bit
                _ => throw new NotImplementedException("Could not determine ANSI color."),
            };
        }

        private static byte[] GetThreeBit(Color color, bool foreground)
        {
            byte? number = color.Number;
            if (number == null || color.Number >= 8)
            {
                number = ColorPalette.ExactOrClosest(ColorSystem.Legacy, color).Number;
            }
            int mod = foreground ? 30 : 40;
            return [(byte)(number!.Value + mod)];
        }

        private static byte[] GetFourBit(Color color, bool foreground)
        {
            byte? number = color.Number;
            if (number == null || color.Number >= 16)
            {
                number = ColorPalette.ExactOrClosest(ColorSystem.Standard, color).Number;
            }
            int mod = number < 8 ? foreground ? 30 : 40 : foreground ? 82 : 92;
            return [(byte)(number!.Value + mod)];
        }

        private static byte[] GetEightBit(Color color, bool foreground)
        {
            byte? number = color.Number ?? ColorPalette.ExactOrClosest(ColorSystem.EightBit, color).Number!;
            byte mod = foreground ? (byte)38 : (byte)48;
            return [mod, 5, (byte)number];
        }

        private static byte[] GetTrueColor(Color color, bool foreground)
        {
            if (color.Number != null)
            {
                return GetEightBit(color, foreground);
            }

            byte mod = foreground ? (byte)38 : (byte)48;
            return [mod, 2, color.R, color.G, color.B];
        }
    }
}
