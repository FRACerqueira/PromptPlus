// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a WaitTimer control.
    /// </summary>
    public interface IWaitTimerControl
    {

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IWaitTimerControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the WaitTimer.
        /// </summary>
        /// <param name="styleType">The <see cref="WaitTimerStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IWaitTimerControl Styles(WaitTimerStyles styleType, Style style);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        IWaitTimerControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Finish answer to show when Wait timer is completed.
        /// </summary>
        /// <param name="text">Text Finish answer</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        IWaitTimerControl Finish(string text);

        /// <summary>
        /// Define if show Elapsed Time .Default 500ms and true.
        /// </summary>
        /// <param name="mileseconds">The interval to show ElapsedTime.</param>
        /// <param name="value">If show Elapsed Time.</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="mileseconds"/> for less than 100 or greater than 1000.</exception>
        IWaitTimerControl ShowElapsedTime(int mileseconds = 500, bool value = true);


        /// <summary>
        /// Define if show the remaining time .Default true.
        /// </summary>
        /// <param name="value">IF true shows the remaining time</param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        IWaitTimerControl IsCountDown(bool value = true);

        /// <summary>
        /// Runs the WaitTimer control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the WaitTimer control execution. </returns>
        ResultPrompt<TimeSpan?> Run(CancellationToken token = default);
    }
}
