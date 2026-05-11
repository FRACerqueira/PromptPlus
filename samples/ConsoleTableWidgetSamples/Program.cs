// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;

namespace ConsoleTableWidgetSamples
{
    internal class Program
    {
        internal class MyTable
        {
            public int Id { get; set; }
            public string MyText { get; set; } = string.Empty;
            public DateTime? MyDate { get; set; }
        }

        internal static MyTable[] CreateItems(int max)
        {
            var result = new List<MyTable>();
            var flag = false;
            for (int i = 1; i < max; i++)
            {
                flag = !flag;
                if (flag)
                {
                    result.Add(new MyTable { Id = i, MyText = $"Test{i}" });
                }
                else
                {
                    result.Add(new MyTable { Id = i, MyDate = DateTime.Now.AddDays(i), MyText = $"Test{i} very very very very very very very very very very very very very very very long" });
                }
            }
            return [.. result];
        }

        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            var data = CreateItems(5);

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash($"Table - custom colors)", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);


            PromptPlus.Widgets.Table<MyTable>()
                .AddItems(data)
                .AddColumn("Id", 10, (item) => item.Id.ToString())
                .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                .AddColumn("My Text1", 15, (item) => $"1:{item.MyText}", maxslidinglines: 2)
                .Styles(TableStyles.Lines, Color.Red)
                .Styles(TableStyles.TableContent, Color.Yellow)
                .Styles(TableStyles.TableHeader, Color.Blue)
                .Styles(TableStyles.TableTitle, Color.Cyan1)
                .Show();

            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Table - Hide Headers)", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            PromptPlus.Widgets.Table<MyTable>()
                .HideHeaders()
                .AddItems(data)
                .AddColumn("Id", 10, (item) => item.Id.ToString())
                .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                .AddColumn("My Text1", 15, (item) => $"1:{item.MyText}", maxslidinglines: 2)
                .Show();
            PromptPlus.Console.WriteLine("");

            var layout = Enum.GetValues<TableLayout>().Cast<TableLayout>();

            foreach (var item in layout)
            {
                PromptPlus.Widgets.DoubleDash($"Table -  layout({item})", DashOptions.DoubleBorder, style: Color.Yellow, extraLines:1);

                PromptPlus.Widgets.Table<MyTable>()
                    .Layout(item)
                    .AddItems(data)
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text1", 15, (item) => $"1:{item.MyText}", maxslidinglines: 2)
                    .Show();
                PromptPlus.Console.WriteLine("");

                PromptPlus.Widgets.DoubleDash($"Table - layout({item}) with Separator Rows", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

                PromptPlus.Widgets.Table<MyTable>()
                    .SeparatorRows()
                    .Layout(item)
                    .AddItems(data)
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text1", 15, (item) => $"1:{item.MyText}", maxslidinglines: 2)
                    .Show();
                PromptPlus.Console.WriteLine("");
            }

            PromptPlus.Controls.KeyPress("Press any key")
                .Options(cfg => cfg.HideOnAbort(true).ShowTooltip(false).EnabledAbortKey(false))
                .Run();
        }
    }
}