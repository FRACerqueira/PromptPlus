using System;
using PPlus.Controls;
using PPlus.Controls.Table;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Table Control to Write to console. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        public static IControlTable<T> Table<T>(string prompt, Action<IPromptConfig> config) where T : class
        {
            return Table<T>(prompt, "", config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        public static IControlTable<T> Table<T>(string prompt, string? description = null) where T : class
        {
            return Table<T>(prompt, description, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns></returns>
        public static IControlTable<T> Table<T>(string prompt, string description, Action<IPromptConfig> config = null) where T : class 
        {
            var opt = new TableOptions<T>(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description
            };
            config?.Invoke(opt);
            return new TableControl<T>(_consoledrive, opt);
        }
    }
}
