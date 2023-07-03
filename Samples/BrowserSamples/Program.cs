using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace BrowserSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            var root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            PromptPlus.DoubleDash("Control:Browser - minimal usage");
            var brw = PromptPlus.Browser("Browser")
                .Run();


            if (!brw.IsAborted)
            {
                PromptPlus.WriteLine($"You item name is {brw.Value.Name}");
                PromptPlus.WriteLine($"You item CurrentFolder is {brw.Value.CurrentFolder}");
                PromptPlus.WriteLine($"You item FullPath is {brw.Value.FullPath}");
                PromptPlus.WriteLine($"You item Length is {brw.Value.Length}");
                PromptPlus.WriteLine($"You item IsFolder {brw.Value.IsFolder}");
            }

            PromptPlus.DoubleDash("Control:Browser - Start Expandall usage");
            PromptPlus.Browser("Browser")
                .Root(root, true)
                .Run();

            PromptPlus.DoubleDash("Control:Browser - Spinner usage");
            PromptPlus.Browser("Browser")
                .Root(root, true)
                .Spinner(SpinnersType.Bounce)
                .Run();

            PromptPlus.DoubleDash("Control:Browser - DisabledRecursiveExpand usage");
            PromptPlus.Browser("Browser", "expand-all has same behaviour expand when used DisabledRecursiveExpand.")
                .Root(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Root.FullName, false)
                .DisabledRecursiveExpand()
                .Run();

            PromptPlus.DoubleDash("Control:Browser - AcceptHiddenAttributes/AcceptSystemAttributes usage");
            PromptPlus.Browser("Browser", "expandall has same behaviour expand when used DisabledRecursiveExpand.")
                .Root(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Root.FullName, false)
                .DisabledRecursiveExpand()
                .AcceptHiddenAttributes(true)
                .AcceptSystemAttributes(true)
                .Run();

            PromptPlus.DoubleDash("Control:Browser - SearchFolderPattern 'P*' usage");
            PromptPlus.Browser("Browser")
                .Root(root, true)
                .SearchFolderPattern("P*")
                .Run();

            PromptPlus.DoubleDash("Control:Browser - Load Only Folders");
            PromptPlus.Browser("Browser")
                .Root(root, true)
                .OnlyFolders(true)
                .Run();

            PromptPlus.DoubleDash("Control:Browser - Valid Selected item function");
            PromptPlus.Browser("Browser")
                .Root(root, true,(item) => !item.IsFolder)
                .Run();

            PromptPlus.DoubleDash("Control:Browser - Change Style");
            PromptPlus.Browser("Browser", "Folder Color.Blue / Color.Yellow ")
                .Root(root, true)
                .Styles(StyleBrowser.UnselectedFolder, Style.Plain.Foreground(Color.Blue))
                .Styles(StyleBrowser.SelectedFolder, Style.Plain.Foreground(Color.Yellow))
                .Styles(StyleBrowser.Lines, Style.Plain.Foreground(Color.Red))
                .Run();

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}