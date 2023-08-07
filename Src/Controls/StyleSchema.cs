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

        internal Dictionary<StyleControls, Style> Init()
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
                    case StyleControls.Sugestion:
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
                    case StyleControls.Slider:
                        auxdic.Add(item, Style.Default.Foreground(ConsoleColor.Cyan).Background(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Chart:
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
                    case StyleControls.Sugestion:
                        auxdic.Add(item, source.Sugestion());
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
                    case StyleControls.Slider:
                        auxdic.Add(item, source.Slider());
                        break;
                    case StyleControls.Chart:
                        auxdic.Add(item, source.Chart());
                        break;
                    default:
                        throw new PromptPlusException($"{item} Not Implemented");
                }
            }
            return new StyleSchema(auxdic);
        }
    }
}
