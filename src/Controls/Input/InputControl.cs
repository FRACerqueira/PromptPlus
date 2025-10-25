// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.Input
{
    internal sealed class InputControl : BaseControlPrompt<string>, IInputControl
    {
        private readonly Dictionary<InputStyles, Style> _optStyles = BaseControlOptions.LoadStyle<InputStyles>();
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Input,[] },
            { ModeView.Suggestion,[] },
            { ModeView.History,[] }
        };

        private Func<char, bool> _acceptInput = (_) => true;
        private Func<string, (bool, string?)>? _predicatevalidselect;
        private Func<string, string>? _changeDescription;
        private string _defaultValue = string.Empty;
        private bool _useDefaultHistory;
        private string _defaultIfEmpty = string.Empty;
        private HistoryOptions? _historyOptions;
        private HotKey? _enabledViewSecret;
        private CaseOptions _inputToCase = CaseOptions.Any;
        private char _secretChar = '#';
        private bool _isinputsecret;
        private bool _passwordvisible;
        private int _maxLength = int.MaxValue;
        private int? _maxWidth;
        private Func<string, string[]>? _suggestionHandler;
        private string[] _suggestions = [];
        private int _curentSuggestion = -1;
        private IList<ItemHistory>? _itemHistories;
        private EmacsBuffer? _inputdata;
        private bool _abortedKeyPress;
        private ModeView _modeView = ModeView.Input;
        private string? _savedinput;
        private Paginator<ItemHistory>? _localpaginator;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string _tooltipModeHistory = string.Empty;
        private string _tooltipModeSuggestion = string.Empty;

        public InputControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _enabledViewSecret = ConfigPlus.HotKeyPasswordView;
        }

        #region IInputControls

        public IInputControl AcceptInput(Func<char, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _acceptInput = value;
            return this;
        }

        public IInputControl ChangeDescription(Func<string, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        public IInputControl Default(string value, bool usedefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = usedefaultHistory;
            return this;
        }

        public IInputControl DefaultIfEmpty(string value)
        {
            _defaultIfEmpty = value;
            return this;
        }

        public IInputControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public IInputControl InputToCase(CaseOptions value)
        {
            _inputToCase = value;
            return this;
        }

        public IInputControl IsSecret(char? value = null, bool enabledView = true)
        {
            _isinputsecret = true;
            _secretChar = value ?? '#';
            if (enabledView)
            {
                _enabledViewSecret = ConfigPlus.HotKeyPasswordView;
            }
            else
            {
                _enabledViewSecret = null;
            }
            return this;
        }

        public IInputControl MaxLength(int maxLength, byte? maxWidth = null)
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
            return this;
        }

        public IInputControl MaxWidth(byte maxWidth)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 1.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public IInputControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IInputControl Styles(InputStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IInputControl SuggestionHandler(Func<string, string[]> value)
        {
            _suggestionHandler = value ?? throw new ArgumentNullException(nameof(value));
            return this;
        }

        public IInputControl PredicateSelected(Func<string, bool> validselect)
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

        public IInputControl PredicateSelected(Func<string, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }
        #endregion

        public override void InitControl(CancellationToken _)
        {
            ValidateSecretInputConstraints();

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
            if (!string.IsNullOrEmpty(_defaultValue))
            {
                _inputdata?.LoadPrintable(_defaultValue);
            }
            _tooltipModeInput = GetTooltipModeInput();
            _tooltipModeHistory = GetTooltipModeHistory();
            _tooltipModeSuggestion = GetTooltipModeSuggestion();
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
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _inputdata!.LoadPrintable(_savedinput!);
                            _savedinput = null;
                            _localpaginator = null;
                            _curentSuggestion = -1;
                            _modeView = ModeView.Input;
                            break;
                        }
                        _abortedKeyPress = true;
                        ResultCtrl = new ResultPrompt<string>(_inputdata!.ToString(), true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _inputdata!.LoadPrintable(_savedinput!);
                            _savedinput = null;
                            _localpaginator = null;
                            _curentSuggestion = -1;
                            _modeView = ModeView.Input;
                            break;
                        }
                        _abortedKeyPress = true;
                        ResultCtrl = new ResultPrompt<string>(_inputdata!.ToString(), true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _savedinput = null;
                            _localpaginator = null;
                            _curentSuggestion = -1;
                            _modeView = ModeView.Input;
                        }
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
                        if (!_isinputsecret && !string.IsNullOrEmpty(_defaultIfEmpty) && finishedresult.Length == 0)
                        {
                            finishedresult = _defaultIfEmpty;
                        }
                        ResultCtrl = new ResultPrompt<string>(finishedresult, false);
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

                    # region Suggestions
                    else if ((_modeView == ModeView.Input || _modeView == ModeView.History) && _suggestionHandler != null && (keyinfo.IsPressTabKey() || keyinfo.IsPressShiftTabKey()))
                    {
                        _indexTooptip = 0;
                        _savedinput = _inputdata!.ToString();
                        _suggestions = _suggestionHandler!(_savedinput);
                        _curentSuggestion = -1;
                        _localpaginator = null;
                        _modeView = ModeView.Suggestion;
                    }
                    else if (_modeView == ModeView.Suggestion && (keyinfo.IsPressTabKey() || keyinfo.IsPressShiftTabKey()))
                    {
                        _indexTooptip = 0;
                        if (keyinfo.IsPressTabKey())
                        {
                            _curentSuggestion++;
                            if (_curentSuggestion > _suggestions.Length - 1)
                            {
                                _curentSuggestion = 0;
                            }
                        }
                        else
                        {
                            _curentSuggestion--;
                            if (_curentSuggestion < 0)
                            {
                                _curentSuggestion = _suggestions.Length - 1;
                            }
                        }
                        _inputdata!.LoadPrintable(_suggestions[_curentSuggestion]);
                        break;
                    }
                    #endregion

                    #region Histories
                    else if ((_modeView == ModeView.Input || _modeView == ModeView.Suggestion) && ConfigPlus.HotKeyShowHistory.Equals(keyinfo) && (_itemHistories?.Count ?? 0) > 0 && _inputdata!.Length >= _historyOptions!.MinPrefixLengthValue)
                    {
                        _indexTooptip = 0;
                        IEnumerable<ItemHistory> subhist = GetItemHistory(FilterMode.StartsWith);
                        if (!subhist.Any())
                        {
                            SetError(Messages.HistoryNotFound);
                            break;
                        }
                        _savedinput = _inputdata!.ToString();
                        _localpaginator = new Paginator<ItemHistory>(
                            FilterMode.StartsWith,
                            subhist,
                            _historyOptions!.PageSizeValue,
                            Optional<ItemHistory>.Empty(),
                            (item1, item2) => item1.History == item2.History,
                            (item) => item.History);
                        _modeView = ModeView.History;
                        _inputdata!.LoadPrintable(_localpaginator.SelectedItem.History);
                        _indexTooptip = 0;
                        _curentSuggestion = -1;
                        break;

                    }
                    else if (_modeView == ModeView.History)
                    {
                        if (keyinfo.IsPressCtrlDeleteKey())
                        {
                            _indexTooptip = 0;
                            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
                            _itemHistories!.Clear();
                            _inputdata!.LoadPrintable(_savedinput!);
                            _indexTooptip = 0;
                            _localpaginator = null;
                            _savedinput = null;
                            _modeView = ModeView.Input;
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
                            _inputdata!.LoadPrintable(_localpaginator.SelectedItem.History);
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
                            _inputdata!.LoadPrintable(_localpaginator.SelectedItem.History);
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _inputdata!.LoadPrintable(_localpaginator.SelectedItem.History);
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _inputdata!.LoadPrintable(_localpaginator.SelectedItem.History);
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
                    }
                    #endregion

                    else if (_enabledViewSecret != null && ConfigPlus.HotKeyPasswordView.Equals(keyinfo))
                    {
                        _passwordvisible = !_passwordvisible;
                        _indexTooptip = 0;
                        break;
                    }

                    else if (_inputdata!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _savedinput = null;
                            _localpaginator = null;
                            _curentSuggestion = -1;
                            _modeView = ModeView.Input;
                            break;
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteHistory(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = ResultCtrl!.Value.Content;
            if (ResultCtrl.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
                else
                {
                    answer = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[InputStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[InputStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            string itemhist = _inputdata?.ToString() ?? string.Empty;
            if (_historyOptions != null && !_abortedKeyPress && !string.IsNullOrEmpty(itemhist))
            {
                FileHistory.AddHistory(itemhist, _historyOptions.ExpirationTimeValue, _itemHistories);
                FileHistory.SaveHistory(_historyOptions.FileNameValue, _itemHistories!, _historyOptions.MaxItemsValue);
            }
        }

        #region private methods

        private void ValidateSecretInputConstraints()
        {
            if (!_isinputsecret)
            {
                _enabledViewSecret = null;
                return;
            }

            if (_historyOptions != null || _suggestionHandler != null ||
                !string.IsNullOrEmpty(_defaultValue) || !string.IsNullOrEmpty(_defaultIfEmpty))
            {
                throw new InvalidOperationException("Secret input cannot be used with history, suggestions or default values.");
            }
        }

        private IEnumerable<ItemHistory> GetItemHistory(FilterMode filterMode)
        {
            DateTime currentTime = DateTime.Now; // Cache the current time
            string inputData = _inputdata!.ToString(); // Cache the input data
            return _itemHistories!.Where(x =>
                currentTime < new DateTime(x.TimeOutTicks) &&
                (filterMode == FilterMode.Contains
                    ? x.History.Contains(inputData, StringComparison.InvariantCultureIgnoreCase)
                    : x.History.StartsWith(inputData, StringComparison.InvariantCultureIgnoreCase)));
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[InputStyles.Prompt]);
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_inputdata!.ToString()) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[InputStyles.Description]);
            }
        }

        private void WriteHistory(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.History)
            {
                return;
            }

            ArraySegment<ItemHistory> subset = _localpaginator!.GetPageData(); // Cache the page data
            screenBuffer.WriteLine(Messages.EntryHistory, _optStyles[InputStyles.Selected]);
            foreach (ItemHistory item in subset)
            {
                string value = item.History;
                if (_localpaginator.TryGetSelected(out ItemHistory selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[InputStyles.Selected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", Style.Default());
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.UnSelected]);
                }
            }

            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[InputStyles.Pagination]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[InputStyles.Error]);
                ClearError();
            }
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
            else if (_modeView == ModeView.Input)
            {
                tooltip = _tooltipModeInput;
            }
            else if (_modeView == ModeView.Suggestion)
            {
                tooltip = _tooltipModeSuggestion;
            }
            else if (_modeView == ModeView.History)
            {
                tooltip = _tooltipModeHistory;
            }
            else
            {
                throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
            screenBuffer.Write(tooltip, _optStyles[InputStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Input => _toggerTooptips[ModeView.Input][_indexTooptip - 1],
                ModeView.Suggestion => _toggerTooptips[ModeView.Suggestion][_indexTooptip - 1],
                ModeView.History => _toggerTooptips[ModeView.History][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            Style styleAnswer = _modeView != ModeView.Input
                ? _optStyles[InputStyles.TaggedInfo]
                : _optStyles[InputStyles.Answer];

            if (_inputdata!.IsVirtualBuffer)
            {
                string str = _inputdata!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, styleAnswer);
            }

            if (_isinputsecret && !_passwordvisible)
            {
                string answer = new(_secretChar, _inputdata!.ToBackward().Length);
                screenBuffer.Write(answer, styleAnswer);
                screenBuffer.SavePromptCursor();
                answer = new string(_secretChar, _inputdata!.ToForward(false).Length);
                if (_inputdata!.IsVirtualBuffer)
                {
                    int spaces = _inputdata!.ToForward().Length - answer.Length;
                    answer += new string(' ', spaces);
                    screenBuffer.Write(answer, styleAnswer);
                    string str = _inputdata.IsHideRightBuffer
                        ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                        : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                    screenBuffer.WriteLine(str, styleAnswer);
                }
                else
                {
                    screenBuffer.WriteLine(answer, styleAnswer);
                }
            }
            else
            {
                screenBuffer.Write(_inputdata!.ToBackward(), styleAnswer);
                screenBuffer.SavePromptCursor();
                if (_inputdata!.IsVirtualBuffer)
                {
                    screenBuffer.Write(_inputdata!.ToForward(), styleAnswer);
                    string str = _inputdata.IsHideRightBuffer
                        ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                        : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                    screenBuffer.WriteLine(str, styleAnswer);
                }
                else
                {
                    screenBuffer.WriteLine(_inputdata!.ToForward(), styleAnswer);
                }
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

                if (mode == ModeView.Input && GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
                }
                if (mode == ModeView.Suggestion)
                {
                    lsttooltips[0] += $", {Messages.TooltipSuggestionEsc}";
                }
                if (mode == ModeView.History)
                {
                    lsttooltips[0] += $", {Messages.TooltipHistoryEsc}";
                }
                lsttooltips.AddRange(EmacsBuffer.GetEmacsTooltips());
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            if (_enabledViewSecret != null)
            {
                tooltip.Append(", ");
                tooltip.Append(string.Format(Messages.TooltipViewPassword, ConfigPlus.HotKeyPasswordView));
            }
            if (_suggestionHandler != null)
            {
                tooltip.Append(", ");
                tooltip.Append(Messages.TooltipSuggestionToggle);
            }
            if (_itemHistories != null && _itemHistories.Count > 0)
            {
                tooltip.Append(", ");
                tooltip.Append(string.Format(Messages.TooltipHistoryShow, ConfigPlus.HotKeyShowHistory, _historyOptions!.MinPrefixLengthValue));
            }
            return tooltip.ToString();
        }

        private string GetTooltipModeHistory()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            if (_suggestionHandler != null)
            {
                tooltip.Append(", ");
                tooltip.Append(Messages.TooltipSuggestionToggle);
            }
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipPages);
            return tooltip.ToString();
        }

        private string GetTooltipModeSuggestion()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            if (_itemHistories != null && _itemHistories.Count > 0)
            {
                tooltip.Append(", ");
                tooltip.Append(string.Format(Messages.TooltipHistoryShow, ConfigPlus.HotKeyShowHistory, _historyOptions!.MinPrefixLengthValue));
            }
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipSuggestionToggle);
            return tooltip.ToString();
        }

        private enum ModeView
        {
            Input,
            Suggestion,
            History
        }

        #endregion
    }
}
