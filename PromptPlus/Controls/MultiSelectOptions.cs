// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusControls.Controls
{
    internal class MultiSelectOptions<T> : BaseOptions
    {
        public IList<T> Items { get; set; } = new List<T>();

        public IList<T> DefaultValues { get; set; } = new List<T>();

        public int? PageSize { get; set; }

        public int Minimum { get; set; }

        public int Maximum { get; set; } = int.MaxValue;

        public Func<T, string> TextSelector { get; set; } = x => x.ToString();
    }
}
