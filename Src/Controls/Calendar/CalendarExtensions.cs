// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using PPlus.Controls;

namespace PPlus
{
    public static partial class PromptPlus
    {

        /// <summary>
        /// Create Calendar Control to Write to console. 
        /// </summary>
        /// <param name="prompt">The prompt text to chart</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        public static IControlCalendar Calendar(string prompt, Action<IPromptConfig> config)
        {
            return Calendar(prompt, "", config);
        }

        /// <summary>
        /// Create Calendar Control to Write to console. 
        /// </summary>
        /// <param name="prompt">The prompt text to chart</param>
        /// <param name="description">The description text to chart</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        public static IControlCalendar Calendar(string prompt, string? description = null)
        {
            return Calendar(prompt, description, null);
        }

        /// <summary>
        /// Create Calendar Control to Write to console. 
        /// </summary>
        /// <param name="prompt">The prompt text to chart</param>
        /// <param name="description">The description text to chart</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        public static IControlCalendar Calendar(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new CalendarOptions(true)
            {
                OptPrompt = (prompt ?? string.Empty).Trim(),
                OptDescription = (description ?? string.Empty).Trim()
            };
            config?.Invoke(opt);
            return new CalendarControl(_consoledrive, opt);
        }
    }
}