// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.Internal;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class AutoCompleteControl : ControlBase<string>, IControlAutoComplete, IDisposable
    {
        private const string Twirl = "|/-\\";
        private readonly AutoCompleteOptions _options;
        private readonly InputBuffer _filterBuffer = new();
        private readonly List<string> _inputItems = new();
        private readonly CancellationTokenSource _ctsObserver = new();
        private Paginator<string> _localpaginator;
        private CancellationTokenSource _cts = new();
        private string _prefixstart = string.Empty;
        private int _index = -1;
        private Task _observerAutocomplete;
        private bool _autoCompleteSendStart;
        private bool _autoCompleteRunning;

        public AutoCompleteControl(AutoCompleteOptions options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
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

        public override void InitControl()
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
            if (_options.CompletionAsyncService == null)
            {
                throw new ArgumentNullException(nameof(_options.CompletionAsyncService));
            }
            _localpaginator = new Paginator<string>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<string>.s_empty, x => x);
            _localpaginator.FirstItem();
            _observerAutocomplete = Task.Run(() => ObserverAutoComplete(CancellationToken.None));
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
                        result = default;
                        return false;
                    }
                }
                else
                {
                    _autoCompleteSendStart = false;
                    keyInfo = WaitKeypress(cancellationToken);
                }
                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (_localpaginator.Count > 0)
                {
                    if (IskeyPageNavagator(keyInfo, _localpaginator))
                    {
                        continue;
                    }
                    else if (PromptPlus.UnSelectFilter.Equals(keyInfo))
                    {
                        _localpaginator.UnSelected();
                        result = default;
                        return isvalidhit;
                    }
                }
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        var findok = _localpaginator.TryGetSelectedItem(out result);
                        try
                        {
                            if (!TryValidate(result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            result = default;
                            SetError(ex);
                            return false;
                        }
                        if (findok)
                        {
                            _filterBuffer.Clear();
                            _filterBuffer.Load(result);
                            _prefixstart = result;
                            return true;
                        }
                        result = default;
                        return false;
                    }
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        _autoCompleteSendStart = false;
                        _cts.Cancel();
                        result = _filterBuffer.ToString();
                        try
                        {
                            if (!TryValidate(result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            result = default;
                            SetError(ex);
                            return false;
                        }
                        if (!_options.AcceptWithoutMatch)
                        {
                            var findok = _localpaginator.TryGetSelectedItem(out result);
                            if (findok)
                            {
                                _filterBuffer.Clear();
                                _filterBuffer.Load(result);
                                _prefixstart = result;
                                return true;
                            }
                            result = default;
                            SetError(string.Format(Messages.AutoCompleteKeyNotfound, _filterBuffer.ToString()));
                            return false;
                        }
                        return true;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _filterBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _filterBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _filterBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _filterBuffer.Delete();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control && _filterBuffer.Length > 0:
                        _filterBuffer.Clear();
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _filterBuffer.Insert(keyInfo.KeyChar);
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            if (_prefixstart != _filterBuffer.ToString())
            {
                _prefixstart = _filterBuffer.ToString();
                if (_autoCompleteRunning)
                {
                    _cts.Cancel();
                }
                if (_prefixstart.Length < _options.MinimumPrefixLength)
                {
                    _autoCompleteSendStart = false;
                    _autoCompleteRunning = false;
                    _inputItems.Clear();
                    _localpaginator = new Paginator<string>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<string>.s_empty, x => x);
                    _localpaginator.UnSelected();
                }
                else
                {
                    _autoCompleteSendStart = true;
                    _autoCompleteRunning = true;
                }
            }
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            screenBuffer.Write(_filterBuffer.ToBackward());
            screenBuffer.PushCursor();
            if (_autoCompleteRunning)
            {
                _index++;
                if (_index > 3)
                {
                    _index = 0;
                }
                screenBuffer.WriteAnswer($" {Twirl[_index]}");
            }
            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
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
            if (!_autoCompleteRunning)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<string>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(item);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(item);
                    }
                }
                if (_localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
                }
            }
            if (_options.ValidateOnDemand && _options.Validators.Count > 0 && _filterBuffer.Length > 0)
            {
                TryValidate(_filterBuffer.ToString(), _options.Validators);
            }
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
                                _tasksRunning = _options.CompletionAsyncService.Invoke(_filterBuffer.ToString(), _options.CompletionMaxCount, lcts.Token);
                                if (_tasksRunning.IsCompletedSuccessfully)
                                {
                                    _inputItems.Clear();
                                    _inputItems.AddRange(_tasksRunning.Result);
                                    _localpaginator = new Paginator<string>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<string>.s_empty, x => x);
                                    _localpaginator.UnSelected();
                                }
                            }
                            catch
                            {
                                _inputItems.Clear();
                                _localpaginator = new Paginator<string>(_inputItems, Math.Min(_options.PageSize.Value, _options.CompletionMaxCount), Optional<string>.s_empty, x => x);
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

        }

        #region IControlAutoComplete

        public IControlAutoComplete Prompt(string value)
        {
            _options.Message = value;
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
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlAutoComplete AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
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
            _options.CompletionAsyncService = value;
            return this;
        }

        public IPromptControls<string> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<string> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<string> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<string> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<string> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion
    }
}
