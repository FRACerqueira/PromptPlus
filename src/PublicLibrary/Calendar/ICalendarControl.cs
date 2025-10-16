// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a calendar control.
    /// </summary>
    public interface ICalendarControl
    {
        /// <summary>
        /// Sets the layout of the calendar.
        /// </summary>
        /// <param name="layout">The <see cref="CalendarLayout"/> to set. Default is <see cref="CalendarLayout.SingleGrid"/>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl Layout(CalendarLayout layout = CalendarLayout.SingleGrid);

        /// <summary>
        /// Sets the culture for displaying calendar values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ICalendarControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and format date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ICalendarControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the first day of the week for the calendar.
        /// </summary>
        /// <param name="firstDayOfWeek">The <see cref="DayOfWeek"/> to set as the first day of the week.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl FirstDayOfWeek(DayOfWeek firstDayOfWeek);

        /// <summary>
        /// Enables or disables weekend days in the calendar.
        /// </summary>
        /// <param name="value">If <c>true</c>, weekends are disabled; otherwise, they are enabled. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl DisabledWeekend(bool value = true);

        /// <summary>
        /// Disables specific dates in the calendar.
        /// </summary>
        /// <param name="dates">The dates to disable. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dates"/> is <c>null</c>.</exception>
        ICalendarControl DisableDates(params DateTime[] dates);


        /// <summary>
        /// Adds a note for a specific date in the calendar.
        /// The <paramref name="note"/> will be ignored for Widget.
        /// </summary>
        /// <param name="value">The date to add the note to.</param>
        /// <param name="note">The note for the date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="note"/> is <c>null</c> or empty.</exception>
        ICalendarControl AddNote(DateTime value, string? note = null);

        /// <summary>
        /// Highlights a specific date in the calendar.
        /// </summary>
        /// <param name="dates">The dates to highlight.Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dates"/> is <c>null</c>.</exception>
        ICalendarControl Highlights(params DateTime[] dates);

        /// <summary>
        /// Overwrites styles for the calendar.
        /// </summary>
        /// <param name="styleType">The <see cref="CalendarStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ICalendarControl Styles(CalendarStyles styleType, Style style);

        /// <summary>
        /// Defines a minimum and maximum valid range of dates for the calendar.
        /// Will be ignored for Widget.
        /// </summary>
        /// <param name="minValue">The minimum date. Must be less than or equal to <paramref name="maxValue"/>.</param>
        /// <param name="maxValue">The maximum date. Must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        ICalendarControl Range(DateTime minValue, DateTime maxValue);

        /// <summary>
        /// Dynamically changes the description using a user-defined function.
        /// Will be ignored for Widget.
        /// </summary>
        /// <param name="value">A function that takes a <see cref="DateTime"/> and returns a description string. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ICalendarControl ChangeDescription(Func<DateTime?, string> value);

        /// <summary>
        /// Applies custom options to the control.
        /// Will be ignored for Widget.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ICalendarControl Options(Action<IControlOptions> options);


        /// <summary>
        /// Sets the initial date for the calendar.
        /// </summary>
        /// <param name="value">The initial <see cref="DateTime"/>. Default is the current date.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl Default(DateTime value);

        /// <summary>
        /// Sets the maximum number of notes displayed per page.
        /// Will be ignored for Widget.
        /// </summary>
        /// <param name="value">The maximum number of notes. Default is 5.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        ICalendarControl PageSize(byte value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="items">The collection.</param>
        /// <param name="interactionaction">The interaction action.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionaction"/> is <c>null</c>.</exception>
        ICalendarControl Interaction<T>(IEnumerable<T> items, Action<T, ICalendarControl> interactionaction);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl PredicateSelected(Func<DateTime?, bool> validselect);

        /// <summary>
        /// Runs the Calendar control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the Calendar control execution. </returns>
        ResultPrompt<DateTime?> Run(CancellationToken token = default);
    }
}
