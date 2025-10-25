// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;

namespace ConsoleTableMultiSelectSamples
{
    internal class Program
    {
        internal class MyComplexCol(string value)
        {
            public string Id { get; } = Guid.NewGuid().ToString()[..8];
            public string Name { get; } = value;
        }

        internal class MyTable
        {
            public int Id { get; set; }
            public required string MyText { get; set; }
            public DateTime? MyDate { get; set; }
            public required MyComplexCol ComplexCol { get; set; }
        }


        internal class MyTableManyCols
        {
            public int Id { get; set; }
            public required string MyText { get; set; }
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

        internal static MyTable[] CreateItems(int max)
        {
            var result = new List<MyTable>();
            var flag = false;
            result.Add(new MyTable { Id = 0, MyDate = DateTime.Now, MyText = $"Test0 linha1{Environment.NewLine}Test0 linha2", ComplexCol = new MyComplexCol("C0") });
            for (int i = 1; i < max; i++)
            {
                flag = !flag;
                if (flag)
                {
                    result.Add(new MyTable { Id = i, MyText = $"Test{i}", ComplexCol = new MyComplexCol($"C{i}") });
                }
                else
                {
                    result.Add(new MyTable { Id = i, MyDate = DateTime.Now.AddDays(i), MyText = $"Test{i} very very very very very very very very very very very very very very very long", ComplexCol = new MyComplexCol($"C{i}") });
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

            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - Autofill", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            var tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Descripion Table")
                        .AddItems(data)
                        .AutoFill(0, 80)
                        .AddFormatType<DateTime>(FmtDate)
                        .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content!.Length})");

            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - Autofill with custom answer", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Descripion Table")
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .TextSelector(x => $"{x.Id}:{x.MyText}")
                    .AddFormatType<DateTime>(FmtDate)
                        .Run();

            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - Autofill with custom colors", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .AddFormatType<DateTime>(FmtDate)
                    .Styles(TableStyles.Lines, Color.Red)
                    .Styles(TableStyles.TableContent, Color.Yellow)
                    .Styles(TableStyles.TableHeader, Color.Blue)
                    .Styles(TableStyles.TableTitle, Color.Cyan1)
                    .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - Autofill and FilterByColumns(Contains) enabled('F4') for column 2 and 3", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

             tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
                        .AddItems(CreateItems(17))
                        .FilterByColumns(FilterMode.Contains,true,2,3)
                        .AutoFill(0, 80)
                        .AddFormatType<DateTime>(FmtDate)
                        .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");


            PromptPlus.Widgets.DoubleDash("TableMultiSelect - Autofill with many columns");
            var newid = -1;

            var tblmany = PromptPlus.Controls.TableMultiSelect<MyTableManyCols>("Your Prompt : ", "Description Table")
                 .Interaction(new MyTableManyCols[5], (_,ctrl) =>
                 {
                     newid++;
                     ctrl.AddItem(new MyTableManyCols() { Id = newid, MyText = "x" });
                 })
                 .AutoFill(10)
                 .AddFormatType<DateTime>(FmtDate)
                 .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {tblmany.IsAborted}, Value({tblmany.Content?.Length})");

            PromptPlus.Widgets.DoubleDash($"TableMultiSelect - Column definition", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            tbl = PromptPlus.Controls.TableMultiSelect<MyTable>("Your Prompt : ", "Description Table")
              .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled", ComplexCol = new MyComplexCol($"C{data.Length}") }, true)
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

            PromptPlus.Console.WriteLine($"IsAborted : {tbl.IsAborted}, Value({tbl.Content?.Length})");

        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}