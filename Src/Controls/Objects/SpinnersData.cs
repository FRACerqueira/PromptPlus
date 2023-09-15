// **********************************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// **********************************************************************************************************

using System.Collections.Generic;

namespace PPlus.Controls.Objects
{
    internal static class SpinnersData
    {
        static SpinnersData()
        {
            FallBackSpinner =
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

            AsciiSpinner =
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

            Dots =
            (new List<string>
            {
                    ".  ",
                    ".. ",
                    "...",
                    "   ",
            }, false, 180);

            DotsScrolling =
                (new List<string>
                {
                        ".  ",
                        ".. ",
                        "...",
                        " ..",
                        "  .",
                        "   ",
                }, false, 200);

            Star =
                (new List<string>
                {
                        "+",
                        "x",
                        "*",
                }, false, 80);

            Flip =
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

            Balloon =
                (new List<string>
                {
                        ".",
                        "o",
                        "O",
                        "°",
                        "O",
                        "o",
                }, true, 120);

            Noise =
                (new List<string>
                {
                        "▓",
                        "▒",
                        "░",
                }, true, 100);

            Bounce =
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

            BoxHeavy =
                (new List<string>
                {
                        "▌",
                        "▀",
                        "▐",
                        "▄",
                }, true, 120);

            BoxHeavy =
                (new List<string>
                {
                        "▌",
                        "▀",
                        "▐",
                        "▄",
                }, true, 120);

            DotArrow =
                (new List<string>
                {
                        ">....",
                        ".>...",
                        "..>..",
                        "...>.",
                        "....>",
                }, false, 120);

            Arrow =
                (new List<string>
                {
                        ">    ",
                        ">>   ",
                        ">>>  ",
                        ">>>> ",
                        ">>>>>",
                }, false, 120);

            DotArrowHeavy =
                (new List<string>
                {
                        "►....",
                        ".►...",
                        "..►..",
                        "...►.",
                        "....►",
                }, false, 120);

            ArrowHeavy =
                (new List<string>
                {
                        "►    ",
                        "►►   ",
                        "►►►  ",
                        "►►►► ",
                        "►►►►►",
                }, false, 120);

            DoubleArrow =
                (new List<string>
                {
                        "»    ",
                        "»»   ",
                        "»»»  ",
                        "»»»» ",
                        "»»»»»",
                }, false, 120);

            RightArrow =
                (new List<string>
                {
                        "→    ",
                        "→→   ",
                        "→→→  ",
                        "→→→→ ",
                        "→→→→→",
                }, false, 120);

            LeftArrow =
                (new List<string>
                {
                        "    ←",
                        "   ←←",
                        "  ←←←",
                        " ←←←←",
                        "←←←←←",
                }, false, 120);

            BouncingBar =
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

            Pipe =
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

            Toggle =
                (new List<string>
                {
                        "=",
                        "*",
                        "-"
                }, false, 80);
        }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) FallBackSpinner { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Star { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) AsciiSpinner { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Dots { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotsScrolling { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Flip { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Balloon { get; private set; }
        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Noise { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Bounce { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) BoxHeavy { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotArrow { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Arrow { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DotArrowHeavy { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) ArrowHeavy { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) DoubleArrow { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) RightArrow { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) LeftArrow { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) BouncingBar { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Pipe { get; private set; }

        public static (IReadOnlyList<string> Frames, bool IsUnicode, int Interval) Toggle { get; private set; }

    }

}
