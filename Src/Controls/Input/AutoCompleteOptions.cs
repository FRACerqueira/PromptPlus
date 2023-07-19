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
        private AutoCompleteOptions()
        {
            throw new PromptPlusException("AutoCompleteOptions CTOR NotImplemented");
        }

        internal AutoCompleteOptions(bool showcursor) : base(showcursor)
        {
        }

        public int PageSize { get; set; } = PromptPlus.Config.PageSize;
        public int MinimumPrefixLength { get; set; } = PromptPlus.Config.CompletionMinimumPrefixLength;
        public int CompletionWaitToStart { get; set; } = PromptPlus.Config.CompletionWaitToStart;
        public int CompletionMaxCount { get; set; } = PromptPlus.Config.CompletionMaxCount;
        public Func<string, int, CancellationToken, Task<string[]>> CompletionAsyncService { get; set; }
        public Spinners Spinner { get; set; } = new Spinners(SpinnersType.Ascii, PromptPlus.Console.IsUnicodeSupported);
        public Style SpinnerStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; } = null;
        public ushort MaxLenght { get; set; } = ushort.MaxValue;
        public string DefaultEmptyValue { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> ChangeDescription { get; set; }
        public Func<SugestionInput, SugestionOutput>? SuggestionHandler { get; set; }
    }
}
