// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace MultiSelectBasicSamples
{
    internal class Program
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
            [Display(Name = "option 7")]
            Opc7,
            [Display(Name = "option 8")]
            Opc8,
            [Display(Name = "option 9")]
            Opc9,
            [Display(Name = "option 10")]
            Opc10,
            [Display(Name = "option 11")]
            Opc11,
            [Display(Name = "option 12")]
            Opc12,
        }

        private static (string City, string other)[] MyCities()
        {
            return new[]
            {
                ("Seattle","any"),
                ("London","any"),
                ("Tokyo","any"),
                ("New York","any"),
                ("Singapore","any"),
                ("Shanghai","any"),
                ("other city 1","any"),
                ("other city 2","any"),
                ("other city 3","any"),
                ("other city 4","any"),
                ("other city 5","any"),
                ("other city 6","any"),
            };
        }
        static void Main(string[] args)
        {


            PromptPlus.WriteLine("Hello, World!");

            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.DoubleDash("Control:MultiSelect - basic usage with PageSize");
            var multsel = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                 .AddItems(new []{ "Seattle", "Boston", "New York", "Tokyo", "Singapore", "Shanghai" })
                 .PageSize(4)
                 .Run();

            if (!multsel.IsAborted)
            {
                foreach (var item in multsel.Value)
                {
                    PromptPlus.WriteLine($"You selected item is {item}");
                }
            }


            PromptPlus.DoubleDash("Control:MultiSelect - basic usage with group and AppendGroupOnDescription");
            multsel = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)")
                 .AddItem("Europe (Any)")
                 .AppendGroupOnDescription()
                 .Run();


            if (!multsel.IsAborted)
            {
                foreach (var item in multsel.Value)
                {
                    PromptPlus.WriteLine($"You selected item is {item}");
                }
            }

            PromptPlus.DoubleDash("Control:MultiSelect - FilterType : StartsWith");
            PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                 .AddItems(new[] { "Seattle", "Boston", "New York", "Tokyo", "Singapore", "Shanghai" })
                .FilterType(FilterMode.StartsWith)
                .Run();

            PromptPlus.DoubleDash("Control:MultiSelect - Diabled Item/selected item");
            PromptPlus.MultiSelect<string>("Which cities would you like to visit?", "London is disabled,Seattle disabled and selected,Tokyo is started selected")
                .AddItem("Seattle",true,true)
                .AddItem("London", true)
                .AddItem("Tokyo",false,true)
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();


            PromptPlus.DoubleDash("Control:MultiSelect - Using Enum");
            PromptPlus.MultiSelect<StyleControls>("MultiSelect", "Style regions/state of controls")
                .Run();

            PromptPlus.DoubleDash("Control:MultiSelect - Using Enum with display attributes");
            PromptPlus.MultiSelect<MyEnum>("SelMultiSelectect")
                .Run();

            PromptPlus.DoubleDash("Control:MultiSelect - Using Range(min,max) selected");
            PromptPlus.MultiSelect<string>("Select","Min.selected 2, max.selected 3")
                .Interaction(MyCities(), (ctrl, item) =>
                {
                    ctrl.AddItem(item.City);
                })
                .Range(2,3)
                .Run();

            PromptPlus.DoubleDash("For other basic features below see - input samples (same behavior)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}