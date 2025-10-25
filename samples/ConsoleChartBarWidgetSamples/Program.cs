// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleChartBarWidgetSamples
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

            PromptPlus.Widgets.ChartBar("Sample Chart Bar", TextAlignment.Left)
                .AddItem("Item 1", 10)
                .AddItem("Item 2", 20)
                .AddItem("Item 3", 30)
                .AddItem("Item 4", 40)
                .AddItem("Item 5", 50)
                .AddItem("Item 6", 60)
                .AddItem("Item 7", 70)
                .Show();

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with legends", extraLines: 1);

            PromptPlus.Widgets.ChartBar("Sample Chart Bar", showlegends:true)
                .AddItem("Item 1", 10)
                .AddItem("Item 2", 20)
                .AddItem("Item 3", 30)
                .AddItem("Item 4", 40)
                .AddItem("Item 5", 50)
                .AddItem("Item 6", 60)
                .AddItem("Item 7", 70)
                .Show();

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with Hide elements and layout", extraLines: 1);

            var items = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                items.Add(i);
            }   
            PromptPlus.Widgets.ChartBar("Sample Chart Bar")
                .Layout(ChartBarLayout.Stacked)
                .Interaction(items, (item, bar) => bar.AddItem($"Label{item}",item))
                .HideElements( HideChart.Percentage)
                .Show();

            PromptPlus.Widgets.DoubleDash("Sample Chart Bar with bar types", extraLines: 1);

            var types = Enum.GetValues<ChartBarType>();
            foreach (var type in types)
            {
                PromptPlus.Widgets.ChartBar("Sample Chart Bar")
                    .BarType(type)
                    .Interaction(items, (item, bar) => bar.AddItem($"Label{item}", item))
                    .Show();
                PromptPlus.Console.WriteLine("");
            }
        }
    }
}
