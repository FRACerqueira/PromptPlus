// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Style funtions Extensions
    /// </summary>
    public static class StyleSchemaExtensions
    {

        /// <summary>
        /// Apply Foreground to <see cref="StyleControls"/>
        /// </summary>
        ///<param name="schema">The <see cref="StyleSchema"/></param>
        /// <param name="styleControl"><see cref="StyleControls"/> to apply</param>
        /// <param name="foreground">Foreground <see cref="Color"/></param>
        /// <returns><see cref="Style"/></returns>
        public static Style ApplyForeground(this StyleSchema schema, StyleControls styleControl, Color foreground)
        {
            return schema.ApplyStyle(styleControl, schema.GetStyle(styleControl).Foreground(foreground));
        }

        /// <summary>
        /// Apply global Background to <see cref="StyleControls"/>
        /// </summary>
        ///<param name="schema">The <see cref="StyleSchema"/></param>
        /// <param name="styleControl"><see cref="StyleControls"/> to apply</param>
        /// <param name="background">Background <see cref="Color"/></param>
        /// <returns><see cref="Style"/></returns>
        public static Style ApplyBackground(this StyleSchema schema, StyleControls styleControl, Color background)
        {
            return schema.ApplyStyle(styleControl, schema.GetStyle(styleControl).Background(background));
        }


        /// <summary>
        /// Get <see cref="Style"/> text Prompt.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Prompt(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Prompt);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TreeView Root.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TreeViewRoot(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TreeViewRoot);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TreeView Parent.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TreeViewParent(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TreeViewParent);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TreeView Child.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TreeViewChild(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TreeViewChild);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Browser File.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style BrowserFile(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.BrowserFile);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Browser Folder.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style BrowserFolder(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.BrowserFolder);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Browser Size.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style BrowserSize(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.BrowserSize);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TreeView Expand.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TreeViewExpand(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TreeViewExpand);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TaskTitle.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TaskTitle(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TaskTitle);
        }

        /// <summary>
        /// Get <see cref="Style"/> text TaskElapsedTime.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TaskElapsedTime(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TaskElapsedTime);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Slider-On(Foreground)/Slider-Off(Background).
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : 'ConsoleColor.DarkGray'</br>
        /// </summary>
        public static Style Slider(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Slider);
        }

        /// <summary>
        /// Get <see cref="Style"/> Spinner.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Spinner(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Spinner);
        }

        /// <summary>
        /// Get <see cref="Style"/> TableTitle.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TableTitle(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TableTitle);
        }

        /// <summary>
        /// Get <see cref="Style"/> TableContent.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TableContent(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TableContent);
        }

        /// <summary>
        /// Get <see cref="Style"/> TableHeader.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TableHeader(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TableHeader);
        }

        /// <summary>
        /// Get <see cref="Style"/> GroupTip.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style OnOff(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.OnOff);
        }

        /// <summary>
        /// Get <see cref="Style"/> GroupTip.
        /// <br>Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style GroupTip(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.GroupTip);
        }

        /// <summary>
        /// Get <see cref="Style"/> ChartLabel.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style ChartLabel(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.ChartLabel);
        }

        /// <summary>
        /// Get <see cref="Style"/> ChartOrder.
        /// <br>Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style ChartOrder(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.ChartOrder);
        }

        /// <summary>
        /// Get <see cref="Style"/> ChartPercent.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style ChartPercent(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.ChartPercent);
        }

        /// <summary>
        /// Get <see cref="Style"/> ChartTitle.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style ChartTitle(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.ChartTitle);
        }

        /// <summary>
        /// Get <see cref="Style"/> ChartValue.
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style ChartValue(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.ChartValue);
        }


        /// <summary>
        /// Get <see cref="Style"/> CalendarDay.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style CalendarDay(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.CalendarDay);
        }

        /// <summary>
        /// Get <see cref="Style"/> CalendarHighlight.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style CalendarHighlight(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.CalendarHighlight);
        }

        /// <summary>
        /// Get <see cref="Style"/> CalendarMonth.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style CalendarMonth(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.CalendarMonth);
        }

        /// <summary>
        /// Get <see cref="Style"/> CalendarWeekDay.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style CalendarWeekDay(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.CalendarWeekDay);
        }


        /// <summary>
        /// Get <see cref="Style"/> CalendarYear.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style CalendarYear(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.CalendarYear);
        }

        /// <summary>
        /// Get <see cref="Style"/> FilterMatch.
        /// <br>Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style FilterMatch(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.FilterMatch);
        }

        /// <summary>
        /// Get <see cref="Style"/> MaskPositive.
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style MaskPositive(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.MaskPositive);
        }

        /// <summary>
        /// Get <see cref="Style"/> MaskNegative.
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style MaskNegative(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.MaskNegative);
        }

        /// <summary>
        /// Get <see cref="Style"/> MaskTypeTip.
        /// <br>Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style MaskTypeTip(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.MaskTypeTip);
        }

        /// <summary>
        /// Get <see cref="Style"/> FilterUnMatch.
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style FilterUnMatch(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.FilterUnMatch);
        }

        /// <summary>
        /// Get <see cref="Style"/> Range.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Ranger(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Ranger);
        }

        /// <summary>
        /// Get <see cref="Style"/> Lines.
        /// <br>Foreground : 'ConsoleColor.White'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Lines(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Lines);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Answer.
        /// <br>Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Answer(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Answer);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Description.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Description(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Description);
        }


        /// <summary>
        /// Get <see cref="Style"/> text Suggestion.
        /// <br>Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Suggestion(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Suggestion);
        }


        /// <summary>
        /// Get <see cref="Style"/> text Selected.
        /// <br>Foreground : 'ConsoleColor.Green'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Selected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Selected);
        }


        /// <summary>
        /// Get <see cref="Style"/> text UnSelected.
        /// <br>Foreground : 'ConsoleColor.Gray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style UnSelected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.UnSelected);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Disabled.
        /// <br>Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Disabled(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Disabled);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Error.
        /// <br>Foreground : 'ConsoleColor.Red'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Error(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Error);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Pagination.
        /// <br>Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Pagination(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Pagination);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Tagged info.
        /// <br>Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style TaggedInfo(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TaggedInfo);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Tooltips.
        /// <br>Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>Background : same Console Background when not set</br>
        /// </summary>
        public static Style Tooltips(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Tooltips);
        }
    }
}
