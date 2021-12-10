// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;


using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class ProgressBarControl : ControlBase<ProgressBarInfo>, IControlProgressbar
    {
        private ProgressBarInfo _laststatus;
        private Task<ProgressBarInfo> _localTask;
        private bool _newInteration = true;
        private readonly ProgressBarOptions _options;
        private double _step;
        private const string Namecontrol = "PromptPlus.ProgressBar";

        public ProgressBarControl(ProgressBarOptions options) : base(Namecontrol, options, false, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            if (_options.UpdateHandler == null)
            {
                throw new ArgumentException(nameof(_options.UpdateHandler), Exceptions.Ex_UpdateHandlerProgressBar);
            }
            _options.InterationId ??= 0;
            _step = double.Parse(_options.Witdth.ToString()) / 100;
            _laststatus = new ProgressBarInfo(0, false, "", _options.InterationId);

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("DoneDelay", _options.DoneDelay.ToString(), LogKind.Property);
                AddLog("ProcessCheckInterval", _options.ProcessCheckInterval.ToString(), LogKind.Property);
                AddLog("Witdth", _options.Witdth.ToString(), LogKind.Property);
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

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

            if (IsEndStatus(_localTask.Status))
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

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
                else
                {
                    screenBuffer.WriteLineDescription("");
                }
                screenBuffer.ClearRestOfLine();
            }

            var bar = Barlength(_laststatus.PercentValue);
            screenBuffer.WriteLineHint("0% ");
            screenBuffer.WriteSliderOn(bar);
            screenBuffer.WriteSliderOff(_options.Witdth - bar);
            screenBuffer.WriteHint(" 100%");
            if (_options.EnabledPromptTooltip)
            {
                screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, !HasDescription, 3);
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

        private static bool IsEndStatus(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion => true,
                _ => false,
            };
        }


        #region IControlProgressbar

        public IControlProgressbar Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlProgressbar UpdateHandler(Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> value)
        {
            _options.UpdateHandler = value;
            return this;
        }

        public IControlProgressbar Width(int value)
        {
            _options.Witdth = value < PromptPlus.ProgressgBarWitdth ? PromptPlus.ProgressgBarWitdth : (value > 200 ? 200 : value);
            return this;
        }

        public IControlProgressbar StartInterationId(object value)
        {
            _options.InterationId = value ?? 0;
            return this;
        }

        public IControlProgressbar Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion

    }
}
