// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls
{
    internal sealed class BaseControlOptions(PromptConfig promptConfig) : IControlOptions
    {
        private string? _description;
        private string? _prompt;

        public bool _showMesssageAbortKey = promptConfig.ShowMesssageAbortKey;
        private bool _enabledAbortKey = promptConfig.EnabledAbortKey;
        private bool _hideAfterFinish = promptConfig.HideAfterFinish;
        private bool _hideOnAbort = promptConfig.HideOnAbort;
        private bool _showTooltip = promptConfig.ShowTooltip;

        public string? PromptValue => _prompt;

        public string? DescriptionValue => _description;

        public bool EnabledAbortKeyValue => _enabledAbortKey;

        public bool ShowMesssageAbortKeyValue => _showMesssageAbortKey;

        public bool HideAfterFinishValue => _hideAfterFinish;

        public bool HideOnAbortValue => _hideOnAbort;

        public bool ShowTooltipValue => _showTooltip;

        public IControlOptions Prompt(string prompt)
        {
            _prompt = prompt;
            return this;
        }

        public IControlOptions Description(string description)
        {
            _description = description;
            return this;
        }

        public IControlOptions ShowMesssageAbortKey(bool isshow = true)
        {
            _showMesssageAbortKey = isshow;
            return this;
        }

        public IControlOptions EnabledAbortKey(bool isEnabled = true)
        {
            _enabledAbortKey = isEnabled;
            return this;
        }

        public IControlOptions HideAfterFinish(bool shouldHide = true)
        {
            _hideAfterFinish = shouldHide;
            return this;
        }

        public IControlOptions HideOnAbort(bool shouldHide = true)
        {
            _hideOnAbort = shouldHide;
            return this;
        }

        public IControlOptions ShowTooltip(bool isVisible = true)
        {
            _showTooltip = isVisible;
            return this;
        }

        public static Dictionary<TS, Style> LoadStyle<TS>() where TS : Enum
        {
            //load default style from ComponentStyles to control
            Array aux = Enum.GetValues(typeof(TS));
            Dictionary<TS, Style> result = [];
            foreach (object? item in aux)
            {
                ComponentStyles styledefault = Enum.Parse<ComponentStyles>(item.ToString()!);
                result.TryAdd((TS)item, StyleSchema.GetStyle(styledefault));
            }
            return result;
        }
    }
}
