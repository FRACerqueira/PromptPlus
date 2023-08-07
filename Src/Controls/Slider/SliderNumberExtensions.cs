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
        /// Create Slider Number Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="config">The description text to write</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        public static IControlSliderNumber SliderNumber(string prompt, Action<IPromptConfig> config = null)
        {
            return SliderNumber(prompt, "", config);
        }

        /// <summary>
        /// Create Slider Number Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        public static IControlSliderNumber SliderNumber(string prompt, string? description, Action<IPromptConfig> config = null)
        {
            var opt = new SliderNumberOptions(_styleschema,_configcontrols,_consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new SliderNumberControl(_consoledrive, opt);
        }
    }
}
