// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the KeyPress control
    /// </summary>
    public interface IControlKeyPress : IPromptControls<ConsoleKeyInfo>
    {
        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        IControlKeyPress Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add Key and Modifiers valids for keypress
        /// </summary>
        /// <param name="key">Key <see cref="ConsoleKey"/></param>
        /// <param name="modifiers">Modifiers <see cref="ConsoleModifiers"/></param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        IControlKeyPress AddKeyValid(ConsoleKey key,ConsoleModifiers? modifiers = null);

        /// <summary>
        /// Overwrite default ConsoleKey string to custom string.
        /// <br>When return null value the control use defaut string</br>
        /// </summary>
        /// <param name="value">Transform function. When return null value the control use defaut string</param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        IControlKeyPress TextKeyValid(Func<ConsoleKeyInfo, string?> value);

        /// <summary>
        /// Overwrite <see cref="SpinnersType"/>. ValueResult value is SpinnersType.Ascii
        /// <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected</br>
        /// </summary>
        /// <param name="spinnersType">Spinners Type</param>
        /// <param name="SpinnerStyle">Style of spinner. <see cref="Style"/></param>
        /// <param name="speedAnimation">Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <param name="customspinner">IEnumerable value for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <returns><see cref="IControlKeyPress"/></returns>
        IControlKeyPress Spinner(SpinnersType spinnersType, Style? SpinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null);

    }
}
