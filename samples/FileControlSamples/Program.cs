// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace FileControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            string root = Directory.GetCurrentDirectory();

            ShowSection("1) Basic - browse the current directory tree");
            var result = PromptPlus.Controls.File("Select a file or folder", "Right/+ expand, Left/- collapse, Enter to select")
                .Root(root)
                .Run();
            PrintResult(result);

            ShowSection("2) Only folders");
            result = PromptPlus.Controls.File("Select a folder")
                .Root(root)
                .OnlyFolders()
                .Run();
            PrintResult(result);

            ShowSection("3) Filter files by pattern (*.cs) and hide size");
            result = PromptPlus.Controls.File("Select a C# file")
                .Root(root)
                .SearchPattern("*.cs")
                .HideSize()
                .SelectFilesOnly()
                .Run();
            PrintResult(result);

            ShowSection("4) Show hidden and system entries + custom page size");
            result = PromptPlus.Controls.File("Browse (incl. hidden/system)")
                .Root(root)
                .ShowHidden()
                .ShowSystem()
                .PageSize(12)
                .Run();
            PrintResult(result);

            ShowSection("5) Styles");
            result = PromptPlus.Controls.File("Styled browser")
                .Root(root)
                .Styles(FileStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(FileStyles.FileTypeFolder, new Style(Color.Cyan, Color.Black))
                .Styles(FileStyles.FileTypeFile, new Style(Color.White, Color.Black))
                .Styles(FileStyles.FileSize, new Style(Color.Gray, Color.Black))
                .Styles(FileStyles.Selected, new Style(Color.Black, Color.Gray))
                .Run();
            PrintResult(result);

            ShowSection("6) Default - pre-select and expand to a path");
            string? firstFile = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
                .FirstOrDefault();
            result = PromptPlus.Controls.File("Default expanded to a file")
                .Root(root)
                .Default(firstFile ?? root)
                .Run();
            PrintResult(result);

            ShowSection("7) EnableHistory - remembers the last selection");
            const string historyKey = "SampleFile.History";
            PromptPlus.Controls.History(historyKey).Remove();
            try
            {
                // First run: pick something; it gets stored in history.
                result = PromptPlus.Controls.File("Pick a file (will be remembered)")
                    .Root(root)
                    .EnableHistory(historyKey)
                    .Run();
                PrintResult(result);

                // Second run: history value is used as the default (tree expands to it).
                result = PromptPlus.Controls.File("Runs again (default from history)")
                    .Root(root)
                    .EnableHistory(historyKey)
                    .Run();
                PrintResult(result);
            }
            finally
            {
                PromptPlus.Controls.History(historyKey).Remove();
            }

            ShowSection("8) Browse from the OS drive root (e.g. C:\\)");
            string osRoot = Path.GetPathRoot(Environment.SystemDirectory) ?? root;
            result = PromptPlus.Controls.File("Browse the system drive", $"Root: {osRoot}")
                .Root(osRoot)
                .PageSize(15)
                .Run();
            PrintResult(result);

            ShowSection("9) ShowFullPath - answer shows only 'parent\\name' (toggle with hotkey)");
            result = PromptPlus.Controls.File("Select (answer shows short name)", "Use the full-path hotkey to toggle")
                .Root(root)
                .ShowFullPath(false)
                .Run();
            PrintResult(result);

            ShowSection("10) Options - custom control options (tooltip/abort behavior)");
            result = PromptPlus.Controls.File("Select a file or folder")
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

        private static void PrintResult(ResultPrompt<FileItem?> result)
        {
            if (result.IsAborted)
            {
                PromptPlus.Console.WriteLine("IsAborted: true");
            }
            else
            {
                var f = result.Content;
                PromptPlus.Console.WriteLine($"Selected: {f?.FullPath} (IsDirectory: {f?.IsDirectory}, Size: {f?.Length})");
            }
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
