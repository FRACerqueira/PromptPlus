// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace PPlus.Controls
{
    internal class InputControl : BaseControl<string>, IControlInput
    {
        private readonly InputOptions _options;
        private EmacsBuffer _inputBuffer;
        private bool _passwordvisible;
        private Paginator<ItemHistory> _localpaginator;
        private IList<ItemHistory> _itemsHistory = new List<ItemHistory>();
        private string _originalText = string.Empty;
        private bool _isInAutoCompleteMode;
        private int _completionsIndex = -1;
        private SuggestionOutput? _completions = null;
        private string _defaultHistoric = null;


        public InputControl(IConsoleControl console, InputOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.HistoryEnabled)
            {
                LoadHistory();
            }
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
            }

            _inputBuffer = new(_options.InputToCase, _options.AcceptInput, _options.MaxLength);

            if (!string.IsNullOrEmpty(_defaultHistoric))
            {
                _inputBuffer.LoadPrintable(_defaultHistoric);
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.DefaultValue))
                {
                    _inputBuffer.LoadPrintable(_options.DefaultValue);
                }
            }
            FinishResult = _inputBuffer.ToString();
            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlInput

        public IControlInput FilterType(FilterMode value)
        {
            if (value == FilterMode.Disabled)
            {
                _options.HistoryMinimumPrefixLength = 0;
            }
            _options.FilterType = value;
            return this;
        }

        public IControlInput OverwriteDefaultFrom(string value, TimeSpan? timeout)
        {
            if (_options.IsSecret)
            {
                throw new PromptPlusException("OverwriteDefaultFrom cannot be used with input secret");
            }
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                if (timeout.Value.TotalMilliseconds == 0)
                {
                    throw new PromptPlusException("timeout must be greater than 0");
                }
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlInput InputToCase(CaseOptions value)
        {
            _options.InputToCase = value;
            return this;
        }

        public IControlInput AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlInput Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlInput DefaultIfEmpty(string value)
        {
            if (_options.IsSecret)
            {
                throw new PromptPlusException("DefaultIfEmpty cannot be used with input secret");
            }
            _options.DefaultEmptyValue = value;
            return this;
        }

        public IControlInput ChangeDescription(Func<string, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlInput HistoryEnabled(string value)
        {
            if (_options.IsSecret)
            {
                throw new PromptPlusException("HistoryEnabled cannot be used with input secret");
            }
            _options.HistoryFileName = value;
            return this;
        }

        public IControlInput HistoryMaxItems(byte value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("HistoryMaxItems must be greater than 0");
            }
            _options.HistoryMaxItems = value;
            return this;
        }

        public IControlInput HistoryMinimumPrefixLength(int value)
        {
            if (value < 0)
            {
                throw new PromptPlusException("HistoryMinimumPrefixLength must be greater than or equal to zero");
            }
            if (_options.FilterType == FilterMode.Disabled && value > 0)
            {
                throw new PromptPlusException("HistoryMinimumPrefixLength mustbe zero when FilterType is Disabled");
            }
            _options.HistoryMinimumPrefixLength = value;
            return this;
        }

        public IControlInput HistoryPageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("HistoryPageSize must be greater than or equal to 1");
            }
            _options.HistoryPageSize = value;
            return this;
        }

        public IControlInput HistoryTimeout(TimeSpan value)
        {
            if (value.TotalMilliseconds == 0)
            {
                throw new PromptPlusException("HistoryTimeout must be greater than 0");
            }
            _options.HistoryTimeout = value;
            return this;
        }

        public IControlInput Default(string value)
        {
            if (_options.IsSecret)
            {
                throw new PromptPlusException("Default cannot be used with input secret");
            }
            _options.DefaultValue = value;
            return this;
        }

        public IControlInput IsSecret(char? value = '#' )
        {
            if (_options.SuggestionHandler != null)
            {
                throw new PromptPlusException("Input secret cannot have suggestionhandler");
            }
            if (_options.DefaultEmptyValue != null)
            {
                throw new PromptPlusException("Input secret cannot have DefaultEmptyValue");
            }
            if ((_options.OverwriteDefaultFrom??string.Empty).Length > 0)
            {
                throw new PromptPlusException("Input secret cannot have OverwriteDefaultFrom");
            }
            if (_options.DefaultValue != null)
            {
                throw new PromptPlusException("Input secret cannot have DefaultValue");
            }
            if (_options.HistoryFileName != null)
            {
                throw new PromptPlusException("Input secret cannot have HistoryEnabled");
            }
            if (value.HasValue)
            { 
                _options.SecretChar = value.Value;
            }
            _options.IsSecret = true;
            return this;
        }

        public IControlInput EnabledViewSecret(HotKey? hotkeypress = null)
        {
            _options.EnabledViewSecret = true;
            if (hotkeypress.HasValue)
            {
                _options.SwitchView = hotkeypress.Value;
            }
            return this;
        }

        public IControlInput SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value)
        {
            if (_options.IsSecret)
            {
                throw new PromptPlusException("Suggestion handler cannot be used with input secret");
            }
            _options.SuggestionHandler = value;
            return this;
        }

        public IControlInput ValidateOnDemand(bool value = true)
        {
            _options.ValidateOnDemand = value;
            return this;
        }

        public IControlInput MaxLength(ushort value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("MaxLength must be greater than 0");
            }
            _options.MaxLength = value;
            return this;
        }

        public IControlInput AcceptInput(Func<char, bool> value)
        {
            _options.AcceptInput = value;
            return this;
        }

        #endregion

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result, bool aborted)
        {
            string answer;
            if (_options.IsSecret)
            {
                answer = new string(_options.SecretChar, result.Length);
            }
            else
            {
                answer = result;
                if (!aborted)
                {
                    SaveHistory(answer);
                    if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                    {
                        SaveDefaultHistory(answer);
                    }
                }
            }

            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            if (_isInAutoCompleteMode || _options.ShowingHistory)
            {
                var answer = FinishResult;
                screenBuffer.WriteSuggestion(_options, answer);
                screenBuffer.SaveCursor();
            }
            else
            {
                if (_options.IsSecret && !_passwordvisible)
                {
                    var answer = new string(_options.SecretChar, _inputBuffer.ToBackward().Length);
                    screenBuffer.WriteAnswer(_options, answer);
                    screenBuffer.SaveCursor();
                    answer = new string(_options.SecretChar, _inputBuffer.ToForward().Length);
                    screenBuffer.WriteAnswer(_options, answer);
                }
                else
                {
                    screenBuffer.WriteAnswer(_options, _inputBuffer.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteAnswer(_options, _inputBuffer.ToForward());
                }
            }
            screenBuffer.WriteLineDescriptionInput(_options, FinishResult);
            screenBuffer.WriteLineValidate(ValidateError, _options);
            screenBuffer.WriteLineTooltipsInput(_options, _isInAutoCompleteMode);
            if (_options.ShowingHistory)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    var value = item.History;
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(_options, value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, value);
                    }
                }
                if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
        }

        public override ResultPrompt<string> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            bool tryagain;
            do
            {
                ClearError();
                tryagain = false;
                var keyInfo = WaitKeypress(cancellationToken);
                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    continue;
                }
                if (CheckAbortKey(keyInfo.Value) && !(_options.ShowingHistory || _isInAutoCompleteMode))
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (_options.IsSecret && _options.EnabledViewSecret && _options.SwitchView.Equals(keyInfo.Value))
                {
                    _passwordvisible = !_passwordvisible;
                    continue;
                }

                _originalText = _inputBuffer.ToString();
                if (_options.InputToCase != CaseOptions.Any)
                {
                    keyInfo = keyInfo.Value.ToCase(_options.InputToCase);
                }

                var acceptedkey = _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value);
                if (acceptedkey)
                {
                    if (_options.ShowingHistory || _isInAutoCompleteMode)
                    {
                        ClearMode();
                    }
                }
                //completed input
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    ClearMode();
                    if (!_options.IsSecret && string.IsNullOrEmpty(FinishResult) && !string.IsNullOrEmpty(_options.DefaultEmptyValue))
                    {
                        _inputBuffer.LoadPrintable(_options.DefaultEmptyValue);
                    }
                    endinput = true;
                    break;
                }
                //apply suggestion
                else if (_options.SuggestionHandler != null && (keyInfo.Value.IsPressTabKey() || keyInfo.Value.IsPressShiftTabKey()))
                {
                    if (!_isInAutoCompleteMode)
                    {
                        _completions = _options.SuggestionHandler.Invoke(new SuggestionInput(_inputBuffer.ToString(), _options.OptContext));
                        if (_completions.HasValue && _completions.Value.Suggestions.Count > 0)
                        {
                            _completionsIndex = -1;
                            _options.ShowingHistory = false;
                            _localpaginator = null;
                            _isInAutoCompleteMode = true;
                        }
                        else
                        {
                            tryagain = true;
                        }
                    }
                    ExecuteAutoComplete(keyInfo.Value.IsPressShiftTabKey());
                }
                //cancel suggestion
                else if (_options.SuggestionHandler != null && _isInAutoCompleteMode && keyInfo.Value.IsPressEscKey())
                {
                    _isInAutoCompleteMode = false;
                    _completionsIndex = -1;
                    _completions = null;
                    _inputBuffer.Clear().LoadPrintable(_originalText);
                }
                //show history
                else if (_options.HistoryEnabled && !_options.ShowingHistory && (keyInfo.Value.IsPressDownArrowKey() || keyInfo.Value.IsPressPageDownKey()) && _itemsHistory.Count > 0 && _inputBuffer.Length >= _options.HistoryMinimumPrefixLength)
                {
                    _localpaginator = new Paginator<ItemHistory>(
                        _options.FilterType,
                        GetItemHistory(_options.FilterType),
                        _options.HistoryPageSize, Optional<ItemHistory>.s_empty, 
                        (item1,item2) => item1.History == item2.History,
                        (item) => item.History);

                    if (_localpaginator.Count > 0)
                    {
                        if (_isInAutoCompleteMode)
                        {
                            _completionsIndex = -1;
                            _completions = null;
                            _isInAutoCompleteMode = false;
                        }
                        _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                        _options.ShowingHistory = true;
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
                //clear history
                else if (_options.HistoryEnabled && _options.ShowingHistory && keyInfo.Value.IsPressCtrlDeleteKey() && _itemsHistory.Count > 0)
                {
                    ClearHistory();
                    _options.ShowingHistory = false;
                    _localpaginator = null;
                }
                //Navegator history
                else if (_options.HistoryEnabled && _options.ShowingHistory && IskeyPageNavegator(keyInfo.Value, _localpaginator))
                {
                    _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                }
                //cancel history
                else if (_options.HistoryEnabled && _options.ShowingHistory && keyInfo.Value.IsPressEscKey())
                {
                    _inputBuffer.Clear().LoadPrintable(_originalText);
                    _options.ShowingHistory = false;
                    _localpaginator = null;
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                _inputBuffer.Clear();
                endinput = true;
                abort = true;
            }
            FinishResult = _inputBuffer.ToString();
            _originalText = FinishResult;
            if (_options.ValidateOnDemand || endinput)
            {
                ClearError();
                if (!TryValidate(FinishResult, _options.Validators))
                {
                    if (!abort)
                    {
                        endinput = false;
                    }
                }
            }
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            return new ResultPrompt<string>(FinishResult, abort,!endinput,notrender);
        }


        private IEnumerable<ItemHistory> GetItemHistory(FilterMode filterMode)
        {
            if (filterMode == FilterMode.Contains)
            {
                return _itemsHistory.Where(x => x.History.Contains(_inputBuffer.ToString(), StringComparison.InvariantCultureIgnoreCase)
                                && DateTime.Now < new DateTime(x.TimeOutTicks));
            }
            else if (filterMode == FilterMode.StartsWith)
            {
                return _itemsHistory.Where(x => x.History.StartsWith(_inputBuffer.ToString(), StringComparison.InvariantCultureIgnoreCase)
                            && DateTime.Now < new DateTime(x.TimeOutTicks));
            }
            return _itemsHistory.Where(x => DateTime.Now < new DateTime(x.TimeOutTicks));
        }

        private void ClearMode()
        {
            _completionsIndex = -1;
            _options.ShowingHistory = false;
            _localpaginator = null;
            _completions = null;
            _isInAutoCompleteMode = false;
        }

        private bool ExecuteAutoComplete(bool Previus)
        {
            if (!_completions.HasValue)
            {
                return false;
            }
            if (Previus)
            {
                PreviusCompletions();
            }
            else
            {
                NextCompletions();
            }
            _inputBuffer.Clear().LoadPrintable(_completions.Value.Suggestions[_completionsIndex]);
            return true;
        }

        private void NextCompletions()
        {
            _completionsIndex++;
            if (_completionsIndex > _completions.Value.Suggestions.Count - 1)
            {
                _completionsIndex = 0;
            }
        }

        private void PreviusCompletions()
        {
            _completionsIndex--;
            if (_completionsIndex < 0)
            {
                _completionsIndex = _completions.Value.Suggestions.Count - 1;
            }
        }

        private void LoadHistory()
        {
            if (_options.HistoryFileName != null)
            {
                _itemsHistory = FileHistory.LoadHistory(_options.HistoryFileName, _options.HistoryMaxItems);
            }
        }

        private void SaveHistory(string? value)
        {
            if (_options.HistoryFileName != null && value != null && value.Length > 0)
            {
                FileHistory.AddHistory(value, _options.HistoryTimeout, _itemsHistory);
                FileHistory.SaveHistory(_options.HistoryFileName, _itemsHistory, _options.HistoryMaxItems);
            }
        }

        private void LoadDefaultHistory()
        {
            _defaultHistoric = null;
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = aux[0].History;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveDefaultHistory(string value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(value, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }

        private void ClearHistory()
        {
            if (_options.HistoryFileName != null)
            {
                FileHistory.ClearHistory(_options.HistoryFileName);
                _itemsHistory.Clear();
            }
        }
    }
}
