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
        /// Create TreeView MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, string? description = null)
        {
            return TreeViewMultiSelect<T>(prompt, description, null);
        }

        /// <summary>
        /// Create TreeView MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new TreeViewOptions<T>(_styleschema,_configcontrols,_consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new TreeViewMultiSelectControl<T>(_consoledrive, opt);
        }

        /// <summary>
        /// Create TreeView MultiSelect Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, Action<IPromptConfig> config)
        {
            var opt = new TreeViewOptions<T>(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new TreeViewMultiSelectControl<T>(_consoledrive, opt);
        }
    }
}
