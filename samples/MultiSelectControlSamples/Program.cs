// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace MultiSelectControlSamples
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Console.Clear();

            const string historyDefaultKey = "SampleMultiSelector.DefaultHistory";
            const string historyUseDefaultKey = "SampleMultiSelector.UseDefaultHistory";

            // Ensure reproducible history-based scenarios.
            PromptPlus.Controls.History(historyDefaultKey).Remove();
            PromptPlus.Controls.History(historyUseDefaultKey).Remove();

            ShowSection("1) Basic MultiSelect with enum type");

            var resultenum = PromptPlus.Controls.MultiSelect<MyEnum>("Select")
                .Run();
            PrintSelectionResult(resultenum);


            ShowSection("2) Default values + history");

            // Pre-load persisted history used by this sample.
            PromptPlus.Controls.History(historyDefaultKey)
                .AddHistory(JsonSerializer.Serialize<string[]>(["Item 2", "Item 3"]))
                .Save();

            var resultstring = PromptPlus.Controls.MultiSelect<string>("Select")
                .AddItem($"Item 1 {new string('x', 150)}zh")
                .AddItem("Item 2")
                .AddItem("Item 3")
                .AddItem("Item 4")
                .AddItem("Item 5")
                .AddItem("Item 6")
                .AddItem("Item 7")
                .Default(["Item 5"], true) // history can override default when useDefaultHistory=true
                .EnabledHistory(historyDefaultKey)
                .PageSize(5)
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("3) Filter mode: StartsWith");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Press 'T' to view this feature.")
                .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"])
                .Filter(FilterMode.StartsWith)
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("4) Range (Min=2, Max=3)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Min. 2, Max. 3")
                .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"])
                .Range(2, 3)
                .Run();
            PrintSelectionResult(resultstring);


            ShowSection("5) Disabled items");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "London and Seattle is disabled")
                .AddItem("Seattle", false,true)
                .AddItem("London", true,true)
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("6) Custom type + Interaction + DefaultMatchBy + ExtraInfo");
            var resultclass = PromptPlus.Controls.MultiSelect<(int id, string City, string other)>("Select")
                .Interaction(MyCities(), (item, ctrl) =>
                {
                    ctrl.AddItem(item);
                })
                .TextSelector(item => item.City)
                .DefaultMatchBy((item1, item2) => item1.id == item2.id)
                .Default([new(4, "New York", "any4")])
                .ExtraInfo(x => x.other)
                .Run();
            PrintSelectionResult(resultclass);

            ShowSection("7) Separators (single, double and custom char)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select")
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
            PrintSelectionResult(resultstring);

            ShowSection("8) Grouped items + Filter:Contains");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Which cities would you like to visit?")
                 .AddGroupedItem("North America", "Seattle", false, true)
                 .AddGroupedItem("North America", "Boston")
                 .AddGroupedItem("North America", "New York")
                 .AddGroupedItems("Asia", ["Tokyo", "Singapore", "Shanghai"])
                 .AddItem("South America (Any)")
                 .AddSeparator()
                 .Filter(FilterMode.Contains)
                 .AddItem("Europe (Any)")
                 .Run();
            PrintSelectionResult(resultstring);

            ShowSection("9) Grouped items + HideTipGroup");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select")
                .AddGroupedItem("America", "Seattle")
                .AddGroupedItem("America", "New York")
                .AddGroupedItem("Asia", "Tokyo")
                .AddGroupedItem("Asia", "Singapore")
                .HideTipGroup()
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("10) PredicateChecked (bool)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "London cannot be selected")
                .AddItems(["Seattle", "London", "Tokyo"])
                .PredicateChecked(city => city != "London")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("11) PredicateChecked (message)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Only Tokyo can be selected")
                .AddItems(["Seattle", "London", "Tokyo"])
                .PredicateChecked(city => city == "Tokyo" ? (true, null) : (false, "Only Tokyo can be selected"))
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("12) ChangeDescription (sync)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .ChangeDescription(item => $"current item: {item}")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("13) Custom styles + options");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Custom style and options")
                .AddItems(["Seattle", "London", "Tokyo"])
                .Styles(MultiSelectStyles.Prompt, Color.Yellow)
                .Styles(MultiSelectStyles.Description, Color.Red)
                .Styles(MultiSelectStyles.Selected, Color.Blue)
                .Options(ctx => { ctx.HideAfterFinish(); })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("14) ViewOnly mode");
            resultstring = PromptPlus.Controls.MultiSelect<string>("View only list")
                .AddItems(["Seattle", "London", "Tokyo", "New York"],true)
                .ViewOnly()
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("15) UseDefaultHistory");
            PromptPlus.Controls.History(historyUseDefaultKey)
                .AddHistory(JsonSerializer.Serialize<string[]>(["Tokyo", "London"]))
                .Save();

            resultstring = PromptPlus.Controls.MultiSelect<string>("Select")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .EnabledHistory(historyUseDefaultKey)
                .UseDefaultHistory()
                .Run();
            PrintSelectionResult(resultstring);

            // ---------- Additional scenarios not covered previously ----------

            ShowSection("16) Async interaction + TextSelectorAsync + ExtraInfoAsync");
            resultclass = PromptPlus.Controls.MultiSelect<(int id, string City, string other)>("Select", "Loaded with InteractionAsync")
                .InteractionAsync(MyCities(), async (item, ctrl) =>
                {
                    await Task.Delay(1);
                    ctrl.AddItem(item);
                })
                .TextSelectorAsync(async item =>
                {
                    await Task.Delay(1);
                    return item.City;
                })
                .ExtraInfoAsync(async item =>
                {
                    await Task.Delay(1);
                    return $"meta:{item.other}";
                })
                .DefaultMatchBy((a, b) => a.id == b.id)
                .Default([new(2, "London", "any2")])
                .Run();
            PrintSelectionResult(resultclass);

            ShowSection("17) ChangeDescriptionAsync");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Description changes asynchronously")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .ChangeDescriptionAsync(async item =>
                {
                    await Task.Delay(1);
                    return $"Current item: {item} (len={item.Length})";
                })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("18) PredicateCheckedAsync (message)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "Only cities starting with 'S' are allowed")
                .AddItems(["Seattle", "London", "Tokyo", "Singapore", "Shanghai"])
                .PredicateCheckedAsync(async city =>
                {
                    await Task.Delay(1);
                    return city.StartsWith('S')
                        ? (true, (string?)null)
                        : (false, "Only cities starting with 'S' can be selected.");
                })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("19) History options (MinPrefixLength + MaxItems + Expiration + FilterType)");
            resultstring = PromptPlus.Controls.MultiSelect<string>("Select", "History appears after typing at least 2 chars")
                .AddItems(["Alpha", "Alpine", "Beta", "Gamma", "Delta"])
                .EnabledHistory(historyDefaultKey, opt => opt
                    .MinPrefixLength(2)
                    .MaxItems(8)
                    .PageSize(3)
                    .FilterType(FilterMode.StartsWith)
                    .ExpirationTime(TimeSpan.FromDays(15)))
                .Run();
            PrintSelectionResult(resultstring);

            // Cleanup persisted sample history.
            PromptPlus.Controls.History(historyDefaultKey).Remove();
            PromptPlus.Controls.History(historyUseDefaultKey).Remove();
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintSelectionResult<T>(ResultPrompt<T[]> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content.Length} selected");
            if (!result.IsAborted && result.Content.Length > 0)
            {
                PromptPlus.Console.WriteLine($"Selected: {string.Join(", ", result.Content.Select(x => x?.ToString()))}");
            }
            PromptPlus.Console.WriteLine("");
        }
    }
}
