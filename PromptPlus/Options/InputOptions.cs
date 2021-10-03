// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromptPlus.Options
{
    public class InputOptions : BaseOptions
    {
        public bool SwithVisiblePassword { get; set; } = true;

        public bool IsPassword { get; set; }

        public object DefaultValue { get; set; }

        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
    }
}
