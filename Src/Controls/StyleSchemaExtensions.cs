// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    public static class StyleSchemaExtensions
    {

        /// <summary>
        /// Apply Foreground to <see cref="StyleControls"/>
        /// </summary>
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
        /// <param name="styleControl"><see cref="StyleControls"/> to apply</param>
        /// <param name="background">Background <see cref="Color"/></param>
        /// <returns><see cref="Style"/></returns>
        public static Style ApplyBackground(this StyleSchema schema, StyleControls styleControl, Color background)
        {
            return schema.ApplyStyle(styleControl, schema.GetStyle(styleControl).Background(background));
        }


        /// <summary>
        /// <para>Get <see cref="Style"/> text Prompt.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.White'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Prompt(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Prompt);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Slider-On(Foreground)/Slider-Off(Background).</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>ValueResult Background : 'ConsoleColor.DarkGray'</br>
        /// </summary>
        public static Style Slider(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Slider);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Answer.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Answer(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Answer);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Description.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Description(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Description);
        }


        /// <summary>
        /// <para>Get <see cref="Style"/> text Sugestion.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Sugestion(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Sugestion);
        }


        /// <summary>
        /// <para>Get <see cref="Style"/> text Selected.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Green'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Selected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Selected);
        }


        /// <summary>
        /// <para>Get <see cref="Style"/> text UnSelected.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Gray'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style UnSelected(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.UnSelected);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Disabled.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Disabled(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Disabled);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Error.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.Red'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Error(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Error);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Pagination.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Pagination(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Pagination);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Tagged info.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style TaggedInfo(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.TaggedInfo);
        }

        /// <summary>
        /// <para>Get <see cref="Style"/> text Tooltips.</para>
        /// <br>ValueResult Foreground : 'ConsoleColor.DarkGray'</br>
        /// <br>ValueResult Background : same Console Background when setted</br>
        /// </summary>
        public static Style Tooltips(this StyleSchema schema)
        {
            return schema.GetStyle(StyleControls.Tooltips);
        }
    }
}
