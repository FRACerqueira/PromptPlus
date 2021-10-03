// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlus.Options
{
    public abstract class BaseOptions
    {
        public BaseOptions(bool hideAfterFinish = false)
        {
            HideAfterFinish = hideAfterFinish;
        }
        public string Message { get; set; }
        public bool EnabledAbortKey { get; set; } = PPlus.EnabledAbortKey;
        public bool EnabledAbortAllPipes { get; set; } = PPlus.EnabledAbortAllPipes;
        public bool EnabledPromptTooltip { get; set; } = PPlus.EnabledPromptTooltip;
        public bool HideAfterFinish { get; set; }

    }
}
