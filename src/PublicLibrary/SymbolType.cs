// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents symbol types used by the UI for status indicators, layout elements, progress bars,
    /// charts, grids, calendars, and input delimiters.
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// Represents a completed action. Default value : ASCII="V" Unicode="√"
        /// </summary>
        Done,

        /// <summary>
        /// Represents an error. Default value : ASCII="!" Unicode="!"
        /// </summary>
        Error,

        /// <summary>
        /// Represents a canceled operation. Default value : ASCII="x" Unicode="x"
        /// </summary>
        Canceled,

        /// <summary>
        /// Represents a selector marker. Default value : ASCII=">" Unicode=">"
        /// </summary>
        Selector,

        /// <summary>
        /// Represents a selected item. Default value : ASCII="[x]" Unicode="[x]"
        /// </summary>
        Selected,

        /// <summary>
        /// Represents an unselected item. Default value : ASCII="[ ]" Unicode="[ ]"
        /// </summary>
        NotSelect,

        /// <summary>
        /// Represents an expanded section. Default value : ASCII="[-]" Unicode="[-]"
        /// </summary>
        Expanded,

        /// <summary>
        /// Represents a collapsed section. Default value : ASCII="[+]" Unicode="[+]"
        /// </summary>
        Collapsed,

        /// <summary>
        /// Marks the beginning of an indented group. Default value : ASCII="|-" Unicode="├─"
        /// </summary>
        IndentGroup,

        /// <summary>
        /// Marks the end of an indented group. Default value : ASCII="|_" Unicode="└─"
        /// </summary>
        IndentEndGroup,

        /// <summary>
        /// Tree line with a crossing. Default value : ASCII=" |-" Unicode=" ├─"
        /// </summary>
        TreeLinecross,

        /// <summary>
        /// Tree line with a corner. Default value : ASCII=" |_" Unicode=" └─"
        /// </summary>
        TreeLinecorner,

        /// <summary>
        /// Vertical tree line. Default value : ASCII=" | " Unicode=" │ "
        /// </summary>
        TreeLinevertical,

        /// <summary>
        /// Space in a tree line. Default value : ASCII="   " Unicode="   "
        /// </summary>
        TreeLinespace,

        /// <summary>
        /// Double border style. Default value : ASCII="=" Unicode="═"
        /// </summary>
        DoubleBorder,

        /// <summary>
        /// Single border style. Default value : ASCII="-" Unicode="─"
        /// </summary>
        SingleBorder,

        /// <summary>
        /// Heavy border style. Default value : ASCII="*" Unicode="■"
        /// </summary>
        HeavyBorder,

        /// <summary>
        /// Light progress bar. Default value : ASCII="-" Unicode="─"
        /// </summary>
        ProgressBarLight,

        /// <summary>
        /// Double-light progress bar. Default value : ASCII="=" Unicode="═"
        /// </summary>
        ProgressBarDoubleLight,

        /// <summary>
        /// Square progress bar. Default value : ASCII="#" Unicode="■"
        /// </summary>
        ProgressBarSquare,

        /// <summary>
        /// Bar progress element. Default value : ASCII="|" Unicode="▐"
        /// </summary>
        ProgressBarBar,

        /// <summary>
        /// ASCII single progress representation. Default value : ASCII="-" Unicode="─"
        /// </summary>
        ProgressBarAsciiSingle,

        /// <summary>
        /// ASCII double progress representation. Default value : ASCII="=" Unicode="═"
        /// </summary>
        ProgressBarAsciiDouble,

        /// <summary>
        /// Dotted progress bar. Default value : ASCII="." Unicode="."
        /// </summary>
        ProgressBarDot,

        /// <summary>
        /// Light slider bar. Default value : ASCII="-" Unicode="─"
        /// </summary>
        SliderBarLight,

        /// <summary>
        /// Double-light slider bar. Default value : ASCII="=" Unicode="═"
        /// </summary>
        SliderBarDoubleLight,

        /// <summary>
        /// Square slider bar. Default value : ASCII="#" Unicode="■"
        /// </summary>
        SliderBarSquare,

        /// <summary>
        /// Chart label element. Default value : ASCII="#" Unicode="■"
        /// </summary>
        ChartLabel,

        /// <summary>
        /// Light chart element. Default value : ASCII="-" Unicode="─"
        /// </summary>
        ChartLight,

        /// <summary>
        /// Square chart element. Default value : ASCII="#" Unicode="■"
        /// </summary>
        ChartSquare,

        /// <summary>
        /// Grid single top-left corner. Default value : ASCII="+" Unicode="┌"
        /// </summary>
        GridSingleTopLeft,

        /// <summary>
        /// Grid single top-center. Default value : ASCII="+" Unicode="┬"
        /// </summary>
        GridSingleTopCenter,

        /// <summary>
        /// Grid single top-right corner. Default value : ASCII="+" Unicode="┐"
        /// </summary>
        GridSingleTopRight,

        /// <summary>
        /// Grid single middle-left. Default value : ASCII="|" Unicode="├"
        /// </summary>
        GridSingleMiddleLeft,

        /// <summary>
        /// Grid single middle-center. Default value : ASCII="+" Unicode="┼"
        /// </summary>
        GridSingleMiddleCenter,

        /// <summary>
        /// Grid single middle-right. Default value : ASCII="|" Unicode="┤"
        /// </summary>
        GridSingleMiddleRight,

        /// <summary>
        /// Grid single bottom-left corner. Default value : ASCII="+" Unicode="└"
        /// </summary>
        GridSingleBottomLeft,

        /// <summary>
        /// Grid single bottom-center. Default value : ASCII="+" Unicode="┴"
        /// </summary>
        GridSingleBottomCenter,

        /// <summary>
        /// Grid single bottom-right corner. Default value : ASCII="+" Unicode="┘"
        /// </summary>
        GridSingleBottomRight,

        /// <summary>
        /// Grid single border left. Default value : ASCII="|" Unicode="│"
        /// </summary>
        GridSingleBorderLeft,

        /// <summary>
        /// Grid single border right. Default value : ASCII="|" Unicode="│"
        /// </summary>
        GridSingleBorderRight,

        /// <summary>
        /// Grid single border top. Default value : ASCII="-" Unicode="─"
        /// </summary>
        GridSingleBorderTop,

        /// <summary>
        /// Grid single border bottom. Default value : ASCII="-" Unicode="─"
        /// </summary>
        GridSingleBorderBottom,

        /// <summary>
        /// Grid single vertical divider. Default value : ASCII="|" Unicode="│"
        /// </summary>
        GridSingleDividerY,

        /// <summary>
        /// Grid single horizontal divider. Default value : ASCII="-" Unicode="─"
        /// </summary>
        GridSingleDividerX,

        /// <summary>
        /// Grid double top-left corner. Default value : ASCII="+" Unicode="╔"
        /// </summary>
        GridDoubleTopLeft,

        /// <summary>
        /// Grid double top-center. Default value : ASCII="+" Unicode="╦"
        /// </summary>
        GridDoubleTopCenter,

        /// <summary>
        /// Grid double top-right corner. Default value : ASCII="+" Unicode="╗"
        /// </summary>
        GridDoubleTopRight,

        /// <summary>
        /// Grid double middle-left. Default value : ASCII="|" Unicode="╠"
        /// </summary>
        GridDoubleMiddleLeft,

        /// <summary>
        /// Grid double middle-center. Default value : ASCII="+" Unicode="╬"
        /// </summary>
        GridDoubleMiddleCenter,

        /// <summary>
        /// Grid double middle-right. Default value : ASCII="|" Unicode="╣"
        /// </summary>
        GridDoubleMiddleRight,

        /// <summary>
        /// Grid double bottom-left corner. Default value : ASCII="+" Unicode="╚"
        /// </summary>
        GridDoubleBottomLeft,

        /// <summary>
        /// Grid double bottom-center. Default value : ASCII="+" Unicode="╩"
        /// </summary>
        GridDoubleBottomCenter,

        /// <summary>
        /// Grid double bottom-right corner. Default value : ASCII="+" Unicode="╝"
        /// </summary>
        GridDoubleBottomRight,

        /// <summary>
        /// Grid double border left. Default value : ASCII="|" Unicode="║"
        /// </summary>
        GridDoubleBorderLeft,

        /// <summary>
        /// Grid double border right. Default value : ASCII="|" Unicode="║"
        /// </summary>
        GridDoubleBorderRight,

        /// <summary>
        /// Grid double border top. Default value : ASCII="=" Unicode="═"
        /// </summary>
        GridDoubleBorderTop,

        /// <summary>
        /// Grid double border bottom. Default value : ASCII="=" Unicode="═"
        /// </summary>
        GridDoubleBorderBottom,

        /// <summary>
        /// Grid double vertical divider. Default value : ASCII="|" Unicode="║"
        /// </summary>
        GridDoubleDividerY,

        /// <summary>
        /// Grid double horizontal divider. Default value : ASCII="=" Unicode="═"
        /// </summary>
        GridDoubleDividerX,

        /// <summary>
        /// Calendar note marker. Default value : ASCII="*" Unicode="*"
        /// </summary>
        CalendarNote,

        /// <summary>
        /// Calendar note highlight. Default value : ASCII="#" Unicode="#"
        /// </summary>
        CalendarNoteHighlight,

        /// <summary>
        /// Calendar day highlight. Default value : ASCII="!" Unicode="!"
        /// </summary>
        CalendarHighlight,

        /// <summary>
        /// Left portion of today's calendar highlight. Default value : ASCII="&lt;" Unicode="&lt;"
        /// </summary>
        CalendarTodayLeft,

        /// <summary>
        /// Right portion of today's calendar highlight. Default value : ASCII=">" Unicode=">"
        /// </summary>
        CalendarTodayRight,

        /// <summary>
        /// Left input delimiter. Default value : ASCII="[" Unicode="["
        /// </summary>
        InputDelimiterLeft,

        /// <summary>
        /// Right input delimiter. Default value : ASCII="]" Unicode="]"
        /// </summary>
        InputDelimiterRight,

        /// <summary>
        /// Left-most input delimiter. Default value : ASCII="{" Unicode="{"
        /// </summary>
        InputDelimiterLeftMost,

        /// <summary>
        /// Right-most input delimiter. Default value : ASCII="}" Unicode="}"
        /// </summary>
        InputDelimiterRightMost,
    }
}

