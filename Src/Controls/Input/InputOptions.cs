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
        private InputOptions()
        {
            throw new PromptPlusException("InputOptions CTOR NotImplemented");
        }

        internal InputOptions(bool showcursor) : base(showcursor)
        {
        }

        public FilterMode FilterType { get; set; } = FilterMode.StartsWith;
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;
        public bool EnabledViewSecret { get; set; } = false;
        public HotKey SwithView { get; set; } = PromptPlus.Config.PasswordViewPress;
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; } = null;
        public ushort MaxLenght { get; set; } = ushort.MaxValue;
        public char SecretChar { get; set; } = PromptPlus.Config.SecretChar.Value;
        public bool IsSecret { get; set; }
        public string DefaultEmptyValue { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> ChangeDescription { get; set; }
        public int HistoryPageSize { get; set; } = PromptPlus.Config.PageSize;
        public int HistoryMinimumPrefixLength { get; set; } = 0;
        public byte HistoryMaxItems { get; set; } = byte.MaxValue;
        public TimeSpan HistoryTimeout { get; set; } = PromptPlus.Config.HistoryTimeout;
        public string? HistoryFileName { get; set; }
        public Func<SugestionInput, SugestionOutput>? SuggestionHandler { get; set; }
        public bool ShowingHistory { get; set; } = false;
        public bool HistoryEnabled => !string.IsNullOrEmpty(HistoryFileName);

    }
}
