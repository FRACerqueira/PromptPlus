// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace TreeViewMultiSelectSamples
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

            var mtrv = PromptPlus
                .TreeViewMultiSelect<MyOrg>("MyMorg")
                .Range(2,4)
                .ExpandAll()
                .RootNode(MyOrg.LoadEmpresa(),
                    (item) => item.Name,
                    separatePath: '|',
                    uniquenode: (item) => item.Id,
                    validselect: (item) => item.TypeInfo == TypeMyOrg.Funcionario)
                .Styles(StyleTreeView.SelectedRoot, Style.Default.Foreground(Color.Yellow))
                .PageSize(5)
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
                            if (fun.Name == "Employee1-Manager1-Tribe1")
                            {
                                ctrl.AddFixedSelect(fun);
                            }
                            ctrl.AddNode(ges, fun);
                        }
                    }
                })
                .Run();

            if (!mtrv.IsAborted)
            {
                foreach (var item in mtrv.Value)
                {
                    PromptPlus.WriteLine($"You items select is {item.Id}:{item.Name}");
                }
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}