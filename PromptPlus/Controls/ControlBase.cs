// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Inspired by the work https://github.com/shibayan/Sharprompt
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal abstract class ControlBase<T> : IFormPlusBase
    {
        private const int IdleReadKey = 8;
        private string _finishResult;
        private readonly CancellationTokenSource _esckeyCancelation;
        private readonly ScreenRender _screenrender;
        private readonly bool _showcursor;
        private readonly bool _enabledAbortEscKey;
        private readonly bool _enabledAbortAllPipes;
        private readonly bool _hideAfterFinish;
        private readonly bool _skiplastrender;
        private bool _toggleSummary = false;

        protected ControlBase(bool hideafterFinish, bool showcursor, bool enabledAbortEscKey, bool enabledAbortAllPipes, bool skiplastrender = false)
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            _skiplastrender = skiplastrender;
            _screenrender = new ScreenRender();
            _hideAfterFinish = hideafterFinish;
            _showcursor = showcursor;
            _enabledAbortEscKey = enabledAbortEscKey;
            _enabledAbortAllPipes = enabledAbortAllPipes;
            _esckeyCancelation = new CancellationTokenSource();
        }

        public string PipeId { get; internal set; }

        public string PipeTitle { get; internal set; }

        public object ContextState { get; internal set; }

        public Func<ResultPipe[], object, bool> Condition { get; internal set; }

        public bool OverPipeLine => !string.IsNullOrEmpty(PipeId);

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
            }
        }

        public void Dispose()
        {
            if (_esckeyCancelation != null)
            {
                _esckeyCancelation.Dispose();
            }

            Thread.CurrentThread.CurrentCulture = PromptPlus.AppCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.AppCultureUI;
        }

        public ResultPromptPlus<T> StartPipeline(Action<ScreenBuffer> summarypipeline, Paginator<ResultPromptPlus<ResultPipe>> pipePaginator, int currentStep, CancellationToken stoptoken)
        {
            _screenrender.StopToken = stoptoken;
            if (!_showcursor)
            {
                _screenrender.HideCursor();
            }

            pipePaginator.EnsureVisibleIndex(currentStep);

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
                            _screenrender.InputRender(summarypipeline);
                        }
                        else
                        {
                            pipePaginator.EnsureVisibleIndex(currentStep);
                            _screenrender.InputRender(InputTemplate);
                        }
                    }
                    T result = default;
                    if (!_linkedCts.IsCancellationRequested)
                    {
                        if (_showcursor && !SummaryPipeLine)
                        {
                            _screenrender.ShowCursor();
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
                                _screenrender.Beep();
                            }
                            if (!hit.HasValue && _skiplastrender)
                            {
                                hit = false;
                                _toggleSummary = true;
                            }
                        }
                        _screenrender.HideCursor();
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
                        if (_hideAfterFinish)
                        {
                            _ = _screenrender.HideLastRender();
                        }
                        else
                        {
                            _screenrender.NewLine();
                        }
                        _screenrender.ShowCursor();
                        return new ResultPromptPlus<T>(result, false);
                    }
                }
            }
            _screenrender.ShowCursor();
            return new ResultPromptPlus<T>(default, true);
        }

        public ResultPromptPlus<T> Start(CancellationToken stoptoken)
        {
            _screenrender.StopToken = stoptoken;
            if (!_showcursor)
            {
                _screenrender.HideCursor();
            }
            T result = default;
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
                        _screenrender.InputRender(InputTemplate);
                    }

                    result = default;
                    if (!_linkedCts.IsCancellationRequested)
                    {
                        if (_showcursor)
                        {
                            _screenrender.ShowCursor();
                        }
                        hit = TryResult(false, _linkedCts.Token, out result);
                        if (!hit.HasValue && PromptPlus.EnabledBeep)
                        {
                            _screenrender.Beep();
                        }
                        if (!hit.HasValue && _skiplastrender)
                        {
                            hit = false;
                            skip = false;
                        }
                        _screenrender.HideCursor();
                        if (_linkedCts.IsCancellationRequested)
                        {
                            if (!_screenrender.HideLastRender(skip))
                            {
                                _linkedCts.Cancel();
                                continue;
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
                        if (_hideAfterFinish)
                        {
                            _ = _screenrender.HideLastRender();
                        }
                        else
                        {
                            _screenrender.NewLine();
                        }
                        _screenrender.ShowCursor();
                        return new ResultPromptPlus<T>(result, false);
                    }
                }
            }
            _screenrender.ShowCursor();
            return new ResultPromptPlus<T>(result, true);
        }

        public abstract bool? TryResult(bool IsSummary, CancellationToken stoptoken, out T result);

        public abstract void InitControl();

        public abstract void InputTemplate(ScreenBuffer screenBuffer);

        public abstract void FinishTemplate(ScreenBuffer screenBuffer, T result);

        public void SetError(string errorMessage) => _screenrender.ErrorMessage = errorMessage;

        public void SetError(ValidationResult validationResult) => _screenrender.ErrorMessage = validationResult.ErrorMessage;

        public void SetError(Exception exception) => _screenrender.ErrorMessage = exception.Message;

        public bool TryValidate(object input, IList<Func<object, ValidationResult>> validators)
        {
            foreach (var validator in validators)
            {
                var result = validator(input);

                if (result != ValidationResult.Success)
                {
                    SetError(result);
                    return false;
                }
            }
            return true;
        }

        public bool IskeyPageNavagator<Tkey>(ConsoleKeyInfo consoleKeyInfo, Paginator<Tkey> paginator)
        {
            if (consoleKeyInfo.Key == ConsoleKey.PageUp && consoleKeyInfo.Modifiers == 0 && paginator.PageCount > 1)
            {
                paginator.PreviousPage(IndexOption.LastItemWhenHasPages);
                return true;
            }
            else if (consoleKeyInfo.Key == ConsoleKey.PageDown && consoleKeyInfo.Modifiers == 0 && paginator.PageCount > 1)
            {
                paginator.NextPage(IndexOption.FirstItemWhenHasPages);
                return true;
            }
            else if (consoleKeyInfo.Key == ConsoleKey.UpArrow && consoleKeyInfo.Modifiers == 0 && paginator.Count > 0)
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
            else if (consoleKeyInfo.Key == ConsoleKey.DownArrow && consoleKeyInfo.Modifiers == 0 && paginator.Count > 0)
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

        public bool CheckDefaultKey(ConsoleKeyInfo keyInfo)
        {
            if (PromptPlus.TooltipKeyPress.Equals(keyInfo))
            {
                EnabledStandardTooltip = !EnabledStandardTooltip;
                return true;
            }
            else if (PromptPlus.AbortKeyPress.Equals(keyInfo) && _enabledAbortEscKey)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = !OverPipeLine;
                return true;
            }
            else if (OverPipeLine && PromptPlus.AbortAllPipesKeyPress.Equals(keyInfo) && _enabledAbortAllPipes)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = true;
            }
            else if (OverPipeLine && PromptPlus.ResumePipesKeyPress.Equals(keyInfo))
            {
                SummaryPipeLine = !SummaryPipeLine;
                return true;
            }
            return false;
        }

        public ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken)
        {
            while (!_screenrender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            return GetKeyAvailable(cancellationToken);
        }

        public ConsoleKeyInfo GetKeyAvailable(CancellationToken cancellationToken)
        {
            if (_screenrender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return _screenrender.KeyPressed;
            }
            return new ConsoleKeyInfo();
        }

        public bool KeyAvailable => _screenrender.KeyAvailable;

        private void SummaryPipeLineToPrompt(Paginator<ResultPromptPlus<ResultPipe>> paginator, CancellationToken stoptoken)
        {
            do
            {
                if (!_screenrender.KeyAvailable && !stoptoken.IsCancellationRequested)
                {
                    stoptoken.WaitHandle.WaitOne(IdleReadKey);
                }
                var keyInfo = new ConsoleKeyInfo();
                if (_screenrender.KeyAvailable && !stoptoken.IsCancellationRequested)
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
