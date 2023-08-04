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
        private SelectOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("SelectOptions CTOR NotImplemented");
        }

        internal SelectOptions(StyleSchema styleSchema, Config config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            TimeoutOverwriteDefault = config.HistoryTimeout;
            PageSize = config.PageSize;
        }

        public bool IsOrderDescending { get; set; }

        public Func<T, object> OrderBy { get; set; }

        public FilterMode FilterType { get; set; } = FilterMode.Contains;

        public string? OverwriteDefaultFrom { get; set; } = null;

        public TimeSpan TimeoutOverwriteDefault { get; set; }

        public List<ItemSelect<T>> Items { get; set; } = new List<ItemSelect<T>>();

        public List<T> DisableItems { get; set; } = new List<T>();

        public List<T> RemoveItems { get; set; } = new List<T>();

        public Func<T, T, bool> EqualItems { get; set; }

        public Optional<T> DefaultValue { get; set; } = Optional<T>.Create(null);

        public Func<T, string> TextSelector { get; set; }

        public Func<T, string> DescriptionSelector { get; set; }

        public int PageSize { get; set; }

        public bool AutoSelect { get; set; }

    }
}
