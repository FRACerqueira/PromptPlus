// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the AlternateScreen control
    /// </summary>

    public interface IControlAlternateScreen: IPromptControls<bool>
    {
        /// <summary>
        /// Action when console/terminal has Capability to swith to AlternateScreen buffer.
        /// <br>If not has capability to swith AlternateScreen, the action not run and the control return false, otherwise the control return true</br>
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        IControlAlternateScreen CustomAction(Action<CancellationToken> value);


        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        IControlAlternateScreen Config(Action<IPromptConfig> context);

        /// <summary>
        /// Set Foreground Color to AlternateScreen buffer.
        /// </summary>
        /// <param name="value">The Foreground</param>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        IControlAlternateScreen ForegroundColor(ConsoleColor value);

        /// <summary>
        /// Set Background Color to AlternateScreen buffer.
        /// </summary>
        /// <param name="value">The Background</param>
        /// <returns><see cref="IControlAlternateScreen"/></returns>
        IControlAlternateScreen BackgroundColor(ConsoleColor value);



    }
}
