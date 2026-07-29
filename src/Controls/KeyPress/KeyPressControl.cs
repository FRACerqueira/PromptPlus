// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace PromptPlusLibrary.Controls.KeyPress
{

    /// <inheritdoc/>
    internal sealed class KeyPressControl : BaseControlPrompt<ConsoleKeyInfo?>, IKeyPressControl
    {
        private readonly Dictionary<KeyPressStyles, Style> _optStyles;
        private readonly Dictionary<(ConsoleKey Key, ConsoleModifiers Modifiers), string?> _keyValids = [];
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private Func<ConsoleKeyInfo, string>? _message;
        private Func<ConsoleKeyInfo, CancellationToken, Task<string>>? _messageAsync;
        private bool _showInvalidkey;
        private ConsoleKeyInfo? _currentKeyPress;
        private string _currentKeyPressText = string.Empty;

        public KeyPressControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<KeyPressStyles>(console.CurrentStyle);
        }

        #region IKeyPressControl

        /// <inheritdoc/>
        public IKeyPressControl AddValidKey(ConsoleKey key, ConsoleModifiers? requiredModifiers = null, string? displayText = null)
        {
            ConsoleModifiers modifiers = requiredModifiers ?? ConsoleModifiers.None;
            _keyValids[(key, modifiers)] = displayText;
            return this;
        }

        /// <inheritdoc/>
        public IKeyPressControl Options(Action<IControlOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configureOptions);
            configureOptions.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IKeyPressControl ShowMessage(Func<ConsoleKeyInfo, string>? message)
        {
            _message = message;
            _messageAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IKeyPressControl ShowMessageAsync(Func<ConsoleKeyInfo, CancellationToken, Task<string>>? message = null)
        {
            _message = null;
            _messageAsync = message;
            return this;
        }

        /// <inheritdoc/>
        public IKeyPressControl Styles(KeyPressStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        /// <inheritdoc/>
        public override void InitControl(CancellationToken cancellationToken)
        {
            _showInvalidkey = _message != null || _messageAsync != null;
            LoadTooltipToggle();
        }

        /// <inheritdoc/>
        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    _currentKeyPress = null;
                    _currentKeyPressText = string.Empty;
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(default!, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(keyinfo, true);
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
                    else if (_keyValids.Count == 0 || IsValidKeypress(keyinfo, out _currentKeyPressText))
                    {
                        _indexTooptip = 0;
                        _currentKeyPress = keyinfo;
                        ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(keyinfo, false);
                        break;
                    }
                    else
                    {
                        _indexTooptip = 0;
                        if (_showInvalidkey)
                        {
                            var msg = _message?.Invoke(keyinfo) ?? _messageAsync?.Invoke(keyinfo, cancellationToken).GetAwaiter().GetResult();
                            if (msg != null)
                            {
                                SetError(msg);
                            }
                        }
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

        /// <inheritdoc/>
        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[KeyPressStyles.Prompt]);

            WriteAnswer(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);

            WriteError(screenBuffer, _optStyles[KeyPressStyles.Error]);
        }

        /// <inheritdoc/>
        public override void WritePrompt(BufferScreen screenBuffer, Style style)
        {
            if (!string.IsNullOrEmpty(OptionsControl.PromptValue))
            {
                screenBuffer.Write(OptionsControl.PromptValue, style);
                if (OptionsControl.SufixAfterPromptValue is not null)
                {
                    screenBuffer.Write(OptionsControl.SufixAfterPromptValue, style);
                }
            }
            else if (_keyValids.Count == 0)
            {
                screenBuffer.Write(PromptPlusResources.PressAnyKey, style);
                if (OptionsControl.SufixAfterPromptValue is not null)
                {
                    screenBuffer.Write(OptionsControl.SufixAfterPromptValue, style);
                }
            }
        }

        /// <inheritdoc/>
        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = _currentKeyPressText;
            if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                answer = PromptPlusResources.CanceledKey;
            }
            WritePrompt(screenBuffer, _optStyles[KeyPressStyles.Prompt]);
            screenBuffer.WriteLine(answer, _optStyles[KeyPressStyles.Answer]);
            return true;
        }

        /// <inheritdoc/>
        public override void FinalizeControl()
        {
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = GetTooltipToggle();
            var renderTooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!renderTooltip.EndsWith('.'))
            {
                renderTooltip = $"{renderTooltip}.";
            }
            screenBuffer.WriteLine(renderTooltip, _optStyles[KeyPressStyles .Tooltips]);
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            screenBuffer.Write(_currentKeyPressText, _optStyles[KeyPressStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[KeyPressStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(OptionsControl.DescriptionValue))
            {
                screenBuffer.WriteLine(OptionsControl.DescriptionValue, _optStyles[KeyPressStyles.Description]);
            }
        }

        private bool IsValidKeypress(ConsoleKeyInfo value, out string keyPressText)
        {
            if (_keyValids.TryGetValue((value.Key, value.Modifiers), out string? text))
            {
                keyPressText = text ?? string.Empty;
                if (string.IsNullOrEmpty(keyPressText) && IsPrintable(value.KeyChar))
                {
                    keyPressText = value.KeyChar.ToString();
                }
                return true;
            }
            keyPressText = string.Empty;
            return false;
        }

        private static readonly UnicodeCategory[] _nonRenderingCategories =
        [
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        ];

        private static bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            if (char.IsWhiteSpace(c))
            {
                return true;
            }

            UnicodeCategory category = char.GetUnicodeCategory(c);
            return category is not UnicodeCategory.Control
                and not UnicodeCategory.OtherNotAssigned
                and not UnicodeCategory.Surrogate;
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips = [];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

    }
}
