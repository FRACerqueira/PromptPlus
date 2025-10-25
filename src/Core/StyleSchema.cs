// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents The Styles Schema of current instance of control
    /// </summary>
    internal static class StyleSchema
    {
        private static readonly Dictionary<ComponentStyles, Style> _Styles;

        static StyleSchema()
        {
            _Styles = Init();
        }

        public static Style GetStyle(ComponentStyles ComponentStyles)
        {
            return _Styles[ComponentStyles];
        }

        private static Dictionary<ComponentStyles, Style> Init()
        {
            Dictionary<ComponentStyles, Style> auxdic = [];
            IEnumerable<ComponentStyles> aux = Enum.GetValues<ComponentStyles>().Cast<ComponentStyles>();
            foreach (ComponentStyles item in aux)
            {
                switch (item)
                {
                    case ComponentStyles.Prompt:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.Answer:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan));
                        break;
                    case ComponentStyles.NegativeValue:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan));
                        break;
                    case ComponentStyles.PositiveValue:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan));
                        break;
                    case ComponentStyles.Description:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.Suggestion:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Yellow));
                        break;
                    case ComponentStyles.Selected:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Green));
                        break;
                    case ComponentStyles.UnSelected:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Gray));
                        break;
                    case ComponentStyles.Disabled:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.Error:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Red));
                        break;
                    case ComponentStyles.Pagination:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.TaggedInfo:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.Tooltips:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.Spinner:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.ElapsedTime:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan));
                        break;
                    case ComponentStyles.Ranger:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.Lines:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.CalendarDay:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.CalendarHighlight:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.CalendarMonth:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.CalendarWeekDay:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.CalendarYear:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.ChartLabel:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.ChartOrder:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.ChartPercent:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.Slider:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan, Style.Default().Background));
                        break;
                    case ComponentStyles.SliderOn:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan, ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.SliderOff:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan, ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.ChartTitle:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.ChartValue:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Cyan));
                        break;
                    case ComponentStyles.GroupTip:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkGray));
                        break;
                    case ComponentStyles.TableTitle:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.TableHeader:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.TableContent:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.FileRoot:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.FileSize:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    case ComponentStyles.FileTypeFile:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Gray));
                        break;
                    case ComponentStyles.FileTypeFolder:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Gray));
                        break;
                    case ComponentStyles.ExpandSymbol:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Gray));
                        break;
                    case ComponentStyles.Root:
                        auxdic.Add(item, Style.Colors(ConsoleColor.White));
                        break;
                    case ComponentStyles.Node:
                        auxdic.Add(item, Style.Colors(ConsoleColor.Gray));
                        break;
                    case ComponentStyles.ChildsCount:
                        auxdic.Add(item, Style.Colors(ConsoleColor.DarkYellow));
                        break;
                    default:
                        throw new NotImplementedException($"{item} Not Implemented");
                }
            }
            return auxdic;
        }

    }
}
