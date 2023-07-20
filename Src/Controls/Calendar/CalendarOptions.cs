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
            ItemsDisabled = new();
            ItemsNotes = new();
            Itemshighlight = new();
        }
        public LayoutCalendar Layout { get; set; } =  LayoutCalendar.SingleGrid;
        public CultureInfo CurrentCulture { get; set; } = null;
        public PolicyInvalidDate PolicyInvalidDate { get; set; } = PolicyInvalidDate.NextDate;
        public Style DescriptionStyle { get; set; } = PromptPlus.StyleSchema.Description();
        public Style LineStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style DisabledStyle { get; set; } = PromptPlus.StyleSchema.Disabled();
        public Style SelectedStyle { get; set; } = PromptPlus.StyleSchema.Selected();
        public Style HighlightStyle { get; set; } = PromptPlus.StyleSchema.TaggedInfo();
        public Style DayStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style MonthStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style YearStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Style WeekDayStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public bool DisabledWeekend { get; set; }
        public HotKey SwitchNotes { get; set; } = PromptPlus.Config.CalendarSwitchNotesPress;
        public int PageSize { get; set; } = 5;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<DateTime, string> ChangeDescription { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;
        public DateTime Maxvalue { get; set; } = DateTime.MaxValue;
        public DateTime Minvalue { get; set; } = DateTime.MinValue;
        public List<DateTime> ItemsDisabled { get; set; }
        public List<DateTime> Itemshighlight { get; set; }
        public List<ItemCalendar> ItemsNotes { get; set; }
        internal bool ShowingNotes { get; set; }
        internal DayOfWeek FirstWeekDay { get; set; } = DayOfWeek.Sunday;

    }
}
