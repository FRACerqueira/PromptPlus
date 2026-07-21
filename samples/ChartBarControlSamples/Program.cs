// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using System.Security.Cryptography;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace ChartBarControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic - simple chart bar");
                var result = PromptPlus.Controls.ChartBar("Select item")
                    .AddItem("Item A", 40)
                    .AddItem("Item B", 85)
                    .AddItem("Item C", 60)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("2) With Title and Layout");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Sales by Region", TextAlignment.Center)
                    .AddItem("North", 120)
                    .AddItem("South", 80)
                    .AddItem("East", 95)
                    .AddItem("West", 110)
                    .Layout(ChartBarLayout.Standard)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("3) Stacked Layout");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Resource Usage", TextAlignment.Center)
                    .AddItem("CPU", 45)
                    .AddItem("Memory", 70)
                    .AddItem("Disk", 30)
                    .Layout(ChartBarLayout.Stacked)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("4) Custom Colors");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Custom Colored Bars", TextAlignment.Center)
                    .AddItem("Red Item", 60, Color.Red)
                    .AddItem("Green Item", 80, Color.Green)
                    .AddItem("Blue Item", 45, Color.Blue)
                    .AddItem("Yellow Item", 70, Color.Yellow)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("5) Different Bar Types");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Light Bar Type", TextAlignment.Center)
                    .BarType(ChartBarType.Light)
                    .AddItem("Product 1", 55)
                    .AddItem("Product 2", 90)
                    .AddItem("Product 3", 70)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("6) With Legends");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Sales with Legends", TextAlignment.Center)
                    .AddItem("Q1", 100)
                    .AddItem("Q2", 120)
                    .AddItem("Q3", 90)
                    .AddItem("Q4", 140)
                    .ShowLegends(true)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("7) Hide Elements - hide values");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Chart without Values", TextAlignment.Center)
                    .AddItem("Item A", 40)
                    .AddItem("Item B", 85)
                    .AddItem("Item C", 60)
                    .HideElements(HideChart.Values)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("8) With Pagination (PageSize = 3)");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Paginated Chart", TextAlignment.Center)
                    .Interaction([""],(_,ctrl) => 
                    {
                        for (int i = 0; i < 320; i++)
                        {
                            ctrl.AddItem($"Item {i + 1}", RandomNumberGenerator.GetInt32(0, 100));
                        }
                    })
                    .PageSize(0)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("9) Ordered by Value (Highest)");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Ordered by Highest Value", TextAlignment.Center)
                    .AddItem("Low", 20)
                    .AddItem("High", 90)
                    .AddItem("Medium", 50)
                    .OrderBy(ChartBarOrder.Highest)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("10) Custom Width and FractionalDigits");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Custom Width (70) & 2 Decimals", TextAlignment.Center)
                    .Width(70)
                    .FractionalDigits(2)
                    .AddItem("Value A", 45.678)
                    .AddItem("Value B", 82.123)
                    .AddItem("Value C", 63.456)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("11) With Custom Description");
                result = PromptPlus.Controls.ChartBar("Select item", "Navigate with arrows, Enter to select")
                    .Title("Chart with Description", TextAlignment.Center)
                    .AddItem("Option 1", 55)
                    .AddItem("Option 2", 75)
                    .AddItem("Option 3", 40)
                    .ChangeDescription(item => $"Selected: {item.Label} ({item.Percent:F1}%)")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("12) Square Bar Type with Validation");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Square Bars with Validation", TextAlignment.Center)
                    .BarType(ChartBarType.Square)
                    .AddItem("Valid", 80)
                    .AddItem("Invalid (< 50)", 30)
                    .AddItem("Valid", 60)
                    .PredicateSelected(item =>
                    {
                        if (item.Value < 50)
                        {
                            return (false, "Value must be >= 50");
                        }
                        return (true, null);
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("13) Disable Layout Switcher");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Layout Switcher Disabled", TextAlignment.Center)
                    .AddItem("Item A", 40)
                    .AddItem("Item B", 85)
                    .AddItem("Item C", 60)
                    .AddItem("Item D", 55)
                    .EnableLayoutSwitcher(false)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("14) Disable Ordering Switcher");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Ordering Switcher Disabled", TextAlignment.Center)
                    .AddItem("Low", 20)
                    .AddItem("High", 90)
                    .AddItem("Medium", 50)
                    .AddItem("Very High", 95)
                    .EnableOrderingSwitcher(false)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("15) Disable Both Switchers");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .Title("Both Switchers Disabled", TextAlignment.Center)
                    .AddItem("Product A", 65)
                    .AddItem("Product B", 45)
                    .AddItem("Product C", 80)
                    .EnableLayoutSwitcher(false)
                    .EnableOrderingSwitcher(false)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("16) Combined: Hide Title + No Percentage");
                result = PromptPlus.Controls.ChartBar("Select item")
                    .AddItem("Data 1", 40)
                    .AddItem("Data 2", 75)
                    .AddItem("Data 3", 50)
                    .HideElements(HideChart.Title | HideChart.Percentage)
                    .Run();
                PrintSelectionResult(result);

            PromptPlus.Console.WriteLine("All ChartBar control samples completed!", ConsolePlus.CurrentStyle);

            PromptPlus.Console.WriteLine("Press any key to exit...", ConsolePlus.CurrentStyle);
            PromptPlus.Console.ReadKey();
        }

        private static void ConfigureCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Console.WriteLine();
            PromptPlus.Widgets.Dash(title);
            PromptPlus.Console.WriteLine();
        }

        private static void PrintSelectionResult(ResultPrompt<ChartItem?> result)
        {
            if (result.IsAborted)
            {
                PromptPlus.Console.WriteLine("Operation aborted or canceled.", ConsolePlus.CurrentStyle.ForeGround(Color.Red));
            }
            else if (result.Content != null)
            {
                PromptPlus.Console.WriteLine($"Selected: {result.Content.Label} (Value: {result.Content.Value}, Percent: {result.Content.Percent:F2}%)", 
                    ConsolePlus.CurrentStyle.ForeGround(Color.Green));
            }
            else
            {
                PromptPlus.Console.WriteLine("No item selected.", ConsolePlus.CurrentStyle.ForeGround(Color.Yellow));
            }
        }
    }
}
