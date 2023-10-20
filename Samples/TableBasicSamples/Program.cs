// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace TableSamples
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

            PromptPlus.DoubleDash("Control:Table - Autofill");

            var tbl = PromptPlus.Table<MyTable>("Your Prompt", "Descripion Table")
                .AddItems(data)
                .AutoFill(0,80)
                .AddFormatType<DateTime>(FmtDate)
                .Run();
            if (!tbl.IsAborted)
            {
            }

            PromptPlus.KeyPress("Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();


            PromptPlus.DoubleDash("Control:Table - Autofill custom colors");

            PromptPlus.Table<MyTable>("Your Prompt", "Descripion Table")
                .AddItems(data)
                .Config(cfg =>
                 {
                    cfg.ApplyStyle(StyleControls.Lines, Style.Default.Foreground(Color.Red));
                    cfg.ApplyStyle(StyleControls.Disabled, Style.Default.Foreground(Color.Magenta1));
                    cfg.ApplyStyle(StyleControls.Selected, Style.Default.Foreground(Color.Aquamarine1));
                 })
                .Styles(TableStyle.Content, Style.Default.Foreground(Color.Yellow))
                .Styles(TableStyle.Header, Style.Default.Foreground(Color.Blue))
                .Styles(TableStyle.Title, Style.Default.Foreground(Color.Cyan1))
                .AutoFill(0, 80)
                .AddFormatType<DateTime>(FmtDate)
                .Run();

            PromptPlus.KeyPress("Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();

            PromptPlus.DoubleDash("Control:Table - with with column definition");
            PromptPlus.Table<MyTable>("Your Prompt", "Descripion Table")
                .Title("Test", titleMode: TableTitleMode.InRow)
                .AddItem(new MyTable { Id = data.Length, MyText = $"Test{data.Length} disabled", ComplexCol = new MyComplexCol($"C{data.Length}") })
                .AddItems(data)
                .AddColumn(field: (item) => item.Id, width: 10)
                .AddColumn(field: (item) => item.MyDate!, width: 15/*,alignment: Alignment.Center*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text: {arg}", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.MyText, width: 20, format: (arg) => $"Text1: {arg}", title: $"Mytext1", maxslidinglines: 2/*, textcrop:true*/)
                .AddColumn(field: (item) => item.ComplexCol, width: 20, format: (arg) => $"{((MyComplexCol)arg).Id}:{((MyComplexCol)arg).Name}")
                .AddColumn(field: (item) => item.ComplexCol.Name, width: 10)
                .AddFormatType<DateTime>(FmtDate)
                .Run();

            PromptPlus.KeyPress("Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();


            var typelayout = Enum.GetValues(typeof(TableLayout));
            var typetit = Enum.GetValues(typeof(TableTitleMode));
            //Title mode
            foreach (var itemt in typetit)
            {
                var tm = (TableTitleMode)Enum.Parse(typeof(TableTitleMode), itemt.ToString()!);
                //Title Grid mode
                foreach (var iteml in typelayout)
                {
                    var hideheaders = true;
                    //hideheaders (true/false)
                    for (int h = 0; h < 2; h++)
                    {
                        hideheaders = !hideheaders;
                        var seprow = true;
                        //SeparatorRows (true/false)
                        for (int i = 0; i < 2; i++)
                        {
                            seprow = !seprow;
                            PromptPlus.DoubleDash($"Autofill Layout({iteml}), Title({itemt}), sep.row({seprow}), hide headers({hideheaders})", style: Style.Default.Foreground(Color.Yellow));
                            var lt = (TableLayout)Enum.Parse(typeof(TableLayout), iteml.ToString()!);
                            PromptPlus.Table<MyTable>("")
                                .Title("Test", Alignment.Center, tm)
                                .SeparatorRows(seprow)
                                .HideHeaders(hideheaders)
                                .Layout(lt)
                                .AddItems(data)
                                .AutoFill()
                                .AddFormatType<DateTime>(FmtDate)
                                .Run(); PromptPlus.KeyPress("Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();
                        }
                    }
                }
            }

            PromptPlus.KeyPress("End sample!, Press any key", cfg => cfg.ShowTooltip(false).HideAfterFinish(true)).Run();

        }

        private static string FmtDate(object arg)
        {
            var value = (DateTime)arg;
            return value.ToString("G");
        }
    }
}