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

        private readonly Dictionary<string, Style> _optStyles = new();

        private readonly IConsoleControl _console;

        private readonly StyleSchema _optStyleSchema;

        private BaseOptions()
        {
            throw new PromptPlusException("BaseOptions CTOR Not Implemented");
        }

        internal BaseOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool? showCursor = true)
        {
            _console = console;
            Config = config;
            _optStyleSchema = StyleSchema.Clone(styleSchema);
            foreach (var item in config._globalSymbols.Keys)
            {
                _optSymbols.Add(item, config._globalSymbols[item]);
            }
            OptShowCursor = showCursor ?? true;
            OptShowTooltip = config.ShowTooltip;
            OptHideAfterFinish = config.HideAfterFinish;
            OptHideOnAbort = config.HideOnAbort;
            OptMinimalRender = config.MinimalRender;
            OptEnabledAbortKey = config.EnabledAbortKey;
            OptDisableChangeTooltip = config.DisableToggleTooltip;
            OptShowOnlyExistingPagination = config.ShowOnlyExistingPagination;
            foreach (var item in Enum.GetNames(typeof(StyleControls)))
            {
                _optStyles.Add(item, _optStyleSchema.GetStyle(Enum.Parse<StyleControls>(item)));
            }
        }

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
        internal bool OptMinimalRender { get; private set; }
        internal Func<int, int, int, string>? OptPaginationTemplate { get; private set; }

        internal void StyleControl(Enum key, Style value)
        {
            if (!_optStyles.TryGetValue(key.ToString(), out _))
            {
                throw new PromptPlusException($"Style({key}) not implemented.");
            }
            _optStyles[key.ToString()] = value;
        }

        internal Style StyleContent(Enum key)
        {
            if (_optStyles.TryGetValue(key.ToString(), out Style result))
            {
                return result;
            }
            throw new PromptPlusException($"Style({key}) not implemented.");
        }

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
        public IPromptConfig MinimalRender(bool value = true)
        { 
            OptMinimalRender = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig PaginationTemplate(Func<int, int, int, string>? value)
        {
            OptPaginationTemplate = value;
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
            _optStyleSchema.ApplyStyle(StyleControls.Description, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Prompt(StringStyle value)
        {
            OptPrompt = value.Text;
            _optStyleSchema.ApplyStyle(StyleControls.Prompt, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Tooltips(StringStyle value)
        {   
            OptToolTip = value.Text;
            _optStyleSchema.ApplyStyle(StyleControls.Tooltips, value.Style);
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
        internal IPromptConfig ApplyStyle(StyleControls styleControl, Style value)
        {
            _optStyleSchema.ApplyStyle(styleControl, value);
            return this;
        }

        /// <inheritdoc/>
        internal IPromptConfig Symbols(SymbolType schema, string value, string? unicode = null)
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
