// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
using PromptPlusObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static class Symbols
        {
            public static Symbol MaskEmpty { get; set; }
            public static Symbol File { get; set; }
            public static Symbol Folder { get; set; }
            public static Symbol Prompt { get; set; }
            public static Symbol Done { get; set; }
            public static Symbol Error { get; set; }
            public static Symbol Selector { get; set; }
            public static Symbol Selected { get; set; }
            public static Symbol NotSelect { get; set; }
            public static Symbol TaskRun { get; set; }
            public static Symbol Skiped { get; set; }

            internal static Symbol IndentGroup { get; set; }
            internal static Symbol IndentEndGroup { get; set; }
            internal static Symbol SymbGroup { get; set; }

        }
    }
}
