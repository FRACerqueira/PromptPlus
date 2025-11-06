// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.RemoteMultiSelect
{
    internal sealed class RemoteMultiSelectControl<T1, T2> : BaseControlPrompt<T1[]>, IRemoteMultiSelectControl<T1, T2> where T1 : class where T2 : class
    {
        private readonly Dictionary<MultiSelectStyles, Style> _optStyles = BaseControlOptions.LoadStyle<MultiSelectStyles>();
        private readonly List<ItemSelect<T1>> _items = [];
        private Func<T1, string>? _uniqueexpression;
        private Func<T1, (bool, string?)>? _predicatevalidselect;
        private Func<T2, (bool, T2, IEnumerable<T1>)>? _predicateSearchItems;
        private Func<Exception, string>? _searchItemsErrorMessage;
        private Func<T1, bool>? _predicateDisabled;
        private T2 _searchItemsControl;
        private bool _searchItemsFinished;
        private Func<T1, string>? _changeDescription;
        private List<T1> _defaultValues = [];
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _filterCaseinsensitive;
        private byte _pageSize;
        private byte _maxWidth;
        private Func<T1, string>? _textSelector;
        private IList<ItemHistory>? _itemHistories;
        private Paginator<ItemSelect<T1>>? _localpaginator;
        private readonly EmacsBuffer _filterBuffer;
        private EmacsBuffer? _resultbuffer;
        private readonly List<ItemSelect<T1>> _checkeditems = [];
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.MultiSelect,[] },
            { ModeView.Filter,[] }
        };
        private ModeView _modeView = ModeView.MultiSelect;
        private int _indexTooptip;
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private bool _isShowAllSeleceted;
        private bool _hideCountSelected;
        private string _lastinput;
        private Task? _loadingItemTask;
        private (Exception? error, bool IsFinished, T2 newsearchItemsControl, IEnumerable<T1> newitems)? _loadingResult;
        private readonly string _loadMoreId = Guid.NewGuid().ToString();


#pragma warning disable IDE0079
#pragma warning disable IDE0290 // Use primary constructor
        public RemoteMultiSelectControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _searchItemsControl = default!;
            _filterBuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _lastinput = string.Empty;
            _pageSize = ConfigPlus.PageSize;
            _maxWidth = ConfigPlus.MaxWidth;
        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079

        #region IRemoteMultiSelectControl

        public IRemoteMultiSelectControl<T1, T2> PredicateDisabled(Func<T1, bool> validdisabled)
        {
            ArgumentNullException.ThrowIfNull(validdisabled);
            _predicateDisabled = validdisabled;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> SearchMoreItems(T2 initialvalue, Func<T2, (bool, T2, IEnumerable<T1>)> values, Func<Exception, string>? erroMessage = null)
        {
            ArgumentNullException.ThrowIfNull(initialvalue);
            ArgumentNullException.ThrowIfNull(values);
            _predicateSearchItems = values;
            _searchItemsErrorMessage = erroMessage;
            _searchItemsControl = initialvalue;
            return this;
        }


        public IRemoteMultiSelectControl<T1, T2> UniqueId(Func<T1, string> uniquevalue)
        {
            ArgumentNullException.ThrowIfNull(uniquevalue);
            _uniqueexpression = uniquevalue;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> DefaultWhenLoad(IEnumerable<T1> values, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            _defaultValues = values.ToList();
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, bool> validselect)
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

        public IRemoteMultiSelectControl<T1, T2> ShowAllSelected(bool value)
        {
            _isShowAllSeleceted = value;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> HideCountSelected(bool value = true)
        {
            _hideCountSelected = value;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> MaxWidth(byte maxWidth)
        {
            if (maxWidth < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 10.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue > (maxvalue ?? int.MaxValue))
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minSelect = minvalue;
            _maxSelect = maxvalue ?? int.MaxValue;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> ChangeDescription(Func<T1, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public IRemoteMultiSelectControl<T1, T2> Filter(FilterMode value, bool caseinsensitive = true)
        {
            _filterType = value;
            _filterCaseinsensitive = caseinsensitive;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> Styles(MultiSelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IRemoteMultiSelectControl<T1, T2> TextSelector(Func<T1, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            ValidateConstraints();

            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    try
                    {
                        _defaultValues = JsonSerializer.Deserialize<T1[]>(_itemHistories[0].History!)!.ToList();
                    }
                    catch (Exception)
                    {
                        //invalid Deserialize history 
                    }
                }
            }

            _resultbuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);

            _localpaginator = new Paginator<ItemSelect<T1>>(
                _filterType,
                _items,
                _pageSize,
                Optional<ItemSelect<T1>>.Empty(),
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => _textSelector!(item.Value),
                null,
                (item) => item.UniqueId != _loadMoreId);


            _loadingItemTask = Task.Run(() => LoadMoreItem(), cancellationToken);

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
                    ConsoleKeyInfo keyinfo = WaitKeypresLoading(cancellationToken);

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.MultiSelect;
                        ResultCtrl = new ResultPrompt<T1[]>([], true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.MultiSelect;
                        ResultCtrl = new ResultPrompt<T1[]>([], true);
                        break;
                    }
                    else if (_loadingItemTask == null && keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                        T1[] result = [.. _checkeditems.Select(x => x.Value)];
                        ResultCtrl = new ResultPrompt<T1[]>(result, false);
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
                    else if (_loadingItemTask != null && keyinfo.Key == ConsoleKey.None && keyinfo.Modifiers == ConsoleModifiers.None)
                    {
                        _searchItemsFinished = _loadingResult!.Value.IsFinished;
                        _searchItemsControl = _loadingResult!.Value.newsearchItemsControl;
                        int index = _items.FindIndex(x => x.UniqueId == _loadMoreId);
                        if (index >= 0)
                        {
                            _items.RemoveAt(index);
                        }
                        foreach (T1 item in _loadingResult!.Value.newitems)
                        {
                            bool disabled = _predicateDisabled?.Invoke(item) ?? false;
                            bool ischeck = _defaultValues.FindIndex(x => _uniqueexpression!(item) == _uniqueexpression!(x)) >= 0;
                            _items.Add(new ItemSelect<T1>(_uniqueexpression!(item), item, disabled, ischeck)
                            {
                                Text = _textSelector!.Invoke(item)
                            });
                            if (ischeck)
                            {
                                _checkeditems.Add(_items[^1]);
                            }
                        }
                        if (!_searchItemsFinished)
                        {
                            _items.Add(new ItemSelect<T1>(_loadMoreId, default!, true)
                            {
                                Text = Messages.LoadMore
                            });
                        }
                        if (_loadingResult!.Value.error != null)
                        {
                            if (_searchItemsErrorMessage != null)
                            {
                                SetError(_searchItemsErrorMessage.Invoke(_loadingResult!.Value.error!));
                            }
                            else
                            {
                                SetError(_loadingResult!.Value.error.Message);
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
                        Optional<ItemSelect<T1>> defaultvalue = Optional<ItemSelect<T1>>.Empty();
                        if (_localpaginator!.SelectedIndex >= 0)
                        {
                            if (!_searchItemsFinished)
                            {
                                defaultvalue = Optional<ItemSelect<T1>>.Set(_localpaginator.SelectedItem!);
                            }
                        }
                        _localpaginator!.UpdatColletion(_items, defaultvalue);
                        if (_searchItemsFinished)
                        {
                            _localpaginator!.End();
                        }
                        _indexTooptip = 0;
                        _loadingResult = null;
                        _loadingItemTask?.Dispose();
                        _loadingItemTask = null;
                        break;
                    }
                    else if (_loadingItemTask == null && !_searchItemsFinished && keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null && _localpaginator.SelectedItem.UniqueId == _loadMoreId)
                    {
                        if (_modeView == ModeView.Filter)
                        {
                            _localpaginator!.UpdateFilter(string.Empty);
                            _filterBuffer.Clear();
                            _modeView = ModeView.MultiSelect;
                        }
                        _loadingItemTask = Task.Run(() => LoadMoreItem(), cancellationToken);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_loadingItemTask == null && _filterType != FilterMode.Disabled && ConfigPlus.HotKeyFilterMode.Equals(keyinfo))
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
                    else if (keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.Disabled && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
                    {
                        int index = _items.FindIndex(x => _uniqueexpression!(x.Value!) == _uniqueexpression(_localpaginator.SelectedItem.Value));
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
                    else if (_localpaginator!.SelectedItem != null && ConfigPlus.HotKeyTooltipToggleAll.Equals(keyinfo))
                    {
                        IEnumerable<ItemSelect<T1>> toselect = _items.Where(x => !x.Disabled && x.UniqueId != _loadMoreId);
                        int qtdcheck = toselect.Count(x => x.ValueChecked);
                        if (qtdcheck == toselect.Count())
                        {
                            foreach (ItemSelect<T1>? item in toselect)
                            {
                                item.ValueChecked = false;
                                _checkeditems.Remove(item);
                            }
                        }
                        else
                        {
                            bool hasinvalidselect = false;
                            string? customerr = null;
                            foreach (ItemSelect<T1>? item in toselect)
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
                    else if (_loadingItemTask == null && _filterType != FilterMode.Disabled && _modeView == ModeView.Filter)
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
            if (_loadingItemTask != null)
            {
                _loadingItemTask.Wait();
                _loadingItemTask.Dispose();
            }
        }

        private enum ModeView
        {
            MultiSelect,
            Filter
        }

        private void SaveHistory(T1[] value)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string aux = JsonSerializer.Serialize<T1[]>(value);
            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
            IList<ItemHistory> hist = FileHistory.AddHistory(aux, _historyOptions!.ExpirationTimeValue, null);
            FileHistory.SaveHistory(_historyOptions!.FileNameValue, hist);

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

        private string GetTooltipModeMultiSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.MultiSelectCheck);
            tooltip.Append(", ");
            tooltip.Append(Messages.SpaceMultSelect);
            tooltip.Append(", ");
            tooltip.Append(string.Format(Messages.TooltipSelectAll, ConfigPlus.HotKeyTooltipToggleAll));
            return tooltip.ToString();
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = _indexTooptip > 0 ? GetTooltipToggle() : GetTooltipModeMultiSelect();
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
            ArraySegment<ItemSelect<T1>> subset = _localpaginator!.GetPageData();
            foreach (ItemSelect<T1> item in subset)
            {
                string value = item.Text!;
                if (_localpaginator.TryGetSelected(out ItemSelect<T1>? selectedItem) && EqualityComparer<ItemSelect<T1>>.Default.Equals(item, selectedItem))
                {
                    if (item.UniqueId == _loadMoreId)
                    {
                        screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[MultiSelectStyles.Answer]);
                        screenBuffer.WriteLine($"{value}", _optStyles[MultiSelectStyles.Answer]);
                        continue;
                    }
                    screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selector), item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.Selected]);
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
                        screenBuffer.WriteLine($" {value}", _optStyles[MultiSelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.WriteLine($" {value}", _optStyles[MultiSelectStyles.Selected]);
                    }
                }
                else
                {
                    if (item.UniqueId == _loadMoreId)
                    {
                        screenBuffer.WriteLine($" {value}", _optStyles[MultiSelectStyles.Disabled]);
                        continue;
                    }
                    screenBuffer.Write(" ", _optStyles[MultiSelectStyles.UnSelected]);
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
                        screenBuffer.WriteLine($"{value}", _optStyles[MultiSelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.WriteLine($" {value}", item.Disabled ? _optStyles[MultiSelectStyles.Disabled] : _optStyles[MultiSelectStyles.UnSelected]);
                    }
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
                screenBuffer.Write(str, styleAnswer);
                if (_loadingItemTask != null)
                {
                    screenBuffer.Write($" ({Messages.Loading})", styleAnswer);
                }
                screenBuffer.WriteLine("", styleAnswer);

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
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[MultiSelectStyles.Error]);
                ClearError();
                return;
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

        private void ValidateConstraints()
        {
            if (_uniqueexpression == null)
            {
                throw new InvalidOperationException("UniqueId cannot be Null.");
            }
            if (_textSelector == null)
            {
                throw new InvalidOperationException("TextSelector cannot be Null.");
            }
            if (_predicateSearchItems == null)
            {
                throw new InvalidOperationException("PredicateSearchItems cannot be Null.");
            }
        }

        private ConsoleKeyInfo WaitKeypresLoading(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_loadingItemTask != null && _loadingItemTask.IsCompleted)
                {
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(true) : new ConsoleKeyInfo();
        }

        private void LoadMoreItem()
        {
            (bool Finished, T2 SearchItemsControl, IEnumerable<T1> Newitems) result = (false, _searchItemsControl, []);
            Exception? err = null;
            try
            {
                result = _predicateSearchItems!.Invoke(_searchItemsControl);
            }
            catch (Exception ex)
            {
                err = ex;
            }
            _loadingResult = (err, result.Finished, result.SearchItemsControl, result.Newitems);
        }

    }
}
