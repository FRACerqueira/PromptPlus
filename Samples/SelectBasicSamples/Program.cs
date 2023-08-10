using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace SelectSamples
{
    internal enum MyEnum
    {
        None,
        [Display(Name = "option one")]
        Opc1,
        [Display(Name = "option two")]
        Opc2,
        [Display(Name = "option three")]
        Opc3
    }


    internal class Program
    {

        private static (string City,string other)[] MyCities()
        {
            return new[]
            {
                ("Seattle","any"),
                ("London","any"),
                ("Tokyo","any"),
                ("New York","any"),
                ("Singapore","any"),
                ("Shanghai","any"),
            };
        }

        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:Select - basic usage");

            var sel = PromptPlus.Select<string>("Select")
                .AddItem("Seattle")
                .AddItem("London")
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();

            if (!sel.IsAborted)
            {
                PromptPlus.WriteLine($"You selected item is {sel.Value}");
            }

            PromptPlus.DoubleDash("Control:Select - PageSize");
            PromptPlus.Select<string>("Select")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .PageSize(3)
                .Run();

            PromptPlus.DoubleDash("Control:Select - FilterType : StartsWith");
            PromptPlus.Select<string>("Select")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .FilterType(FilterMode.StartsWith)
                .Run();

            PromptPlus.DoubleDash("Control:Select - FilterType : Contains");
            PromptPlus.Select<string>("Select")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .FilterType(FilterMode.Contains)
                .Run();

            PromptPlus.DoubleDash("Control:Select - FilterType : AutoSelect");
            PromptPlus.Select<string>("Select","Press 'T' to view this feature, Tokyo will be auto-selected (only one starting with T)")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .FilterType(FilterMode.StartsWith)
                .AutoSelect()
                .Run();

            PromptPlus.DoubleDash("Control:Select - Diabled Item");
            PromptPlus.Select<string>("Select", "London is disabled")
                .AddItem("Seattle")
                .AddItem("London", true)
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();

            PromptPlus.DoubleDash("Control:Select - Using Enum");
            PromptPlus.Select<StyleControls>("Select", "Style regions/state of controls")
                .Run();

            PromptPlus.DoubleDash("Control:Select - Using Enum with display attributes");
            var sel1 = PromptPlus.Select<MyEnum>("Select")
                .Run();
            if (!sel1.IsAborted)
            {
                PromptPlus.WriteLine($"You selected item is {sel1.Value}");
            }

            PromptPlus.DoubleDash("Control:Select - Using Interaction");
            PromptPlus.Select<string>("Select")
                .Interaction(MyCities(),(ctrl,item) => 
                { 
                    ctrl.AddItem(item.City);
                })
                .Run();


            PromptPlus.DoubleDash("For other basic features below see - input samples (same behavior)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLine(". [yellow]Default[/] - InputBasicSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}