// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Switch
{
    internal sealed class SwitchControl : BaseControlPrompt<bool?>, ISwitchControl, ISwitchWidget
    {
        private readonly Dictionary<SwitchStyles, Style> _optStyles;
        private Func<bool, string>? _changeDescription;
        private Func<bool, Task<string>>? _changeDescriptionAsync;
        private bool? _defaultValue;
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;
        private bool _currentValue;
        private string _offValue = PromptPlusResources.NoChar;
        private string _onValue = PromptPlusResources.YesChar;
        private string _offAnswer = PromptPlusResources.NoChar;
        private string _onAnswer = PromptPlusResources.YesChar;
        private byte _width;
        private string[] _toggerTooptips = [];
        private int _indexTooptip;

        public SwitchControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<SwitchStyles>(console.CurrentStyle);
            _width = ConfigPrompt.SwitchWidth;
        }

        #region ISwitchControl, ISwitchWidget

        public ISwitchControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        public ISwitchControl Styles(SwitchStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public ISwitchControl Default(bool value, bool useDefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        public ISwitchControl OffValue(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _offValue = value;
            _offAnswer = value;
            return this;
        }

        public ISwitchControl OffValue(EmojiName emojiName, string fallbacktext)
        {
            ArgumentNullException.ThrowIfNull(fallbacktext);
            string emoji = (EmojiValue)emojiName;
            _offValue = string.IsNullOrWhiteSpace(emoji) ? fallbacktext : emoji;
            _offAnswer = fallbacktext;
            return this;
        }

        public ISwitchControl OnValue(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _onValue = value;
            _onAnswer = value;
            return this;
        }

        public ISwitchControl OnValue(EmojiName emojiName, string fallbacktext)
        {
            ArgumentNullException.ThrowIfNull(fallbacktext);
            string emoji = (EmojiValue)emojiName;
            _onValue = string.IsNullOrWhiteSpace(emoji) ? fallbacktext : emoji;
            _onAnswer = fallbacktext;
            return this;
        }

        public ISwitchControl EnableHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public ISwitchControl ChangeDescription(Func<bool, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        public ISwitchControl ChangeDescriptionAsync(Func<bool, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        ISwitchWidget ISwitchWidget.Styles(SwitchStyles styleType, Style style)
        {
            Styles(styleType, style);
            return this;
        }

        /// <inheritdoc/>
        ISwitchWidget ISwitchWidget.OffValue(string value)
        {
            OffValue(value);
            return this;
        }

        /// <inheritdoc/>
        ISwitchWidget ISwitchWidget.OffValue(EmojiName emojiName, string fallbacktext)
        {
            OffValue(emojiName, fallbacktext);
            return this;
        }

        /// <inheritdoc/>
        ISwitchWidget ISwitchWidget.OnValue(string value)
        {
            OnValue(value);
            return this;
        }

        /// <inheritdoc/>
        ISwitchWidget ISwitchWidget.OnValue(EmojiName emojiName, string fallbacktext)
        {
            OnValue(emojiName, fallbacktext);
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (IsWidget)
            {
                _historyOptions = null;
            }

            if (_historyOptions is not null)
            {
                try
                {
                    _itemHistories = [.. FileHistory
                        .LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue)
                        .Where(x => TryDeserializeHistoryValue(x.History, out _))];
                }
                catch
                {
                    _itemHistories = [];
                }

                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out bool historyValue))
                    {
                        _defaultValue = historyValue;
                    }
                }
            }

            _currentValue = _defaultValue ?? false;

            if (_width < 4)
            {
                _width = 4;
            }

            if (!IsWidget)
            {
                LoadTooltipToggle();
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<bool?>(default!, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<bool?>(_currentValue, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<bool?>(_currentValue, false);
                        SaveHistory();
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
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
                    else if (keyinfo.IsPressLeftArrowKey())
                    {
                        _currentValue = false;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressRightArrowKey())
                    {
                        _currentValue = true;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressSpaceKey())
                    {
                        _currentValue = !_currentValue;
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }

            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidget)
            {
                WritePrompt(screenBuffer, _optStyles[SwitchStyles.Prompt]);

                WriteAnswer(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteSwitch(screenBuffer);

            if (!IsWidget)
            {
                WriteTooltip(screenBuffer);

                WriteError(screenBuffer, _optStyles[SwitchStyles.Error]);
            }
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[SwitchStyles.Prompt]);

            string answer = ResultCtrl!.Value.IsAborted
                ? OptionsControl.EnabledAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty
                : AnswerToString(_currentValue);

            screenBuffer.WriteLine(answer, _optStyles[SwitchStyles.Answer]);

            return true;
        }

        public override void FinalizeControl()
        {
            // none
        }

        private string AnswerToString(bool value) => value ? _onAnswer : _offAnswer;

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            string answer = AnswerToString(_currentValue);
            screenBuffer.Write(answer, _optStyles[SwitchStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[SwitchStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(_currentValue)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(_currentValue) ?? OptionsControl.DescriptionValue;
            }

            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[SwitchStyles.Description]);
            }
        }

        private void WriteSwitch(BufferScreen screenBuffer)
        {
            int barWidth = Math.Max(2, (int)_width);
            int thumbIndex = _currentValue ? barWidth - 1 : 0;
            Style stateStyle = _currentValue ? _optStyles[SwitchStyles.SwitchOn] : _optStyles[SwitchStyles.SwitchOff];

            screenBuffer.Write($"{_offValue} ", _optStyles[SwitchStyles.Ranger]);

            for (int i = 0; i < barWidth; i++)
            {
                if (i == thumbIndex)
                {
                    screenBuffer.Write(" ", _optStyles[SwitchStyles.Slider]);
                }
                else
                {
                    screenBuffer.Write(" ", stateStyle);
                }
            }

            screenBuffer.Write($" {_onValue}", _optStyles[SwitchStyles.Ranger]);
            screenBuffer.WriteLine("", _optStyles[SwitchStyles.Prompt]);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }

            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[SwitchStyles.Tooltips]);
        }

        private static string GetTooltipMain()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipSwitch);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                GetTooltipMain()
            ];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private void SaveHistory()
        {
            if (_historyOptions is null)
            {
                return;
            }

            string serializedValue = JsonSerializer.Serialize(_currentValue);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        private static bool TryDeserializeHistoryValue(string value, out bool result)
        {
            try
            {
                result = JsonSerializer.Deserialize<bool>(value);
                return true;
            }
            catch
            {
                return bool.TryParse(value, out result);
            }
        }
    }
}
