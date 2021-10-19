// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromptPlusControls.Controls
{
    internal class InputOptions : BaseOptions
    {
        public bool SwithVisiblePassword { get; set; } = true;

        public bool IsPassword { get; set; }

        public string DefaultValue { get; set; }

        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
    }
}
