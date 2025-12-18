// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.Switch
{
    internal sealed class SwitchControl : BaseControlPrompt<bool?>, ISwitchControl, ISwitchWidget
    {
        private readonly Dictionary<SwitchStyles, Style> _optStyles = BaseControlOptions.LoadStyle<SwitchStyles>();
        private Func<bool, string>? _changeDescription;
        private bool _defaultValue;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private byte _width;
        private readonly List<string> _toggerTooptips = [];
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private bool _currentValue;
        private string? _onValue;
        private string? _offValue;

        public void InternalDefault(bool value)
        {
            _defaultValue = value;
            _useDefaultHistory = false;
        }
        public void InternalOffValue(string value)
        {
            _offValue = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
        }

        public void InternalOnValue(string value)
        {
            _onValue = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
        }


        public SwitchControl(bool widget, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(widget, console, promptConfig, baseControlOptions)
        {
            _width = ConfigPlus.SwitchWidth;
        }

        #region ISwitchControl

        ISwitchWidget ISwitchWidget.Styles(SwitchStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        ISwitchWidget ISwitchWidget.Width(byte value)
        {
            if (value < 6)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 6");
            }
            _width = value;
            return this;
        }


        public ISwitchControl ChangeDescription(Func<bool, string> value)
        {
            _changeDescription = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            throw new NotImplementedException();
        }

        public ISwitchControl Default(bool value, bool usedefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = usedefaultHistory;
            return this;
        }

        public ISwitchControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            }
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }

        public ISwitchControl OffValue(string value)
        {
            _offValue = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            return this;
        }

        public ISwitchControl OnValue(string value)
        {
            _onValue = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            return this;
        }

        public ISwitchControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public ISwitchControl Styles(SwitchStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public ISwitchControl Width(byte value)
        {
            if (value < 6)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 6");
            }
            _width = value;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_useDefaultHistory && _historyOptions != null)
            {
                string? lasthist = FileHistory
                    .LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue)
                    .Select(x => x.History)
                    .FirstOrDefault();
                if (bool.TryParse(lasthist ?? _defaultValue.ToString(), out bool histbool))
                {
                    _defaultValue = histbool;
                }
            }
            _currentValue = _defaultValue;
            if (!IsWidgetControl)
            {
                LoadTooltipToggle();
                _tooltipModeInput = GetTooltipModeInput();
            }
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.SwitchKeyNavigator);
            tooltip.ToString();
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            _toggerTooptips.Clear();
            if (GeneralOptions.EnabledAbortKeyValue)
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}");
            }
            else
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}");
            }
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidgetControl)
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);

                WriteError(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteSwitch(screenBuffer);

            if (!IsWidgetControl)
            {
                WriteTooltip(screenBuffer);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[SwitchStyles.Error]);
                ClearError();
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
            screenBuffer.Write(tooltip!, _optStyles[SwitchStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private void WriteSwitch(BufferScreen screenBuffer)
        {
            screenBuffer.Write($"{ValueToString(false)} ", _optStyles[SwitchStyles.Ranger]);
            if (_currentValue)
            {
                screenBuffer.Write(new string(' ', _width / 2), _optStyles[SwitchStyles.SliderOn].Background(_optStyles[SwitchStyles.SliderOn].Background));
                screenBuffer.Write(new string(' ', _width / 2), _optStyles[SwitchStyles.SliderOn].Background(_optStyles[SwitchStyles.SliderOn].Foreground));
            }
            else
            {
                screenBuffer.Write(new string(' ', _width / 2), _optStyles[SwitchStyles.SliderOff].Background(_optStyles[SwitchStyles.SliderOff].Foreground));
                screenBuffer.Write(new string(' ', _width / 2), _optStyles[SwitchStyles.SliderOff].Background(_optStyles[SwitchStyles.SliderOff].Background));
            }
            screenBuffer.WriteLine($" {ValueToString(true)}", _optStyles[SwitchStyles.Ranger]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_currentValue) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[SwitchStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            string answer = ValueToString(_currentValue);
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft), _optStyles[SwitchStyles.Answer]);
            screenBuffer.Write(answer, _optStyles[SwitchStyles.Answer]);
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight), _optStyles[SwitchStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[SwitchStyles.Answer]);
        }

        private string ValueToString(bool currentValue)
        {
            return currentValue ? _onValue ?? Messages.OnValue : _offValue ?? Messages.OffValue;
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SwitchStyles.Prompt]);
            }
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
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        ResultCtrl = new ResultPrompt<bool?>(_currentValue, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        ResultCtrl = new ResultPrompt<bool?>(_currentValue, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        ResultCtrl = new ResultPrompt<bool?>(_currentValue, false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    #endregion

                    else if (keyinfo.IsPressLeftArrowKey() || keyinfo.IsPressRightArrowKey())
                    {
                        _currentValue = !_currentValue;
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
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SwitchStyles.Prompt]);
            }
            string answer = ResultCtrl!.Value.IsAborted
                ? GeneralOptions.ShowMesssageAbortKeyValue ? Messages.CanceledKey : string.Empty
                : ValueToString(_currentValue);
            screenBuffer.WriteLine(answer, _optStyles[SwitchStyles.Answer]);

            return true;
        }

        public override void FinalizeControl()
        {
            // none
        }
    }
}

