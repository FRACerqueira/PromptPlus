// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.KeyPress
{
    internal sealed class KeyPressControl : BaseControlPrompt<ConsoleKeyInfo?>, IKeyPressControl
    {
        private readonly Dictionary<KeyPressStyles, Style> _optStyles = BaseControlOptions.LoadStyle<KeyPressStyles>();
        private readonly List<(ConsoleKeyInfo KeyPress, string? Text)> _keyValids = [];
        private readonly List<string> _toggerTooptips = [];
        private Spinners? _spinner;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string? _currentspinnerFrame;
        private bool _showInvalidkey;

#pragma warning disable IDE0290 // Use primary constructor
        public KeyPressControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {

        }
#pragma warning restore IDE0290 // Use primary constructor

        #region IKeyPressControl

        public IKeyPressControl AddKeyValid(ConsoleKey key, ConsoleModifiers? modifiers = null, string? showtext = null)
        {
            modifiers ??= ConsoleModifiers.None;
            _keyValids.Add((new ConsoleKeyInfo((char)key, key, modifiers.Value.HasFlag(ConsoleModifiers.Shift), modifiers.Value.HasFlag(ConsoleModifiers.Alt), modifiers.Value.HasFlag(ConsoleModifiers.Control)), showtext));
            return this;
        }
        public IKeyPressControl Interaction<T>(IEnumerable<T> items, Action<T, IKeyPressControl> interactionaction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionaction);
            foreach (T? item in items)
            {
                interactionaction.Invoke(item, this);
            }
            return this;
        }

        public IKeyPressControl ShowInvalidKey(bool value = true)
        {
            _showInvalidkey = value;
            return this;
        }


        public IKeyPressControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }
        public IKeyPressControl Styles(KeyPressStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }
        public IKeyPressControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken _)
        {
            _tooltipModeInput = string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip);
            LoadTooltipToggle();
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
                    ConsoleKeyInfo? keyinfo = WaitKeypressSpinner(cancellationToken);

                    if (!keyinfo.HasValue)
                    {
                        ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(null, true);
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None)
                    {
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo.Value) && !IsValidKeypress(keyinfo.Value))
                    {
                        ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(keyinfo.Value, true);
                        break;
                    }

                    else if (IsTooltipToggerKeyPress(keyinfo!.Value) && !IsValidKeypress(keyinfo.Value))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo.Value) && !IsValidKeypress(keyinfo.Value))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_keyValids.Count == 0 || IsValidKeypress(keyinfo.Value))
                    {
                        ResultCtrl = new ResultPrompt<ConsoleKeyInfo?>(keyinfo.Value, false);
                        break;
                    }
                    else
                    {
                        SetError(string.Format(Messages.InvalidKey, ConsoleKeyInfoText(keyinfo.Value)));
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
                else
                {
                    answer = string.Empty;
                }
            }
            else
            {
                answer = ValidTextKeypress(ResultCtrl!.Value.Content!.Value);
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[KeyPressStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[KeyPressStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[KeyPressStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_currentspinnerFrame != null)
            {
                screenBuffer.Write(_currentspinnerFrame, _optStyles[KeyPressStyles.Spinner]);
            }
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[KeyPressStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.DescriptionValue))
            {
                screenBuffer.WriteLine(GeneralOptions.DescriptionValue, _optStyles[KeyPressStyles.Description]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (_showInvalidkey && !string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[KeyPressStyles.Error]);
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
            screenBuffer.Write(tooltip!, _optStyles[KeyPressStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
                ];
            if (_keyValids.Count == 0)
            {
                lsttooltips[0] = $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {Messages.AnyKey}";

            }
            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            foreach ((ConsoleKeyInfo KeyPress, string? Text) item in _keyValids)
            {
                if (string.IsNullOrEmpty(item.Text))
                {
                    lsttooltips.Add(string.Format(Messages.ValidAnyKey, ConsoleKeyInfoText(item.KeyPress)));
                }
                else
                {
                    lsttooltips.Add(string.Format(Messages.ValidAnyKey, $"{ConsoleKeyInfoText(item.KeyPress)}={item.Text}"));
                }
            }
            _toggerTooptips.Clear();
            _toggerTooptips.AddRange(lsttooltips);
        }


        private bool IsValidKeypress(ConsoleKeyInfo value)
        {
            if (_keyValids.Count == 0)
            {
                return false;
            }

            return _keyValids.Any(x => x.KeyPress.Key == value.Key && x.KeyPress.Modifiers == value.Modifiers);
        }
        private string ConsoleKeyInfoText(ConsoleKeyInfo value)
        {
            StringBuilder result = new();
            if (value.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                result.Append("Crtl+");
            }
            if (value.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                result.Append("Shift+");
            }
            if (value.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                result.Append("Alt+");
            }
            if (IsPrintable(value.KeyChar))
            {
                result.Append(value.KeyChar);
            }
            else
            {
                result.Append(value.Key.ToString());
            }
            return result.ToString();
        }

        private string ValidTextKeypress(ConsoleKeyInfo value)
        {
            (ConsoleKeyInfo KeyPress, string? Text) aux = _keyValids.First(x => x.KeyPress.Key == value.Key && x.KeyPress.Modifiers == value.Modifiers);
            if (!string.IsNullOrEmpty(aux.Text))
            {
                return aux.Text;
            }
            StringBuilder result = new();
            if (IsPrintable(aux.KeyPress.KeyChar))
            {
                result.Append(ConsoleKeyInfoText(aux.KeyPress));
            }
            else
            {
                result.Append(Messages.PressedKey);
            }
            return result.ToString();
        }


        private readonly UnicodeCategory[] _nonRenderingCategories =
        [
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        ];

        private bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }
            return char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));
        }

        private ConsoleKeyInfo? WaitKeypressSpinner(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_spinner != null && _spinner.HasNextFrame(out string? newframe))
                {
                    _currentspinnerFrame = newframe;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            if (ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                return ConsolePlus.ReadKey(true);
            }
            return null;
        }
    }
}
