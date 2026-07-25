// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and displaying a slider control that lets the user
    /// pick a numeric value by moving a bar between a minimum and a maximum limit.
    /// </summary>
    /// <remarks>
    /// Every configuration method returns the same <see cref="ISliderControl"/> instance, so the calls can be
    /// chained together (fluent style). Call <see cref="Run(CancellationToken)"/> last to display the control
    /// and read the value chosen by the user.
    /// </remarks>
    public interface ISliderControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and validation).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        ISliderControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Selects the character set used to draw the slider bar. Default is <see cref="SliderBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The visual style of the bar, one of the <see cref="SliderBarType"/> values.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        ISliderControl BarType(SliderBarType type);

        /// <summary>
        /// Overrides the colors of a specific region of the slider, such as the prompt, the answer or the bar itself.
        /// </summary>
        /// <param name="styleType">The region to restyle, one of the <see cref="SliderStyles"/> values.</param>
        /// <param name="style">The <see cref="Style"/> (colors) to apply to that region. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        ISliderControl Styles(SliderStyles styleType, Style style);

        /// <summary>
        /// Sets the culture used to format the numeric value (decimal separator, digit grouping, and so on).
        /// Defaults to the current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        ISliderControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture used to format the numeric value from a culture name (for example <c>"en-US"</c> or <c>"pt-BR"</c>).
        /// Defaults to the current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name, for example <c>"en-US"</c>. Cannot be <c>null</c> or empty.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ISliderControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Defines the lower and upper limits the slider can reach. Defaults to <c>0</c> for <paramref name="minvalue"/>
        /// and <c>100</c> for <paramref name="maxvalue"/>.
        /// </summary>
        /// <param name="minvalue">The smallest value the user can select.</param>
        /// <param name="maxvalue">The largest value the user can select.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        ISliderControl Range(double minvalue, double maxvalue);

        /// <summary>
        /// Sets the width of the slider bar, measured in console characters. Default is <c>30</c> and the value must be at bet <c>10</c> and <c>100</c>.
        /// </summary>
        /// <param name="value">The width of the bar, in characters.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <c>10</c> or greater than <c>100</c>.</exception>
        ISliderControl Width(byte value);

        /// <summary>
        /// Sets the value that is pre-selected when the slider is first shown. Default is <c>0</c>.
        /// </summary>
        /// <param name="value">The initial value. It must be inside the range defined by <see cref="Range(double, double)"/>.</param>
        /// <param name="useDefaultHistory">When <c>true</c> and history is enabled via <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>, the last saved value is used instead of <paramref name="value"/>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the minimum/maximum range.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ISliderControl Default(double value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables value history, persisting the chosen value to a file so it can be reused as the default on the next run.
        /// </summary>
        /// <param name="filename">The file name used to store the history. Cannot be <c>null</c>.</param>
        /// <param name="options">An optional callback to configure the <see cref="IHistoryOptions"/> (such as expiration).</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        ISliderControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets how many decimal places are shown for the slider value. Default is <c>0</c> (whole numbers).
        /// </summary>
        /// <param name="value">The number of fractional digits, from <c>0</c> to <c>5</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is greater than <c>5</c>.</exception>
        ISliderControl FractionalDigits(byte value);

        /// <summary>
        /// Chooses how the user changes the value and how the control is drawn. Default is <see cref="SliderLayout.LeftRight"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="SliderLayout.LeftRight"/> uses the Left/Right arrows and shows the bar, while
        /// <see cref="SliderLayout.UpDown"/> uses the Up/Down arrows, hides the bar and does not show widgets.
        /// </remarks>
        /// <param name="value">The layout to use, one of the <see cref="SliderLayout"/> values.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        ISliderControl Layout(SliderLayout value);

        /// <summary>
        /// Sets the amount added or removed on each small change (arrow keys). Default is 1/100 of the range.
        /// </summary>
        /// <param name="value">The increment applied on a small step.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ISliderControl Step(double value);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the amount added or removed on each large change (for example Page Up/Page Down). Default is 1/10 of the range.
        /// </summary>
        /// <param name="value">The increment applied on a large step.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        ISliderControl LargeStep(double value);

        /// <summary>
        /// Changes the color of the bar dynamically according to the current value (for example green when high, red when low).
        /// </summary>
        /// <param name="value">A function that receives the current value and returns the <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISliderControl ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Paints the bar with a gradient that transitions across the supplied colors as the value grows.
        /// </summary>
        /// <param name="colors">The ordered colors used to build the gradient. Cannot be <c>null</c> or empty.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="colors"/> is <c>null</c> or empty.</exception>
        ISliderControl ChangeGradient(params Color[] colors);

        /// <summary>
        /// Updates the description text shown with the slider according to the current value.
        /// </summary>
        /// <param name="value">A function that receives the current value and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISliderControl ChangeDescription(Func<double, string> value);

        /// <summary>
        /// Asynchronous version of <see cref="ChangeDescription(Func{double, string})"/> that updates the description
        /// text according to the current value (useful when the text comes from an asynchronous source).
        /// </summary>
        /// <param name="value">A function that receives the current value and asynchronously returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISliderControl  ChangeDescriptionAsync(Func<double, Task<string>> value);

        /// <summary>
        /// Hides one or more visual elements of the slider (such as delimiters or the range display). By default every element is shown.
        /// </summary>
        /// <param name="value">The elements to hide. Combine <see cref="HideSlider"/> values with a bitwise OR.</param>
        /// <returns>The same <see cref="ISliderControl"/> instance, so additional settings can be chained.</returns>
        ISliderControl HideElements(HideSlider value);

        /// <summary>
        /// Displays the slider and blocks until the user confirms or cancels, returning the selected value.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the prompt while it is waiting for input. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the chosen value as a <see cref="double"/>, or a <c>null</c> value when the prompt is cancelled.</returns>
        ResultPrompt<double?> Run(CancellationToken token = default);
    }
}
