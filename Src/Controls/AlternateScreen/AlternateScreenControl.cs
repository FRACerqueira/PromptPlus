// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PPlus.Controls.AlternateScreen;
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
           
            try
            {
                // Switch to alternate screen
                ConsolePlus.IsControlText = true;
                ConsolePlus.Write("\u001b[?1049h\u001b[H", clearrestofline: false);
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
            }
        }

        public override ResultPrompt<bool> TryResult(CancellationToken cancellationToken)
        {
            return new ResultPrompt<bool>(_resultaction, false);
        }

    }
}
