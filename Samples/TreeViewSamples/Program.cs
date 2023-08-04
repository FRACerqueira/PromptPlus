using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace TreeViewSamples
{
    internal enum TypeMyOrg
    {
        Empresa,
        Tribe,
        Gestor,
        Funcionario
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
        public TypeMyOrg TypeInfo { get; set; } = TypeMyOrg.Funcionario;

        public static MyOrg LoadEmpresa()
        {
            return new MyOrg
            {
                Name = "MyOrg",
                TypeInfo = TypeMyOrg.Empresa,
            };
        }

        public static MyOrg[] LoadAreas()
        {
            var t1 = new MyOrg
            {
                Name = "Tribe1",
                TypeInfo = TypeMyOrg.Tribe
            };
            var t2 = new MyOrg
            {
                Name = "Tribe2",
                TypeInfo = TypeMyOrg.Tribe
            };
            var t3 = new MyOrg
            {
                Name = "Tribe3",
                TypeInfo = TypeMyOrg.Tribe
            };
            return new MyOrg[]
            {
                t1,
                t2,
                t3
            };
        }
        public static MyOrg[] LoadGestores(MyOrg area)
        {

            var g1 = new MyOrg { Name = $"Manager1-{area.Name}", TypeInfo = TypeMyOrg.Gestor };
            var g2 = new MyOrg { Name = $"Manager2-{area.Name}", TypeInfo = TypeMyOrg.Gestor };
            return new MyOrg[]
            {
                g1,
                g2
            };
        }

        public static MyOrg[] LoadFuncionarios(MyOrg gestor)
        {
            return new[]
            {
                new MyOrg{ Name = $"Employee1-{gestor.Name}", TypeInfo = TypeMyOrg.Funcionario },
                new MyOrg{ Name = $"Employee2-{gestor.Name}", TypeInfo = TypeMyOrg.Funcionario }
            };
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //Ensure ValueResult Culture for all controls
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

            PromptPlus.WriteLine("Hello, World!");
            PromptPlus.DoubleDash("Control:TreeView - Common usage");

            var trv = PromptPlus.TreeView<MyOrg>("MyMorg")
                .RootNode(MyOrg.LoadEmpresa(),
                    (item) => item.Name,
                    expandall: false,
                    separatePath: '|',
                    validselect: (item) => item.TypeInfo == TypeMyOrg.Funcionario,
                    setdisabled: (item) => item.TypeInfo != TypeMyOrg.Funcionario,
                    uniquenode: (item) => item.Id)
                .Styles(StyleTreeView.SelectedRoot, Style.Default.Foreground(Color.Yellow))
                .Interaction(MyOrg.LoadAreas(), (ctrl, item) =>
                {
                    ctrl.AddNode(item);
                    var gests = MyOrg.LoadGestores(item);
                    foreach (var ges in gests)
                    {
                        ctrl.AddNode(item, ges);
                        var funcs = MyOrg.LoadFuncionarios(ges);
                        foreach (var fun in funcs)
                        {
                            ctrl.AddNode(ges, fun);
                        }
                    }
                })
                .Run();

            if (!trv.IsAborted)
            {
                PromptPlus.WriteLine($"You item select is {trv.Value.Id}:{trv.Value.Name}");
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}