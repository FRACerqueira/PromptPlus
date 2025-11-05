// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a Switch control.
    /// </summary>
    public interface ISwitchControl
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ISwitchControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Switch control.
        /// </summary>
        /// <param name="styleType">The <see cref="SwitchStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISwitchControl Styles(SwitchStyles styleType, Style style);


        /// <summary>
        /// Sets the width of the Switch. Default value is 6.
        /// </summary>
        /// <param name="value">The width of the Switch.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 6.</exception>
        ISwitchControl Width(byte value);

        /// <summary>
        /// Sets the initial value of the Switch. Default is false (off).
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="usedefaultHistory">Indicates whether to use the default value from history (if enabled with <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>).</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl Default(bool value, bool usedefaultHistory = true);

        /// <summary>
        /// Sets the text for the 'off' value. Default value comes from resource.
        /// </summary>
        /// <param name="value">The text for the off state.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OffValue(string value);

        /// <summary>
        /// Sets the text for the 'on' value. Default value comes from resource.
        /// </summary>
        /// <param name="value">The text for the on state.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OnValue(string value);

        /// <summary>
        /// Enables history and applies custom options to the History feature. 
        /// </summary>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure the <see cref="IHistoryOptions"/>. Can be <c>null</c>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ISwitchControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Dynamically changes the description of the Switch based on its current value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISwitchControl ChangeDescription(Func<bool, string> value);

        /// <summary>
        /// Runs the Switch control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the Switch control execution.</returns>
        ResultPrompt<bool?> Run(CancellationToken token = default);
    }
}
