using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class AutoCompleteOptions: BaseOptions
    {
        private AutoCompleteOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("AutoCompleteOptions CTOR NotImplemented");
        }

        internal AutoCompleteOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            PageSize = config.PageSize;
            MinimumPrefixLength = config.CompletionMinimumPrefixLength;
            CompletionWaitToStart = config.CompletionWaitToStart;
            CompletionMaxCount = config.CompletionMaxCount;
            SpinnerStyle = styleSchema.Prompt();
            TimeoutOverwriteDefault = config.HistoryTimeout;
        }

        public int PageSize { get; set; }
        public int MinimumPrefixLength { get; set; }
        public int CompletionWaitToStart { get; set; }
        public int CompletionMaxCount { get; set; }
        public Func<string, int, CancellationToken, Task<string[]>> CompletionAsyncService { get; set; }
        public Spinners Spinner { get; set; } = new Spinners(SpinnersType.Ascii, PromptPlus.IsUnicodeSupported);
        public Style SpinnerStyle { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; }
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; } 
        public ushort MaxLength { get; set; } = ushort.MaxValue;
        public string DefaultEmptyValue { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> ChangeDescription { get; set; }
        public Func<SuggestionInput, SuggestionOutput>? SuggestionHandler { get; set; }
    }
}
