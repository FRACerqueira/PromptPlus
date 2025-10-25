// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides methods for configuring and displaying a lightweight calendar widget, including layout customization, 
    /// culture settings, and visual styling options.
    /// </summary>
    public interface ICalendarWidget
    {
        /// <summary>
        /// Sets the visual layout style of the calendar widget.
        /// </summary>
        /// <param name="layout">The layout style to use. Defaults to SingleGrid.</param>
        /// <returns>The current <see cref="ICalendarWidget"/> instance for chaining.</returns>
        ICalendarWidget Layout(CalendarLayout layout = CalendarLayout.SingleGrid);

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
