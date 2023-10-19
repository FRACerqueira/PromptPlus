// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Controls.Objects;
using System;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        public static IControlBrowserSelect Browser(string prompt, string? description = null)
        {
            return Browser(prompt, description, null);
        }

        /// <summary>
        /// Create Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        public static IControlBrowserSelect Browser(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new BrowserOptions(_styleschema,_configcontrols,_consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
                Spinner = new Spinners(SpinnersType.Ascii, _consoledrive.IsUnicodeSupported)
            };
            config?.Invoke(opt);
            return new BrowserSelectControl(_consoledrive, opt);
        }

        /// <summary>
        /// Create Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        public static IControlBrowserSelect Browser(string prompt, Action<IPromptConfig> config)
        {
            var opt = new BrowserOptions(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt,
                Spinner = new Spinners(SpinnersType.Ascii, _consoledrive.IsUnicodeSupported)
            };
            config?.Invoke(opt);
            return new BrowserSelectControl(_consoledrive, opt);
        }
    }
}
