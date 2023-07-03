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
        /// Create Slider Switch on/off Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        public static IControlSliderSwitch SliderSwitch(string prompt, Action<IPromptConfig> config = null)
        {
            return SliderSwitch(prompt, "", config);
        }

        /// <summary>
        /// Create Slider Switch on/off Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        public static IControlSliderSwitch SliderSwitch(string prompt, string? description, Action<IPromptConfig> config = null)
        {
            var opt = new SliderSwitchOptions(true)
            {
                OptPrompt = prompt,
                OptDescription = description,
            };
            config?.Invoke(opt);
            return new SliderSwitchControl(_consoledrive, opt);
        }
    }
}