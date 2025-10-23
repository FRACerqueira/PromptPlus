// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a calendar widget.
    /// </summary>
    public interface ICalendarWidget
    {
        /// <summary>
        /// Sets the layout of the calendar.
        /// </summary>
        /// <param name="layout">The <see cref="CalendarLayout"/> to set. Default is <see cref="CalendarLayout.SingleGrid"/>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        ICalendarWidget Layout(CalendarLayout layout = CalendarLayout.SingleGrid);

        /// <summary>
        /// Sets the culture for displaying calendar values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        ICalendarWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and format date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        ICalendarWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the first day of the week for the calendar.
        /// </summary>
        /// <param name="firstDayOfWeek">The <see cref="DayOfWeek"/> to set as the first day of the week.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        ICalendarWidget FirstDayOfWeek(DayOfWeek firstDayOfWeek);

        /// <summary>
        /// Overwrites styles for the calendar.
        /// </summary>
        /// <param name="styleType">The <see cref="CalendarStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ICalendarWidget Styles(CalendarStyles styleType, Style style);

        /// <summary>
        /// Display the Calendar widget.
        /// </summary>
        void Show();
    }
}
