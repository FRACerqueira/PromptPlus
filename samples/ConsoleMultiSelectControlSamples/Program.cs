// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using PromptPlusLibrary;

namespace ConsoleMultiSelectControlSamples
{
    internal class Program
    {
        private enum MyEnum
        {
            [Display(Order = 0)]
            None,
            [Display(Name = "option seven", Order = 7)]
            Op7,
            [Display(Name = "option one", Order = 1)]
            Opc1,
            [Display(Name = "option two", Order = 2)]
            Opc2,
            [Display(Name = "option three", Order = 3)]
            Opc3,
            [Display(Name = "option four", Order = 4)]
            Opc4,
            [Display(Name = "option five", Order = 5)]
            Opc5,
            [Display(Name = "option six", Order = 6)]
            Opc6,
        }

        internal static (int id, string City, string other)[] MyCities()
        {
            return
            [
                (1,"Seattle","any1"),
                (2,"London","any2"),
                (3,"Tokyo","any3"),
                (4,"New York","any4"),
                (5,"Singapore","any5"),
                (6,"Shanghai","any6"),
            ];
        }

        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample basic MultiSelector with enum type", extraLines: 1);

            var resultenum = PromptPlus.Controls.MultiSelect<MyEnum>("Select : ")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");


            PromptPlus.Widgets.DoubleDash("Sample basic MultiSelector with default and history", extraLines: 1);

            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("SampleMultiSelector")
                .AddHistory(JsonSerializer.Serialize<string[]>(["Item 2", "Item 3"]))
                .Save();

            var resultstring = PromptPlus.Controls.MultiSelect<string>("Select : ")
                .AddItem("Item 1")
                .AddItem("Item 2")
                .AddItem("Item 3")
                .AddItem("Item 4")
                .AddItem("Item 5")
                .AddItem("Item 6")
                .AddItem("Item 7")
                .Default(["Item 5"], true) //Default value but not selected because of history 
                .EnabledHistory("SampleMultiSelector")
                .PageSize(5)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");

            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("SampleMultiSelector")
                .Remove();

            PromptPlus.Widgets.DoubleDash("Sample MultiSelector with Filter:StartsWith");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select : ", "Press 'F4' + 'T' to view this feature.")
                .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"])
                .Filter(FilterMode.StartsWith)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");

            PromptPlus.Widgets.DoubleDash("Sample MultiSelector with Ranger");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select : ", "Min. 2, Max. 3")
                .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"])
                .Range(2, 3)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");


            PromptPlus.Widgets.DoubleDash("Sample Selector with disabled item");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select : ", "London and Seattle is disabled")
                .AddItem("Seattle", false,true)
                .AddItem("London", true,true)
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");

            PromptPlus.Widgets.DoubleDash("Sample MultiSelector with interaction and custom type");
            var resultclass = PromptPlus.Controls.MultiSelect<(int id, string City, string other)>("Select : ")
                .Interaction(MyCities(), (item, ctrl) =>
                {
                    ctrl.AddItem(item);
                })
                .TextSelector(item => item.City)
                .ChangeDescription(item => $"current other info: {item.other}")
                .EqualItems((item1, item2) => item1.id == item2.id)
                .Default([new(4, "New York", "any4")])
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");

            PromptPlus.Widgets.DoubleDash("Sample MultiSelector with separator");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select : ")
                .AddItem("Seattle")
                .AddItem("New York")
                .AddSeparator() //Default SeparatorLine : SeparatorLine.SingleLine
                .AddItem("Tokyo")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .AddSeparator(SeparatorLine.DoubleLine)
                .AddItem("London")
                .AddSeparator(SeparatorLine.UserChar, '*')
                .AddItem("Other city")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");

            PromptPlus.Widgets.DoubleDash("Sample Selector with group and Filter:Contains");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Which cities would you like to visit? ")
                 .AddGroupedItem("North America", "Seattle", false, true)
                 .AddGroupedItem("North America", "Boston")
                 .AddGroupedItem("North America", "New York")
                 .AddGroupedItems("Asia", ["Tokyo", "Singapore", "Shanghai"])
                 .AddItem("South America (Any)")
                 .AddSeparator()
                 .Filter(FilterMode.Contains)
                 .AddItem("Europe (Any)")
                 .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultenum.IsAborted}, Value: {resultenum.Content.Length} selected");
        }
    }
}
