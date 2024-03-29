﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace TableSelectSamples
{
    internal class Program
    {
        internal class MyComplexCol
        {
            public MyComplexCol(string value)
            {
                Name = value;
            }
            public string Id { get; } = Guid.NewGuid().ToString()[..8];
            public string Name { get; }
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
            return result.ToArray();
        }

        static void Main()
        {

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            var data = CreateItems(5);

            PromptPlus.DoubleDash("Control:TableSelect Autofill basic usage");

            var tbl = PromptPlus.TableSelect<MyTable>("Your Prompt", "Descripion Table")
                .AddItems(data)
                .AutoFill(0, 80)
                .AddFormatType<DateTime>(FmtDate)
                .Templates(
                    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();
            if (!tbl.IsAborted)
            {
            }

            PromptPlus.DoubleDash("Control:TableSelect Autofill HideSelectorRow");

            PromptPlus.TableSelect<MyTable>("Your Prompt", "Descripion Table")
                .AddItems(data)
                .HideSelectorRow()
                .AutoFill(0, 80)
                .AddFormatType<DateTime>(FmtDate)
                .Templates(
                    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();

            PromptPlus.DoubleDash("Control:TableSelect Autofill custom colors");

            PromptPlus.TableSelect<MyTable>("Your Prompt", "Descripion Table")
                .AddItems(data)
                .Styles(TableSelectStyle.Lines, Style.Default.Foreground(Color.Red))
                .Styles(TableSelectStyle.Disabled, Style.Default.Foreground(Color.Magenta1))
                .Styles(TableSelectStyle.Selected, Style.Default.Foreground(Color.Aquamarine1))
                .Styles(TableSelectStyle.TableContent, Style.Default.Foreground(Color.Yellow))
                .Styles(TableSelectStyle.TableHeader, Style.Default.Foreground(Color.Blue))
                .Styles(TableSelectStyle.TableTitle, Style.Default.Foreground(Color.Cyan1))
                .AutoFill(0, 80)
                .AddFormatType<DateTime>(FmtDate)
                .Templates(
                    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();


            PromptPlus.DoubleDash("Control:TableSelect Autofill with many columns and RowNavigation(Default)");
            var newid = -1;
            PromptPlus.TableSelect<MyTableManyCols>("Your Prompt", "Descripion Table")
                 .Interaction<object>(new Array[5], (ctrl, _) =>
                 {
                     newid++;
                     ctrl.AddItem(new MyTableManyCols() { Id = newid, MyText = "x" });
                 })
                 .AutoFill(10)
                 .FilterByColumns(FilterMode.Contains)
                 .AddFormatType<DateTime>(FmtDate)
                 .Templates(
                     selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                     finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                 .Run();

            PromptPlus.DoubleDash("Control:TableSelect Autofill with many columns and ColumnsNavigation");
            PromptPlus.TableSelect<MyTableManyCols>("Your Prompt", "Descripion Table")
                 .Interaction<object>(new Array[5], (ctrl, _) =>
                 {
                     ctrl.AddItem(new MyTableManyCols() { MyText = "x" });
                 })
                 .AutoFill(10)
                 .AddFormatType<DateTime>(FmtDate)
                 .ColumnsNavigation()
                 .Templates(
                     selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                     finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                 .Run();

            PromptPlus.DoubleDash("Control:TableSelect with with column definition");
            PromptPlus.TableSelect<MyTable>("Your Prompt", "Descripion Table")
                .Title("Test", titleMode: TableTitleMode.InRow)
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
                .Templates(
                    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();

            PromptPlus.KeyPress("End sample!, Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();

        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}