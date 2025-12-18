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

namespace PromptPlusLibrary.Controls.Select
{
    internal sealed class SelectControl<T> : BaseControlPrompt<T>, ISelectControl<T>
    {
        private readonly Dictionary<SelectStyles, Style> _optStyles = BaseControlOptions.LoadStyle<SelectStyles>();
        private readonly EmacsBuffer _filterBuffer;
        private readonly List<ItemSelect<T>> _items = [];
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, string?>? _extraInfo;
        private int _sequence;
        private bool _autoSelect;
        private Func<T, string>? _changeDescription;
        private Func<T, T, bool> _equalItems = (x, y) => x?.Equals(y) ?? false;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _filterCaseinsensitive;
        private byte _pageSize;
        private bool _hideTipGroup;
        private Func<T, string>? _textSelector;
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
        private string _tooltipModeSelect = string.Empty;
        private int _lengthSeparationline;
        private string _lastinput;
        private byte _maxWidth;
        private EmacsBuffer? _answerBuffer;
        private bool _updatePosAnswerBuffer;

        public SelectControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _pageSize = ConfigPlus.PageSize;
            _filterBuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _lastinput = string.Empty;
            _maxWidth = ConfigPlus.MaxWidth;
        }


        #region ISelectControl

        public ISelectControl<T> MaxWidth(byte maxWidth)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 1.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public ISelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfo = extraInfoNode;
            return this;
        }

        public ISelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public ISelectControl<T> PredicateSelected(Func<T, bool> validselect)
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

        public ISelectControl<T> EqualItems(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
            _equalItems = comparer;
            return this;
        }

        public ISelectControl<T> AddItem(T value, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _sequence++;
            _items.Add(new ItemSelect<T>(_sequence.ToString(), value, disable));
            return this;
        }

        public ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            foreach (T? value in values)
            {
                AddItem(value, disable);
            }
            return this;
        }

        public ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(group, nameof(group));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            int lastindex = _items.FindLastIndex((x) => x.Group == group);
            if (lastindex < 0)
            {
                _sequence++;
                _items.Add(new ItemSelect<T>(_sequence.ToString(), value, disable)
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
            _items.Add(new ItemSelect<T>(_sequence.ToString(), value, disable)
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

        public ISelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            foreach (T? value in values)
            {
                AddGroupedItem(group, value, disable);
            }
            return this;
        }

        public ISelectControl<T> AutoSelect(bool value = true)
        {
            _autoSelect = value;
            return this;
        }

        public ISelectControl<T> ChangeDescription(Func<T, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public ISelectControl<T> Default(T value, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _defaultValue = Optional<T>.Set(value);
            _useDefaultHistory = useDefaultHistory;
            return this;
        }
        
        public ISelectControl<T> DefaultHistory(bool useDefaultHistory = true)
        {
            _defaultValue = Optional<T>.Empty();
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

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

        public ISelectControl<T> Filter(FilterMode value, bool caseinsensitive = true)
        {
            _filterType = value;
            _filterCaseinsensitive = caseinsensitive;
            return this;
        }


        public ISelectControl<T> Interaction(IEnumerable<T> items, Action<T, ISelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T? item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        public ISelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public ISelectControl<T> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public ISelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
        {
            char separator = separatorLine switch
            {
                SeparatorLine.SingleLine => ConfigPlus.GetSymbol(SymbolType.SingleBorder)[0],
                SeparatorLine.DoubleLine => ConfigPlus.GetSymbol(SymbolType.DoubleBorder)[0],
                SeparatorLine.UserChar => value ?? throw new ArgumentNullException(nameof(value), "Char separator is null"),
                _ => throw new ArgumentOutOfRangeException(nameof(separatorLine), "SeparatorLine not supported")
            };
            _sequence++;
#pragma warning disable CS8604 // Possible null reference argument.
            _items.Add(new ItemSelect<T>(_sequence.ToString(), default, true)
            {
                CharSeparation = separator,
                Text = ""
            });
#pragma warning restore CS8604 // Possible null reference argument.
            return this;
        }

        public ISelectControl<T> HideTipGroup(bool value = true)
        {
            _hideTipGroup = value;
            return this;
        }

        public ISelectControl<T> Styles(SelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public ISelectControl<T> TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            _answerBuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);
            _updatePosAnswerBuffer = true;
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
                }
            }

            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    try
                    {
                        _defaultValue = Optional<T>.Set(JsonSerializer.Deserialize<T>(_itemHistories[0].History!)!);
                    }
                    catch (Exception)
                    {
                        //invalid Deserialize history 
                    }
                }
            }

            Optional<ItemSelect<T>> defvaluepage = Optional<ItemSelect<T>>.Empty();

            if (_defaultValue.HasValue)
            {
                ItemSelect<T>? found = _items.FirstOrDefault(x => !x.Disabled && !x.CharSeparation.HasValue && _equalItems.Invoke(x.Value!, _defaultValue.Value));
                if (found != null)
                {
                    defvaluepage = Optional<ItemSelect<T>>.Set(found);
                }
            }

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
            if (_localpaginator!.SelectedIndex >= 0 && _localpaginator.SelectedItem!.Disabled)
            {
                SetError(Messages.SelectionDisabled);
            }
            _tooltipModeSelect = GetTooltipModeSelect();
            LoadTooltipToggle();
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteErroAndGroupDescription(screenBuffer);

            WriteDescription(screenBuffer);

            WriteListSelect(screenBuffer);

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
                    _updatePosAnswerBuffer = true;

                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
#pragma warning disable CS8604 // Possible null reference argument.
                        ResultCtrl = new ResultPrompt<T>(default, true);
#pragma warning restore CS8604 // Possible null reference argument.
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        if (_localpaginator!.SelectedItem != null)
                        {
                            ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem!.Value!, true);
                        }
                        else
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            ResultCtrl = new ResultPrompt<T>(default, true);
#pragma warning restore CS8604 // Possible null reference argument.
                        }
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        if (_localpaginator.SelectedItem.Disabled)
                        {
                            SetError(Messages.SelectionDisabled);
                            break;
                        }
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_localpaginator!.SelectedItem.Value) ?? (true, null);
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
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem.Value, false);
                        SaveHistory(_localpaginator!.SelectedItem.Value);
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
                        _filterBuffer.Clear();
                        _modeView = _modeView != ModeView.Filter ? ModeView.Filter : ModeView.Select;
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
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
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
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
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
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        string filter = _filterBuffer.ToString();
                        if (_filterCaseinsensitive)
                        {
                            filter = filter.ToUpperInvariant();
                            _lastinput = _lastinput.ToUpperInvariant();
                        }
                        if (filter! != _lastinput)
                        {
                            _localpaginator!.UpdateFilter(filter);
                        }
                        if (_localpaginator!.Count == 1 && _autoSelect && _localpaginator!.SelectedIndex >= 0 && !_localpaginator!.SelectedItem!.Disabled)
                        {
                            _modeView = ModeView.Select;
                            ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem!.Value, false);
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
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
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
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = _localpaginator!.SelectedItem.Text!;
            }
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SelectStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[SelectStyles.Answer]);
            return true;
        }
        public override void FinalizeControl()
        {
            //none
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
                result.Add(new Tuple<int, ItemSelect<T>>(order, new ItemSelect<T>(_sequence.ToString(), item, false)
                {
                    Text = _textSelector?.Invoke(item)
                }));
            }
            foreach (Tuple<int, ItemSelect<T>>? item in result.OrderBy(x => x.Item1))
            {
                _items.Add(item.Item2);
            }
        }

        private void SaveHistory(T value)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string aux = JsonSerializer.Serialize<T>(value);
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

        private string GetTooltipModeSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipPages);
            return tooltip.ToString();
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = _indexTooptip > 0 ? GetTooltipToggle() : _tooltipModeSelect;
            screenBuffer.Write(tooltip, _optStyles[SelectStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Select => _toggerTooptips[ModeView.Select][_indexTooptip - 1],
                ModeView.Filter => _toggerTooptips[ModeView.Filter][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
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
                            ? $" {ConfigPlus.GetSymbol(SymbolType.IndentEndGroup)}"
                            : $" {ConfigPlus.GetSymbol(SymbolType.IndentGroup)}";
                    }
                }
                if (item.IsFirstItemGroup)
                {
                    screenBuffer.WriteLine($" {group}", _optStyles[SelectStyles.UnSelected]);

                }
                if (_localpaginator.TryGetSelected(out ItemSelect<T>? selectedItem) && EqualityComparer<ItemSelect<T>>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[SelectStyles.Selected]);
                    screenBuffer.Write($"{indentgroup}", _optStyles[SelectStyles.Lines]);
                    if (item.Disabled)
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.Write($" {value}", _optStyles[SelectStyles.Selected]);
                    }
                    if (!string.IsNullOrEmpty(item.ExtraText))
                    {
                        screenBuffer.Write($"({item.ExtraText})", item.Disabled ? _optStyles[SelectStyles.Disabled] : _optStyles[SelectStyles.Selected]);

                    }
                    screenBuffer.WriteLine("", Style.Default());

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
                    if (!string.IsNullOrEmpty(item.ExtraText))
                    {
                        screenBuffer.Write($"({item.ExtraText})", item.Disabled ? _optStyles[SelectStyles.Disabled] : _optStyles[SelectStyles.TaggedInfo]);
                    }
                    screenBuffer.WriteLine("", Style.Default());
                }
            }
            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[SelectStyles.Pagination]);
            }
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
                string str = _answerBuffer!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, _optStyles[SelectStyles.Answer]);
                screenBuffer.Write(_answerBuffer!.ToBackward(), _optStyles[SelectStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.Write(_answerBuffer!.ToForward(), _optStyles[SelectStyles.Answer]);
                str = _answerBuffer.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.WriteLine(str, _optStyles[SelectStyles.Answer]);
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
            screenBuffer.Write(_filterBuffer.ToBackward(), found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(_filterBuffer.ToForward(), found);
            screenBuffer.WriteLine($" ({Messages.Filter})", _optStyles[SelectStyles.TaggedInfo]);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SelectStyles.Prompt]);
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
                    screenBuffer.Write($"{Messages.Group}: {_localpaginator!.SelectedItem.Group}", _optStyles[SelectStyles.GroupTip]);
                }
            }
            if (!string.IsNullOrEmpty(ValidateError))
            {
                if (hastip)
                {
                    screenBuffer.Write(", ", _optStyles[SelectStyles.GroupTip]);
                }
                screenBuffer.WriteLine(ValidateError, _optStyles[SelectStyles.Error]);
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
                screenBuffer.WriteLine(desc, _optStyles[SelectStyles.Description]);
            }
        }

    }
}
