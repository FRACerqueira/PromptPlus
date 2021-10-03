// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

using PromptPlus.ValueObjects;

namespace PromptPlus.Internal
{
    internal abstract class FormBase<T> : IFormPPlusBase
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
        protected FormBase(bool hideafterFinish, bool showcursor, bool enabledAbortEscKey, bool enabledAbortAllPipes, bool skiplastrender = false)
        {
            Thread.CurrentThread.CurrentCulture = PPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PPlus.DefaultCulture;

            _skiplastrender = skiplastrender;
            _screenrender = new ScreenRender();
            _hideAfterFinish = hideafterFinish;
            _showcursor = showcursor;
            _enabledAbortEscKey = enabledAbortEscKey;
            _enabledAbortAllPipes = enabledAbortAllPipes;
            _esckeyCancelation = new CancellationTokenSource();
        }

        private Paginator<T> _paginator;

        public Paginator<T> Paginator
        {
            get { return _paginator; }
            set
            {
                if (_paginator != null)
                {
                    _paginator.Dispose();
                }
                _paginator = value;
            }
        }

        public string PipeId { get; set; }

        public string PipeTitle { get; set; }

        public object ContextState { get; set; }

        public Func<ResultPipe[], object, bool> PipeCondition { get; set; }

        public bool OverPipeLine => !string.IsNullOrEmpty(PipeId);

        public bool SummaryPipeLine { get; set; }

        public bool AbortedAll { get; set; }

        public bool EnabledStandardTooltip { get; set; } = PPlus.EnabledStandardTooltip;

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
            _screenrender.Dispose();
            if (Paginator != null)
            {
                Paginator.Dispose();
            }
            if (_esckeyCancelation != null)
            {
                _esckeyCancelation.Dispose();
            }

            Thread.CurrentThread.CurrentCulture = PPlus.AppCulture;
            Thread.CurrentThread.CurrentUICulture = PPlus.AppCultureUI;

        }

        public ResultPPlus<T> StartPipeline(Action<ScreenBuffer> summarypipeline, Paginator<ResultPPlus<ResultPipe>> pipePaginator, int currentStep, CancellationToken stoptoken)
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
                while (!_linkedCts.IsCancellationRequested)
                {
                    if (hit.HasValue)
                    {
                        if (!_screenrender.HideLastRender())
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
                            hit = false;
                        }
                        else
                        {
                            hit = TryGetResult(false, _linkedCts.Token, out result);
                            if (!hit.HasValue && PPlus.EnabledBeep)
                            {
                                _screenrender.Beep();
                            }
                        }
                        _screenrender.HideCursor();
                        if (_linkedCts.IsCancellationRequested)
                        {
                            if (!_screenrender.HideLastRender())
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
                        return new ResultPPlus<T>(result, false);
                    }
                }
            }
            _screenrender.ShowCursor();
            return new ResultPPlus<T>(default, true);
        }

        public ResultPPlus<T> Start(CancellationToken stoptoken)
        {
            _screenrender.StopToken = stoptoken;
            if (!_showcursor)
            {
                _screenrender.HideCursor();
            }
            using (var _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_esckeyCancelation.Token, stoptoken))
            {
                bool? hit = true;
                while (!_linkedCts.IsCancellationRequested)
                {
                    if (hit.HasValue)
                    {
                        if (!_screenrender.HideLastRender(_skiplastrender))
                        {
                            _linkedCts.Cancel();
                            continue;
                        }
                        _screenrender.InputRender(InputTemplate);
                    }

                    T result = default;
                    if (!_linkedCts.IsCancellationRequested)
                    {
                        if (_showcursor)
                        {
                            _screenrender.ShowCursor();
                        }
                        hit = TryGetResult(false, _linkedCts.Token, out result);
                        if (!hit.HasValue && PPlus.EnabledBeep)
                        {
                            _screenrender.Beep();
                        }
                        _screenrender.HideCursor();
                        if (_linkedCts.IsCancellationRequested)
                        {
                            if (!_screenrender.HideLastRender(_skiplastrender))
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
                        return new ResultPPlus<T>(result, false);
                    }
                }
            }
            _screenrender.ShowCursor();
            return new ResultPPlus<T>(default, true);
        }

        public abstract bool? TryGetResult(bool IsSummary, CancellationToken stoptoken, out T result);

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
            if (PPlus.TooltipKeyPress.Equals(keyInfo))
            {
                EnabledStandardTooltip = !EnabledStandardTooltip;
                return true;
            }
            else if (PPlus.AbortKeyPress.Equals(keyInfo) && _enabledAbortEscKey)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = false;
                return true;
            }
            else if (OverPipeLine && PPlus.AbortAllPipesKeyPress.Equals(keyInfo) && _enabledAbortAllPipes)
            {
                _esckeyCancelation.Cancel();
                AbortedAll = true;
            }
            else if (OverPipeLine && PPlus.ResumePipesKeyPress.Equals(keyInfo))
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

        private void SummaryPipeLineToPrompt(Paginator<ResultPPlus<ResultPipe>> paginator, CancellationToken stoptoken)
        {
            do
            {
                if (!_screenrender.KeyAvailable && !stoptoken.IsCancellationRequested)
                {
                    stoptoken.WaitHandle.WaitOne(IdleReadKey);
                }
                ConsoleKeyInfo keyInfo;
                if (_screenrender.KeyAvailable && !stoptoken.IsCancellationRequested)
                {
                    keyInfo = GetKeyAvailable(stoptoken);
                }
                else
                {
                    continue;
                }
                if (PPlus.ResumePipesKeyPress.Equals(keyInfo))
                {
                    SummaryPipeLine = false;
                    return;
                }
                if (keyInfo.Key == ConsoleKey.PageUp && keyInfo.Modifiers == 0 && paginator.PageCount > 1)
                {
                    paginator.PreviousPage(IndexOption.LastItemWhenHasPages);
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.PageDown && keyInfo.Modifiers == 0 && paginator.PageCount > 1)
                {
                    paginator.NextPage(IndexOption.FirstItemWhenHasPages);
                    return;
                }
                TryGetResult(true, stoptoken, out _);
            } while (_screenrender.KeyAvailable && !stoptoken.IsCancellationRequested);
        }
    }
}
