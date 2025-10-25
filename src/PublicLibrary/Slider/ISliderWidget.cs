// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a slider widget.
    /// </summary>
    public interface ISliderWidget
    {
        /// <summary>
        /// Sets the graphical style of the slider bar. Default is <see cref="SliderBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The <see cref="SliderBarType"/> to apply.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        ISliderWidget Fill(SliderBarType type);

        /// <summary>
        /// Overwrites styles for the slider widget.
        /// </summary>
        /// <param name="styleType">The <see cref="SliderStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISliderWidget Styles(SliderStyles styleType, Style style);

        /// <summary>
        /// Sets the culture for displaying numeric values. Default value is the current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ISliderWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for displaying numeric values using a culture name. Default value is the current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ISliderWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the width of the slider bar. Default value is 40. Must be greater than or equal to 10.
        /// </summary>
        /// <param name="value">The width of the slider bar.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        ISliderWidget Width(byte value);

        /// <summary>
        /// Dynamically changes the style color of the slider bar based on its value.
        /// </summary>
        /// <param name="value">A function that determines the <see cref="Style"/> based on the current slider value.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISliderWidget ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Dynamically changes the gradient colors of the slider bar.
        /// </summary>
        /// <param name="colors">The gradient colors to apply. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="colors"/> is <c>null</c> or empty.</exception>
        ISliderWidget ChangeGradient(params Color[] colors);


        /// <summary>
        /// Hides specific elements of the slider. Default is to show all elements.
        /// </summary>
        /// <param name="value">The <see cref="HideSlider"/> elements to hide.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        ISliderWidget HideElements(HideSlider value);

        /// <summary>
        /// Displays the slider widget.
        /// </summary>
        void Show();
    }
}
