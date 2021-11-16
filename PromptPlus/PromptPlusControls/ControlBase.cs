// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

using PromptPlusInternal;

using PromptPlusObjects;

namespace PromptPlusControls
{
    internal abstract class ControlBase<T> : IFormPlusBase
    {
        private const int IdleReadKey = 8;
        private string _finishResult;
        private readonly CancellationTokenSource _esckeyCancelation;
        private readonly ScreenRender _screenrender;
        private readonly bool _showcursor;
        private readonly bool _skiplastrender;
        private readonly BaseOptions _options;
        private bool _toggleSummary = false;

        protected ControlBase(BaseOptions options, bool showcursor, bool skiplastrender = false)
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            _options = options;
            _skiplastrender = skiplastrender;
            _screenrender = new ScreenRender();
            _showcursor = showcursor;
            _esckeyCancelation = new CancellationTokenSource();
        }

        public string PipeId { get; internal set; }

        public string PipeTitle { get; internal set; }

        public object ContextState { get; internal set; }

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
                ScreenRender.HideCursor();
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
                        ScreenRender.ShowCursor();
                        return new ResultPromptPlus<T>(result, false);
                    }
                }
            }
            ScreenRender.ShowCursor();
            return new ResultPromptPlus<T>(default, true);
        }

        public ResultPromptPlus<T> Start(CancellationToken stoptoken)
        {
            _screenrender.StopToken = stoptoken;
            if (!_showcursor)
            {
                ScreenRender.HideCursor();
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
                        if (_options.HideAfterFinish)
                        {
                            _ = _screenrender.HideLastRender();
                        }
                        else
                        {
                            ScreenRender.NewLine();
                        }
                        ScreenRender.ShowCursor();
                        return new ResultPromptPlus<T>(result, false);
                    }
                }
            }
            ScreenRender.ShowCursor();
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

        public static bool IskeyPageNavagator<Tkey>(ConsoleKeyInfo consoleKeyInfo, Paginator<Tkey> paginator)
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
            if (PromptPlus.ToggleVisibleDescription.Equals(keyInfo) && _options.HasDescription)
            {
                HideDescription = !HideDescription;
                return true;
            }
            if (PromptPlus.TooltipKeyPress.Equals(keyInfo))
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
                return true;
            }
            return false;
        }

        public ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken)
        {
            while (!ScreenRender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            return GetKeyAvailable(cancellationToken);
        }

        public ConsoleKeyInfo GetKeyAvailable(CancellationToken cancellationToken)
        {
            if (ScreenRender.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return ScreenRender.KeyPressed;
            }
            return new ConsoleKeyInfo();
        }

        public bool KeyAvailable => ScreenRender.KeyAvailable;

        public bool HasDescription => _options.HasDescription;

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
