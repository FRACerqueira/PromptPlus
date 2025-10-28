// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleNodeTreeRemoteSelectSamples
{
    public class MyRemoteControl
    {
        public int RootLastItem { get; set; }
        public bool RootEndList { get; set; }

        public Dictionary<string, (int,bool)> StatusChilds = [];
    }

    public class Number
    {
        public int Value { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool ChildAllowed { get; set; }
    }

    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample Remote Selector");

            var resultclass = PromptPlus.Controls.NodeTreeRemoteSelect<Number, MyRemoteControl>("Select : ")
                .TextSelector(item => item.Name)
                .UniqueId(item => item.Value.ToString())
                .AddRootNode(new Number { Value = int.MinValue, Name = "Root Node" })
                .PredicateSearchRootNode(new MyRemoteControl(), GetAllNumbers, (err) => err.Message)
                .PredicateSearchChildNode(GetNodesChildsNumber, (err) => err.Message)
                .PredicateChildAllowed(item => item.ChildAllowed)
                .PageSize(7)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultclass.IsAborted}, Value ID: {resultclass.Content.Value!}");

        }

        private static (bool, MyRemoteControl, IEnumerable<Number>) GetNodesChildsNumber(Number number, MyRemoteControl control)
        {
            throw new NotImplementedException();
        }

        private static (bool, MyRemoteControl, IEnumerable<Number>) GetAllNumbers(MyRemoteControl control)
        {
            if (control.RootEndList)
            {
                return (control.RootEndList, control, []);
            }
            var endlist = false;
            var ini = control.RootLastItem;
            var max = control.RootLastItem;
            if (control.RootLastItem + 35 > int.MaxValue-1)
            {
                max = control.RootLastItem + (int.MaxValue - 1 - control.RootLastItem);
                endlist = true;
            }
            else
            { 
                max = control.RootLastItem + 35;
            }
            var lst = new List<Number>();
            for (int i = ini; i < max; i++)
            {
                lst.Add(new Number { Value = i, Name = $"number {i}" , ChildAllowed = (i % 2 == 0)});
            }
            var status = new MyRemoteControl { RootLastItem = max, RootEndList = endlist };

            Thread.Sleep(5000);

            return (status.RootEndList, status, lst);
        }
    }
}
