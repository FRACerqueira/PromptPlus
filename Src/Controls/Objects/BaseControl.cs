// ***************************************************************************************
// MIT LICENCE
// Copyright (c) 2019 shibayan.
// https://github.com/shibayan/Sharprompt
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;

namespace PPlus.Controls.Objects
{
    internal abstract class BaseControl<T> : IBaseControl
    {
        private string _finishResult;
        private readonly BaseOptions _options;
        private string _errorMessage;
        private readonly ScreenBuffer _screenBuffer;
        private InforRender? _lastBufferLines;
        private int? _maxlastline;
        public CancellationToken CancellationToken { get; private set; }
        public readonly CultureInfo AppcurrentCulture;

        protected BaseControl(IConsoleControl console, BaseOptions options)
        {
            _options = options;
            AppcurrentCulture = Thread.CurrentThread.CurrentCulture;
            if (options.Config.AppCulture != AppcurrentCulture)
            {
                options.Config.AppCulture = AppcurrentCulture;
            }
            _errorMessage = string.Empty;
            _screenBuffer = new();
            _lastBufferLines = null;
            EnabledTooltip = options.OptShowTooltip;
            ConsolePlus = console;
            _maxlastline = null;
        }

        public IConsoleControl ConsolePlus { get; }

        public bool EnabledTooltip { get; set; }

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

        public ResultPrompt<T> Run(CancellationToken? stoptoken)
        {
            stoptoken ??= CancellationToken.None;
            if (ConsolePlus.Provider != "Memory")
            {
                while (KeyAvailable)
                {
                    WaitKeypress(CancellationToken.None);
                }
            }
            ResultPrompt<T> result;
            var oldcursor = ConsolePlus.CursorVisible;
            try
            {
                Thread.CurrentThread.CurrentCulture = _options.Config.DefaultCulture;
                Thread.CurrentThread.CurrentUICulture = _options.Config.DefaultCulture;

                var initvalue = InitControl(stoptoken.Value);

                if (FindAction(StageControl.OnStartControl, out var useractin))
                {
                    useractin.Invoke(_options.OptContext, initvalue);
                }

                result = Start(stoptoken.Value);
                if (FindAction(StageControl.OnFinishControl, out var useractout))
                {
                    useractout.Invoke(_options.OptContext, FinishResult);
                }
                FinalizeControl(stoptoken.Value);
            }
            catch (Exception ex)
            {
                throw new PromptPlusException(ex.Message,ex);
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
                Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            }
            return result;
        }

        private void RenderBuffer()
        {
            if (_screenBuffer.Buffer.Length == 0)
            { 
                return;
            }
            int inittop = ConsolePlus.CursorTop;
            (int left, int top)? cursor = null;
            var lines = 0;
            foreach (var item in _screenBuffer.Buffer)
            {
                if (item.SaveCursor)
                {
                    cursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                }
                else
                {
                    var qtd = 0;
                    var postop = ConsolePlus.CursorTop;
                    ConsolePlus.IsControlText = item.SkipMarkup;
                    qtd = ConsolePlus.Write(item.Text, item.Style, item.ClearRestOfLine);
                    ConsolePlus.IsControlText = false;
                    if (ConsolePlus.IsTerminal && postop + qtd > ConsolePlus.BufferHeight)
                    {
                        var dif = postop + qtd - ConsolePlus.BufferHeight;
                        inittop -= dif;
                        if (cursor != null)
                        {
                            cursor = (cursor.Value.left, cursor.Value.top - dif);
                        }
                    }
                    lines += qtd;
                }
            }
            if (!_maxlastline.HasValue)
            { 
                _maxlastline = lines;
            }
            if (_maxlastline.Value < lines)
            {
                _maxlastline = lines;
            }
            else if (_maxlastline.Value > lines)
            {
                var cleartop = ConsolePlus.CursorTop;
                for (int i = 0; i < _maxlastline.Value-lines; i++)
                {
                    ConsolePlus.SetCursorPosition(0, cleartop + i + 1);
                    ConsolePlus.Write(" ", null, true);
                }
                ConsolePlus.SetCursorPosition(0, cleartop);
            }
            _lastBufferLines = new InforRender(
                inittop,
                _maxlastline.Value);
            if (cursor.HasValue)
            {
                ConsolePlus.SetCursorPosition(cursor.Value.left, cursor.Value.top);
            }
            _screenBuffer.Clear();
        }

        private void ClearLastRender(bool clearall)
        {
            if (_lastBufferLines.HasValue)
            {
                ConsolePlus.SetCursorPosition(0, _lastBufferLines.Value.CursorTop);
                if (clearall)
                {
                    for (int i = 0; i < _lastBufferLines.Value.QtdLines; i++)
                    {
                        ConsolePlus.SetCursorPosition(0, _lastBufferLines.Value.CursorTop+i);
                        ConsolePlus.Write("", null, true);
                    }
                }
                ConsolePlus.SetCursorPosition(0, _lastBufferLines.Value.CursorTop);
                _lastBufferLines = null;
            }
        }

        public void ClearBuffer()
        {
            while (KeyAvailable)
            {
                WaitKeypress(CancellationToken.None);
            }
        }

        public ResultPrompt<T> Start(CancellationToken stoptoken)
        {
            ConsolePlus.CursorVisible = false;
            var result = new ResultPrompt<T>(default, true);
            bool notrender = false;
            while (!stoptoken.IsCancellationRequested)
            {
                if (!notrender)
                {
                    ClearLastRender(result.ClearLastRender);
                    InputTemplate(_screenBuffer);
                    RenderBuffer();
                    if (FindAction(StageControl.OnInputRender, out var useractin))
                    {
                        useractin.Invoke(_options.OptContext, result);
                    }
                }
                if (_options.OptShowCursor)
                {
                    ConsolePlus.CursorVisible = true;
                }
                result = TryResult(stoptoken);
                if (FindAction(StageControl.OnTryAcceptInput, out var useracttryin))
                {
                    useracttryin.Invoke(_options.OptContext, result);
                }
                notrender = result.NotRender;
                ConsolePlus.CursorVisible = false;
                if (!result.IsRunning)
                {
                    ClearLastRender(true);
                    FinishTemplate(_screenBuffer, result.Value, result.IsAborted);
                    RenderBuffer();
                    if (FindAction(StageControl.OnFinishControl, out var useractend))
                    {
                        useractend.Invoke(_options.OptContext, result);
                    }
                    if (_options.OptHideAfterFinish || (result.IsAborted && _options.OptHideOnAbort))
                    {
                        ClearLastRender(true);
                    }
                    break;
                }
            }
            return result;
        }

        public abstract ResultPrompt<T> TryResult(CancellationToken cancellationToken);

        public abstract string InitControl(CancellationToken cancellationToken);

        public abstract void InputTemplate(ScreenBuffer screenBuffer);

        public abstract void FinishTemplate(ScreenBuffer screenBuffer, T result, bool aborted);

        public abstract void FinalizeControl(CancellationToken cancellationToken);

        public void ClearError()
        {
            _errorMessage = null;
        }

        public void SetError(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        private void SetError(ValidationResult validationResult)
        {
            _errorMessage = validationResult.ErrorMessage;
        }

        public string ValidateError => _errorMessage;

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

        public static bool IskeyPageNavegator<Tkey>(ConsoleKeyInfo consoleKeyInfo, Paginator<Tkey> paginator)
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
            else if (consoleKeyInfo.IsPressDownArrowKey() && paginator.Count > 0)
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

        public bool CheckTooltipKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (_options.Config.TooltipKeyPress.Equals(keyInfo))
            {
                if (!_options.OptDisableChangeTooltip)
                {
                    _options.OptShowTooltip = !_options.OptShowTooltip;
                }
                return true;
            }
            return false;
        }

        public bool CheckAbortKey(ConsoleKeyInfo keyInfo)
        {
            if (_options.OptEnabledAbortKey && _options.Config.AbortKeyPress.Equals(keyInfo))
            {
                return true;
            }
            return false;
        }

        public ConsoleKeyInfo? WaitKeypress(CancellationToken cancellationToken)
        {
            return ConsolePlus.WaitKeypress(true, cancellationToken);
        }

        public bool KeyAvailable => ConsolePlus.KeyAvailable;

        public bool FindAction(StageControl value, out Action<object, object> action)
        {
            _options.OptUserActions.TryGetValue(value, out action);
            return action != null;
        }
    }
}
