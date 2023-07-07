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
        /// Create TreeView Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        public static IControlTreeViewSelect<T> TreeView<T>(string prompt, string? description = null)
        {
            return TreeView<T>(prompt, description, null);
        }

        /// <summary>
        /// Create TreeView Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        public static IControlTreeViewSelect<T> TreeView<T>(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new TreeViewOptions<T>(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new TreeViewSelectControl<T>(_consoledrive, opt);
        }

        /// <summary>
        /// Create TreeView Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        public static IControlTreeViewSelect<T> TreeView<T>(string prompt, Action<IPromptConfig> config)
        {
            var opt = new TreeViewOptions<T>(true)
            {
                OptPrompt = prompt
            };
            config?.Invoke(opt);
            return new TreeViewSelectControl<T>(_consoledrive, opt);
        }
    }
}
