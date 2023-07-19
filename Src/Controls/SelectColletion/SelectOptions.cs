// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class SelectOptions<T>: BaseOptions
    {
        private SelectOptions()
        {
            throw new PromptPlusException("SelectOptions CTOR NotImplemented");
        }

        internal SelectOptions(bool showcursor) : base(showcursor)
        {
        }

        public bool IsOrderDescending { get; set; }

        public Func<T, object> OrderBy { get; set; } = null;

        public FilterMode FilterType { get; set; } = FilterMode.Contains;

        public string? OverwriteDefaultFrom { get; set; } = null;

        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;

        public List<ItemSelect<T>> Items { get; set; } = new List<ItemSelect<T>>();

        public List<T> DisableItems { get; set; } = new List<T>();

        public List<T> RemoveItems { get; set; } = new List<T>();

        public Func<T, T, bool> EqualItems { get; set; } = null;

        public Optional<T> DefaultValue { get; set; } = Optional<T>.Create(null);

        public Func<T, string> TextSelector { get; set; } = null;

        public Func<T, string> DescriptionSelector { get; set; } = null;

        public int PageSize { get; set; } = PromptPlus.Config.PageSize;

        public bool AutoSelect { get; set; }

    }
}
