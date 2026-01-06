// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.Calendar
{
    internal sealed class CalendarControl : BaseControlPrompt<DateTime?>, ICalendarControl, ICalendarWidget
    {
        private CultureInfo _culture;
        private CalendarLayout _layout;
        private DayOfWeek _firstdayOfWeek;
        private Paginator<string>? _localpaginator;
        private int _indexTooptip;
        private Func<DateTime?, (bool, string?)>? _predicatevalidselect;
        private Func<DateTime?, string>? _changeDescription;
        private DateTime? _defaultValue;
        private bool _disabledWeekend;
        private readonly HotKey? _hotKeySwitchNotes;
        private int _pageSize;
        private DateTime _minRangeDate;
        private DateTime _maxRangeDate;
        private readonly HashSet<(CalendarItem Scope, DateOnly Date, string? scopetext)> _itemscope = [];
        private readonly Dictionary<CalendarStyles, Style> _optStyles = BaseControlOptions.LoadStyle<CalendarStyles>();
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Input,[] },
            { ModeView.ShowNotes,[] },
        };
        private ModeView _modeView = ModeView.Input;
        private string _tooltipModeInput = string.Empty;
        private string _tooltipModeShowNote = string.Empty;
        private DateTime? _selectedDate;
        private DateTime _currentDate;
        private DayOfWeek[] _weekdays = [];
        private bool _isAnyEvent;


        public CalendarControl(bool isWidget, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _firstdayOfWeek = ConfigPlus.FirstDayOfWeek;
            _culture = ConfigPlus.DefaultCulture;
            _layout = CalendarLayout.SingleGrid; 
            _hotKeySwitchNotes = isWidget ? null : ConfigPlus.HotKeySwitchNotes;
            _pageSize = isWidget ? int.MaxValue : ConfigPlus.PageSize;
            _minRangeDate = DateTime.MinValue;
            _maxRangeDate = DateTime.MaxValue;
            _selectedDate = null;
            _defaultValue = null;
            _currentDate = DateTime.Today;
        }

        public void InternalDefault(DateTime value)
        {
            _defaultValue = value;
        }

        #region ICalendarControl

        ICalendarControl ICalendarControl.Highlights(params DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            foreach (DateTime item in dates)
            {
                _itemscope.Add((CalendarItem.Highlight, DateOnly.FromDateTime(item), null));
            }
            return this;
        }

        ICalendarControl ICalendarControl.AddNote(DateTime date, string? note)
        {
            ArgumentNullException.ThrowIfNull(note);
            _itemscope.Add((CalendarItem.Note, DateOnly.FromDateTime(date), note));
            return this;
        }

        ICalendarControl ICalendarControl.PredicateSelected(Func<DateTime?, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        ICalendarControl ICalendarControl.PredicateSelected(Func<DateTime?, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        ICalendarControl ICalendarControl.ChangeDescription(Func<DateTime?, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        ICalendarControl ICalendarControl.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        ICalendarControl ICalendarControl.Default(DateTime value)
        {
            _defaultValue = value;
            return this;
        }

        ICalendarControl ICalendarControl.Interaction<T>(IEnumerable<T> items, Action<T, ICalendarControl> interactionaction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionaction);

            foreach (T? item in items)
            {
                interactionaction.Invoke(item, this);
            }
            return this;
        }

        ICalendarControl ICalendarControl.DisableDates(params DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            foreach (DateTime item in dates)
            {
                _itemscope.Add((CalendarItem.Disabled, DateOnly.FromDateTime(item), null));
            }
            return this;
        }

        ICalendarControl ICalendarControl.DisabledWeekend(bool value)
        {
            _disabledWeekend = value;
            return this;
        }

        ICalendarControl ICalendarControl.FirstDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            _firstdayOfWeek = firstDayOfWeek;
            return this;
        }

        ICalendarControl ICalendarControl.Layout(CalendarLayout layout)
        {
            _layout = layout;
            return this;
        }

        ICalendarControl ICalendarControl.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        ICalendarControl ICalendarControl.PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Page size must be greater than zero.");
            }
            _pageSize = value;
            return this;
        }

        ICalendarControl ICalendarControl.Range(DateTime minValue, DateTime maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than max value.");
            }
            _minRangeDate = minValue;
            _maxRangeDate = maxValue;
            return this;
        }

        ICalendarControl ICalendarControl.Styles(CalendarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        ICalendarWidget ICalendarWidget.Layout(CalendarLayout layout)
        {
            _layout = layout;
            return this;
        }

        ICalendarWidget ICalendarWidget.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        ICalendarWidget ICalendarWidget.FirstDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            _firstdayOfWeek = firstDayOfWeek;
            return this;
        }

        ICalendarWidget ICalendarWidget.Styles(CalendarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken _)
        {
            if (_defaultValue.HasValue)
            {
                _currentDate = _defaultValue.Value;
            }
            _weekdays = GetWeekdays();
            if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
            {
                _selectedDate = _currentDate;
            }
            if (!IsWidgetControl)
            {
                LoadTooltipToggle();
                _tooltipModeInput = GetTooltipModeInput();
                _tooltipModeShowNote = GetTooltipModeShowNotes();
            }
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidgetControl)
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);

                WriteError(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteCalendar(screenBuffer);

            if (!IsWidgetControl)
            {
                WriteNotes(screenBuffer);

                WriteTooltip(screenBuffer);
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltips

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.ShowNotes)
                        {
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                        }
                        ResultCtrl = new ResultPrompt<DateTime?>(null, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.ShowNotes)
                        {
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                        }
                        ResultCtrl = new ResultPrompt<DateTime?>(null, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.ShowNotes)
                        {
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                        }
                        if (!_selectedDate.HasValue)
                        {
                            SetError(Messages.ValidateInvalid);
                            break;
                        }
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_selectedDate) ?? (true, null);
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(Messages.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        ResultCtrl = new ResultPrompt<DateTime?>(_selectedDate, false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips[_modeView].Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    #endregion

                    #region ShowNotes

                    else if (_modeView == ModeView.Input && _isAnyEvent && ConfigPlus.HotKeySwitchNotes.Equals(keyinfo))
                    {
                        DateOnly localdateref = DateOnly.FromDateTime(_currentDate);
                        _indexTooptip = 0;
                        if (!IsNote(localdateref))
                        {
                            SetError(Messages.NoteNotFound);
                            break;
                        }
                        _localpaginator = new Paginator<string>(
                            FilterMode.Disabled,
                            GetNotes(localdateref),
                            _pageSize,
                            Optional<string>.Empty(),
                            (item1, item2) => item1 == item2,
                            (item) => item);
                        _modeView = ModeView.ShowNotes;
                        break;
                    }
                    else if (_modeView == ModeView.ShowNotes)
                    {
                        if (ConfigPlus.HotKeySwitchNotes.Equals(keyinfo))
                        {
                            _indexTooptip = 0;
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                            break;
                        }
                        else if (keyinfo.IsPressDownArrowKey())
                        {
                            bool ok = _localpaginator!.IsLastPageItem ? _localpaginator.NextPage(IndexOption.FirstItem) : _localpaginator.NextItem();
                            if (ok)
                            {
                                _indexTooptip = 0;
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressUpArrowKey())
                        {
                            bool ok = _localpaginator!.IsFirstPageItem ? _localpaginator!.PreviousPage(IndexOption.LastItem) : _localpaginator!.PreviousItem();
                            if (ok)
                            {
                                _indexTooptip = 0;
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressCtrlHomeKey())
                        {
                            if (_localpaginator!.Home())
                            {
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressCtrlEndKey())
                        {
                            if (_localpaginator!.End())
                            {
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion

                    //Today
                    else if (keyinfo.IsPressHomeKey() && IsValidToday())
                    {
                        if (_selectedDate.HasValue)
                        {
                            if (DateOnly.FromDateTime(_selectedDate.Value) == DateOnly.FromDateTime(DateTime.Today))
                            {
                                continue;
                            }
                        }
                        _indexTooptip = 0;
                        _currentDate = DateTime.Today;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //next year
                    else if (keyinfo.IsPressPageUpKey() && _currentDate.AddYears(1) <= _maxRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddYears(1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //previous year
                    else if (keyinfo.IsPressPageDownKey() && _currentDate.AddYears(-1) >= _minRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddYears(-1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //next month
                    else if (keyinfo.IsPressTabKey() && _currentDate.AddMonths(1) < _maxRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddMonths(1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //previous month
                    else if (keyinfo.IsPressShiftTabKey() && _currentDate.AddMonths(-1) >= _minRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddMonths(-1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //next dayofweek
                    else if (keyinfo.IsPressDownArrowKey(true) && _currentDate.AddDays(7) <= _maxRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddDays(7);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //previous dayofweek
                    else if (keyinfo.IsPressUpArrowKey(true) && _currentDate.AddDays(-7) >= _minRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddDays(-7);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //next day
                    else if (keyinfo.IsPressRightArrowKey(true) && _currentDate.AddDays(1) <= _maxRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddDays(1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                    //previous day
                    else if (keyinfo.IsPressLeftArrowKey(true) && _currentDate.AddDays(-1) >= _minRangeDate)
                    {
                        _indexTooptip = 0;
                        DateTime aux = _currentDate.AddDays(-1);
                        _currentDate = aux;
                        _selectedDate = null;
                        if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
                        {
                            _selectedDate = _currentDate;
                        }
                        break;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void FinalizeControl()
        {
            //none
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (_selectedDate.HasValue && !ResultCtrl!.Value.IsAborted)
            {
                answer = ResultCtrl!.Value.Content!.Value.ToString("d");
            }

            if (ResultCtrl!.Value.IsAborted)
            {
                answer = GeneralOptions.ShowMesssageAbortKeyValue ? Messages.CanceledKey : string.Empty;
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[CalendarStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[CalendarStyles.Answer]);
            return true;
        }

        #region private methods

        private bool IsValidToday()
        {
            DateTime aux = DateTime.Today;
            return aux >= _minRangeDate && aux <= _maxRangeDate;
        }

        private bool IsValidSelect(DateOnly date)
        {
            return (!_disabledWeekend || date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday) && !IsDateDisable(date);
        }

        private string[] GetNotes(DateOnly currentDate)
        {
            return [.. _itemscope
                .Where(x => x.Scope == CalendarItem.Note &&  x.Date == currentDate  && !string.IsNullOrWhiteSpace(x.scopetext))
                .Select(x => x.scopetext!)];
        }

        private void WriteNotes(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.ShowNotes)
            {
                return;
            }

            ArraySegment<string> subset = _localpaginator!.GetPageData(); // Cache the page data
            screenBuffer.WriteLine(string.Format(Messages.ShowingCalendarNotes, _currentDate.ToString("d")), _optStyles[CalendarStyles.TaggedInfo]);
            foreach (string item in subset)
            {
                if (_localpaginator.TryGetSelected(out string? selectedItem) && EqualityComparer<string>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[CalendarStyles.Selected]);
                    screenBuffer.WriteLine($" {item}", _optStyles[CalendarStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", _optStyles[CalendarStyles.UnSelected]);
                    screenBuffer.WriteLine($" {item}", _optStyles[CalendarStyles.UnSelected]);
                }
            }

            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[CalendarStyles.Pagination]);
            }
        }

        private void WriteCalendar(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.Input)
            {
                return;
            }

            switch (_layout)
            {
                case CalendarLayout.AsciiSingleGrid:

                    WriteBufferCalendar(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight, false)[0]);
                    break;
                case CalendarLayout.SingleGrid:
                    WriteBufferCalendar(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight, true)[0]);
                    break;
                case CalendarLayout.AsciiDoubleGrid:
                    WriteBufferCalendar(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight, false)[0]);
                    break;
                case CalendarLayout.DoubleGrid:
                    WriteBufferCalendar(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft, true)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight, true)[0]);
                    break;
                default:
                    throw new NotImplementedException($"Layout: {_layout} Not Implemented");
            }
        }

        private void WriteBufferCalendar(BufferScreen screenBuffer, char dividerx, char topleft, char topright, char borderleft, char borderright, char bottomleft, char bottomright)
        {
            DateOnly refcalendar = new(_currentDate.Year, _currentDate.Month, 1);
            string curmonth = refcalendar.ToString("MMMM", _culture).PadRight(28);
            string curyear = refcalendar.ToString("yyyy", _culture);
            curmonth = $"{curmonth[..1].ToUpperInvariant()}{curmonth[1..]}";

            string line = new(dividerx, 35);

            screenBuffer.Write(topleft, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(line, _optStyles[CalendarStyles.Lines]);
            screenBuffer.WriteLine(topright, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write($" {curmonth}", _optStyles[CalendarStyles.CalendarMonth]);
            screenBuffer.Write($" {curyear} ", _optStyles[CalendarStyles.CalendarYear]);
            screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(line, _optStyles[CalendarStyles.CalendarMonth]);
            screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);

            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            foreach (DayOfWeek item in _weekdays)
            {
                string abr = _culture.DateTimeFormat.AbbreviatedDayNames[(int)item];
                abr = $"{abr[..1].ToUpperInvariant()}{abr[1..]}";
                if (abr.Length < 3)
                {
                    abr = abr.PadLeft(3, ' ');
                }
                if (abr.Length > 3)
                {
                    abr = abr[..3];
                }
                if (item == _currentDate.DayOfWeek)
                {
                    abr = $"{ConfigPlus.GetSymbol(SymbolType.CalendarTodayLeft, true)[0]}{abr}{ConfigPlus.GetSymbol(SymbolType.CalendarTodayRight, true)[0]}";
                    screenBuffer.Write(abr, _optStyles[CalendarStyles.Selected]);
                }
                else
                {
                    abr = $" {abr} ";
                    screenBuffer.Write(abr, _optStyles[CalendarStyles.CalendarWeekDay]);
                }
            }
            screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);

            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(line, _optStyles[CalendarStyles.CalendarMonth]);
            screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);

            DayOfWeek[] weekdays = CalendarControl.MonthWeekDays(refcalendar.Year, refcalendar.Month);

            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            foreach (DayOfWeek item in _weekdays)
            {
                if (item != weekdays[refcalendar.Day - 1])
                {
                    screenBuffer.Write("     ", _optStyles[CalendarStyles.Lines]);
                }
                else
                {
                    WriteDay(screenBuffer, refcalendar);
                    refcalendar = refcalendar.AddDays(1);
                }
            }
            screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);
            while (refcalendar.Month == _currentDate.Month)
            {
                screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
                for (int i = 0; i < 7; i++)
                {
                    if (refcalendar.Month == _currentDate.Month)
                    {
                        WriteDay(screenBuffer, refcalendar);
                        refcalendar = refcalendar.AddDays(1);
                    }
                    else
                    {
                        screenBuffer.Write("     ", _optStyles[CalendarStyles.Lines]);
                    }
                }
                screenBuffer.WriteLine(borderright, _optStyles[CalendarStyles.Lines]);
            }

            screenBuffer.Write(bottomleft, _optStyles[CalendarStyles.Lines]);
            screenBuffer.Write(line, _optStyles[CalendarStyles.Lines]);
            screenBuffer.WriteLine(bottomright, _optStyles[CalendarStyles.Lines]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_selectedDate!.Value) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[CalendarStyles.Description]);
            }
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[CalendarStyles.Prompt]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[CalendarStyles.Error]);
                ClearError();
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            Style styleAnswer = _modeView != ModeView.Input
                ? _optStyles[CalendarStyles.TaggedInfo]
                : _optStyles[CalendarStyles.Answer];

            if (!_selectedDate.HasValue)
            {
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine("", styleAnswer);
                return;
            }
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft), styleAnswer);
            screenBuffer.Write(_selectedDate.Value.ToString("d"), styleAnswer);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight), styleAnswer);
            screenBuffer.WriteLine(" ", styleAnswer);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            else
            {
                tooltip = _modeView == ModeView.Input
                    ? _tooltipModeInput
                    : _modeView == ModeView.ShowNotes
                    ? _tooltipModeShowNote
                    : throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
            screenBuffer.Write(tooltip!, _optStyles[CalendarStyles.Tooltips]);
        }

        private void LoadTooltipToggle()
        {
            if (IsWidgetControl)
            {
                return;
            }

            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips = [];
                if (IAnyEvent())
                {
                    lsttooltips.Add(Messages.TooltipPages);
                }
                if (GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}");
                }
                else
                {
                    lsttooltips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}");
                }
                if (mode == ModeView.Input)
                {
                    lsttooltips.Add(Messages.MoveDays);
                    lsttooltips.Add(Messages.MoveDayWeek);
                    lsttooltips.Add(Messages.MoveMonth);
                    lsttooltips.Add(Messages.MoveYear);
                    lsttooltips.Add(Messages.MoveToday);
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipModeShowNotes()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(string.Format(Messages.TooltipToggleNotes, _hotKeySwitchNotes!.Value));
            return tooltip.ToString();
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.InputFinishEnter);
            if (IAnyEvent())
            {
                tooltip.Append(", ");
                tooltip.Append(string.Format(Messages.TooltipToggleNotes, _hotKeySwitchNotes!.Value));
            }
            return tooltip.ToString();
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Input => _toggerTooptips[ModeView.Input][_indexTooptip - 1],
                ModeView.ShowNotes => _toggerTooptips[ModeView.ShowNotes][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };
        }

        private DayOfWeek[] GetWeekdays()
        {
            return [.. Enumerable.Range(0, 7).Select(i => (DayOfWeek)(((int)_firstdayOfWeek + i) % 7))];
        }

        private static DayOfWeek[] MonthWeekDays(int year, int month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            List<DayOfWeek> result = [];
            for (int day = 0; day < daysInMonth; day++)
            {
                result.Add(new DateTime(year, month, day + 1).DayOfWeek);
            }

            return [.. result];
        }

        private void WriteDay(BufferScreen screenBuffer, DateOnly refdate)
        {
            DateOnly localcurretdate = DateOnly.FromDateTime(_currentDate);
            string cday = refdate.ToString("dd");
            string strnote = IsNote(refdate) ?
                IsHighlight(refdate) ? ConfigPlus.GetSymbol(SymbolType.CalendarNoteHighlight, true) : ConfigPlus.GetSymbol(SymbolType.CalendarNote, true) :
                IsHighlight(refdate) ? ConfigPlus.GetSymbol(SymbolType.CalendarHighlight, true) : " ";
            CalendarStyles style = CalendarStyles.CalendarDay;

            if (IsDateDisable(refdate))
            {
                style = CalendarStyles.Disabled;
            }
            else if (refdate == localcurretdate)
            {
                style = CalendarStyles.Selected;
            }
            else if (IsHighlight(refdate))
            {
                style = CalendarStyles.CalendarHighlight;
            }
            string formattedDay = refdate == localcurretdate
                ? $"{strnote}{ConfigPlus.GetSymbol(SymbolType.CalendarTodayLeft, true)[0]}{cday}{ConfigPlus.GetSymbol(SymbolType.CalendarTodayRight, true)[0]}"
                : $" {strnote}{cday} ";

            screenBuffer.Write(formattedDay, _optStyles[style]);
        }

        private bool IsDateDisable(DateOnly date)
        {
            return _disabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) || _itemscope.Any(x => x.Date == date && x.Scope == CalendarItem.Disabled);
        }

        private bool IsNote(DateOnly date)
        {
            return _itemscope.Any(x => x.Date == date && x.Scope == CalendarItem.Note);
        }

        private bool IsHighlight(DateOnly date)
        {
            return _itemscope.Any(x => x.Date == date && x.Scope == CalendarItem.Highlight);
        }

        private bool IAnyEvent()
        {
            _isAnyEvent = _itemscope.Any(x => x.Scope == CalendarItem.Note);
            return _isAnyEvent;
        }

        private enum ModeView
        {
            Input,
            ShowNotes
        }

        #endregion
    }
}
