// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal abstract class BaseOptions
    {
        public BaseOptions(bool? hideAfterFinish = null)
        {
            if (!hideAfterFinish.HasValue)
            {
                hideAfterFinish = PromptPlus.HideAfterFinish;
            }
            HideAfterFinish = hideAfterFinish.Value;
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _description = value.Trim();
                }
                else
                {
                    _description = value;
                }
            }
        }
        public bool HasDescription => !string.IsNullOrEmpty(_description);
        public string Message { get; set; }
        public bool EnabledAbortKey { get; set; } = PromptPlus.EnabledAbortKey;
        public bool EnabledAbortAllPipes { get; set; } = PromptPlus.EnabledAbortAllPipes;
        public bool EnabledPromptTooltip { get; set; } = PromptPlus.EnabledPromptTooltip;
        public bool HideSymbolPromptAndResult { get; set; } = PromptPlus.HideSymbolPromptAndDone;
        public bool HideAfterFinish { get; set; } = PromptPlus.HideAfterFinish;
        public Dictionary<StageControl, Action<object, string>> UserActions { get; set; } = new();
        public Func<SugestionInput, SugestionOutput> SuggestionHandler { get; set; }
        public bool EnterSuggestionTryFininsh { get; set; } = PromptPlus.EnterSuggestionTryFininsh;
        public bool AcceptInputTab { get; set; } = PromptPlus.AcceptInputTab;
        public object Context { get; set; }

    }
}
