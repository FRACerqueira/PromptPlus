// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

using PromptPlus.Internal;
using PromptPlus.Options;
using PromptPlus.Resources;
using PromptPlus.ValueObjects;

namespace PromptPlus.Forms
{
    internal class ProgressBarForm : FormBase<ProgressBarInfo>
    {
        private ProgressBarInfo _laststatus;
        private Task<ProgressBarInfo> _localTask;
        private bool _newInteration = true;
        private readonly ProgressBarOptions _options;
        private readonly double _step;

        public ProgressBarForm(ProgressBarOptions options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes, true)
        {
            if (options.UpdateHandler == null)
            {
                throw new ArgumentException(nameof(options.UpdateHandler), Exceptions.Ex_UpdateHandlerProgressBar);
            }

            _options = options;

            _step = double.Parse(_options.Witdth.ToString()) / 100;
            _laststatus = new ProgressBarInfo(0, false, "", _options.InterationId);
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out ProgressBarInfo result)
        {

            if (_laststatus.Finished)
            {
                if (!summary)
                {
                    Thread.Sleep(_options.DoneDelay);
                }
                result = _laststatus;
                return true;
            }
            if (!summary && CheckDefaultKey(GetKeyAvailable(cancellationToken)))
            {
                result = _laststatus;
                return false;
            }
            if (_newInteration)
            {
                _localTask = _options.UpdateHandler.Invoke(_laststatus, cancellationToken);
                _newInteration = false;
            }
            else
            {
                if (!summary)
                {
                    Thread.Sleep(_options.ProcessCheckInterval);
                }
            }

            if (IsEndStaus(_localTask.Status))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _laststatus = new ProgressBarInfo(_laststatus.PercentValue, true, _laststatus.Message, _laststatus.InterationId);
                }
                else
                {
                    _laststatus = _localTask.Result;
                }
                _localTask.Dispose();
                _newInteration = true;
            }

            result = _laststatus;
            return false;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            screenBuffer.WriteAnswer($" {_laststatus.PercentValue}% ");
            if (!string.IsNullOrEmpty(_laststatus.Message))
            {
                screenBuffer.WriteAnswer(_laststatus.Message);
            }

            screenBuffer.PushCursor();
            screenBuffer.ClearRestOfLine();

            var bar = Barlength(_laststatus.PercentValue);
            screenBuffer.WriteLineHint("0% ");
            screenBuffer.WriteSliderOn(bar);
            screenBuffer.WriteSliderOff(_options.Witdth - bar);
            screenBuffer.WriteHint(" 100%");
            if (_options.EnabledPromptTooltip)
            {
                screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, 3);
            }
            screenBuffer.ClearRestOfLine();
        }

        private int Barlength(int value) => (int)(value * _step);

        public override void FinishTemplate(ScreenBuffer screenBuffer, ProgressBarInfo result)
        {
            screenBuffer.WriteDone(_options.Message);
            screenBuffer.WriteAnswer("100%");
        }

#pragma warning disable IDE0066 // Convert switch statement to expression
        private bool IsEndStaus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                case TaskStatus.RanToCompletion:
                    return true;
                default:
                    return false;
            }
        }
#pragma warning restore IDE0066 // Convert switch statement to expression

    }
}
