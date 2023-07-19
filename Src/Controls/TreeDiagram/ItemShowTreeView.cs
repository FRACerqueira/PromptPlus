// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    internal struct ItemShowTreeView
    {
        public ItemShowTreeView()
        {
            TextExpand = string.Empty;
            TextItem = string.Empty;
            TextLines = string.Empty;
            TextFullpath = string.Empty;
            TextSeleted = string.Empty;
            TextSize = string.Empty;
        }
        public string TextSize { get; set; }
        public string TextSeleted { get; set; }
        public string TextLines { get; set; }
        public string TextItem { get; set; }
        public string TextFullpath { get; set; }
        public string TextExpand { get; set; }
        public string TextFlat 
        {
            get
            {
                if (!string.IsNullOrEmpty(TextExpand))
                {
                    return $"{TextLines}{TextExpand}{TextSeleted}{TextItem}{TextSize}";
                }
                return $"{TextLines}{TextSeleted}{TextItem}{TextSize}";
            }
        } 
    }
}
