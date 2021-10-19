// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class ProgressBarControl : ControlBase<ProgressBarInfo>, IControlProgressbar
    {
        private ProgressBarInfo _laststatus;
        private Task<ProgressBarInfo> _localTask;
        private bool _newInteration = true;
        private readonly ProgressBarOptions _options;
        private double _step;

        public ProgressBarControl(ProgressBarOptions options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            if (_options.UpdateHandler == null)
            {
                throw new ArgumentException(nameof(_options.UpdateHandler), Exceptions.Ex_UpdateHandlerProgressBar);
            }
            _options.InterationId ??= 0;
            _step = double.Parse(_options.Witdth.ToString()) / 100;
            _laststatus = new ProgressBarInfo(0, false, "", _options.InterationId);
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out ProgressBarInfo result)
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
                if (PipeId != null || AbortedAll)
                {
                    return null;
                }
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
            if (result.Finished)
            {
                screenBuffer.WriteAnswer("100%");
            }
            else
            {
                screenBuffer.WriteAnswer($"{result.PercentValue}% - {Messages.CanceledText}");
            }
        }

        private bool IsEndStaus(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion => true,
                _ => false,
            };
        }


        #region IControlProgressbar

        public IControlProgressbar Prompt(string value)
        {
            _options.Message = value;
            return this;
        }

        public IControlProgressbar UpdateHandler(Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> value)
        {
            _options.UpdateHandler = value;
            return this;
        }

        public IControlProgressbar Width(int value)
        {
            _options.Witdth = value < PromptPlus.ProgressgBarWitdth ? PromptPlus.ProgressgBarWitdth : (value > 100 ? 100 : value);
            return this;
        }

        public IControlProgressbar StartInterationId(object value)
        {
            _options.InterationId = value ?? 0;
            return this;
        }

        public IPromptControls<ProgressBarInfo> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<ProgressBarInfo> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<ProgressBarInfo> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<ProgressBarInfo> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<ProgressBarInfo> Run(CancellationToken? value = null)
        {
            InitControl();
            return Start(value ?? CancellationToken.None);
        }

        public IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
        {
            PipeCondition = condition;
            return this;
        }

        public IFormPlusBase AddPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion

    }
}
