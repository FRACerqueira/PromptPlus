// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a Date in Calendar
    /// </summary>
    public class ItemCalendar
    {
        private readonly List<string> _notes;
        private ItemCalendar()
        {
        }

        /// <summary>
        /// create a instance of day with notes
        /// </summary>
        public ItemCalendar(int day, int month,int year, params string[] notes)
        {
            Day = day;
            Date = new DateTime(year,month,day);
            _notes = new();
            if (notes != null)
            {
                _notes.AddRange(notes.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        /// <summary>
        /// create a instance of day with notes
        /// </summary>
        public ItemCalendar(DateTime date, params string[] notes)
        {
            Day = date.Day;
            Date = date;
            _notes = new();
            if (notes != null)
            {
                _notes.AddRange(notes.Where(x => !string.IsNullOrEmpty(x)));
            }
        }


        /// <summary>
        /// Get/Set Date
        /// </summary>
        public int Day { get; }
        /// <summary>
        /// Get/Set notes of Date
        /// </summary>
        public IEnumerable<string> Notes => _notes;

        internal DateTime  Date { get; }

        /// <summary>
        /// Add note
        /// </summary>
        /// <param name="value"></param>
        public void AddNote(string value)
        { 
            _notes.Add(value);  
        }

    }
}
