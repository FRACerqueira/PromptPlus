// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Microsoft.Extensions.Logging;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal abstract class ControlBase<T> : IFormPlusBase
    {
        private const int IdleReadKey = 8;
        private string _finishResult;
        private CancellationTokenSource _esckeyCancelation;
        private readonly ScreenRender _screenrender;
        private readonly bool _showcursor;
        private readonly bool _skiplastrender;
        private readonly BaseOptions _options;
        private bool _toggleSummary = false;

        public readonly ControlLog Logs;


        public readonly CultureInfo AppcurrentCulture;
        public readonly CultureInfo AppcurrentUICulture;

        protected ControlBase(string sourcelog,BaseOptions options, bool showcursor, bool skiplastrender = false)
        {
            AppcurrentCulture = Thread.CurrentThread.CurrentCulture;
            AppcurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            Logs = new ControlLog(sourcelog);
            _options = options;
            _skiplastrender = skiplastrender;
            _screenrender = new ScreenRender();
            _showcursor = showcursor;
        }

        public string PipeId { get; internal set; }

        public string PipeTitle { get; internal set; }

        public object ContextState { get; internal set; }

        public Action<ScreenBuffer> Wizardtemplate { get; set; }

        public Func<ResultPipe[], object, bool> Condition { get; internal set; }

        public bool OverPipeLine => !string.IsNullOrEmpty(PipeId);

        public bool HideDescription { get; set; }

        public bool SummaryPipeLine { get; set; }

        public bool AbortedAll { get; set; }

        public bool EnabledStandardTooltip { get; set; } = PromptPlus.EnabledStandardTooltip;

        public string FinishResult
        {
            get
            {
                return _finishResult ?? "";
            }
            set
            {
                _finishResult = value;
                if (PromptPlus.EnabledLogControl)
                {
                    AddLog("FinishResult",_finishResult,LogKind.Property);
                }

            }
        }

        public void Dispose()
        {
            if (_esckeyCancelation != null)
            {
                _esckeyCancelation.Dispose();
            }
        }

        public void AddLog(string key, string message, LogKind logKind, LogLevel level = LogLevel.Debug)
        {
            Logs.Add(level, key, message, logKind);
        }
       
        public ResultPromptPlus<T> Run(CancellationToken? value)
        {
            try
            {
                PromptPlus.ExclusiveMode = true;

                var initvalue = InitControl();
                if (FindAction(StageControl.OnStartControl, out var useractin))
                {
                    useractin.Invoke(initvalue);
                }
                var aux = Start(value ?? CancellationToken.None);
                if (FindAction(StageControl.OnFinishControl, out var useractout))
                {
                    useractout.Invoke(aux.Value);
                }
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
                PromptPlus.ExclusiveMode = false;
            }
        }

        public ResultPromptPlus<T> StartPipeline(Action<ScreenBuffer> summarypipeline, Paginator<ResultPromptPlus<ResultPipe>> pipePaginator, int currentStep, CancellationToken stoptoken)
        {
            if (PromptPlus.DisabledAllTooltips)
            {
                EnabledStandardTooltip = false;
            }
            try
            {
                PromptPlus.ExclusiveMode = true;
                Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
                Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
                _screenrender.StopToken = stoptoken;
                if (!_showcursor)
                {
                    ScreenRender.HideCursor();
                }
                _esckeyCancelation = new CancellationTokenSource();

                pipePaginator.EnsureVisibleIndex(currentStep);

                if (PromptPlus.EnabledLogControl)
                {
                    AddLog("StartPipeline.Culture", Thread.CurrentThread.CurrentCulture.Name, LogKind.Method);
                }
                using (var _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_esckeyCancelation.Token, stoptoken))
                {
                    bool? hit = true;
                    var fistskip = true;
                    while (!_linkedCts.IsCancellationRequested)
                    {
                        var skip = _skiplastrender;
                        if (SummaryPipeLine && fistskip)
                        {
                            skip = false;
                            fistskip = false;
                        }
                        if (_toggleSummary)
                        {
                            skip = false;
                            _toggleSummary = false;
                        }
                        if (hit.HasValue)
                        {
                            if (!_screenrender.HideLastRender(skip))
                            {
                                _linkedCts.Cancel();
                                continue;
                            }
                            if (SummaryPipeLine)
                            {
                                _screenrender.ClearBuffer();
                                _screenrender.InputRender(summarypipeline);
                            }
                            else
                            {
                                pipePaginator.EnsureVisibleIndex(currentStep);
                                _screenrender.ClearBuffer();
                                _screenrender.InputRender(InputTemplate);
                                if (FindAction(StageControl.OnInputRender, out var useract))
                                {
                                    useract.Invoke(null);
                                }
                            }
                        }
                        T result = default;
                        if (!_linkedCts.IsCancellationRequested)
                        {
                            if (_showcursor && !SummaryPipeLine)
                            {
                                ScreenRender.ShowCursor();
                            }
                            if (SummaryPipeLine)
                            {
                                SummaryPipeLineToPrompt(pipePaginator, _linkedCts.Token);
                                _toggleSummary = true;
                                hit = false;
                            }
                            else
                            {
                                hit = TryResult(false, _linkedCts.Token, out result);
                                if (!hit.HasValue && PromptPlus.EnabledBeep && PipeId == null)
                                {
                                    ScreenRender.Beep();
                                }
                                if (!hit.HasValue && _skiplastrender)
                                {
                                    hit = false;
                                    _toggleSummary = true;
                                }
                            }
                            ScreenRender.HideCursor();
                            if (_linkedCts.IsCancellationRequested && !_esckeyCancelation.IsCancellationRequested)
                            {
                                if (!_screenrender.HideLastRender())
                                {
                                    _linkedCts.Cancel();
                                    continue;
                                }
                                continue;
                            }
                        }

                        if (_esckeyCancelation.IsCancellationRequested || (hit.HasValue && hit.Value))
                        {
                            if (!_screenrender.HideLastRender())
                            {
                                _linkedCts.Cancel();
                                continue;
                            }
                            _screenrender.FinishRender(FinishTemplate, result);
                            if (_options.HideAfterFinish)
                            {
                                _ = _screenrender.HideLastRender();
                            }
                            else
                            {
                                ScreenRender.NewLine();
                            }
                            if (PromptPlus.EnabledLogControl)
                            {
                                if (_esckeyCancelation.IsCancellationRequested)
                                {
                                    AddLog("EscCancelation", "true", LogKind.Abort);
                                }
                                else if (stoptoken.IsCancellationRequested)
                                {
                                    AddLog("MainCancelation", "true", LogKind.Abort);
                                }
                            }
                            ScreenRender.ShowCursor();
                            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
                            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;
                            return new ResultPromptPlus<T>(result, false);
                        }
                    }
                }

                if (PromptPlus.EnabledLogControl)
                {
                    if (_esckeyCancelation.IsCancellationRequested)
                    {
                        AddLog("EscCancelation", "true", LogKind.Abort);
                    }
                    else if (stoptoken.IsCancellationRequested)
                    {
                        AddLog("MainCancelation", "true", LogKind.Abort);
                    }
                }
                ScreenRender.ShowCursor();
                Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
                Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;
                return new ResultPromptPlus<T>(default, true);
            }
            finally 
            {
                PromptPlus.ExclusiveMode = false;
            }
        }

        public ResultPromptPlus<T> Start(CancellationToken stoptoken)
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
            if (PromptPlus.DisabledAllTooltips)
            {
                EnabledStandardTooltip = false;
            }

            _screenrender.StopToken = stoptoken;
            if (!_showcursor)
            {
                ScreenRender.HideCursor();
            }

            T result = default;
            _esckeyCancelation = new CancellationTokenSource();

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("Start.Culture", Thread.CurrentThread.CurrentCulture.Name, LogKind.Method);
            }

            using (var _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_esckeyCancelation.Token, stoptoken))
            {
                bool? hit = true;
                var skip = _skiplastrender;
                while (!_linkedCts.IsCancellationRequested)
                {
                    if (hit.HasValue)
                    {
                        if (!_screenrender.HideLastRender(skip))
                        {
                            _linkedCts.Cancel();
                            continue;
                        }
                        _screenrender.ClearBuffer();
                        if (Wizardtemplate is not null)
                        {
                            _screenrender.InputRender(Wizardtemplate,true);
                            PromptPlus.IsRunningWithCommandDotNet = true;
                        }
                        _screenrender.InputRender(InputTemplate);
                        if (FindAction(StageControl.OnInputRender, out var useractin))
                        {
                            useractin.Invoke(null);
                        }
                        PromptPlus.IsRunningWithCommandDotNet = false;
                    }
                    result = default;
                    if (!_linkedCts.IsCancellationRequested)
                    {
                        if (_showcursor)
                        {
                            ScreenRender.ShowCursor();
                        }
                        hit = TryResult(false, _linkedCts.Token, out result);
                        if (!hit.HasValue && PromptPlus.EnabledBeep)
                        {
                            ScreenRender.Beep();
                        }
                        if (!hit.HasValue && _skiplastrender)
                        {
                            hit = false;
                            skip = false;
                        }
                        ScreenRender.HideCursor();
                        if (_linkedCts.IsCancellationRequested)
                        {
                            if (!_screenrender.HideLastRender(skip))
                            {
                                _linkedCts.Cancel();
                            }
                            continue;
                        }
                    }

                    if (!_linkedCts.IsCancellationRequested && hit.HasValue && hit.Value)
                    {
                        if (!_screenrender.HideLastRender())
                        {
                            _linkedCts.Cancel();
                            continue;
                        }
                        _screenrender.FinishRender(FinishTemplate, result);
                        if (_options.HideAfterFinish)
                        {
                            _ = _screenrender.HideLastRender();
                        }
                        else
                        {
                            ScreenRender.NewLine();
                        }
                        if (PromptPlus.EnabledLogControl)
                        {
                            if (_esckeyCancelation.IsCancellationRequested)
                            {
                                AddLog("EscCancelation", "true", LogKind.Abort);
                            }
                            else if (stoptoken.IsCancellationRequested)
                            {
                                AddLog("MainCancelation", "true", LogKind.Abort);
                            }
                        }
                        ScreenRender.ShowCursor();
                        if (result == null)
                        {
                            return new ResultPromptPlus<T>(result, true,true);
                        }
                        return new ResultPromptPlus<T>(result, false);
                    }
                }
            }

            if (PromptPlus.EnabledLogControl)
            {
                if (_esckeyCancelation.IsCancellationRequested)
                {
                    AddLog("EscCancelation", "true", LogKind.Abort);
                }
                else if (stoptoken.IsCancellationRequested)
                {
                    AddLog("MainCancelation", "true", LogKind.Abort);
                }
            }

            ScreenRender.ShowCursor();
            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

            return new ResultPromptPlus<T>(result, true);
        }

        public abstract bool? TryResult(bool IsSummary, CancellationToken stoptoken, out T result);

        public abstract T InitControl();

        public abstract void InputTemplate(ScreenBuffer screenBuffer);

        public abstract void FinishTemplate(ScreenBuffer screenBuffer, T result);

        public void ClearError()
        {
            _screenrender.ErrorMessage = null;
        }

        public void SetError(string errorMessage)
        {
            _screenrender.ErrorMessage = errorMessage;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("SetError", errorMessage, LogKind.MsgErro);
            }
        }

        private void SetError(ValidationResult validationResult)
        {
            _screenrender.ErrorMessage = validationResult.ErrorMessage;
        }

        public void SetError(Exception exception)
        {
            _screenrender.ErrorMessage = exception.Message;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("SetError", exception.Message, LogKind.MsgErro);
            }
        }

        public bool TryValidate(object input, IList<Func<object, ValidationResult>> validators, bool ondemand)
        {
            foreach (var validator in validators)
            {
                var result = validator(input);

                if (result != ValidationResult.Success)
                {
                    SetError(result);
                    if (!ondemand && PromptPlus.EnabledLogControl)
                    {
                        AddLog("SetError", result.ErrorMessage, LogKind.MsgErro);
                    }
                    return false;
                }
                if (PromptPlus.EnabledLogControl)
                {
                    foreach (var item in validator.GetInvocationList())
                    {
                        var match = nameblockRegEx.Value.Match(item.Method.Name);
                        if (match.Captures.Count == 1)
                        {
                            AddLog("ValidationResult", match.Groups["name"].Value, LogKind.Validator);
                        }

                    }
                }
            }
            return true;
        }

        private static readonly Lazy<Regex> nameblockRegEx = new(
            () => new Regex("\\<(?<name>.*?)\\>", RegexOptions.IgnoreCase),
            isThreadSafe: true);

        public static string CreateMessageHitSugestion(bool tryfinish,string entertext)
        {
            var msg = new StringBuilder();
            msg.Append(Messages.ReadlineSugestionhit);
            msg.Append(", ");
            msg.Append(Messages.ReadlineSugestionMode);
            if (!tryfinish)
            {
                msg.Append(", ");
                msg.Append(Messages.EnterAcceptSugestion);
            }
            else
            {
                msg.Append(", ");
                msg.Append(entertext);
            }
            return msg.ToString();
        }

        public bool IskeyPageNavagator<Tkey>(ConsoleKeyInfo consoleKeyInfo, Paginator<Tkey> paginator)
        {
            if (consoleKeyInfo.IsPressPageUpKey() && paginator.PageCount > 1)
            {
                paginator.PreviousPage(IndexOption.LastItemWhenHasPages);
                return true;
            }
            else if (consoleKeyInfo.IsPressPageDownKey() && paginator.PageCount > 1) 
            {
                paginator.NextPage(IndexOption.FirstItemWhenHasPages);
                return true;
            }
            else if (consoleKeyInfo.IsPressUpArrowKey() && paginator.Count > 0)
            {
                if (paginator.IsFistPageItem)
                {
                    paginator.PreviousPage(IndexOption.LastItem);
                }
                else
                {
                    paginator.PreviousItem();
                }
                return true;
            }
            else if (consoleKeyInfo.IsPressDownArrowKey()&& paginator.Count > 0)
            {
                if (paginator.IsLastPageItem)
                {
                    paginator.NextPage(IndexOption.FirstItem);
                }
                else
                {
                    paginator.NextItem();
                }
                return true;
            }
            return false;
        }

        public bool CheckDefaultWizardKey(ConsoleKeyInfo keyInfo)
        {
            if (PromptPlus.ToggleVisibleDescription.Equals(keyInfo) && _options.HasDescription)
            {
                HideDescription = !HideDescription;
                return true;
            }
            else if (PromptPlus.TooltipKeyPress.Equals(keyInfo))
            {
                EnabledStandardTooltip = !EnabledStandardTooltip;
                return true;
            }
            return false;
        }

        public bool CheckAbortKey(ConsoleKeyInfo keyInfo)
        {
            if (PromptPlus.AbortKeyPress.Equals(keyInfo) && _options.EnabledAbortKey)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = !OverPipeLine;
                return true;
            }
            return false;
        }

        public bool CheckDefaultKey(ConsoleKeyInfo keyInfo)
        {
            if (PromptPlus.DisabledAllTooltips)
            {
                EnabledStandardTooltip = false;
            }
            if (PromptPlus.ToggleVisibleDescription.Equals(keyInfo) && _options.HasDescription)
            {
                HideDescription = !HideDescription;
                return true;
            }
            if (PromptPlus.TooltipKeyPress.Equals(keyInfo) && !PromptPlus.DisabledAllTooltips)
            {
                EnabledStandardTooltip = !EnabledStandardTooltip;
                return true;
            }
            else if (PromptPlus.AbortKeyPress.Equals(keyInfo) && _options.EnabledAbortKey)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = !OverPipeLine;
                return true;
            }
            else if (OverPipeLine && PromptPlus.AbortAllPipesKeyPress.Equals(keyInfo) && _options.EnabledAbortAllPipes)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = true;
            }
            else if (OverPipeLine && PromptPlus.ResumePipesKeyPress.Equals(keyInfo))
            {
                SummaryPipeLine = !SummaryPipeLine;
            }
            return false;
        }

        public static ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken)
        {
            while (!ScreenRender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            return GetKeyAvailable(cancellationToken);
        }

        public static ConsoleKeyInfo GetKeyAvailable(CancellationToken cancellationToken)
        {
            if (ScreenRender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return ScreenRender.KeyPressed;
            }
            return new ConsoleKeyInfo();
        }

        public static bool KeyAvailable => ScreenRender.KeyAvailable;

        public bool HasDescription => _options.HasDescription;

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("EnabledAbortKey", value.ToString(), LogKind.Property);
            }
            return this;
        }

        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("EnabledAbortAllPipes", value.ToString(), LogKind.Property);
            }
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("EnabledPromptTooltip", value.ToString(), LogKind.Property);
            }
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            if (PromptPlus.EnabledLogControl)
            {
                AddLog("HideAfterFinish", value.ToString(), LogKind.Property);
            }
            return this;
        }

        public IPromptConfig AcceptInputTab(bool value)
        {
            if (_options.SuggestionHandler != null)
            {
                _options.AcceptInputTab = false;
            }
            else
            {
                _options.AcceptInputTab = value;
            }
            AddLog("AcceptInputTab", value.ToString(), LogKind.Property);
            return this;
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

        public IFormPlusBase AddExtraAction(StageControl stage, Action<object> useraction)
        { 
            if (useraction is null)
            {
                throw new ArgumentException(nameof(useraction));
            }
            if (_options.UserActions.ContainsKey(stage))
            {
                _options.UserActions.Remove(stage);
            }
            _options.UserActions.Add(stage, useraction);
            return this;
        }

        private bool FindAction(StageControl value, out Action<object> action)
        {
            action = null;
            if (_options.UserActions.ContainsKey(value))
            {
                action = _options.UserActions[value];
            }
            return action != null;
        }

        private void SummaryPipeLineToPrompt(Paginator<ResultPromptPlus<ResultPipe>> paginator, CancellationToken stoptoken)
        {
            do
            {
                if (!ScreenRender.KeyAvailable && !stoptoken.IsCancellationRequested)
                {
                    stoptoken.WaitHandle.WaitOne(IdleReadKey);
                }
                var keyInfo = new ConsoleKeyInfo();
                if (ScreenRender.KeyAvailable && !stoptoken.IsCancellationRequested)
                {
                    keyInfo = GetKeyAvailable(stoptoken);
                }
                if (PromptPlus.ResumePipesKeyPress.Equals(keyInfo))
                {
                    SummaryPipeLine = false;
                    continue;
                }
                if (keyInfo.Key == ConsoleKey.PageUp && keyInfo.Modifiers == 0 && paginator.PageCount > 1)
                {
                    paginator.PreviousPage(IndexOption.LastItemWhenHasPages);
                    continue;
                }
                else if (keyInfo.Key == ConsoleKey.PageDown && keyInfo.Modifiers == 0 && paginator.PageCount > 1)
                {
                    paginator.NextPage(IndexOption.FirstItemWhenHasPages);
                    continue;
                }
                var result = TryResult(true, stoptoken, out _);
                if (result.HasValue && result.Value)
                {
                    SummaryPipeLine = false;
                }
            } while (SummaryPipeLine && !stoptoken.IsCancellationRequested);
            _toggleSummary = true;
        }
    }
}
