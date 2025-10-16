// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PromptPlusLibrary.Core.Colors
{
    internal static class ColorTable
    {
        private static readonly ImmutableDictionary<int, string> _nameLookup;
        private static readonly ImmutableDictionary<string, int> _numberLookup;

        static ColorTable()
        {
            _numberLookup = ImmutableDictionary.CreateRange(GenerateTable());
            Dictionary<int, string> aux = [];
            foreach (KeyValuePair<string, int> pair in _numberLookup)
            {
                aux.TryAdd(pair.Value, pair.Key);
            }
            _nameLookup = ImmutableDictionary.CreateRange(aux);
        }

        public static Color GetColorRGB(int number)
        {
            if (number < 0 || number > 255)
            {
                throw new ArgumentException("ColorRGB number must be between 0 and 255");
            }

            return ColorPalette.EightBit[number];
        }

        public static Color? GetColorRGB(string name)
        {
            if (!_numberLookup.TryGetValue(name.ToLowerInvariant(), out int number))
            {
                return null;
            }

            if (number > ColorPalette.EightBit.Count - 1)
            {
                return null;
            }

            return ColorPalette.EightBit[number];
        }

        public static string? GetName(int number)
        {
            _nameLookup.TryGetValue(number, out string? name);
            return name;
        }

        private static Dictionary<string, int> GenerateTable()
        {
            return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "black", 0 },
                { "maroon", 1 },
                { "green", 2 },
                { "olive", 3 },
                { "navy", 4 },
                { "purple", 5 },
                { "teal", 6 },
                { "silver", 7 },
                { "grey", 8 },
                { "gray", 8 },
                { "red", 9 },
                { "lime", 10 },
                { "yellow", 11 },
                { "blue", 12 },
                { "fuchsia", 13 },
                { "magenta", 13 },
                { "aqua", 14 },
                { "cyan", 14 },
                { "white", 15 }
            };
        }
    }
}
