using System;
using PPlus.Controls;
using PPlus.Controls.Table;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Write Table in console. 
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
        /// Write Table in console. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        public static IControlTable<T> Table<T>(string prompt, string? description = null) where T : class
        {
            return Table<T>(prompt, description, null);
        }

        /// <summary>
        /// Write Table in console. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
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

        /// <summary>
        /// Create Table Select Control. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTableSelect{T}"/></returns>
        public static IControlTableSelect<T> TableSelect<T>(string prompt, Action<IPromptConfig> config) where T : class
        {
            return TableSelect<T>(prompt, "", config);
        }

        /// <summary>
        /// Create Table Select Control. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlTableSelect{T}"/></returns>
        public static IControlTableSelect<T> TableSelect<T>(string prompt, string? description = null) where T : class
        {
            return TableSelect<T>(prompt, description, null);
        }

        /// <summary>
        /// Create Table Select Control. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTableSelect{T}"/></returns>
        public static IControlTableSelect<T> TableSelect<T>(string prompt, string description, Action<IPromptConfig> config = null) where T : class
        {
            var opt = new TableOptions<T>(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
                IsInteraction = true
            };
            config?.Invoke(opt);
            return new TableSelectControl<T>(_consoledrive, opt);
        }

        /// <summary>
        /// Create Table MultiSelect Control. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        public static IControlTableMultiSelect<T> TableMultiSelect<T>(string prompt, Action<IPromptConfig> config) where T : class
        {
            return TableMultiSelect<T>(prompt, "", config);
        }

        /// <summary>
        /// Create Table MultiSelect Control. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        public static IControlTableMultiSelect<T> TableMultiSelect<T>(string prompt, string? description = null) where T : class
        {
            return TableMultiSelect<T>(prompt, description, null);
        }

        /// <summary>
        /// Create Table MultiSelect Control to Write to console. 
        /// </summary>
        /// <typeparam name="T">type of data to table</typeparam>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        public static IControlTableMultiSelect<T> TableMultiSelect<T>(string prompt, string description, Action<IPromptConfig> config = null) where T : class
        {
            var opt = new TableOptions<T>(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
                IsInteraction = true
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new TableMultiSelectControl<T>(_consoledrive, opt);
        }
    }
}
