// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MultiTreeControlSamples
{
    /// <summary>
    /// A simple user item used to populate the generic multi-tree.
    /// Marked [Serializable] so that EnableHistory can persist checked values (see sample 8).
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
            Node root     = new() { Id = 1,  Name = "Company",     Info = "root" };
            Node eng      = new() { Id = 2,  Name = "Engineering", Info = "dept" };
            Node backend  = new() { Id = 3,  Name = "Backend",     Info = "team" };
            Node api      = new() { Id = 4,  Name = "API",         Info = "service" };
            Node database = new() { Id = 5,  Name = "Database",    Info = "service" };
            Node frontend = new() { Id = 6,  Name = "Frontend",    Info = "team" };
            Node web      = new() { Id = 7,  Name = "Web",         Info = "app" };
            Node mobile   = new() { Id = 8,  Name = "Mobile",      Info = "app" };
            Node sales    = new() { Id = 9,  Name = "Sales",       Info = "dept" };
            Node emea     = new() { Id = 10, Name = "EMEA",        Info = "region" };
            Node apac     = new() { Id = 11, Name = "APAC",        Info = "region" };
            Node hr       = new() { Id = 12, Name = "HR",          Info = "dept" };

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("1) Basic usage - Root, TextSelector, DefaultMatchBy, AddLast, Space to check, Enter to confirm");
            var t1 = PromptPlus.Controls.MultiTree<Node>(
                    "Check items and press Enter",
                    "Space=check/uncheck  Enter=confirm  ESC=abort")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t1Eng  = t1.AddLast(eng);
            var t1Back = t1Eng.AddLast(backend);
            t1Back.AddLast(api);
            t1Back.AddLast(database);
            var t1Front = t1Eng.AddLast(frontend);
            t1Front.AddLast(web);
            t1Front.AddLast(mobile);
            t1.AddLast(sales);
            t1.AddLast(hr);

            PrintResult(t1.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("2) CascadeCheck(true) - checking a container marks all descendants");
            var t2 = PromptPlus.Controls.MultiTree<Node>(
                    "Check 'Engineering' and see all children get marked",
                    "CascadeCheck is ON (default)")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .CascadeCheck(true);   // default — explicit for clarity

            BuildFullTree(t2, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t2.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("3) CascadeCheck(false) - checking a container does NOT propagate to children");
            var t3 = PromptPlus.Controls.MultiTree<Node>(
                    "Check 'Engineering': only the container itself is marked",
                    "CascadeCheck is OFF")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .CascadeCheck(false);

            BuildFullTree(t3, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t3.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("4) RecursiveMarkWithCtrlSpace(true) - user chooses: Space=single, Ctrl+Space=recursive");
            var t4 = PromptPlus.Controls.MultiTree<Node>(
                    "Space marks only the item itself; Ctrl+Space marks the item and all children",
                    "CascadeCheck is ON + RecursiveMarkWithCtrlSpace gives users control")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .CascadeCheck(true)
                .RecursiveMarkWithCtrlSpace(true);

            BuildFullTree(t4, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t4.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("5) CheckLeafOnly - only leaf nodes can be checked (containers are blocked)");
            var t5 = PromptPlus.Controls.MultiTree<Node>(
                    "Try Space on a container: it is rejected",
                    "Only leaves (API, Database, Web, ...) can be checked")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .CheckLeafOnly();

            BuildFullTree(t5, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t5.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("6) ExtraInfo, PathSeparator('.') and ShowFullPath");
            var t6 = PromptPlus.Controls.MultiTree<Node>(
                    "Checked items show full dotted path in FinishTemplate",
                    "Use the full-path hotkey to toggle short/long display during navigation")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .PathSeparator('.')
                .ShowFullPath(true)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            BuildFullTree(t6, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t6.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("7) Range(2, 4) - Enter is rejected until the checked count is within bounds");
            var t7 = PromptPlus.Controls.MultiTree<Node>(
                    "Select between 2 and 4 items",
                    "Enter is rejected until the count is within [2, 4]")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Range(2, 4);

            BuildFullTree(t7, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t7.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("8) Default - pre-checked items auto-expand the tree");
            var t8 = PromptPlus.Controls.MultiTree<Node>(
                    "API and Mobile are pre-checked",
                    "Tree auto-expands to show each pre-checked node")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default([api, mobile]);

            BuildFullTree(t8, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t8.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("9) PredicateChecked - only items matching the predicate can be checked");
            var t9 = PromptPlus.Controls.MultiTree<Node>(
                    "Only 'service' items (API, Database) can be checked",
                    "Try checking 'HR' or 'Sales' to see the error message")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PredicateChecked(n =>
                    n.Info == "service"
                        ? (true, null)
                        : (false, $"'{n.Name}' is a {n.Info}, not a service."));

            BuildFullTree(t9, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t9.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("10) PredicateCheckedAsync - async predicate validation");
            var t10 = PromptPlus.Controls.MultiTree<Node>(
                    "Async predicate: only 'app' items (Web, Mobile) can be checked",
                    "Simulates an async validation call")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PredicateCheckedAsync(async n =>
                {
                    await Task.Delay(1);
                    return n.Info == "app";
                });

            BuildFullTree(t10, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t10.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("11) ViewOnly - Space disabled; pre-checked defaults returned on Enter");
            var t11 = PromptPlus.Controls.MultiTree<Node>(
                    "ViewOnly: Space is disabled; pre-checked defaults returned on Enter",
                    "API and EMEA are pre-checked defaults")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default([api, emea])
                .ViewOnly();

            BuildFullTree(t11, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t11.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("12) EnableHistory - remembers checked nodes across runs");
            const string historyKey = "SampleMultiTree.History";
            PromptPlus.Controls.History(historyKey).Remove();
            try
            {
                var t11First = PromptPlus.Controls.MultiTree<Node>("Check some nodes (will be remembered)")
                    .Root(root)
                    .TextSelector(n => n.Name)
                    .DefaultMatchBy((a, b) => a.Id == b.Id)
                    .EnableHistory(historyKey);
                BuildFullTree(t11First, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
                PrintResult(t11First.Run());

                var t11Second = PromptPlus.Controls.MultiTree<Node>("Runs again (checked state from history)")
                    .Root(root)
                    .TextSelector(n => n.Name)
                    .DefaultMatchBy((a, b) => a.Id == b.Id)
                    .EnableHistory(historyKey);
                BuildFullTree(t11Second, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
                PrintResult(t11Second.Run());
            }
            finally
            {
                PromptPlus.Controls.History(historyKey).Remove();
            }

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("13) EnableHistory - Default combined with a named history key");
            var t12 = PromptPlus.Controls.MultiTree<Node>(
                    "History is ON: checked values from the last run are restored",
                    "First run uses the Default; next runs load the last checked set from the .history file")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default([api, database])
                .EnableHistory("multi-tree-history");

            BuildFullTree(t12, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t12.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("14) ChangeDescription - description updates as cursor moves");
            var t13 = PromptPlus.Controls.MultiTree<Node>(
                    "The description changes for every node under the cursor",
                    "(base description; will be replaced dynamically)")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .ChangeDescription(n => $"[Id={n.Id}] {n.Name} - kind: {n.Info ?? "(none)"}");
            BuildFullTree(t13, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t13.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("15) ChangeDescriptionAsync - async description");
            var t14 = PromptPlus.Controls.MultiTree<Node>("Async description (simulated I/O)")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .ChangeDescriptionAsync(async n =>
                {
                    await Task.Delay(1);
                    return $"async: {n.Name} is a {n.Info ?? "node"}";
                });
            BuildFullTree(t14, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t14.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("16) Filter with Contains mode");
            var t15 = PromptPlus.Controls.MultiTree<Node>(
                    "Type any letters to filter (searches the full path)",
                    "Space still checks/unchecks matched nodes; Backspace clears filter")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Filter(FilterMode.Contains);
            BuildFullTree(t15, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t15.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("17) Filter with StartsWith mode");
            var t16 = PromptPlus.Controls.MultiTree<Node>(
                    "Filters against the node name (StartsWith)",
                    "Try typing 'Group-01' or 'Item-001'")
                .Root(new Node { Id = 300_000, Name = "Root (15 groups x 40 items)", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Filter(FilterMode.StartsWith)
                .PageSize(15);
            PopulateGrouped(t16, groupCount: 15, itemsPerGroup: 40, startId: 300_001);
            PrintResult(t16.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("18) Interaction<T1> - populate the tree from an external source");
            var flatDepts = new (string Dept, string[] Teams)[]
            {
                ("Engineering", ["Backend", "Frontend", "DevOps"]),
                ("Sales",       ["EMEA", "APAC"]),
                ("HR",          Array.Empty<string>()),
            };

            int nextId = 1000;
            var t17 = PromptPlus.Controls.MultiTree<Node>(
                    "Tree populated by Interaction<T1>",
                    "Enumerates flat data and calls AddLast per department + per team")
                .Root(new Node { Id = 999, Name = "Company (from Interaction)" })
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Interaction(flatDepts, (dept, ctrl) =>
                {
                    var deptNode = ctrl.AddLast(new Node { Id = nextId++, Name = dept.Dept, Info = "dept" });
                    foreach (string team in dept.Teams)
                        deptNode.AddLast(new Node { Id = nextId++, Name = team, Info = "team" });
                });

            PrintResult(t17.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("19) InteractionAsync<T1> - async population variant");
            var t18 = PromptPlus.Controls.MultiTree<Node>("Tree populated by InteractionAsync<T1>")
                .Root(new Node { Id = 999, Name = "Company (from InteractionAsync)" })
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .InteractionAsync(flatDepts, async (dept, ctrl) =>
                {
                    await Task.Delay(1);
                    var deptNode = ctrl.AddLast(new Node { Id = nextId++, Name = dept.Dept, Info = "dept" });
                    foreach (string team in dept.Teams)
                        deptNode.AddLast(new Node { Id = nextId++, Name = team, Info = "team" });
                });

            PrintResult(t18.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("20) Styles + Options + PageSize - fully customized rendering");
            var t19 = PromptPlus.Controls.MultiTree<Node>("Fully customized rendering")
                .Root(root)
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PageSize(10)
                .Styles(MultiTreeStyles.Prompt,      new Style(Color.Yellow,  Color.Black))
                .Styles(MultiTreeStyles.Root,        new Style(Color.Magenta, Color.Black))
                .Styles(MultiTreeStyles.Node,        new Style(Color.Cyan,    Color.Black))
                .Styles(MultiTreeStyles.ChildsCount, new Style(Color.Gray,    Color.Black))
                .Styles(MultiTreeStyles.Selected,    new Style(Color.Black,   Color.Gray))
                .Options(opt =>
                {
                    opt.ShowTooltip(true);
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                });

            BuildFullTree(t19, eng, backend, api, database, frontend, web, mobile, sales, emea, apac, hr);
            PrintResult(t19.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("21) Large dataset + Default deep in the tree");
            // 30 departments, 20 teams each = 630 nodes.
            var flatBig = Enumerable.Range(1, 30)
                .Select(d => (Dept: $"Dept-{d:00}",
                              Teams: Enumerable.Range(1, 20).Select(i => $"Team-{d:00}-{i:00}").ToArray()))
                .ToArray();

            int bigId = 10_000;
            Node? deepTarget1 = null, deepTarget2 = null;

            var t20 = PromptPlus.Controls.MultiTree<Node>(
                    "Built by Interaction<T1>; Dept-10→Team-10-05 and Dept-20→Team-20-12 pre-checked",
                    "Tree auto-expands to each pre-checked node")
                .Root(new Node { Id = 9999, Name = "BigCompany", Info = "root" })
                .TextSelector(n => n.Name)
                .ExtraInfo(n => n.Info)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .PageSize(15)
                .Interaction(flatBig, (d, ctrl) =>
                {
                    var deptNode = ctrl.AddLast(new Node { Id = bigId++, Name = d.Dept, Info = "dept" });
                    foreach (string t in d.Teams)
                    {
                        var leaf = new Node { Id = bigId++, Name = t, Info = "team" };
                        deptNode.AddLast(leaf);
                        if (d.Dept == "Dept-10" && t == "Team-10-05") deepTarget1 = leaf;
                        if (d.Dept == "Dept-20" && t == "Team-20-12") deepTarget2 = leaf;
                    }
                });

            List<Node> preChecked = [];
            if (deepTarget1 is not null) preChecked.Add(deepTarget1);
            if (deepTarget2 is not null) preChecked.Add(deepTarget2);
            if (preChecked.Count > 0) t20.Default(preChecked);

            PrintResult(t20.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("22) AddFirst / AddAfter / AddBefore - sibling ordering");
            var t21 = PromptPlus.Controls.MultiTree<Node>("Notice the order of the top-level nodes")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t21Eng   = t21.AddLast(eng);
            var t21Sales = t21.AddLast(sales);
            t21.AddBefore(t21Sales, hr);                // → [Eng, HR, Sales]
            var t21Front = t21.AddFirst(frontend);      // → [Frontend, Eng, HR, Sales]
            t21.AddAfter(t21Eng, backend);              // → [Frontend, Eng, Backend, HR, Sales]
            t21Front.AddLast(web);
            t21Front.AddLast(mobile);

            PrintResult(t21.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("23) Disabled nodes - AddLast(value, disable: true)");
            // A disabled node is still shown and can still be navigated to and
            // expanded/collapsed; only checking/unchecking it directly is blocked, with the
            // same message as CheckLeafOnly. A cascading check on an ancestor still passes
            // through a disabled container to reach its enabled descendants.
            var t22 = PromptPlus.Controls.MultiTree<Node>(
                    "'Sales' is disabled - Space on it is rejected",
                    "Check 'Sales' itself: rejected. Check 'Company': EMEA/APAC still get checked.")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t22Eng = t22.AddLast(eng);
            var t22Back = t22Eng.AddLast(backend);
            t22Back.AddLast(api);
            t22Back.AddLast(database);
            var t22Sales = t22.AddLast(sales, disable: true);
            t22Sales.AddLast(emea);
            t22Sales.AddLast(apac);
            t22.AddLast(hr);

            PrintResult(t22.Run());

            // A Default(...)/history target that resolves to a disabled node IS force-checked
            // (unlike Tree/Select) and survives a later F2 mass-uncheck untouched, matching
            // IMultiSelectControl's behavior for disabled defaults.
            var t22b = PromptPlus.Controls.MultiTree<Node>(
                    "Default force-checks the disabled 'Sales' node",
                    "Sales shows checked even though Space on it is rejected; F2 will not uncheck it")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default([sales]);

            var t22bEng = t22b.AddLast(eng);
            t22bEng.AddLast(backend);
            t22b.AddLast(sales, disable: true).AddLast(emea);
            t22b.AddLast(hr);

            PrintResult(t22b.Run());

            // ──────────────────────────────────────────────────────────────────────────
            ShowSection("24) Construction-time check - AddLast(value, check: true)");
            // Unlike Default(...), check:true does not auto-expand the tree to reveal the node -
            // it just starts pre-checked, quietly, same spirit as
            // IMultiSelectControl<T>.AddItem(ischecked:)/IMultiTableControl<T>.AddItem(ischecked:).
            // AddLast/AddFirst/AddAfter/AddBefore all return IMultiTreeNode<T> (not the plain
            // ITreeNode<T> that TreeControl uses), so chaining deeper into the tree keeps access
            // to check, not just the top-level calls made directly off the control.
            var t23 = PromptPlus.Controls.MultiTree<Node>(
                    "'API' starts pre-checked (check: true) - tree stays collapsed",
                    "Compare with Default(...), which would auto-expand down to the checked node")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id);

            var t23Eng = t23.AddLast(eng);
            var t23Back = t23Eng.AddLast(backend);
            t23Back.AddLast(api, check: true);
            t23Back.AddLast(database);
            t23.AddLast(sales);
            t23.AddLast(hr);

            PrintResult(t23.Run());

            // check:true on a container cascades to its descendants exactly like an interactive
            // check does; check and Default are additive - neither one clears the other.
            var t23b = PromptPlus.Controls.MultiTree<Node>(
                    "'Engineering' starts pre-checked (cascades) + 'HR' via Default",
                    "Both mechanisms compose - Engineering/Backend/API/Database/Frontend/Web/Mobile + HR")
                .Root(root)
                .TextSelector(n => n.Name)
                .DefaultMatchBy((a, b) => a.Id == b.Id)
                .Default([hr]);

            var t23bEng = t23b.AddLast(eng, check: true);
            var t23bBack = t23bEng.AddLast(backend);
            t23bBack.AddLast(api);
            t23bBack.AddLast(database);
            var t23bFront = t23bEng.AddLast(frontend);
            t23bFront.AddLast(web);
            t23bFront.AddLast(mobile);
            t23b.AddLast(sales);
            t23b.AddLast(hr);

            PrintResult(t23b.Run());
        }

        // ─── helpers ────────────────────────────────────────────────────────────────

        private static void PopulateGrouped(IMultiTreeControl<Node> ctrl, int groupCount, int itemsPerGroup, int startId)
        {
            int id = startId;
            for (int g = 1; g <= groupCount; g++)
            {
                var group = ctrl.AddLast(new Node { Id = id++, Name = $"Group-{g:000}", Info = "group" });
                for (int i = 1; i <= itemsPerGroup; i++)
                    group.AddLast(new Node { Id = id++, Name = $"Item-{g:000}-{i:0000}", Info = "leaf" });
            }
        }

        private static void BuildFullTree(
            IMultiTreeControl<Node> t,
            Node eng, Node backend, Node api, Node database,
            Node frontend, Node web, Node mobile,
            Node sales, Node emea, Node apac,
            Node hr)
        {
            var nEng  = t.AddLast(eng);
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

        private static void PrintResult(ResultPrompt<Node[]> result)
        {
            if (result.IsAborted)
            {
                PromptPlus.Console.WriteLine("IsAborted: true");
            }
            else
            {
                Node[] nodes = result.Content ?? [];
                if (nodes.Length == 0)
                {
                    PromptPlus.Console.WriteLine("Selected: (none)");
                }
                else
                {
                    foreach (Node n in nodes)
                        PromptPlus.Console.WriteLine($"  ✓ Id={n.Id} Name={n.Name} Info={n.Info}");
                }
            }
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
