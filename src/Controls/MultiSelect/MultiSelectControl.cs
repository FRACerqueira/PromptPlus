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

namespace PromptPlusLibrary.Controls.MultiSelect
{
    /// <inheritdoc/>
    internal sealed class MultiSelectControl<T> : BaseControlPrompt<T[]>, IMultiSelectControl<T>
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer line, optional error/group line, optional description line,
        /// tooltip line and an extra row for the pagination footer when active.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 7;

        // Cached composite format strings for improved performance
        private static readonly CompositeFormat s_multiSelectMinSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMinSelection);
        private static readonly CompositeFormat s_multiSelectMaxSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMaxSelection);
        private static readonly CompositeFormat s_tooltipCountCheckFormat = CompositeFormat.Parse(PromptPlusResources.TooltipCountCheck);

        private readonly Dictionary<MultiSelectStyles, Style> _optStyles;
        private readonly EmacsConsoleBuffer _filterBuffer;
        private readonly List<ItemSelect<T>> _items = [];
        private Func<T, (bool, string?)>? _predicatevalidcheck;
        private Func<T, Task<(bool, string?)>>? _predicatevalidcheckAsync;
        private Func<T, string?>? _extraInfo;
        private Func<T, Task<string?>>? _extraInfoAsync;
        private int _sequence;
        private Func<T, string>? _changeDescription;
        private Func<T, Task<string>>? _changeDescriptionAsync;
        private Func<T, T, bool> _DefaultMatchBy = EqualityComparer<T>.Default.Equals;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private IEnumerable<T> _defaultValues = [];
        private bool _useDefaultHistory;
        private bool _forceDefaultFromHistory;
        private HistoryOptions? _historyOptions;
        private FilterMode _filterType = FilterMode.Disabled;
        private byte _pageSize;
        private int _effectivePageSize;
        private bool _hideTipGroup;
        private Func<T, string>? _textSelector;
        private Func<T, Task<string>>? _textSelectorAsync;
        private IList<ItemHistory>? _itemHistories;
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
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private bool _onfilterOnlySelected;
        private int _countChecked;
        // Snapshot of _countChecked used the last time tooltip strings were built. When it
        // transitions across the 0 boundary the "filter all selected" hint must be added/removed,
        // so we rebuild the tooltip cache lazily on the next frame.
        private int _lastCountCheckedTooltip = -1;
        private bool _lastFilterOnlySelectedTooltip;

        public MultiSelectControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<MultiSelectStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
            _filterBuffer = new(false, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
        }


        #region IMultIMultiSelectControl

        /// <inheritdoc/>
        public IMultiSelectControl<T> ViewOnly(bool value = true)
        {
            _viewOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfo = extraInfoNode;
            _extraInfoAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoAsync = extraInfoNode;
            _extraInfo = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = validselect;
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = (input) => (validselect(input), (string?)null);
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = validselect;
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
            _DefaultMatchBy = comparer;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> AddItem(T value, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, disable, ischecked));
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            foreach (T? value in values)
            {
                AddItem(value, ischecked, disable);
            }
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiSelectControl<T>, Task> interactionAction)
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
        public IMultiSelectControl<T> AddGroupedItem(string group, T value, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            int lastindex = _items.FindLastIndex((x) => x.Group == group);
            if (lastindex < 0)
            {
                _sequence++;
                _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, false)
                {
                    Group = group,
                    IsFirstItemGroup = true,
                    IsLastItemGroup = true
                });
            }
            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(CultureInfo.CurrentCulture), value, disable, ischecked)
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
        public IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            foreach (T? value in values)
            {
                AddGroupedItem(group, value, ischecked, disable);
            }
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            if (values.Any())
            {
                _defaultValue = Optional<T>.Set(values.First());
            }
            _defaultValues = values;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> UseDefaultHistory()
        {
            _defaultValue = Optional<T>.Empty();
            _useDefaultHistory = true;
            _forceDefaultFromHistory = true;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
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
        public IMultiSelectControl<T> Filter(FilterMode value)
        {
            _filterType = value;
            return this;
        }


        /// <inheritdoc/>
        public IMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiSelectControl<T>> interactionAction)
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
        public IMultiSelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue > (maxvalue ?? int.MaxValue))
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minSelect = minvalue;
            _maxSelect = maxvalue ?? int.MaxValue;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> PageSize(byte value)
        {
            // value == 0 means "auto-fit to console height" (see ComputeEffectivePageSize).
            // Any positive value is the user's preferred maximum and is later clamped to the
            // height available on screen.
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
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
        public IMultiSelectControl<T> HideTipGroup(bool value = true)
        {
            _hideTipGroup = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            _textSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiSelectControl<T> TextSelectorAsync(Func<T, Task<string>> value)
        {
            _textSelectorAsync = value ?? throw new ArgumentNullException(nameof(value), "TextSelectorAsync is null");
            _textSelector = null;
            return this;
        }

        #endregion

        /// <inheritdoc/>
        public override void InitControl(CancellationToken cancellationToken)
        {
            bool loadedDefaultsFromHistory = false;
            _answerBuffer = new(true, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
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
                    // Display width (terminal columns), not .Length — a CJK item/group name is fewer
                    // characters but more columns, so the AddSeparator() divider line used to end up
                    // shorter than the widest CJK item/group it was meant to span.
                    int textWidth = item.Text.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
                    if (textWidth > _lengthSeparationline)
                    {
                        _lengthSeparationline = textWidth;
                    }
                    int groupWidth = (item.Group ?? string.Empty).GetDisplayLength() is { Length: > 0 } gd ? gd[0] : 0;
                    if (groupWidth > _lengthSeparationline)
                    {
                        _lengthSeparationline = groupWidth;
                    }
                }
            }
            if (_viewOnly)
            {
                _historyOptions = null;
            }
            // UseDefaultHistory() overrides any values supplied by Default(), but only when history
            // is actually enabled — otherwise it has no effect and Default()'s values stand.
            bool overrideDefaultsFromHistory = _forceDefaultFromHistory && _historyOptions != null;
            if (_defaultValues.Any() && !overrideDefaultsFromHistory)
            {
                _defaultValue = Optional<T>.Set(_defaultValues.First());
            }
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out T[] histvalues))
                    {
                        if (histvalues.Length > 0)
                        {
                            _defaultValue = Optional<T>.Set(histvalues.First());
                            // set checked items with history (honoring the selection predicate:
                            // rejected values are silently skipped so the initial checked set never
                            // contains items the predicate would forbid)
                            foreach (var item in histvalues)
                            {
                                int index = _items.FindIndex(x => _DefaultMatchBy.Invoke(x.Value!, item));
                                if (index >= 0 && TryValidateCheckPredicate(_items[index].Value, out _))
                                {
                                    _items[index].ValueChecked = true;
                                }
                            }

                            loadedDefaultsFromHistory = true;
                        }
                    }
                }
            }
            if (_defaultValues.Any() && !loadedDefaultsFromHistory && !overrideDefaultsFromHistory)
            {
                foreach (var item in _defaultValues)
                {
                    int index = _items.FindIndex(x => _DefaultMatchBy.Invoke(x.Value!, item));
                    // Honor the selection predicate: rejected defaults are silently skipped.
                    if (index >= 0 && TryValidateCheckPredicate(_items[index].Value, out _))
                    {
                        _items[index].ValueChecked = true;
                    }
                }
            }
            _defaultValues = [];

            _countChecked = _items.Count(x => x.ValueChecked && !x.IsFirstItemGroup);

            Optional<ItemSelect<T>> defvaluepage = Optional<ItemSelect<T>>.Empty();

            if (_defaultValue.HasValue)
            {
                ItemSelect<T>? found = _items.FirstOrDefault(x => !x.Disabled && !x.CharSeparation.HasValue && _DefaultMatchBy.Invoke(x.Value!, _defaultValue.Value));
                if (found != null)
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
                (item) => (item.IsFirstItemGroup ? item.Group! : item.Text!),
                (item) => !item.CharSeparation.HasValue,
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

            WritePrompt(screenBuffer, _optStyles[MultiSelectStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteGroupDescription(screenBuffer);
            WriteDescription(screenBuffer);
            WriteListSelect(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[MultiSelectStyles.Error]);
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
                    bool updatePosAnswerBufferBeforeThisKey = _updatePosAnswerBuffer;
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        // A resize/cancel breaks out below without ever reaching the specific
                        // resets a real navigation/scroll key would apply. Restore whatever this
                        // flag was BEFORE this iteration force-set it to `true` above, so a resize
                        // never changes whether the next render reloads the answer buffer from the
                        // current checked-items summary vs. preserves a scroll the user had just
                        // navigated to on a long answer preview.
                        _updatePosAnswerBuffer = updatePosAnswerBufferBeforeThisKey;
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            _modeView = ModeView.Select;
                            ResultCtrl = new ResultPrompt<T[]>([], true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip
                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
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
                            ResultCtrl = new ResultPrompt<T[]>([.. _items.Where(x => x.ValueChecked).Select(x => x.Value)], false);
                            break;
                        }
                        if (_countChecked < _minSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_multiSelectMinSelectionFormat, _minSelect));
                            break;
                        }
                        if (_countChecked > _maxSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_multiSelectMaxSelectionFormat, _maxSelect));
                            break;
                        }
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T[]>([.. _items.Where(x => x.ValueChecked).Select(x => x.Value)], false);
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

                    else if (_onfilterOnlySelected && _modeView == ModeView.Select && ConfigPrompt.HotKeyFilterAllSelected.Equals(keyinfo))
                    {
                        int index = -1;
                        if (_localpaginator!.SelectedItem != null)
                        {
                            index = _items.FindIndex(x => x.UniqueId == _localpaginator!.SelectedItem!.UniqueId);
                        }
                        _onfilterOnlySelected = false;
                        _localpaginator!.UpdateCollection(_items);
                        _localpaginator!.UpdateFilter(string.Empty);
                        _filterBuffer!.Clear();
                        if (index >= 0)
                        {
                            _localpaginator!.EnsureVisibleIndex(index);
                        }
                        _filterBuffer!.Clear();

                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_onfilterOnlySelected && _countChecked > 0 && _modeView == ModeView.Select && ConfigPrompt.HotKeyFilterAllSelected.Equals(keyinfo))
                    {
                        _onfilterOnlySelected = true;
                        _localpaginator!.UpdateCollection(_items.Where(x => x.ValueChecked));
                        _localpaginator!.UpdateFilter(string.Empty);
                        _filterBuffer!.Clear();
                        _indexTooptip = 0;
                        break;
                    }
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
                    else if (!_viewOnly && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && _onfilterOnlySelected)
                    {
                        foreach (var item in _localpaginator!.AllItems().Where(x => !x.Disabled && !x.IsFirstItemGroup && x.CharSeparation == null))
                        {
                            item.ValueChecked = false;
                            _countChecked--;
                        }
                        _onfilterOnlySelected = false;
                        _localpaginator!.UpdateCollection(_items);
                        _localpaginator!.UpdateFilter(string.Empty);
                        _filterBuffer!.Clear();
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && _modeView == ModeView.Filter && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && !_onfilterOnlySelected)
                    {
                        int grpcount = _localpaginator!.AllItems().Count(x => !x.Disabled && !x.IsFirstItemGroup && x.CharSeparation == null);
                        bool grpavaluecheck = _localpaginator.AllItems().Count(x => x.ValueChecked) != grpcount;
                        foreach (var item in _localpaginator.AllItems().Where(x => !x.Disabled && !x.IsFirstItemGroup && x.CharSeparation == null))
                        {
                            // The predicate only gates checking an item; unchecking never needs it
                            // (mass operation: items rejected by the predicate are silently skipped
                            // while checking, no error shown).
                            if (grpavaluecheck && !TryValidateCheckPredicate(item.Value, out _))
                            {
                                continue;
                            }
                            if (item.ValueChecked != grpavaluecheck)
                            {
                                item.ValueChecked = grpavaluecheck;
                                _countChecked += grpavaluecheck ? 1 : -1;
                            }
                        }
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && _modeView == ModeView.Select && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && !_onfilterOnlySelected)
                    {
                        int grpcount = _items.Count(x => !x.IsFirstItemGroup && x.CharSeparation == null && !x.Disabled);
                        int grpcheck = _items.Count(x => x.ValueChecked && !x.Disabled);
                        bool grpavaluecheck = grpcheck != grpcount;
                        foreach (var item in _items.Where(x => !x.IsFirstItemGroup && x.CharSeparation == null && !x.Disabled))
                        {
                            // The predicate only gates checking an item; unchecking never needs it
                            // (mass operation: items rejected by the predicate are silently skipped
                            // while checking, no error shown).
                            if (grpavaluecheck && !TryValidateCheckPredicate(item.Value, out _))
                            {
                                continue;
                            }
                            if (item.ValueChecked != grpavaluecheck)
                            {
                                item.ValueChecked = grpavaluecheck;
                                _countChecked += grpavaluecheck ? 1 : -1;
                            }
                        }
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.Disabled && _localpaginator.SelectedItem.IsFirstItemGroup && _localpaginator.SelectedItem.CharSeparation == null)
                    {
                        _indexTooptip = 0;
                        int index = _items.FindIndex(x => x.UniqueId == _localpaginator.SelectedItem.UniqueId);
                        int grpcheck = _items.Count(x => x.Group == _items[index].Group && !x.IsFirstItemGroup && x.CharSeparation == null && !x.Disabled && x.ValueChecked);
                        int grpcount = _items.Count(x => x.Group == _items[index].Group && !x.IsFirstItemGroup && x.CharSeparation == null && !x.Disabled);
                        bool grpavaluecheck = grpcheck != grpcount;
                        foreach (var item in _items.Where(x => x.Group == _items[index].Group && !x.IsFirstItemGroup && x.CharSeparation == null && !x.Disabled))
                        {
                            // The predicate only gates checking an item; unchecking never needs it
                            // (mass operation: items rejected by the predicate are silently skipped
                            // while checking, no error shown).
                            if (grpavaluecheck && !TryValidateCheckPredicate(item.Value, out _))
                            {
                                continue;
                            }
                            if (item.ValueChecked != grpavaluecheck)
                            {
                                item.ValueChecked = grpavaluecheck;
                                _countChecked += grpavaluecheck ? 1 : -1;
                            }
                        }
                        if (_countChecked == 0 && _onfilterOnlySelected)
                        {
                            _onfilterOnlySelected = false;
                            _localpaginator!.UpdateCollection(_items);
                            _localpaginator!.UpdateFilter(string.Empty);
                            _filterBuffer!.Clear();
                        }
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.Disabled && !_localpaginator.SelectedItem.IsFirstItemGroup && _localpaginator.SelectedItem.CharSeparation == null)
                    {
                        _indexTooptip = 0;
                        if (_localpaginator!.SelectedItem.ValueChecked)
                        {
                            // Unchecking never needs the predicate — it only gates checking an item.
                            _localpaginator.SelectedItem.ValueChecked = false;
                            _countChecked--;
                        }
                        else
                        {
                            if (!TryValidateCheckPredicate(_localpaginator!.SelectedItem.Value, out string? message))
                            {
                                SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.PredicateSelectInvalid : message);
                                break;
                            }
                            _localpaginator.SelectedItem.ValueChecked = true;
                            _countChecked++;
                        }
                        if (_countChecked == 0 && _onfilterOnlySelected)
                        {
                            _onfilterOnlySelected = false;
                            _localpaginator!.UpdateCollection(_items);
                            _localpaginator!.UpdateFilter(string.Empty);
                            _filterBuffer!.Clear();
                        }
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        UpdateFilterFromBuffer();
                        if (string.IsNullOrEmpty(_filterBuffer.ToString()))
                        {
                            _modeView = ModeView.Select;
                            _localpaginator!.UpdateCollection(_items);
                            _localpaginator!.UpdateFilter(string.Empty);
                            _filterBuffer!.Clear();
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
                            if (!_onfilterOnlySelected)
                            {
                                _localpaginator!.UpdateCollection(_items.Where(x => !x.CharSeparation.HasValue && !x.IsFirstItemGroup));
                            }
                            else
                            {
                                _localpaginator!.UpdateCollection(_items.Where(x => !x.CharSeparation.HasValue && !x.IsFirstItemGroup && x.ValueChecked));
                            }
                            UpdateFilterFromBuffer();
                            if (string.IsNullOrEmpty(_filterBuffer.ToString()))
                            {
                                _modeView = ModeView.Select;
                            }
                            SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_modeView == ModeView.Select && !_onfilterOnlySelected && _localpaginator!.SelectedItem != null && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
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

        /// <inheritdoc/>
        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _modeView = ModeView.Select;
            _updatePosAnswerBuffer = false;
            WritePrompt(screenBuffer, _optStyles[MultiSelectStyles.Prompt]);
            if (!ResultCtrl!.Value.IsAborted)
            {
                screenBuffer.WriteLine(BuildCheckedItemsText(), _optStyles[MultiSelectStyles.Answer]);
            }
            else if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                screenBuffer.WriteLine(PromptPlusResources.CanceledKey, _optStyles[MultiSelectStyles.Answer]);
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
            string serializedCheckedItems = JsonSerializer.Serialize(_items.Where(x => x.ValueChecked).Select(x => x.Value).ToArray());
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedCheckedItems, _historyOptions.ExpirationTimeValue, hist);
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

        private void SetSelectionDisabledErrorIfNeeded(bool ignoreViewOnly = false)
        {
            if ((!_viewOnly || ignoreViewOnly) && _localpaginator?.SelectedItem?.Disabled == true)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
        }

        private bool TryValidateCheckPredicate(T value, out string? message)
        {
            (bool ok, string? validationMessage) = _predicatevalidcheckAsync != null
                ? _predicatevalidcheckAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidcheck?.Invoke(value) ?? (true, null));
            message = validationMessage;
            return ok;
        }

        private void SetRangeValidationErrorIfNeeded()
        {
            if (_countChecked < _minSelect)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_multiSelectMinSelectionFormat, _minSelect));
                return;
            }

            if (_countChecked > _maxSelect)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_multiSelectMaxSelectionFormat, _maxSelect));
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
        }

        /// <summary>
        /// Builds the comma-separated text of all checked items.
        /// Uses string.Join (single-pass, buffered)
        /// instead of LINQ Aggregate to avoid O(n^2) intermediate string allocations and to
        /// safely return an empty string when no item is checked.
        /// </summary>
        private string BuildCheckedItemsText()
            => string.Join(',', _items.Where(x => x.ValueChecked).Select(x => x.Text!));

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    GetTooltipSelect()
                ];
                lsttooltips.Add(PromptPlusResources.TooltipPages);
                if (!_viewOnly)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyToggleAll}:{PromptPlusResources.TooltipCheckAll}");
                }
                if (mode == ModeView.Select)
                {
                    // Only advertise the "filter all selected" hotkey when it actually does
                    // something: either there is at least one checked item (to enter the view) or
                    // we are already inside the "only selected" view (to leave it).
                    if (_countChecked > 0 || _onfilterOnlySelected)
                    {
                        lsttooltips.Add($"{ConfigPrompt.HotKeyFilterAllSelected}:{PromptPlusResources.TooltipFilterAllSelected}");
                    }
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
                tooltip.Append(PromptPlusResources.TooltipCheckItem);
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
            // Reload tooltip cache when the "checked" state crosses the 0 boundary or when the
            // "only selected" view toggles: both change whether the FilterAllSelected hint applies.
            bool hasChecked = _countChecked > 0;
            bool hadChecked = _lastCountCheckedTooltip > 0;
            if (hasChecked != hadChecked || _lastFilterOnlySelectedTooltip != _onfilterOnlySelected)
            {
                LoadTooltipToggle();
                _lastCountCheckedTooltip = _countChecked;
                _lastFilterOnlySelectedTooltip = _onfilterOnlySelected;
            }
            string? tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[MultiSelectStyles.Tooltips]);
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
            }
            ;
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
                    value = new string(item.CharSeparation.Value, _lengthSeparationline + GetSymbol(SymbolType.Selected).Length + 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Group) && _modeView != ModeView.Filter && !_onfilterOnlySelected)
                    {
                        indentgroup = item.IsLastItemGroup
                            ? $" {GetSymbol(SymbolType.IndentEndGroup)}"
                            : $" {GetSymbol(SymbolType.IndentGroup)}";
                    }
                }
                if (_localpaginator.SelectedIndex >= 0 && item.UniqueId == _localpaginator.SelectedItem.UniqueId)
                {
                    screenBuffer.Write($"{GetSymbol(SymbolType.Selector)}", _optStyles[MultiSelectStyles.Selected]);
                    if (item.IsFirstItemGroup)
                    {
                        screenBuffer.Write($"{group}", _optStyles[MultiSelectStyles.Selected]);
                    }
                    else
                    {
                        screenBuffer.Write($"{indentgroup}", _optStyles[MultiSelectStyles.Lines]);
                        if (!item.CharSeparation.HasValue)
                        {
                            if (item.ValueChecked)
                            {
                                screenBuffer.Write(GetSymbol(SymbolType.Selected), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                            }
                            else
                            {
                                screenBuffer.Write(GetSymbol(SymbolType.NotSelect), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                            }
                        }
                        if (!item.CharSeparation.HasValue)
                        {
                            if (item.Disabled)
                            {
                                screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.Disabled]);
                            }
                            else
                            {
                                screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.Selected]);
                            }
                        }
                        else
                        {
                            screenBuffer.Write($"{value}", _optStyles[MultiSelectStyles.Selected]);
                        }
                        if (HasExtraInfo(item, out string extraInfo))
                        {
                            screenBuffer.Write(extraInfo, item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                        }
                    }
                    screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
                }
                else
                {
                    screenBuffer.Write(' ', ConsoleHandler.CurrentStyle);
                    if (item.IsFirstItemGroup)
                    {
                        screenBuffer.Write($"{group}", _optStyles[MultiSelectStyles.UnSelected]);
                    }
                    else
                    {
                        screenBuffer.Write($"{indentgroup}", _optStyles[MultiSelectStyles.Lines]);
                        if (!item.CharSeparation.HasValue)
                        {
                            if (item.ValueChecked)
                            {
                                screenBuffer.Write(GetSymbol(SymbolType.Selected), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                            }
                            else
                            {
                                screenBuffer.Write(GetSymbol(SymbolType.NotSelect), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                            }
                        }
                        if (!item.CharSeparation.HasValue)
                        {
                            if (item.Disabled)
                            {
                                screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.Disabled]);
                            }
                            else
                            {
                                screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.UnSelected]);
                            }
                        }
                        else
                        {
                            screenBuffer.Write($"{value}", _optStyles[MultiSelectStyles.UnSelected]);
                        }
                        if (HasExtraInfo(item, out string extraInfo))
                        {
                            screenBuffer.Write(extraInfo, item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.TaggedInfo]);
                        }
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
                template = $"{template} {string.Format(CultureInfo.CurrentCulture, s_tooltipCountCheckFormat, _countChecked)}";
                screenBuffer.WriteLine(template, _optStyles[MultiSelectStyles.Pagination]);
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
                    ItemSelect<T> selected = _localpaginator.SelectedItem;
                    // A group header's own Text mirrors its first child's value (an AddGroupedItem
                    // artifact); show the group name instead, matching what WriteListSelect renders
                    // for that row. Headers never carry ExtraInfo (WriteListSelect never resolves
                    // it for them either), so the check below is skipped for them naturally.
                    if (selected.IsFirstItemGroup)
                    {
                        text = selected.Group!;
                    }
                    else
                    {
                        text = selected.Text!;
                        // Shown live only: the list row can overflow the console width with no way
                        // to scroll it back into view, while the answer line already supports
                        // horizontal scrolling (ViewportSlice) — a second, reliable place to read
                        // it. The final answer (BuildCheckedItemsText) intentionally stays plain.
                        if (HasExtraInfo(selected, out string extraInfo))
                        {
                            text += extraInfo;
                        }
                    }
                }
                if (_updatePosAnswerBuffer)
                {
                    _answerBuffer!.LoadPrintable(text);
                    _answerBuffer.ToHome();
                }
                int promptWidth = GetPromptDisplayWidth();
                (string visibleLeft, string visibleRight) = ViewportSlice(_answerBuffer!, promptWidth);
                screenBuffer.Write(visibleLeft, _optStyles[MultiSelectStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine(visibleRight, _optStyles[MultiSelectStyles.Answer]);
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
            Style found = _optStyles[MultiSelectStyles.TaggedInfo];
            if (_localpaginator!.TotalCount == 0)
            {
                found = _optStyles[MultiSelectStyles.Error];
            }
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[MultiSelectStyles.TaggedInfo]);
        }

        private void WriteGroupDescription(BufferScreen screenBuffer)
        {
            if (!_hideTipGroup && _localpaginator!.SelectedItem is not null)
            {
                if (!string.IsNullOrEmpty(_localpaginator!.SelectedItem.Group))
                {
                    screenBuffer.WriteLine(_localpaginator!.SelectedItem.Group, _optStyles[MultiSelectStyles.GroupTip]);
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
                screenBuffer.WriteLine(desc, _optStyles[MultiSelectStyles.Description]);
            }
        }

    }
}
