using System.Globalization;
using System.IO.Pipes;
using PPlus;
using PPlus.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace TableBasicSamples
{
    internal class Program
    {
        internal class MyTable
        {
            public int Id { get; set; }
            public required string MyText { get; set; }
            public required DateTime MyDate { get; set; }
        }



        static void Main(string[] args)
        {
            static MyTable[] CreateItem()
            {
                return new MyTable[]
                {
                    new MyTable { Id = 1, MyDate = new DateTime().AddDays(1), MyText = "Test1" },
                    new MyTable { Id = 2, MyDate = new DateTime().AddDays(2), MyText = "Test2 very very very very very very very very  long" },
                    new MyTable { Id = 3, MyDate = new DateTime().AddDays(3), MyText = "Test3" }
                };
            }

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.Table<MyTable>()
                .Title("Test",titleMode:TableTitleMode.InRow)
                .WithSeparatorRows()
                .AddItems(CreateItem())
                .AddColumn((item) => item.Id, 10)
                .AddColumn((item) => item.MyDate, 15)
                .AddColumn((item) => item.MyText, 20, format:(arg) => $"Text: {arg}")
                .AddFormatType<DateTime>(FmtDate)
                .AddFormatType<int>(FmtInt)
                .EnabledInteractionUser(
                    (item, col) => $"Current ID : {item.Id}. [yellow]Current col {col}[/]",
                    (item, col) => $"[green]Selected ID : {item.Id}. Current col {col}[/]")
                .Run();

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