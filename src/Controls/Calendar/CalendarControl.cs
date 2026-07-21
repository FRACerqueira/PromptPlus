// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Calendar
{
    internal sealed class CalendarControl : BaseControlPrompt<DateTime?>, ICalendarControl, ICalendarWidget
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer line, optional error/group line, optional description line,
        /// tooltip line and an extra row for the pagination footer when active.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 16;

        // Cached composite format string for improved performance
        private static readonly CompositeFormat s_showingCalendarNotesFormat = CompositeFormat.Parse(PromptPlusResources.ShowingCalendarNotes);


        private readonly Dictionary<CalendarStyles, Style> _optStyles;
        private readonly HashSet<(CalendarItem Scope, DateOnly Date, string scopetext)> _itemscope = [];
        private readonly HashSet<DateOnly> _disabledDates = [];
        private readonly HashSet<DateOnly> _highlightDates = [];
        private readonly HashSet<DateOnly> _noteDates = [];
        private readonly Dictionary<DateOnly, List<string>> _notesByDate = [];
        private bool _hasAnyNote;
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Input,[] },
            { ModeView.ShowNotes,[] },
        };

        private CultureInfo _culture;
        private CalendarLayout _layout = CalendarLayout.SingleGrid;
        private DayOfWeek _firstdayOfWeek;
        private Paginator<(string uniqueID, string note)>? _localpaginator;
        private int _indexTooptip;
        private Func<DateTime?, (bool, string?)>? _predicatevalidselect;
        private Func<DateTime?, Task<(bool, string?)>>? _predicatevalidselectAsync;
        private Func<DateTime?, string>? _changeDescription;
        private Func<DateTime?, Task<string>>? _changeDescriptionAsync;
        private DateTime? _defaultValue;
        private bool _disabledWeekend;
        private DateTime _minRangeDate = DateTime.MinValue;
        private DateTime _maxRangeDate = DateTime.MaxValue;
        private DateTime _currentDate = DateTime.Today;
        private ModeView _modeView = ModeView.Input;
        private DateTime? _selectedDate;
        private DayOfWeek[] _weekdays = [];
        private byte _pageSize;
        private int _effectivePageSize;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private readonly EmacsConsoleBuffer _noteBuffer ;
        private IList<ItemHistory>? _itemHistories;
        private (string uniqueID, string note)[] _notesViewCache = [];
        private int _lastPaginatorConsoleWidth = -1;
        private int _lastPaginatorConsoleHeight = -1;

        public CalendarControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<CalendarStyles>(console.CurrentStyle);
            _firstdayOfWeek = ConfigPrompt.FirstDayOfWeek;
            _culture = ConfigPrompt.DefaultCulture;
            _pageSize = ConfigPrompt.PageSize;
            _noteBuffer = new(true, CaseOptions.Any, ConfigPrompt.EmacsKeyBindings, (_) => true);
        }

        #region ICalendarControl, ICalendarWidget

        /// <inheritdoc/>
        public ICalendarControl Layout(CalendarLayout layout = CalendarLayout.SingleGrid)
        {
            _layout = layout;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            _culture = culture;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl FirstDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            _firstdayOfWeek = firstDayOfWeek;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl DisabledWeekend(bool value = true)
        {
            _disabledWeekend = value;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl DisableDates(params DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            foreach (DateTime item in dates)
            {
                AddScopeItem(CalendarItem.Disabled, DateOnly.FromDateTime(item), string.Empty);
            }
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl AddNote(DateTime value, string? note = null)
        {
            AddScopeItem(CalendarItem.Note, DateOnly.FromDateTime(value), note ?? string.Empty);
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl AddNotes((DateTime, string?)[] notes)
        {
            ArgumentNullException.ThrowIfNull(notes);
            foreach (var (date, note) in notes)
            {
                AddScopeItem(CalendarItem.Note, DateOnly.FromDateTime(date), note ?? string.Empty);
            }
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Highlights(params DateTime[] dates)
        {
            foreach (DateTime item in dates)
            {
                AddScopeItem(CalendarItem.Highlight, DateOnly.FromDateTime(item), string.Empty);
            }
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Styles(CalendarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Range(DateTime minValue, DateTime maxValue)
        {
            DateTime minDate = minValue.Date;
            DateTime maxDate = maxValue.Date;
            if (minDate > maxDate)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than max value.");
            }
            _minRangeDate = minDate;
            _maxRangeDate = maxDate == DateTime.MaxValue.Date
                ? DateTime.MaxValue
                : maxDate.AddDays(1).AddTicks(-1);
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl ChangeDescription(Func<DateTime?, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl ChangeDescriptionAsync(Func<DateTime?, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Default(DateTime value, bool useDefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            }
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }


        /// <inheritdoc/>
        public ICalendarControl PageSize(byte value)
        {
            // value == 0 means "auto-fit to console height" (see ComputeEffectivePageSize).
            // Any positive value is the user's preferred maximum and is later clamped to the
            // height available on screen.
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl Interaction<T>(IEnumerable<T> items, Action<T, ICalendarControl> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl InteractionAsync<T>(IEnumerable<T> items, Func<T, ICalendarControl, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T item in items)
            {
                interactionAction.Invoke(item, this)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl PredicateSelected(Func<DateTime?, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl PredicateSelected(Func<DateTime?, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) => (validselect(input), (string?)null);
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = validselect;
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidselect = null;
            return this;
        }


        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.Layout(CalendarLayout layout)
        {
            _layout = layout;
            return this;
        }

        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.DisableDates(params DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            foreach (DateTime item in dates)
            {
                AddScopeItem(CalendarItem.Disabled, DateOnly.FromDateTime(item), string.Empty);
            }
            return this;
        }

        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.Highlights(params DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            foreach (DateTime item in dates)
            {
                AddScopeItem(CalendarItem.Highlight, DateOnly.FromDateTime(item), string.Empty);
            }
            return this;
        }

        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            _culture = culture;
            return this;
        }

        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.FirstDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            _firstdayOfWeek = firstDayOfWeek;
            return this;
        }

        /// <inheritdoc/>
        ICalendarWidget ICalendarWidget.Styles(CalendarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (IsWidget)
            {
                _historyOptions = null;
            }
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out DateTime? historyValue))
                    {
                        _defaultValue = historyValue;
                    }
                }
            }

            if (_defaultValue.HasValue)
            {
                if (IsDateInRange(_defaultValue.Value))
                {
                    _currentDate = _defaultValue.Value;
                }
            }
            _weekdays = GetWeekdays();
            if (IsValidSelect(DateOnly.FromDateTime(_currentDate)) && TrySelectionPredicate(_currentDate))
            {
                _selectedDate = _currentDate;
            }
            if (!IsWidget)
            {
                LoadTooltipToggle();
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            _modeView = ModeView.Input;
                            ResultCtrl = new ResultPrompt<DateTime?>(default!, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.ShowNotes)
                        {
                            _localpaginator = null;
                            _notesViewCache = [];
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
                            _notesViewCache = [];
                            _modeView = ModeView.Input;
                        }
                        if (!_selectedDate.HasValue)
                        {
                            SetError(PromptPlusResources.InvalidDateSelect);
                            break;
                        }
                        (bool ok, string? message) = ValidateSelection();
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(PromptPlusResources.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        ResultCtrl = new ResultPrompt<DateTime?>(_selectedDate, false);
                        SaveHistory();
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

                    else if (!IsWidget &&  _modeView == ModeView.Input && ConfigPrompt.HotKeyCalendarSwitchNotes.Equals(keyinfo) && IsNote(DateOnly.FromDateTime(_currentDate)))
                    {
                        _indexTooptip = 0;
                        EnsurePaginatorPageSizeUpToDate();
                        DateOnly localdateref = DateOnly.FromDateTime(_currentDate);
                        _notesViewCache = GetNotes(localdateref);
                        _localpaginator = new Paginator<(string uniqueID, string note)>(
                            FilterMode.Disabled,
                            _notesViewCache,
                            _effectivePageSize,
                            Optional<(string,string)>.Empty(),
                            (item1, item2) => item1.uniqueID == item2.uniqueID);

                        if (_localpaginator!.SelectedIndex >=0 )
                        {
                            _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                            _noteBuffer.ToHome();
                        }
                        _modeView = ModeView.ShowNotes;
                        _lastPaginatorConsoleWidth = ConsoleHandler.Width;
                        _lastPaginatorConsoleHeight = ConsoleHandler.Height;
                        break;
                    }
                    else if (_modeView == ModeView.ShowNotes)
                    {
                         if (keyinfo.KeyChar == '\t')
                        {
                            _indexTooptip = 0;
                            continue;
                        }
                        if (ConfigPrompt.HotKeyCalendarSwitchNotes.Equals(keyinfo))
                        {
                            _indexTooptip = 0;
                            _localpaginator = null;
                            _notesViewCache = [];
                            _noteBuffer.Clear();
                            _modeView = ModeView.Input;
                            break;
                        }
                        else
                        {
                            EnsurePaginatorPageSizeUpToDate();
                        }
                        if (keyinfo.IsPressDownArrowKey())
                        {
                            bool ok = _localpaginator!.IsLastPageItem ? _localpaginator.NextPage(IndexOption.FirstItem) : _localpaginator.NextItem();
                            if (ok)
                            {
                                _indexTooptip = 0;
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
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
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
                                break;
                            }
                            continue;
                        }
                        else if (keyinfo.IsPressCtrlHomeKey())
                        {
                            if (_localpaginator!.Home())
                            {
                                _indexTooptip = 0;
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
                                break;
                            }
                        }
                        else if (keyinfo.IsPressCtrlEndKey())
                        {
                            if (_localpaginator!.End())
                            {
                                _indexTooptip = 0;
                                _noteBuffer.LoadPrintable(_localpaginator.SelectedItem.note);
                                _noteBuffer.ToHome();
                                break;
                            }
                        }
                        else if (!_noteBuffer!.IsPrintable(keyinfo.KeyChar) && _noteBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                        {
                            _indexTooptip = 0;
                            break;
                        }
                        else if (_noteBuffer!.IsPrintable(keyinfo.KeyChar))
                        {
                            char keyCharUpper = char.ToUpperInvariant(keyinfo.KeyChar);
                            int start = _localpaginator!.CurrentIndex;
                            // Use the cached item text instead of re-invoking the (possibly async) text
                            // selector for every item on each keystroke.
                            var notes = _notesViewCache;
                            for (int i = 0; i < notes.Length; i++)
                            {
                                if (i < start + 1)
                                {
                                    continue;
                                }
                                if (StartsWithIgnoreCase(notes[i].note, keyCharUpper))
                                {
                                    _localpaginator.EnsureVisibleIndex(i);
                                    _indexTooptip = 0;
                                    break;
                                }
                            }
                            if (start == _localpaginator!.CurrentIndex)
                            {
                                for (int i = 0; i < start; i++)
                                {
                                    if (StartsWithIgnoreCase(notes[i].note, keyCharUpper))
                                    {
                                        _localpaginator.EnsureVisibleIndex(i);
                                        _indexTooptip = 0;
                                        break;
                                    }
                                }
                            }
                            if (start != _localpaginator!.CurrentIndex)
                            {
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
                    else if (keyinfo.IsPressPageUpKey() && IsDateInRange(_currentDate.AddYears(1)))
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
                    else if (keyinfo.IsPressPageDownKey() && IsDateInRange(_currentDate.AddYears(-1)))
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
                    else if (keyinfo.IsPressTabKey() && IsDateInRange(_currentDate.AddMonths(1)))
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
                    else if (keyinfo.IsPressShiftTabKey() && IsDateInRange(_currentDate.AddMonths(-1)))
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
                    else if (keyinfo.IsPressDownArrowKey(true) && IsDateInRange(_currentDate.AddDays(7)))
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
                    else if (keyinfo.IsPressUpArrowKey(true) && IsDateInRange(_currentDate.AddDays(-7)))
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
                    else if (keyinfo.IsPressRightArrowKey(true) && IsDateInRange(_currentDate.AddDays(1)))
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
                    else if (keyinfo.IsPressLeftArrowKey(true) && IsDateInRange(_currentDate.AddDays(-1)))
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
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        private void SaveHistory()
        {
            if (_historyOptions == null)
            {
                return;
            }
            var selectedValue = _selectedDate;
            string serializedValue = JsonSerializer.Serialize(selectedValue);
            IList<ItemHistory> hist = _itemHistories ?? FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;

        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidget)
            {
                WritePrompt(screenBuffer, _optStyles[CalendarStyles.Prompt]);

                WriteAnswer(screenBuffer);

                WriteDescription(screenBuffer);

            }

            WriteCalendar(screenBuffer);

            if (!IsWidget)
            {
                WriteNotes(screenBuffer);

                WriteTooltip(screenBuffer);

                WriteError(screenBuffer, _optStyles[CalendarStyles.Error]);
            }
        }

        private (string,string)[] GetNotes(DateOnly currentDate)
        {
            if (!_notesByDate.TryGetValue(currentDate, out List<string>? notes) || notes.Count == 0)
            {
                return [];
            }

            (string, string)[] result = new (string, string)[notes.Count];
            for (int i = 0; i < notes.Count; i++)
            {
                result[i] = (i.ToString(CultureInfo.InvariantCulture), notes[i]);
            }

            return result;
        }

        private (bool ok, string? message) ValidateSelection()
        {
            if (_predicatevalidselectAsync is not null)
            {
                return _predicatevalidselectAsync
                    .Invoke(_selectedDate)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }

            return _predicatevalidselect?.Invoke(_selectedDate) ?? (true, null);
        }

        /// <summary>
        /// Evaluates the optional selection predicate for <paramref name="value"/>, returning
        /// <c>true</c> when no predicate is configured or when it accepts the value. Used to decide
        /// whether a default/history date may be pre-selected (rejected values are not honored).
        /// </summary>
        private bool TrySelectionPredicate(DateTime? value)
        {
            if (_predicatevalidselect == null && _predicatevalidselectAsync == null)
            {
                return true;
            }
            (bool ok, _) = _predicatevalidselectAsync != null
                ? _predicatevalidselectAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidselect?.Invoke(value) ?? (true, (string?)null));
            return ok;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (_selectedDate.HasValue && !ResultCtrl!.Value.IsAborted)
            {
                answer = ResultCtrl!.Value.Content!.Value.ToString("d", _culture);
            }

            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.EnabledAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            WritePrompt(screenBuffer, _optStyles[CalendarStyles.Prompt]);
            screenBuffer.WriteLine(answer, _optStyles[CalendarStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void WriteNotes(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.ShowNotes)
            {
                return;
            }
            EnsurePaginatorPageSizeUpToDate();

            ArraySegment<(string uniqueID, string note)> subset = _localpaginator!.GetPageData(); // Cache the page data
            string selectorSymbol = GetSymbol(SymbolType.Selector);
            screenBuffer.WriteLine(string.Format(_culture, s_showingCalendarNotesFormat, _currentDate.ToString("d", _culture)), _optStyles[CalendarStyles.Tooltips]);
            foreach ((string uniqueID, string note) in subset)
            {

                if (_localpaginator.SelectedIndex >= 0 && uniqueID == _localpaginator.SelectedItem.uniqueID)
                {
                    screenBuffer.Write(selectorSymbol, _optStyles[CalendarStyles.Selected]);
                    screenBuffer.Write(' ', _optStyles[CalendarStyles.Selected]);
                    screenBuffer.WriteLine(note, _optStyles[CalendarStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", _optStyles[CalendarStyles.UnSelected]);
                    screenBuffer.Write(' ', _optStyles[CalendarStyles.UnSelected]);
                    screenBuffer.WriteLine(note, _optStyles[CalendarStyles.UnSelected]);
                }
            }
            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
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
                       GetSymbol(SymbolType.GridSingleDividerX, false)[0],
                       GetSymbol(SymbolType.GridSingleTopLeft, false)[0],
                       GetSymbol(SymbolType.GridSingleTopRight, false)[0],
                       GetSymbol(SymbolType.GridSingleBorderLeft, false)[0],
                       GetSymbol(SymbolType.GridSingleBorderRight, false)[0],
                       GetSymbol(SymbolType.GridSingleBottomLeft, false)[0],
                       GetSymbol(SymbolType.GridSingleBottomRight, false)[0]);
                    break;
                case CalendarLayout.SingleGrid:
                    WriteBufferCalendar(screenBuffer,
                       GetSymbol(SymbolType.GridSingleDividerX)[0],
                       GetSymbol(SymbolType.GridSingleTopLeft)[0],
                       GetSymbol(SymbolType.GridSingleTopRight)[0],
                       GetSymbol(SymbolType.GridSingleBorderLeft)[0],
                       GetSymbol(SymbolType.GridSingleBorderRight)[0],
                       GetSymbol(SymbolType.GridSingleBottomLeft)[0],
                       GetSymbol(SymbolType.GridSingleBottomRight)[0]);
                    break;
                case CalendarLayout.AsciiDoubleGrid:
                    WriteBufferCalendar(screenBuffer,
                       GetSymbol(SymbolType.GridDoubleDividerX,false)[0],
                       GetSymbol(SymbolType.GridDoubleTopLeft,false)[0],
                       GetSymbol(SymbolType.GridDoubleTopRight, false)[0],
                       GetSymbol(SymbolType.GridDoubleBorderLeft,false)[0],
                       GetSymbol(SymbolType.GridDoubleBorderRight,false)[0],
                       GetSymbol(SymbolType.GridDoubleBottomLeft,false)[0],
                       GetSymbol(SymbolType.GridDoubleBottomRight,false)[0]);
                    break;
                case CalendarLayout.DoubleGrid:
                    WriteBufferCalendar(screenBuffer,
                        GetSymbol(SymbolType.GridDoubleDividerX)[0],
                        GetSymbol(SymbolType.GridDoubleTopLeft)[0],
                        GetSymbol(SymbolType.GridDoubleTopRight)[0],
                        GetSymbol(SymbolType.GridDoubleBorderLeft)[0],
                        GetSymbol(SymbolType.GridDoubleBorderRight)[0],
                        GetSymbol(SymbolType.GridDoubleBottomLeft)[0],
                        GetSymbol(SymbolType.GridDoubleBottomRight)[0]);
                    break;
                default:
                    throw new NotImplementedException($"Layout: {_layout} Not Implemented");
            }
        }

        private void WriteBufferCalendar(BufferScreen screenBuffer, char dividerx, char topleft, char topright, char borderleft, char borderright, char bottomleft, char bottomright)
        {
            DateOnly refcalendar = new(_currentDate.Year, _currentDate.Month, 1);
            DateOnly currentDateOnly = DateOnly.FromDateTime(_currentDate);
            DayOfWeek currentWeekDay = refcalendar.DayOfWeek;
            char calendarTodayLeft = GetSymbol(SymbolType.CalendarTodayLeft)[0];
            char calendarTodayRight = GetSymbol(SymbolType.CalendarTodayRight)[0];
            string calendarNoteHighlight = GetSymbol(SymbolType.CalendarNoteHighlight);
            string calendarNote = GetSymbol(SymbolType.CalendarNote);
            string calendarHighlight = GetSymbol(SymbolType.CalendarHighlight);
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
                    abr = $"{calendarTodayLeft}{abr}{calendarTodayRight}";
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

            screenBuffer.Write(borderleft, _optStyles[CalendarStyles.Lines]);
            foreach (DayOfWeek item in _weekdays)
            {
                if (item != currentWeekDay)
                {
                    screenBuffer.Write("     ", _optStyles[CalendarStyles.Lines]);
                }
                else
                {
                    WriteDay(screenBuffer, refcalendar, currentDateOnly, calendarTodayLeft, calendarTodayRight, calendarNoteHighlight, calendarNote, calendarHighlight);
                    refcalendar = refcalendar.AddDays(1);
                    currentWeekDay = refcalendar.DayOfWeek;
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
                        WriteDay(screenBuffer, refcalendar, currentDateOnly, calendarTodayLeft, calendarTodayRight, calendarNoteHighlight, calendarNote, calendarHighlight);
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

        private void WriteDay(
            BufferScreen screenBuffer,
            DateOnly refdate,
            DateOnly currentDate,
            char calendarTodayLeft,
            char calendarTodayRight,
            string calendarNoteHighlight,
            string calendarNote,
            string calendarHighlight)
        {
            bool isCurrentDate = refdate == currentDate;
            bool isHighlightedDate = IsHighlight(refdate);
            bool hasNote = IsNote(refdate);
            bool isDisabledDate = IsDateOutOfRange(refdate) || IsDateDisable(refdate);
            string cday = refdate.ToString("dd", _culture);
            string strnote = hasNote
                ? isHighlightedDate ? calendarNoteHighlight : calendarNote
                : isHighlightedDate ? calendarHighlight : " ";
            CalendarStyles style = CalendarStyles.CalendarDay;

            if (isDisabledDate)
            {
                style = CalendarStyles.Disabled;
            }
            else if (isCurrentDate)
            {
                style = CalendarStyles.Selected;
            }
            else if (isHighlightedDate)
            {
                style = CalendarStyles.CalendarHighlight;
            }
            string formattedDay = isCurrentDate
                ? $"{strnote}{calendarTodayLeft}{cday}{calendarTodayRight}"
                : $" {strnote}{cday} ";

            screenBuffer.Write(formattedDay, _optStyles[style]);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[CalendarStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            switch (_modeView)
            {
                case ModeView.Input:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.Input].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.Input][_indexTooptip];
                    }
                case ModeView.ShowNotes:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.ShowNotes].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.ShowNotes][_indexTooptip];
                    }
                default:
                    throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
            ;
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (IsValidSelect(DateOnly.FromDateTime(_currentDate)))
            {
                if (_changeDescriptionAsync is not null)
                {
                    desc = _changeDescriptionAsync.Invoke(_currentDate)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }
                else
                {
                    desc = _changeDescription?.Invoke(_currentDate) ?? OptionsControl.DescriptionValue;
                }
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[CalendarStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.ShowNotes)
            {
                int promptWidth = GetPromptDisplayWidth();
                (string visibleLeft, string visibleRight) = ViewportSlice(_noteBuffer!, promptWidth);
                screenBuffer.Write(visibleLeft, _optStyles[CalendarStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine(visibleRight, _optStyles[CalendarStyles.Answer]);
            }
            else
            {
                if (!_selectedDate.HasValue)
                {
                    screenBuffer.SavePromptCursor();
                    screenBuffer.WriteLine("", _optStyles[CalendarStyles.Answer]);
                    return;
                }
                screenBuffer.Write(_selectedDate.Value.ToString("d", _culture), _optStyles[CalendarStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine("", _optStyles[CalendarStyles.Answer]);
            }
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    GetTooltipSelect()
                ];
                if (mode == ModeView.Input)
                {
                    lsttooltips.Add(PromptPlusResources.MoveDays);
                    lsttooltips.Add(PromptPlusResources.MoveDayWeek);
                    lsttooltips.Add(PromptPlusResources.MoveMonth);
                    lsttooltips.Add(PromptPlusResources.MoveYear);
                    lsttooltips.Add(PromptPlusResources.MoveToday);
                }
                if (mode == ModeView.ShowNotes)
                {
                    lsttooltips.Add(PromptPlusResources.TooltipPages);
                    lsttooltips.Add(PromptPlusResources.TooltipNavegateTextPrompt);
                    lsttooltips.Add(PromptPlusResources.TooltipJump);
                    lsttooltips.AddRange(GetEmacsTooltips(true));

                }
                if (OptionsControl.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                }
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            if (!IsWidget && IAnyNote())
            {
                tooltip.Append(_culture, $"{ConfigPrompt.HotKeyCalendarSwitchNotes}:{PromptPlusResources.TooltipToggleNotes}");
                tooltip.Append('.');
            }

            return tooltip.ToString();
        }

        private bool IsValidToday()
        {
            return IsDateInRange(DateTime.Today);
        }

        private bool IsValidSelect(DateOnly date)
        {
            if (IsDateOutOfRange(date))
            {
                return false;
            }
            return !IsDateDisable(date);
        }

        private bool IsDateOutOfRange(DateOnly date)
        {
            DateOnly minDate = DateOnly.FromDateTime(_minRangeDate);
            DateOnly maxDate = DateOnly.FromDateTime(_maxRangeDate);
            return date < minDate || date > maxDate;
        }

        private bool IsDateInRange(DateTime dateTime)
        {
            return !IsDateOutOfRange(DateOnly.FromDateTime(dateTime));
        }

        private bool IsDateDisable(DateOnly date)
        {
            return (_disabledWeekend && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)) || _disabledDates.Contains(date);
        }

        private bool IsNote(DateOnly date)
        {
            return _noteDates.Contains(date);
        }

        private bool IsHighlight(DateOnly date)
        {
            return _highlightDates.Contains(date);
        }

        private bool IAnyNote()
        {
            return _hasAnyNote;
        }

        private void EnsurePaginatorPageSizeUpToDate()
        {
            int currentWidth = ConsoleHandler.Width;
            int currentHeight = ConsoleHandler.Height;
            if (currentWidth == _lastPaginatorConsoleWidth && currentHeight == _lastPaginatorConsoleHeight)
            {
                return;
            }

            _lastPaginatorConsoleWidth = currentWidth;
            _lastPaginatorConsoleHeight = currentHeight;

            int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            if (targetPageSize == _effectivePageSize)
            {
                return;
            }

            _effectivePageSize = targetPageSize;
            _localpaginator?.UpdatePageSize(_effectivePageSize);
        }

        private static bool StartsWithIgnoreCase(string value, char uppercaseMatch)
        {
            return !string.IsNullOrEmpty(value) && char.ToUpperInvariant(value[0]) == uppercaseMatch;
        }

        private void AddScopeItem(CalendarItem scope, DateOnly date, string scopeText)
        {
            if (!_itemscope.Add((scope, date, scopeText)))
            {
                return;
            }

            switch (scope)
            {
                case CalendarItem.Disabled:
                    _disabledDates.Add(date);
                    break;
                case CalendarItem.Highlight:
                    _highlightDates.Add(date);
                    break;
                case CalendarItem.Note:
                    _noteDates.Add(date);
                    _hasAnyNote = true;
                    if (!string.IsNullOrWhiteSpace(scopeText))
                    {
                        if (!_notesByDate.TryGetValue(date, out List<string>? notes))
                        {
                            notes = [];
                            _notesByDate[date] = notes;
                        }
                        notes.Add(scopeText);
                    }
                    break;
            }
        }

        private enum ModeView
        {
            Input,
            ShowNotes
        }
        private DayOfWeek[] GetWeekdays()
        {
            return [.. Enumerable.Range(0, 7).Select(i => (DayOfWeek)(((int)_firstdayOfWeek + i) % 7))];
        }


    }
}
