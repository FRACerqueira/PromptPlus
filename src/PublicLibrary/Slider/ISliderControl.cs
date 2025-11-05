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
    /// Provides functionality for configuring and interacting with a slider control.
    /// </summary>
    public interface ISliderControl
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ISliderControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the graphical representation of the slider bar. Default is <see cref="SliderBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The graphical fill type, <see cref="SliderBarType"/>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        ISliderControl Fill(SliderBarType type);

        /// <summary>
        /// Overwrites styles for the slider control.
        /// </summary>
        /// <param name="styleType">The <see cref="SliderStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISliderControl Styles(SliderStyles styleType, Style style);

        /// <summary>
        /// Sets the culture for displaying numeric values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ISliderControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for displaying numeric values using a culture name. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ISliderControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Defines the minimum and maximum range values. Default value is 100 for <paramref name="maxvalue"/> and 0 for <paramref name="minvalue"/>.
        /// </summary>
        /// <param name="minvalue">The minimum number.</param>
        /// <param name="maxvalue">The maximum number.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        ISliderControl Range(double minvalue, double maxvalue);

        /// <summary>
        /// Sets the width of the slider bar. Default value is 40. Must be >= 10.
        /// </summary>
        /// <param name="value">The width of the slider bar.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        ISliderControl Width(byte value);

        /// <summary>
        /// Sets the initial value of the slider. Default is 0.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="usedefaultHistory">Indicates whether to use the default value from history (if enabled via <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>).</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than the maximum value or less than the minimum value.</exception>
        ISliderControl Default(double value, bool usedefaultHistory = true);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure the <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ISliderControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the number of fractional digits for the slider value. Default is 0.
        /// </summary>
        /// <param name="value">The number of fractional digits.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than 5.</exception>
        ISliderControl FracionalDig(byte value);

        /// <summary>
        /// Defines the layout for changing values. Default value is <see cref="SliderLayout.LeftRight"/>.
        /// </summary>
        /// <remarks>
        /// When layout equals <see cref="SliderLayout.UpDown"/>, the slider control does not show widgets.
        /// </remarks>
        /// <param name="value">The <see cref="SliderLayout"/>.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        ISliderControl Layout(SliderLayout value);

        /// <summary>
        /// Defines the step value for incremental changes. Default value is 1/100 of the range.
        /// </summary>
        /// <param name="value">The step value.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        ISliderControl Step(double value);

        /// <summary>
        /// Defines the large step value for incremental changes. Default value is 1/10 of the range.
        /// </summary>
        /// <param name="value">The large step value.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        ISliderControl LargeStep(double value);

        /// <summary>
        /// Dynamically changes the style color of the slider bar based on its value.
        /// </summary>
        /// <param name="value">A function to determine the style based on the current value.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISliderControl ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Dynamically changes the gradient colors of the slider bar.
        /// </summary>
        /// <param name="colors">The gradient colors to apply.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="colors"/> is <c>null</c> or empty.</exception>
        ISliderControl ChangeGradient(params Color[] colors);

        /// <summary>
        /// Dynamically changes the description of the slider based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISliderControl ChangeDescription(Func<double, string> value);

        /// <summary>
        /// Hides specific elements of the slider. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        ISliderControl HideElements(HideSlider value);

        /// <summary>
        /// Runs the slider control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the slider control execution.</returns>
        ResultPrompt<double?> Run(CancellationToken token = default);
    }
}
