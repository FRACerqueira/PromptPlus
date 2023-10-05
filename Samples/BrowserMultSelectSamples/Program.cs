// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus;
using PPlus.Controls;

namespace BrowserMultSelectSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            var root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - minimal usage");
            var brw = PromptPlus.BrowserMultiSelect("Browser")
                .Run();


            if (!brw.IsAborted)
            {
                foreach (var item in brw.Value)
                {
                    PromptPlus.WriteLine($"You item name is {item.Name}");
                    PromptPlus.WriteLine($"You item CurrentFolder is {item.CurrentFolder}");
                    PromptPlus.WriteLine($"You item FullPath is {item.FullPath}");
                    PromptPlus.WriteLine($"You item Length is {item.Length}");
                    PromptPlus.WriteLine($"You item IsFolder {item.IsFolder}");
                }
            }

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - SelectAll at startup usage");
            PromptPlus.BrowserMultiSelect("Browser")
                .SelectAll(item => !item.IsFolder)
                .Run();



            PromptPlus.DoubleDash("Control:BrowserMultiSelect - Start Expandall usage");
            PromptPlus.BrowserMultiSelect("Browser")
                .Root(root, true)
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - Spinner usage");
            PromptPlus.BrowserMultiSelect("Browser")
                .Root(root, true)
                .Spinner(SpinnersType.Bounce)
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - DisabledRecursiveExpand usage");
            PromptPlus.BrowserMultiSelect("Browser", "expandall has same behavior expand when used DisabledRecursiveExpand.")
                .Root(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Root.FullName, false)
                .DisabledRecursiveExpand()
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - AcceptHiddenAttributes/AcceptSystemAttributes usage");
            PromptPlus.BrowserMultiSelect("Browser", "expandall has same behavior expand when used DisabledRecursiveExpand.")
                .Root(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Root.FullName, false)
                .DisabledRecursiveExpand()
                .AcceptHiddenAttributes(true)
                .AcceptSystemAttributes(true)
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - SearchFolderPattern 'P*' usage");
            PromptPlus.BrowserMultiSelect("Browser")
                .Root(root, true)
                .SearchFolderPattern("P*")
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - Load Only Folders");
            PromptPlus.BrowserMultiSelect("Browser")
                .Root(root, true)
                .OnlyFolders(true)
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - Valid Selected item function");
            PromptPlus.BrowserMultiSelect("Browser")
                .Root(root, true, (item) => !item.IsFolder)
                .Run();

            PromptPlus.DoubleDash("Control:BrowserMultiSelect - Change Style");
            PromptPlus.BrowserMultiSelect("Browser", "Folder Color.Blue / Color.Yellow ")
                .Root(root, true)
                .Styles(StyleBrowser.UnselectedFolder, Style.Default.Foreground(Color.Blue))
                .Styles(StyleBrowser.SelectedFolder, Style.Default.Foreground(Color.Yellow))
                .Styles(StyleBrowser.Lines, Style.Default.Foreground(Color.Red))
                .Run();

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}