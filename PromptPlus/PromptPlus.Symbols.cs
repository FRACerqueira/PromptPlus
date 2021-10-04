// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static class Symbols
        {
            public static Symbol MaskEmpty { get; set; } = new Symbol("■", "  ");
            public static Symbol File { get; set; } = new Symbol("■", "- ");
            public static Symbol Folder { get; set; } = new Symbol("►", "> ");
            public static Symbol Prompt { get; set; } = new Symbol("→", "->");
            public static Symbol Done { get; set; } = new Symbol("√", "V ");
            public static Symbol Error { get; set; } = new Symbol("»", ">>");
            public static Symbol Selector { get; set; } = new Symbol("›", "> ");
            public static Symbol Selected { get; set; } = new Symbol("♦", "* ");
            public static Symbol NotSelect { get; set; } = new Symbol("○", "  ");
            public static Symbol TaskRun { get; set; } = new Symbol("♦", "* ");
            public static Symbol Skiped { get; set; } = new Symbol("×", "x ");
        }
    }
}
