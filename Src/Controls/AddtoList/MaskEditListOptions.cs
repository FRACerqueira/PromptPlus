// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class MaskEditListOptions : MaskEditOptions
    {
        internal MaskEditListOptions(bool showcursor) : base(showcursor)
        {
        }
        public HotKey RemoveItemPress { get; set; } = PromptPlus.Config.RemoveItemPress;
        public HotKey EditItemPress { get; set; } = PromptPlus.Config.EditItemPress;
        public IList<ItemListControl> Items { get; set; } = new List<ItemListControl>();
        public bool AllowDuplicate { get; set; } = false;
        public int PageSize { get; set; } = PromptPlus.Config.PageSize;
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = int.MaxValue;

    }
}
