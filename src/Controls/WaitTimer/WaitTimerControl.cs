// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PromptPlusLibrary.Controls.WaitTimer
{
    internal sealed class WaitTimerControl : BaseControlPrompt<TimeSpan?>, IWaitTimerControl, IDisposable
    {
        private readonly Dictionary<WaitTimerStyles, Style> _optStyles = BaseControlOptions.LoadStyle<WaitTimerStyles>();
        private readonly List<string> _toggerTooptips = [];
        private string? _finish;
        private bool _showElapsedTime;
        private int _intervalShowElapsedTime;
        private Spinners? _spinner;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string? _currentspinnerFrame;
        private readonly TimeSpan _timeoutcount;
        private TimeSpan _currenttimer;
        private TimeSpan _answertimer;
        private Timer? _timer;
        private DateTime _dateTimeOffset = DateTime.Now;
        private bool _hasupdateanswer;
        private bool _isCountDown;
        private bool _disposed;

#pragma warning disable IDE0290 // Use primary constructor
        public WaitTimerControl(TimeSpan timer, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _isCountDown = true;
            _timeoutcount = timer;
            _currenttimer = TimeSpan.Zero;
        }
#pragma warning restore IDE0290 // Use primary constructor

        #region IWaitTimerControl

        public IWaitTimerControl Finish(string text)
        {
            _finish = text;
            return this;
        }

        public IWaitTimerControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IWaitTimerControl IsCountDown(bool value = true)
        {
            _isCountDown = value;
            return this;
        }

        public IWaitTimerControl ShowElapsedTime(int mileseconds = 500, bool value = true)
        {
            if (mileseconds < 100 || mileseconds > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(mileseconds), "Mileseconds must be between 100 and 1000.");
            }
            _showElapsedTime = value;
            _intervalShowElapsedTime = mileseconds;
            return this;
        }

        public IWaitTimerControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        public IWaitTimerControl Styles(WaitTimerStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken _)
        {
            string message = $"{string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip)}, {Messages.TooltipWaitTimer} {_timeoutcount:hh\\:mm\\:ss\\:ff}";
            _tooltipModeInput = message;
            LoadTooltipToggle();
            _dateTimeOffset = DateTime.Now;
            if (_showElapsedTime)
            {
                _timer = new Timer(UpdateElapsedTime, null, 0, _intervalShowElapsedTime);
            }
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);
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
                    ConsoleKeyInfo? keyinfo = WaitKeypressTimer(cancellationToken);

                    if (!keyinfo.HasValue)
                    {
                        ResultCtrl = new ResultPrompt<TimeSpan?>(null, true);
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.None)
                    {
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.Alt)
                    {
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.Control)
                    {
                        ResultCtrl = new ResultPrompt<TimeSpan?>(_currenttimer, false);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo.Value))
                    {
                        ResultCtrl = new ResultPrompt<TimeSpan?>(_currenttimer, true);
                        break;
                    }

                    else if (IsTooltipToggerKeyPress(keyinfo!.Value))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo.Value))
                    {
                        _indexTooptip = 0;
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


        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = ResultCtrl!.Value.IsAborted
                ? GeneralOptions.ShowMesssageAbortKeyValue ? Messages.CanceledKey : string.Empty
                : !string.IsNullOrEmpty(_finish) ? _finish : $"{_answertimer:hh\\:mm\\:ss\\:ff} ";
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitTimerStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[WaitTimerStyles.Answer]);
            return true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                FinalizeControl();
            }
        }

        public override void FinalizeControl()
        {
            if (!_disposed)
            {
                _disposed = true;
                _timer?.Dispose();
            }
        }

        private void UpdateElapsedTime(object? state)
        {
            _answertimer = _isCountDown ? _timeoutcount.Subtract(_currenttimer) : _currenttimer;
            _hasupdateanswer = true;
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
                ];

            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            _toggerTooptips.Clear();
            _toggerTooptips.AddRange(lsttooltips);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitTimerStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_showElapsedTime)
            {
                screenBuffer.Write($"{_answertimer:hh\\:mm\\:ss\\:ff} ", _optStyles[WaitTimerStyles.ElapsedTime]);
            }
            if (_currentspinnerFrame != null)
            {
                screenBuffer.Write(_currentspinnerFrame, _optStyles[WaitTimerStyles.Spinner]);
            }
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[WaitTimerStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.DescriptionValue))
            {
                screenBuffer.WriteLine(GeneralOptions.DescriptionValue, _optStyles[WaitTimerStyles.Description]);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip = _tooltipModeInput;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            screenBuffer.Write(tooltip!, _optStyles[WaitTimerStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private ConsoleKeyInfo? WaitKeypressTimer(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                _currenttimer = DateTime.Now - _dateTimeOffset;
                if (_currenttimer >= _timeoutcount)
                {
                    _currenttimer = _timeoutcount;
                    _answertimer = _currenttimer;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, true);
                }
                else if (_spinner != null && _spinner.HasNextFrame(out string? newframe))
                {
                    _currentspinnerFrame = newframe;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                if (_hasupdateanswer)
                {
                    _hasupdateanswer = false;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, true, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(true) : null;
        }

    }
}
