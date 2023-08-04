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
        /// Create Select Control. 
        /// </summary>
        /// <typeparam name="T">Typeof T</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        public static IControlSelect<T> Select<T>(string prompt, Action<IPromptConfig> config)
        {
            return Select<T>(prompt, "", config);
        }

        /// <summary>
        /// Create Select Control. 
        /// </summary>
        /// <typeparam name="T">Typeof T</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        public static IControlSelect<T> Select<T>(string prompt, string? description = null)
        {
            return Select<T>(prompt, description, null);
        }

        /// <summary>
        /// Create Select Control. 
        /// </summary>
        /// <typeparam name="T">Typeof T</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        public static IControlSelect<T> Select<T>(string prompt, string description , Action<IPromptConfig> config = null)
        {
            var opt = new SelectOptions<T>(_styleschema,_configcontrols,_consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new SelectControl<T>(_consoledrive, opt);
        }
    }
}
