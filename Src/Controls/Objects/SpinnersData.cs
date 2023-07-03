using System.Collections.Generic;

namespace PPlus.Controls.Objects
{
    internal static class SpinnersData
    {
        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) FallBackSpinner =
            (new List<string>
            {
                    "-",
                    "\\",
                    "|",
                    "/",
                    "-",
                    "\\",
                    "|",
                    "/",
            }, false, 50);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) AsciiSpinner =
            (new List<string>
            {
                    "-",
                    "\\",
                    "|",
                    "/",
                    "-",
                    "\\",
                    "|",
                    "/",
            }, false, 50);


        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Dots=
            (new List<string>
            {
                    ".  ",
                    ".. ",
                    "...",
                    "   ",
            }, false, 180);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotsScrolling =
            (new List<string>
            {
                    ".  ",
                    ".. ",
                    "...",
                    " ..",
                    "  .",
                    "   ",
            }, false, 200);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Star =
            (new List<string>
            {
                    "+",
                    "x",
                    "*",
            }, false, 80);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Flip =
            (new List<string>
            {
                    "_",
                    "_",
                    "_",
                    "-",
                    "`",
                    "`",
                    "'",
                    "´",
                    "-",
                    "_",
                    "_",
                    "_",
            }, false, 80);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Balloon =
            (new List<string>
            {
                    ".",
                    "o",
                    "O",
                    "°",
                    "O",
                    "o",
            }, true, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Noise =
            (new List<string>
            {
                    "▓",
                    "▒",
                    "░",
            }, true, 100);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Bounce =
            (new List<string>
            {
                "o     ",
                " o    ",
                "  o   ",
                "   o  ",
                "    o ",
                "     o",
                "    o ",
                "   o  ",
                "  o   ",
                " o    ",
                "o     "
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) BoxHeavy =
            (new List<string>
            {
                    "▌",
                    "▀",
                    "▐",
                    "▄",
            }, true, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotArrow =
            (new List<string>
            {
                    ">....",
                    ".>...",
                    "..>..",
                    "...>.",
                    "....>",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Arrow =
            (new List<string>
            {
                    ">    ",
                    ">>   ",
                    ">>>  ",
                    ">>>> ",
                    ">>>>>",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotArrowHeavy =
            (new List<string>
            {
                    "►....",
                    ".►...",
                    "..►..",
                    "...►.",
                    "....►",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) ArrowHeavy =
            (new List<string>
            {
                    "►    ",
                    "►►   ",
                    "►►►  ",
                    "►►►► ",
                    "►►►►►",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DoubleArrow =
            (new List<string>
            {
                    "»    ",
                    "»»   ",
                    "»»»  ",
                    "»»»» ",
                    "»»»»»",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) RightArrow =
            (new List<string>
            {
                    "→    ",
                    "→→   ",
                    "→→→  ",
                    "→→→→ ",
                    "→→→→→",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) LeftArrow =
            (new List<string>
            {
                    "    ←",
                    "   ←←",
                    "  ←←←",
                    " ←←←←",
                    "←←←←←",
            }, false, 120);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) BouncingBar =
            (new List<string>
            {
                    "    ",
                    "=   ",
                    "==  ",
                    "=== ",
                    " ===",
                    "  ==",
                    "   =",
                    "    ",
                    "   =",
                    "  ==",
                    " ===",
                    "====",
                    "=== ",
                    "==  ",
                    "=   "
            }, false, 80);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Pipe =
            (new List<string>
            {
                    "┤",
                    "┘",
                    "┴",
                    "└",
                    "├",
                    "┌",
                    "┬",
                    "┐"
            }, true, 80);

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Toggle =
            (new List<string>
            {
                    "=",
                    "*",
                    "-"
            }, false, 80);

    }

}
