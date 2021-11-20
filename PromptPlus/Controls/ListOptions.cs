// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    internal class ListOptions<T> : BaseOptions
    {
        public bool AllowDuplicate { get; set; } = true;
        public int? PageSize { get; set; }
        public int Minimum { get; set; } = 0;
        public bool UpperCase { get; set; } = false;
        public int Maximum { get; set; } = int.MaxValue;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<T, string> TextSelector { get; set; } = x => x.ToString();
        public MaskedOptions MaskedOption { get; set; } = new MaskedOptions();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> DescriptionSelector { get; set; }

    }
}
