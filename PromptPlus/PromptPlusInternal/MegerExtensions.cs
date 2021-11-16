// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using PromptPlusObjects;

namespace PromptPlusInternal
{
    internal static class MegerExtensions
    {
        public static void Merge(this IList<Func<object, ValidationResult>> source, IEnumerable<Func<object, ValidationResult>> validators)
        {
            foreach (var validator in validators ?? Enumerable.Empty<Func<object, ValidationResult>>())
            {
                source.Add(validator);
            }
        }

        public static void Merge(this IList<SingleProcess> source, SingleProcess process)
        {
            source.Add(process);
        }
    }
}
