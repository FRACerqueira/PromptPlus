// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MultiFileControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            string root = Directory.GetCurrentDirectory();

            ShowSection("1) Basic - check multiple files/folders in the current directory tree");
            var result = PromptPlus.Controls.MultiFile("Check files or folders", "Space to check, Right/+ expand, Left/- collapse, Enter to confirm")
                .Root(root)
                .Run();
            PrintResult(result);

            ShowSection("2) Only folders");
            result = PromptPlus.Controls.MultiFile("Check one or more folders")
                .Root(root)
                .OnlyFolders()
                .Run();
            PrintResult(result);

            ShowSection("3) Filter files by pattern (*.cs), files only, hide size");
            result = PromptPlus.Controls.MultiFile("Check C# files")
                .Root(root)
                .SearchPattern("*.cs")
                .HideSize()
                .SelectFilesOnly()
                .Run();
            PrintResult(result);

            ShowSection("4) Show hidden and system entries + custom page size");
            result = PromptPlus.Controls.MultiFile("Browse (incl. hidden/system)")
                .Root(root)
                .ShowHidden()
                .ShowSystem()
                .PageSize(12)
                .Run();
            PrintResult(result);

            ShowSection("5) Range - require between 2 and 4 checked items");
            result = PromptPlus.Controls.MultiFile("Check between 2 and 4 items", "Enter is blocked until the range is satisfied")
                .Root(root)
                .Range(2, 4)
                .Run();
            PrintResult(result);

            ShowSection("6) Styles");
            result = PromptPlus.Controls.MultiFile("Styled browser")
                .Root(root)
                .Styles(MultiFileStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(MultiFileStyles.FileTypeFolder, new Style(Color.Cyan, Color.Black))
                .Styles(MultiFileStyles.FileTypeFile, new Style(Color.White, Color.Black))
                .Styles(MultiFileStyles.FileSize, new Style(Color.Gray, Color.Black))
                .Styles(MultiFileStyles.Selected, new Style(Color.Black, Color.Gray))
                .Run();
            PrintResult(result);

            ShowSection("7) Default - pre-check several paths and expand to the first");
            var defaults = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
                .Take(3)
                .ToArray();
            result = PromptPlus.Controls.MultiFile("Pre-checked defaults")
                .Root(root)
                .Default(defaults)
                .Run();
            PrintResult(result);

            ShowSection("8) EnabledHistory - remembers the last checked items");
            const string historyKey = "SampleMultiFile.History";
            PromptPlus.Controls.History(historyKey).Remove();
            try
            {
                // First run: check something; it gets stored in history.
                result = PromptPlus.Controls.MultiFile("Check files (will be remembered)")
                    .Root(root)
                    .EnabledHistory(historyKey)
                    .Run();
                PrintResult(result);

                // Second run: history values are used as the defaults (tree expands to the first).
                result = PromptPlus.Controls.MultiFile("Runs again (defaults from history)")
                    .Root(root)
                    .EnabledHistory(historyKey)
                    .Run();
                PrintResult(result);
            }
            finally
            {
                PromptPlus.Controls.History(historyKey).Remove();
            }

            ShowSection("9) Browse from the OS drive root (e.g. C:\\)");
            string osRoot = Path.GetPathRoot(Environment.SystemDirectory) ?? root;
            result = PromptPlus.Controls.MultiFile("Browse the system drive", $"Root: {osRoot}")
                .Root(osRoot)
                .PageSize(15)
                .Run();
            PrintResult(result);

            ShowSection("10) ShowFullPath - answer shows only 'parent\\name' (toggle with hotkey)");
            result = PromptPlus.Controls.MultiFile("Check items (answer shows short name)", "Use the full-path hotkey to toggle")
                .Root(root)
                .ShowFullPath(false)
                .Run();
            PrintResult(result);

            ShowSection("11) Options - custom control options (tooltip/abort behavior)");
            result = PromptPlus.Controls.MultiFile("Check files or folders")
                .Root(root)
                .Options(opt =>
                {
                    opt.ShowTooltip(true);
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("12) RecursiveMarkWithCtrlSpace - Space toggles the item, Ctrl+Space marks the folder recursively");
            result = PromptPlus.Controls.MultiFile("Check files or folders", "Space: check/uncheck. Ctrl+Space: check/uncheck a folder recursively")
                .Root(root)
                .RecursiveMarkWithCtrlSpace()
                .Run();
            PrintResult(result);

            ShowSection("13) CascadeCheck(false) - Space never marks folder contents recursively");
            result = PromptPlus.Controls.MultiFile("Space only toggles the item itself (folder or file)", "Recursive marking is disabled - folders are marked as single items")
                .Root(root)
                .CascadeCheck(false)
                .Run();
            PrintResult(result);

            ShowSection("14) CascadeCheck(true) + RecursiveMarkWithCtrlSpace(true) - user chooses!");
            result = PromptPlus.Controls.MultiFile("Space=single item, Ctrl+Space=recursive", "CascadeCheck ON + RecursiveMarkWithCtrlSpace gives full control")
                .Root(root)
                .CascadeCheck(true)
                .RecursiveMarkWithCtrlSpace(true)
                .Run();
            PrintResult(result);
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintResult(ResultPrompt<FileItem[]> result)
        {
            if (result.IsAborted)
            {
                PromptPlus.Console.WriteLine("IsAborted: true");
            }
            else
            {
                FileItem[] items = result.Content ?? [];
                PromptPlus.Console.WriteLine($"Checked {items.Length} item(s):");
                foreach (FileItem f in items)
                {
                    PromptPlus.Console.WriteLine($"  - {f.FullPath} (IsDirectory: {f.IsDirectory}, Size: {f.Length})");
                }
            }
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
