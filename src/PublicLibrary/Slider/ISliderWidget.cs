// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a slider Widget.
    /// </summary>
    public interface ISliderWidget
    {
        /// <summary>
        /// Sets the Graphical-based of slider Bar. Default is <see cref="SliderBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The Graphical-based fill type, <see cref="SliderBarType"/>.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        ISliderWidget Fill(SliderBarType type);

        /// <summary>
        /// Overwrites styles for the  slider Widget.
        /// </summary>
        /// <param name="styleType">The <see cref="SliderStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISliderWidget Styles(SliderStyles styleType, Style style);

        /// <summary>
        /// Sets the culture for displaying numeric values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ISliderWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for displaying numeric values using a culture name. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ISliderWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the width of the SliderBar. Default value is 40. Must be >= 10.
        /// </summary>
        /// <param name="value">The width of the SliderBar.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        ISliderWidget Width(byte value);

        /// <summary>
        /// Dynamically changes the style color of the SliderBar based on its value.
        /// </summary>
        /// <param name="value">A function to ISliderWidget the style based on the current value.</param>
        /// <returns>The current <see cref="ISliderControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISliderWidget ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Dynamically changes the gradient colors of the SliderBar.
        /// </summary>
        /// <param name="colors">The gradient colors to apply.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="colors"/> is <c>null</c> or empty.</exception>
        ISliderWidget ChangeGradient(params Color[] colors);


        /// <summary>
        /// Hides specific elements of the slider. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="ISliderWidget"/> instance for chaining.</returns>
        ISliderWidget HideElements(HideSlider value);

        /// <summary>
        /// Display the slider widget.
        /// </summary>
        void Show();
    }
}
