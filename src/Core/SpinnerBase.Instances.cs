// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Core
{
    // Arrow and motion-based spinner implementations for Spinner.
    // The abstract contract lives in SpinnerBase.cs and the public catalog in SpinnerInstance.Known.cs.
    internal abstract partial class SpinnerBase
    {
        private sealed class DefaultSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⣷",
                "⣯",
                "⣟",
                "⡿",
                "⢿",
                "⣻",
                "⣽",
                "⣾",
            ];
        }
        private sealed class AsciiSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "-",
                "\\",
                "|",
                "/",
                "-",
                "\\",
                "|",
                "/",
            ];
        }
        private sealed class DotsSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠋",
                "⠙",
                "⠹",
                "⠸",
                "⠼",
                "⠴",
                "⠦",
                "⠧",
                "⠇",
                "⠏",
            ];
        }
        private sealed class Dots2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⣾",
                "⣽",
                "⣻",
                "⢿",
                "⡿",
                "⣟",
                "⣯",
                "⣷",
            ];
        }
        private sealed class Dots3Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠋",
                "⠙",
                "⠚",
                "⠞",
                "⠖",
                "⠦",
                "⠴",
                "⠲",
                "⠳",
                "⠓",
            ];
        }
        private sealed class Dots4Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠄",
                "⠆",
                "⠇",
                "⠋",
                "⠙",
                "⠸",
                "⠰",
                "⠠",
                "⠰",
                "⠸",
                "⠙",
                "⠋",
                "⠇",
                "⠆",
            ];
        }
        private sealed class Dots5Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠋",
                "⠙",
                "⠚",
                "⠒",
                "⠂",
                "⠂",
                "⠒",
                "⠲",
                "⠴",
                "⠦",
                "⠖",
                "⠒",
                "⠐",
                "⠐",
                "⠒",
                "⠓",
                "⠋",
            ];
        }
        private sealed class Dots6Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠁",
                "⠉",
                "⠙",
                "⠚",
                "⠒",
                "⠂",
                "⠂",
                "⠒",
                "⠲",
                "⠴",
                "⠤",
                "⠄",
                "⠄",
                "⠤",
                "⠴",
                "⠲",
                "⠒",
                "⠂",
                "⠂",
                "⠒",
                "⠚",
                "⠙",
                "⠉",
                "⠁",
            ];
        }
        private sealed class Dots7Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠈",
                "⠉",
                "⠋",
                "⠓",
                "⠒",
                "⠐",
                "⠐",
                "⠒",
                "⠖",
                "⠦",
                "⠤",
                "⠠",
                "⠠",
                "⠤",
                "⠦",
                "⠖",
                "⠒",
                "⠐",
                "⠐",
                "⠒",
                "⠓",
                "⠋",
                "⠉",
                "⠈",
            ];
        }
        private sealed class Dots8Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠁",
                "⠁",
                "⠉",
                "⠙",
                "⠚",
                "⠒",
                "⠂",
                "⠂",
                "⠒",
                "⠲",
                "⠴",
                "⠤",
                "⠄",
                "⠄",
                "⠤",
                "⠠",
                "⠠",
                "⠤",
                "⠦",
                "⠖",
                "⠒",
                "⠐",
                "⠐",
                "⠒",
                "⠓",
                "⠋",
                "⠉",
                "⠈",
                "⠈",
            ];
        }
        private sealed class Dots9Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⢹",
                "⢺",
                "⢼",
                "⣸",
                "⣇",
                "⡧",
                "⡗",
                "⡏",
            ];
        }
        private sealed class Dots10Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⢄",
                "⢂",
                "⢁",
                "⡁",
                "⡈",
                "⡐",
                "⡠",
            ];
        }
        private sealed class Dots11Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠁",
                "⠂",
                "⠄",
                "⡀",
                "⢀",
                "⠠",
                "⠐",
                "⠈",
            ];
        }
        private sealed class Dots12Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⢀⠀",
                "⡀⠀",
                "⠄⠀",
                "⢂⠀",
                "⡂⠀",
                "⠅⠀",
                "⢃⠀",
                "⡃⠀",
                "⠍⠀",
                "⢋⠀",
                "⡋⠀",
                "⠍⠁",
                "⢋⠁",
                "⡋⠁",
                "⠍⠉",
                "⠋⠉",
                "⠋⠉",
                "⠉⠙",
                "⠉⠙",
                "⠉⠩",
                "⠈⢙",
                "⠈⡙",
                "⢈⠩",
                "⡀⢙",
                "⠄⡙",
                "⢂⠩",
                "⡂⢘",
                "⠅⡘",
                "⢃⠨",
                "⡃⢐",
                "⠍⡐",
                "⢋⠠",
                "⡋⢀",
                "⠍⡁",
                "⢋⠁",
                "⡋⠁",
                "⠍⠉",
                "⠋⠉",
                "⠋⠉",
                "⠉⠙",
                "⠉⠙",
                "⠉⠩",
                "⠈⢙",
                "⠈⡙",
                "⠈⠩",
                "⠀⢙",
                "⠀⡙",
                "⠀⠩",
                "⠀⢘",
                "⠀⡘",
                "⠀⠨",
                "⠀⢐",
                "⠀⡐",
                "⠀⠠",
                "⠀⢀",
                "⠀⡀",
            ];
        }
        private sealed class Dots13Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⣼",
                "⣹",
                "⢻",
                "⠿",
                "⡟",
                "⣏",
                "⣧",
                "⣶",
            ];
        }
        private sealed class Dots14Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠉⠉",
                "⠈⠙",
                "⠀⠹",
                "⠀⢸",
                "⠀⣰",
                "⢀⣠",
                "⣀⣀",
                "⣄⡀",
                "⣆⠀",
                "⡇⠀",
                "⠏⠀",
                "⠋⠁",
            ];
        }
        private sealed class Dots8BitSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠀",
                "⠁",
                "⠂",
                "⠃",
                "⠄",
                "⠅",
                "⠆",
                "⠇",
                "⡀",
                "⡁",
                "⡂",
                "⡃",
                "⡄",
                "⡅",
                "⡆",
                "⡇",
                "⠈",
                "⠉",
                "⠊",
                "⠋",
                "⠌",
                "⠍",
                "⠎",
                "⠏",
                "⡈",
                "⡉",
                "⡊",
                "⡋",
                "⡌",
                "⡍",
                "⡎",
                "⡏",
                "⠐",
                "⠑",
                "⠒",
                "⠓",
                "⠔",
                "⠕",
                "⠖",
                "⠗",
                "⡐",
                "⡑",
                "⡒",
                "⡓",
                "⡔",
                "⡕",
                "⡖",
                "⡗",
                "⠘",
                "⠙",
                "⠚",
                "⠛",
                "⠜",
                "⠝",
                "⠞",
                "⠟",
                "⡘",
                "⡙",
                "⡚",
                "⡛",
                "⡜",
                "⡝",
                "⡞",
                "⡟",
                "⠠",
                "⠡",
                "⠢",
                "⠣",
                "⠤",
                "⠥",
                "⠦",
                "⠧",
                "⡠",
                "⡡",
                "⡢",
                "⡣",
                "⡤",
                "⡥",
                "⡦",
                "⡧",
                "⠨",
                "⠩",
                "⠪",
                "⠫",
                "⠬",
                "⠭",
                "⠮",
                "⠯",
                "⡨",
                "⡩",
                "⡪",
                "⡫",
                "⡬",
                "⡭",
                "⡮",
                "⡯",
                "⠰",
                "⠱",
                "⠲",
                "⠳",
                "⠴",
                "⠵",
                "⠶",
                "⠷",
                "⡰",
                "⡱",
                "⡲",
                "⡳",
                "⡴",
                "⡵",
                "⡶",
                "⡷",
                "⠸",
                "⠹",
                "⠺",
                "⠻",
                "⠼",
                "⠽",
                "⠾",
                "⠿",
                "⡸",
                "⡹",
                "⡺",
                "⡻",
                "⡼",
                "⡽",
                "⡾",
                "⡿",
                "⢀",
                "⢁",
                "⢂",
                "⢃",
                "⢄",
                "⢅",
                "⢆",
                "⢇",
                "⣀",
                "⣁",
                "⣂",
                "⣃",
                "⣄",
                "⣅",
                "⣆",
                "⣇",
                "⢈",
                "⢉",
                "⢊",
                "⢋",
                "⢌",
                "⢍",
                "⢎",
                "⢏",
                "⣈",
                "⣉",
                "⣊",
                "⣋",
                "⣌",
                "⣍",
                "⣎",
                "⣏",
                "⢐",
                "⢑",
                "⢒",
                "⢓",
                "⢔",
                "⢕",
                "⢖",
                "⢗",
                "⣐",
                "⣑",
                "⣒",
                "⣓",
                "⣔",
                "⣕",
                "⣖",
                "⣗",
                "⢘",
                "⢙",
                "⢚",
                "⢛",
                "⢜",
                "⢝",
                "⢞",
                "⢟",
                "⣘",
                "⣙",
                "⣚",
                "⣛",
                "⣜",
                "⣝",
                "⣞",
                "⣟",
                "⢠",
                "⢡",
                "⢢",
                "⢣",
                "⢤",
                "⢥",
                "⢦",
                "⢧",
                "⣠",
                "⣡",
                "⣢",
                "⣣",
                "⣤",
                "⣥",
                "⣦",
                "⣧",
                "⢨",
                "⢩",
                "⢪",
                "⢫",
                "⢬",
                "⢭",
                "⢮",
                "⢯",
                "⣨",
                "⣩",
                "⣪",
                "⣫",
                "⣬",
                "⣭",
                "⣮",
                "⣯",
                "⢰",
                "⢱",
                "⢲",
                "⢳",
                "⢴",
                "⢵",
                "⢶",
                "⢷",
                "⣰",
                "⣱",
                "⣲",
                "⣳",
                "⣴",
                "⣵",
                "⣶",
                "⣷",
                "⢸",
                "⢹",
                "⢺",
                "⢻",
                "⢼",
                "⢽",
                "⢾",
                "⢿",
                "⣸",
                "⣹",
                "⣺",
                "⣻",
                "⣼",
                "⣽",
                "⣾",
                "⣿",
            ];
        }
        private sealed class DotsCircleSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⢎ ",
                "⠎⠁",
                "⠊⠑",
                "⠈⠱",
                " ⡱",
                "⢀⡰",
                "⢄⡠",
                "⢆⡀",
            ];
        }
        private sealed class SandSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠁",
                "⠂",
                "⠄",
                "⡀",
                "⡈",
                "⡐",
                "⡠",
                "⣀",
                "⣁",
                "⣂",
                "⣄",
                "⣌",
                "⣔",
                "⣤",
                "⣥",
                "⣦",
                "⣮",
                "⣶",
                "⣷",
                "⣿",
                "⡿",
                "⠿",
                "⢟",
                "⠟",
                "⡛",
                "⠛",
                "⠫",
                "⢋",
                "⠋",
                "⠍",
                "⡉",
                "⠉",
                "⠑",
                "⠡",
                "⢁",
            ];
        }
        private sealed class LineSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(130);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "-",
                "\\",
                "|",
                "/",
            ];
        }
        private sealed class Line2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "⠂",
                "-",
                "–",
                "—",
                "–",
                "-",
            ];
        }
        private sealed class PipeSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "┤",
                "┘",
                "┴",
                "└",
                "├",
                "┌",
                "┬",
                "┐",
            ];
        }
        private sealed class SimpleDotsSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(400);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                ".  ",
                ".. ",
                "...",
                "   ",
            ];
        }
        private sealed class SimpleDotsScrollingSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(200);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                ".  ",
                ".. ",
                "...",
                " ..",
                "  .",
                "   ",
            ];
        }
        private sealed class StarSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(70);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "✶",
                "✸",
                "✹",
                "✺",
                "✹",
                "✷",
            ];
        }
        private sealed class Star2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "+",
                "x",
                "*",
            ];
        }
        private sealed class FlipSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(70);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
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
            ];
        }
        private sealed class HamburgerSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "☱",
                "☲",
                "☴",
            ];
        }
        private sealed class GrowVerticalSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▁",
                "▃",
                "▄",
                "▅",
                "▆",
                "▇",
                "▆",
                "▅",
                "▄",
                "▃",
            ];
        }
        private sealed class GrowHorizontalSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▏",
                "▎",
                "▍",
                "▌",
                "▋",
                "▊",
                "▉",
                "▊",
                "▋",
                "▌",
                "▍",
                "▎",
            ];
        }
        private sealed class BalloonSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(140);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                " ",
                ".",
                "o",
                "O",
                "@",
                "*",
                " ",
            ];
        }
        private sealed class Balloon2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                ".",
                "o",
                "O",
                "°",
                "O",
                "o",
                ".",
            ];
        }
        private sealed class NoiseSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▓",
                "▒",
                "░",
            ];
        }
        private sealed class BounceSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⠁",
                "⠂",
                "⠄",
                "⠂",
            ];
        }
        private sealed class BoxBounceSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▖",
                "▘",
                "▝",
                "▗",
            ];
        }
        private sealed class BoxBounce2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▌",
                "▀",
                "▐",
                "▄",
            ];
        }
        private sealed class TriangleSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◢",
                "◣",
                "◤",
                "◥",
            ];
        }
        private sealed class BinarySpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "010010",
                "001100",
                "100101",
                "111010",
                "111101",
                "010111",
                "101011",
                "111000",
                "110011",
                "110101",
            ];
        }
        private sealed class ArcSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◜",
                "◠",
                "◝",
                "◞",
                "◡",
                "◟",
            ];
        }
        private sealed class CircleSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◡",
                "⊙",
                "◠",
            ];
        }
        private sealed class SquareCornersSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(180);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◰",
                "◳",
                "◲",
                "◱",
            ];
        }
        private sealed class CircleQuartersSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◴",
                "◷",
                "◶",
                "◵",
            ];
        }
        private sealed class CircleHalvesSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◐",
                "◓",
                "◑",
                "◒",
            ];
        }
        private sealed class SquishSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "╫",
                "╪",
            ];
        }
        private sealed class ToggleSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(250);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⊶",
                "⊷",
            ];
        }
        private sealed class Toggle2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▫",
                "▪",
            ];
        }
        private sealed class Toggle3Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "□",
                "■",
            ];
        }
        private sealed class Toggle4Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "■",
                "□",
                "▪",
                "▫",
            ];
        }
        private sealed class Toggle5Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▮",
                "▯",
            ];
        }
        private sealed class Toggle6Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(300);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "ဝ",
                "၀",
            ];
        }
        private sealed class Toggle7Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⦾",
                "⦿",
            ];
        }
        private sealed class Toggle8Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◍",
                "◌",
            ];
        }
        private sealed class Toggle9Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "◉",
                "◎",
            ];
        }
        private sealed class Toggle10Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "㊂",
                "㊀",
                "㊁",
            ];
        }
        private sealed class Toggle11Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⧇",
                "⧆",
            ];
        }
        private sealed class Toggle12Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "☗",
                "☖",
            ];
        }
        private sealed class Toggle13Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "=",
                "*",
                "-",
            ];
        }
        private sealed class ArrowSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "←",
                "↖",
                "↑",
                "↗",
                "→",
                "↘",
                "↓",
                "↙",
            ];
        }
        private sealed class Arrow2Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "⬆️ ",
                "↗️ ",
                "➡️ ",
                "↘️ ",
                "⬇️ ",
                "↙️ ",
                "⬅️ ",
                "↖️ ",
            ];
        }
        private sealed class Arrow3Spinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▹▹▹▹▹",
                "▸▹▹▹▹",
                "▹▸▹▹▹",
                "▹▹▸▹▹",
                "▹▹▹▸▹",
                "▹▹▹▹▸",
            ];
        }
        private sealed class BouncingBarSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "[    ]",
                "[=   ]",
                "[==  ]",
                "[=== ]",
                "[====]",
                "[ ===]",
                "[  ==]",
                "[   =]",
                "[    ]",
                "[   =]",
                "[  ==]",
                "[ ===]",
                "[====]",
                "[=== ]",
                "[==  ]",
                "[=   ]",
            ];
        }
        private sealed class BouncingBallSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "( ●    )",
                "(  ●   )",
                "(   ●  )",
                "(    ● )",
                "(     ●)",
                "(    ● )",
                "(   ●  )",
                "(  ●   )",
                "( ●    )",
                "(●     )",
            ];
        }
        private sealed class SmileySpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(200);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "😄 ",
                "😝 ",
            ];
        }
        private sealed class MonkeySpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(300);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🙈 ",
                "🙈 ",
                "🙉 ",
                "🙊 ",
            ];
        }
        private sealed class HeartsSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "💛 ",
                "💙 ",
                "💜 ",
                "💚 ",
                "❤️ ",
            ];
        }
        private sealed class ClockSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🕛 ",
                "🕐 ",
                "🕑 ",
                "🕒 ",
                "🕓 ",
                "🕔 ",
                "🕕 ",
                "🕖 ",
                "🕗 ",
                "🕘 ",
                "🕙 ",
                "🕚 ",
            ];
        }
        private sealed class EarthSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(180);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🌍 ",
                "🌎 ",
                "🌏 ",
            ];
        }
        private sealed class MaterialSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(17);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "███▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "████▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "███████▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "████████▁▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "██████████▁▁▁▁▁▁▁▁▁▁",
                "███████████▁▁▁▁▁▁▁▁▁",
                "█████████████▁▁▁▁▁▁▁",
                "██████████████▁▁▁▁▁▁",
                "██████████████▁▁▁▁▁▁",
                "▁██████████████▁▁▁▁▁",
                "▁██████████████▁▁▁▁▁",
                "▁██████████████▁▁▁▁▁",
                "▁▁██████████████▁▁▁▁",
                "▁▁▁██████████████▁▁▁",
                "▁▁▁▁█████████████▁▁▁",
                "▁▁▁▁██████████████▁▁",
                "▁▁▁▁██████████████▁▁",
                "▁▁▁▁▁██████████████▁",
                "▁▁▁▁▁██████████████▁",
                "▁▁▁▁▁██████████████▁",
                "▁▁▁▁▁▁██████████████",
                "▁▁▁▁▁▁██████████████",
                "▁▁▁▁▁▁▁█████████████",
                "▁▁▁▁▁▁▁█████████████",
                "▁▁▁▁▁▁▁▁████████████",
                "▁▁▁▁▁▁▁▁████████████",
                "▁▁▁▁▁▁▁▁▁███████████",
                "▁▁▁▁▁▁▁▁▁███████████",
                "▁▁▁▁▁▁▁▁▁▁██████████",
                "▁▁▁▁▁▁▁▁▁▁██████████",
                "▁▁▁▁▁▁▁▁▁▁▁▁████████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁██████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                "█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                "███▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                "████▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                "█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "██████▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "████████▁▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "█████████▁▁▁▁▁▁▁▁▁▁▁",
                "███████████▁▁▁▁▁▁▁▁▁",
                "████████████▁▁▁▁▁▁▁▁",
                "████████████▁▁▁▁▁▁▁▁",
                "██████████████▁▁▁▁▁▁",
                "██████████████▁▁▁▁▁▁",
                "▁██████████████▁▁▁▁▁",
                "▁██████████████▁▁▁▁▁",
                "▁▁▁█████████████▁▁▁▁",
                "▁▁▁▁▁████████████▁▁▁",
                "▁▁▁▁▁████████████▁▁▁",
                "▁▁▁▁▁▁███████████▁▁▁",
                "▁▁▁▁▁▁▁▁█████████▁▁▁",
                "▁▁▁▁▁▁▁▁█████████▁▁▁",
                "▁▁▁▁▁▁▁▁▁█████████▁▁",
                "▁▁▁▁▁▁▁▁▁█████████▁▁",
                "▁▁▁▁▁▁▁▁▁▁█████████▁",
                "▁▁▁▁▁▁▁▁▁▁▁████████▁",
                "▁▁▁▁▁▁▁▁▁▁▁████████▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            ];
        }
        private sealed class MoonSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🌑 ",
                "🌒 ",
                "🌓 ",
                "🌔 ",
                "🌕 ",
                "🌖 ",
                "🌗 ",
                "🌘 ",
            ];
        }
        private sealed class RunnerSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(140);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🚶 ",
                "🏃 ",
            ];
        }
        private sealed class PongSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▐⠂       ▌",
                "▐⠈       ▌",
                "▐ ⠂      ▌",
                "▐ ⠠      ▌",
                "▐  ⡀     ▌",
                "▐  ⠠     ▌",
                "▐   ⠂    ▌",
                "▐   ⠈    ▌",
                "▐    ⠂   ▌",
                "▐    ⠠   ▌",
                "▐     ⡀  ▌",
                "▐     ⠠  ▌",
                "▐      ⠂ ▌",
                "▐      ⠈ ▌",
                "▐       ⠂▌",
                "▐       ⠠▌",
                "▐       ⡀▌",
                "▐      ⠠ ▌",
                "▐      ⠂ ▌",
                "▐     ⠈  ▌",
                "▐     ⠂  ▌",
                "▐    ⠠   ▌",
                "▐    ⡀   ▌",
                "▐   ⠠    ▌",
                "▐   ⠂    ▌",
                "▐  ⠈     ▌",
                "▐  ⠂     ▌",
                "▐ ⠠      ▌",
                "▐ ⡀      ▌",
                "▐⠠       ▌",
            ];
        }
        private sealed class SharkSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▐|\\____________▌",
                "▐_|\\___________▌",
                "▐__|\\__________▌",
                "▐___|\\_________▌",
                "▐____|\\________▌",
                "▐_____|\\_______▌",
                "▐______|\\______▌",
                "▐_______|\\_____▌",
                "▐________|\\____▌",
                "▐_________|\\___▌",
                "▐__________|\\__▌",
                "▐___________|\\_▌",
                "▐____________|\\▌",
                "▐____________/|▌",
                "▐___________/|_▌",
                "▐__________/|__▌",
                "▐_________/|___▌",
                "▐________/|____▌",
                "▐_______/|_____▌",
                "▐______/|______▌",
                "▐_____/|_______▌",
                "▐____/|________▌",
                "▐___/|_________▌",
                "▐__/|__________▌",
                "▐_/|___________▌",
                "▐/|____________▌",
            ];
        }
        private sealed class DqpbSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames =>
            [
                "d",
                "q",
                "p",
                "b",
            ];
        }
        private sealed class WeatherSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "☀️ ",
                "☀️ ",
                "☀️ ",
                "🌤 ",
                "⛅️ ",
                "🌥 ",
                "☁️ ",
                "🌧 ",
                "🌨 ",
                "🌧 ",
                "🌨 ",
                "🌧 ",
                "🌨 ",
                "⛈ ",
                "🌨 ",
                "🌧 ",
                "🌨 ",
                "☁️ ",
                "🌥 ",
                "⛅️ ",
                "🌤 ",
                "☀️ ",
                "☀️ ",
            ];
        }
        private sealed class ChristmasSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(400);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🌲",
                "🎄",
            ];
        }
        private sealed class GrenadeSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "،  ",
                "′  ",
                " ´ ",
                " ‾ ",
                "  ⸌",
                "  ⸊",
                "  |",
                "  ⁎",
                "  ⁕",
                " ෴ ",
                "  ⁓",
                "   ",
                "   ",
                "   ",
            ];
        }
        private sealed class PointSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(125);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "∙∙∙",
                "●∙∙",
                "∙●∙",
                "∙∙●",
                "∙∙∙",
            ];
        }
        private sealed class LayerSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(150);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "-",
                "=",
                "≡",
            ];
        }
        private sealed class BetaWaveSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "ρββββββ",
                "βρβββββ",
                "ββρββββ",
                "βββρβββ",
                "ββββρββ",
                "βββββρβ",
                "ββββββρ",
            ];
        }
        private sealed class FingerDanceSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🤘 ",
                "🤟 ",
                "🖖 ",
                "✋ ",
                "🤚 ",
                "👆 ",
            ];
        }
        private sealed class FistBumpSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🤜　　　　🤛 ",
                "🤜　　　　🤛 ",
                "🤜　　　　🤛 ",
                "　🤜　　🤛　 ",
                "　　🤜🤛　　 ",
                "　🤜✨🤛　　 ",
                "🤜　✨　🤛　 ",
            ];
        }
        private sealed class SoccerHeaderSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                " 🧑⚽️       🧑 ",
                "🧑  ⚽️      🧑 ",
                "🧑   ⚽️     🧑 ",
                "🧑    ⚽️    🧑 ",
                "🧑     ⚽️   🧑 ",
                "🧑      ⚽️  🧑 ",
                "🧑       ⚽️🧑  ",
                "🧑      ⚽️  🧑 ",
                "🧑     ⚽️   🧑 ",
                "🧑    ⚽️    🧑 ",
                "🧑   ⚽️     🧑 ",
                "🧑  ⚽️      🧑 ",
            ];
        }
        private sealed class MindblownSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "😐 ",
                "😐 ",
                "😮 ",
                "😮 ",
                "😦 ",
                "😦 ",
                "😧 ",
                "😧 ",
                "🤯 ",
                "💥 ",
                "✨ ",
                "　 ",
                "　 ",
                "　 ",
            ];
        }
        private sealed class SpeakerSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🔈 ",
                "🔉 ",
                "🔊 ",
                "🔉 ",
            ];
        }
        private sealed class OrangePulseSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🔸 ",
                "🔶 ",
                "🟠 ",
                "🟠 ",
                "🔶 ",
            ];
        }
        private sealed class BluePulseSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🔹 ",
                "🔷 ",
                "🔵 ",
                "🔵 ",
                "🔷 ",
            ];
        }
        private sealed class OrangeBluePulseSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🔸 ",
                "🔶 ",
                "🟠 ",
                "🟠 ",
                "🔶 ",
                "🔹 ",
                "🔷 ",
                "🔵 ",
                "🔵 ",
                "🔷 ",
            ];
        }
        private sealed class TimeTravelSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "🕛 ",
                "🕚 ",
                "🕙 ",
                "🕘 ",
                "🕗 ",
                "🕖 ",
                "🕕 ",
                "🕔 ",
                "🕓 ",
                "🕒 ",
                "🕑 ",
                "🕐 ",
            ];
        }
        private sealed class AestheticSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                "▰▱▱▱▱▱▱",
                "▰▰▱▱▱▱▱",
                "▰▰▰▱▱▱▱",
                "▰▰▰▰▱▱▱",
                "▰▰▰▰▰▱▱",
                "▰▰▰▰▰▰▱",
                "▰▰▰▰▰▰▰",
                "▰▱▱▱▱▱▱",
            ];
        }
        private sealed class DwarfFortressSpinner :  SpinnerBase
        {
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);
            public override bool IsUnicode => true;
            public override IReadOnlyList<string> Frames =>
            [
                " ██████£££  ",
                "☺██████£££  ",
                "☺██████£££  ",
                "☺▓█████£££  ",
                "☺▓█████£££  ",
                "☺▒█████£££  ",
                "☺▒█████£££  ",
                "☺░█████£££  ",
                "☺░█████£££  ",
                "☺ █████£££  ",
                " ☺█████£££  ",
                " ☺█████£££  ",
                " ☺▓████£££  ",
                " ☺▓████£££  ",
                " ☺▒████£££  ",
                " ☺▒████£££  ",
                " ☺░████£££  ",
                " ☺░████£££  ",
                " ☺ ████£££  ",
                "  ☺████£££  ",
                "  ☺████£££  ",
                "  ☺▓███£££  ",
                "  ☺▓███£££  ",
                "  ☺▒███£££  ",
                "  ☺▒███£££  ",
                "  ☺░███£££  ",
                "  ☺░███£££  ",
                "  ☺ ███£££  ",
                "   ☺███£££  ",
                "   ☺███£££  ",
                "   ☺▓██£££  ",
                "   ☺▓██£££  ",
                "   ☺▒██£££  ",
                "   ☺▒██£££  ",
                "   ☺░██£££  ",
                "   ☺░██£££  ",
                "   ☺ ██£££  ",
                "    ☺██£££  ",
                "    ☺██£££  ",
                "    ☺▓█£££  ",
                "    ☺▓█£££  ",
                "    ☺▒█£££  ",
                "    ☺▒█£££  ",
                "    ☺░█£££  ",
                "    ☺░█£££  ",
                "    ☺ █£££  ",
                "     ☺█£££  ",
                "     ☺█£££  ",
                "     ☺▓£££  ",
                "     ☺▓£££  ",
                "     ☺▒£££  ",
                "     ☺▒£££  ",
                "     ☺░£££  ",
                "     ☺░£££  ",
                "     ☺ £££  ",
                "      ☺£££  ",
                "      ☺£££  ",
                "      ☺▓££  ",
                "      ☺▓££  ",
                "      ☺▒££  ",
                "      ☺▒££  ",
                "      ☺░££  ",
                "      ☺░££  ",
                "      ☺ ££  ",
                "       ☺££  ",
                "       ☺££  ",
                "       ☺▓£  ",
                "       ☺▓£  ",
                "       ☺▒£  ",
                "       ☺▒£  ",
                "       ☺░£  ",
                "       ☺░£  ",
                "       ☺ £  ",
                "        ☺£  ",
                "        ☺£  ",
                "        ☺▓  ",
                "        ☺▓  ",
                "        ☺▒  ",
                "        ☺▒  ",
                "        ☺░  ",
                "        ☺░  ",
                "        ☺   ",
                "        ☺  &",
                "        ☺ ☼&",
                "       ☺ ☼ &",
                "       ☺☼  &",
                "      ☺☼  & ",
                "      ‼   & ",
                "     ☺   &  ",
                "    ‼    &  ",
                "   ☺    &   ",
                "  ‼     &   ",
                " ☺     &    ",
                "‼      &    ",
                "      &     ",
                "      &     ",
                "     &   ░  ",
                "     &   ▒  ",
                "    &    ▓  ",
                "    &    £  ",
                "   &    ░£  ",
                "   &    ▒£  ",
                "  &     ▓£  ",
                "  &     ££  ",
                " &     ░££  ",
                " &     ▒££  ",
                "&      ▓££  ",
                "&      £££  ",
                "      ░£££  ",
                "      ▒£££  ",
                "      ▓£££  ",
                "      █£££  ",
                "     ░█£££  ",
                "     ▒█£££  ",
                "     ▓█£££  ",
                "     ██£££  ",
                "    ░██£££  ",
                "    ▒██£££  ",
                "    ▓██£££  ",
                "    ███£££  ",
                "   ░███£££  ",
                "   ▒███£££  ",
                "   ▓███£££  ",
                "   ████£££  ",
                "  ░████£££  ",
                "  ▒████£££  ",
                "  ▓████£££  ",
                "  █████£££  ",
                " ░█████£££  ",
                " ▒█████£££  ",
                " ▓█████£££  ",
                " ██████£££  ",
                " ██████£££  ",
            ];
        }
    }
}
