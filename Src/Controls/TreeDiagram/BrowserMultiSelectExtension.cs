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
        /// Create MultiSelect Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        /// <returns><see cref="IControlBrowserMultiSelect"/></returns>
        public static IControlBrowserMultiSelect BrowserMultiSelect(string prompt, string? description = null)
        {
            return BrowserMultiSelect(prompt, description, null);
        }

        /// <summary>
        /// Create MultiSelect Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        /// <param name="description">The description text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlBrowserMultiSelect"/></returns>
        public static IControlBrowserMultiSelect BrowserMultiSelect(string prompt, string description, Action<IPromptConfig> config = null)
        {
            var opt = new BrowserOptions(_styleschema, _configcontrols, _consoledrive, true)
            {
                OptPrompt = prompt,
                OptDescription = description,
                Spinner = new Spinners(SpinnersType.Ascii, _consoledrive.IsUnicodeSupported)
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new BrowserMultiSelectControl(_consoledrive, opt);
        }

        /// <summary>
        /// Create MultiSelect Browser Control. 
        /// </summary>
        /// <param name="prompt">The prompt text to write</param>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlBrowserMultiSelect"/></returns>
        public static IControlBrowserMultiSelect BrowserMultiSelect(string prompt, Action<IPromptConfig> config)
        {
            var opt = new BrowserOptions(_styleschema,_configcontrols,_consoledrive, true)
            {
                OptPrompt = prompt,
                Spinner = new Spinners(SpinnersType.Ascii, _consoledrive.IsUnicodeSupported)
            };
            config?.Invoke(opt);
            opt.ApplyStyle(StyleControls.Answer, opt.OptStyleSchema.Answer().Overflow(Overflow.Ellipsis));
            return new BrowserMultiSelectControl(_consoledrive, opt);
        }
    }
}
