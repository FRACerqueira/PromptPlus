// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusControls.Controls
{
    internal class SelectOptions<T> : BaseOptions
    {
        public IList<T> Items { get; set; } = new List<T>();
        public IList<T> HideItems { get; set; } = new List<T>();
        public IList<T> DisableItems { get; set; } = new List<T>();
        public T DefaultValue { get; set; }
        public int? PageSize { get; set; }
        public Func<T, string> TextSelector { get; set; } = x => x?.ToString();
        public bool AutoSelectIfOne { get; set; }
    }
}
