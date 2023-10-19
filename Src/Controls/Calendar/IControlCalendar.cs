// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Calendar control
    /// </summary>
    public interface IControlCalendar : IPromptControls<DateTime>
    {
        /// <summary>
        /// The layout canlendar. Default value is 'CalendarLayout.SingleBorde'
        /// </summary>
        /// <param name="value">The <see cref="CalendarLayout"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Layout(CalendarLayout value);

        /// <summary>
        /// <see cref="CultureInfo"/> to on show value format.
        /// </summary>
        /// <param name="value">CultureInfo to use</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to show value format.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Culture(string value);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Layout external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Interaction<T1>(IEnumerable<T1> values, Action<IControlCalendar, T1> action);

        /// <summary>
        /// Styles for Calendar content
        /// </summary>
        /// <param name="styletype"><see cref="StyleCalendar"/> of content</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Styles(StyleCalendar styletype, Style value);

        /// <summary>
        /// Initial date.Default value is current date.
        /// </summary>
        /// <param name="value"><see cref="DateTime"/></param>
        /// <param name="policy">
        /// Policy to next/previous valid date if selected date is invalid
        /// </param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Default(DateTime value, PolicyInvalidDate policy = PolicyInvalidDate.NextDate);



        /// <summary>
        /// Defines a minimum and maximum range date
        /// </summary>
        /// <param name="minvalue">Minimum date</param>
        /// <param name="maxvalue">Maximum date</param>
        /// <returns><see cref="IControlCalendar"/></returns>;
        IControlCalendar Range(DateTime minvalue, DateTime maxvalue);

        /// <summary>
        /// Disabled Weekends.Default false;
        /// </summary>
        /// <param name="value">Disabled weekends</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar DisabledWeekends(bool value = true);

        /// <summary>
        /// Add scope(Note/Highlight/Disabled) items to calendar. 
        /// </summary>
        /// <param name="scope">The <see cref="CalendarScope"/> of item</param>
        /// <param name="values">The <see cref="ItemCalendar"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar AddItems(CalendarScope scope, params ItemCalendar[] values);

 
        /// <summary>
        /// Overwrite a HotKey to show/hide notes of day. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to show/hide notes of day</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar HotKeySwitchNotes(HotKey value);

        /// <summary>
        /// Set max.item notes view per page.
        /// <br>Default value : 5.The value must be greater than or equal to 1</br>
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar PageSize(int value);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embedding</br>
        /// </summary>
        /// <param name="validators">the function validator. <see cref="ValidationResult"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar AddValidators(params Func<object, ValidationResult>[] validators);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar ChangeDescription(Func<DateTime, string> value);


        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">Action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Config(Action<IPromptConfig> context);
    }
}
