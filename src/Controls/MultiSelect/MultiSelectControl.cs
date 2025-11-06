// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PromptPlusLibrary.Controls.MultiSelect
{
    internal sealed class MultiSelectControl<T> : BaseControlPrompt<T[]>, IMultiSelectControl<T>
    {
        private readonly Dictionary<MultiSelectStyles, Style> _optStyles = BaseControlOptions.LoadStyle<MultiSelectStyles>();
        private readonly List<ItemSelect<T>> _items = [];
        private int _sequence;
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, string>? _changeDescription;
        private Func<T, T, bool> _equalItems = (x, y) => x?.Equals(y) ?? false;
        private Func<T, string?>? _extraInfo;
        private IEnumerable<T>? _defaultValues;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _filterCaseinsensitive;
        private byte _pageSize;
        private bool _hideTipGroup;
        private Func<T, string>? _textSelector;
        private IList<ItemHistory>? _itemHistories;
        private Paginator<ItemSelect<T>>? _localpaginator;
        private readonly EmacsBuffer _filterBuffer;
        private EmacsBuffer? _resultbuffer;
        private readonly List<ItemSelect<T>> _checkeditems = [];
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.MultiSelect,[] },
            { ModeView.Filter,[] }
        };
        private ModeView _modeView = ModeView.MultiSelect;
        private int _indexTooptip;
        private int _lengthSeparationline;
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private byte _maxWidth;
        private bool _isShowAllSeleceted;
        private bool _hideCountSelected;
        private bool _hasGroup;
        private string _lastinput;


#pragma warning disable IDE0079 
#pragma warning disable IDE0290 // Use primary constructor
        public MultiSelectControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _filterBuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _lastinput = string.Empty;
            _maxWidth = ConfigPlus.MaxWidth;
            _pageSize = ConfigPlus.PageSize;
        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079

        #region IMultiSelect

        public IMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfo = extraInfoNode;
            return this;
        }

        public IMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect)
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

        public IMultiSelectControl<T> ShowAllSelected(bool value)
        {
            _isShowAllSeleceted = value;
            return this;
        }

        public IMultiSelectControl<T> HideCountSelected(bool value = true)
        {
            _hideCountSelected = value;
            return this;
        }

        public IMultiSelectControl<T> MaxWidth(byte maxWidth)
        {
            if (maxWidth < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 10.");
            }
            _maxWidth = maxWidth;
            return this;
        }

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

        public IMultiSelectControl<T> EqualItems(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
            _equalItems = comparer;
            return this;
        }

        public IMultiSelectControl<T> AddItem(T value, bool valuechecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(), value, valuechecked, disable));
            return this;
        }

        public IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool valuechecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            foreach (T? value in values)
            {
                AddItem(value, disable);
            }
            return this;
        }

        public IMultiSelectControl<T> AddGroupedItem(string group, T value, bool valuechecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            int lastindex = _items.FindLastIndex((x) => x.Group == group);
            if (lastindex < 0)
            {
                _sequence++;
                _items.Add(new ItemSelect<T>(_sequence.ToString(), value, valuechecked, disable)
                {
                    Group = group,
                    IsFirstItemGroup = true,
                    IsLastItemGroup = true
                });
                return this;
            }
            if (lastindex != _items.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(group), "Group already exists");
            }
            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(), value, valuechecked, disable)
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

        public IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool valuechecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            foreach (T? value in values)
            {
                AddGroupedItem(group, value, disable);
            }
            return this;
        }

        public IMultiSelectControl<T> ChangeDescription(Func<T, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            _defaultValues = values;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        public IMultiSelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public IMultiSelectControl<T> Filter(FilterMode value, bool caseinsensitive = true)
        {
            _filterType = value;
            _filterCaseinsensitive = caseinsensitive;
            return this;
        }



        public IMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, IMultiSelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T? item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        public IMultiSelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IMultiSelectControl<T> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
        {
            char separator = separatorLine switch
            {
                SeparatorLine.SingleLine => ConsolePlus.IsUnicodeSupported ? '─' : '-',
                SeparatorLine.DoubleLine => ConsolePlus.IsUnicodeSupported ? '═' : '=',
                SeparatorLine.UserChar => value ?? throw new ArgumentNullException(nameof(value), "Char separator is null"),
                _ => throw new ArgumentOutOfRangeException(nameof(separatorLine), "SeparatorLine not supported")
            };
            _sequence++;
#pragma warning disable CS8604 // Possible null reference argument.
            _items.Add(new ItemSelect<T>(_sequence.ToString(), default, false, true)
            {
                CharSeparation = separator,
                Text = ""
            });
#pragma warning restore CS8604 // Possible null reference argument.
            return this;
        }

        public IMultiSelectControl<T> HideTipGroup(bool value = true)
        {
            _hideTipGroup = value;
            return this;
        }

        public IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IMultiSelectControl<T> TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (typeof(T).IsEnum)
            {
                _textSelector ??= EnumDisplay;
                if (_items.Count == 0)
                {
                    LoadEnum();
                }
            }
            else
            {
                _textSelector ??= (x) => x?.ToString() ?? string.Empty;
                foreach (ItemSelect<T>? item in _items.Where(x => !x.CharSeparation.HasValue))
                {
                    item.Text = _textSelector.Invoke(item.Value);
                    if (item.Text.Length > _lengthSeparationline)
                    {
                        _lengthSeparationline = item.Text.Length;
                    }
                    if ((item.Group ?? string.Empty).Length > _lengthSeparationline)
                    {
                        _lengthSeparationline = item.Text.Length;
                    }
                    if (!string.IsNullOrEmpty(item.Group))
                    {
                        _hasGroup = true;
                    }
                }
            }

            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    try
                    {
                        _defaultValues = JsonSerializer.Deserialize<T[]>(_itemHistories[0].History!)!;
                    }
                    catch (Exception)
                    {
                        //invalid Deserialize history 
                    }
                }
            }

            Optional<ItemSelect<T>> defvaluepage = Optional<ItemSelect<T>>.Empty();

            if (_defaultValues != null && _defaultValues.Any())
            {
                bool hasdefvaluepage = false;
                foreach (T? item in _defaultValues)
                {
                    int index = _items.FindIndex(x => _equalItems.Invoke(x.Value!, item));
                    if (index >= 0)
                    {
                        _items[index].ValueChecked = true;
                        _checkeditems.Add(_items[index]);
                        if (!hasdefvaluepage)
                        {
                            hasdefvaluepage = true;
                            defvaluepage = Optional<ItemSelect<T>>.Set(_items[index]);
                        }
                    }
                }
            }

            _resultbuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);


            LoadExtraInfo();

            _localpaginator = new Paginator<ItemSelect<T>>(
                _filterType,
                _items,
                _pageSize,
                defvaluepage,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => (!item.IsFirstItemGroup && !item.CharSeparation.HasValue) ? item.Text! : string.Empty,
                (item) => !item.CharSeparation.HasValue || !string.IsNullOrEmpty(item.Text),
                (item) => !item.CharSeparation.HasValue);

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (_checkeditems.Count == 0)
            {
                _resultbuffer!.Clear();
            }
            else
            {
                _resultbuffer!.LoadPrintable(_checkeditems.Select(x => x.Text!).Aggregate((x, y) => $"{x},{y}"));
            }
            LoadTooltipToggle();

        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteErroAndGroupDescription(screenBuffer);

            WriteDescription(screenBuffer);

            WriteListMultiSelect(screenBuffer);

            WriteTooltip(screenBuffer);
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

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.MultiSelect;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.MultiSelect;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                            break;
                        }
                        if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                            break;
                        }
                        _indexTooptip = 0;
                        _modeView = ModeView.MultiSelect;
                        T[] result = [.. _checkeditems.Select(x => x.Value)];
                        ResultCtrl = new ResultPrompt<T[]>(result, false);
                        SaveHistory(result);
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

                    else if (_filterType != FilterMode.Disabled && ConfigPlus.HotKeyFilterMode.Equals(keyinfo))
                    {
                        _localpaginator!.UpdateFilter(string.Empty);
                        _filterBuffer!.Clear();
                        _modeView = _modeView != ModeView.Filter ? ModeView.Filter : ModeView.MultiSelect;
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
                        _indexTooptip = 0;
                        break;
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
                        if (!_localpaginator!.Home())
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        if (!_localpaginator!.End())
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.Disabled)
                    {
                        int index = _items.FindIndex(x => _equalItems(x.Value!, _localpaginator.SelectedItem.Value));
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_items[index].Value) ?? (true, null);
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
                        if (!_items[index].ValueChecked)
                        {
                            _checkeditems.Add(_items[index]);
                        }
                        else
                        {
                            _checkeditems.Remove(_items[index]);
                        }
                        if (_checkeditems.Count == 0)
                        {
                            _resultbuffer!.Clear();
                        }
                        else
                        {
                            _resultbuffer!.LoadPrintable(_checkeditems.Select(x => x.Text!).Aggregate((x, y) => $"{x},{y}"));
                        }
                        _items[index].ValueChecked = !_items[index].ValueChecked;
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                        }
                        else if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_localpaginator!.SelectedItem != null && (!string.IsNullOrEmpty(_localpaginator!.SelectedItem.Group) || !_hasGroup) && ConfigPlus.HotKeyTooltipToggleAll.Equals(keyinfo))
                    {
                        IEnumerable<ItemSelect<T>> toselect = _items.Where(x => x.Group == _localpaginator.SelectedItem.Group && !x.CharSeparation.HasValue && !x.Disabled);
                        int qtdcheck = toselect.Count(x => x.ValueChecked && !x.Disabled);
                        if (qtdcheck == toselect.Count())
                        {
                            foreach (ItemSelect<T>? item in toselect)
                            {
                                item.ValueChecked = false;
                                _checkeditems.Remove(item);
                            }
                        }
                        else
                        {
                            bool hasinvalidselect = false;
                            string? customerr = null;
                            foreach (ItemSelect<T>? item in toselect)
                            {
                                (bool ok, string? message) = _predicatevalidselect?.Invoke(item.Value) ?? (true, null);
                                if (!ok)
                                {
                                    hasinvalidselect = true;
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        customerr = message;
                                    }
                                }
                                else
                                {
                                    if (!item.ValueChecked)
                                    {
                                        item.ValueChecked = true;
                                        _checkeditems.Add(item);
                                    }
                                }
                            }
                            if (hasinvalidselect)
                            {
                                if (string.IsNullOrEmpty(customerr))
                                {
                                    SetError(Messages.PredicateSelectInvalid);
                                }
                                else
                                {
                                    SetError(customerr);
                                }
                            }
                        }
                        if (_checkeditems.Count == 0)
                        {
                            _resultbuffer!.Clear();
                        }
                        else
                        {
                            _resultbuffer!.LoadPrintable(_checkeditems.Select(x => x.Text!).Aggregate((x, y) => $"{x},{y}"));
                        }
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                        }
                        else if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter)
                    {
                        if (keyinfo.IsPressSpecialKey(ConsoleKey.Spacebar, ConsoleModifiers.Shift))
                        {
                            keyinfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        }
                        if (!_filterBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                        {
                            continue;
                        }
                        string filter = _filterBuffer.ToString();
                        if (_filterCaseinsensitive)
                        {
                            filter = filter.ToUpperInvariant();
                            _lastinput = _lastinput.ToUpperInvariant();
                        }
                        if (_lastinput != filter)
                        {
                            _localpaginator!.UpdateFilter(filter);
                        }
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_modeView == ModeView.MultiSelect && !_resultbuffer!.IsPrintable(keyinfo.KeyChar) && _resultbuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                }
                _lastinput = _filterBuffer.ToString();
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            else
            {
                answer = _resultbuffer!.ToString();
                if (!_isShowAllSeleceted)
                {
                    if (answer.Length > _maxWidth)
                    {
                        answer = answer[.._maxWidth] + "...";
                    }
                    else
                    {
                        answer += "...";
                    }
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[MultiSelectStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[MultiSelectStyles.Answer].Overflow(Overflow.Ellipsis));
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private enum ModeView
        {
            MultiSelect,
            Filter
        }

        private void LoadEnum()
        {

            IEnumerable<T> aux = Enum.GetValues(typeof(T)).Cast<T>();
            List<Tuple<int, ItemSelect<T>>> result = [];
            foreach (T item in aux)
            {
                string? name = item!.ToString();
                DisplayAttribute? displayAttribute = typeof(T).GetField(name!)?.GetCustomAttribute<DisplayAttribute>();
                int order = displayAttribute?.GetOrder() ?? int.MaxValue;
                _sequence++;
                result.Add(new Tuple<int, ItemSelect<T>>(order, new ItemSelect<T>(_sequence.ToString(), item, false, false)
                {
                    Text = _textSelector?.Invoke(item)
                }));
            }
            foreach (Tuple<int, ItemSelect<T>>? item in result.OrderBy(x => x.Item1))
            {
                _items.Add(item.Item2);
            }
        }

        private void SaveHistory(T[] value)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string aux = JsonSerializer.Serialize<T[]>(value);
            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
            IList<ItemHistory> hist = FileHistory.AddHistory(aux, _historyOptions!.ExpirationTimeValue, null);
            FileHistory.SaveHistory(_historyOptions!.FileNameValue, hist);

        }

        private static string EnumDisplay(T value)
        {
            string name = value!.ToString()!;
            DisplayAttribute? displayAttribute = value.GetType().GetField(name)?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.GetName() ?? name;
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {Messages.InputFinishEnter}"
                ];
                if (GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
                }
                lsttooltips.Add(Messages.TooltipPages);
                if (_filterType != FilterMode.Disabled)
                {
                    lsttooltips.Add(string.Format(Messages.TooltipFilterMode, ConfigPlus.HotKeyFilterMode));
                }
                if (mode == ModeView.Filter)
                {
                    lsttooltips.AddRange(EmacsBuffer.GetEmacsTooltips());
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipModeMultiSelect(bool withselectall)
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.MultiSelectCheck);
            tooltip.Append(", ");
            tooltip.Append(Messages.SpaceMultSelect);
            if (withselectall)
            {
                tooltip.Append(", ");
                tooltip.Append(string.Format(Messages.TooltipSelectAll, ConfigPlus.HotKeyTooltipToggleAll));
            }
            return tooltip.ToString();
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = _indexTooptip > 0
                ? GetTooltipToggle()
                : GetTooltipModeMultiSelect(!_hasGroup || (_localpaginator!.SelectedItem != null && !string.IsNullOrEmpty(_localpaginator!.SelectedItem.Group)));
            screenBuffer.Write(tooltip, _optStyles[MultiSelectStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.MultiSelect => _toggerTooptips[ModeView.MultiSelect][_indexTooptip - 1],
                ModeView.Filter => _toggerTooptips[ModeView.Filter][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };
        }

        private void WriteListMultiSelect(BufferScreen screenBuffer)
        {
            ArraySegment<ItemSelect<T>> subset = _localpaginator!.GetPageData();
            foreach (ItemSelect<T> item in subset)
            {
                string value = item.Text!;
                string? group = item.IsFirstItemGroup ? item.Group : string.Empty;
                string indentgroup = string.Empty;
                if (item.CharSeparation.HasValue)
                {
                    value = new string(item.CharSeparation.Value, _lengthSeparationline + ConfigPlus.GetSymbol(SymbolType.NotSelect).Length + 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Group) && _modeView != ModeView.Filter)
                    {
                        indentgroup = item.IsLastItemGroup
                            ? $" {ConfigPlus.GetSymbol(SymbolType.IndentEndGroup)}"
                            : $" {ConfigPlus.GetSymbol(SymbolType.IndentGroup)}";
                    }
                }
                if (item.IsFirstItemGroup)
                {
                    screenBuffer.WriteLine($" {group}", _optStyles[MultiSelectStyles.UnSelected]);

                }
                if (_localpaginator.TryGetSelected(out ItemSelect<T>? selectedItem) && EqualityComparer<ItemSelect<T>>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selector), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                    if (!string.IsNullOrEmpty(indentgroup))
                    {
                        screenBuffer.Write($"{indentgroup}", _optStyles[MultiSelectStyles.Lines]);
                    }
                    if (item.ValueChecked)
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                    }
                    else
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                    }
                    if (item.Disabled)
                    {
                        screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.Write($" {value}", _optStyles[MultiSelectStyles.Selected]);
                    }
                    if (!string.IsNullOrEmpty(item.ExtraText))
                    {
                        screenBuffer.Write($"({item.ExtraText})", item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
                    }
                    screenBuffer.WriteLine("", Style.Default());
                }
                else
                {
                    screenBuffer.Write($" {indentgroup}", _optStyles[MultiSelectStyles.Lines]);
                    if (!item.CharSeparation.HasValue)
                    {
                        if (item.ValueChecked)
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                        }
                        else
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                        }
                    }
                    if (item.CharSeparation.HasValue)
                    {
                        screenBuffer.Write($"{value}", _optStyles[MultiSelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.Write($" {value}", item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                    }
                    if (!string.IsNullOrEmpty(item.ExtraText))
                    {
                        screenBuffer.Write($"({item.ExtraText})", item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.TaggedInfo]);
                    }
                    screenBuffer.WriteLine("", Style.Default());
                }
            }
            string template = ConfigPlus.PaginationTemplate.Invoke(
                                _localpaginator.TotalCountValid,
                                _localpaginator.SelectedPage + 1,
                                _localpaginator.PageCount)!;
            screenBuffer.Write(template, _optStyles[MultiSelectStyles.Pagination]);
            if (!_hideCountSelected)
            {
                screenBuffer.Write(string.Format(Messages.TooltipCountCheck, _checkeditems.Count), _optStyles[MultiSelectStyles.TaggedInfo]);
            }
            screenBuffer.WriteLine("", Style.Default());
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.MultiSelect)
            {
                Style styleAnswer = _optStyles[MultiSelectStyles.Answer];
                string str = _resultbuffer!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, styleAnswer);

                screenBuffer.Write(_resultbuffer!.ToBackward(), styleAnswer);
                screenBuffer.SavePromptCursor();
                screenBuffer.Write(_resultbuffer!.ToForward(true), styleAnswer);
                str = _resultbuffer.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.WriteLine(str, styleAnswer);
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
            screenBuffer.Write(_filterBuffer.ToBackward(), found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(_filterBuffer.ToForward(), found);
            screenBuffer.WriteLine($" ({Messages.Filter})", _optStyles[MultiSelectStyles.TaggedInfo]);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[MultiSelectStyles.Prompt]);
            }
        }

        private void WriteErroAndGroupDescription(BufferScreen screenBuffer)
        {
            bool hastip = false;
            if (!_hideTipGroup && _localpaginator!.SelectedItem is not null)
            {
                if (!string.IsNullOrEmpty(_localpaginator!.SelectedItem.Group))
                {
                    hastip = true;
                    screenBuffer.Write($"{Messages.Group}: {_localpaginator!.SelectedItem.Group}", _optStyles[MultiSelectStyles.GroupTip]);
                }
            }
            if (!string.IsNullOrEmpty(ValidateError))
            {
                if (hastip)
                {
                    screenBuffer.Write(", ", _optStyles[MultiSelectStyles.GroupTip]);
                }
                screenBuffer.WriteLine(ValidateError, _optStyles[MultiSelectStyles.Error]);
                ClearError();
                return;
            }
            if (hastip)
            {
                screenBuffer.WriteLine("", Style.Default());
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_localpaginator!.SelectedItem.Value) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[MultiSelectStyles.Description]);
            }
        }


        private void LoadExtraInfo()
        {
            if (_extraInfo == null)
            {
                return;
            }
            foreach (ItemSelect<T> item in _items)
            {
                item.ExtraText = _extraInfo.Invoke(item.Value!);
            }
        }

    }
}
