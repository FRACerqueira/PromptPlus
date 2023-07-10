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
        /// Create MaskEdit Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        public static IControlMaskEdit MaskEdit(string prompt, Action<IPromptConfig> config = null)
        {
            return MaskEdit(prompt,"",config);
        }

        /// <summary>
        /// Create MaskEdit Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        public static IControlMaskEdit MaskEdit(string prompt, string? description , Action<IPromptConfig> config = null)
        {
            var opt = new MaskEditOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new MaskEditControl(_consoledrive, opt);
        }
    }
}
