// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleFileMultiSelectControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic File Multi Select", extraLines: 1);

            var result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Search Filter('F4')", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with SearchPattern('*.LOG')", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .SearchPattern("*.LOG")
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");

            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Only Folders", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Folders: ", "My description")
                .Root("C:/")
                .OnlyFolders()
                .EnabledSearchFilter(FilterMode.Contains)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Predicate Disabled", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .PredicateDisabled((x) => x.IsFolder)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Predicate seleted", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .PredicateSelected((x) => !x.IsFolder)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Accept System and Hidden files", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .AcceptSystemAttributes()
                .AcceptHiddenAttributes()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");

            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Hide Size", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .HideSizeInfo()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");

            PromptPlus.Widgets.DoubleDash("Sample File MultiSelect with HideZeroEntries and FileSize > 1000000 (1MB)", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .HideZeroEntries()
                .HideFilesBySize(1000000)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");


            PromptPlus.Widgets.DoubleDash("Sample File Multi Select with Custom colors", extraLines: 1);

            result = PromptPlus.Controls.FileMultiSelect("Select Files/Folders: ", "My description")
                .Root("C:/")
                .Styles(FileStyles.Lines, Color.Red)
                .Styles(FileStyles.FileSize, Color.LightCoral)
                .Styles(FileStyles.FileRoot, Color.Cyan1)
                .Styles(FileStyles.FileTypeFile, Color.LightCyan1)
                .Styles(FileStyles.FileTypeFolder, Color.Yellow)
                .Styles(FileStyles.ExpandSymbol, Color.Olive)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");

        }
    }
}
