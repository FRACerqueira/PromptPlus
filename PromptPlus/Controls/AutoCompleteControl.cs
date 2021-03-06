// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal class AutoCompleteControl : ControlBase<string>, IControlAutoComplete, IDisposable
    {
        private const string Namecontrol = "PromptPlus.AutoComplete";
        private const string Twirl = "|/-\\";
        private readonly AutoCompleteOptions _options;
        private readonly ReadLineBuffer _inputBuffer = new();
        private readonly List<ValueDescription<string>> _inputItems = new();
        private readonly CancellationTokenSource _ctsObserver = new();
        private readonly string _startDescription;

        private Paginator<ValueDescription<string>> _localpaginator;
        private CancellationTokenSource _cts = new();
        private string _prefixstart = string.Empty;
        private int _index = -1;
        private Task _observerAutocomplete;
        private bool _autoCompleteSendStart;
        private bool _autoCompleteRunning;
        public AutoCompleteControl(AutoCompleteOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
            _startDescription = _options.Description;
        }

        public new void Dispose()
        {
            _cts.Cancel();
            _ctsObserver.Cancel();
            while (!_observerAutocomplete.IsCompleted)
            {
                Thread.Sleep(100);
            }
            _observerAutocomplete.Dispose();
            _ctsObserver.Dispose();
            _cts.Dispose();
            base.Dispose();
        }

        public override string InitControl()
        {
            if (_options.MinimumPrefixLength < AutoCompleteOptions.MinPrefixLength)
            {
                _options.MinimumPrefixLength = AutoCompleteOptions.MinPrefixLength;
            }
            if (_options.CompletionInterval < 0)
            {
                _options.CompletionInterval = 0;
            }
            if (_options.CompletionMaxCount < AutoCompleteOptions.MinCompletionMaxCount)
            {
                _options.CompletionMaxCount = AutoCompleteOptions.MinCompletionMaxCount;
            }
            _options.PageSize ??= _options.CompletionMaxCount;
            if (_options.PageSize < 1)
            {
                _options.PageSize = 1;
            }
            if (_options.CompletionAsyncService == null && _options.CompletionWithDescriptionAsyncService == null)
            {
                throw new ArgumentNullException(nameof(_options.CompletionAsyncService));
            }
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
                AddLog("CaseInsensitive", _options.CaseInsensitive.ToString(), LogKind.Property);
                AddLog("MinimumPrefixLength", _options.MinimumPrefixLength.ToString(), LogKind.Property);
                AddLog("CompletionMaxCount", _options.CompletionMaxCount.ToString(), LogKind.Property);
                AddLog("CompletionInterval", _options.CompletionInterval.ToString(), LogKind.Property);
            }

            _localpaginator = new Paginator<ValueDescription<string>>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<ValueDescription<string>>.s_empty, x => x.Value);
            _localpaginator.FirstItem();
            _observerAutocomplete = Task.Run(() => ObserverAutoComplete(CancellationToken.None));

            return _inputBuffer.ToString();

        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out string result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                ConsoleKeyInfo keyInfo;
                if (_autoCompleteRunning)
                {
                    keyInfo = GetKeyAvailable(cancellationToken);
                    if (keyInfo.KeyChar == (char)0)
                    {
                        cancellationToken.WaitHandle.WaitOne(_options.SpeedAnimation);
                        continue;
                    }
                }
                else
                {
                    _autoCompleteSendStart = false;
                    keyInfo = WaitKeypress(cancellationToken);
                }
                if (CheckDefaultKey(keyInfo))
                {
                    break;
                }
                if (_localpaginator.Count > 0)
                {
                    if (IskeyPageNavagator(keyInfo, _localpaginator))
                    {
                        break;
                    }
                    else if (PromptPlus.UnSelectFilter.Equals(keyInfo))
                    {
                        _localpaginator.UnSelected();
                        break;
                    }
                }
                if (keyInfo.IsPressSpecialKey(ConsoleKey.Enter, ConsoleModifiers.Control))
                {
                    var findok = _localpaginator.TryGetSelectedItem(out var aux);
                    if (findok)
                    {
                        _inputBuffer.Clear();
                        _inputBuffer.LoadPrintable(aux.Value);
                        _prefixstart = aux.Value;
                    }
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    _autoCompleteSendStart = false;
                    _cts.Cancel();
                    result = _inputBuffer.ToString();
                    try
                    {
                        if (!TryValidate(result, _options.Validators, false))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetError(ex);
                        break;
                    }
                    if (!_options.AcceptWithoutMatch)
                    {
                        var findok = _localpaginator.TryGetSelectedItem(out var aux);
                        if (!findok)
                        {
                            SetError(string.Format(Messages.AutoCompleteKeyNotfound, _inputBuffer.ToString()));
                            break;
                        }
                        result = aux.Value;
                        _inputBuffer.Clear();
                        _inputBuffer.LoadPrintable(result);
                        _prefixstart = result;
                    }
                    return true;
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    _inputBuffer.Clear();
                }
                else
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        var oldiput = _inputBuffer.ToString().ToLowerInvariant();
                        _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo, _inputBuffer.ToString(), out var acceptedkey);
                        if (!acceptedkey)
                        {
                            isvalidhit = null;
                        }
                    }
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            var localinput = _inputBuffer.ToString();
            var localprefix = _prefixstart;
            if (_options.CaseInsensitive)
            {
                localinput = localinput.ToLowerInvariant();
                localprefix = localprefix.ToLowerInvariant();
            }
            if (localprefix != localinput)
            {
                _prefixstart = _inputBuffer.ToString();
                if (_autoCompleteRunning)
                {
                    _cts.Cancel();
                }
                if (_prefixstart.Length < _options.MinimumPrefixLength)
                {
                    _autoCompleteSendStart = false;
                    _autoCompleteRunning = false;
                    _inputItems.Clear();
                    _localpaginator = new Paginator<ValueDescription<string>>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<ValueDescription<string>>.s_empty, x => x.Value);
                    _localpaginator.UnSelected();
                }
                else
                {
                    _autoCompleteSendStart = true;
                    _autoCompleteRunning = true;
                }
            }
            if (_options.DynamicDescription)
            {
                if (_inputItems.Count == 0 || _autoCompleteRunning)
                {
                    _options.Description = _startDescription;
                }
                else
                {
                    if (_localpaginator.SelectedIndex < 0)
                    {
                        _options.Description = _startDescription;
                    }
                    else
                    {
                        _options.Description = _localpaginator.SelectedItem.Description;
                    }
                }
            }
            result = default;
            return isvalidhit;
        }

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            screenBuffer.Write(_inputBuffer.ToBackward());
            screenBuffer.PushCursor();
            screenBuffer.Write(_inputBuffer.ToForward());
            if (_autoCompleteRunning)
            {
                _index++;
                if (_index > 3)
                {
                    _index = 0;
                }
                screenBuffer.WriteAnswer($" {Twirl[_index]}");
            }
            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }
            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_localpaginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(Messages.AutoCompleteKeyNavigation);
                }
            }
            if (!_autoCompleteRunning && _inputBuffer.ToString().Length >= _options.MinimumPrefixLength)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<string>.Default.Equals(item.Value, selectedItem.Value))
                    {
                        screenBuffer.WriteLineSelector(item.Value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(item.Value);
                    }
                }
                if (_localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
                }
            }
            if (_options.ValidateOnDemand && _options.Validators.Count > 0 && _inputBuffer.Length > 0)
            {
                TryValidate(_inputBuffer.ToString(), _options.Validators, true);
            }
            return _inputBuffer.ToString();
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result)
        {
            _cts.Cancel();
            _autoCompleteRunning = false;
            screenBuffer.WriteDone(_options.Message);
            if (result != null)
            {
                FinishResult = result;
            }
            screenBuffer.WriteAnswer(FinishResult);
        }

        private void ObserverAutoComplete(CancellationToken cancellationToken)
        {
            Task<string[]> _tasksRunning = null;
            Task<ValueDescription<string>[]> _tasksDescriptionRunning = null;
            while (!_ctsObserver.IsCancellationRequested)
            {
                Thread.Sleep(100);
                if (_autoCompleteSendStart)
                {
                    _autoCompleteRunning = true;
                    if (_tasksRunning == null)
                    {
                        using (var lcts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken))
                        {
                            lcts.Token.WaitHandle.WaitOne(_options.CompletionInterval);
                            try
                            {
                                if (!_options.DynamicDescription)
                                {
                                    _tasksRunning = _options.CompletionAsyncService.Invoke(_inputBuffer.ToString(), _options.CompletionMaxCount, lcts.Token);
                                    if (_tasksRunning.IsCompletedSuccessfully)
                                    {
                                        _inputItems.Clear();
                                        foreach (var item in _tasksRunning.Result)
                                        {
                                            _inputItems.Add(new ValueDescription<string>(item, null));
                                        }
                                        _localpaginator = new Paginator<ValueDescription<string>>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<ValueDescription<string>>.s_empty, x => x.Value);
                                        _localpaginator.UnSelected();
                                    }
                                }
                                else
                                {
                                    _tasksDescriptionRunning = _options.CompletionWithDescriptionAsyncService.Invoke(_inputBuffer.ToString(), _options.CompletionMaxCount, lcts.Token);
                                    if (_tasksDescriptionRunning.IsCompletedSuccessfully)
                                    {
                                        _inputItems.Clear();
                                        _inputItems.AddRange(_tasksDescriptionRunning.Result);
                                        _localpaginator = new Paginator<ValueDescription<string>>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<ValueDescription<string>>.s_empty, x => x.Value);
                                        _localpaginator.UnSelected();
                                    }
                                }
                            }
                            catch
                            {
                                _inputItems.Clear();
                                _localpaginator = new Paginator<ValueDescription<string>>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<ValueDescription<string>>.s_empty, x => x.Value);
                            }
                        }
                        _tasksRunning = null;
                        if (!_cts.IsCancellationRequested)
                        {
                            _autoCompleteRunning = false;
                        }
                        _cts = new CancellationTokenSource();
                    }
                    else
                    {
                        _cts.Cancel();
                    }
                }
            }

            if (_tasksRunning != null)
            {
                _tasksRunning.Dispose();
            }
            if (_tasksDescriptionRunning != null)
            {
                _tasksDescriptionRunning.Dispose();
            }
        }

        #region IControlAutoComplete

        public IControlAutoComplete Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlAutoComplete SpeedAnimation(int value)
        {
            _options.SpeedAnimation = value;
            if (_options.SpeedAnimation < 10)
            {
                _options.SpeedAnimation = 10;
            }
            if (_options.SpeedAnimation > 1000)
            {
                _options.SpeedAnimation = 1000;
            }
            return this;
        }

        public IControlAutoComplete CaseInsensitive(bool value)
        {
            _options.CaseInsensitive = value;
            return this;
        }

        public IControlAutoComplete PageSize(int value)
        {
            if (value < 0)
            {
                _options.PageSize = null;
            }
            else
            {
                _options.PageSize = value;
            }
            return this;

        }

        public IControlAutoComplete AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlAutoComplete AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlAutoComplete ValidateOnDemand()
        {
            _options.ValidateOnDemand = true;
            return this;
        }

        public IControlAutoComplete AcceptWithoutMatch()
        {
            _options.AcceptWithoutMatch = true;
            return this;
        }

        public IControlAutoComplete MinimumPrefixLength(int value)
        {
            _options.MinimumPrefixLength = value;
            return this;
        }

        public IControlAutoComplete CompletionInterval(int value)
        {
            _options.CompletionInterval = value;
            return this;
        }

        public IControlAutoComplete CompletionMaxCount(int value)
        {
            _options.CompletionMaxCount = value;
            return this;
        }

        public IControlAutoComplete CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value)
        {
            _options.CompletionWithDescriptionAsyncService = null;
            _options.CompletionAsyncService = value;
            _options.DynamicDescription = false;
            return this;
        }

        public IControlAutoComplete CompletionWithDescriptionAsyncService(Func<string, int, CancellationToken, Task<ValueDescription<string>[]>> value)
        {
            _options.CompletionAsyncService = null;
            _options.CompletionWithDescriptionAsyncService = value;
            _options.DynamicDescription = true;
            return this;
        }

        public IControlAutoComplete Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public new ResultPromptPlus<string> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                var aux = Start(value ?? CancellationToken.None);
                if (PromptPlus.EnabledLogControl)
                {
                    aux.LogControl = Logs;
                }
                PromptPlus.WriteLog(aux.LogControl);
                return aux;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion
    }
}
