using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusObjects;

namespace PromptPlusControls
{
    internal class AutoCompleteOptions : BaseOptions
    {
        public const int MinCompletionMaxCount = 3;
        public const int MinPrefixLength = 1;

        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public bool AcceptWithoutMatch { get; set; } = false;
        public int? PageSize { get; set; }
        public int MinimumPrefixLength { get; set; } = 3;
        public int CompletionInterval { get; set; } = 1000;
        public int CompletionMaxCount { get; set; } = 10;
        public Func<string, int, CancellationToken, Task<string[]>> CompletionAsyncService { get; set; }
        public Func<string, int, CancellationToken, Task<ValueDescription<string>[]>> CompletionWithDescriptionAsyncService { get; set; }
        public int SpeedAnimation { get; set; } = PromptPlus.SpeedAnimation;
        public bool DynamicDescription { get; set; }


    }
}
