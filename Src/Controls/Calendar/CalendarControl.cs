// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

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

        public IControlCalendar Range(DateTime minvalue, DateTime maxvalue)
        {
            if (minvalue > maxvalue)
            {
                throw new PromptPlusException($"Range invalid. Minvalue({minvalue.Date:d}) > Maxvalue({maxvalue.Date:d})");
            }
            _options.Maxvalue = maxvalue.Date;
            _options.Minvalue = minvalue.Date;
            return this; 
        }

        public IControlCalendar AddItems(CalendarScope scope, params ItemCalendar[] values)
        {
            switch (scope)
            {
                case CalendarScope.Note:
                    _options.ItemsNotes.AddRange(values.Where(x =>  (x.Note??string.Empty).Length > 0));
                    break;
                case CalendarScope.Highlight:
                    _options.Itemshighlight.AddRange(values.Select(x => x.Date));
                    _options.ItemsNotes.AddRange(values.Where(x => (x.Note ?? string.Empty).Length > 0));
                    break;
                case CalendarScope.Disabled:
                    _options.ItemsDisabled.AddRange(values.Select(x => x.Date));
                    break;
                default:
                    break;
            }
            return this;
        }


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

        public IControlCalendar Layout(CalendarLayout value)
        {
            _options.Layout = value;
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

        public IControlCalendar PageSize(int value)
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

        public IControlCalendar Default(DateTime value, PolicyInvalidDate policy = PolicyInvalidDate.NextDate)
        {
            _options.StartDate = value.Date;
            _options.PolicyInvalidDate = policy;
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

        public IControlCalendar Styles(StyleCalendar styletype, Style value)
        {
            switch (styletype)
            {
                case StyleCalendar.Line:
                    _options.LineStyle = value;
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
            _options.ItemsDisabled = _options.ItemsDisabled
                .Distinct().ToList();

            _options.ItemsNotes = _options.ItemsNotes
                  .GroupBy(p => p.Id)
                  .Select(g => g.First())
                  .ToList();

            _options.Itemshighlight = _options.Itemshighlight
                .Distinct().ToList();

            _lasdescription = _options.OptDescription;
            _options.CurrentCulture ??= _options.Config.AppCulture;
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
            var aux = _options.StartDate;
            if (_options.PolicyInvalidDate == PolicyInvalidDate.NextDate)
            {
                aux = NextDate(aux);
                if (!IsValidSelect(aux))
                {
                    aux = PreviousDate(_options.StartDate);
                }
            }
            else
            {
                aux = PreviousDate(aux);
                if (!IsValidSelect(aux))
                {
                    aux = NextDate(_options.StartDate);
                }
            }
            _currentdate = aux;
            return _currentdate.ToString("d");
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
                if (keyInfo.Value.IsPressEnterKey())
                {
                    if (!IsValidSelect(_currentdate))
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
                    var aux = DateTime.Now.Date;
                    if (_options.PolicyInvalidDate == PolicyInvalidDate.NextDate)
                    {
                        aux = NextDate(aux);
                        if (!IsValidSelect(aux))
                        {
                            aux = PreviousDate(DateTime.Now.Date);
                        }
                    }
                    else
                    {
                        aux = PreviousDate(aux);
                        if (!IsValidSelect(aux))
                        {
                            aux = NextDate(DateTime.Now.Date);
                        }
                    }
                    if (!IsValidSelect(aux))
                    {
                        SetError(Messages.SelectionInvalid);
                        break;
                    }
                    _currentdate = aux;
                    break;
                }
                //next year
                else if (keyInfo.Value.IsPressPageUpKey() && _currentdate.Date.Year < _options.Maxvalue.Year)
                {
                    if (!DateTime.IsLeapYear(_currentdate.Year + 1) && _currentdate.Month == 2 && _currentdate.Day > 28)
                    {
                        var locallastday = _currentdate.Day;
                        var aux = NextDate(new DateTime(_currentdate.Year + 1, _currentdate.Month, 28).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year + 1, _currentdate.Month, _currentdate.Day).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //previous year
                else if (keyInfo.Value.IsPressPageDownKey() && _currentdate.Date.Year > _options.Minvalue.Year)
                {
                    if (!DateTime.IsLeapYear(_currentdate.Year - 1) && _currentdate.Month == 2 && _currentdate.Day > 28)
                    {
                        var locallastday = _currentdate.Day;
                        var aux = PreviousDate(new DateTime(_currentdate.Year - 1, _currentdate.Month, 28).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = PreviousDate(new DateTime(_currentdate.Year - 1, _currentdate.Month, _currentdate.Day).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //next month
                else if (keyInfo.Value.IsPressTabKey())
                {
                    if (_currentdate.Month == 12 && _currentdate.Year == _options.Maxvalue.Year)
                    {
                        continue;
                    }
                    if (_currentdate.Month == 12)
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year + 1, 1, _currentdate.Day).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
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
                            aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, 28).Date);
                        }
                        else if (DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month + 1 == 2 && _currentdate.Day > 29)
                        {
                            locallastday = _currentdate.Day;
                            aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, 29).Date);
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
                                aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, maxday).Date);
                            }
                            else
                            {
                                if (_oldday.HasValue)
                                {
                                    if (_oldday > maxday)
                                    {
                                        aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, maxday).Date);
                                    }
                                    else
                                    {
                                        aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, _oldday.Value).Date);
                                        locallastday = null;
                                    }
                                }
                                else
                                {
                                    aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month + 1, _currentdate.Day).Date);
                                }
                            }
                        }
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    break;
                }
                //previous month
                else if (keyInfo.Value.IsPressShiftTabKey())
                {
                    if (_currentdate.Month == 1 && _currentdate.Year == _options.Minvalue.Year)
                    {
                        continue;
                    }
                    if (_currentdate.Month == 1)
                    {
                        var aux = PreviousDate(new DateTime(_currentdate.Year - 1, 12, _currentdate.Day).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
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
                            aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, 28).Date);
                        }
                        else if (DateTime.IsLeapYear(_currentdate.Year) && _currentdate.Month - 1 == 2 && _currentdate.Day > 29)
                        {
                            locallastday = _currentdate.Day;
                            aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, 29).Date);
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
                                aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, maxday).Date);
                            }
                            else
                            {
                                if (_oldday.HasValue)
                                {
                                    if (_oldday > maxday)
                                    {
                                        aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, maxday).Date);
                                    }
                                    else
                                    {
                                        aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, _oldday.Value).Date);
                                        locallastday = null;
                                    }
                                }
                                else
                                {
                                    aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month - 1, _currentdate.Day).Date);
                                }
                            }
                        }
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _oldday = locallastday;
                        _currentdate = aux;
                    }
                    break;
                }
                //next dayofweek
                else if (keyInfo.Value.IsPressDownArrowKey(true))
                {
                    if (_currentdate.Month == 12 && _currentdate.Day+7 > 31 && _currentdate.Year == _options.Maxvalue.Year)
                    {
                        continue;
                    }
                    var aux = NextDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(7).Date, _currentdate.DayOfWeek);
                    if (!IsValidSelect(aux))
                    {
                        SetError(Messages.SelectionInvalid);
                        break;
                    }
                    _currentdate = aux;
                    break;
                }
                //previous dayofweek
                else if (keyInfo.Value.IsPressUpArrowKey(true))
                {
                    if (_currentdate.Month == 1 && _currentdate.Day - 7 < 1 && _currentdate.Year == _options.Minvalue.Year)
                    {
                        continue;
                    }
                    var aux = PreviousDayOfWeek(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-7).Date, _currentdate.DayOfWeek);
                    if (!IsValidSelect(aux))
                    {
                        SetError(Messages.SelectionInvalid);
                        break;
                    }
                    _currentdate = aux;
                    break;
                }
                //previous day
                else if (keyInfo.Value.IsPressLeftArrowKey(true))
                {
                    if (_currentdate.Month == 1 && _currentdate.Day - 1 == 0 && _currentdate.Year == _options.Minvalue.Year)
                    {
                        continue;
                    }
                    if (_currentdate.Day == 1)
                    {
                        var aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-1).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = PreviousDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(-1).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //next day
                else if (keyInfo.Value.IsPressRightArrowKey(true))
                {
                    if (_currentdate.Month == 12 && _currentdate.Day == 31 && _currentdate.Year == _options.Maxvalue.Year)
                    {
                        continue;
                    }
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
                        var aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(1).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    else
                    {
                        var aux = NextDate(new DateTime(_currentdate.Year, _currentdate.Month, _currentdate.Day).AddDays(1).Date);
                        if (!IsValidSelect(aux))
                        {
                            SetError(Messages.SelectionInvalid);
                            break;
                        }
                        _currentdate = aux;
                    }
                    break;
                }
                //show Notes
                else if (!_options.ShowingNotes && _options.SwitchNotes.Equals(keyInfo.Value) && HasNote(_currentdate))
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
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            return new ResultPrompt<DateTime>(_currentdate, abort, !endinput,notrender);
        }

        private DateTime NextDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            if (_options.DisabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                return _options.Maxvalue;
            }
            while (!IsValidSelect(date) || date.DayOfWeek != dayOfWeek)
            {
                if (date.Date >= _options.Maxvalue)
                {
                    return date;
                }
                date = date.AddDays(1);
            }
            return date;
        }

        private DateTime PreviousDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            if (_options.DisabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                return _options.Minvalue;
            }
            while (!IsValidSelect(date) || date.DayOfWeek != dayOfWeek)
            {
                if (date.Date <= _options.Minvalue)
                {
                    return date;
                }
                date = date.AddDays(-1);
            }
            return date;
        }

        private DateTime NextDate(DateTime date)
        {
            if (date.Date < _options.Minvalue)
            {
                date = _options.Minvalue.Date;
            }
            if (date.Date > _options.Maxvalue)
            {
                date = _options.Maxvalue.Date;
            }
            while (!IsValidSelect(date))
            {
                if (date.Date >= _options.Maxvalue.Date)
                {
                    return date;
                }
                date = date.AddDays(1);
            }
            return date;
        }

        private DateTime PreviousDate(DateTime date)
        {
            if (date.Date < _options.Minvalue)
            {
                date = _options.Minvalue.Date;
            }
            if (date.Date > _options.Maxvalue)
            {
                date = _options.Maxvalue.Date;
            }
            while (!IsValidSelect(date))
            {
                if (date.Date <= _options.Minvalue.Date)
                {
                    return date;
                }
                date = date.AddDays(-1);
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

        private string[] GetNotes(DateTime date)
        { 
            return _options.ItemsNotes.Where(x => x.Date.Date == date).Select(x => x.Note).ToArray();
        }

        private bool IsValidSelect(DateTime date)
        {
            if (date.Date < _options.Minvalue.Date)
            {
                return false;
            }
            if (date.Date > _options.Maxvalue.Date)
            {
                return false;
            }
            if (_options.ItemsDisabled.Any(x => x.Date == date.Date))
            {
                return false;
            }
            if (_options.DisabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            return true;
        }

        private bool IsDateDisable(DateTime date)
        {
            if (_options.DisabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }
            return _options.ItemsDisabled.Any(x => x.Date == date.Date);
        }

        private bool HasNote(DateTime date)
        {
            return _options.ItemsNotes.Any(x => x.Date.Date == date.Date);
        }

        private bool IsHighlight(DateTime date)
        {
            return _options.Itemshighlight.Any(x => x.Date == date.Date);

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
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                        Messages.SelectFinishEnter,
                        Messages.TooltipPagesNotes,
                        string.Format(Messages.TooltipToggleNotes,_options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}\n{2}, {3}",
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        Messages.SelectFinishEnter,
                        Messages.TooltipPagesNotes,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
                return;
            }
            var linedays = Messages.MoveDays;
            var lineweek = Messages.MoveDayWeek;
            var linemonth = Messages.MoveMonth;
            var lineyear = Messages.MoveYear;
            var linetoday = $", {Messages.MoveToday}";

            screenBuffer.NewLine();
            if (_options.OptEnabledAbortKey)
            {
                if (!string.IsNullOrEmpty(linedays))
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}",
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                        Messages.SelectFinishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes),
                        $"{linedays}, {lineweek}"), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}\n{2}, {3}",
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                        Messages.SelectFinishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes)), _options.OptStyleSchema.Tooltips());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(linedays))
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}",
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        Messages.SelectFinishEnter,
                        string.Format(Messages.TooltipToggleNotes, _options.SwitchNotes),
                        $"{linedays}, {lineweek}"), _options.OptStyleSchema.Tooltips());
                }
                else
                {
                    screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                        string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                        Messages.SelectFinishEnter,
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
                switch (_options.Layout)
                {
                    case CalendarLayout.SingleGrid:
                    case CalendarLayout.AsciiSingleGrid:
                        WriteCalendarAsciiSingleGrid(screenBuffer, currentdate);
                        break;
                    case CalendarLayout.HeavyGrid:
                    case CalendarLayout.DoubleGrid:
                    case CalendarLayout.AsciiDoubleGrid:
                        WriteCalendarAsciiDoubleGrid(screenBuffer, currentdate);
                        break;
                    default:
                        throw new PromptPlusException($"Layout: {_options.Layout} Not Implemented");
                }
                return;
            }
            switch (_options.Layout)
            {
                case CalendarLayout.HeavyGrid:
                    WriteCalendarHeavyGrid(screenBuffer, currentdate);
                    break;
                case CalendarLayout.SingleGrid:
                    WriteCalendarSingleGrid(screenBuffer, currentdate);
                    break;
                case CalendarLayout.DoubleGrid:
                    WriteCalendarDoubleGrid(screenBuffer, currentdate);
                    break;
                case CalendarLayout.AsciiSingleGrid:
                    WriteCalendarAsciiSingleGrid(screenBuffer, currentdate);
                    break;
                case CalendarLayout.AsciiDoubleGrid:
                    WriteCalendarAsciiDoubleGrid(screenBuffer, currentdate);
                    break;
                default:
                    throw new PromptPlusException($"Layout: {_options.Layout} Not Implemented");
            }
        }

        private void WriteCalendarAsciiSingleGrid(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(28);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("+-----------------------------------+", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("|-----------------------------------|", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                abr = $" {abr} ";
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
            }
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("|-----------------------------------|", _options.LineStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('|', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("     ", _options.LineStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    auxdate = auxdate.AddDays(1);
                }
            }
            screenBuffer.AddBuffer('|', _options.LineStyle);
            var maxdate = false;
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('|', _options.LineStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        if (auxdate.Date == DateTime.MaxValue.Date)
                        {
                            if (!maxdate)
                            {
                                WriteDay(screenBuffer, auxdate);
                                maxdate = true;
                            }
                            else
                            {
                                screenBuffer.AddBuffer("     ", _options.LineStyle);
                            }
                        }
                        else
                        {
                            WriteDay(screenBuffer, auxdate);
                            auxdate = auxdate.AddDays(1);
                        }
                    }
                    else
                    {
                        screenBuffer.AddBuffer("     ", _options.LineStyle);
                    }
                }
                screenBuffer.AddBuffer('|', _options.LineStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("+-----------------------------------+", _options.LineStyle);
        }

        private void WriteCalendarAsciiDoubleGrid(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(28);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("+===================================+", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("|===================================|", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('|', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                abr = $" {abr} ";
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
            }
            screenBuffer.AddBuffer('|', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("|===================================|", _options.LineStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('|', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("     ", _options.LineStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    auxdate = auxdate.AddDays(1);
                }
            }
            screenBuffer.AddBuffer('|', _options.LineStyle);
            var maxdate = false;
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('|', _options.LineStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        if (auxdate.Date == DateTime.MaxValue.Date)
                        {
                            if (!maxdate)
                            {
                                WriteDay(screenBuffer, auxdate);
                                maxdate = true;
                            }
                            else
                            {
                                screenBuffer.AddBuffer("     ", _options.LineStyle);
                            }
                        }
                        else
                        {
                            WriteDay(screenBuffer, auxdate);
                            auxdate = auxdate.AddDays(1);
                        }
                    }
                    else
                    {
                        screenBuffer.AddBuffer("     ", _options.LineStyle);
                    }
                }
                screenBuffer.AddBuffer('|', _options.LineStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("+===================================+", _options.LineStyle);
        }

        private void WriteCalendarSingleGrid(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(28);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("┌───────────────────────────────────┐", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('│', _options.LineStyle);
            screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            screenBuffer.AddBuffer('│', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("├───────────────────────────────────┤", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('│', _options.LineStyle);


            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                { 
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                abr = $" {abr} ";
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
            }
            screenBuffer.AddBuffer('│', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("├───────────────────────────────────┤", _options.LineStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('│', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("     ", _options.LineStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    auxdate = auxdate.AddDays(1);
                }
            }
            screenBuffer.AddBuffer('│', _options.LineStyle);
            var maxdate = false;
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('│', _options.LineStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        if (auxdate.Date == DateTime.MaxValue.Date)
                        {
                            if (!maxdate)
                            {
                                WriteDay(screenBuffer, auxdate);
                                maxdate = true;
                            }
                            else
                            {
                                screenBuffer.AddBuffer("     ", _options.LineStyle);
                            }
                        }
                        else
                        {
                            WriteDay(screenBuffer, auxdate);
                            auxdate = auxdate.AddDays(1);
                        }
                    }
                    else
                    {
                        screenBuffer.AddBuffer("     ", _options.LineStyle);
                    }
                }
                screenBuffer.AddBuffer('│', _options.LineStyle);
                if (auxdate.Date == DateTime.MaxValue.Date)
                {
                    break;
                }
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("└───────────────────────────────────┘", _options.LineStyle);
        }

        private void WriteCalendarDoubleGrid(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(28);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("╔═══════════════════════════════════╗", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('║', _options.LineStyle);
            screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            screenBuffer.AddBuffer('║', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("╠═══════════════════════════════════╣", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('║', _options.LineStyle);


            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                abr = $" {abr} ";
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
            }
            screenBuffer.AddBuffer('║', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("╠═══════════════════════════════════╣", _options.LineStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('║', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("     ", _options.LineStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    auxdate = auxdate.AddDays(1);
                }
            }
            screenBuffer.AddBuffer('║', _options.LineStyle);
            var maxdate = false;
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('║', _options.LineStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        if (auxdate.Date == DateTime.MaxValue.Date)
                        {
                            if (!maxdate)
                            {
                                WriteDay(screenBuffer, auxdate);
                                maxdate = true;
                            }
                            else
                            {
                                screenBuffer.AddBuffer("     ", _options.LineStyle);
                            }
                        }
                        else
                        {
                            WriteDay(screenBuffer, auxdate);
                            auxdate = auxdate.AddDays(1);
                        }
                    }
                    else
                    {
                        screenBuffer.AddBuffer("     ", _options.LineStyle);
                    }
                }
                screenBuffer.AddBuffer('║', _options.LineStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("╚═══════════════════════════════════╝", _options.LineStyle);
        }

        private void WriteCalendarHeavyGrid(ScreenBuffer screenBuffer, DateTime currentdate)
        {
            var curmonth = currentdate.Date.ToString("MMMM", _options.CurrentCulture).PadRight(28);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            var curyear = currentdate.Date.ToString("yyyy", _options.CurrentCulture);
            screenBuffer.AddBuffer("▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('▐', _options.LineStyle);
            screenBuffer.AddBuffer($" {curmonth}", _options.MonthStyle);
            screenBuffer.AddBuffer($" {curyear} ", _options.YearStyle);
            screenBuffer.AddBuffer('▌', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌", _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer('▐', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                var abr = _options.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                abr = $" {abr} ";
                if (item == _currentdate.DayOfWeek)
                {
                    screenBuffer.AddBuffer(abr, _options.SelectedStyle);
                }
                else
                {
                    screenBuffer.AddBuffer(abr, _options.WeekDayStyle);
                }
            }
            screenBuffer.AddBuffer('▌', _options.LineStyle);
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌", _options.LineStyle);
            screenBuffer.NewLine();

            var auxdate = new DateTime(_currentdate.Year, _currentdate.Month, 1);
            var weekdays = MonthWeekDays(DateTime.DaysInMonth(_currentdate.Year, _currentdate.Month));
            screenBuffer.AddBuffer('▐', _options.LineStyle);
            foreach (var item in _Weekdays)
            {
                if (item != weekdays[auxdate.Day - 1])
                {
                    screenBuffer.AddBuffer("     ", _options.LineStyle);
                }
                else
                {
                    WriteDay(screenBuffer, auxdate);
                    auxdate = auxdate.AddDays(1);
                }
            }
            screenBuffer.AddBuffer('▌', _options.LineStyle);
            var maxdate = false;
            while (auxdate.Month == _currentdate.Month)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer('▐', _options.LineStyle);
                for (int i = 0; i < 7; i++)
                {
                    if (auxdate.Month == _currentdate.Month)
                    {
                        if (auxdate.Date == DateTime.MaxValue.Date)
                        {
                            if (!maxdate)
                            {
                                WriteDay(screenBuffer, auxdate);
                                maxdate = true;
                            }
                            else
                            {
                                screenBuffer.AddBuffer("     ", _options.LineStyle);
                            }
                        }
                        else
                        {
                            WriteDay(screenBuffer, auxdate);
                            auxdate = auxdate.AddDays(1);
                        }
                    }
                    else
                    {
                        screenBuffer.AddBuffer("     ", _options.LineStyle);
                    }
                }
                screenBuffer.AddBuffer('▌', _options.LineStyle);
            }
            screenBuffer.NewLine();
            screenBuffer.AddBuffer("▐▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▌", _options.LineStyle);
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
                if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
        }

        private void WriteDay(ScreenBuffer screenBuffer, DateTime auxdate)
        {
            var cday = auxdate.Date.ToString("dd");
            var strnote = " ";
            if (HasNote(auxdate))
            {
                strnote = "*";
            }
            if (IsDateDisable(auxdate))
            {
                screenBuffer.AddBuffer($" {strnote}{cday} ", _options.DisabledStyle);
            }
            else if (auxdate == _currentdate)
            {
                screenBuffer.AddBuffer($" {strnote}{cday} ", _options.SelectedStyle);
            }
            else
            {
                if (IsHighlight(auxdate))
                {
                    screenBuffer.AddBuffer($" {strnote}{cday} ", _options.HighlightStyle);
                }
                else
                {
                    screenBuffer.AddBuffer($" {strnote}{cday} ", _options.DayStyle);
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
