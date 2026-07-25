// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace TableControlSamples;

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
        new(1,  "Notebook Pro",    "Electronics",  1_299.99m,  45,  true,  "Taiwan",  "High-performance laptop"),
        new(2,  "Wireless Mouse",  "Peripherals",     29.90m, 320,  true,  "China",   "Ergonomic design"),
        new(3,  "USB-C Hub",       "Peripherals",     49.50m, 210,  true,  "China",   "7-in-1 multiport"),
        new(4,  "4K Monitor",      "Electronics",    599.00m,  28,  true,  "South Korea", "HDR display"),
        new(5,  "Mechanical Kbd",  "Peripherals",    149.90m,  95,  true,  "USA",     "Clicky switches"),
        new(6,  "Web Cam HD",      "Peripherals",     79.00m, 180,  true,  "China",   "1080p 30fps"),
        new(7,  "Desk Lamp",       "Office",          39.90m, 500,  true,  "Germany", "LED dimmable"),
        new(8,  "Legacy Adapter",  "Discontinued",     9.99m,   3,  false, "China",   "No longer produced"),
        new(9,  "Tablet Stand",    "Office",          24.90m, 410,  true,  "Brazil",  "Adjustable angle"),
        new(10, "Headset 7.1",     "Audio",          199.90m,  60,  true,  "Japan",   "Surround sound"),
    ];

    private static readonly Employee[] s_employees =
    [
        new(1,  "Alice",   "Smith",    "Engineering",   "Senior Dev",      "alice@co.io",   "+1-555-0101", "2019-03-11", 120_000m, "New York",    true,  "Tech lead"),
        new(2,  "Bob",     "Johnson",  "Engineering",   "Mid Dev",         "bob@co.io",     "+1-555-0102", "2021-06-01", 85_000m,  "Remote",      true,  "Backend specialist"),
        new(3,  "Carol",   "White",    "Design",        "UX Designer",     "carol@co.io",   "+1-555-0103", "2020-01-15", 95_000m,  "San Francisco",true, "Figma expert"),
        new(4,  "David",   "Brown",    "Sales",         "Account Exec",    "david@co.io",   "+1-555-0104", "2022-09-20", 70_000m,  "Chicago",     true,  "Top performer"),
        new(5,  "Eve",     "Davis",    "HR",            "HR Manager",      "eve@co.io",     "+1-555-0105", "2018-07-30", 80_000m,  "New York",    true,  "Culture champion"),
        new(6,  "Frank",   "Martinez", "Engineering",   "Intern",          "frank@co.io",   "+1-555-0106", "2024-01-08", 30_000m,  "Remote",      false, "On leave"),
        new(7,  "Grace",   "Lee",      "Finance",       "CFO",             "grace@co.io",   "+1-555-0107", "2015-04-22", 180_000m, "New York",    true,  "Board member"),
        new(8,  "Hank",    "Wilson",   "Marketing",     "Content Writer",  "hank@co.io",    "+1-555-0108", "2023-03-14", 55_000m,  "Austin",      true,  "SEO focused"),
    ];

    // -- Entry point ----------------------------------------------------------
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        PromptPlus.Console.Clear();

        // --------------------------------------------------------------------
        // 1 - Basic table
        //     Demonstrates: AddColumn (formatter, width, alignment, overflow),
        //     AddItems, Filter(Contains/Answer),
        //     ChangeDescription
        // --------------------------------------------------------------------
        ShowSection("1) Basic table - columns, filter, description");

        var r1 = PromptPlus.Controls.Table<Product>("Select a product", "All column features in one table")
            // Id: fixed width 4, right-aligned, not filterable
            .AddColumn("Id",       x => x.Id,        width: 4,  alignment: ColumnAlignment.Right)
            // Name: auto width, left-aligned (default), filterable, ellipsis on overflow
            .AddColumn("Name",     x => x.Name, isFilterable: true)
            // Category: centered
            .AddColumn("Category", x => x.Category,  alignment: ColumnAlignment.Center)
            // Price: right-aligned, custom formatter
            .AddColumn("Price",    x => x.Price,     v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
            // Stock: right-aligned, fixed width 7
            .AddColumn("Stock",    x => x.Stock,     width: 7,  alignment: ColumnAlignment.Right)
            // Origin: auto width, filterable
            .AddColumn("Origin",   x => x.Origin,    isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.Answer)
            .ChangeDescription(item => $"Category: {item.Category} | Origin: {item.Origin} | Available: {item.Available}")
            .Run();
        PrintResult(r1);

        // --------------------------------------------------------------------
        // 2 - AddItem with disabled rows 
        //     Demonstrates: AddItem(disable:true), PredicateSelected returning bool
        // --------------------------------------------------------------------
        ShowSection("2) AddItem with disabled rows + PredicateSelected(bool)");

        var r2 = PromptPlus.Controls.Table<Product>("Select an available product")
            .AddColumn("Name",      x => x.Name)
            .AddColumn("Category",  x => x.Category)
            .AddColumn("Available", x => x.Available ? "Yes" : "No", alignment: ColumnAlignment.Center, width: 10)
            // Disabled row: Available == false
            .Interaction(s_products, (p, ctrl) => ctrl.AddItem(p, disable: !p.Available))
            // Only allow selecting items where Available == true
            .PredicateSelected(p => p.Available)
            .Run();
        PrintResult(r2);

        // --------------------------------------------------------------------
        // 3 - PredicateSelected (bool + message) - sync and async
        //     Demonstrates: PredicateSelected with custom error message,
        //     PredicateSelectedAsync with custom error message
        // --------------------------------------------------------------------
        ShowSection("3a) PredicateSelected(bool, message) - only Electronics or Peripherals");

        var r3a = PromptPlus.Controls.Table<Product>("Select product from Electronics/Peripherals")
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products)
            .TextSelector(item => $"{item.Name} (${item.Price:N2})")
            .PredicateSelected(p => p.Category is "Electronics" or "Peripherals"
                ? (true, null)
                : (false, $"Category '{p.Category}' is not allowed. Choose Electronics or Peripherals."))
            .Run();
        PrintResult(r3a);

        ShowSection("3b) PredicateSelectedAsync(bool, message) - only stock > 100");

        var r3b = PromptPlus.Controls.Table<Product>("Select product with stock > 100")
            .AddColumn("Name",  x => x.Name)
            .AddColumn("Stock", x => x.Stock, alignment: ColumnAlignment.Right, width: 8)
            .AddItems(s_products)
            .TextSelector(item => $"{item.Name} (${item.Price:N2})")
            .PredicateSelectedAsync(async p =>
            {
                await Task.Delay(1); // simulate async lookup
                return p.Stock > 100
                    ? (true, (string?)null)
                    : (false, $"Stock is {p.Stock}, must be > 100.");
            })
            .Run();
        PrintResult(r3b);

        // --------------------------------------------------------------------
        // 4 - Filter modes
        //     Demonstrates all FilterMode values (Contains, StartsWith, Disabled)
        //     and FilterTableMode values (Answer, ColumnFilters)
        // --------------------------------------------------------------------
        ShowSection("4a) Filter: Contains + Answer (filter input searches the answer text)");

        PromptPlus.Controls.Table<Product>("Search product - Contains / Answer")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.Answer)
            .TextSelector(p => p.Name)
            .Run();

        ShowSection("4b) Filter: StartsWith + ColumnFilters (filter searches all filterable columns)");

        PromptPlus.Controls.Table<Product>("Search product - StartsWith / ColumnFilters")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.StartsWith, FilterTableMode.ColumnFilters)
            .TextSelector(item => $"{item.Name}")
            .Run();

        ShowSection("4c) Filter: Contains + ColumnFilters (each filterable column is searched)");

        PromptPlus.Controls.Table<Product>("Search product - Contains / ColumnFilters")
            .AddColumn("Name",     x => x.Name,     isFilterable: true)
            .AddColumn("Category", x => x.Category, isFilterable: true)
            .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
            .AddColumn("Notes",    x => x.Notes,    isFilterable: true)
            .AddItems(s_products)
            .Filter(FilterMode.Contains, FilterTableMode.ColumnFilters)
            .TextSelector(item => $"{item.Name}")
            .Run();

        // --------------------------------------------------------------------
        // 5 - LayoutMode variations
        //     Demonstrates: SingleBox (default), DoubleBox, SingleASCII,
        //     DoubleASCII, None - same data, each layout mode
        // --------------------------------------------------------------------
        ShowSection("5a) LayoutMode: SingleBox (default - Unicode single-line box)");
        RunLayoutDemo(TableLayoutMode.SingleBox);

        ShowSection("5b) LayoutMode: DoubleBox (Unicode double-line box)");
        RunLayoutDemo(TableLayoutMode.DoubleBox);

        ShowSection("5c) LayoutMode: SingleASCII (single-line, ASCII characters only)");
        RunLayoutDemo(TableLayoutMode.SingleASCII);

        ShowSection("5d) LayoutMode: DoubleASCII (double-line, ASCII characters only)");
        RunLayoutDemo(TableLayoutMode.DoubleASCII);

        ShowSection("5e) LayoutMode: None (no border characters at all)");
        RunLayoutDemo(TableLayoutMode.None);

        // --------------------------------------------------------------------
        // 6 - HideElements (HideTable flags)
        //     Demonstrates each flag individually and a fully-stripped table.
        //     HideTable.None = show everything (default)
        //     Flags: OuterBorder, Header, ColumnSeparator, RowSeparator
        // --------------------------------------------------------------------
        ShowSection("6a) HideElements: HideTable.None - all borders visible (default)");
        RunHideDemo(HideTable.None, "None - show everything");

        ShowSection("6b) HideElements: OuterBorder - hide the outer frame");
        RunHideDemo(HideTable.OuterBorder, "OuterBorder - no outer frame");

        ShowSection("6c) HideElements: Header - hide the entire header row");
        RunHideDemo(HideTable.Header, "Header - no header row");

        ShowSection("6d) HideElements: ColumnSeparator - hide vertical column dividers");
        RunHideDemo(HideTable.ColumnSeparator, "ColumnSeparator - no column dividers");

        ShowSection("6e) HideElements: RowSeparator - hide horizontal row dividers");
        RunHideDemo(HideTable.RowSeparator, "RowSeparator - no row dividers");

        ShowSection("6f) HideElements: OuterBorder | RowSeparator - minimal framed look");
        RunHideDemo(HideTable.OuterBorder | HideTable.RowSeparator, "OuterBorder+RowSeparator");

        ShowSection("6g) HideElements: all flags - borderless / flat table");
        RunHideDemo(
            HideTable.OuterBorder | HideTable.Header | HideTable.ColumnSeparator | HideTable.RowSeparator,
            "All hidden - completely flat");

        // --------------------------------------------------------------------
        // 7 - ChangeDescriptionAsync + TextSelectorAsync
        //     Demonstrates async callbacks for both description and answer text
        // --------------------------------------------------------------------
        ShowSection("7) ChangeDescriptionAsync + TextSelectorAsync");

        var r7 = PromptPlus.Controls.Table<Product>("Select product (async description + answer)")
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
        PrintResult(r7);

        // --------------------------------------------------------------------
        // 8 - Interaction + InteractionAsync
        //     Demonstrates loading rows through sync and async iterations
        // --------------------------------------------------------------------
        ShowSection("8) Interaction + InteractionAsync - dynamic row loading");

        var r8 = PromptPlus.Controls.Table<Product>("Select product (loaded via interaction)")
            .AddColumn("Id",    x => x.Id,    width: 4, alignment: ColumnAlignment.Right)
            .AddColumn("Name",  x => x.Name)
            .AddColumn("Price", x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            // Sync: load first 5 products
            .Interaction(s_products.Take(5), (p, ctrl) => ctrl.AddItem(p))
            // Async: load remaining products (last one disabled)
            .InteractionAsync(s_products.Skip(5), async (p, ctrl) =>
            {
                await Task.Delay(1);
                ctrl.AddItem(p, disable: !p.Available);
            })
            .ChangeDescription(p => $"{p.Notes}")
            .TextSelector(item => $"{item.Name}")
            .Run();
        PrintResult(r8);

        // --------------------------------------------------------------------
        // 9 - Default + EnableHistory + UseDefaultHistory
        //     Demonstrates persisting the last selection across runs
        // --------------------------------------------------------------------
        ShowSection("9) Default + EnableHistory + UseDefaultHistory");

        var r9 = PromptPlus.Controls.Table<Product>("Select product (history enabled - run twice!)")
            .AddColumn("Id",   x => x.Id,   width: 4, alignment: ColumnAlignment.Right)
            .AddColumn("Name", x => x.Name, isFilterable: true)
            .AddItems(s_products)
            .DefaultMatchBy((a, b) => a.Id == b.Id)
            .Default(s_products[0], useDefaultHistory: true)
            .EnableHistory("table-product-history")
            .UseDefaultHistory()
            .TextSelector(item => $"{item.Name}")
            .Run();
        PrintResult(r9);

        // --------------------------------------------------------------------
        // 10 - HorizontalScroll: Full (preview of next column visible)
        //      Demonstrates: wide table (12 columns), HorizontalScrollMode.Full
        //      The next hidden column is rendered as a faded preview
        // --------------------------------------------------------------------
        ShowSection("10) HorizontalScroll: Full - viewport shifts, next col previewed");

        BuildEmployeeTable("Select employee (HorizontalScroll.Full)", HorizontalScrollMode.Full)
            .Run();

        // --------------------------------------------------------------------
        // 11 - HorizontalScroll: Column (one column at a time)
        // --------------------------------------------------------------------
        ShowSection("11) HorizontalScroll: Column - navigates one column at a time");

        BuildEmployeeTable("Select employee (HorizontalScroll.Column)", HorizontalScrollMode.Column)
            .Run();

        // --------------------------------------------------------------------
        // 12 - ViewOnly + PageSize
        //      Demonstrates: read-only display mode and custom page size
        // --------------------------------------------------------------------
        ShowSection("12) ViewOnly + PageSize(4) - display-only, 4 rows per page");

        var r12 = PromptPlus.Controls.Table<Product>("Product catalogue (view only)", "Press Esc or Enter to exit")
            .AddColumn("Id",       x => x.Id,       width: 4,  alignment: ColumnAlignment.Right)
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price,    v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddColumn("Stock",    x => x.Stock,    alignment: ColumnAlignment.Right, width: 7)
            .AddColumn("Available", x => x.Available ? "?" : "?", alignment: ColumnAlignment.Center, width: 10)
            .AddColumn("Notes", x => x.Notes, alignment: ColumnAlignment.Left)//, width: 20)
            .AddItems(s_products)
            .AddItem(new Product(11, "Gaming Chair", "Furniture", 299.99m, 15, true, "USA", "Ergonomic design" + new string('-', 320) + "xz"))
            .PageSize(4)
            .ViewOnly()
            .TextSelector(item => $"{item.Name}-{item.Notes}")
            .Run();
        PrintResult(r12);

        // --------------------------------------------------------------------
        // 13 - Custom Styles (TableStyles)
        //      Demonstrates: overriding individual style regions with Styles()
        // --------------------------------------------------------------------
        ShowSection("13) Custom Styles - override Borders, Header, Selected, Disabled");

        var r13 = PromptPlus.Controls.Table<Product>("Select product (custom styles)")
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .Interaction(s_products, (p, ctrl) => ctrl.AddItem(p, disable: !p.Available))
            .Styles(TableStyles.BorderLines,    new Style(Color.Silver,  Color.Black))
            .Styles(TableStyles.HeaderText,     new Style(Color.Cyan,    Color.Black))
            .Styles(TableStyles.SelectedCell,   new Style(Color.Black,   Color.Cyan))
            .Styles(TableStyles.UnselectedCell, new Style(Color.White,   Color.Black))
            .Styles(TableStyles.DisabledRow,    new Style(Color.Silver,  Color.Black))
            .TextSelector(item => $"{item.Name}")
            .Run();
        PrintResult(r13);
    }

    // -- Helpers --------------------------------------------------------------

    // Reusable: same 3-column table, only LayoutMode changes (section 5)
    private static void RunLayoutDemo(TableLayoutMode mode)
    {
        PromptPlus.Controls.Table<Product>($"LayoutMode.{mode}")
            .LayoutMode(mode)
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products.Take(4))
            .TextSelector(item => $"{item.Name} (${item.Price:N2})")
            .Run();
    }

    // Reusable: same 3-column table, only HideElements changes (section 6)
    private static void RunHideDemo(HideTable hide, string label)
    {
        PromptPlus.Controls.Table<Product>($"HideElements: {label}")
            .HideElements(hide)
            .AddColumn("Name",     x => x.Name)
            .AddColumn("Category", x => x.Category)
            .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
            .AddItems(s_products.Take(4))
            .TextSelector(item => $"{item.Name} (${item.Price:N2})")
            .Run();
    }

    // Reusable: 12-column Employee table for horizontal scroll demos (sections 10-11)
    private static ITableControl<Employee> BuildEmployeeTable(string prompt, HorizontalScrollMode scroll)
    {
        return PromptPlus.Controls.Table<Employee>(prompt, "12 columns - use ? ? to scroll columns")
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
            .AddColumn("Salary",     e => e.Salary,     v => $"${v:N0}", width: 10, alignment: ColumnAlignment.Right)
            .AddColumn("Location",   e => e.Location,   width: 15)
            .AddColumn("Active",     e => e.Active ? "Yes" : "No", width: 7, alignment: ColumnAlignment.Center)
            .AddColumn("Notes",      e => e.Notes,      width: 22)
            .Interaction(s_employees, (emp, ctrl) => ctrl.AddItem(emp, disable: !emp.Active))
            .DefaultMatchBy((a, b) => a.Id == b.Id)
            .Filter(FilterMode.Contains, FilterTableMode.ColumnFilters)
            .ChangeDescription(e => $"{e.FirstName} {e.LastName} | {e.Department} - {e.Notes}")
            .TextSelector(e => $"{e.FirstName} {e.LastName}");
    }

    private static void ShowSection(string title)
    {
        PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.DoubleBorderUpDown, 1);
    }

    private static void PrintResult<T>(ResultPrompt<TableResult<T>> result)
    {
        if (result.IsAborted)
        {
            PromptPlus.Console.WriteLine("  [Aborted]", new Style(Color.Silver, Color.Black));
        }
        else
        {
            PromptPlus.Console.WriteLine(
                $"  RowIndex={result.Content.RowIndex}  " +
                $"ColumnIndex={result.Content.ColumnIndex}  " +
                $"Value={result.Content.Value}",
                new Style(Color.Green, Color.Black));
        }
        PromptPlus.Console.WriteLine("");
    }
}
