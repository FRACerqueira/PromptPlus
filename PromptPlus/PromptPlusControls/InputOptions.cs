// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromptPlusControls
{
    internal class InputOptions : BaseOptions
    {
        public bool SwithVisiblePassword { get; set; } = true;
        public bool IsPassword { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> DescriptionSelector { get; set; }
    }
}
