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
        /// <br>ValueResult Foreground : 'ConsoleColor.White'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Prompt(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Prompt);
        }


        /// <summary>
        /// Get <see cref="Style"/> text Chart.
        /// <br>ValueResult Foreground : 'ConsoleColor.White'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Chart(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Chart);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Slider-On(Foreground)/Slider-Off(Background).
        /// <br>ValueResult Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>ValueResult Background : 'ConsoleColor.DarkGray'</br>
        /// </summary>
        public static Style Slider(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Slider);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Answer.
        /// <br>ValueResult Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Answer(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Answer);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Description.
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Description(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Description);
        }


        /// <summary>
        /// Get <see cref="Style"/> text Suggestion.
        /// <br>ValueResult Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Suggestion(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Suggestion);
        }


        /// <summary>
        /// Get <see cref="Style"/> text Selected.
        /// <br>ValueResult Foreground : 'ConsoleColor.Green'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Selected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Selected);
        }


        /// <summary>
        /// Get <see cref="Style"/> text UnSelected.
        /// <br>ValueResult Foreground : 'ConsoleColor.Gray'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style UnSelected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.UnSelected);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Disabled.
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Disabled(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Disabled);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Error.
        /// <br>ValueResult Foreground : 'ConsoleColor.Red'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Error(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Error);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Pagination.
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Pagination(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Pagination);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Tagged info.
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style TaggedInfo(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TaggedInfo);
        }

        /// <summary>
        /// Get <see cref="Style"/> text Tooltips.
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when set</br>
        /// </summary>
        public static Style Tooltips(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Tooltips);
        }
    }
}
