// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class BrowserOptions : BaseOptions
    {
        public BrowserFilter Filter { get; set; } = BrowserFilter.None;
        public string PrefixExtension { get; set; }
        public bool AllowNotSelected { get; set; }
        public string DefaultValue { get; set; }
        public string RootFolder { get; set; }
        private string _searchPattern;
        public string SearchPattern
        {
            get { return _searchPattern ?? "*"; }
            set { _searchPattern = value; }
        }
        public int? PageSize { get; set; }
        public bool SupressHidden { get; set; } = true;
        public bool ShowNavigationCurrentPath { get; set; } = true;
        public bool ShowSearchPattern { get; set; } = true;
    }
}
