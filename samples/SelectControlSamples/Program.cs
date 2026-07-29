// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace SelectControlSamples
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
            PromptPlus.Config.SufixAfterPrompt = string.Empty;
            PromptPlus.Console.Clear();


            const string historyDefaultKey = "SampleSelector.DefaultHistory";
            const string historyUseDefaultKey = "SampleSelector.UseDefaultHistory";

            // Ensure reproducible history-based scenarios.
            PromptPlus.Controls.History(historyDefaultKey).Remove();
            PromptPlus.Controls.History(historyUseDefaultKey).Remove();

            ShowSection("1) Basic Select with enum type");

            var resultenum = PromptPlus.Controls.Select<MyEnum>("Select : ")
                .Run();
            PrintSelectionResult(resultenum);

            ShowSection("2) Default value + history");

            // Pre-load persisted history used by this sample.
            PromptPlus.Controls.History(historyDefaultKey)
                .AddHistory(JsonSerializer.Serialize<string>("Item 2"))
                .Save();

            var resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddItem($"Item 1 {new string('x', 150)}zh")
                .AddItem("Item 1")
                .AddItem("Item 2")
                .AddItem("Item 3")
                .AddItem("Item 4")
                .AddItem("Item 5")
                .AddItem("Item 6")
                .AddItem("Item 7")
                .Default("Item 3", true) // history can override default when useDefaultHistory=true
                .EnableHistory(historyDefaultKey)
                .PageSize(5)
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("3) Filter:StartsWith + AutoSelect");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "Press letter or 'T' to Tokyo will be auto-selected (only one starting with T)")
                .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"])
                .Filter(FilterMode.StartsWith)
                .AutoSelect()
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("4) ExtraInfo (sync)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddItem("Seattle")
                .AddItem("London")
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .ExtraInfo(x => $"Length: {x.Length}")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("5) Disabled items");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "London and Seattle is disabled")
                .AddItem("Seattle", true)
                .AddItem("London", true)
                .AddItem("Tokyo")
                .AddItem("New York")
                .AddItem("Singapore")
                .AddItem("Shanghai")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("6) Custom type + Interaction + DefaultMatchBy + ChangeDescription");
            var resultclass = PromptPlus.Controls.Select<(int id, string City, string other)>("Select : ")
                .Interaction(MyCities(), (item, ctrl) =>
                {
                    ctrl.AddItem(item);
                })
                .TextSelector(item => item.City)
                .ChangeDescription(item => $"current other info: {item.other}")
                .DefaultMatchBy((item1, item2) => item1.id == item2.id)
                .Default(new(4, "New York", "any4"))
                .Run();
            PrintSelectionResult(resultclass);

            ShowSection("7) Separators (single, double and custom char)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ")
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
            resultstring = PromptPlus.Controls.Select<string>("Which cities would you like to visit? ")
                 .AddGroupedItems("North America", ["Seattle", "Boston", "New York"])
                 .AddGroupedItems("Asia", ["Tokyo", "Singapore", "Shanghai"])
                 .AddItem("South America (Any)")
                 .AddSeparator()
                 .AddItem("Europe (Any)")
                 .Filter(FilterMode.Contains)
                 .Run();
            PrintSelectionResult(resultstring);

            ShowSection("9) Grouped items + HideTipGroup");
            resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddGroupedItem("America", "Seattle")
                .AddGroupedItem("America", "New York")
                .AddGroupedItem("Asia", "Tokyo")
                .AddGroupedItem("Asia", "Singapore")
                .HideTipGroup()
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("10) PredicateSelected (bool)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddItems(["Seattle", "London", "Tokyo"])
                .PredicateSelected(city => city != "London")
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("11) PredicateSelected (message)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddItems(["Seattle", "London", "Tokyo"])
                .PredicateSelected(city => city == "Tokyo" ? (true, null) : (false, "Only Tokyo can be selected"))
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("12) Custom styles + options");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "Custom style and options")
                .AddItems(["Seattle", "London", "Tokyo"])
                .Styles(SelectStyles.Prompt, Color.Yellow)
                .Styles(SelectStyles.Description, Color.Red)
                .Styles(SelectStyles.Selected, Color.Blue)
                .Options(_ => { })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("13) ViewOnly mode");
            resultstring = PromptPlus.Controls.Select<string>("View only list: ")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .Default("Tokyo")
                .ViewOnly()
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("14) UseDefaultHistory");
            PromptPlus.Controls.History(historyUseDefaultKey)
                .AddHistory(JsonSerializer.Serialize<string>("Tokyo"))
                .Save();

            resultstring = PromptPlus.Controls.Select<string>("Select : ")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .EnableHistory(historyUseDefaultKey)
                .UseDefaultHistory()
                .Run();
            PrintSelectionResult(resultstring);

            // ---------- Additional scenarios ----------

            ShowSection("15) InteractionAsync + TextSelectorAsync + ExtraInfoAsync");
            resultclass = PromptPlus.Controls.Select<(int id, string City, string other)>("Select : ", "Loaded with InteractionAsync")
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
                .Default(new(2, "London", "any2"))
                .Run();
            PrintSelectionResult(resultclass);

            ShowSection("16) ChangeDescriptionAsync");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "Description changes asynchronously")
                .AddItems(["Seattle", "London", "Tokyo", "New York"])
                .ChangeDescriptionAsync(async item =>
                {
                    await Task.Delay(1);
                    return $"Current item: {item} (len={item.Length})";
                })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("17) PredicateSelectedAsync (bool)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "Only cities starting with 'S' are allowed")
                .AddItems(["Seattle", "London", "Tokyo", "Singapore", "Shanghai"])
                .PredicateSelectedAsync(async city =>
                {
                    await Task.Delay(1);
                    return city.StartsWith('S');
                })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("18) PredicateSelectedAsync (message)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "Only cities ending with 'o' are allowed")
                .AddItems(["Seattle", "London", "Tokyo", "Rio", "Cairo"])
                .PredicateSelectedAsync(async city =>
                {
                    await Task.Delay(1);
                    return city.EndsWith("o", StringComparison.OrdinalIgnoreCase)
                        ? (true, (string?)null)
                        : (false, "Only cities ending with 'o' can be selected.");
                })
                .Run();
            PrintSelectionResult(resultstring);

            ShowSection("19) History options (MinPrefixLength + MaxItems + Expiration + FilterType)");
            resultstring = PromptPlus.Controls.Select<string>("Select : ", "History appears after typing at least 2 chars")
                .AddItems(["Alpha", "Alpine", "Beta", "Gamma", "Delta"])
                .EnableHistory(historyDefaultKey, opt => opt
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
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
        }

        private static void PrintSelectionResult<T>(ResultPrompt<T> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

        }
    }
}
