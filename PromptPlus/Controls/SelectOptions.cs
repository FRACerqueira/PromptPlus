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

        Func<T, string> _textSelector = x => x?.ToString();
        public Func<T, string> TextSelector
        {
            get { return _textSelector; }
            set
            {
                if (value == null)
                {
                    value = x => x?.ToString();
                }
                _textSelector = value;
            }
        }

        public Func<T, string> DescriptionSelector { get; set; } = null;
        public bool AutoSelectIfOne { get; set; }
    }
}
