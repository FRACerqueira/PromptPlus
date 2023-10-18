// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    internal class InputOptions : BaseOptions
    {
        private InputOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("InputOptions CTOR NotImplemented");
        }

        internal InputOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            TimeoutOverwriteDefault = config.HistoryTimeout;
            SwitchView = config.PasswordViewPress;
            SecretChar = config.SecretChar.Value;
            HistoryPageSize = config.PageSize;
            HistoryTimeout = config.HistoryTimeout;
        }

        public FilterMode FilterType { get; set; } = FilterMode.StartsWith;
        public string? OverwriteDefaultFrom { get; set; }
        public TimeSpan TimeoutOverwriteDefault { get; set; }
        public bool EnabledViewSecret { get; set; }
        public HotKey SwitchView { get; set; }
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; }
        public ushort MaxLength { get; set; } = ushort.MaxValue;
        public char SecretChar { get; set; }
        public bool IsSecret { get; set; }
        public string DefaultEmptyValue { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> ChangeDescription { get; set; }
        public int HistoryPageSize { get; set; }
        public int HistoryMinimumPrefixLength { get; set; }
        public byte HistoryMaxItems { get; set; } = byte.MaxValue;
        public TimeSpan HistoryTimeout { get; set; }
        public string? HistoryFileName { get; set; }
        public Func<SuggestionInput, SuggestionOutput>? SuggestionHandler { get; set; }
        public bool ShowingHistory { get; set; }
        public bool HistoryEnabled => !string.IsNullOrEmpty(HistoryFileName);
    }
}
