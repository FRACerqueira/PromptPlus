// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.WaitTimer
{
    internal sealed class WaitCommandControl : BaseControlPrompt<Exception?>, IWaitCommandControl, IDisposable
    {
        private readonly Dictionary<WaitCommandStyles, Style> _optStyles = BaseControlOptions.LoadStyle<WaitCommandStyles>();
        private readonly List<string> _toggerTooptips = [];
        private string? _finish;
        private bool _showElapsedTime;
        private int _intervalShowElapsedTime;
        private Spinners? _spinner;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string? _currentspinnerFrame;
        private TimeSpan _currenttimer;
        private TimeSpan _answertimer;
        private Timer? _timer;
        private DateTime _dateTimeOffset = DateTime.Now;
        private bool _hasupdateanswer;
        private Action? _commandHandler; 
        private bool _disposed;
        private Task? _taskCommand;
        private Exception? _exceptionCommand = null!;

#pragma warning disable IDE0290 // Use primary constructor
        public WaitCommandControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _currenttimer = TimeSpan.Zero;
        }
#pragma warning restore IDE0290 // Use primary constructor

        #region IWaitTimerControl


        public IWaitCommandControl CommandHandler(Action commandaction)
        {
            _commandHandler = commandaction;
            return this;
        }

        public IWaitCommandControl Finish(string text)
        {
            _finish = text;
            return this;
        }

        public IWaitCommandControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IWaitCommandControl ShowElapsedTime(int mileseconds = 500, bool value = true)
        {
            if (mileseconds < 100 || mileseconds > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(mileseconds), "Mileseconds must be between 100 and 1000.");
            }
            _showElapsedTime = value;
            _intervalShowElapsedTime = mileseconds;
            return this;
        }

        public IWaitCommandControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        public IWaitCommandControl Styles(WaitCommandStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(_commandHandler);
            string message = $"{string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip)}";
            _tooltipModeInput = message;
            LoadTooltipToggle();
            _dateTimeOffset = DateTime.Now;
            if (_showElapsedTime)
            {
                _timer = new Timer(UpdateElapsedTime, null, 0, _intervalShowElapsedTime);
            }
            _taskCommand = Task.Run(() =>
            {
                try
                {
                    _commandHandler.Invoke();
                }
                catch (Exception ex)
                {
                    _exceptionCommand = ex;
                }
            }, cancellationToken);
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
                        ResultCtrl = new ResultPrompt<Exception?>(_exceptionCommand, true);
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
                        ResultCtrl = new ResultPrompt<Exception?>(_exceptionCommand, false);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo.Value))
                    {
                        ResultCtrl = new ResultPrompt<Exception?>(_exceptionCommand, true);
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
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitCommandStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[WaitCommandStyles.Answer]);
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
            _answertimer = _currenttimer;
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
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitCommandStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_showElapsedTime)
            {
                screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft), _optStyles[WaitCommandStyles.Answer]);
                screenBuffer.Write($"{_answertimer:hh\\:mm\\:ss\\:ff}", _optStyles[WaitCommandStyles.ElapsedTime]);
                screenBuffer.SavePromptCursor();
                screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight), _optStyles[WaitCommandStyles.Answer]);
            }
            if (_currentspinnerFrame != null)
            {
                screenBuffer.Write(_currentspinnerFrame, _optStyles[WaitCommandStyles.Spinner]);
                if (!_showElapsedTime)
                {
                    screenBuffer.SavePromptCursor();
                }
            }
            else
            {
                if (!_showElapsedTime)
                {
                    screenBuffer.SavePromptCursor();
                }
            }
            screenBuffer.WriteLine("", _optStyles[WaitCommandStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.DescriptionValue))
            {
                screenBuffer.WriteLine(GeneralOptions.DescriptionValue, _optStyles[WaitCommandStyles.Description]);
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
            screenBuffer.Write(tooltip!, _optStyles[WaitCommandStyles.Tooltips]);
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
                if (_spinner != null && _spinner.HasNextFrame(out string? newframe))
                {
                    _currentspinnerFrame = newframe;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                if (_hasupdateanswer)
                {
                    _hasupdateanswer = false;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, true, false);
                }
                if (_taskCommand != null)
                {
                    if (_taskCommand.IsCompleted)
                    {
                        _taskCommand.Dispose();
                        _taskCommand = null;
                        _hasupdateanswer = false;
                        return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, true);
                    }
                }
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(true) : null;
        }
    }
}
