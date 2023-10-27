// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class MultiSelectOptions<T> : BaseOptions
    {
        private MultiSelectOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("MultiSelectOptions CTOR NotImplemented");
        }

        internal MultiSelectOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            TimeoutOverwriteDefault = config.HistoryTimeout;
            PageSize = config.PageSize;
            SelectAllPress = config.SelectAllPress;
            InvertSelectedPress = config.InvertSelectedPress;
        }

        public string? OverwriteDefaultFrom { get; set; } = null;

        public TimeSpan TimeoutOverwriteDefault { get; set; }

        public List<ItemMultSelect<T>> Items { get; set; } = new List<ItemMultSelect<T>>();

        public List<T> RemoveItems { get; set; } = new List<T>();

        public List<T> DisableItems { get; set; } = new List<T>();

        public Optional<IList<T>> DefaultValues { get; set; } = Optional<IList<T>>.Empty();

        public Func<T, T, bool> EqualItems { get; set; }

        public bool IsOrderDescending { get; set; }

        public Func<T, object> OrderBy { get; set; }

        public int Minimum { get; set; }

        public int Maximum { get; set; } = int.MaxValue;

        public bool ShowGroupTip { get; set; }

        public Func<T, string> TextSelector { get; set; }

        public Func<T, string> DescriptionSelector { get; set; }

        public int PageSize { get; set; }

        public FilterMode FilterType { get; set; } = FilterMode.Contains;

        public HotKey SelectAllPress { get; set; }

        public HotKey InvertSelectedPress { get; set; }
    }
}
