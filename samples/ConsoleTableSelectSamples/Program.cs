// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;
using System.Text.Json;

namespace ConsoleTableSelectSamples
{
    internal class Program
    {
        static MyTable? Fixeddata;

        internal class MyComplexCol()
        {
            public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
            public string Name { get; set; } = string.Empty;
        }

        internal class MyTable
        {
            public MyTable()
            {
                ComplexCol = new MyComplexCol();
            }
            public int Id { get; set; }
            public string MyText { get; set; } = string.Empty;
            public DateTime? MyDate { get; set; }
            public MyComplexCol ComplexCol { get; set; }
        }


        internal class MyTableManyCols
        {
            public int Id { get; set; }
            public string MyText { get; set; } = string.Empty;
            public string D01 { get; set; } = new string('x', 20);
            public string D02 { get; set; } = new string('x', 20);
            public string D03 { get; set; } = new string('x', 20);
            public string D04 { get; set; } = new string('x', 20);
            public string D05 { get; set; } = new string('x', 20);
            public string D06 { get; set; } = new string('x', 20);
            public string D07 { get; set; } = new string('x', 20);
            public string D08 { get; set; } = new string('x', 20);
            public string D09 { get; set; } = new string('x', 20);
            public string D10 { get; set; } = new string('x', 20);
            public string D11 { get; set; } = new string('x', 20);
            public string D12 { get; set; } = new string('x', 20);
            public string D13 { get; set; } = new string('x', 20);
            public string D14 { get; set; } = new string('x', 20);
            public string D15 { get; set; } = new string('x', 20);
            public string D16 { get; set; } = new string('x', 20);
            public string D17 { get; set; } = new string('x', 20);
            public string D18 { get; set; } = new string('x', 20);
            public string D19 { get; set; } = new string('x', 20);
            public string D20 { get; set; } = new string('x', 20);
            public string D21 { get; set; } = new string('x', 20);
            public string D22 { get; set; } = new string('x', 20);
            public string D23 { get; set; } = new string('x', 20);
            public string D24 { get; set; } = new string('x', 20);
            public string D25 { get; set; } = new string('x', 20);
            public string D26 { get; set; } = new string('x', 20);
            public string D27 { get; set; } = new string('x', 20);
            public string D28 { get; set; } = new string('x', 20);
            public string D29 { get; set; } = new string('x', 20);
            public string D30 { get; set; } = new string('x', 20);
            public string D31 { get; set; } = new string('x', 20);
        }

        internal static void CreateFixedItems()
        {
            Fixeddata = new MyTable { Id = 0, MyDate = DateTime.Now, MyText = $"Test0 linha1{Environment.NewLine}Test0 linha2", ComplexCol = new MyComplexCol { Name = "C0" }};
        }

        internal static MyTable[] CreateItems(int max)
        {
            var result = new List<MyTable>();
            var flag = false;
            result.Add(Fixeddata);
            for (int i = 1; i < max; i++)
            {
                flag = !flag;
                if (flag)
                {
                    result.Add(new MyTable { Id = i, MyText = $"Test{i}", ComplexCol = new MyComplexCol{ Name = $"C{i}" } });
                }
                else
                {
                    result.Add(new MyTable { Id = i, MyDate = DateTime.Now.AddDays(i), MyText = $"Test{i} very very very very very very very very very very very very very very very long", ComplexCol = new MyComplexCol { Name = $"C{i}" } });
                }
            }
            return [.. result];
        }

        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            CreateFixedItems();

            var data = CreateItems(5);

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash($"TableSelect - Autofill", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            var tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Descripion Table")
                        .AddItems(data)
                        .AutoFill(0, 80)
                        .AddFormatType<DateTime>(FmtDate)
                        .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");

            PromptPlus.Widgets.DoubleDash($"TableSelect - Autofill with custom answer", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Descripion Table")
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .TextSelector(x => $"{x.Id}:{x.MyText.Replace(Environment.NewLine," ")}".Trim())
                    .AddFormatType<DateTime>(FmtDate)
                        .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");


            PromptPlus.Widgets.DoubleDash("TableSelect - Autofill with default and history", extraLines: 1);

            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("SampleTableSelector")
                .AddHistory(JsonSerializer.Serialize<MyTable>(data[3]))
                .Save();

            tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Descripion Table")
                        .AddItems(data)
                        .AutoFill(0, 80)
                        .AddFormatType<DateTime>(FmtDate)
                        .EqualItems((item1,item2) => item1.Id == item2.Id)
                        .TextSelector(x => $"{x.Id}:{x.MyText.Replace(Environment.NewLine, " ")}".Trim())
                        .Default(Fixeddata!, true) //Default value but not selected because of history 
                        .EnabledHistory("SampleTableSelector")
                        .PageSize(5)
                        .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");


            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("SampleTableSelector")
                .Remove();


            PromptPlus.Widgets.DoubleDash($"TableSelect - Autofill with custom colors", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Description Table")
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .AddFormatType<DateTime>(FmtDate)
                    .Styles(TableStyles.Lines, Color.Red)
                    .Styles(TableStyles.TableContent, Color.Yellow)
                    .Styles(TableStyles.TableHeader, Color.Blue)
                    .Styles(TableStyles.TableTitle, Color.Cyan1)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");


            PromptPlus.Widgets.DoubleDash($"TableSelect - Autofill and FilterByColumns(Contains) enabled('F4') for column 2 and 3", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

             tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Description Table")
                        .AddItems(CreateItems(17))
                        .FilterByColumns(FilterMode.Contains,true,2,3)
                        .AutoFill(0, 80)
                        .AddFormatType<DateTime>(FmtDate)
                        .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");


            PromptPlus.Widgets.DoubleDash("TableSelect - Autofill with many columns");
            var newid = -1;

            var tblmany = PromptPlus.Controls.TableSelect<MyTableManyCols>("Your Prompt : ", "Description Table")
                 .Interaction(new MyTableManyCols[5], (_,ctrl) =>
                 {
                     newid++;
                     ctrl.AddItem(new MyTableManyCols() { Id = newid, MyText = "x" });
                 })
                 .AutoFill(10)
                 .AddFormatType<DateTime>(FmtDate)
                 .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tblmany.IsAborted}, Value({tblmany.Content?.Id})");


            PromptPlus.Widgets.DoubleDash($"TableSelect - Column definition", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableSelect<MyTable>("Your Prompt : ", "Description Table")
                .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled", ComplexCol = new MyComplexCol { Name = $"C{data.Length}" } }, true)
                .AddItems(data)
                .AddColumn(field: (item) => item.Id, width: 10)
                .AddColumn(field: (item) => item.MyDate!, width: 15/*,alignment: Alignment.Center*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text: {arg}", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text1: {arg}", title: $"Mytext1", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text2: {arg}", title: $"Mytext2", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text3: {arg}", title: $"Mytext3", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text4: {arg}", title: $"Mytext4", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text5: {arg}", title: $"Mytext5", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text8: {arg}", title: $"Mytext8", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text9: {arg}", title: $"Mytext9", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text10: {arg}", title: $"Mytext10", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.ComplexCol, width: 20, format: (arg) => $"{((MyComplexCol)arg).Id}:{((MyComplexCol)arg).Name}")
                .AddColumn(field: (item) => item.ComplexCol.Name, width: 10)
                .AddFormatType<DateTime>(FmtDate)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Id})");
        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}