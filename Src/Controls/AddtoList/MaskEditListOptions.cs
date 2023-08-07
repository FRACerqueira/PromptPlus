// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class MaskEditListOptions : MaskEditOptions
    {
        private MaskEditListOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("MaskEditListOptions CTOR NotImplemented");
        }

        internal MaskEditListOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            RemoveItemPress = config.RemoveItemPress;
            EditItemPress = config.EditItemPress;
            PageSize = config.PageSize;
        }
        public HotKey RemoveItemPress { get; set; }
        public HotKey EditItemPress { get; set; }
        public IList<ItemListControl> Items { get; set; } = new List<ItemListControl>();
        public bool AllowDuplicate { get; set; }
        public int PageSize { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;

    }
}
