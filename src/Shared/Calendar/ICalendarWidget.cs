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
    /// Provides a fluent API for configuring and rendering a read-only monthly calendar widget.
    /// </summary>
    /// <remarks>
    /// A widget is for display only: unlike <see cref="ICalendarControl"/>, it does not
    /// accept user input or return a selected date. Call <see cref="Show"/> last to
    /// render the calendar on the console.
    /// </remarks>
    public interface ICalendarWidget
    {
        /// <summary>
        /// Sets the visual layout of the calendar grid. Default is <see cref="CalendarLayout.SingleGrid"/>.
        /// </summary>
        /// <param name="layout">The <see cref="CalendarLayout"/> to use.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        ICalendarWidget Layout(CalendarLayout layout = CalendarLayout.SingleGrid);

        /// <summary>
        /// Disables specific dates in the calendar.
        /// </summary>
        /// <param name="dates">The dates to disable. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dates"/> is <c>null</c>.</exception>
        ICalendarWidget DisableDates(params DateTime[] dates);

        /// <summary>
        /// Highlights one or more dates in the calendar.
        /// </summary>
        /// <param name="dates">The dates to highlight. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dates"/> is <c>null</c>.</exception>
        ICalendarWidget Highlights(params DateTime[] dates);

        /// <summary>
        /// Sets the culture used for displaying calendar values such as month names, weekday names, and number formats.
        /// </summary>
        /// <param name="culture">The culture information to use for localization. Cannot be null.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the culture parameter is null.</exception>
        /// <remarks>If not set, the widget will use the current PromptPlus culture settings.</remarks>
        ICalendarWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and format date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ICalendarWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets which day should appear as the first day of each week in the calendar display.
        /// </summary>
        /// <param name="firstDayOfWeek">The day to use as the start of each week.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <remarks>This affects the layout of days in the calendar grid.</remarks>
        ICalendarWidget FirstDayOfWeek(DayOfWeek firstDayOfWeek);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the calendar widget.
        /// </summary>
        /// <param name="styleType">The <see cref="CalendarStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ICalendarWidget Styles(CalendarStyles styleType, Style style);

        /// <summary>
        /// Renders the calendar widget on the console using the current configuration.
        /// Call this method last.
        /// </summary>
        void Show();
    }
}
