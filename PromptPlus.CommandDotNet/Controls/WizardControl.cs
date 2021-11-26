using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CommandDotNet.Builders;

using PPlus.CommandDotNet.Resources;
using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.CommandDotNet.Controls
{
    internal class WizardControl : ControlBase<IArgumentNode>, IControlWizard
    {
        private readonly WizardOptions _options;
        private bool _isFinish;
        private IArgumentNode _resultControl;

        public WizardControl(WizardOptions options) : base(options, true, false)
        {
            _options = options;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IArgumentNode result)
        {
            screenBuffer.WriteDone(_options.Message);
            if (result != null)
            {
                FinishResult = result.Name;
                screenBuffer.WriteAnswer(FinishResult);
            }
        }

        public override void InitControl()
        {
            if (_options.WizardControl is null)
            {
                throw new ArgumentException(Exceptions.Ex_WizardControl);
            }
            _options.WizardControl.GetType().UnderlyingSystemType.GetProperty("PipeId").SetValue(_options.WizardControl, null);
        }

        public void WizardTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(_options.Message, _options.ForeColor , _options.BackColor );
            if (_options.TokenArgs is not null)
            {
                foreach (var item in _options.TokenArgs)
                {
                    if (!item.IsEnabled)
                    {
                        screenBuffer.Write($" {item.ArgValue}", _options.MissingForeColor, _options.BackColor );
                    }
                    else
                    {
                        if (item.IsSecret)
                        {
                            screenBuffer.Write($" {new string(PromptPlus.PasswordChar, 3)}", _options.ForeColor, _options.BackColor);
                        }
                        else
                        {
                            screenBuffer.Write($" {item.ArgValue}", _options.ForeColor, _options.BackColor);
                        }
                    }
                }
            }
            screenBuffer.Write(" ");
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (!_options.IsRootControl)
            {
                ShowInputTemplate(screenBuffer);
                return;
            }

            var prop = _options.WizardControl.GetType().UnderlyingSystemType
                .GetProperty("Wizardtemplate");

            var action = new Action<ScreenBuffer>((s) => WizardTemplate(s));

            var delegateIntance = Delegate.CreateDelegate(prop.PropertyType, action.Target, action.Method);
            prop.SetValue(_options.WizardControl, delegateIntance);

            _options.WizardControl.GetType().UnderlyingSystemType
                .GetMethod("InitControl").Invoke(_options.WizardControl, null);

            var start = _options.WizardControl.GetType().UnderlyingSystemType
                .GetMethod("Start");

            try
            {
                var ctrl = start.Invoke(_options.WizardControl, new object[]
                {
                    CancellationToken.None
                });
                var isAbort = ((ResultPromptPlus<IArgumentNode>)ctrl).IsAborted;
                _resultControl = null;
                if (!isAbort)
                {
                    _resultControl = ((ResultPromptPlus<IArgumentNode>)ctrl).Value;
                    _isFinish = true;
                }
                else
                {
                    ShowInputTemplate(screenBuffer);
                }
            }
            finally
            {
                var disp = _options.WizardControl.GetType().UnderlyingSystemType
                    .GetMethod("Dispose");
                disp.Invoke(_options.WizardControl, null);
            }
            _options.IsRootControl = false;
        }

        private void ShowInputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(_options.Message,_options.ForeColor, _options.BackColor);
            if (_options.TokenArgs is not null)
            {
                foreach (var item in _options.TokenArgs)
                {
                    if (!item.IsEnabled)
                    {
                        screenBuffer.Write($" {item.ArgValue}", _options.MissingForeColor, _options.BackColor);
                    }
                    else
                    {
                        if (item.IsSecret)
                        {
                            screenBuffer.Write($" {new string(PromptPlus.PasswordChar, 3)}", _options.ForeColor, _options.BackColor);
                        }
                        else
                        {
                            screenBuffer.Write($" {item.ArgValue}", _options.ForeColor, _options.BackColor);
                        }
                    }
                }
            }
            screenBuffer.Write(" ");

            screenBuffer.PushCursor();
            if (HasDescription)
            {
                screenBuffer.WriteLine();
                screenBuffer.WriteAnswer(_options.Description);
            }
            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }
            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteHint($", Enter: Next, {_options.Build} Run");
                }
            }
        }

        public override bool? TryResult(bool IsSummary, CancellationToken stoptoken, out IArgumentNode result)
        {
            bool? isvalidhit = false;
            result = null;
            do
            {
                if (!_options.IsRootControl)
                {
                    if (_isFinish)
                    {
                        result = _resultControl;
                        return true;
                    }
                    var keyInfo = WaitKeypress(stoptoken);
                    if (CheckDefaultWizardKey(keyInfo))
                    {
                        _options.IsRootControl = false;
                        continue;
                    }
                    else if (CheckAbortKey(keyInfo))
                    {
                        return true;
                    }
                    else if (_options.Build.Equals(keyInfo))
                    {
                        if (_options.TokenArgs.Any(t => !t.IsEnabled))
                        {
                            SetError(Resources.Messages.WizardMissingOperands);
                            return false;
                        }
                        result = _resultControl;
                        return true;
                    }
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                            _options.IsRootControl = true;
                            break;
                    }
                }
            } while (KeyAvailable && !stoptoken.IsCancellationRequested);
            return isvalidhit;
        }

        public IControlWizard UpdateTokenArgs(IEnumerable<WizardArgs> args)
        {
            _options.TokenArgs = args;
            return this;
        }

        public IControlWizard Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }


        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<IArgumentNode> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

    }
}
