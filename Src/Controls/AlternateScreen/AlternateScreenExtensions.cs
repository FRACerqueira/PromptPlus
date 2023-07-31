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
        /// Create CustomAction Control
        /// </summary>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        public static IControlAlternateScreen AlternateScreen()
        {
            return AlternateScreen(null);
        }

        /// <summary>
        /// Create CustomAction Control
        /// </summary>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        public static IControlAlternateScreen AlternateScreen(Action<IPromptConfig> config)
        {
            var opt = new AlternateScreenOtions(true)
            {
            };
            config?.Invoke(opt);
            return new AlternateScreenControl(_consoledrive, opt);
        }
    }
}