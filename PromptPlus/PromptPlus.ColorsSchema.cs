// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static class ColorSchema
        {
            public static ConsoleColor Description { get; set; } = ConsoleColor.Cyan;
            public static ConsoleColor Pagination { get; set; } = ConsoleColor.DarkGray;
            public static ConsoleColor Hint { get; set; } = ConsoleColor.DarkGray;
            public static ConsoleColor Answer { get; set; } = ConsoleColor.Cyan;
            public static ConsoleColor Select { get; set; } = ConsoleColor.Green;
            public static ConsoleColor Disabled { get; set; } = ConsoleColor.DarkGray;
            public static ConsoleColor Filter { get; set; } = ConsoleColor.Yellow;
            public static ConsoleColor Error { get; set; } = ConsoleColor.Red;
            public static ConsoleColor DoneSymbol { get; set; } = ConsoleColor.Cyan;
            public static ConsoleColor PromptSymbol { get; set; } = ConsoleColor.Green;
            public static ConsoleColor SliderBackcolor { get; set; } = ConsoleColor.DarkGray;
            public static ConsoleColor SliderForecolor { get; set; } = ConsoleColor.Cyan;

        }
    }
}
