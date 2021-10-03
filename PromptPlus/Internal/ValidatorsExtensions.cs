// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PromptPlus.Internal
{
    internal static class ValidatorsExtensions
    {
        public static void Merge(this IList<Func<object, ValidationResult>> source, IEnumerable<Func<object, ValidationResult>> validators)
        {
            foreach (var validator in validators ?? Enumerable.Empty<Func<object, ValidationResult>>())
            {
                source.Add(validator);
            }
        }
    }
}
