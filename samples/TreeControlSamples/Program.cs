// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace TreeControlSamples
{
    /// <summary>
    /// A simple user item used to populate the generic tree. Marked [Serializable] so that
    /// EnabledHistory can persist the selected value (see sample 8).
    /// </summary>
    [Serializable]
    public sealed class Node
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Info { get; set; }

        public override string ToString() => Name;
    }

    internal static class Program
    {
        private static void Main()
        {
            ConfigureCulture();
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // Sample data reused by all sections: a small company org-chart.
            //
            //   Company
            //   ├── Engineering
            //   │   ├── Backend
            //   │   │   ├── API
            //   │   │   └── Database
            //   │   └── Frontend
            //   │       ├── Web
            //   │       └── Mobile
            //   ├── Sales
            //   │   ├── EMEA
            //   │   └── APAC
            //   └── HR
            Node root       = new() { Id = 1,  Name = "Company",     Info = "root" };
            Node eng        = new() { Id = 2,  Name = "Engineering", Info = "dept" };
            Node backend    = new() { Id = 3,  Name = "Backend",     Info = "team" };
            Node api        = new() { Id = 4,  Name = "API",         Info = "service" };
            Node database   = new() { Id = 5,  Name = "Database",    Info = "service" };
            Node frontend   = new() { Id = 6,  Name = "Frontend",    Info = "team" };
            Node web        = new() { Id = 7,  Name = "Web",         Info = "app" };
            Node mobile     = new() { Id = 8,  Name = "Mobile",      Info = "app" };
            Node sales      = new() { Id = 9,  Name = "Sales",       Info = "dept" };
            Node emea       = new() { Id = 10, Name = "EMEA",        Info = "region" };
            Node apac       = new() { Id = 11, Name = "APAC",        Info = "region" };
            Node hr         = new() { Id = 12, Name = "HR",          Info = "dept" };

            // Each section builds a fresh control (samples are independent).
            //
            // --------------------------------------------------------------------------------
            ShowSection("1) Basic usage - Root, TextSelector, DefaultMatchBy, AddLast, nested AddLast");
            var t1 = PromptPlus.Controls.Tree<Node>(
                    "Pick any item",
                    "Right/+ expand, Left/- collapse, Enter to confirm")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t1Eng = t1.AddLast(eng);
            var t1Back = t1Eng.AddLast(backend);
            t1Back.AddLast(api);
            t1Back.AddLast(database);
            var t1Front = t1Eng.AddLast(frontend);
            t1Front.AddLast(web);
            t1Front.AddLast(mobile);
            t1.AddLast(sales);
            t1.AddLast(hr);

            PrintResult(t1.Run());

            // --------------------------------------------------------------------------------
            ShowSection("2) AddFirst / AddAfter / AddBefore - controlling sibling order");
            var t2 = PromptPlus.Controls.Tree<Node>("Notice the order of the top-level nodes")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            // Build: [Engineering, Sales]
            var t2Eng = t2.AddLast(eng);
            var t2Sales = t2.AddLast(sales);
            // Insert HR BEFORE Sales   -> [Engineering, HR, Sales]
            t2.AddBefore(t2Sales, hr);
            // Prepend Frontend as first-level so it appears at the very top
            // -> [Frontend, Engineering, HR, Sales]
            var t2Front = t2.AddFirst(frontend);
            // Insert Backend AFTER Engineering (as a first-level sibling)
            // -> [Frontend, Engineering, Backend, HR, Sales]
            t2.AddAfter(t2Eng, backend);

            // Nested children on Frontend so users can actually expand it.
            t2Front.AddLast(web);
            t2Front.AddLast(mobile);

            PrintResult(t2.Run());

            // --------------------------------------------------------------------------------
            ShowSection("3) ExtraInfo, PathSeparator ('.') and ShowFullPath");
            var t3 = PromptPlus.Controls.Tree<Node>(
                    "Answer shows full dotted path",
                    "Use the full-path hotkey to toggle short/long answer")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)             // Rendered next to each node label
                .PathSeparator('.')                 // Company.Engineering.Backend.API
                .ShowFullPath(true)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            BuildFullTree(t3, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t3.Run());

            // --------------------------------------------------------------------------------
            ShowSection("4) SelectLeafOnly - Enter is blocked on containers");
            var t4 = PromptPlus.Controls.Tree<Node>(
                    "Try Enter on a container: it will be rejected",
                    "Only leaves (API, Database, Web, ...) can be confirmed")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .SelectLeafOnly();

            BuildFullTree(t4, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t4.Run());

            // --------------------------------------------------------------------------------
            ShowSection("5) Default - pre-select a deep item (the tree auto-expands to it)");
            var t5 = PromptPlus.Controls.Tree<Node>("The 'Database' leaf is pre-selected")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default(database);

            BuildFullTree(t5, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t5.Run());

            // --------------------------------------------------------------------------------
            ShowSection("6) PredicateSelected - block confirmation with a custom rule");
            var t6 = PromptPlus.Controls.Tree<Node>(
                    "Only 'service' items can be confirmed",
                    "Try to select 'HR' or 'Sales' -> you'll see an error message")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PredicateSelected(n =>
                    n.Info == "service"
                        ? (true, null)
                        : (false, $"'{n.Name}' is a {n.Info}, not a service."));

            BuildFullTree(t6, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t6.Run());

            // --------------------------------------------------------------------------------
            ShowSection("7) PredicateSelectedAsync + ViewOnly");
            // First: async predicate that pretends to hit a service. Only 'app' items pass.
            var t7a = PromptPlus.Controls.Tree<Node>(
                    "Async predicate: only 'app' items pass",
                    "Enter on Web/Mobile succeeds, others get rejected")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PredicateSelectedAsync(async n =>
                {
                    await Task.Delay(1);
                    return n.Info == "app";
                });

            BuildFullTree(t7a, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t7a.Run());

            // Then: ViewOnly - user navigates freely; Enter returns the Default value (null here, no Default set).
            var t7b = PromptPlus.Controls.Tree<Node>(
                    "ViewOnly: navigate freely, Enter returns default value (null - no Default set)",
                    "Press ESC to abort or Enter to finish returning null")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .ViewOnly();

            BuildFullTree(t7b, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t7b.Run());

            // --------------------------------------------------------------------------------
            ShowSection("8) EnabledHistory - remembers the last selected node");
            const string historyKey = "SampleTree.History";
            PromptPlus.Controls.History(historyKey).Remove();
            try
            {
                // First run: user picks something; it gets serialized into history.
                var t8First = PromptPlus.Controls.Tree<Node>("Pick a node (will be remembered)")
                    .Root(root)
                    .TextSelector(n => n.Name)
                    .DefaultMatchBy((a, b) => a.Id == b.Id)
                    .EnabledHistory(historyKey);
                BuildFullTree(t8First, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
                PrintResult(t8First.Run());

                // Second run: history is used as the default (tree auto-expands to it).
                var t8Second = PromptPlus.Controls.Tree<Node>("Runs again (default comes from history)")
                    .Root(root)
                    .TextSelector(n => n.Name)
                    .DefaultMatchBy((a, b) => a.Id == b.Id)
                    .EnabledHistory(historyKey);
                BuildFullTree(t8Second, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
                PrintResult(t8Second.Run());
            }
            finally
            {
                PromptPlus.Controls.History(historyKey).Remove();
            }

            // --------------------------------------------------------------------------------
            ShowSection("9) Interaction / InteractionAsync - populate the tree from an external source");
            // A flat list is transformed into a two-level tree via Interaction<T1>.
            var flatDepts = new (string Dept, string[] Teams)[]
            {
                ("Engineering", new [] { "Backend", "Frontend", "DevOps" }),
                ("Sales",       new [] { "EMEA", "APAC" }),
                ("HR",          Array.Empty<string>()),
            };

            int nextId = 1000;
            var t9 = PromptPlus.Controls.Tree<Node>(
                    "Tree populated by Interaction<T1>",
                    "Enumerates flat data and calls AddLast per department + per team")
                .Root(new Node { Id = 999, Name = "Company (from Interaction)" })
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Interaction(flatDepts, (dept, ctrl) =>
                {
                    var deptNode = ctrl.AddLast(new Node { Id = nextId++, Name = dept.Dept, Info = "dept" });
                    foreach (var team in dept.Teams)
                    {
                        deptNode.AddLast(new Node { Id = nextId++, Name = team, Info = "team" });
                    }
                });

            PrintResult(t9.Run());

            // InteractionAsync: same idea, but with an awaited callback per item.
            var t9Async = PromptPlus.Controls.Tree<Node>("Tree populated by InteractionAsync<T1>")
                .Root(new Node { Id = 999, Name = "Company (from InteractionAsync)" })
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .InteractionAsync(flatDepts, async (dept, ctrl) =>
                {
                    await Task.Delay(1);
                    var deptNode = ctrl.AddLast(new Node { Id = nextId++, Name = dept.Dept, Info = "dept" });
                    foreach (var team in dept.Teams)
                    {
                        deptNode.AddLast(new Node { Id = nextId++, Name = team, Info = "team" });
                    }
                });

            PrintResult(t9Async.Run());

            // --------------------------------------------------------------------------------
            ShowSection("10) Styles + Options + PageSize");
            var t10 = PromptPlus.Controls.Tree<Node>("Fully customized rendering")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PageSize(10)
                .Styles(TreeStyles.Prompt,       new Style(Color.Yellow,  Color.Black))
                .Styles(TreeStyles.Root,         new Style(Color.Magenta, Color.Black))
                .Styles(TreeStyles.Node,         new Style(Color.Cyan,    Color.Black))
                .Styles(TreeStyles.ChildsCount,  new Style(Color.Gray,    Color.Black))
                .Styles(TreeStyles.Selected,     new Style(Color.Black,   Color.Gray))
                .Options(opt =>
                {
                    opt.ShowTooltip(true);
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                });

            BuildFullTree(t10, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t10.Run());

            // --------------------------------------------------------------------------------
            // Large dataset samples (500+ items) - showcase the lazy expand/collapse behavior:
            // even though the underlying model contains thousands of nodes, only the currently
            // visible slice is materialized in the render list.
            // --------------------------------------------------------------------------------
            ShowSection("11) Large dataset - 25 groups x 25 items = 625 nodes (flat under root)");
            var t11 = PromptPlus.Controls.Tree<Node>(
                    "Only expanded branches are materialized",
                    "Type a letter to jump; PageUp/PageDown to page")
                .Root(new Node { Id = 100_000, Name = "Root (25 groups x 25 items)", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PageSize(15);
            int total11 = PopulateGrouped(t11, groupCount: 25, itemsPerGroup: 25, startId: 100_001);
            PromptPlus.Console.WriteLine($"(built {total11:N0} nodes)");
            PrintResult(t11.Run());

            // --------------------------------------------------------------------------------
            ShowSection("12) Large dataset via Interaction<T1> + Default deep in the tree");
            // 30 departments, each with 20 teams (600 leaves + 30 containers = 630 nodes).
            var flatBig = Enumerable.Range(1, 30)
                .Select(d => (Dept: $"Dept-{d:00}",
                              Teams: Enumerable.Range(1, 20).Select(i => $"Team-{d:00}-{i:00}").ToArray()))
                .ToArray();

            int bigId = 10_000;
            Node? deepTarget = null;

            var t12 = PromptPlus.Controls.Tree<Node>(
                    "Built by Interaction<T1>; Default expands to a deep leaf",
                    "Dept-15 -> Team-15-10 is pre-selected")
                .Root(new Node { Id = 9999, Name = "BigCompany", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PageSize(15)
                .Interaction(flatBig, (d, ctrl) =>
                {
                    var deptNode = ctrl.AddLast(new Node { Id = bigId++, Name = d.Dept, Info = "dept" });
                    foreach (var t in d.Teams)
                    {
                        var leaf = new Node { Id = bigId++, Name = t, Info = "team" };
                        deptNode.AddLast(leaf);
                        if (d.Dept == "Dept-15" && t == "Team-15-10")
                        {
                            deepTarget = leaf;
                        }
                    }
                });

            if (deepTarget is not null)
            {
                t12.Default(deepTarget);
            }
            PrintResult(t12.Run());

            // --------------------------------------------------------------------------------
            ShowSection("13) Very large + SelectLeafOnly - 10 groups x 100 leaves = 1010 nodes");
            var t13 = PromptPlus.Controls.Tree<Node>(
                    "Only leaves can be confirmed",
                    "Container Enter is rejected; try a leaf")
                .Root(new Node { Id = 200_000, Name = "Root (10 groups x 100 items)", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .SelectLeafOnly()
                .PageSize(15);
            int total13 = PopulateGrouped(t13, groupCount: 10, itemsPerGroup: 100, startId: 200_001);
            PromptPlus.Console.WriteLine($"(built {total13:N0} nodes)");
            PrintResult(t13.Run());

            // --------------------------------------------------------------------------------
            ShowSection("14) ChangeDescription - description updates as you navigate");
            var t14 = PromptPlus.Controls.Tree<Node>(
                    "The description below changes for every selected node",
                    "(base description; will be replaced dynamically)")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .ChangeDescription(n => $"[Id={n.Id}] {n.Name} - kind: {n.Info ?? "(none)"}");
            BuildFullTree(t14, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t14.Run());

            // --------------------------------------------------------------------------------
            ShowSection("15) ChangeDescriptionAsync - async variant");
            var t15 = PromptPlus.Controls.Tree<Node>("Async description (simulated I/O)")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .ChangeDescriptionAsync(async n =>
                {
                    await Task.Delay(1);
                    return $"async: {n.Name} is a {n.Info ?? "node"}";
                });
            BuildFullTree(t15, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t15.Run());

            // --------------------------------------------------------------------------------
            ShowSection("16) Filter (Contains) - type to search; matches show the full path");
            var t16 = PromptPlus.Controls.Tree<Node>(
                    "Type any letters to filter (searches the full path)",
                    "Backspace clears; empty filter returns to the tree view")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Filter(FilterMode.Contains);
            BuildFullTree(t16, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t16.Run());

            // --------------------------------------------------------------------------------
            ShowSection("17) Filter (StartsWith) on a large dataset");
            var t17 = PromptPlus.Controls.Tree<Node>(
                    "Filters against the FULL PATH text of each node",
                    "Try typing 'Group-01' or 'Item-005'")
                .Root(new Node { Id = 300_000, Name = "Root (15 groups x 40 items)", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Filter(FilterMode.StartsWith)
                .PageSize(15);
            int total17 = PopulateGrouped(t17, groupCount: 15, itemsPerGroup: 40, startId: 300_001);
            PromptPlus.Console.WriteLine($"(built {total17:N0} nodes)");
            PrintResult(t17.Run());

            // --------------------------------------------------------------------------------
            ShowSection("18) Disabled nodes - AddLast(value, disable: true)");
            // A disabled node is still shown and can still be navigated to and expanded/collapsed;
            // only confirming it (Enter) is blocked, with the same message as SelectLeafOnly.
            // ViewOnly ignores Disabled entirely, same as PredicateSelected/SelectLeafOnly.
            var t18 = PromptPlus.Controls.Tree<Node>(
                    "'Sales' is disabled - Enter on it is rejected",
                    "You can still navigate onto it and expand/collapse it; only Enter is blocked")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t18Eng = t18.AddLast(eng);
            var t18Back = t18Eng.AddLast(backend);
            t18Back.AddLast(api);
            t18Back.AddLast(database);
            var t18Sales = t18.AddLast(sales, disable: true);
            t18Sales.AddLast(emea);
            t18Sales.AddLast(apac);
            t18.AddLast(hr);

            PrintResult(t18.Run());

            // A Default(...)/history target that resolves to a disabled node is never pre-selected
            // (mirrors Select/Table) - the cursor stays at its natural starting position instead of
            // expanding down to a node the user could never confirm anyway.
            var t18b = PromptPlus.Controls.Tree<Node>(
                    "Default points to a disabled node ('Sales')",
                    "The tree does NOT expand to it - Sales can never be confirmed anyway")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default(sales);

            var t18bEng = t18b.AddLast(eng);
            t18bEng.AddLast(backend);
            t18b.AddLast(sales, disable: true).AddLast(emea);
            t18b.AddLast(hr);

            PrintResult(t18b.Run());
        }

        /// <summary>
        /// Populates the target tree with N groups, each holding M leaves. Returns the total number
        /// of nodes added (groups + leaves + 1 for the root that already exists).
        /// </summary>
        private static int PopulateGrouped(ITreeControl<Node> ctrl, int groupCount, int itemsPerGroup, int startId)
        {
            int id = startId;
            int total = 1; // root already exists on the control
            for (int g = 1; g <= groupCount; g++)
            {
                var group = ctrl.AddLast(new Node { Id = id++, Name = $"Group-{g:000}", Info = "group" });
                total++;
                for (int i = 1; i <= itemsPerGroup; i++)
                {
                    group.AddLast(new Node { Id = id++, Name = $"Item-{g:000}-{i:0000}", Info = "leaf" });
                    total++;
                }
            }
            return total;
        }

        /// <summary>Populates the shared org-chart used by most samples.</summary>
        private static void BuildFullTree(
            ITreeControl<Node> t,
            Node eng, Node backend, Node api, Node database,
            Node frontend, Node web, Node mobile,
            Node sales, Node emea, Node apac,
            Node hr)
        {
            var nEng = t.AddLast(eng);
            var nBack = nEng.AddLast(backend);
            nBack.AddLast(api);
            nBack.AddLast(database);
            var nFront = nEng.AddLast(frontend);
            nFront.AddLast(web);
            nFront.AddLast(mobile);

            var nSales = t.AddLast(sales);
            nSales.AddLast(emea);
            nSales.AddLast(apac);

            t.AddLast(hr);
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintResult(ResultPrompt<Node?> result)
        {
            if (result.IsAborted)
            {
                PromptPlus.Console.WriteLine("IsAborted: true");
            }
            else
            {
                Node? n = result.Content;
                PromptPlus.Console.WriteLine($"Selected: Id={n?.Id} Name={n?.Name} Info={n?.Info}");
            }
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
