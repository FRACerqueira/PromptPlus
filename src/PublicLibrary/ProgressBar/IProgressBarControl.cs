// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Interface for configuring and interacting with a Progress Bar control.
    /// </summary>
    public interface IProgressBarControl
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IProgressBarControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the Graphical-based of Progress Bar. Default is <see cref="ProgressBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The Graphical-based fill type, <see cref="ProgressBarType"/>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Fill(ProgressBarType type);

        /// <summary>
        /// Overwrites styles for the Progress Bar Control.
        /// </summary>
        /// <param name="styleType">The <see cref="ProgressBarStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IProgressBarControl Styles(ProgressBarStyles styleType, Style style);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Sets the culture for displaying numeric values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        IProgressBarControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for displaying numeric values using a culture name. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        /// <exception cref="CultureNotFoundException">Thrown when the specified culture name is not valid.</exception>
        IProgressBarControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Defines a minimum and maximum range values. Default value is 100 for <paramref name="maxvalue"/> and 0 for <paramref name="minvalue"/>.
        /// </summary>
        /// <param name="minvalue">Minimum number</param>
        /// <param name="maxvalue">Maximum number</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        IProgressBarControl Range(double minvalue, double maxvalue);

        /// <summary>
        /// Sets the width of the ProgressBar. Default value is 80. Must be >= 10.
        /// </summary>
        /// <param name="value">The width of the ProgressBar.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        IProgressBarControl Width(byte value);

        /// <summary>
        /// Sets the initial value of the ProgressBar. Default is 0.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than maximum value or <paramref name="value"/> is less than minimum value.</exception>
        IProgressBarControl Default(double value);

        /// <summary>
        /// Sets the fractional digits for the ProgressBar value. Default is 0.
        /// </summary>
        /// <param name="value">The number of fractional digits.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than 5.</exception>
        IProgressBarControl FracionalDig(byte value);

        /// <summary>
        /// Dynamically changes the style color of the ProgressBar based on its value.
        /// </summary>
        /// <param name="value">A function to determine the style based on the current value.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Dynamically changes the gradient colors of the ProgressBar.
        /// </summary>
        /// <param name="colors">The gradient colors to apply.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="colors"/> is <c>null</c> or empty.</exception>
        IProgressBarControl ChangeGradient(params Color[] colors);

        /// <summary>
        /// Dynamically changes the description of the ProgressBar based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl ChangeDescription(Func<double, string> value);

        /// <summary>
        /// Sets the text to display when the ProgressBar is completed.
        /// </summary>
        /// <param name="finishtext">The text to display.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Finish(string finishtext);

        /// <summary>
        /// Sets a handler to update the ProgressBar values dynamically.
        /// </summary>
        /// <param name="value">The handler to update values. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl UpdateHandler(Action<HandlerProgressBar, CancellationToken> value);

        /// <summary>
        /// Hides specific elements of the ProgressBar. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl HideElements(HideProgressBar value);

        /// <summary>
        /// Define the interval to update Progressbar status and Spinner. Default 100ms.
        /// </summary>
        /// <param name="milliseconds">The interval in milliseconds to update the ProgressBar status and Spinner.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="milliseconds"/> is less than 100 or greater than 1000.</exception>
        IProgressBarControl IntervalUpdate(int milliseconds = 100);


        /// <summary>
        /// Runs the ProgressBar control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> containing the final state <see cref="StateProgress"/> of the ProgressBar control.</returns>
        ResultPrompt<StateProgress> Run(CancellationToken token = default);
    }
}
