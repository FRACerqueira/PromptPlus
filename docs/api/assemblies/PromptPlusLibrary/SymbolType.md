![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### SymbolType enumeration
</br>


#### Represents symbol types used by the UI for status indicators, layout elements, progress bars, charts, grids, calendars, and input delimiters.

```csharp
public enum SymbolType
```

### Values

| name | value | description |
| --- | --- | --- |
| Done | `0` | Represents a completed action. Default value : ASCII="V" Unicode="√" |
| Error | `1` | Represents an error. Default value : ASCII="!" Unicode="!" |
| Canceled | `2` | Represents a canceled operation. Default value : ASCII="x" Unicode="x" |
| Selector | `3` | Represents a selector marker. Default value : ASCII="&gt;" Unicode="&gt;" |
| Selected | `4` | Represents a selected item. Default value : ASCII="[x]" Unicode="[x]" |
| NotSelect | `5` | Represents an unselected item. Default value : ASCII="[ ]" Unicode="[ ]" |
| Expanded | `6` | Represents an expanded section. Default value : ASCII="[-]" Unicode="[-]" |
| Collapsed | `7` | Represents a collapsed section. Default value : ASCII="[+]" Unicode="[+]" |
| IndentGroup | `8` | Marks the beginning of an indented group. Default value : ASCII="&#x7C;-" Unicode="├─" |
| IndentEndGroup | `9` | Marks the end of an indented group. Default value : ASCII="&#x7C;_" Unicode="└─" |
| TreeLinecross | `10` | Tree line with a crossing. Default value : ASCII=" &#x7C;-" Unicode=" ├─" |
| TreeLinecorner | `11` | Tree line with a corner. Default value : ASCII=" &#x7C;_" Unicode=" └─" |
| TreeLinevertical | `12` | Vertical tree line. Default value : ASCII=" &#x7C; " Unicode=" │ " |
| TreeLinespace | `13` | Space in a tree line. Default value : ASCII=" " Unicode=" " |
| DoubleBorder | `14` | Double border style. Default value : ASCII="=" Unicode="═" |
| SingleBorder | `15` | Single border style. Default value : ASCII="-" Unicode="─" |
| HeavyBorder | `16` | Heavy border style. Default value : ASCII="*" Unicode="■" |
| ProgressBarLight | `17` | Light progress bar. Default value : ASCII="-" Unicode="─" |
| ProgressBarDoubleLight | `18` | Double-light progress bar. Default value : ASCII="=" Unicode="═" |
| ProgressBarSquare | `19` | Square progress bar. Default value : ASCII="#" Unicode="■" |
| ProgressBarBar | `20` | Bar progress element. Default value : ASCII="&#x7C;" Unicode="▐" |
| ProgressBarAsciiSingle | `21` | ASCII single progress representation. Default value : ASCII="-" Unicode="─" |
| ProgressBarAsciiDouble | `22` | ASCII double progress representation. Default value : ASCII="=" Unicode="═" |
| ProgressBarDot | `23` | Dotted progress bar. Default value : ASCII="." Unicode="." |
| SliderBarLight | `24` | Light slider bar. Default value : ASCII="-" Unicode="─" |
| SliderBarDoubleLight | `25` | Double-light slider bar. Default value : ASCII="=" Unicode="═" |
| SliderBarSquare | `26` | Square slider bar. Default value : ASCII="#" Unicode="■" |
| ChartLabel | `27` | Chart label element. Default value : ASCII="#" Unicode="■" |
| ChartLight | `28` | Light chart element. Default value : ASCII="-" Unicode="─" |
| ChartSquare | `29` | Square chart element. Default value : ASCII="#" Unicode="■" |
| GridSingleTopLeft | `30` | Grid single top-left corner. Default value : ASCII="+" Unicode="┌" |
| GridSingleTopCenter | `31` | Grid single top-center. Default value : ASCII="+" Unicode="┬" |
| GridSingleTopRight | `32` | Grid single top-right corner. Default value : ASCII="+" Unicode="┐" |
| GridSingleMiddleLeft | `33` | Grid single middle-left. Default value : ASCII="&#x7C;" Unicode="├" |
| GridSingleMiddleCenter | `34` | Grid single middle-center. Default value : ASCII="+" Unicode="┼" |
| GridSingleMiddleRight | `35` | Grid single middle-right. Default value : ASCII="&#x7C;" Unicode="┤" |
| GridSingleBottomLeft | `36` | Grid single bottom-left corner. Default value : ASCII="+" Unicode="└" |
| GridSingleBottomCenter | `37` | Grid single bottom-center. Default value : ASCII="+" Unicode="┴" |
| GridSingleBottomRight | `38` | Grid single bottom-right corner. Default value : ASCII="+" Unicode="┘" |
| GridSingleBorderLeft | `39` | Grid single border left. Default value : ASCII="&#x7C;" Unicode="│" |
| GridSingleBorderRight | `40` | Grid single border right. Default value : ASCII="&#x7C;" Unicode="│" |
| GridSingleBorderTop | `41` | Grid single border top. Default value : ASCII="-" Unicode="─" |
| GridSingleBorderBottom | `42` | Grid single border bottom. Default value : ASCII="-" Unicode="─" |
| GridSingleDividerY | `43` | Grid single vertical divider. Default value : ASCII="&#x7C;" Unicode="│" |
| GridSingleDividerX | `44` | Grid single horizontal divider. Default value : ASCII="-" Unicode="─" |
| GridDoubleTopLeft | `45` | Grid double top-left corner. Default value : ASCII="+" Unicode="╔" |
| GridDoubleTopCenter | `46` | Grid double top-center. Default value : ASCII="+" Unicode="╦" |
| GridDoubleTopRight | `47` | Grid double top-right corner. Default value : ASCII="+" Unicode="╗" |
| GridDoubleMiddleLeft | `48` | Grid double middle-left. Default value : ASCII="&#x7C;" Unicode="╠" |
| GridDoubleMiddleCenter | `49` | Grid double middle-center. Default value : ASCII="+" Unicode="╬" |
| GridDoubleMiddleRight | `50` | Grid double middle-right. Default value : ASCII="&#x7C;" Unicode="╣" |
| GridDoubleBottomLeft | `51` | Grid double bottom-left corner. Default value : ASCII="+" Unicode="╚" |
| GridDoubleBottomCenter | `52` | Grid double bottom-center. Default value : ASCII="+" Unicode="╩" |
| GridDoubleBottomRight | `53` | Grid double bottom-right corner. Default value : ASCII="+" Unicode="╝" |
| GridDoubleBorderLeft | `54` | Grid double border left. Default value : ASCII="&#x7C;" Unicode="║" |
| GridDoubleBorderRight | `55` | Grid double border right. Default value : ASCII="&#x7C;" Unicode="║" |
| GridDoubleBorderTop | `56` | Grid double border top. Default value : ASCII="=" Unicode="═" |
| GridDoubleBorderBottom | `57` | Grid double border bottom. Default value : ASCII="=" Unicode="═" |
| GridDoubleDividerY | `58` | Grid double vertical divider. Default value : ASCII="&#x7C;" Unicode="║" |
| GridDoubleDividerX | `59` | Grid double horizontal divider. Default value : ASCII="=" Unicode="═" |
| CalendarNote | `60` | Calendar note marker. Default value : ASCII="*" Unicode="*" |
| CalendarNoteHighlight | `61` | Calendar note highlight. Default value : ASCII="#" Unicode="#" |
| CalendarHighlight | `62` | Calendar day highlight. Default value : ASCII="!" Unicode="!" |
| CalendarTodayLeft | `63` | Left portion of today's calendar highlight. Default value : ASCII="&lt;" Unicode="&lt;" |
| CalendarTodayRight | `64` | Right portion of today's calendar highlight. Default value : ASCII="&gt;" Unicode="&gt;" |
| InputDelimiterLeft | `65` | Left input delimiter. Default value : ASCII="[" Unicode="[" |
| InputDelimiterRight | `66` | Right input delimiter. Default value : ASCII="]" Unicode="]" |
| InputDelimiterLeftMost | `67` | Left-most input delimiter. Default value : ASCII="{" Unicode="{" |
| InputDelimiterRightMost | `68` | Right-most input delimiter. Default value : ASCII="}" Unicode="}" |

### See Also

* namespace [PromptPlusLibrary](../PromptPlus.md)

<!-- DO NOT EDIT: generated by xmldocmd for PromptPlus.dll -->
