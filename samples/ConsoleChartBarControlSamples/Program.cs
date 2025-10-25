// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleChartBarControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic Chart Bar", extraLines: 1);

            var result = PromptPlus.Controls.ChartBar("Select bar: ")
                .Title("Sample Chart Bar")
                .AddItem("Item 1", 10)
                .AddItem("Item 2", 20)
                .AddItem("Item 3", 30)
                .AddItem("Item 4", 40)
                .AddItem("Item 5", 50)
                .AddItem("Item 6", 60)
                .AddItem("Item 7", 70)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content!.Label!} {result.Content!.Value} {result.Content!.Percent}%");

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with legends", extraLines: 1);

            result = PromptPlus.Controls.ChartBar("Select bar: ")
                .Title("Sample Chart Bar")
                .AddItem("Item 1", 10)
                .AddItem("Item 2", 10)
                .AddItem("Item 3", 30)
                .AddItem("Item 4", 40)
                .AddItem("Item 5", 50)
                .AddItem("Item 6", 60)
                .AddItem("Item 7", 70)
                .ShowLegends()
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content!.Label!} {result.Content!.Value}  {result.Content!.Percent}%");

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with Hide elements and initial layout", extraLines: 1);

            var items = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                items.Add(i);
            }   
            result = PromptPlus.Controls.ChartBar("Select bar: ")
                .Title("Sample Chart Bar")
                .Layout(ChartBarLayout.Stacked)
                .Interaction<int>(items, (item, bar) => bar.AddItem($"Label{item}",item))
                .HideElements( HideChart.Percentage | HideChart.Ordering | HideChart.Layout | HideChart.ChartbarAtFinish)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content!.Label!} {result.Content!.Value}  {result.Content!.Percent}%");

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with bar types", extraLines: 1);

            var types = Enum.GetValues<ChartBarType>();
            foreach (var type in types)
            {
                result = PromptPlus.Controls.ChartBar("Select bar: ")
                    .BarType(type)
                    .Interaction<int>(items, (item, bar) => bar.AddItem($"Label{item}", item))
                    .HideElements(HideChart.Percentage | HideChart.Ordering | HideChart.Layout | HideChart.ChartbarAtFinish)
                    .ChangeDescription((item) => $"Select {item.Label} / {type}")    
                    .Run();
                PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content!.Label!} {result.Content!.Value}  {result.Content!.Percent}%");
            }
        }
    }
}
