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
        /// <typeparam name="T1">Type external colletion</typeparam>
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
        /// Initial date to show.Default value is current date.
        /// </summary>
        /// <param name="value"><see cref="DateTime"/></param>
        /// <param name="nextdateifdisabled">Policy to next/previous date if seleted date is disabled</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar Default(DateTime value,bool nextdateifdisabled = true);

        /// <summary>
        /// Disabled Change day.
        /// </summary>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar DisabledChangeDay();

        /// <summary>
        /// Disabled Change month.
        /// </summary>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar DisabledChangeMonth();

        /// <summary>
        /// Disabled Change year.
        /// </summary>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar DisabledChangeYear();

        /// <summary>
        /// Disabled Weekends.
        /// </summary>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar DisabledWeekends();
        
        /// <summary>
        /// Add Notes in current month/year.
        /// <br>This function is triggered every month/year change</br>
        /// </summary>
        /// <param name="value"> The function with params year and month. Return <see cref="ItemCalendar"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar AddNotes(Func<int, int, ItemCalendar[]> value);

        /// <summary>
        /// Add Notes in current month/year with Highlight style.
        /// <br>This function is triggered every month/year change</br>
        /// </summary>
        /// <param name="value"> The function with params year and month. Return <see cref="ItemCalendar"/></param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar AddNotesHighlight(Func<int, int, ItemCalendar[]>? value);

        /// <summary>
        /// Add Disabled days in current month/year.
        /// <br>This function is triggered every month/year change</br>
        /// </summary>
        /// <param name="value"> The function with params year and month. Return Enumerable of disabled days</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar AddDisabled(Func<int,int, int[]>? value);


        /// <summary>
        /// Range of valid month.
        /// </summary>
        ///<param name="min">Min. valid month</param>
        ///<param name="max">Max. valid month</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar RangeMonth(int min, int max);

        /// <summary>
        /// Range of valid year.
        /// </summary>
        ///<param name="min">Min. valid year</param>
        ///<param name="max">Max. valid year</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar RangeYear(int min, int max);

        /// <summary>
        /// Overwrite a HotKey to show/hide notes of day. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to show/hide notes of day</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar HotKeySwitchNotes(HotKey value);

        /// <summary>
        /// Set max.item view per page on notes.Default value for this control is 5.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar NotesPageSize(int value);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlCalendar"/></returns>
        IControlCalendar OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embeding</br>
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
