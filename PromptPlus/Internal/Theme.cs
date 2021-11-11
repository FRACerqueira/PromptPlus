// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusControls.ValueObjects
{
    internal class Theme
    {
        public Theme()
        {
            HotKeys = new();
            Colors = new();
            Symbols = new();
        }

        public string Culture { get; set; }

        public bool EnabledBeep { get; set; }

        public bool EnabledStandardTooltip { get; set; }

        public bool EnabledPromptTooltip { get; set; }

        public bool EnabledAbortKey { get; set; }

        public bool EnabledAbortAllPipes { get; set; }

        public char? PasswordChar { get; set; }

        public ThemeHotKeys HotKeys { get; set; }

        public class ThemeHotKeys
        {
            public string AbortAllPipesKeyPress { get; set; }
            public string AbortKeyPress { get; set; }
            public string TooltipKeyPress { get; set; }
            public string ResumePipesKeyPress { get; set; }
            public string UnSelectFilter { get; set; }
            public string SwitchViewPassword { get; set; }
            public string SelectAll { get; set; }
            public string InvertSelect { get; set; }
            public string RemoveAll { get; set; }
        }

        public ThemeColors Colors { get; set; }

        public class ThemeColors
        {
            public ConsoleColor ForeColorSchema { get; set; }
            public ConsoleColor BackColorSchema { get; set; }
            public ConsoleColor Pagination { get; set; }
            public ConsoleColor Hint { get; set; }
            public ConsoleColor Answer { get; set; }
            public ConsoleColor Select { get; set; }
            public ConsoleColor Disabled { get; set; }
            public ConsoleColor Filter { get; set; }
            public ConsoleColor Error { get; set; }
            public ConsoleColor DoneSymbol { get; set; }
            public ConsoleColor PromptSymbol { get; set; }
            public ConsoleColor SliderBackcolor { get; set; }
            public ConsoleColor SliderForecolor { get; set; }
        }

        public ThemeSymbols Symbols { get; set; }

        public class ThemeSymbols
        {
            public Symbol MaskEmpty { get; set; }
            public Symbol File { get; set; }
            public Symbol Folder { get; set; }
            public Symbol Prompt { get; set; }
            public Symbol Done { get; set; }
            public Symbol Error { get; set; }
            public Symbol Selector { get; set; }
            public Symbol Selected { get; set; }
            public Symbol NotSelect { get; set; }
            public Symbol TaskRun { get; set; }
            public Symbol Skiped { get; set; }
        }
    }
}
