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
        private CalendarOptions() : base(null,null, null, true)
        {
            throw new PromptPlusException("CalendarOptions CTOR NotImplemented");
        }

        internal CalendarOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            ItemsDisabled = new();
            ItemsNotes = new();
            Itemshighlight = new();
            SwitchNotes = config.CalendarSwitchNotesPress;
            TimeoutOverwriteDefault = config.HistoryTimeout;
        }
        public CalendarLayout Layout { get; set; } =  CalendarLayout.SingleGrid;
        public CultureInfo CurrentCulture { get; set; } = null;
        public PolicyInvalidDate PolicyInvalidDate { get; set; } = PolicyInvalidDate.NextDate;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public bool DisabledWeekend { get; set; }
        public HotKey SwitchNotes { get; set; }
        public int PageSize { get; set; } = 5;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<DateTime, string> ChangeDescription { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; }
        public DateTime Maxvalue { get; set; } = DateTime.MaxValue;
        public DateTime Minvalue { get; set; } = DateTime.MinValue;
        public List<DateTime> ItemsDisabled { get; set; }
        public List<DateTime> Itemshighlight { get; set; }
        public List<ItemCalendar> ItemsNotes { get; set; }
        internal bool ShowingNotes { get; set; }
        internal DayOfWeek FirstWeekDay { get; set; } = DayOfWeek.Sunday;

    }
}
