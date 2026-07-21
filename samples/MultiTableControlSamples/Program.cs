// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace MultiTableControlSamples;

// -- Data models --------------------------------------------------------------
// Product: used in most sections (8 columns, enough to trigger horizontal scroll)
internal sealed record Product(
    int     Id,
    string  Name,
    string  Category,
    decimal Price,
    int     Stock,
    bool    Available,
    string  Origin,
    string  Notes);

// Employee: used in the HorizontalScroll sections (12 columns)
internal sealed record Employee(
    int     Id,
    string  FirstName,
    string  LastName,
    string  Department,
    string  Position,
    string  Email,
    string  Phone,
    string  HireDate,
    decimal Salary,
    string  Location,
    bool    Active,
    string  Notes);

// -- Static datasets ----------------------------------------------------------
internal static class Program
{
    private static readonly Product[] s_products =
    [
        new(1,  "Notebook Pro",    "Electronics",  1_299.99m,  45,  true,  "Taiwan",       "High-performance laptop"),
        new(2,  "Wireless Mouse",  "Peripherals",     29.90m, 320,  true,  "China",        "Ergonomic design"),
        new(3,  "USB-C Hub",       "Peripherals",     49.50m, 210,  true,  "China",        "7-in-1 multiport"),
        new(4,  "4K Monitor",      "Electronics",    599.00m,  28,  true,  "South Korea",  "HDR display"),
        new(5,  "Mechanical Kbd",  "Peripherals",    149.90m,  95,  true,  "USA",          "Clicky switches"),
        new(6,  "Web Cam HD",      "Peripherals",     79.00m, 180,  true,  "China",        "1080p 30fps"),
        new(7,  "Desk Lamp",       "Office",          39.90m, 500,  true,  "Germany",      "LED dimmable"),
        new(8,  "Legacy Adapter",  "Discontinued",     9.99m,   3,  false, "China",        "No longer produced"),
        new(9,  "Tablet Stand",    "Office",          24.90m, 410,  true,  "Brazil",       "Adjustable angle"),
        new(10, "Headset 7.1",     "Audio",          199.90m,  60,  true,  "Japan",        "Surround sound"),
    ];

    private static readonly Employee[] s_employees =
    [
        new(1,  "Alice",   "Smith",    "Engineering", "Senior Dev",     "alice@co.io",  "+1-555-0101", "2019-03-11", 120_000m, "New York",     true,  "Tech lead"),
        new(2,  "Bob",     "Johnson",  "Engineering", "Mid Dev",        "bob@co.io",    "+1-555-0102", "2021-06-01",  85_000m, "Remote",       true,  "Backend specialist"),
        new(3,  "Carol",   "White",    "Design",      "UX Designer",    "carol@co.io",  "+1-555-0103", "2020-01-15",  95_000m, "San Francisco",true,  "Figma expert"),
        new(4,  "David",   "Brown",    "Sales",       "Account Exec",   "david@co.io",  "+1-555-0104", "2022-09-20",  70_000m, "Chicago",      true,  "Top performer"),
        new(5,  "Eve",     "Davis",    "HR",          "HR Manager",     "eve@co.io",    "+1-555-0105", "2018-07-30",  80_000m, "New York",     true,  "Culture champion"),
        new(6,  "Frank",   "Martinez", "Engineering", "Intern",         "frank@co.io",  "+1-555-0106", "2024-01-08",  30_000m, "Remote",       false, "On leave"),
        new(7,  "Grace",   "Lee",      "Finance",     "CFO",            "grace@co.io",  "+1-555-0107", "2015-04-22", 180_000m, "New York",     true,  "Board member"),
        new(8,  "Hank",    "Wilson",   "Marketing",   "Content Writer", "hank@co.io",   "+1-555-0108", "2023-03-14",  55_000m, "Austin",       true,  "SEO focused"),
    ];

    // -- Entry point ----------------------------------------------------------
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        PromptPlus.Console.Clear();

        // --------------------------------------------------------------------
        // 1 - Basic multi-table
        //     Demonstrates: AddColumn (formatter, width, alignment),
        //     AddItems, Default, DefaultMatchBy, Filter(Contains/Answer),
        //     ChangeDescription, TextSelector
        // --------------------------------------------------------------------
        ShowSection("1) Basic multi-table - columns, filter, description");

        var r1 = PromptPlus.Controls.MultiTable<Product>("Select products", "Check all desired items")
            // Id: fixed width 4, right-aligned, not filterable
            .AddColumn("Id",       x => x.Id,       width: 4,  alignment: ColumnAlignment.Right)
            // Name: auto width, left-aligned (default), filterable
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            // Category: centered
            .AddColumn("Category", x => x.Category, alignment: ColumnAlignment.Center)
            // Price: right-aligned, custom formatter
            .AddColumn("Price",    x => x.Price,    v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
            // Stock: right-aligned, fixed width 7
            .AddColumn("Stock",    x => x.Stock,    width: 7,  alignment: ColumnAlignment.Right)
            // Origin: auto width, filterable
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.Answer)
            .ChangeDescription(item => $"Category: {item.Category} | Origin: {item.Origin} | Available: {item.Available}")
            .Run();
        PrintResult(r1);

        // --------------------------------------------------------------------
        // 2 - AddItem with ischecked / disable + Range
        //     Demonstrates: AddItem(ischecked:true), AddItem(disable:true),
        //     Range(min, max) constraint
        // --------------------------------------------------------------------
        ShowSection("2) AddItem(ischecked/disable) + Range(2,4) - must check 2 to 4 items");

        var r2 = PromptPlus.Controls.MultiTable<Product>("Select 2 to 4 products", "Available items pre-checked")
            .AddColumn("Name",      x => x.Name)
            .AddColumn("Category",  x => x.Category)
            .AddColumn("Available", x => x.Available ? "Yes" : "No",
                alignment: ColumnAlignment.Center, width: 10)
            // Pre-check available items; disable the discontinued one
            .Interaction(s_products, (p, ctrl) =>
                ctrl.AddItem(p, ischecked: p.Available, disable: !p.Available))
            .Range(minvalue: 2, maxvalue: 4)
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r2);

        // --------------------------------------------------------------------
        // 3 - AddItems with ischecked flag
        //     Demonstrates: bulk pre-checking all items via AddItems(ischecked:true)
        //     and then using Range to require at least one unchecked at completion
        // --------------------------------------------------------------------
        ShowSection("3) AddItems(ischecked:true) - all rows start checked, Range(1)");

        var r3 = PromptPlus.Controls.MultiTable<Product>("Deselect products to exclude")
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products, ischecked: true)
            .Range(minvalue: 1)
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r3);

        // --------------------------------------------------------------------
        // 4 - PredicateSelected (bool overload)
        //     Demonstrates: restricting which rows can be toggled at all
        // --------------------------------------------------------------------
        ShowSection("4) PredicateSelected(bool) - only Electronics or Peripherals can be checked");

        var r4 = PromptPlus.Controls.MultiTable<Product>("Select Electronics/Peripherals products")
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products)
            .PredicateSelected(p => p.Category is "Electronics" or "Peripherals")
            .TextSelector(item => $"{item.Name} (${item.Price:N2})")
            .Run();
        PrintResult(r4);

        // --------------------------------------------------------------------
        // 5 - PredicateSelected (bool + message overload)
        //     Demonstrates: custom validation error message per rejected row
        // --------------------------------------------------------------------
        ShowSection("5) PredicateSelected(bool, message) - custom error message per item");

        var r5 = PromptPlus.Controls.MultiTable<Product>("Select products (stock > 50 only)")
            .AddColumn("Name",  x => x.Name)
            .AddColumn("Stock", x => x.Stock, alignment: ColumnAlignment.Right, width: 7)
            .AddItems(s_products)
            .PredicateSelected(p => p.Stock > 50
                ? (true, null)
                : (false, $"'{p.Name}' has only {p.Stock} units in stock (minimum: 51)."))
            .TextSelector(item => $"{item.Name} (stock: {item.Stock})")
            .Run();
        PrintResult(r5);

        // --------------------------------------------------------------------
        // 6 - PredicateSelectedAsync (bool + message overload)
        //     Demonstrates: async predicate with error message
        // --------------------------------------------------------------------
        ShowSection("6) PredicateSelectedAsync(bool, message) - async availability check");

        var r6 = PromptPlus.Controls.MultiTable<Product>("Select available products (async check)")
            .AddColumn("Name",      x => x.Name)
            .AddColumn("Available", x => x.Available ? "Yes" : "No",
                alignment: ColumnAlignment.Center, width: 10)
            .AddItems(s_products)
            .PredicateSelectedAsync(async p =>
            {
                await Task.Delay(1); // simulate async lookup
                return p.Available
                    ? (true, (string?)null)
                    : (false, $"'{p.Name}' is discontinued and cannot be selected.");
            })
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r6);

        // --------------------------------------------------------------------
        // 7 - Filter modes
        //     Demonstrates all FilterMode values and FilterTableMode values
        // --------------------------------------------------------------------
        ShowSection("7a) Filter: Contains + Answer (filter searches the answer text)");

        PromptPlus.Controls.MultiTable<Product>("Filter products - Contains / Answer")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.Answer)
            .TextSelector(p => p.Name)
            .Run();

        ShowSection("7b) Filter: StartsWith + ColumnFilters (each filterable column is searched)");

        PromptPlus.Controls.MultiTable<Product>("Filter products - StartsWith / ColumnFilters")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.StartsWith, FilterTableMode.ColumnFilters)
            .TextSelector(item => item.Name)
            .Run();

        ShowSection("7c) Filter: Contains + ColumnFilters (searches all filterable columns)");

        PromptPlus.Controls.MultiTable<Product>("Filter products - Contains / ColumnFilters")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddColumn("Notes",    x => x.Notes,    isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.ColumnFilters)
            .TextSelector(item => item.Name)
            .Run();

        // --------------------------------------------------------------------
        // 8 - LayoutMode variations
        //     Demonstrates: SingleBox (default), DoubleBox, SingleASCII,
        //     DoubleASCII, None
        // --------------------------------------------------------------------
        ShowSection("8a) LayoutMode: SingleBox (default - Unicode single-line box)");
        RunLayoutDemo(TableLayoutMode.SingleBox);

        ShowSection("8b) LayoutMode: DoubleBox (Unicode double-line box)");
        RunLayoutDemo(TableLayoutMode.DoubleBox);

        ShowSection("8c) LayoutMode: SingleASCII (single-line, ASCII characters only)");
        RunLayoutDemo(TableLayoutMode.SingleASCII);

        ShowSection("8d) LayoutMode: DoubleASCII (double-line, ASCII characters only)");
        RunLayoutDemo(TableLayoutMode.DoubleASCII);

        ShowSection("8e) LayoutMode: None (no border characters at all)");
        RunLayoutDemo(TableLayoutMode.None);

        // --------------------------------------------------------------------
        // 9 - HideElements (HideTable flags)
        //     Demonstrates each flag individually and a fully-stripped table
        // --------------------------------------------------------------------
        ShowSection("9a) HideElements: HideTable.None - all borders visible (default)");
        RunHideDemo(HideTable.None, "None - show everything");

        ShowSection("9b) HideElements: OuterBorder - hide the outer frame");
        RunHideDemo(HideTable.OuterBorder, "OuterBorder - no outer frame");

        ShowSection("9c) HideElements: Header - hide the entire header row");
        RunHideDemo(HideTable.Header, "Header - no header row");

        ShowSection("9d) HideElements: ColumnSeparator - hide vertical column dividers");
        RunHideDemo(HideTable.ColumnSeparator, "ColumnSeparator - no column dividers");

        ShowSection("9e) HideElements: RowSeparator - hide horizontal row dividers");
        RunHideDemo(HideTable.RowSeparator, "RowSeparator - no row dividers");

        ShowSection("9f) HideElements: OuterBorder | RowSeparator - minimal framed look");
        RunHideDemo(HideTable.OuterBorder | HideTable.RowSeparator, "OuterBorder+RowSeparator");

        ShowSection("9g) HideElements: all flags - borderless / flat table");
        RunHideDemo(
            HideTable.OuterBorder | HideTable.Header | HideTable.ColumnSeparator | HideTable.RowSeparator,
            "All hidden - completely flat");

        // --------------------------------------------------------------------
        // 10 - ChangeDescriptionAsync + TextSelectorAsync
        //      Demonstrates async callbacks for description and answer text
        // --------------------------------------------------------------------
        ShowSection("10) ChangeDescriptionAsync + TextSelectorAsync");

        var r10 = PromptPlus.Controls.MultiTable<Product>("Select products (async description + answer)")
            .AddColumn("Name",  x => x.Name)
            .AddColumn("Price", x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddColumn("Stock", x => x.Stock, alignment: ColumnAlignment.Right, width: 7)
            .AddItems(s_products)
            .ChangeDescriptionAsync(async p =>
            {
                await Task.Delay(1);
                return $"(async) {p.Notes} - Origin: {p.Origin}";
            })
            .TextSelectorAsync(async p =>
            {
                await Task.Delay(1);
                return $"[async] {p.Name} | ${p.Price:N2}";
            })
            .Run();
        PrintResult(r10);

        // --------------------------------------------------------------------
        // 11 - Interaction + InteractionAsync
        //      Demonstrates dynamic row loading via sync and async iteration
        // --------------------------------------------------------------------
        ShowSection("11) Interaction + InteractionAsync - dynamic row loading");

        var r11 = PromptPlus.Controls.MultiTable<Product>("Select products (loaded via interaction)")
            .AddColumn("Id",    x => x.Id,    width: 4,  alignment: ColumnAlignment.Right)
            .AddColumn("Name",  x => x.Name)
            .AddColumn("Price", x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            // Sync: load first 5 products, pre-check them
            .Interaction(s_products.Take(5), (p, ctrl) => ctrl.AddItem(p, ischecked: true))
            // Async: load the remaining products (disable unavailable ones)
            .InteractionAsync(s_products.Skip(5), async (p, ctrl) =>
            {
                await Task.Delay(1);
                ctrl.AddItem(p, disable: !p.Available);
            })
            .ChangeDescription(p => p.Notes)
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r11);

        // --------------------------------------------------------------------
        // 12 - Default + DefaultMatchBy + EnabledHistory + UseDefaultHistory
        //      Demonstrates persisting the checked set across runs
        // --------------------------------------------------------------------
        ShowSection("12) Default + DefaultMatchBy + EnabledHistory + UseDefaultHistory (run twice!)");

        var r12 = PromptPlus.Controls.MultiTable<Product>(
                "Select products (history enabled - run twice!)")
            .AddColumn("Id",   x => x.Id,   width: 4, alignment: ColumnAlignment.Right)
            .AddColumn("Name", x => x.Name, isFilterable: true)
            .AddItems(s_products)
            .DefaultMatchBy((a, b) => a.Id == b.Id)
            .Default([s_products[0], s_products[2]])
            .EnabledHistory("multitable-product-history")
            .UseDefaultHistory()
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r12);

        // --------------------------------------------------------------------
        // 13 - ViewOnly + PageSize
        //      Demonstrates: read-only display mode with pre-checked rows
        // --------------------------------------------------------------------
        ShowSection("13) ViewOnly + Default + PageSize(4) - display-only, checkboxes read-only");

        var r13 = PromptPlus.Controls.MultiTable<Product>(
                "Product catalogue (view only)", "Press Esc or Enter to exit")
            .AddColumn("Id",        x => x.Id,       width: 4,  alignment: ColumnAlignment.Right)
            .AddColumn("Name",      x => x.Name)
            .AddColumn("Category",  x => x.Category)
            .AddColumn("Price",     x => x.Price,    v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddColumn("Stock",     x => x.Stock,    alignment: ColumnAlignment.Right, width: 7)
            .AddColumn("Available", x => x.Available ? "Yes" : "No",
                alignment: ColumnAlignment.Center, width: 10)
            .AddItems(s_products)
            // Pre-check available items so they appear checked in view-only mode
            .Default(s_products.Where(p => p.Available))
            .DefaultMatchBy((a, b) => a.Id == b.Id)
            .PageSize(4)
            .ViewOnly()
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r13);

        // --------------------------------------------------------------------
        // 14 - HorizontalScroll: Full (12-column Employee table)
        //      Demonstrates: wide table, HorizontalScrollMode.Full preview
        // --------------------------------------------------------------------
        ShowSection("14) HorizontalScroll: Full - viewport shifts, next col previewed");

        BuildEmployeeTable("Select employees (HorizontalScroll.Full)", HorizontalScrollMode.Full)
            .Run();

        // --------------------------------------------------------------------
        // 15 - HorizontalScroll: Column (one column at a time)
        // --------------------------------------------------------------------
        ShowSection("15) HorizontalScroll: Column - navigates one column at a time");

        BuildEmployeeTable("Select employees (HorizontalScroll.Column)", HorizontalScrollMode.Column)
            .Run();

        // --------------------------------------------------------------------
        // 16 - Custom Styles (MultiTableStyles)
        //      Demonstrates: overriding individual style regions with Styles()
        // --------------------------------------------------------------------
        ShowSection("16) Custom Styles - override BorderLines, Header, Selected, Disabled");

        var r16 = PromptPlus.Controls.MultiTable<Product>("Select products (custom styles)")
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .Interaction(s_products, (p, ctrl) => ctrl.AddItem(p, disable: !p.Available))
            .Styles(MultiTableStyles.BorderLines,    new Style(Color.Silver, Color.Black))
            .Styles(MultiTableStyles.HeaderText,     new Style(Color.Cyan,   Color.Black))
            .Styles(MultiTableStyles.SelectedCell,   new Style(Color.Black,  Color.Cyan))
            .Styles(MultiTableStyles.UnselectedCell, new Style(Color.White,  Color.Black))
            .Styles(MultiTableStyles.DisabledRow,    new Style(Color.Silver, Color.Black))
            .TextSelector(item => item.Name)
            .Run();
        PrintResult(r16);
    }

    // -- Helpers --------------------------------------------------------------

    // Reusable: same 3-column table, only LayoutMode changes (section 8)
    private static void RunLayoutDemo(TableLayoutMode mode)
    {
        PromptPlus.Controls.MultiTable<Product>($"LayoutMode.{mode}")
            .LayoutMode(mode)
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products.Take(4))
            .TextSelector(item => item.Name)
            .Run();
    }

    // Reusable: same 3-column table, only HideElements changes (section 9)
    private static void RunHideDemo(HideTable hide, string label)
    {
        PromptPlus.Controls.MultiTable<Product>($"HideElements: {label}")
            .HideElements(hide)
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products.Take(4))
            .TextSelector(item => item.Name)
            .Run();
    }

    // Reusable: 12-column Employee table for horizontal scroll demos (sections 14-15)
    private static IMultiTableControl<Employee> BuildEmployeeTable(
        string prompt, HorizontalScrollMode scroll)
    {
        return PromptPlus.Controls.MultiTable<Employee>(prompt, "12 columns - use \u2190 \u2192 to scroll columns")
            .HorizontalScroll(scroll)
            .PageSize(5)
            .AddColumn("Id",         e => e.Id,         width: 4,  alignment: ColumnAlignment.Right)
            .AddColumn("First Name", e => e.FirstName,  width: 12, isFilterable: true)
            .AddColumn("Last Name",  e => e.LastName,   width: 12, isFilterable: true)
            .AddColumn("Department", e => e.Department, width: 14)
            .AddColumn("Position",   e => e.Position,   width: 16)
            .AddColumn("Email",      e => e.Email,      width: 20)
            .AddColumn("Phone",      e => e.Phone,      width: 14)
            .AddColumn("Hire Date",  e => e.HireDate,   width: 11, alignment: ColumnAlignment.Center)
            .AddColumn("Salary",     e => e.Salary,     v => $"${v:N0}", width: 10,
                alignment: ColumnAlignment.Right)
            .AddColumn("Location",   e => e.Location,   width: 15)
            .AddColumn("Active",     e => e.Active ? "Yes" : "No", width: 7,
                alignment: ColumnAlignment.Center)
            .AddColumn("Notes",      e => e.Notes,      width: 22)
            .Interaction(s_employees, (emp, ctrl) =>
                ctrl.AddItem(emp, ischecked: emp.Active, disable: !emp.Active))
            .DefaultMatchBy((a, b) => a.Id == b.Id)
            .Filter(FilterMode.Contains, FilterTableMode.ColumnFilters)
            .ChangeDescription(e => $"{e.FirstName} {e.LastName} | {e.Department} - {e.Notes}")
            .TextSelector(e => $"{e.FirstName} {e.LastName}");
    }

    private static void ShowSection(string title)
    {
        PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
    }

    private static void PrintResult(ResultPrompt<Product[]> result)
    {
        if (result.IsAborted)
        {
            PromptPlus.Console.WriteLine("  [Aborted]", new Style(Color.Silver, Color.Black));
        }
        else if (result.Content.Length == 0)
        {
            PromptPlus.Console.WriteLine("  [No items selected]", new Style(Color.Silver, Color.Black));
        }
        else
        {
            PromptPlus.Console.WriteLine(
                $"  Selected ({result.Content.Length}): " +
                string.Join(", ", result.Content.Select(p => p.Name)),
                new Style(Color.Green, Color.Black));
        }
        PromptPlus.Console.WriteLine("");
    }

    private static void PrintResult(ResultPrompt<Employee[]> result)
    {
        if (result.IsAborted)
        {
            PromptPlus.Console.WriteLine("  [Aborted]", new Style(Color.Silver, Color.Black));
        }
        else if (result.Content.Length == 0)
        {
            PromptPlus.Console.WriteLine("  [No employees selected]", new Style(Color.Silver, Color.Black));
        }
        else
        {
            PromptPlus.Console.WriteLine(
                $"  Selected ({result.Content.Length}): " +
                string.Join(", ", result.Content.Select(e => $"{e.FirstName} {e.LastName}")),
                new Style(Color.Green, Color.Black));
        }
        PromptPlus.Console.WriteLine("");
    }
}
