// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal class TableOptions<T> : BaseOptions
    {
        private TableOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("TableOptions CTOR NotImplemented");
        }

        internal TableOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            TimeoutOverwriteDefault = config.HistoryTimeout;
            PageSize = config.PageSize;
            SelectAllPress = config.SelectAllPress;
            InvertSelectedPress = config.InvertSelectedPress;
        }

        public bool AutoSelect { get; set; }

        public bool IsOrderDescending { get; set; }

        public Func<T, object> OrderBy { get; set; }

        public FilterMode FilterType { get; set; } = FilterMode.Disabled;

        public string? OverwriteDefaultFrom { get; set; } = null;

        public TimeSpan TimeoutOverwriteDefault { get; set; }

        public List<ItemTableRow<T>> Items { get; set; } = new();

        public List<T> DisableItems { get; set; } = new();

        public List<T> RemoveItems { get; set; } = new();

        public Func<T, T, bool> EqualItems { get; set; }

        public Optional<T> DefaultValue { get; set; } = Optional<T>.Create(null);

        public Optional<IList<T>> DefaultValues { get; set; } = Optional<IList<T>>.Create(null);

        public int PageSize { get; set; }

        public ushort[] FilterColumns { get; set; } = Array.Empty<ushort>();

        public TableLayout Layout { get; set; } = TableLayout.SingleGridFull;

        public string Title { get; set; } = string.Empty;

        public Alignment TitleAlignment { get; set; } = Alignment.Center;

        public TableTitleMode TitleMode { get; set; } = TableTitleMode.InLine;

        public List<ItemItemColumn<T>> Columns { get; set; } = new List<ItemItemColumn<T>>();

        public bool HideSelectorRow { get; set; }

        public bool HideHeaders { get; set; }
        
        public bool SeparatorRows { get; set; }

        public bool HasAutoFit{ get; set; }

        public ushort[] AutoFitColumns { get; set; } = Array.Empty<ushort>();

        public Dictionary<Type,Func<object, string>> FormatTypes { get; set; } = new();

        public bool IsInteraction { get; set; }

        public bool IsColumnsNavigation { get; set; }

        public Func<T, int, int, string> SelectedTemplate { get; set; }

        public Func<T, int, int, string> FinishTemplate { get; set; }

        public Func<IEnumerable<T>, string> MultiSelectedTemplate { get; set; }

        public Func<IEnumerable<T>, string> MultiFinishTemplate { get; set; }

        public Func<T, int, int, string> ChangeDescription { get; set; }

        public bool AutoFill { get; set; }

        public ushort? MinColWidth { get; set; }

        public ushort? MaxColWidth { get; set; }

        public int Minimum { get; set; }

        public int Maximum { get; set; } = int.MaxValue;

        public HotKey SelectAllPress { get; set; }

        public HotKey InvertSelectedPress { get; set; }


    }
}
