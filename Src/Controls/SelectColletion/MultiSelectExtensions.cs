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
        /// Create MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        public static IControlMultiSelect<T> MultiSelect<T>(string prompt, Action<IPromptConfig> config)
        {
            return MultiSelect<T>(prompt, "", config);
        }
        
        /// <summary>
        /// Create MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        public static IControlMultiSelect<T> MultiSelect<T>(string prompt, string? description = null)
        {
            return MultiSelect<T>(prompt, description, null);
        }

        /// <summary>
        /// Create MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        public static IControlMultiSelect<T> MultiSelect<T>(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new MultiSelectOptions<T>(true)
            {
                OptPrompt = prompt,
                OptDescription = description    
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new MultiSelectControl<T>(_consoledrive, opt);
        }

    }
}
