// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleFileSelectControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample basic File Select", extraLines: 1);

            var result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath??string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with Search Filter('F4')", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with SearchPattern('*.LOG')", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .SearchPattern("*.LOG")
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");

            PromptPlus.Widgets.DoubleDash("Sample File Select with Only Folders", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select Folder: ", "My description")
                .Root("/")
                .OnlyFolders()
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with Predicate Disabled", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File: ", "My description")
                .Root("/")
                .PredicateDisabled((x) => x.IsFolder)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with Predicate seleted", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File: ", "My description")
                .Root("/")
                .PredicateSelected((x) => !x.IsFolder)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with Accept System and Hidden files", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .AcceptSystemAttributes()
                .AcceptHiddenAttributes()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");

            PromptPlus.Widgets.DoubleDash("Sample File Select with Hide Size", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .HideSizeInfo()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with HideZeroEntries and FileSize > 1000000 (1MB)", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .HideZeroEntries()
                .HideFilesBySize(1000000)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");


            PromptPlus.Widgets.DoubleDash("Sample File Select with Custom colors", extraLines: 1);

            result = PromptPlus.Controls.FileSelect("Select File/Folder: ", "My description")
                .Root("/")
                .Styles(FileStyles.Lines, Color.Red)
                .Styles(FileStyles.FileSize, Color.LightCoral)
                .Styles(FileStyles.FileRoot, Color.Cyan1)
                .Styles(FileStyles.FileTypeFile, Color.LightCyan1)
                .Styles(FileStyles.FileTypeFolder, Color.Yellow)
                .Styles(FileStyles.ExpandSymbol, Color.Olive)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FullPath ?? string.Empty}");

        }
    }
}
