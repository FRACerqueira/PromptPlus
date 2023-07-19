using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class MultiSelectOptions<T> : BaseOptions
    {
        private MultiSelectOptions()
        {
            throw new PromptPlusException("MultiSelectOptions CTOR NotImplemented");
        }

        internal MultiSelectOptions(bool showcursor) : base(showcursor)
        {
        }

        public string? OverwriteDefaultFrom { get; set; } = null;

        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;

        public List<ItemMultSelect<T>> Items { get; set; } = new List<ItemMultSelect<T>>();

        public List<T> RemoveItems { get; set; } = new List<T>();

        public List<T> DisableItems { get; set; } = new List<T>();

        public Optional<IList<T>> DefaultValues { get; set; } = Optional<IList<T>>.Create(null);

        public Func<T, T, bool> EqualItems { get; set; } = null;

        public bool IsOrderDescending { get; set; }

        public Func<T, object> OrderBy { get; set; } = null;

        public int Minimum { get; set; }

        public int Maximum { get; set; } = int.MaxValue;

        public bool ShowGroupOnDescription { get; set; }

        public Func<T, string> TextSelector { get; set; } = null;

        public Func<T, string> DescriptionSelector { get; set; } = null;

        public int PageSize { get; set; } = PromptPlus.Config.PageSize;

        public FilterMode FilterType { get; set; } = FilterMode.Contains;

        public HotKey SelectAllPress { get; set; } = PromptPlus.Config.SelectAllPress;

        public HotKey InvertSelectedPress { get; set; } = PromptPlus.Config.InvertSelectedPress;
    }
}
