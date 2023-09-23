// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

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
            public DateTime? MyDate { get; set; }
            public required MyComplexCol ComplexCol { get; set; }

        }



        static void Main(string[] args)
        {
            static MyTable[] CreateItems()
            {
                var result = new List<MyTable>();
                var flag = false;
                result.Add(new MyTable { Id = 0, MyDate = new DateTime(), MyText = $"Test0 linha1{Environment.NewLine}Test3 linha2", ComplexCol = new MyComplexCol("C0") });
                for (int i = 1; i < 15; i++)
                {
                    flag = !flag;
                    if (flag)
                    {
                        result.Add(new MyTable { Id = i, MyText = $"Test{i}", ComplexCol = new MyComplexCol($"C{i}") });
                    }
                    else
                    {
                        result.Add(new MyTable { Id = i, MyDate = new DateTime().AddDays(i), MyText = $"Test{i} very very very very very very very very  long", ComplexCol = new MyComplexCol($"C{i}") });
                    }
                }
                return result.ToArray();
            }

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            var data = CreateItems();

            var tbl = PromptPlus.Table<MyTable>("Select Column","Descripion Table")
                //.Config(cfg => cfg.ShowOnlyExistingPagination(true))
                //.HideHeaders()
                //.HideSelectorRow()
                //.Default(data[1])
                //.ChangeDescription((item,row,col) => $"{item.ComplexCol.Name}")
                //.PageSize(6)
                .EnableColumnsNavigation()
                .WithSeparatorRows()
                .FilterByColumns(FilterMode.Contains, 1, 4)
                .Layout(TableLayout.SingleGridFull)
                .Title("Test", titleMode: TableTitleMode.InRow)
                .AddItems(data)
                .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled", ComplexCol = new MyComplexCol($"C{data.Length}") }, true)
                .AddColumn(field: (item) => item.Id, width: 10)
                .AddColumn(field: (item) => item.MyDate!, width: 15/*,alignment: Alignment.Center*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text: {arg}", maxslidinglines: 5/*, textcrop:true*/)
                .AddColumn(field: (item) => item.ComplexCol, width: 20, format: (arg) => $"{((MyComplexCol)arg).Id}:{((MyComplexCol)arg).Name}")
                .AddColumn(field: (item) => item.ComplexCol.Name, width: 10)
                //.AutoFit(1,2)
                .AddFormatType<DateTime>(FmtDate)
                .AddFormatType<int>(FmtInt)
                //.EnabledInteractionUser(
                //    selectedTemplate: (item, row, col) => $"Current ID : {item.Id}. [yellow]Current row {row}, Current col {col}[/]",
                //    finishTemplate: (item, row, col) => $"[green]Selected ID : {item.Id}. Current row {row}, Current col {col}[/]")
                .Run();
            if (!tbl.IsAborted)
            { 
            }

            Console.ReadKey();
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