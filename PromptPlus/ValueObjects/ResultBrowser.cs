// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System.IO;

namespace PromptPlusControls.ValueObjects
{
    public struct ResultBrowser
    {
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
            var prefix = isfile ? PromptPlus.Symbols.File : PromptPlus.Symbols.Folder;
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
