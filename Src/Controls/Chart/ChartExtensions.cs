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
        /// Create ChartBar Control to Write to console. 
        /// </summary>
        /// <param name="title">The title text to chart</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlChartBar"/></returns>
        public static IControlChartBar ChartBar(string title, Action<IPromptConfig> config)
        {
            return ChartBar(title, "", config);
        }

        /// <summary>
        /// Create ChartBar Control to Write to console. 
        /// </summary>
        /// <param name="title">The title text to chart</param>
        /// <param name="description">The description text to chart</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        public static IControlChartBar ChartBar(string title, string? description = null)
        {
            return ChartBar(title, description, null);
        }

        /// <summary>
        /// Create ChartBar Control to Write to console. 
        /// </summary>
        /// <param name="title">The prompt text to chart</param>
        /// <param name="description">The description text to chart</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlChartBar"/></returns>
        public static IControlChartBar ChartBar(string title, string description, Action<IPromptConfig> config = null)
        {
            var opt = new ChartBarOptions(false)
            {
                OptPrompt = (title ?? string.Empty).Trim(),
                OptDescription = (description ?? string.Empty).Trim()
            };
            opt.EnabledAbortKey(false);
            config?.Invoke(opt);
            return new ChartBarControl(_consoledrive, opt);
        }
    }
}
