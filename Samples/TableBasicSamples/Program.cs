using System.ComponentModel;
using System.Globalization;
using System.IO.Pipes;
using PPlus;
using PPlus.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace TableBasicSamples
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
            public required DateTime MyDate { get; set; }
            public required MyComplexCol ComplexCol { get; set; }

        }



        static void Main(string[] args)
        {
            static MyTable[] CreateItems()
            {
                return new MyTable[]
                {
                    new MyTable { Id = 1, MyDate = new DateTime().AddDays(1), MyText = "Test1", ComplexCol = new MyComplexCol("C1") },
                    new MyTable { Id = 2, MyDate = new DateTime().AddDays(2), MyText = "Test2 very very very very very very very very  long", ComplexCol = new MyComplexCol("C2")},
                    new MyTable { Id = 3, MyDate = new DateTime().AddDays(3), MyText = $"Test3 linha1{Environment.NewLine}Test3 linha2" ,ComplexCol = new MyComplexCol("C3")},
                    new MyTable { Id = 4, MyDate = new DateTime().AddDays(1), MyText = "Test4" ,ComplexCol = new MyComplexCol("C4")},
                    new MyTable { Id = 5, MyDate = new DateTime().AddDays(1), MyText = "Test5" ,ComplexCol = new MyComplexCol("C5")}
                };
            }

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            var data = CreateItems();

            var tbl = PromptPlus.Table<MyTable>("Select Column","Descripion Table")
                //.Config(cfg => cfg.ShowOnlyExistingPagination(true))
                //.HideHeaders()
                //.HideSelectorRow()
                //.PageSize(3)
                //.Default(data[1])
                .FilterByColumns(FilterMode.Contains, 1, 4)
                .EnableColumnsNavigation()
                .Layout(TableLayout.SingleGridFull)
                .Title("Test", titleMode: TableTitleMode.InRow)
                .WithSeparatorRows()
                .AddItems(data)
                .AddItem(new MyTable { Id = 6, MyDate = new DateTime().AddDays(1), MyText = "Test6 disabled", ComplexCol = new MyComplexCol("C6") }, true)
                .AddColumn(field: (item) => item.Id, width: 10)
                .AddColumn(field: (item) => item.MyDate, width: 15/*,alignment: Alignment.Center*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text: {arg}", maxslidinglines: 5/*, textcrop:true*/)
                .AddColumn(field: (item) => item.ComplexCol, width: 20, format: (arg) => $"{((MyComplexCol)arg).Id}:{((MyComplexCol)arg).Name}")
                .AddColumn(field: (item) => item.ComplexCol.Name, width: 10)
                .AutoFit(1,2)
                .AddFormatType<DateTime>(FmtDate)
                .AddFormatType<int>(FmtInt)
                .EnabledInteractionUser(
                    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();
            if (!tbl.IsAborted)
            { 
            }
        }

        private static string FmtInt(object arg)
        {
            var value = (int)arg;
            return value.ToString();
        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}