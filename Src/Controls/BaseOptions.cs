// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    /// <inheritdoc cref="IPromptConfig"/>
    public abstract class BaseOptions : IPromptConfig
    {
        private readonly Dictionary<SymbolType, (string value, string unicode)> _optSymbols = new();
        private readonly IConsoleControl _console;

        private BaseOptions()
        {
            throw new PromptPlusException("BaseOptions CTOR Not Implemented");
        }

        internal BaseOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool? showCursor = true)
        {
            _console = console;
            Config = config;
            OptStyleSchema = StyleSchema.Clone(styleSchema);
            foreach (var item in config._globalSymbols.Keys)
            {
                _optSymbols.Add(item, config._globalSymbols[item]);
            }
            OptShowCursor = showCursor ?? true;
            OptShowTooltip = config.ShowTooltip;
            OptHideAfterFinish = config.HideAfterFinish;
            OptHideOnAbort = config.HideOnAbort;
            OptEnabledAbortKey = config.EnabledAbortKey;
            OptDisableChangeTooltip = config.DisableToggleTooltip;
            OptShowOnlyExistingPagination = config.ShowOnlyExistingPagination;
        }

        internal StyleSchema OptStyleSchema { get; }
        internal ConfigControls Config { get; }
        internal bool OptShowCursor { get; set; } = false;
        internal string OptPrompt { get; set; } = string.Empty;
        internal string OptDescription { get; set; } = string.Empty;
        internal bool OptShowTooltip { get; set; }
        internal bool OptDisableChangeTooltip { get; set; }
        internal bool OptShowOnlyExistingPagination { get; set; }

        internal Dictionary<StageControl, Action<object, object?>> OptUserActions { get; private set; } = new();
        internal string OptToolTip { get; private set; } = string.Empty;
        internal bool OptHideAfterFinish { get; private set; }
        internal bool OptHideOnAbort { get; private set; }
        internal bool OptEnabledAbortKey { get; private set; }
        internal object? OptContext { get; private set; }
        internal bool OptHideAnswer { get; private set; }


        #region IPromptConfig

        /// <inheritdoc/>
        public IPromptConfig EnabledAbortKey(bool value = true)
        {
            OptEnabledAbortKey = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig ShowTooltip(bool value = true)
        {
            OptShowTooltip = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig ShowOnlyExistingPagination(bool value = true)
        {
            OptShowOnlyExistingPagination = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig DisableToggleTooltip(bool value = true)
        {
            OptDisableChangeTooltip = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig HideAfterFinish(bool value = true)
        {
            OptHideAfterFinish = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig HideOnAbort(bool value = true)
        {
            OptHideOnAbort = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig HideAnswer(bool value = true)
        { 
            OptHideAnswer = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig SetContext(object value)
        {
            OptContext = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig AddExtraAction(StageControl stage, Action<object, object?> useraction)
        {
            OptUserActions.Remove(stage);
            OptUserActions.Add(stage, useraction);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Description(StringStyle value)
        {
            OptDescription = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Description, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Prompt(StringStyle value)
        {
            OptPrompt = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Prompt, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Tooltips(StringStyle value)
        {   
            OptToolTip = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Tooltips, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Description(string value)
        {
            OptDescription = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Prompt(string value)
        {
            OptPrompt = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Tooltips(string value)
        {
            OptToolTip = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig ApplyStyle(StyleControls styleControl, Style value)
        {
            OptStyleSchema.ApplyStyle(styleControl, value);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Symbols(SymbolType schema, string value, string? unicode = null)
        {
            _optSymbols[schema] = (value,unicode??value);
            return this;
        }

        internal string Symbol(SymbolType schema)
        {
            if (_console.IsUnicodeSupported)
            {
                return _optSymbols[schema].unicode;
            }
            return _optSymbols[schema].value;
        }

        #endregion
    }
}
