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

            PromptPlus.Widgets.DoubleDash($"Table - Autofill custom colors)", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);


            PromptPlus.Widgets.Table<MyTable>()
                .AddItems(data)
                .AutoFill(0, 80)
                .Styles(TableStyles.Lines, Color.Red)
                .Styles(TableStyles.TableContent, Color.Yellow)
                .Styles(TableStyles.TableHeader, Color.Blue)
                .Styles(TableStyles.TableTitle, Color.Cyan1)
                .AddFormatType<DateTime>(FmtDate)
                .Show();

            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Table - with with column definition)", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            PromptPlus.Widgets.Table<MyTable>()
                .AddItems(data)
                .AddColumn(field: (item) => item.Id, width: 10)
                .AddColumn(field: (item) => item.MyDate!, width: 15/*,alignment: Alignment.Center*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text: {arg}", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text1: {arg}", title: $"Mytext1", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.ComplexCol, width: 20, format: (arg) => $"{((MyComplexCol)arg).Id}:{((MyComplexCol)arg).Name}")
                .AddColumn(field: (item) => item.ComplexCol.Name, width: 10)
                .AddFormatType<DateTime>(FmtDate)
                .Show();
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash($"Table - Hide Headers)", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

            PromptPlus.Widgets.Table<MyTable>()
                .HideHeaders()
                .AddItems(data)
                .AutoFill(0, 80)
                .AddFormatType<DateTime>(FmtDate)
                .Show();
            PromptPlus.Console.WriteLine("");

            var layout = Enum.GetValues<TableLayout>().Cast<TableLayout>();

            foreach (var item in layout)
            {
                PromptPlus.Widgets.DoubleDash($"Table - Autofill layout({item})", DashOptions.DoubleBorder, style: Color.Yellow, extraLines:1);

                PromptPlus.Widgets.Table<MyTable>()
                    .Layout(item)
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .AddFormatType<DateTime>(FmtDate)
                    .Show();
                PromptPlus.Console.WriteLine("");

                PromptPlus.Widgets.DoubleDash($"Table - Autofill layout({item}) with Separator Rows", DashOptions.DoubleBorder, style: Color.Yellow, extraLines: 1);

                PromptPlus.Widgets.Table<MyTable>()
                    .SeparatorRows()
                    .Layout(item)
                    .AddItems(data)
                    .AutoFill(0, 80)
                    .AddFormatType<DateTime>(FmtDate)
                    .Show();
                PromptPlus.Console.WriteLine("");
            }

            PromptPlus.Controls.KeyPress("Press any key")
                .Options(cfg => cfg.HideOnAbort(true).ShowTooltip(false).EnabledAbortKey(false))
                .Run();
        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}