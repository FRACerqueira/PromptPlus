// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

using PromptPlusControls.Internal;

namespace PromptPlusControls.Controls
{
    internal class MultiSelectOptions<T> : BaseOptions
    {
        public IList<ItemMultSelect<T>> Items { get; set; } = new List<ItemMultSelect<T>>();
        public IList<T> HideItems { get; set; } = new List<T>();
        public IList<T> DisableItems { get; set; } = new List<T>();
        public IList<T> DefaultValues { get; set; } = new List<T>();
        public int? PageSize { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
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
    }
}
