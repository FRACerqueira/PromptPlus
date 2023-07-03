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
        /// Create Add to MaskEdit List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        public static IControlMaskEditList AddtoMaskEditList(string prompt, string description = null)
        {
            return AddtoMaskEditList(prompt, description, null);
        }

        /// <summary>
        /// Create Add to MaskEdit List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        public static IControlMaskEditList AddtoMaskEditList(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new MaskEditListOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new MaskEditListControl(_consoledrive, opt);
        }

        /// <summary>
        /// Create Add to MaskEdit List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        public static IControlMaskEditList AddtoMaskEditList(string prompt, Action<IPromptConfig> config)
        {
            var opt = new MaskEditListOptions(true)
            {
                OptPrompt = prompt
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new MaskEditListControl(_consoledrive, opt);
        }
    }
}