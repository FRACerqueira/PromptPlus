// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;
using System.Text.Json;

namespace ConsoleTableMultiSelectSamples
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

            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - basic", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);
            var tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                    .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled" }, true)
                    .AddItems(data)
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text1", 15, (item) => $"1:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text2", 15, (item) => $"2:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text3", 15, (item) => $"3:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text4", 15, (item) => $"4:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text5", 15, (item) => $"5:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text6", 15, (item) => $"6:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text7", 15, (item) => $"7:{item.MyText}", maxslidinglines: 2)
                    .AddColumn("My Text8", 15, (item) => $"8:{item.MyText}", maxslidinglines: 2)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            PromptPlus.Widgets.DoubleDash($"TableMultiSelect -with custom answer", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                    .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled" }, true)
                    .AddItems(data)
                    .TextSelector(x => $"{x.Id}:{x.MyText.Replace(Environment.NewLine, " ")}".Trim())
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text", 20, (item) => item.MyText, maxslidinglines: 2)
                    .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            PromptPlus.Widgets.DoubleDash("TableMultiSelect - with history", extraLines: 1);
            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("SampleTableMultiSelect")
                .AddHistory(JsonSerializer.Serialize<MyTable>(data[3]))
                .Save();

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Descripion Table")
                    .AddItems(data)
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text", 20, (item) => item.MyText, maxslidinglines: 2)
                    .EqualItems((item1, item2) => item1.Id == item2.Id)
                    .TextSelector(x => $"{x.Id}:{x.MyText.Replace(Environment.NewLine, " ")}".Trim())
                    .EnabledHistory("SampleTableSelector")
                    .PageSize(5)
                    .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("SampleTableMultiSelect")
                .Remove();

            PromptPlus.Widgets.DoubleDash($"TableMulSelect - with custom colors", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);
                
            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                    .AddItems(data)
                    .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled" }, true)
                    .AddColumn("Id", 10, (item) => item.Id.ToString())
                    .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                    .AddColumn("My Text", 20, (item) => item.MyText, maxslidinglines: 2)
                    .Styles(TableStyles.Lines, Color.Red)
                    .Styles(TableStyles.TableContent, Color.Yellow)
                    .Styles(TableStyles.TableHeader, Color.Blue)
                    .Styles(TableStyles.TableTitle, Color.Cyan1)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - FilterByColumns(Contains) enabled('F4')", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                   .AddItems(CreateItems(17))
                   .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled" }, true)
                   .AddColumn("Id", 10, (item) => item.Id.ToString())
                   .AddColumn("Date", 20, (item) => (item.MyDate ?? DateTime.Now).ToString("G"))
                   .AddColumn("My Text", 20, (item) => item.MyText, maxslidinglines: 2)
                   .Filter(FilterMode.Contains, true)
                   .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");
        }
    }
}