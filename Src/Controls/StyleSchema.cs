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
            _Styles = new Dictionary<StyleControls, Style>();
            Init();
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

        private void Init()
        {
            var aux = Enum.GetValues(typeof(StyleControls)).Cast<StyleControls>();
            foreach (var item in aux) 
            {
                switch (item)
                {
                    case StyleControls.Prompt:
                        _Styles.Add(item,Style.Plain.Foreground(ConsoleColor.White));
                        break;
                    case StyleControls.Answer:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Cyan));
                        break;
                    case StyleControls.Description:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.Sugestion:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Yellow));
                        break;
                    case StyleControls.Selected:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Green));
                        break;
                    case StyleControls.UnSelected:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Gray));
                        break;
                    case StyleControls.Disabled:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Error:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Red));
                        break;
                    case StyleControls.Pagination:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.TaggedInfo:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.DarkYellow));
                        break;
                    case StyleControls.Tooltips:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Slider:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.Cyan).Background(ConsoleColor.DarkGray));
                        break;
                    case StyleControls.Chart:
                        _Styles.Add(item, Style.Plain.Foreground(ConsoleColor.White));
                        break;
                    default:
                        throw new PromptPlusException($"{item} Not Implemented");
                }
            }
        }

        internal void UpdateBackgoundColor(Color backgoundcolor)
        {
            Style.Plain = Style.Plain.Background(backgoundcolor);
            var aux = Enum.GetValues(typeof(StyleControls)).Cast<StyleControls>();
            foreach (var item in aux)
            {
                var stl = _Styles[item];
                stl = stl.Background(backgoundcolor);
                _Styles[item] = stl;
            }
        }
    }
}
