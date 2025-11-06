// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.RemoteSelect
{
    internal sealed class RemoteSelectControl<T1, T2> : BaseControlPrompt<T1>, IRemoteSelectControl<T1, T2> where T1 : class where T2 : class
    {
        private readonly Dictionary<SelectStyles, Style> _optStyles = BaseControlOptions.LoadStyle<SelectStyles>();
        private readonly EmacsBuffer _filterBuffer;
        private readonly List<ItemSelect<T1>> _items = [];
        private Func<T1, (bool, string?)>? _predicatevalidselect;
        private Func<T1, string>? _changeDescription;
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _filterCaseinsensitive;
        private byte _pageSize = 10;
        private Func<T1, string>? _textSelector;
        private Paginator<ItemSelect<T1>>? _localpaginator;
        private Func<T1, string>? _uniqueexpression;
        private Func<T2, (bool, T2, IEnumerable<T1>)>? _predicateSearchItems;
        private Func<T1, bool>? _predicateDisabled;
        private T2 _searchItemsControl;
        private bool _searchItemsFinished;
        private Func<Exception, string>? _searchItemsErrorMessage;
        private Task? _loadingItemTask;
        private (Exception? error, bool IsFinished, T2 newsearchItemsControl, IEnumerable<T1> newitems)? _loadingResult;
        private readonly string _loadMoreId = Guid.NewGuid().ToString();
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
        private string _lastinput;


        public RemoteSelectControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _searchItemsControl = default!;
            _filterBuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _lastinput = string.Empty;
            _pageSize = ConfigPlus.PageSize;
        }


        #region IRemoteSelectControl


        public IRemoteSelectControl<T1, T2> PredicateDisabled(Func<T1, bool> validdisabled)
        {
            ArgumentNullException.ThrowIfNull(validdisabled);
            _predicateDisabled = validdisabled;
            return this;
        }

        public IRemoteSelectControl<T1, T2> PredicateSelected(Func<T1, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IRemoteSelectControl<T1, T2> PredicateSelected(Func<T1, bool> validselect)
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

        public IRemoteSelectControl<T1, T2> ChangeDescription(Func<T1, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public IRemoteSelectControl<T1, T2> Filter(FilterMode value, bool caseinsensitive = true)
        {
            _filterType = value;
            _filterCaseinsensitive = caseinsensitive;
            return this;
        }


        public IRemoteSelectControl<T1, T2> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IRemoteSelectControl<T1, T2> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public IRemoteSelectControl<T1, T2> Styles(SelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IRemoteSelectControl<T1, T2> TextSelector(Func<T1, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        public IRemoteSelectControl<T1, T2> UniqueId(Func<T1, string> uniquevalue)
        {
            ArgumentNullException.ThrowIfNull(uniquevalue);
            _uniqueexpression = uniquevalue;
            return this;
        }

        public IRemoteSelectControl<T1, T2> SearchMoreItems(T2 initialvalue, Func<T2, (bool, T2, IEnumerable<T1>)> values, Func<Exception, string>? erroMessage = null)
        {
            ArgumentNullException.ThrowIfNull(initialvalue);
            ArgumentNullException.ThrowIfNull(values);
            _predicateSearchItems = values;
            _searchItemsErrorMessage = erroMessage;
            _searchItemsControl = initialvalue;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            ValidateConstraints();

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
            _tooltipModeSelect = GetTooltipModeSelect();
            LoadTooltipToggle();
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

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
                    ConsoleKeyInfo keyinfo = WaitKeypresLoading(cancellationToken);


                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T1>(default!, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = _localpaginator!.SelectedItem != null
                            ? new ResultPrompt<T1>(_localpaginator!.SelectedItem!.Value!, true)
                            : new ResultPrompt<T1>(default!, true);
                        break;
                    }
                    else if (_loadingItemTask == null && keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                        ResultCtrl = new ResultPrompt<T1>(_localpaginator!.SelectedItem.Value, false);
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
                            _items.Add(new ItemSelect<T1>(_uniqueexpression!(item), item, disabled)
                            {
                                Text = _textSelector!.Invoke(item)
                            });
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
                            _modeView = ModeView.Select;
                        }
                        _loadingItemTask = Task.Run(() => LoadMoreItem(), cancellationToken);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_loadingItemTask == null && _filterType != FilterMode.Disabled && ConfigPlus.HotKeyFilterMode.Equals(keyinfo))
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
                        if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                        if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                            if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                            if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
                        if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
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
                        if (_localpaginator.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_loadingItemTask == null && _filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
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
                        if (_localpaginator!.SelectedItem != null && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
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
            if (_loadingItemTask != null)
            {
                _loadingItemTask.Wait();
                _loadingItemTask.Dispose();
            }
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
            ArraySegment<ItemSelect<T1>> subset = _localpaginator!.GetPageData();
            foreach (ItemSelect<T1> item in subset)
            {
                string value = item.Text!;
                if (_localpaginator.TryGetSelected(out ItemSelect<T1>? selectedItem) && EqualityComparer<ItemSelect<T1>>.Default.Equals(item, selectedItem))
                {
                    if (item.UniqueId == _loadMoreId)
                    {
                        screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[SelectStyles.Answer]);
                        screenBuffer.WriteLine($" {value}", _optStyles[SelectStyles.Answer]);
                        continue;
                    }
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[SelectStyles.Selected]);
                    if (item.Disabled)
                    {
                        screenBuffer.WriteLine($" {value}", _optStyles[SelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.WriteLine($" {value}", _optStyles[SelectStyles.Selected]);
                    }
                }
                else
                {
                    if (item.UniqueId == _loadMoreId)
                    {
                        screenBuffer.WriteLine($"  {value}", _optStyles[SelectStyles.Disabled]);
                        continue;
                    }
                    if (item.Disabled)
                    {
                        screenBuffer.WriteLine($"  {value}", _optStyles[SelectStyles.Disabled]);
                    }
                    else
                    {
                        screenBuffer.WriteLine($"  {value}", _optStyles[SelectStyles.UnSelected]);
                    }
                }
            }
            string template = ConfigPlus.PaginationTemplate.Invoke(
                _localpaginator.TotalCountValid,
                _localpaginator.SelectedPage + 1,
                _localpaginator.PageCount
            )!;
            screenBuffer.WriteLine(template, _optStyles[SelectStyles.Pagination]);
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Select)
            {
                string text = string.Empty;
                if (_loadingItemTask != null)
                {
                    text = Messages.Loading;
                }
                else
                {
                    if (_localpaginator!.SelectedIndex >= 0 && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
                    {
                        text = _localpaginator!.SelectedItem.Text!;
                    }
                }
                if (_localpaginator!.SelectedIndex >= 0 && _localpaginator!.SelectedItem.Disabled && _localpaginator.SelectedItem.UniqueId != _loadMoreId)
                {
                    screenBuffer.WriteLine(text, _optStyles[SelectStyles.Disabled]);
                }
                else
                {
                    screenBuffer.WriteLine(text, _optStyles[SelectStyles.Answer]);
                }
                screenBuffer.SavePromptCursor();
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

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[SelectStyles.Error]);
                ClearError();
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
