// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Globalization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and displaying a read-only slider widget that draws a
    /// numeric value as a horizontal bar, without waiting for user interaction.
    /// </summary>
    /// <remarks>
    /// A widget is meant for display only: unlike <see cref="ISliderControl"/>, it does not read input from the user.
    /// Every configuration method returns the same <see cref="ISliderWidget"/> instance, so the calls can be
    /// chained together (fluent style). Call <see cref="Show"/> last to render the bar on the console.
    /// </remarks>
    public interface ISliderWidget
    {
        /// <summary>
        /// Selects the character set used to draw the slider bar. Default is <see cref="SliderBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The visual style of the bar, one of the <see cref="SliderBarType"/> values.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        ISliderWidget BarType(SliderBarType type);

        /// <summary>
        /// Overrides the colors of a specific region of the slider, such as the answer text or the bar itself.
        /// </summary>
        /// <param name="styleType">The region to restyle, one of the <see cref="SliderStyles"/> values.</param>
        /// <param name="style">The <see cref="Style"/> (colors) to apply to that region. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        ISliderWidget Styles(SliderStyles styleType, Style style);

        /// <summary>
        /// Sets the culture used to format the numeric value (decimal separator, digit grouping, and so on).
        /// Defaults to the current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        ISliderWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture used to format the numeric value from a culture name (for example <c>"en-US"</c> or <c>"pt-BR"</c>).
        /// Defaults to the current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name, for example <c>"en-US"</c>. Cannot be <c>null</c> or empty.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ISliderWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the width of the slider bar, measured in console characters. Default is <c>40</c> and the value must be at least <c>10</c>.
        /// </summary>
        /// <param name="value">The width of the bar, in characters.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <c>10</c>.</exception>
        ISliderWidget Width(byte value);

        /// <summary>
        /// Changes the color of the bar dynamically according to the current value (for example green when high, red when low).
        /// </summary>
        /// <param name="value">A function that receives the current value and returns the <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISliderWidget ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Paints the bar with a gradient that transitions across the supplied colors as the value grows.
        /// </summary>
        /// <param name="colors">The ordered colors used to build the gradient. Cannot be <c>null</c> or empty.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="colors"/> is <c>null</c> or empty.</exception>
        ISliderWidget ChangeGradient(params Color[] colors);


        /// <summary>
        /// Hides one or more visual elements of the slider (such as delimiters or the range display). By default every element is shown.
        /// </summary>
        /// <param name="value">The elements to hide. Combine <see cref="HideSlider"/> values with a bitwise OR.</param>
        /// <returns>The same <see cref="ISliderWidget"/> instance, so additional settings can be chained.</returns>
        ISliderWidget HideElements(HideSlider value);

        /// <summary>
        /// Renders the slider bar on the console using the current configuration. Call this method last.
        /// </summary>
        void Show();
    }
}
