// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace ChartBarWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            try
            {
                ShowSection("1) Basic Widget - display only");
                PromptPlus.Widgets.ChartBar()
                    .AddItem("Item A", 40)
                    .AddItem("Item B", 85)
                    .AddItem("Item C", 60)
                    .Show();

                ShowSection("2) With Title and Layout");
                PromptPlus.Widgets.ChartBar()
                    .Title("Sales Dashboard", TextAlignment.Center)
                    .AddItem("North Region", 120)
                    .AddItem("South Region", 80)
                    .AddItem("East Region", 95)
                    .AddItem("West Region", 110)
                    .Layout(ChartBarLayout.Standard)
                    .Show();

                ShowSection("3) Stacked Layout");
                PromptPlus.Widgets.ChartBar()
                    .Title("System Resources", TextAlignment.Center)
                    .AddItem("CPU Usage", 45)
                    .AddItem("Memory Usage", 70)
                    .AddItem("Disk Usage", 30)
                    .Layout(ChartBarLayout.Stacked)
                    .Show();

                ShowSection("4) Custom Colors");
                PromptPlus.Widgets.ChartBar()
                    .Title("Performance Metrics", TextAlignment.Center)
                    .AddItem("Response Time", 60, Color.Red)
                    .AddItem("Throughput", 80, Color.Green)
                    .AddItem("Error Rate", 25, Color.Blue)
                    .AddItem("Success Rate", 95, Color.Yellow)
                    .Show();

                ShowSection("5) Light Bar Type");
                PromptPlus.Widgets.ChartBar()
                    .Title("Product Sales", TextAlignment.Center)
                    .BarType(ChartBarType.Light)
                    .AddItem("Product A", 55)
                    .AddItem("Product B", 90)
                    .AddItem("Product C", 70)
                    .AddItem("Product D", 45)
                    .Show();

                ShowSection("6) Square Bar Type with Legends");
                PromptPlus.Widgets.ChartBar()
                    .Title("Quarterly Revenue", TextAlignment.Center)
                    .BarType(ChartBarType.Square)
                    .AddItem("Q1 2024", 100)
                    .AddItem("Q2 2024", 120)
                    .AddItem("Q3 2024", 90)
                    .AddItem("Q4 2024", 140)
                    .ShowLegends(true)
                    .Show();

                ShowSection("7) Hide Values");
                PromptPlus.Widgets.ChartBar()
                    .Title("Simplified Chart", TextAlignment.Center)
                    .AddItem("Category A", 40)
                    .AddItem("Category B", 85)
                    .AddItem("Category C", 60)
                    .HideElements(HideChart.Values)
                    .Show();

                ShowSection("8) Hide Percentage");
                PromptPlus.Widgets.ChartBar()
                    .Title("Chart Without Percentages", TextAlignment.Center)
                    .AddItem("Data 1", 75)
                    .AddItem("Data 2", 50)
                    .AddItem("Data 3", 95)
                    .HideElements(HideChart.Percentage)
                    .Show();

                ShowSection("9) Ordered by Highest Value");
                PromptPlus.Widgets.ChartBar()
                    .Title("Top Performers", TextAlignment.Center)
                    .AddItem("Low Performer", 20)
                    .AddItem("Top Performer", 90)
                    .AddItem("Mid Performer", 50)
                    .AddItem("Good Performer", 70)
                    .OrderBy(ChartBarOrder.Highest)
                    .Show();

                ShowSection("10) Ordered by Smallest Value");
                PromptPlus.Widgets.ChartBar()
                    .Title("Bottom to Top", TextAlignment.Center)
                    .AddItem("High Value", 90)
                    .AddItem("Low Value", 20)
                    .AddItem("Mid Value", 50)
                    .OrderBy(ChartBarOrder.Smallest)
                    .Show();

                ShowSection("11) Custom Width and Precision");
                PromptPlus.Widgets.ChartBar()
                    .Title("High Precision Data", TextAlignment.Center)
                    .Width(70)
                    .FractionalDigits(3)
                    .AddItem("Measurement A", 45.6789)
                    .AddItem("Measurement B", 82.1234)
                    .AddItem("Measurement C", 63.4567)
                    .Show();

                ShowSection("12) Multiple Items with Legends");
                PromptPlus.Widgets.ChartBar()
                    .Title("Annual Report - All Regions", TextAlignment.Center)
                    .AddItem("North", 120)
                    .AddItem("South", 80)
                    .AddItem("East", 95)
                    .AddItem("West", 110)
                    .AddItem("Central", 105)
                    .AddItem("International", 150)
                    .ShowLegends(true)
                    .Show();

                ShowSection("13) Compact Chart (hide title and ordering)");
                PromptPlus.Widgets.ChartBar()
                    .AddItem("Item 1", 60)
                    .AddItem("Item 2", 40)
                    .AddItem("Item 3", 80)
                    .HideElements(HideChart.Title | HideChart.Ordering)
                    .Show();

                ShowSection("14) Stacked with Custom Description");
                PromptPlus.Widgets.ChartBar()
                    .Title("Resource Distribution", TextAlignment.Center)
                    .Layout(ChartBarLayout.Stacked)
                    .AddItem("Available", 30)
                    .AddItem("In Use", 50)
                    .AddItem("Reserved", 20)
                    .ChangeDescription(item => $"Current state: {item.Label}")
                    .Show();

                PromptPlus.Console.WriteLine();
                PromptPlus.Console.WriteLine("All ChartBar widget samples completed!", ConsolePlus.CurrentStyle.ForeGround(Color.Green));
            }
            catch (Exception ex)
            {
                PromptPlus.Console.WriteException(ex, ConsolePlus.CurrentStyle);
            }
            finally
            {
                PromptPlus.Console.WriteLine();
                PromptPlus.Console.WriteLine("Press any key to exit...", ConsolePlus.CurrentStyle);
                PromptPlus.Console.ReadKey();
            }
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
    }
}
