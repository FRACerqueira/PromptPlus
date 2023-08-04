// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using System;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Progress Bar Control
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlProgressBar{Object}"/></returns>
        public static IControlProgressBar<object> ProgressBar(string prompt, string? description = null)
        {
            return ProgressBar<object>(ProgressBarType.Fill, prompt, null, description, null);
        }

        /// <summary>
        /// Create Progress Bar Control
        /// </summary>
        /// <typeparam name="T">Typeof return</typeparam>
        /// <param name="barType">The type Progress Bar. <see cref="ProgressBarType"/></param>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="defaultresult">The instance result</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        public static IControlProgressBar<T> ProgressBar<T>(ProgressBarType barType, string prompt, T defaultresult, string description = null)
        {
            return ProgressBar(barType, prompt, defaultresult, description, null);   
        }

        /// <summary>
        /// Create instance Progress Bar Control
        /// </summary>
        /// <typeparam name="T">Typeof return</typeparam>
        /// <param name="barType">The type Progress Bar. <see cref="ProgressBarType"/></param>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="defaultresult">The starting value for the result</param>
        /// <param name="description">The description text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        public static IControlProgressBar<T> ProgressBar<T>(ProgressBarType barType, string prompt, T defaultresult, string description, Action<IPromptConfig> config = null)
        {
            var opt = new ProgressBarOptions<T>(_styleschema,_configcontrols,_consoledrive, false)
            {
                ValueResult = defaultresult,
                BarType = barType,
                OptDescription = description,
                OptPrompt = prompt
            };
            config?.Invoke(opt);
            return new ProgressBarControl<T>(_consoledrive, opt);
        }
    }
}

