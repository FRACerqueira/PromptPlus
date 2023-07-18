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
    internal class CalendarOptions : BaseOptions
    {
        private CalendarOptions()
        {
            throw new PromptPlusException("CalendarOptions CTOR NotImplemented");
        }

        internal CalendarOptions(bool showcursor) : base(showcursor)
        {
        }

        public CultureInfo CurrentCulture { get; set; } = null;
        public bool DefaultNextdateifdisabled { get; set; } = true;
        public Style DescriptionStyle { get; set; } = PromptPlus.StyleSchema.Description();
        public Style GridStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style DisabledStyle { get; set; } = PromptPlus.StyleSchema.Disabled();
        public Style SelectedStyle { get; set; } = PromptPlus.StyleSchema.Selected();
        public Style HighlightStyle { get; set; } = PromptPlus.StyleSchema.TaggedInfo();
        public Style DayStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style MonthStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style YearStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style WeekDayStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public bool DisabledChangeDay { get; set; }
        public bool DisabledChangeMonth { get; set; }
        public bool DisabledChangeYear { get; set; }
        public bool DisabledToday { get; set; }
        public bool DisabledWeekend { get; set; }
        public Func<int, int, IEnumerable<int>> FuncDisabledDays { get; set; }
        public Func<int, int, IEnumerable<ItemCalendar>> FuncNotesDays { get; set; }
        public Func<int, int, IEnumerable<ItemCalendar>> FuncHighlightDays { get; set; }
        public int MinMonth { get; set; } = 1;
        public int MaxMonth { get; set; } = 12;
        public int MinYear { get; set; } = DateTime.MinValue.Year;
        public int MaxYear { get; set; } = DateTime.MaxValue.Year;
        public HotKey SwitchNotes { get; set; } = PromptPlus.Config.CalendarSwitchNotesPress;
        public int PageSize { get; set; } = 5;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<DateTime, string> ChangeDescription { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;

        internal bool ShowingNotes { get; set; }
        internal DayOfWeek FirstWeekDay { get; set; } = DayOfWeek.Sunday;

    }
}
