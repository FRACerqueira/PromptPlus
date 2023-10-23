// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Styles Schema of current instance of control
    /// </summary>
    public class StyleSchema
    {
        private readonly Dictionary<StyleControls, Style> _Styles;

        internal StyleSchema()
        {
            _Styles = Init();
        }

        private StyleSchema(Dictionary<StyleControls, Style> newtyles)
        {
            _Styles = newtyles;
        }

        /// <summary>
        /// Apply style current instance of control
        /// </summary>
        /// <param name="styleControl"><see cref="StyleControls"/> to apply</param>
        /// <param name="value"><see cref="Style"/> value to apply</param>
        /// <returns><see cref="Style"/></returns>
        public Style ApplyStyle(StyleControls styleControl, Style value)
        {
            _Styles[styleControl] = value;
            return _Styles[styleControl];
        }

        internal Style GetStyle(StyleControls styleControls)
        {
            return _Styles[styleControls];
        }

        internal static Dictionary<StyleControls, Style> Init()
        {
            var auxdic = new Dictionary<StyleControls, Style>();
            var aux = Enum.GetValues(typeof(StyleControls)).Cast<StyleControls>();
            foreach (var item in aux) 
            {
                switch (item)
                {
                    case StyleControls.Prompt:
                        auxdic.Add(item,Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.Answer:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.Description:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.Suggestion:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Yellow));
                        break;
                    case StyleControls.Selected:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Green));
                        break;
                    case StyleControls.UnSelected:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Gray));
                        break;
                    case StyleControls.Disabled:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Error:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Red));
                        break;
                    case StyleControls.Pagination:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.TaggedInfo:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.Tooltips:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Spinner:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.Slider:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan).Background(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Ranger:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.Lines:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.FilterMatch:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Yellow));
                        break;
                    case StyleControls.FilterUnMatch:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.MaskTypeTip:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Yellow));
                        break;
                    case StyleControls.MaskNegative:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.MaskPositive:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.CalendarDay:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.CalendarHighlight:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.CalendarMonth:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.CalendarWeekDay:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.CalendarYear:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.ChartLabel:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.ChartOrder:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.ChartPercent:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.ChartTitle:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.ChartValue:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.GroupTip:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.OnOff:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TableTitle:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TableHeader:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TableContent:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TaskTitle:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TaskElapsedTime:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.BrowserFile:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.BrowserFolder:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.BrowserSize:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TreeViewExpand:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TreeViewRoot:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TreeViewParent:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.TreeViewChild:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.White));
                        break;
                    default:
                        throw new PromptPlusException($"{item} Not Implemented");
                }
            }
            return auxdic;
        }

        internal void UpdateBackgoundColor(Color backgoundcolor)
        {
            var aux = Enum.GetValues(typeof(StyleControls)).Cast<StyleControls>();
            foreach (var item in aux)
            {
                var stl = _Styles[item];
                stl = stl.Background(backgoundcolor);
                _Styles[item] = stl;
            }
        }

        internal static StyleSchema Clone(StyleSchema source)
        {
            var auxdic = new Dictionary<StyleControls, Style>();
            var aux = Enum.GetValues(typeof(StyleControls)).Cast<StyleControls>();
            foreach (var item in aux)
            {
                switch (item)
                {
                    case StyleControls.Prompt:
                        auxdic.Add(item, source.Prompt());
                        break;
                    case StyleControls.Answer:
                        auxdic.Add(item, source.Answer());
                        break;
                    case StyleControls.Description:
                        auxdic.Add(item, source.Description());
                        break;
                    case StyleControls.Suggestion:
                        auxdic.Add(item, source.Suggestion());
                        break;
                    case StyleControls.Selected:
                        auxdic.Add(item, source.Selected());
                        break;
                    case StyleControls.UnSelected:
                        auxdic.Add(item, source.UnSelected());
                        break;
                    case StyleControls.Disabled:
                        auxdic.Add(item, source.Disabled());
                        break;
                    case StyleControls.Error:
                        auxdic.Add(item, source.Error());
                        break;
                    case StyleControls.Pagination:
                        auxdic.Add(item, source.Pagination());
                        break;
                    case StyleControls.TaggedInfo:
                        auxdic.Add(item, source.TaggedInfo());
                        break;
                    case StyleControls.Tooltips:
                        auxdic.Add(item, source.Tooltips());
                        break;
                    case StyleControls.Spinner:
                        auxdic.Add(item, source.Spinner());
                        break;
                    case StyleControls.Slider:
                        auxdic.Add(item, source.Slider());
                        break;
                    case StyleControls.Ranger:
                        auxdic.Add(item, source.Ranger());
                        break;
                    case StyleControls.Lines:
                        auxdic.Add(item, source.Lines());
                        break;
                    case StyleControls.FilterMatch:
                        auxdic.Add(item, source.FilterMatch());
                        break;
                    case StyleControls.FilterUnMatch:
                        auxdic.Add(item, source.FilterUnMatch());
                        break;
                    case StyleControls.MaskTypeTip:
                        auxdic.Add(item, source.MaskTypeTip());
                        break;
                    case StyleControls.MaskNegative:
                        auxdic.Add(item, source.MaskNegative());
                        break;
                    case StyleControls.MaskPositive:
                        auxdic.Add(item, source.MaskPositive());
                        break;
                    case StyleControls.CalendarDay:
                        auxdic.Add(item, source.CalendarDay());
                        break;
                    case StyleControls.CalendarHighlight:
                        auxdic.Add(item, source.CalendarHighlight());
                        break;
                    case StyleControls.CalendarMonth:
                        auxdic.Add(item, source.CalendarMonth());
                        break;
                    case StyleControls.CalendarWeekDay:
                        auxdic.Add(item, source.CalendarWeekDay());
                        break;
                    case StyleControls.CalendarYear:
                        auxdic.Add(item, source.CalendarYear());
                        break;
                    case StyleControls.ChartLabel:
                        auxdic.Add(item, source.ChartLabel());
                        break;
                    case StyleControls.ChartOrder:
                        auxdic.Add(item, source.ChartOrder());
                        break;
                    case StyleControls.ChartPercent:
                        auxdic.Add(item, source.ChartPercent());
                        break;
                    case StyleControls.ChartTitle:
                        auxdic.Add(item, source.ChartTitle());
                        break;
                    case StyleControls.ChartValue:
                        auxdic.Add(item, source.ChartValue());
                        break;
                    case StyleControls.GroupTip:
                        auxdic.Add(item, source.GroupTip());
                        break;
                    case StyleControls.OnOff:
                        auxdic.Add(item, source.OnOff());
                        break;
                    case StyleControls.TableTitle:
                        auxdic.Add(item, source.TableTitle());
                        break;
                    case StyleControls.TableHeader:
                        auxdic.Add(item, source.TableHeader());
                        break;
                    case StyleControls.TableContent:
                        auxdic.Add(item, source.TableContent());
                        break;
                    case StyleControls.TaskTitle:
                        auxdic.Add(item, source.TaskTitle());
                        break;
                    case StyleControls.TaskElapsedTime:
                        auxdic.Add(item, source.TaskElapsedTime());
                        break;
                    case StyleControls.BrowserFile:
                        auxdic.Add(item, source.BrowserFile());
                        break;
                    case StyleControls.BrowserFolder:
                        auxdic.Add(item, source.BrowserFolder());
                        break;
                    case StyleControls.BrowserSize:
                        auxdic.Add(item, source.BrowserSize());
                        break;
                    case StyleControls.TreeViewExpand:
                        auxdic.Add(item, source.TreeViewExpand());
                        break;
                    case StyleControls.TreeViewRoot:
                        auxdic.Add(item, source.TreeViewRoot());
                        break;
                    case StyleControls.TreeViewParent:
                        auxdic.Add(item, source.TreeViewParent());
                        break;
                    case StyleControls.TreeViewChild:
                        auxdic.Add(item, source.TreeViewChild());
                        break;
                    default:
                        throw new PromptPlusException($"{item} Not Implemented");
                }
            }
            return new StyleSchema(auxdic);
        }
    }
}
