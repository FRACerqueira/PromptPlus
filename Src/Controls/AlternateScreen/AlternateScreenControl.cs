// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal class AlternateScreenControl : BaseControl<bool>, IControlAlternateScreen
    {
        private readonly AlternateScreenOtions _options;
        private bool _resultaction = false;

        public AlternateScreenControl(IConsoleControl console, AlternateScreenOtions options) : base(console, options)
        {
            _options = options;
        }

        #region IControlAlternateScreen

        public IControlAlternateScreen CustomAction(Action<CancellationToken> value)
        {
            _options.CustomAction = value;
            return this;
        }

        public IControlAlternateScreen Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlAlternateScreen ForegroundColor(ConsoleColor value)
        {
            _options.ForeColor = value;
            return this;
        }

        public IControlAlternateScreen BackgroundColor(ConsoleColor value)
        {
            _options.BackColor = value;
            return this;
        }

        #endregion

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            //none
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result, bool aborted)
        {
            //none
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.CustomAction == null)
            {
                throw new PromptPlusException("Not have process to run");
            }

            return string.Empty;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (ConsolePlus.IsLegacy || !ConsolePlus.SupportsAnsi)
            {
                _resultaction = false;
            }
            _resultaction = true;

            var curforecolor = ConsolePlus.ForegroundColor;
            var curbackcolor = ConsolePlus.BackgroundColor;

            try
            {

                ConsolePlus.ForegroundColor = _options.ForeColor;
                ConsolePlus.BackgroundColor = _options.BackColor;
                // Switch to alternate screen
                ConsolePlus.IsControlText = true;
                ConsolePlus.Write("\u001b[?1049h", clearrestofline: false);
                ConsolePlus.IsControlText = false;
                ConsolePlus.Clear();
                _options.CustomAction.Invoke(CancellationToken);
            }
            finally
            {
                // Switch back to primary screen
                ConsolePlus.IsControlText = true;
                ConsolePlus.Write("\u001b[?1049l", clearrestofline: false);
                ConsolePlus.IsControlText = false;
                ConsolePlus.ForegroundColor = curforecolor;
                ConsolePlus.BackgroundColor = curbackcolor;
            }
        }

        public override ResultPrompt<bool> TryResult(CancellationToken cancellationToken)
        {
            return new ResultPrompt<bool>(_resultaction, false);
        }

    }
}
