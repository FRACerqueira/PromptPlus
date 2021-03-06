// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.IO;

using static PPlus.PromptPlus;

namespace PPlus.Objects
{
    public struct ResultBrowser
    {
        public ResultBrowser()
        {
            PathValue = null;
            SelectedValue = null;
            NotFound = true;
            IsFile = false;
            AliasSelected = null;
        }

        internal ResultBrowser(string folder, string vale, bool notfound)
        {
            PathValue = folder;
            SelectedValue = vale;
            NotFound = notfound;
            IsFile = false;
            AliasSelected = SelectedValue;
        }

        internal ResultBrowser(string folder, string vale, bool notfound, bool isfile, bool showpath)
        {
            PathValue = folder;
            SelectedValue = vale;
            NotFound = notfound;
            IsFile = isfile;
            var prefix = isfile ? Symbols.File : Symbols.Folder;
            if (showpath)
            {
                AliasSelected = $"{prefix} {Path.Combine(PathValue, SelectedValue)}";
            }
            else
            {
                AliasSelected = $"{prefix} {SelectedValue}";
            }
        }

        public string PathValue { get; }
        public string SelectedValue { get; }
        public bool NotFound { get; }
        internal bool IsFile { get; }
        internal string AliasSelected { get; }
    }
}
