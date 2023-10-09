// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace SelectUserScopeSamples
{
    internal enum MyEnum
    {
        None,
        [Display(Name = "option 1")]
        Opc1,
        [Display(Name = "option 2")]
        Opc2,
        [Display(Name = "option 3")]
        Opc3,
        [Display(Name = "option 4")]
        Opc4,
        [Display(Name = "option 5")]
        Opc5,
        [Display(Name = "option 6")]
        Opc6,
    }

    internal class MyClass
    {
        public int Id { get; set; }
        public required string MyText { get; set; }
        public required string MyDesc { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsHide { get; set; }
    }

    internal class Program
    {
        private static IEnumerable<MyClass>? _datasample;
        static void Main()
        {
            _datasample = LoadData();
            PromptPlus.WriteLine("Hello, World!");

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:Select - Using class with AdderScope and OrderBy");
            var sel = PromptPlus.Select<MyClass>("Select")
                .AddItems(_datasample)
                .AddItemsTo(AdderScope.Disable, _datasample.Where(x => x.IsDisabled))
                .AddItemsTo(AdderScope.Remove, _datasample.Where(x => x.IsHide))
                .TextSelector(x => x.MyText)
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .ChangeDescription(x => x.MyDesc)
                .Default(_datasample.First(x => x.Id == 1))
                .OrderBy((item) => item.Id)
                .Run();

            PromptPlus.DoubleDash("Control:Select - Using enum with AdderScope");
            PromptPlus.Select<MyEnum>("Select")
                .AddItemsTo(AdderScope.Disable, MyEnum.None)
                .AddItemsTo(AdderScope.Remove, MyEnum.Opc1)
                .Run();
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }

        private static IEnumerable<MyClass> LoadData()
        {
            var aux = new List<MyClass>
            {
                new MyClass { Id = 4, MyText = "Text4", MyDesc="Text4 for id=4", IsDisabled = false, IsHide = false },
                new MyClass { Id = 5, MyText = "Text5", MyDesc="Text5 for id=5", IsDisabled = false, IsHide = false },
                new MyClass { Id = 6, MyText = "Text6", MyDesc="Text6 for id=6", IsDisabled = false, IsHide = true },
                new MyClass { Id = 1, MyText = "Text1", MyDesc="Text1 for id=1", IsDisabled = false, IsHide = false },
                new MyClass { Id = 0, MyText = "Text1", MyDesc="Text1 for id=0", IsDisabled = false, IsHide = false },
                new MyClass { Id = 2, MyText = "Text2", MyDesc="Text2 for id=2", IsDisabled = true, IsHide = false },
                new MyClass { Id = 3, MyText = "Text3", MyDesc="Text3 for id=3", IsDisabled = true, IsHide = false },
            };
            return aux;
        }
    }
}