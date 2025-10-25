// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a Switch widget.
    /// </summary>
    public interface ISwitchWidget
    {
        /// <summary>
        /// Overwrites styles for the Switch Control.
        /// </summary>
        /// <param name="styleType">The <see cref="SwitchStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        ISwitchWidget Styles(SwitchStyles styleType, Style style);


        /// <summary>
        /// Sets the width of the Switch. Default value is 6.
        /// </summary>
        /// <param name="value">The width of the Switch.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than 6.</exception>
        ISwitchWidget Width(byte value);

        /// <summary>
        /// Displays the Switch.
        /// </summary>
        void Show();
    }
}
