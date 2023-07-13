using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class AutoCompleteControl: BaseControl<string>, IControlAutoComplete, IDisposable
    {
        private bool _disposed = false;
        private readonly AutoCompleteOptions _options;
        private EmacsBuffer _inputBuffer;
        private string _defaultHistoric = null;
        private readonly List<string> _inputItems = new();
        private Paginator<string> _localpaginator;
        private CancellationTokenSource _cts = new();
        private readonly CancellationTokenSource _ctsObserver = new();
        private Task _observerAutoComplete;
        private bool _autoCompleteSendStart;
        private bool _autoCompleteRunning;
        private bool _finishautoCompleteRunning;

        public AutoCompleteControl(IConsoleControl console, AutoCompleteOptions options) : base(console, options)
        {
            _options = options;
        }

        internal bool IsAutoCompleteFinish => _finishautoCompleteRunning;


        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.CompletionAsyncService == null)
            {
                throw new PromptPlusException("AutoComplete.CompletionAsyncService is requeried!");
            }

            _localpaginator = new Paginator<string>(
                FilterMode.Contains,
                _inputItems,
                Math.Min(_options.PageSize, _options.CompletionMaxCount),
                Optional<string>.s_empty,
                (item1, item2) => item1 == item2,
                (item) => item);

            _localpaginator.FirstItem();

            if (!cancellationToken.IsCancellationRequested)
            {
                _observerAutoComplete = Task.Run(() =>
                {
                    var tk = ObserverAutoComplete(_ctsObserver.Token);
                 }, CancellationToken.None);
            }
            else
            {
                FinishResult = _inputBuffer.ToString();
                return FinishResult;
            }

            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
            }

            _inputBuffer = new(_options.InputToCase, _options.AcceptInput, _options.MaxLenght);

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
            Dispose();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cts?.Cancel();
                    _ctsObserver?.Cancel();
                    _observerAutoComplete?.Wait(CancellationToken.None);
                    _cts?.Dispose();
                    _ctsObserver?.Dispose();

                }
                _disposed = true;
            }
        }

        #endregion

        #region IControlAutoComplete

        public IControlAutoComplete PageSize(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.PageSize = value;
            return this;
        }

        public IControlAutoComplete Spinner(SpinnersType spinnersType, Style? spinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
        {
            if (spinnersType == SpinnersType.Custom && customspinner.Any())
            {
                throw new PromptPlusException("Custom spinner not have data");
            }
            if (spinnersType == SpinnersType.Custom)
            {
                _options.Spinner = new Spinners(SpinnersType.Custom, ConsolePlus.IsUnicodeSupported,speedAnimation??80,customspinner);
            }
            else
            {
                _options.Spinner = new Spinners(spinnersType, ConsolePlus.IsUnicodeSupported);
            }
            if (spinnerStyle.HasValue)
            {
                _options.SpinnerStyle = spinnerStyle.Value;
            }
            return this;
        }

        public IControlAutoComplete MinimumPrefixLength(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.MinimumPrefixLength = value;
            return this;
        }

        public IControlAutoComplete CompletionWaitToStart(int value)
        {
            if (value < 100)
            {
                value = 100;
            }
            _options.CompletionWaitToStart = value;
            return this;
        }

        public IControlAutoComplete CompletionMaxCount(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.CompletionMaxCount = value;
            return this;
        }

        public IControlAutoComplete CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value)
        {
            _options.CompletionAsyncService = value;
            return this;
        }

        public IControlAutoComplete DefaultIfEmpty(string value)
        {
            _options.DefaultEmptyValue = value;
            return this;
        }

        public IControlAutoComplete Default(string value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlAutoComplete OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlAutoComplete InputToCase(CaseOptions value)
        {
            _options.InputToCase = value;
            return this;
        }

        public IControlAutoComplete AcceptInput(Func<char, bool> value)
        {
            _options.AcceptInput = value;
            return this;
        }

        public IControlAutoComplete MaxLenght(ushort value)
        {
            _options.MaxLenght = value;
            return this;
        }

        public IControlAutoComplete AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlAutoComplete ValidateOnDemand(bool value = true)
        {
            _options.ValidateOnDemand = value;
            return this;
        }

        public IControlAutoComplete ChangeDescription(Func<string, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlAutoComplete Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        #endregion

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            screenBuffer.WriteAnswer(_options, _inputBuffer.ToBackward());
            screenBuffer.SaveCursor();
            screenBuffer.WriteAnswer(_options, _inputBuffer.ToForward());
            if (_autoCompleteRunning)
            {
                var spn = _options.Spinner.NextFrame(CancellationToken);
                screenBuffer.AddBuffer($" {spn}",_options.SpinnerStyle,true);
            }
            screenBuffer.WriteLineDescriptionAutoComplete(_options, FinishResult);
            screenBuffer.WriteLineValidate(ValidateError, _options);
            screenBuffer.WriteLineTooltipsAutoComplete(_options, _localpaginator.TotalCount > 0 && !_autoCompleteRunning);
            if (!_autoCompleteRunning && _inputBuffer.ToString().Length >= _options.MinimumPrefixLength && _localpaginator.TotalCount > 0)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<string>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, item);
                    }
                }
                if (_localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result, bool aborted)
        {
            _ctsObserver.Cancel();
            var answer = result;
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            if (!aborted)
            {
                if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                {
                    SaveDefaultHistory(answer);
                }
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<string> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            var oldinput = _inputBuffer.ToString();
            bool tryagain;
            bool skipstartComplete;
            do
            {
                ClearError();
                tryagain = false;
                ConsoleKeyInfo? keyInfo;
                skipstartComplete = false;
                if (_autoCompleteRunning)
                {
                    if (!KeyAvailable)
                    {
                        cancellationToken.WaitHandle.WaitOne(10);
                        break;
                    }
                }
                if (_finishautoCompleteRunning)
                {
                    _finishautoCompleteRunning = false;
                    break;
                }

                keyInfo = WaitKeypress(cancellationToken);

                if (!keyInfo.HasValue)
                {
                    _autoCompleteSendStart = false;
                    _cts.Cancel(); 
                    endinput = true;
                    abort = true;
                    break;
                }
                else if (CheckAbortKey(keyInfo.Value))
                {
                    _autoCompleteSendStart = false;
                    _cts.Cancel();
                    abort = true;
                    endinput = true;
                    break;
                }
                else if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    break;
                }
                if (_options.InputToCase != CaseOptions.Any)
                {
                    keyInfo = keyInfo.Value.ToCase(_options.InputToCase);
                }
                var acceptedkey = _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value);
                if (acceptedkey)
                {
                    break;
                }
                if (!_autoCompleteRunning && _localpaginator.Count > 0)
                {
                    if (keyInfo.Value.IskeyPageNavagator(_localpaginator))
                    {
                        _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem);
                        skipstartComplete = true;
                        break;
                    }
                }
                //completed input
                if (keyInfo.Value.IsPressEnterKey())
                {
                    if (string.IsNullOrEmpty(FinishResult) && !string.IsNullOrEmpty(_options.DefaultEmptyValue))
                    {
                        _inputBuffer.LoadPrintable(_options.DefaultEmptyValue);
                    }
                    endinput = true;
                    break;
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
            if (_inputBuffer.Length < _options.MinimumPrefixLength)
            {
                if (_autoCompleteRunning)
                {
                    _cts.Cancel();
                    _autoCompleteRunning = false;
                }
                _autoCompleteSendStart = false;
                _autoCompleteRunning = false;
                _inputItems.Clear();
                _localpaginator = new Paginator<string>(
                    FilterMode.Contains,
                    _inputItems, 
                    Math.Min(_options.PageSize, _options.CompletionMaxCount), 
                    Optional<string>.s_empty,
                    (item1, item2) => item1 == item2,
                    (item) => item);
                _localpaginator.UnSelected();
            }
            else
            {
                if (!skipstartComplete)
                {
                    if (!_autoCompleteRunning && oldinput != _inputBuffer.ToString())
                    {
                        _autoCompleteRunning = true;
                        _autoCompleteSendStart = true;
                    }
                    else
                    {
                        _autoCompleteSendStart = (oldinput != _inputBuffer.ToString());

                    }
                }
            }
            var clear = !_autoCompleteRunning;
            if (_options.Spinner.IsReseted)
            {
                clear = true;
            }
            return new ResultPrompt<string>(FinishResult, abort, !endinput,false, clear);
        }

        private void LoadDefaultHistory()
        {
            _defaultHistoric = null;
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom,1);
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

        private Task ObserverAutoComplete(CancellationToken cancellationToken)
        {
            Task<string[]> _tasksRunning = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_autoCompleteSendStart)
                {
                    _autoCompleteRunning = true;
                    if (_tasksRunning == null)
                    {
                        _autoCompleteSendStart = false;
                        using (var lcts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken))
                        {
                            lcts.Token.WaitHandle.WaitOne(_options.CompletionWaitToStart);
                            if (!lcts.Token.IsCancellationRequested)
                            {
                                _tasksRunning = _options.CompletionAsyncService.Invoke(_inputBuffer.ToString(), _options.CompletionMaxCount, lcts.Token);
                                if (_tasksRunning.IsCompletedSuccessfully)
                                {
                                    _inputItems.Clear();
                                    _inputItems.AddRange(_tasksRunning.Result.ToArray());
                                    _localpaginator = new Paginator<string>(
                                        FilterMode.Contains,
                                        _inputItems,
                                        Math.Min(_options.PageSize, _options.CompletionMaxCount),
                                        Optional<string>.s_empty,
                                        (item1, item2) => item1 == item2,
                                        (item) => item);
                                    _localpaginator.UnSelected();
                                }
                            }
                        }
                        if (_tasksRunning == null || _tasksRunning.IsCompleted)
                        {
                            _options.Spinner.Reset();
                            _tasksRunning = null;
                            _autoCompleteRunning = false;
                            _finishautoCompleteRunning = true;
                            _cts = new CancellationTokenSource();
                        }
                    }
                    else
                    {
                        _options.Spinner.Reset();
                        _cts.Cancel();
                    }
                }
                else
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.WaitHandle.WaitOne(10);
                    }
                }
            }
            _finishautoCompleteRunning = true;
            if (_tasksRunning != null &&  !_tasksRunning.IsCompleted)
            {
                _cts.Cancel();
                _tasksRunning.Wait(CancellationToken.None);
            }
            _tasksRunning?.Dispose();
            return Task.CompletedTask;
        }
    }
}
