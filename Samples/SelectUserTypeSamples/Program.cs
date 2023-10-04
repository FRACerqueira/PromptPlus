// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;

namespace SelectUserTypeSamples
{
    internal class MyClass
    {
        public int Id { get; set; }
        public required string MyText { get; set; }
        public required string MyDesc { get; set; }
    }

    internal class Program
    {
        private static IEnumerable<MyClass>? _datasample;
        static void Main()
        {
            _datasample = LoadData();
            Console.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:Select - Using class");
            var sel = PromptPlus.Select<MyClass>("Select")
                .AddItems(_datasample)
                .TextSelector(x => x.MyText)
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .ChangeDescription(x => x.MyDesc)
                .Default(_datasample.First(x => x.Id == 1))
                .Run();
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }

        private static IEnumerable<MyClass> LoadData()
        {
            var aux = new List<MyClass>
            {
                new MyClass { Id = 0, MyText = "Text1", MyDesc="Text1 for id=0"},
                new MyClass { Id = 1, MyText = "Text1", MyDesc="Text1 for id=1"},
                new MyClass { Id = 2, MyText = "Text2", MyDesc="Text2 for id=2"},
                new MyClass { Id = 3, MyText = "Text3", MyDesc="Text3 for id=3"},
                new MyClass { Id = 4, MyText = "Text4", MyDesc="Text4 for id=4"},
                new MyClass { Id = 5, MyText = "Text5", MyDesc="Text5 for id=5"},
                new MyClass { Id = 6, MyText = "Text6", MyDesc="Text6 for id=6"}
            };
            return aux;
        }
    }
}