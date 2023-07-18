// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the SliderNumber control
    /// </summary>
    public interface IControlSliderNumber : IPromptControls<double>
    {
        /// <summary>
        /// <see cref="CultureInfo"/> to validate input value format.
        /// </summary>
        /// <param name="value">CultureInfo to use on validate</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to validate input when the type is not generic.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use on validate</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Culture(string value);

        /// <summary>
        /// Initial value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Default(double value);

        /// <summary>
        /// Define the KeyPress to change value. Default value is Left or Right.
        /// <br>When MoveKeyPress equal Up or Down , slider control not show Widgets</br>
        /// </summary>
        /// <param name="value">Left/Right or Up/Down. <see cref="SliderNumberType"/></param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber MoveKeyPress(SliderNumberType value);

        /// <summary>
        /// Define Width to Widgets. Default value is 40.
        /// </summary>
        /// <param name="value">Width</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Width(int value);

        /// <summary>
        /// Define type Bar to Slider.
        /// <br>Valid only When MoveKeyPress equal left/right mode, otherwise its is ignored</br>
        /// </summary>
        /// <param name="value">The <see cref="SliderBarType"/>. Default value 'SliderBarType.Fill'</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber BarType(SliderBarType value);

        /// <summary>
        /// Defines a minimum and maximum range values
        /// </summary>
        /// <param name="minvalue">Minimum number</param>
        /// <param name="maxvalue">Maximum number</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Range(double minvalue, double maxvalue);

        /// <summary>
        /// Define the short step to change. Default value is 1/100 of range
        /// </summary>
        /// <param name="value">short step to change</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Step(double value);

        /// <summary>
        /// Define the large step to change. Default value is 1/10 of range
        /// </summary>
        /// <param name="value">short step to change</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber LargeStep(double value);

        /// <summary>
        /// Define the Fracional Digits of value. Default is 0.
        /// </summary>
        /// <param name="value">Fracional Digits</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber FracionalDig(int value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber Config(Action<IPromptConfig> context);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber ChangeDescription(Func<double, string> value);

        /// <summary>
        /// Dynamically change color Widgets
        /// <br>Valid only When MoveKeyPress equal left/right mode, otherwise its is ignored</br>
        /// </summary>
        /// <param name="value">function to change color</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber ChangeColor(Func<double, Color> value);

        /// <summary>
        /// Dynamically Change Gradient color Widgets
        /// <br>Valid only When MoveKeyPress equal left/right mode, otherwise its is ignored</br>
        /// </summary>
        /// <param name="colors">list of colors Gradient</param>
        /// <returns><see cref="IControlSliderNumber"/></returns>
        IControlSliderNumber ChangeGradient(params Color[] colors);

    }
}
