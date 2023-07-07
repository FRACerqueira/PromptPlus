// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlProgressBar<T> :IPromptControls<ResultProgessBar<T>>
    {
        /// <summary>
        /// Hide elements progress bar Widgets. Default is Show all elements
        /// <br>For more one element use | separate (Enum Flag)</br>
        /// </summary>
        /// <param name="value">element to hide. <see cref="HideProgressBar"/></param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> HideElements(HideProgressBar value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// <see cref="CultureInfo"/> to on show value format.
        /// </summary>
        /// <param name="value">CultureInfo to use</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Culture(CultureInfo value);

        /// <summary>
        /// Set <see cref="char"/> to show progress.Default value '#'
        /// <br>Valid on ProgressBarType.Char, otherwise is ignored </br>
        /// </summary>
        /// <param name="value">Char to show</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> CharBar(char value);

        /// <summary>
        /// <see cref="CultureInfo"/> to show value format.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Culture(string value);

        /// <summary>
        /// Finish answer to show when progressbar is completed.
        /// </summary>
        /// <param name="text">text Finish answer</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Finish(string text);

        /// <summary>
        /// Overwrite <see cref="SpinnersType"/>. Default value is SpinnersType.Ascii
        /// <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected</br>
        /// </summary>
        /// <param name="spinnersType">Spinners Type</param>
        /// <param name="SpinnerStyle">Style of spinner. <see cref="Style"/></param>
        /// <param name="speedAnimation">Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <param name="customspinner">IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Spinner(SpinnersType spinnersType, Style? SpinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null);

        /// <summary>
        /// Define Width to Widgets. Default value is 80.
        /// </summary>
        /// <param name="value">Width</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Width(int value);

        /// <summary>
        /// Initial value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> Default(double value);

        /// <summary>
        /// Define the Fracional Digits of value. Default is 0.
        /// </summary>
        /// <param name="value">Fracional Digits</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> FracionalDig(int value);

        /// <summary>
        /// Dynamically change Style Widgets
        /// </summary>
        /// <param name="value">function to change color</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Dynamically Change Gradient color Widgets
        /// </summary>
        /// <param name="colors">list of colors Gradient</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> ChangeGradient(params Color[] colors);

        /// <summary>
        /// Handler to execute Update ProgressBar values.
        /// </summary>
        /// <param name="value">Handler.See <see cref="UpdateProgressBar{t}"/> to change value</param>
        /// <returns><see cref="IControlProgressBar{T}"/></returns>
        IControlProgressBar<T> UpdateHandler(Action<UpdateProgressBar<T>, CancellationToken> value);

    }
}
