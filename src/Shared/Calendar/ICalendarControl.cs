// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running an interactive monthly calendar control.
    /// </summary>
    /// <remarks>
    /// The control renders a full monthly grid where the user navigates day-by-day, moves between
    /// months and years, and confirms the highlighted date by pressing Enter. Optional features
    /// include a min/max selectable date range, disabled days, weekend blocking, date notes,
    /// date highlighting, and history persistence. Call <see cref="Run(CancellationToken)"/> last
    /// to display the control and read the selected date.
    /// </remarks>
    public interface ICalendarControl
    {
        /// <summary>
        /// Sets the calendar layout style, controlling how dates and grid lines are rendered.
        /// </summary>
        /// <param name="layout">The <see cref="CalendarLayout"/> to set. Default is <see cref="CalendarLayout.SingleGrid"/>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl Layout(CalendarLayout layout = CalendarLayout.SingleGrid);

        /// <summary>
        /// Sets the culture used to display calendar values. The default is the current PromptPlus culture.
        /// </summary>
        /// <remarks>
        /// The culture affects the display of month names, day names, and date formatting.
        /// Changes to culture will be reflected immediately in the calendar display.
        /// </remarks>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ICalendarControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture used for date parsing and validation. The default is the current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name used for parsing and validating dates. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ICalendarControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the first day of the week for the calendar.
        /// </summary>
        /// <param name="firstDayOfWeek">The <see cref="DayOfWeek"/> to set as the first day of the week.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl FirstDayOfWeek(DayOfWeek firstDayOfWeek);

        /// <summary>
        /// Enables or disables weekend date selection in the calendar.
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
        /// Add a note to a specific date in the calendar.
        /// </summary>
        /// <param name="value">The date to add the note to.</param>
        /// <param name="note">The note for the date.if <c>null</c>, an empty string will be used.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl AddNote(DateTime value, string? note = null);

        /// <summary>
        /// Adds notes to specific dates in the calendar.
        /// </summary>
        /// <param name="notes">The notes for calendar. Cannot be <c>null</c>. If a note is <c>null</c>, an empty string will be used.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl AddNotes((DateTime, string?)[] notes);

        /// <summary>
        /// Highlights one or more dates in the calendar.
        /// </summary>
        /// <param name="dates">The dates to highlight. Cannot be <c>null</c>.</param>
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
        /// Defines an inclusive range of valid dates that can be selected in the calendar.
        /// </summary>
        /// <param name="minValue">The minimum date. Must be less than or equal to <paramref name="maxValue"/>.</param>
        /// <param name="maxValue">The maximum date. Must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        ICalendarControl Range(DateTime minValue, DateTime maxValue);

        /// <summary>
        /// Dynamically updates the prompt description using the currently selected date.
        /// </summary>
        /// <param name="value">A function that receives the selected date and returns a description string. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ICalendarControl ChangeDescription(Func<DateTime?, string> value);

        /// <summary>
        /// Dynamically updates the prompt description using the currently selected date through an asynchronous callback.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ICalendarControl ChangeDescriptionAsync(Func<DateTime?, Task<string>> value);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ICalendarControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the initial date for the calendar.
        /// </summary>
        /// <param name="value">The initial <see cref="DateTime"/>. Default is the current date.</param>
        /// <param name="useDefaultHistory">If <c>true</c>, uses the value from history when enabled; otherwise, uses <paramref name="value"/>.</param>
        /// <remarks>
        /// if the provided date is outside the defined range (if any), it will be ignored.
        /// </remarks>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ICalendarControl Default(DateTime value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables history and applies custom configuration to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ICalendarControl EnableHistory(string filename, Action<IHistoryOptions>? options = null);


        /// <summary>
        /// Sets the maximum number of notes displayed per page. The default value is 0.
        /// Valid range is 0-255.
        /// </summary>
        /// <remarks>
        /// A value of 0 automatically computes the page size based on screen height, reserving lines for header, footer, and pagination.
        /// If the provided value exceeds the available screen height (minus reserved lines), it is coerced to the maximum allowed value.
        /// </remarks>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <param name="value">The maximum number of items per page.</param>
        ICalendarControl PageSize(byte value);

        /// <summary>
        /// Executes a synchronous interaction for each item in the collection, allowing custom calendar configuration per item.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="interactionAction">The action executed for each item to configure the calendar control.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ICalendarControl Interaction<T>(IEnumerable<T> items, Action<T, ICalendarControl> interactionAction);

        /// <summary>
        /// Executes an asynchronous interaction for each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The asynchronous action executed for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ICalendarControl InteractionAsync<T>(IEnumerable<T> items, Func<T, ICalendarControl, Task> interactionAction);

        /// <summary>
        /// Sets a validation predicate to determine whether the selected date is valid.
        /// </summary>
        /// <param name="isValidSelection">A synchronous predicate that returns <c>true</c> when the selected date is valid.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl PredicateSelected(Func<DateTime?, bool> isValidSelection);

        /// <summary>
        /// Sets an asynchronous validation predicate to determine whether the selected date is valid.
        /// </summary>
        /// <param name="isValidSelection">An asynchronous predicate that returns <c>true</c> when the selected date is valid.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<bool>> isValidSelection);

        /// <summary>
        /// Sets a validation predicate that determines whether the selected date is valid and returns an optional error message.
        /// </summary>
        /// <param name="validateSelection">A synchronous predicate that returns a tuple where the first value indicates validity and the second is an optional error message.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        ICalendarControl PredicateSelected(Func<DateTime?, (bool, string?)> validateSelection);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected date is valid and returns an optional error message.
        /// </summary>
        /// <param name="validateSelection">An asynchronous predicate that returns a tuple where the first value indicates validity and the second is an optional error message.</param>
        /// <returns>The current <see cref="ICalendarControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<(bool, string?)>> validateSelection);

        /// <summary>
        /// Displays the calendar control and blocks until the user confirms or cancels,
        /// returning the selected date.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected <see cref="DateTime"/>, or an aborted result if cancelled.</returns>
        ResultPrompt<DateTime?> Run(CancellationToken token = default);
    }
}
