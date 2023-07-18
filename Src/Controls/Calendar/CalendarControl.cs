using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal class CalendarControl : BaseControl<DateTime>, IControlCalendar
    {
        private readonly CalendarOptions _options;
        private DateTime _currentdate;
        private IEnumerable<ItemCalendar>? _itemsNoteshighlight;
        private IEnumerable<ItemCalendar>? _itemsNotes;
        private IEnumerable<int>? _daysDisabled;
        private int? _oldday = null;
        private Paginator<string> _localpaginator;
        private DayOfWeek[] _Weekdays;
        private string _defaultHistoric = null;
        private DateTime? _lastcurrentDate;
        private string _lasdescription = null;

        public CalendarControl(IConsoleControl console, CalendarOptions options) : base(console, options)
        {
            _options = options;
        }

        #region IControlCalendar


        public IControlCalendar OverwriteDefaultFrom(string value, TimeSpan? timeout)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlCalendar ChangeDescription(Func<DateTime, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }


        public IControlCalendar AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlCalendar Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlCalendar DisabledWeekends()
        { 
            _options.DisabledWeekend = true;
            return this;
        }

        public IControlCalendar NotesPageSize(int value)
        {
            if (value < 1)
            { 
                value = 1;
            }
            _options.PageSize = value;
            return this;
        }
        public IControlCalendar HotKeySwitchNotes(HotKey value)
        {
            _options.SwitchNotes = value;
            return this;
        }

        public IControlCalendar Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlCalendar Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlCalendar Default(DateTime value, bool nextdateifdisabled = true)
        {
            _options.StartDate = value.Date;
            _options.DefaultNextdateifdisabled = nextdateifdisabled;
            return this;
        }

        public IControlCalendar DisabledChangeDay()
        {
            _options.DisabledChangeDay = true;
            return this;
        }

        public IControlCalendar DisabledChangeMonth()
        {
            _options.DisabledChangeMonth = true;
            return this;
        }

        public IControlCalendar DisabledChangeYear()
        {
            _options.DisabledChangeYear = true;
            return this;
        }

        public IControlCalendar AddNotes(Func<int, int, ItemCalendar[]> value)
        {
            _options.FuncNotesDays = value;
            return this;
        }

        public IControlCalendar AddNotesHighlight(Func<int, int, ItemCalendar[]>? value)
        {
            _options.FuncHighlightDays = value;
            return this;
        }

        public IControlCalendar AddDisabled(Func<int, int, int[]>? value)
        {
            _options.FuncDisabledDays = value;
            return this;
        }

        public IControlCalendar Interaction<T1>(IEnumerable<T1> values, Action<IControlCalendar, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlCalendar RangeMonth(int min, int max)
        {
            if (min > max)
            {
                throw new PromptPlusException("minimum greater than maximum");
            }
            if (min < 1)
            {
                throw new PromptPlusException("minimum greater than 1");
            }
            if (max > 12)
            {
                throw new PromptPlusException("maximum greater than 12");
            }
            _options.MinMonth = min;
            _options.MaxMonth = max;
            return this;
        }

        public IControlCalendar RangeYear(int min, int max)
        {
            if (min > max)
            {
                throw new PromptPlusException("minimum greater than maximum");
            }
            if (min < DateTime.MinValue.Year)
            {
                throw new PromptPlusException($"minimum greater than {DateTime.MinValue.Year}");
            }
            if (max > DateTime.MaxValue.Year)
            {
                throw new PromptPlusException($"maximum greater than {DateTime.MaxValue.Year}");
            }
            _options.MinYear = min;
            _options.MaxYear = max;
            return this;
        }

        public IControlCalendar Styles(StyleCalendar styletype, Style value)
        {
            switch (styletype)
            {
                case StyleCalendar.Grid:
                    _options.GridStyle = value;
                    break;
                case StyleCalendar.Disabled:
                    _options.DisabledStyle = value;
                    break;
                case StyleCalendar.Selected:
                    _options.SelectedStyle = value;
                    break;
                case StyleCalendar.Highlight:
                    _options.HighlightStyle = value;
                    break;
                case StyleCalendar.Day:
                    _options.DayStyle = value;
                    break;
                case StyleCalendar.Month:
                    _options.MonthStyle = value;
                    break;
                case StyleCalendar.Year:
                    _options.YearStyle = value;
                    break;
                case StyleCalendar.WeekDay:
                    _options.WeekDayStyle = value;
                    break;
                default:
                    throw new PromptPlusException($"StyleChart: {styletype} Not Implemented");
            }
            return this;
        }

        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            _lasdescription = _options.OptDescription;
            if (_options.CurrentCulture == null)
            {
                _options.CurrentCulture = PromptPlus.Config.AppCulture;
            }
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
            }
            if (!string.IsNullOrEmpty(_defaultHistoric))
            {
                if (DateTime.TryParse(_defaultHistoric, _options.CurrentCulture, DateTimeStyles.None, out var newdefault))
                {
                    _options.StartDate = newdefault;
                }
            }

            _Weekdays = GetWeekdays();
            _options.FirstWeekDay = _options.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (_options.StartDate.Month < _options.MinMonth)
            {
                _options.StartDate = new DateTime(_options.StartDate.Day, _options.MinMonth, _options.StartDate.Year);
            }
            if (_options.StartDate.Month > _options.MaxMonth)
            {
                _options.StartDate = new DateTime(_options.StartDate.Day, _options.MaxMonth, _options.StartDate.Year);
            }
            if (_options.StartDate.Year < _options.MinYear)
            {
                _options.StartDate = new DateTime(_options.StartDate.Day, _options.StartDate.Month, _options.MinYear);
            }
            if (_options.StartDate.Year > _options.MaxYear)
            {
                _options.StartDate = new DateTime(_options.StartDate.Day, _options.StartDate.Month, _options.MaxYear);
            }
            _currentdate = LoadData(_options.StartDate, _options.DefaultNextdateifdisabled);
            return _currentdate.ToString("d");
        }

        private DateTime NextDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            var curmonth = _currentdate.Month;
            var curyear = _currentdate.Month;
            if (date.Month != curmonth || date.Year != curyear)
            {
                curmonth = date.Month;
                curyear = date.Month;
                LoadFilters(date);
            }
            while (IsDateDisable(date) || date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
                if (date.Month != curmonth || date.Year != curyear)
                {
                    curmonth = date.Month;
                    curyear = date.Month;
                    LoadFilters(date);
                }
            }
            return date;
        }

        private DateTime PreviousDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            var curmonth = _currentdate.Month;
            var curyear = _currentdate.Month;
            if (date.Month != curmonth || date.Year != curyear)
            {
                curmonth = date.Month;
                curyear = date.Month;
                LoadFilters(date);
            }
            while (IsDateDisable(date) || date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(-1);
                if (date.Month != curmonth || date.Year != curyear)
                {
                    curmonth = date.Month;
                    curyear = date.Month;
                    LoadFilters(date);
                }
            }
            return date;
        }

        private void LoadDefaultHistory()
        {
            _defaultHistoric = null;
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = aux[0].History;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveDefaultHistory(string value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(value, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }

        private DateTime NextDate(DateTime date)
        {
            var curmonth = _currentdate.Month;
            var curyear = _currentdate.Month;
            if (date.Month != curmonth || date.Year != curyear)
            {
                curmonth = date.Month;
                curyear = date.Month;
                LoadFilters(date);

            }
            while (IsDateDisable(date))
            {
                if (date.Year == 9999 && date.Month == 12 && date.Day == 31)
                {
                    return date;
                }
                date = date.AddDays(1);
                if (date.Month != curmonth || date.Year != curyear)
                {
                    curmonth = date.Month;
                    curyear = date.Month;
                    LoadFilters(date);
                }
            }
            return date;
        }

        private IEnumerable<int> Weekends(int year, int month)
        {
            var result = new List<int>();
            var aux = new DateTime(year, month, 1);
            var dt = new DateTime(year, month, 1);
            while (aux.Month == dt.Month)
            {
                if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                { 
                    result.Add(dt.Day);
                }
                dt = dt.AddDays(1);
            }
            return result;
        }

        private DateTime PreviousDate(DateTime date)
        {
            var curmonth = _currentdate.Month;
            var curyear = _currentdate.Month;
            if (date.Month != curmonth || date.Year != curyear)
            {
                curmonth = date.Month;
                curyear = date.Month;
                LoadFilters(date);
            }
            while (IsDateDisable(date))
            {
                if (date.Year == 0 && date.Month == 1 && date.Day == 1)
                {
                    return date;
                }

                date = date.AddDays(-1);
                if (date.Month != curmonth || date.Year != curyear)
                {
                    curmonth = date.Month;
                    curyear = date.Month;
                    LoadFilters(date);
                }
            }
            return date;
        }

        private void LoadFilters(DateTime date)
        {
            if (_options.FuncDisabledDays != null)
            {
                _daysDisabled = _options.FuncDisabledDays.Invoke(date.Year, date.Month);
            }
            if (_options.DisabledWeekend)
            {
                if (_daysDisabled == null)
                {
                    _daysDisabled = Weekends(date.Year, date.Month);
                }
                else
                {
                    var aux = _daysDisabled.ToList();
                    foreach (var item in Weekends(date.Year, date.Month))
                    {
                        if (!aux.Contains(item))
                        {
                            aux.Add(item);
                        }
                    }
                    _daysDisabled = aux;
                }
            }
            if (_options.FuncHighlightDays != null)
            {
                _itemsNoteshighlight = _options.FuncHighlightDays.Invoke(date.Year, date.Month);
            }
            if (_options.FuncNotesDays != null)
            {
                _itemsNotes = _options.FuncNotesDays.Invoke(date.Year, date.Month);
            }
        }

        private DateTime LoadData(DateTime date, bool nextDate)
        {
            LoadFilters(date);
            if (IsDateDisable(date))
            {
                if (nextDate)
                {
                    date = NextDate(date);
                }
                else
                {
                    date = PreviousDate(date);
                }
            }
            return date;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            //none
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            WriteTitle(screenBuffer);
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.AddBuffer(ValidateError, _options.OptStyleSchema.Error(), true);
                screenBuffer.NewLine();
            }
            WriteCalendar(screenBuffer, _currentdate);
            WriteNotes(screenBuffer);
            WriteTooltip(screenBuffer);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, DateTime result, bool aborted)
        {
            string answer = _currentdate.ToString("d");
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                {
                    SaveDefaultHistory(answer);
                }
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<DateTime> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            bool tryagain;
            do
            {
                ClearError();
                tryagain = false;
                var keyInfo = WaitKeypress(cancellationToken);
                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    break;
                }
                if (CheckAbortKey(keyInfo.Value))
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (_options.ShowingNotes)
                {
                    //hide Notes
                    if(_options.ShowingNotes && _options.SwitchNotes.Equals(keyInfo.Value))
                    {
                        _options.ShowingNotes = false;
                        break;
                    }
                    if (IskeyPageNavegator(keyInfo.Value, _localpaginator))
                    {
                        break;
                    }
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tryagain = true;
                    }
                    continue;
                }
                //Today
                if (keyInfo.Value.IsPressHomeKey(true))
                {
                    if (!IsValidToday() || _currentdate.Date == DateTime.Now.Date)
                    {
                        tryagain = true;
                        continue;
                    }
                    _currentdate = DateTime.Now.Date;
                    if (_options.DefaultNextdateifdisabled)
                    {
                        _currentdate = NextDate(_currentdate);
                    }
                    else
                    {
                        _currentdate = PreviousDate(_currentdate);
                    }
                    break;
                }
                //next year
                else if (keyInfo.Value.IsPressPageUpKey() && _currentdate.Year < 9999 && !_options.DisabledChangeYear)
                {
                    if (!DateTime.IsLeapYear(_currentdate.Year + 1) && _currentdate.Month == 2 && _currentdate.Day > 28)
                    {
                        var locallastday = _currentdate.Day;
                        var aux = LoadData(new DateTime(_currentdate.Year + 1, _currentdate.Month, 28).Date, true);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year + 1, _currentdate.Month, _currentdate.Day).Date);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //previous year
                else if (keyInfo.Value.IsPressPageDownKey() && _currentdate.Year > 0 && !_options.DisabledChangeYear)
                {
                    if (!DateTime.IsLeapYear(_currentdate.Year - 1) && _currentdate.Month == 2 && _currentdate.Day > 28)
                    {
                        var locallastday = _currentdate.Day;
                        var aux = LoadData(new DateTime(_currentdate.Year - 1, _currentdate.Month, 28).Date, true);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year - 1, _currentdate.Month, _currentdate.Day).Date);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //next month
                else if (keyInfo.Value.IsPressTabKey() && !_options.DisabledChangeMonth)
                {
                    if (_currentdate.Month == 12)
                    {
                        var aux = LoadData(new DateTime(_currentdate.Year + 1, 1, _currentdate.Day).Date, true);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        DateTime aux;
                        int? locallastday = _oldday;
                        if (!DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month + 1 == 2 && _currentdate.Day > 28)
                        {
                            locallastday = _currentdate.Day;
                            aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, 28).Date, true);
                        }
                        else if (DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month + 1 == 2 && _currentdate.Day > 29)
                        {
                            locallastday = _currentdate.Day;
                            aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, 29).Date, true);
                        }
                        else
                        {
                            int maxday;
                            if (_currentdate.Month + 2 > 12)
                            {
                                maxday = 31;
                            }
                            else
                            {
                                maxday = new DateTime(_currentdate.Year, _currentdate.Month + 2, 1).AddDays(-1).Day;
                            }
                            if (_currentdate.Day > maxday)
                            {
                                if (!_oldday.HasValue)
                                {
                                    locallastday = _currentdate.Day;
                                }
                                aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, maxday).Date, true);
                            }
                            else
                            {
                                if (_oldday.HasValue)
                                {
                                    if (_oldday > maxday)
                                    {
                                        aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, maxday).Date, true);
                                    }
                                    else
                                    {
                                        aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, _oldday.Value).Date, true);
                                        locallastday = null;
                                    }
                                }
                                else
                                {
                                    aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month + 1, _currentdate.Day).Date, true);
                                }
                            }
                        }
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    break;
                }
                //previous month
                else if (keyInfo.Value.IsPressShiftTabKey() && !_options.DisabledChangeMonth)
                {
                    if (_currentdate.Month == 1)
                    {
                        var aux = LoadData(new DateTime(_currentdate.Year - 1, 12, _currentdate.Day).Date, false);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        DateTime aux;
                        int? locallastday = _oldday;
                        if (!DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month - 1 == 2 && _currentdate.Day > 28)
                        {
                            locallastday = _currentdate.Day;
                            aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, 28).Date, false);
                        }
                        else if (DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month - 1 == 2 && _currentdate.Day > 29)
                        {
                            locallastday = _currentdate.Day;
                            aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, 29).Date, false);
                        }
                        else
                        {
                            var maxday = new DateTime(_currentdate.Year, _currentdate.Month, 1).AddDays(-1).Day;
                            if (_currentdate.Day > maxday)
                            {
                                if (!_oldday.HasValue)
                                {
                                    locallastday = _currentdate.Day;
                                }
                                aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, maxday).Date, false);
                            }
                            else
                            {
                                if (_oldday.HasValue)
                                {
                                    if (_oldday > maxday)
                                    {
                                        aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, maxday).Date, false);
                                    }
                                    else
                                    {
                                        aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, _oldday.Value).Date, false);
                                        locallastday = null;
                                    }
                                }
                                else
                                {
                                    aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month - 1, _currentdate.Day).Date, false);
                                }
                            }
                        }
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    break;
                }
                //next dayofweek
                else if (keyInfo.Value.IsPressDownArrowKey(true) && !_options.DisabledChangeDay)
                {
                    if (_options.DisabledChangeMonth)
                    {
                        var aux = NextDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(7).Date, _currentdate.DayOfWeek);
                        if (aux.Month != _currentdate.Month)
                        {
                            tryagain = true;
                            continue;
                        }
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(7).Date, _currentdate.DayOfWeek);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //previous dayofweek
                else if (keyInfo.Value.IsPressUpArrowKey(true) && !_options.DisabledChangeDay)
                {
                    if (_options.DisabledChangeMonth)
                    {
                        var aux = PreviousDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-7).Date, _currentdate.DayOfWeek);
                        if (aux.Month != _currentdate.Month)
                        {
                            tryagain = true;
                            continue;
                        }
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = PreviousDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-7).Date, _currentdate.DayOfWeek);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //previous day
                else if (keyInfo.Value.IsPressLeftArrowKey(true) && !_options.DisabledChangeDay)
                {
                    if (_currentdate.Day == 1)
                    {
                        if (_options.DisabledChangeMonth)
                        {
                            tryagain = true;
                            continue;
                        }
                        var aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-1).Date, false);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-1).Date);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //next day
                else if (keyInfo.Value.IsPressRightArrowKey(true) && !_options.DisabledChangeDay)
                {
                    int maxday;
                    if (_currentdate.Month + 2 > 12)
                    {
                        maxday = 31;
                    }
                    else
                    {
                        maxday = new DateTime(_currentdate.Year, _currentdate.Month + 2, 1).AddDays(-1).Day;
                    }
                    if (_currentdate.Day == maxday)
                    {
                        if (_options.DisabledChangeMonth)
                        {
                            tryagain = true;
                            continue;
                        }
                        var aux = LoadData(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(1).Date, true);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(1).Date);
                        if (!IsValidRange(aux))
                        {
                            tryagain = true;
                            continue;
                        }
                        _currentdate = aux;

                    }
                    break;
                }
                //show Notes
                else if (!_options.ShowingNotes && _options.SwitchNotes.Equals(keyInfo.Value) && (HasNote(_itemsNotes, _currentdate) || HasNote(_itemsNoteshighlight, _currentdate)))
                {
                    _localpaginator = new Paginator<string>(
                        FilterMode.StartsWith,
                        GetNotes(_currentdate),
                        _options.PageSize, Optional<string>.s_empty,
                        (item1, item2) => item1 == item2,
                        (item) => item);

                    if (_localpaginator.Count > 0)
                    {
                        _options.ShowingNotes = true;
                    }
                    else
                    {
                        tryagain = true;
                        continue;
                    }
                    break;
                }
                //completed input
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    if (!IsValidRange(_currentdate) || IsDateDisable(_currentdate))
                    {
                        SetError(Messages.SelectionInvalid);
                    }
                    else
                    {
                        if (!TryValidate(_currentdate, _options.Validators))
                        {
                            if (!abort)
                            {
                                endinput = false;
                            }
                        }
                        else
                        {
                            endinput = true;
                        }
                    }
                    break;
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                endinput = true;
                abort = true;
            }
            return new ResultPrompt<DateTime>(_currentdate, abort, !endinput);
        }

        private string[] GetNotes(DateTime date)
        { 
            var result = new List<string>();
            if (HasNote(_itemsNotes, _currentdate))
            {
                foreach (var item in _itemsNotes.First(x => x.Date.Date == date.Date).Notes)
                {
                    if (!result.Contains(item, StringComparer.InvariantCultureIgnoreCase))
                    {
                        result.Add(item);
                    }
                }
            }
            if (HasNote(_itemsNoteshighlight, _currentdate))
            {
                foreach (var item in _itemsNoteshighlight.First(x => x.Date.Date == date.Date).Notes)
                {
                    if (!result.Contains(item, StringComparer.InvariantCultureIgnoreCase))
                    {
                        result.Add(item);
                    }
                }
            }
            return result.ToArray();
        }


        private bool IsValidRange(DateTime date)
        {
            if (date.Month < _options.MinMonth)
            {
                return false;
            }
            if (date.Month > _options.MaxMonth)
            {
                return false;
            }
            if (date.Year < _options.MinYear)
            {
                return false;
            }
            if (date.Year > _options.MaxYear)
            {
                return false;
            }
            return true;
        }

        private bool IsValidToday()
        {
            if (_options.DisabledChangeYear)
            {
                if (DateTime.Now.Year != _currentdate.Year)
                {
                    return false;
                }
            }
            if (_options.DisabledChangeMonth)
            {
                if (DateTime.Now.Month != _currentdate.Month)
                {
                    return false;
                }
            }
            if (_options.DisabledChangeDay)
            {
                if (DateTime.Now.Day != _currentdate.Day)
                {
                    return false;
                }
            }
            if (!IsValidRange(DateTime.Now))
            {
                return false;
            }
            return true;
        }

        private bool IsDateDisable(DateTime date)
        {
            if (_daysDisabled == null)
            {
                return false;
            }
            return _daysDisabled.Any(x => x == date.Day);
        }
        private bool HasNote(IEnumerable<ItemCalendar> items, DateTime date)
        {
            if (items == null)
            {
                return false;
            }
            return items.Any(x => x.Date.Date == date.Date && x.Notes != null && x.Notes.Any(x => !string.IsNullOrEmpty(x)));
        }

        private bool IsHighlight(DateTime date)
        {
            if (_itemsNoteshighlight == null)
            {
                return false;
            }
            return _itemsNoteshighlight.Any(x => x.Date.Date == date.Date);
        }

        private void WriteTooltip(ScreenBuffer screenBuffer)
        {
            if (!_options.OptShowTooltip)
            {
                return;
            }
            if (_options.ShowingNotes)
            {
                screenBuffer.NewLine();
                if (_options.OptEnabledAbortKey)
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3} {4}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                        Messages.SelectFisnishEnter,
                        Messages.TooltipPagesNotes,
                        string.Format(Messages.TooltipToggleNotes,_options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}\n{2}, {3}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        Messages.SelectFisnishEnter,
                        Messages.TooltipPagesNotes,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
                return;
            }
            var linedays = string.Empty;
            var lineweek = string.Empty;
            var linemonth = string.Empty;
            var lineyear = string.Empty;
            var linetoday = string.Empty;
            if (!_options.DisabledChangeDay)
            {
                linedays = Messages.MoveDays;
                lineweek = Messages.MoveDayWeek;
            }
            if (!_options.DisabledChangeMonth)
            {
                linemonth = Messages.MoveMonth;
            }
            if (!_options.DisabledChangeYear)
            {
                lineyear = Messages.MoveYear;
            }
            if (!_options.DisabledToday)
            {
                linetoday = $", {Messages.MoveToday}";
            }
            screenBuffer.NewLine();
            if (_options.OptEnabledAbortKey)
            {
                if (!string.IsNullOrEmpty(linedays))
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                        Messages.SelectFisnishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes),
                        $"{linedays}, {lineweek}"), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}\n{2}, {3}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                        Messages.SelectFisnishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(linedays))
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        Messages.SelectFisnishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes),
                        $"{linedays}, {lineweek}"), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        Messages.SelectFisnishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
            }
            if (!string.IsNullOrEmpty(linemonth) && !string.IsNullOrEmpty(lineyear))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{linemonth}, {lineyear}{linetoday}", _options.OptStyleSchema.Tooltips());
            }
            else if (!string.IsNullOrEmpty(linemonth) && string.IsNullOrEmpty(lineyear))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{linemonth}{linetoday}", _options.OptStyleSchema.Tooltips());
            }
            else if (string.IsNullOrEmpty(linemonth) && !string.IsNullOrEmpty(lineyear))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{lineyear}{linetoday}", _options.OptStyleSchema.Tooltips());
            }
            else
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(linetoday.Replace(",","").Trim(), _options.OptStyleSchema.Tooltips());
            }
        }

        private void WriteTitle(ScreenBuffer screenBuffer)
        {
            var desc = _lasdescription;
            if (_options.ChangeDescription != null)
            {
                if (!_lastcurrentDate.HasValue)
                {
                    desc = _options.ChangeDescription(_currentdate);
                    _lastcurrentDate = _currentdate;
                }
                else
                {
                    if (_lastcurrentDate.Value.Date != _currentdate.Date)
                    {
                        desc = _options.ChangeDescription(_currentdate);
                        _lastcurrentDate = _currentdate;
                    }
                }
            }
            if (string.IsNullOrEmpty(desc))
            {
                desc = _options.OptDescription;
            }
            _lasdescription = desc;

            if (string.IsNullOrEmpty(_options.OptPrompt) && string.IsNullOrEmpty(_options.OptDescription))
            {
                return;
            }
            if (!string.IsNullOrEmpty(_options.OptPrompt))
            {
                screenBuffer.AddBuffer(_options.OptPrompt, _options.OptStyleSchema.Prompt());
                screenBuffer.AddBuffer(": ", _options.OptStyleSchema.Prompt());
            }
            screenBuffer.AddBuffer(_currentdate.ToString("d"), _options.OptStyleSchema.Answer());
            screenBuffer.SaveCursor();
            if (_options.ShowingNotes)
            {
                screenBuffer.AddBuffer($" ({Messages.ShowingNotes})", _options.OptStyleSchema.TaggedInfo());
            }
            screenBuffer.NewLine();
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.AddBuffer(desc, _options.OptStyleSchema.Description());
                screenBuffer.NewLine();
            }
        }
        private void WriteCalendar(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            if (!ConsolePlus.IsUnicodeSupported)
            {
                WriteCalendarAscii(screenBuffer, currentdate);
                return;
            }
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(20);
            curmonth = $"{curmonth.Substring(0, 1).ToUpperInvariant()}{curmonth.Substring(1)}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("┌───────────────────────────┐", _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('│', _options.GridStyle);
            if (_options.DisabledChangeMonth)
            {
                screenBuffer.AddBuffer($" {curmonth}", _options.DisabledStyle);
            }
            else
            {
                screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            }
            if (_options.DisabledChangeYear)
            {
                screenBuffer.AddBuffer($" {curyear} ", _options.DisabledStyle);
            }
            else
            {
                screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            }
            screenBuffer.AddBuffer('│', _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("├───┬───┬───┬───┬───┬───┬───┤", _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('│', _options.GridStyle);


            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr.Substring(0,1).ToUpperInvariant()}{abr.Substring(1)}";
                if (abr.Length < 3)
                { 
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr.Substring(0,3);
                }
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
                screenBuffer.AddBuffer('│', _options.GridStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("├───┼───┼───┼───┼───┼───┼───┤", _options.GridStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('│', _options.GridStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("   │", _options.GridStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    screenBuffer.AddBuffer("│", _options.GridStyle);
                    auxdate = auxdate.AddDays(1);
                }
            }
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('│', _options.GridStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        WriteDay(screenBuffer, auxdate);
                        screenBuffer.AddBuffer("│", _options.GridStyle);
                        auxdate = auxdate.AddDays(1);
                    }
                    else
                    {
                        screenBuffer.AddBuffer("   │", _options.GridStyle);
                    }
                }
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("└───┴───┴───┴───┴───┴───┴───┘", _options.GridStyle);
        }

        private void WriteCalendarAscii(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM",_options.CurrentCulture).PadRight(20);
            curmonth = $"{curmonth.Substring(0, 1).ToUpperInvariant()}{curmonth.Substring(1)}";
            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("+---------------------------+", _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.GridStyle);
            if (_options.DisabledChangeMonth)
            {
                screenBuffer.AddBuffer($" {curmonth}", _options.DisabledStyle);
            }
            else
            {
                screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            }
            if (_options.DisabledChangeYear)
            {
                screenBuffer.AddBuffer($" {curyear} ", _options.DisabledStyle);
            }
            else
            {
                screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            }
            screenBuffer.AddBuffer('|', _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("|---+---+---+---+---+---+---|", _options.GridStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.GridStyle);

            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr.Substring(0, 1).ToUpperInvariant()}{abr.Substring(1)}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr.Substring(0, 3);
                }
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr.Substring(0, 3), _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr.Substring(0, 3), _options.WeekDayStyle);
                }
                screenBuffer.AddBuffer('|', _options.GridStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("+---+---+---+---+---+---+---|", _options.GridStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('|', _options.GridStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("   |", _options.GridStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    screenBuffer.AddBuffer("|", _options.GridStyle);
                    auxdate = auxdate.AddDays(1);
                }
            }
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('|', _options.GridStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        WriteDay(screenBuffer, auxdate);
                        screenBuffer.AddBuffer("|", _options.GridStyle);
                        auxdate = auxdate.AddDays(1);
                    }
                    else
                    {
                        screenBuffer.AddBuffer("   |", _options.GridStyle);
                    }
                }
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("+---+---+---+---+---+---+---+", _options.GridStyle);
        }
        private void WriteNotes(ScreenBuffer screenBuffer)
        {
            if (_options.ShowingNotes)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<string>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, item);
                    }
                }
                if (_localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
        }

        private void WriteDay(ScreenBuffer screenBuffer, DateTime auxdate)
        {
            var cday = auxdate.Date.ToString("dd");
            var strnote = " ";
            if (HasNote(_itemsNoteshighlight, auxdate) || HasNote(_itemsNotes, auxdate))
            {
                strnote = "*";
            }
            if (IsDateDisable(auxdate))
            {
                screenBuffer.AddBuffer($"{strnote}{cday}", _options.DisabledStyle);
            }
            else if (auxdate == _currentdate)
            {
                screenBuffer.AddBuffer($"{strnote}{cday}", _options.SelectedStyle);
            }
            else
            {
                if (IsHighlight(auxdate))
                {
                    screenBuffer.AddBuffer($"{strnote}{cday}", _options.HighlightStyle);
                }
                else
                {
                    screenBuffer.AddBuffer($"{strnote}{cday}", _options.DayStyle);
                }
            }
        }

        private DayOfWeek[] MonthWeekDays(int daysInMonth)
        {
            var result = new List<DayOfWeek>();
            for (var day = 0; day < daysInMonth; day++)
            {
                result.Add(new DateTime(_currentdate.Year, _currentdate.Month, day + 1).DayOfWeek);
            }

            return result.ToArray();
        }



        private DayOfWeek[] GetWeekdays()
        {
            var week = new DayOfWeek[7];
            for (int i = (int)_options.FirstWeekDay; i < 7; i++)
            {
                if (Enum.IsDefined(typeof(DayOfWeek), i))
                {
                    week[i] = (DayOfWeek)Enum.Parse(typeof(DayOfWeek),i.ToString());
                }
            }
            return week;
        }
    }
}
