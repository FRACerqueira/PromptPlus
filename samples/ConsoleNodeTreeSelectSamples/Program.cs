// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;
using System.Globalization;

namespace ConsoleNodeTreeSelectSamples
{
    internal enum TypeMyOrg
    {
        Organization,
        Tribe,
        Manager,
        Employee
    }
    internal class MyOrg
    {
        private readonly string _uniqueId;

        public MyOrg()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public string Id => _uniqueId;
        public string Name { get; set; } = string.Empty;
        public TypeMyOrg TypeInfo { get; set; } = TypeMyOrg.Employee;

        public static MyOrg LoadOrganization()
        {
            return new MyOrg
            {
                Name = "MyOrg",
                TypeInfo = TypeMyOrg.Organization,
            };
        }

        public static MyOrg[] LoadTribe()
        {
            var result = new List<MyOrg>();
            for (int i = 1; i < 10; i++)
            {
                result.Add(new MyOrg
                {
                    Name = $"Tribe{i}",
                    TypeInfo = TypeMyOrg.Tribe
                });
            }
            return result.ToArray();
        }

        public static MyOrg[] LoadManager(MyOrg area)
        {
            var qtd = Program.rnd.Next(1, 5);
            var result = new List<MyOrg>();
            for (int i = 0; i < qtd; i++)
            {
                result.Add(new MyOrg
                {
                    Name = $"Manager{i}-{area.Name}",
                    TypeInfo = TypeMyOrg.Manager
                });
            }
            return result.ToArray();
        }

        public static MyOrg[] LoadEmployee(MyOrg gestor)
        {
            Random rnd = new Random();
            var qtd = Program.rnd.Next(1, 10);
            var result = new List<MyOrg>();
            for (int i = 0; i < qtd; i++)
            {
                result.Add(new MyOrg
                {
                    Name = $"Employee{i}-{gestor.Name}",
                    TypeInfo = TypeMyOrg.Employee
                });
            }
            return result.ToArray();
        }
    }
    internal class Program
    {
        internal static Random rnd = new Random();
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample Node Tree Select", extraLines: 1);

            var root = MyOrg.LoadOrganization();

            var result = PromptPlus.Controls.NodeTreeSelect<MyOrg>("Node : ","My description")
                .TextSelector((x) => x.Name)
                .AddRootNode(root)
                .AddChildNode(root, new MyOrg { Name = "Tribe0 Empty", TypeInfo = TypeMyOrg.Tribe })
                .Interaction(MyOrg.LoadTribe(), (item,ctrl) =>
                {
                    ctrl.AddChildNode(root,item);
                    var gests = MyOrg.LoadManager(item);
                    foreach (var ges in gests)
                    {
                        ctrl.AddChildNode(item, ges);
                        var funcs = MyOrg.LoadEmployee(ges);
                        foreach (var fun in funcs)
                        {
                            ctrl.AddChildNode(ges, fun);
                        }
                    }
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content?.Name ?? string.Empty}");

            PromptPlus.Widgets.DoubleDash("Sample Node Tree Select with Predicate Disabled", extraLines: 1);

            result = PromptPlus.Controls.NodeTreeSelect<MyOrg>("Select Employee : ", "My description")
                .TextSelector((x) => x.Name)
                .PredicateDisabled(x => x.TypeInfo != TypeMyOrg.Employee)
                .AddRootNode(root)
                .AddChildNode(root, new MyOrg { Name = "Tribe0 Empty", TypeInfo = TypeMyOrg.Tribe })
                .Interaction(MyOrg.LoadTribe(), (item, ctrl) =>
                {
                    ctrl.AddChildNode(root, item);
                    var gests = MyOrg.LoadManager(item);
                    foreach (var ges in gests)
                    {
                        ctrl.AddChildNode(item, ges);
                        var funcs = MyOrg.LoadEmployee(ges);
                        foreach (var fun in funcs)
                        {
                            ctrl.AddChildNode(ges, fun);
                        }
                    }
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content?.Name ?? string.Empty}");

            PromptPlus.Widgets.DoubleDash("Sample Node Tree Select with Predicate seleted", extraLines: 1);

            result = PromptPlus.Controls.NodeTreeSelect<MyOrg>("Select Manager : ", "My description")
                .TextSelector((x) => x.Name)
                .PredicateSelected(x => x.TypeInfo == TypeMyOrg.Manager)
                .AddRootNode(root)
                .AddChildNode(root, new MyOrg { Name = "Tribe0 Empty", TypeInfo = TypeMyOrg.Tribe })
                .Interaction(MyOrg.LoadTribe(), (item, ctrl) =>
                {
                    ctrl.AddChildNode(root, item);
                    var gests = MyOrg.LoadManager(item);
                    foreach (var ges in gests)
                    {
                        ctrl.AddChildNode(item, ges);
                        var funcs = MyOrg.LoadEmployee(ges);
                        foreach (var fun in funcs)
                        {
                            ctrl.AddChildNode(ges, fun);
                        }
                    }
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content?.Name ?? string.Empty}");

            PromptPlus.Widgets.DoubleDash("Sample Node Tree Select with Custom colors", extraLines: 1);

            result = PromptPlus.Controls.NodeTreeSelect<MyOrg>("Node : ", "My description")
                .TextSelector((x) => x.Name)
                .Styles(NodeTreeStyles.Lines, Color.Red)
                .Styles(NodeTreeStyles.ChildsCount, Color.LightCoral)
                .Styles(NodeTreeStyles.Root, Color.Cyan1)
                .Styles(NodeTreeStyles.Node, Color.LightCyan3)
                .Styles(NodeTreeStyles.ExpandSymbol, Color.Olive)
                .AddRootNode(root)
                .AddChildNode(root, new MyOrg { Name = "Tribe0 Empty", TypeInfo = TypeMyOrg.Tribe })
                .Interaction(MyOrg.LoadTribe(), (item, ctrl) =>
                {
                    ctrl.AddChildNode(root, item);
                    var gests = MyOrg.LoadManager(item);
                    foreach (var ges in gests)
                    {
                        ctrl.AddChildNode(item, ges);
                        var funcs = MyOrg.LoadEmployee(ges);
                        foreach (var fun in funcs)
                        {
                            ctrl.AddChildNode(ges, fun);
                        }
                    }
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content?.Name ?? string.Empty}");

        }
    }
}