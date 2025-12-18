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

namespace PromptPlusLibrary.Controls.AutoComplete
{
    internal sealed class AutoCompleteControl : BaseControlPrompt<string>, IAutoCompleteControl, IDisposable
    {
        private readonly Dictionary<AutoComleteStyles, Style> _optStyles = BaseControlOptions.LoadStyle<AutoComleteStyles>();
        private string[] _toggerTooptips = [];
        private Func<char, bool> _acceptInput;
        private Func<string, (bool, string?)>? _predicatevalidselect;
        private Func<string, string>? _changeDescription;
        private string _defaultValue;
        private bool _useDefaultHistory;
        private string _defaultIfEmpty;
        private HistoryOptions? _historyOptions;
        private CaseOptions _inputToCase;
        private int _maxLength;
        private int? _maxWidth;
        private Func<string, CancellationToken, Task<string[]>>? _autocompleteHandler;
        private Func<string, string>? _autocompleteTextSelector;
        private EmacsBuffer? _inputdata;
        private IList<ItemHistory>? _itemHistories;
        private Paginator<string>? _localpaginator;
        private int _indexTooptip;
        private string _tooltipModeAutoComlete;
        private byte _pageSize;
        private byte _minimumPrefixLength;
        private int _completionWaitToStart;
        private int _completionMaxCount;
        private Spinners _spinner;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _observerAutoComplete;
        private bool _autoCompleteRunning;
        private bool _finishautoCompleteRunning;
        private bool _disposed;
        private string? _currentspinnerFrame;
        private string _lastinput;

        public AutoCompleteControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _acceptInput = (_) => true;
            _defaultValue = string.Empty;
            _defaultIfEmpty = string.Empty;
            _inputToCase = CaseOptions.Any;
            _maxLength = int.MaxValue;
            _maxWidth = ConfigPlus.MaxWidth;
            _pageSize = ConfigPlus.PageSize;
            _minimumPrefixLength = ConfigPlus.MinimumPrefixLength;
            _completionWaitToStart = ConfigPlus.CompletionWaitToStart;
            _completionMaxCount = int.MaxValue;
            _spinner = new(SpinnersType.Ascii);
            _tooltipModeAutoComlete = string.Empty;
            _lastinput = string.Empty;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                FinalizeControl();
            }
        }

        #region IAutoCompleteControl

        public IAutoCompleteControl TextSelector(Func<string, string> value)
        {
            _autocompleteTextSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector cannot be null");
            return this;
        }

        public IAutoCompleteControl PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public IAutoCompleteControl MinimumPrefixLength(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "MinimumPrefixLength must be greater or equal than 1");
            }
            _minimumPrefixLength = value;
            return this;
        }

        public IAutoCompleteControl CompletionWaitToStart(int value)
        {
            if (value < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "CompletionWaitToStart must be greater or equal than 100");
            }
            _completionWaitToStart = value;
            return this;
        }

        public IAutoCompleteControl CompletionMaxCount(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "CompletionMaxCount must be greater or equal than 1");
            }
            _completionMaxCount = value;
            return this;
        }

        public IAutoCompleteControl CompletionAsyncService(Func<string, CancellationToken, Task<string[]>> value)
        {
            _autocompleteHandler = value ?? throw new ArgumentNullException(nameof(value), "CompletionAsyncService cannot be null");
            return this;
        }

        public IAutoCompleteControl Default(string value, bool usedefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = usedefaultHistory;
            return this;
        }

        public IAutoCompleteControl DefaultIfEmpty(string value)
        {
            _defaultIfEmpty = value;
            return this;
        }

        public IAutoCompleteControl InputToCase(CaseOptions value)
        {
            _inputToCase = value;
            return this;
        }

        public IAutoCompleteControl AcceptInput(Func<char, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _acceptInput = value;
            return this;
        }

        public IAutoCompleteControl MaxLength(int maxLength, byte? maxWidth = null)
        {
            if (maxLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MaxLength must be greater than or equal to 1.");
            }
            _maxLength = maxLength;
            if (maxWidth.HasValue)
            {
                if (maxWidth < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 1.");
                }
                _maxWidth = maxWidth;
            }
            if (_maxLength < _maxWidth)
            {
                _maxWidth = _maxLength;
            }
            return this;
        }

        public IAutoCompleteControl MaxWidth(byte maxWidth)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 1.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public IAutoCompleteControl PredicateSelected(Func<string, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IAutoCompleteControl PredicateSelected(Func<string, bool> validselect)
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

        public IAutoCompleteControl ChangeDescription(Func<string, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        public IAutoCompleteControl Styles(AutoComleteStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IAutoCompleteControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IAutoCompleteControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public IAutoCompleteControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_autocompleteHandler == null)
            {
                throw new InvalidOperationException("CompletionAsyncService is requeried!");
            }
            _autocompleteTextSelector ??= (item) => item;
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    _defaultValue = _itemHistories[0].History;
                }
            }
            _maxWidth = _maxWidth.HasValue && _maxWidth.Value > _maxLength ? _maxLength : _maxWidth;

            _inputdata = new EmacsBuffer(false, _inputToCase, _acceptInput, _maxLength, _maxWidth);

            EmptyPaginator();

            if (!string.IsNullOrEmpty(_defaultValue))
            {
                _inputdata?.LoadPrintable(_defaultValue);
            }
            _tooltipModeAutoComlete = GetTooltipModeAutoComplete();
            LoadTooltipToggle();
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
                    ConsoleKeyInfo? keyinfo = WaitKeypressAutoComplete(cancellationToken);

                    #region default Press to Finish and tooltip

                    if (keyinfo!.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.None)
                    {
                        //spinner animation or autocomplete
                        _indexTooptip = 0;
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo!.Value))
                    {
                        _cancellationTokenSource?.Cancel();
                        _indexTooptip = 0;
                        EmptyPaginator();
                        ResultCtrl = new ResultPrompt<string>(_inputdata!.ToString(), true);
                        break;
                    }
                    else if (keyinfo.Value.IsPressEnterKey())
                    {
                        _cancellationTokenSource?.Cancel();
                        _indexTooptip = 0;
                        EmptyPaginator();
                        string finishedresult = _inputdata!.ToString();
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(finishedresult) ?? (true, null);
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
                        if (!string.IsNullOrEmpty(_defaultIfEmpty) && finishedresult.Length == 0)
                        {
                            finishedresult = _defaultIfEmpty;
                        }
                        ResultCtrl = new ResultPrompt<string>(finishedresult, false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo.Value))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo.Value))
                    {
                        _indexTooptip = 0;
                        break;
                    }

                    #endregion

                    else if (keyinfo!.Value.IsPressDownArrowKey())
                    {
                        bool ok = _localpaginator!.IsLastPageItem ? _localpaginator.NextPage(IndexOption.FirstItem) : _localpaginator.NextItem();
                        if (ok)
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (keyinfo!.Value.IsPressUpArrowKey())
                    {
                        bool ok = _localpaginator!.IsFirstPageItem ? _localpaginator!.PreviousPage(IndexOption.LastItem) : _localpaginator!.PreviousItem();
                        if (ok)
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (keyinfo!.Value.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (keyinfo!.Value.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (keyinfo!.Value.IsPressCtrlHomeKey())
                    {
                        if (_localpaginator!.Home())
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (keyinfo!.Value.IsPressCtrlEndKey())
                    {
                        if (_localpaginator!.End())
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_autocompleteTextSelector!(_localpaginator.SelectedItem));
                            _lastinput = _inputdata.ToString();
                            break;
                        }
                    }
                    else if (_inputdata!.TryAcceptedReadlineConsoleKey(keyinfo.Value))
                    {
                        _cancellationTokenSource?.Cancel();
                        _observerAutoComplete?.Wait(cancellationToken);
                        _indexTooptip = 0;
                        if (_inputdata.Length < _minimumPrefixLength)
                        {
                            EmptyPaginator();
                        }
                        else if (_inputdata.Length >= _minimumPrefixLength && _lastinput != _inputdata.ToString())
                        {
                            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                            _observerAutoComplete = Task.Run(async () => await ExecuteAutoComplete(_cancellationTokenSource.Token), cancellationToken);
                        }
                        _lastinput = _inputdata.ToString();
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteAutoComplete(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = ResultCtrl!.Value.Content;
            if (ResultCtrl.Value.IsAborted)
            {
                answer = GeneralOptions.ShowMesssageAbortKeyValue ? Messages.CanceledKey : string.Empty;
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[AutoComleteStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[AutoComleteStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            if (!_disposed)
            {
                _disposed = true;
                _cancellationTokenSource?.Cancel();
                _observerAutoComplete?.Wait(CancellationToken.None);
                _cancellationTokenSource?.Dispose();
                _observerAutoComplete?.Dispose();
            }
        }

        private void EmptyPaginator()
        {
            if (_localpaginator != null && _localpaginator!.TotalCount == 0)
            {
                return;
            }
            _localpaginator = new Paginator<string>(
            FilterMode.Disabled,
            [],
            _pageSize,
            Optional<string>.Empty(),
            (item1, item2) => item1 == item2,
            null);
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                Messages.TooltipPages
            ];
            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}");
            }
            else
            {
                lsttooltips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}");
            }
            lsttooltips.AddRange(EmacsBuffer.GetEmacsTooltips());
            _toggerTooptips = [.. lsttooltips];
        }

        private string GetTooltipModeAutoComplete()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.InputFinishEnter);
            return tooltip.ToString();
        }

        private async Task ExecuteAutoComplete(CancellationToken token)
        {
            EmptyPaginator();
            _autoCompleteRunning = true;
            token.WaitHandle.WaitOne(_completionWaitToStart);
            if (!token.IsCancellationRequested)
            {
                try
                {
                    _localpaginator = new Paginator<string>(
                        FilterMode.Disabled,
                        await _autocompleteHandler!.Invoke(_inputdata!.ToString(), token),
                        Math.Min(_pageSize, _completionMaxCount),
                        Optional<string>.Empty(),
                        (item1, item2) => item1 == item2,
                        (item) => item);
                }
                catch (Exception)
                {
                    SetError(Messages.ErrorAutoCompleteService);
                }
                finally
                {
                    _localpaginator!.UnSelect();
                    _finishautoCompleteRunning = true;
                }
            }
            _autoCompleteRunning = false;
        }

        private ConsoleKeyInfo? WaitKeypressAutoComplete(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_finishautoCompleteRunning)
                {
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                else if (_autoCompleteRunning && _spinner!.HasNextFrame(out string? newframe))
                {
                    _currentspinnerFrame = newframe;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(true) : null;
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[AutoComleteStyles.Prompt]);
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_inputdata!.ToString()) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[AutoComleteStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            Style styleAnswer = _optStyles[AutoComleteStyles.Answer];

            if (_inputdata!.IsVirtualBuffer)
            {
                string str = _inputdata!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, styleAnswer);
            }
            screenBuffer.Write(_inputdata!.ToBackward(), styleAnswer);
            screenBuffer.SavePromptCursor();
            if (_inputdata!.IsVirtualBuffer)
            {
                screenBuffer.Write(_inputdata!.ToForward(), styleAnswer);
                string str = _inputdata.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.Write(str, styleAnswer);
            }
            else
            {
                screenBuffer.Write(_inputdata!.ToForward(), styleAnswer);
            }
            if (_currentspinnerFrame != null && _autoCompleteRunning)
            {
                screenBuffer.Write(" ", styleAnswer);
                screenBuffer.Write(_currentspinnerFrame, _optStyles[AutoComleteStyles.Spinner]);
            }
            screenBuffer.WriteLine("", styleAnswer);
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[AutoComleteStyles.Error]);
                ClearError();
            }
        }

        private void WriteAutoComplete(BufferScreen screenBuffer)
        {
            _finishautoCompleteRunning = false;
            if (_localpaginator!.TotalCount == 0)
            {
                return;
            }
            screenBuffer.WriteLine(Messages.EntryOptions, _optStyles[AutoComleteStyles.Selected]);
            ArraySegment<string> subset = _localpaginator!.GetPageData(); // Cache the page data
            foreach (string item in subset)
            {
                if (_localpaginator.TryGetSelected(out string? selectedItem) && EqualityComparer<string>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[AutoComleteStyles.Selected]);
                    screenBuffer.WriteLine($" {item}", _optStyles[AutoComleteStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", Style.Default());
                    screenBuffer.WriteLine($" {item}", _optStyles[AutoComleteStyles.UnSelected]);
                }
            }

            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[AutoComleteStyles.Pagination]);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = _indexTooptip > 0 ? _toggerTooptips[_indexTooptip - 1] : _tooltipModeAutoComlete;
            screenBuffer.Write(tooltip, _optStyles[AutoComleteStyles.Tooltips]);
        }
    }
}
