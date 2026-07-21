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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Select
{
    /// <inheritdoc/>
    internal sealed class SelectControl<T> : BaseControlPrompt<T>, ISelectControl<T>
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer line, optional error/group line, optional description line,
        /// tooltip line and an extra row for the pagination footer when active.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 7;

        private readonly Dictionary<SelectStyles, Style> _optStyles;
        private readonly EmacsConsoleBuffer _filterBuffer;
        private readonly List<ItemSelect<T>> _items = [];
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, Task<(bool, string?)>>? _predicatevalidselectAsync;
        private Func<T, string?>? _extraInfo;
        private Func<T, Task<string?>>? _extraInfoAsync;
        private int _sequence;
        private bool _autoSelect;
        private Func<T, string>? _changeDescription;
        private Func<T, Task<string>>? _changeDescriptionAsync;
        private Func<T, T, bool> _DefaultMatchBy = EqualityComparer<T>.Default.Equals;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;

        private FilterMode _filterType = FilterMode.Disabled;
        private byte _pageSize;
        private int _effectivePageSize;
        private bool _hideTipGroup;
        private Func<T, string>? _textSelector;
        private Func<T, Task<string>>? _textSelectorAsync;
        private Paginator<ItemSelect<T>>? _localpaginator;
        private enum ModeView
        {
            Select,
            Filter
        }
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select,[] },
            { ModeView.Filter,[] }
        };
        private ModeView _modeView = ModeView.Select;
        private int _indexTooptip;
        private int _lengthSeparationline;
        private string _lastinput = string.Empty;
        private bool _viewOnly;
        private EmacsConsoleBuffer? _answerBuffer;
        private bool _updatePosAnswerBuffer;

        public SelectControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<SelectStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
            _filterBuffer = new(false, CaseOptions.Any, ConfigPrompt.EmacsKeyBindings, (_) => true);
        }


        #region ISelectControl

        /// <inheritdoc/>
        public ISelectControl<T> ViewOnly(bool value = true)
        {
            _viewOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfo = extraInfoNode;
            _extraInfoAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoAsync = extraInfoNode;
            _extraInfo = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) => (validselect(input), (string?)null);
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = validselect;
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
            _DefaultMatchBy = comparer;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AddItem(T value, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, disable));
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            foreach (T? value in values)
            {
                AddItem(value, disable);
            }
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ISelectControl<T>, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T1 item in items)
            {
                interactionAction.Invoke(item, this)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            int lastindex = _items.FindLastIndex((x) => x.Group == group);
            if (lastindex < 0)
            {
                _sequence++;
                _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, disable)
                {
                    Group = group,
                    IsFirstItemGroup = true,
                    IsLastItemGroup = true
                });
            }
            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, disable)
            {
                Group = group,
                IsLastItemGroup = true
            });
            while (lastindex >= 0)
            {
                if (_items[lastindex].Group != group)
                {
                    break;
                }
                _items[lastindex].IsLastItemGroup = false;
                lastindex--;
            }
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            foreach (T? value in values)
            {
                AddGroupedItem(group, value, disable);
            }
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AutoSelect(bool value = true)
        {
            _autoSelect = value;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> Default(T value, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _defaultValue = Optional<T>.Set(value);
            _useDefaultHistory = useDefaultHistory;
            return this;
        }
        
        /// <inheritdoc/>
        public ISelectControl<T> UseDefaultHistory()
        {
            _defaultValue = Optional<T>.Empty();
            _useDefaultHistory = true;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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
        public ISelectControl<T> Filter(FilterMode value)
        {
            _filterType = value;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ISelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T1 item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> PageSize(byte value)
        {
            // value == 0 means "auto-fit to console height" (see ComputeEffectivePageSize).
            // Any positive value is the user's preferred maximum and is later clamped to the
            // height available on screen.
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
        {
            char separator = separatorLine switch
            {
                SeparatorLine.SingleLine => GetSymbol(SymbolType.SingleBorder)[0],
                SeparatorLine.DoubleLine => GetSymbol(SymbolType.DoubleBorder)[0],
                SeparatorLine.UserChar => value ?? throw new ArgumentNullException(nameof(value), "Char separator is null"),
                _ => throw new ArgumentOutOfRangeException(nameof(separatorLine), "SeparatorLine not supported")
            };
            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), default!, true)
            {
                CharSeparation = separator,
                Text = ""
            });
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> HideTipGroup(bool value = true)
        {
            _hideTipGroup = value;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> Styles(SelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            _textSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ISelectControl<T> TextSelectorAsync(Func<T, Task<string>> value)
        {
            _textSelectorAsync = value ?? throw new ArgumentNullException(nameof(value), "TextSelectorAsync is null");
            _textSelector = null;
            return this;
        }

        #endregion

        /// <inheritdoc/>
        public override void InitControl(CancellationToken cancellationToken)
        {
            _answerBuffer = new(true, CaseOptions.Any, ConfigPrompt.EmacsKeyBindings, (_) => true);
            _updatePosAnswerBuffer = true;
            if (typeof(T).IsEnum)
            {
                if (_textSelectorAsync is null)
                {
                    _textSelector ??= EnumDisplay;
                }
                if (_items.Count == 0)
                {
                    LoadEnum();
                }
            }
            else
            {
                if (_textSelectorAsync is null)
                {
                    _textSelector ??= (x) => x?.ToString() ?? string.Empty;
                }
                foreach (ItemSelect<T>? item in _items.Where(x => !x.CharSeparation.HasValue))
                {
                    item.Text = GetItemText(item.Value);
                    if (item.Text.Length > _lengthSeparationline)
                    {
                        _lengthSeparationline = item.Text.Length;
                    }
                    int groupLen = (item.Group ?? string.Empty).Length;
                    if (groupLen > _lengthSeparationline)
                    {
                        _lengthSeparationline = groupLen;
                    }
                }
            }
            if (_viewOnly)
            {
                _historyOptions = null;
                _autoSelect = false;
            }
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out T historyValue))
                    {
                        _defaultValue = Optional<T>.Set(historyValue);
                    }
                }
            }

            Optional<ItemSelect<T>> defvaluepage = Optional<ItemSelect<T>>.Empty();

            if (_defaultValue.HasValue)
            {
                ItemSelect<T>? found = _items.FirstOrDefault(x => !x.Disabled && !x.CharSeparation.HasValue && _DefaultMatchBy.Invoke(x.Value!, _defaultValue.Value));
                // Honor the selection predicate: a default/history value rejected by the predicate
                // does not position the cursor on it.
                if (found != null && TrySelectionPredicate(found.Value))
                {
                    defvaluepage = Optional<ItemSelect<T>>.Set(found);
                }
            }

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);

            _localpaginator = new Paginator<ItemSelect<T>>(
                _filterType,
                _items,
                _effectivePageSize,
                defvaluepage,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.IsFirstItemGroup ? item.Group! : item.Text!,
                (item) => !item.CharSeparation.HasValue && !item.IsFirstItemGroup,
                (item) => !item.CharSeparation.HasValue && !item.IsFirstItemGroup);

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (!_viewOnly && _localpaginator!.SelectedIndex >= 0 && _localpaginator.SelectedItem!.Disabled)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
            LoadTooltipToggle();
        }

        /// <inheritdoc/>
        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            // Re-evaluate the effective page size every frame so the visible items count
            // stays in sync with the current console height (after any terminal resize).
            int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localpaginator?.UpdatePageSize(_effectivePageSize);
            }

            WritePrompt(screenBuffer, _optStyles[SelectStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteGroupDescription(screenBuffer);
            WriteDescription(screenBuffer);
            WriteListSelect(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[SelectStyles.Error]);
        }

        /// <inheritdoc/>
        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            _modeView = ModeView.Select;
                            ResultCtrl = new ResultPrompt<T>(default!, true);
                        }
                        break;
                    }
                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip
                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = _localpaginator!.SelectedItem != null
                            ? new ResultPrompt<T>(_localpaginator.SelectedItem.Value!, true)
                            : new ResultPrompt<T>(default!, true);
                        break;
                    }
                    else if (keyinfo.IsPressTabKey() || (keyinfo.IsPressFilterActivationKey() && _localpaginator!.SelectedItem != null))
                    {
                        _indexTooptip = 0;
                        _updatePosAnswerBuffer = false;
                        continue;
                    }
                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        if (_viewOnly)
                        {
                            _modeView = ModeView.Select;
                            ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem.Value, false);
                            break;
                        }
                        if (_localpaginator.SelectedItem.Disabled)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        (bool ok, string? message) = _predicatevalidselectAsync is not null
                            ? _predicatevalidselectAsync.Invoke(_localpaginator!.SelectedItem.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                            : _predicatevalidselect?.Invoke(_localpaginator!.SelectedItem.Value) ?? (true, null);
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
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem.Value, false);
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

                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                        {
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localpaginator.NextItem();
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator!.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator!.PreviousItem();
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            SetSelectionDisabledErrorIfNeeded();
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            SetSelectionDisabledErrorIfNeeded();
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        if (!_localpaginator!.Home())
                        {
                            continue;
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        if (!_localpaginator!.End())
                        {
                            continue;
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        UpdateFilterFromBuffer();
                        if (TryAutoSelectSingleItem())
                        {
                            break;
                        }
                        SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Select && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        var keifilter = keyinfo;
                        if (keifilter.IsPressFilterActivationKey())
                        {
                            keifilter = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        }
                        if (_filterBuffer.TryAcceptedReadlineConsoleKey(keifilter))
                        {
                            _modeView = ModeView.Filter;
                            UpdateFilterFromBuffer();
                            if (TryAutoSelectSingleItem())
                            {
                                break;
                            }
                            SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
                        break;
                    }
                    else if (_modeView == ModeView.Select  && _localpaginator!.SelectedItem != null && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        string keyChar = keyinfo.KeyChar.ToString();
                        int start = _localpaginator.CurrentIndex;
                        // Use the cached item text instead of re-invoking the (possibly async) text
                        // selector for every item on each keystroke.
                        int index = _items.FindIndex(start + 1, x => (x.Text ?? GetItemText(x.Value)).StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        if (index < 0 && start >= 0)
                        {
                            index = _items.FindIndex(0, x => (x.Text ?? GetItemText(x.Value)).StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        }
                        if (index >= 0)
                        {
                            _localpaginator.EnsureVisibleIndex(index);
                            _indexTooptip = 0;
                            break;
                        }
                    }
                }
                _lastinput = _filterBuffer.ToString();
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        private void SetSelectionDisabledErrorIfNeeded(bool ignoreViewOnly = false)
        {
            if ((!_viewOnly || ignoreViewOnly) && _localpaginator?.SelectedItem?.Disabled == true)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
        }

        private void UpdateFilterFromBuffer()
        {
            string filter = _filterBuffer.ToString();
            if (!filter.Equals(_lastinput, StringComparison.OrdinalIgnoreCase))
            {
                _localpaginator!.UpdateFilter(filter);
            }

            _lastinput = filter;
            if (string.IsNullOrEmpty(filter))
            {
                _modeView = ModeView.Select;
            }
        }

        private bool TryAutoSelectSingleItem()
        {
            if (_localpaginator!.Count == 1
                && _autoSelect
                && _localpaginator.SelectedIndex >= 0
                && !_localpaginator.SelectedItem!.Disabled)
            {
                _modeView = ModeView.Select;
                ResultCtrl = new ResultPrompt<T>(_localpaginator.SelectedItem.Value, false);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _modeView = ModeView.Select;
            _updatePosAnswerBuffer = false;
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = _localpaginator.SelectedItem.Text!;
            }
            else if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                answer = PromptPlusResources.CanceledKey;
            }
            WritePrompt(screenBuffer, _optStyles[SelectStyles.Prompt]);
            if (!_viewOnly)
            {
                screenBuffer.WriteLine(answer, _optStyles[SelectStyles.Answer]);
            }
            else
            {
                if (_defaultValue.HasValue)
                {
                    var found = _items.FirstOrDefault(x => !x.Disabled && !x.CharSeparation.HasValue && _DefaultMatchBy.Invoke(x.Value!, _defaultValue.Value));
                    if (found is not null)
                    {
                        screenBuffer.WriteLine(found.Text!, _optStyles[SelectStyles.Answer]);
                    }
                }
                else
                {
                    screenBuffer.WriteLine("", _optStyles[SelectStyles.Answer]);
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public override void FinalizeControl()
        {
            //none
        }

        private void LoadEnum()
        {
            List<(int Order, ItemSelect<T> Item)> result = [];
            foreach (T enumValue in Enum.GetValues(typeof(T)).Cast<T>())
            {
                string? name = enumValue!.ToString();
                DisplayAttribute? displayAttribute = typeof(T).GetField(name!)?.GetCustomAttribute<DisplayAttribute>();
                int order = displayAttribute?.GetOrder() ?? int.MaxValue;
                _sequence++;
                result.Add((order, new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), enumValue, false)
                {
                    Text = GetItemText(enumValue)
                }));
            }
            foreach ((_, ItemSelect<T> item) in result.OrderBy(x => x.Order))
            {
                _items.Add(item);
            }
        }

        private void SaveHistory()
        {
            if (_historyOptions == null)
            {
                return;
            }
            T selectedValue = _localpaginator!.SelectedItem.Value;
            string serializedValue = JsonSerializer.Serialize(selectedValue);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;

        }

        private static string EnumDisplay(T value)
        {
            string name = value!.ToString()!;
            DisplayAttribute? displayAttribute = value.GetType().GetField(name)?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.GetName() ?? name;
        }

        private string GetItemText(T value)
        {
            if (_textSelectorAsync is not null)
            {
                return _textSelectorAsync.Invoke(value)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            return _textSelector?.Invoke(value) ?? string.Empty;
        }

        /// <summary>
        /// Evaluates the optional selection predicate for <paramref name="value"/>, returning
        /// <c>true</c> when no predicate is configured or when it accepts the value. Used to decide
        /// whether a default/history value may position the cursor (rejected values are not honored).
        /// </summary>
        private bool TrySelectionPredicate(T value)
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

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    GetTooltipSelect()                
                ];
                lsttooltips.Add(PromptPlusResources.TooltipPages);
                if (mode == ModeView.Select)
                {
                    if (!_viewOnly && _filterType != FilterMode.Disabled)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipFilter);
                    }
                    if (!_viewOnly)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipNavegateTextPrompt);
                    }
                    // Jump-by-first-char is only reachable when filter is disabled (otherwise any
                    // printable key transitions the control into filter mode instead of jumping).
                    if (!_viewOnly && _filterType == FilterMode.Disabled)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipJump);
                    }
                }
                if (OptionsControl.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                }
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
                lsttooltips.AddRange(GetEmacsTooltips(_viewOnly));
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipSelect()
        {
            StringBuilder tooltip = new();
            if (!_viewOnly)
            {
                tooltip.Append(PromptPlusResources.TooltipEnterFinish);
                tooltip.Append('.');
            }
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            return tooltip.ToString();
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
            screenBuffer.WriteLine(tooltip, _optStyles[SelectStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            switch (_modeView)
            {
                case ModeView.Select:
                    { 
                        if (_indexTooptip >= _toggerTooptips[ModeView.Select].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.Select][_indexTooptip];
                    }
                case ModeView.Filter:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.Filter].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.Filter][_indexTooptip];
                    }
                default:
                    throw new NotImplementedException($"ModeView {_modeView} not implemented.");    
            };
        }

        private void WriteListSelect(BufferScreen screenBuffer)
        {
            ArraySegment<ItemSelect<T>> subset = _localpaginator!.GetPageData();
            foreach (ItemSelect<T> item in subset)
            {
                string value = item.Text!;
                string? group = item.IsFirstItemGroup ? item.Group : string.Empty;
                string indentgroup = string.Empty;
                if (item.CharSeparation.HasValue)
                {
                    value = new string(item.CharSeparation.Value, _lengthSeparationline);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Group) && _modeView != ModeView.Filter)
                    {
                        indentgroup = item.IsLastItemGroup
                            ? $" {GetSymbol(SymbolType.IndentEndGroup)}"
                            : $" {GetSymbol(SymbolType.IndentGroup)}";
                    }
                }
                if (item.IsFirstItemGroup)
                {
                    screenBuffer.WriteLine($" {group}", _optStyles[SelectStyles.UnSelected]);
                }
                else if (_localpaginator.SelectedIndex >=0 && item.UniqueId == _localpaginator.SelectedItem.UniqueId) 
                {
                    screenBuffer.Write(GetSymbol(SymbolType.Selector), _optStyles[SelectStyles.Selected]);
                    screenBuffer.Write(indentgroup, _optStyles[SelectStyles.Lines]);
                    if (!item.CharSeparation.HasValue && item.Disabled)
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.Selected]);
                    }
                    if (HasExtraInfo(item, out string extraInfo))
                    {
                        screenBuffer.Write(extraInfo, item.Disabled ? _optStyles[SelectStyles.Disabled] : _optStyles[SelectStyles.Selected]);
                    }
                    screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
                }
                else
                {
                    screenBuffer.Write($" {indentgroup}", _optStyles[SelectStyles.Lines]);
                    if (!item.CharSeparation.HasValue && item.Disabled)
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.UnSelected]);
                    }
                    if (HasExtraInfo(item, out string extraInfo))
                    {
                        screenBuffer.Write(extraInfo, item.Disabled ? _optStyles[SelectStyles.Disabled] : _optStyles[SelectStyles.TaggedInfo]);
                    }
                    screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
                }
            }
            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[SelectStyles.Pagination]);
            }
        }

        private bool HasExtraInfo(ItemSelect<T> item, out string extraInfo)
        {
            if (_extraInfoAsync is not null && item.ExtraText == null)
            {
                item.ExtraText = _extraInfoAsync.Invoke(item.Value)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult() ?? string.Empty;
            }
            else if (_extraInfo != null && item.ExtraText == null)
            {
                item.ExtraText = _extraInfo.Invoke(item.Value) ?? string.Empty;
            }
            if (string.IsNullOrWhiteSpace(item.ExtraText))
            {
                extraInfo = string.Empty;
                return false;
            }
            extraInfo = $"{OptionsControl.PrefixExtraInfoValue}{item.ExtraText}{OptionsControl.SuffixExtraInfoValue}";
            return true;
        }


        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Select)
            {
                string text = string.Empty;
                if (_localpaginator!.SelectedIndex >= 0)
                {
                    text = _localpaginator!.SelectedItem.Text!;
                }
                if (_updatePosAnswerBuffer)
                {
                    _answerBuffer!.LoadPrintable(text);
                    _answerBuffer.ToHome();
                }
                int promptWidth = GetPromptDisplayWidth();
                (string visibleLeft, string visibleRight) = ViewportSlice(_answerBuffer!, promptWidth);
                screenBuffer.Write(visibleLeft, _optStyles[SelectStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine(visibleRight, _optStyles[SelectStyles.Answer]);
            }
            else if (_modeView == ModeView.Filter)
            {
                WriteAnswerFilter(screenBuffer);
            }
            else
            {
                throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
        }


        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _optStyles[SelectStyles.TaggedInfo];
            if (_localpaginator!.TotalCount == 0)
            {
               found = _optStyles[SelectStyles.Error];
            }
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[SelectStyles.TaggedInfo]);
        }

        private void WriteGroupDescription(BufferScreen screenBuffer)
        {
            if (!_hideTipGroup && _localpaginator!.SelectedItem is not null)
            {
                if (!string.IsNullOrEmpty(_localpaginator!.SelectedItem.Group))
                {
                    screenBuffer.WriteLine(_localpaginator!.SelectedItem.Group, _optStyles[SelectStyles.GroupTip]);
                }
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (_localpaginator!.SelectedItem is not null)
            {
                if (_changeDescriptionAsync is not null)
                {
                    desc = _changeDescriptionAsync.Invoke(_localpaginator.SelectedItem.Value)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }
                else
                {
                    desc = _changeDescription?.Invoke(_localpaginator.SelectedItem.Value) ?? OptionsControl.DescriptionValue;
                }
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[SelectStyles.Description]);
            }
        }

    }
}
