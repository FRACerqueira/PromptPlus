// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a Date in Calendar
    /// </summary>
    public class ItemCalendar
    {
        private ItemCalendar()
        {
        }

        /// <summary>
        /// Create a instance of day with notes
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="note">The note</param>
        public ItemCalendar(DateTime date, string note= null)
        {
            Date = date.Date;
            Note = (note??string.Empty).Trim();
            Id = date.Date.ToString("s") + note;
        }


        /// <summary>
        /// Get Date
        /// </summary>
        public DateTime  Date { get; }

        /// <summary>
        /// Get note
        /// </summary>
        public string Note { get; }

        internal string Id { get; }


    }
}
