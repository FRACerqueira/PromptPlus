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
        /// Create Add to List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlList"/></returns>
        public static IControlList AddtoList(string prompt, string description = null)
        {
            return AddtoList(prompt, description, null);
        }

        /// <summary>
        /// Create Add to List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        public static IControlList AddtoList(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new ListOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            config?.Invoke(opt);
            return new ListControl(_consoledrive, opt);
        }

        /// <summary>
        /// Create Add to List Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        public static IControlList AddtoList(string prompt, Action<IPromptConfig> config)
        {
            var opt = new ListOptions(true)
            {
                OptPrompt = prompt
            };
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            config?.Invoke(opt);
            return new ListControl(_consoledrive, opt);
        }
    }
}
