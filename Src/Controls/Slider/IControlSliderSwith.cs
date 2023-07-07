// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlSliderSwitch : IPromptControls<bool>
    {
        /// <summary>
        /// Default value for swith
        /// </summary>
        /// <param name="value">true is 'on', otherwise 'off'</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch Default(bool value);

        /// <summary>
        /// Text to 'off' value. Default value comes from resource.
        /// </summary>
        /// <param name="value">text off</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch OffValue(string value);

        /// <summary>
        /// Text to 'on' value. Default value comes from resource.
        /// </summary>
        /// <param name="value">text on</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch OnValue(string value);

        /// <summary>
        /// Define Width to Widgets. Default value is 6.
        /// </summary>
        /// <param name="value">Width</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch Width(int value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch Config(Action<IPromptConfig> context);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch ChangeDescription(Func<bool, string> value);

        /// <summary>
        /// Change Color when state 'On'. 
        /// <br>state-On(Foreground)/Background</br>
        /// <br>Default Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Default Background : 'ConsoleColor.DarkGray'</br>
        /// </summary>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch ChangeColorOn(Style value);

        /// <summary>
        /// Change Color when state 'Off'. 
        /// <br>state-Off(Foreground)/Background</br>
        /// <br>Default Foreground : 'ConsoleColor.Cyan'</br>
        /// <br>Default Background : 'ConsoleColor.DarkGray'</br>
        /// </summary>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlSliderSwitch"/></returns>
        IControlSliderSwitch ChangeColorOff(Style value);
    }
}
