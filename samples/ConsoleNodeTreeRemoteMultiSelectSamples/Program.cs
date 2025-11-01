// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleNodeTreeRemoteMultiSelectSamples
{
    /// <summary>
    /// This class is for demonstration purposes only and does not represent best practice in this context.
    /// </summary>
    public class MyRemoteControl
    {
        public int LastItem { get; set; }
        public bool EndList { get; set; }
        public byte MaxEmpty { get; set; } = 5;
        public bool FlagErro { get; set; }
    }

    public class Number
    {
        public int Value { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool ChildAllowed { get; set; } = true;
        public long Size { get; set; }
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

            PromptPlus.Widgets.DoubleDash("Sample Remote Multi Selector");

            var resultclass = PromptPlus.Controls.NodeTreeRemoteMultiSelect<Number, MyRemoteControl>("Select : ", "Odd number is folder and max root is 100. Childs is fixed at 10 items without more childs")
                .AddRootNode(new Number { Value = int.MinValue, Name = "Root Node" }, new MyRemoteControl())
                .TextSelector(item => item.Name)
                .ExtraInfo(GetSizeInfo)
                .UniqueId(item => item.Value.ToString())
                .SearchMoreItems(GetNodes, (err) => err.Message)
                .PredicateChildAllowed(item => item.ChildAllowed)
                .PageSize(13)
                .DisableRecursiveCount(false)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultclass.IsAborted}, Value ID: {resultclass.Content.Length}");

        }

        private static string? GetSizeInfo(Number number)
        {
            if (!number.ChildAllowed)
            {
                return BytesToString(1024 * (number.Value + 10));
            }
            return null;
        }

        private static (bool, MyRemoteControl, IEnumerable<(bool,Number)>) GetNodes(Number number, MyRemoteControl control)
        {
            //Root nodes
            var lst = new List<(bool,Number)>();
            if (number.Value == int.MinValue)
            {
                if (control.EndList)
                {
                    return (control.EndList, control, []);
                }
                var endlist = false;
                var ini = control.LastItem;
                var max = control.LastItem;
                if (control.LastItem + 35 > 100)
                {
                    max = 100;
                    endlist = true;
                }
                else
                {
                    max = control.LastItem + 35;
                }
                for (int i = ini; i < max; i++)
                {
                    lst.Add((false,new Number { Value = i, Name = $"number {i}", ChildAllowed = (i % 2 != 0) }));
                }
                var status = new MyRemoteControl { LastItem = max, EndList = endlist };

                Thread.Sleep(5000);

                return (status.EndList, status, lst);
            }
            if (control.MaxEmpty > 1)
            {
                var text = $"Other Child {500 + control.MaxEmpty}";
                lst.Add((false,new Number { Value = 500+ control.MaxEmpty, Name = text, ChildAllowed = true }));
                control.MaxEmpty--;
            }
            else
            {
                if (control.FlagErro)
                {
                    if (control.MaxEmpty > 0)
                    {
                        var text = $"Other Child {500 + control.MaxEmpty}";
                        lst.Add((false,new Number { Value = 500 + control.MaxEmpty, Name = text, ChildAllowed = true }));
                        control.MaxEmpty--;
                    }
                }
                else
                {
                    control.FlagErro = true;
                    throw new Exception("Simulated error getting child nodes, try again!");
                }
            }
            //Child nodes
            var randomQtd = new Random(DateTime.Now.Second);
            var maxqtd = randomQtd.Next(1,8);
            for (int i = 0; i < maxqtd; i++)
            {
                var value = randomQtd.Next(200, 300) + i;
                lst.Add((false,new Number { Value = value, Name = $"number {value}", ChildAllowed = false, Size = randomQtd.NextInt64(500, 3000000)}));
            }
            return (control.MaxEmpty == 0, control, lst);

        }

        private static string BytesToString(long value)
        {
            string[] suf = ["", " KB", " MB", " GB", " TB", " PB", " EB"]; //Longs run out around EB
            if (value == 0)
            {
                return "0";
            }
            int place = Convert.ToInt32(Math.Floor(Math.Log(value, 1024)));
            double num = Math.Round(value / Math.Pow(1024, place), 0);
            return $"{num}{suf[place]}";
        }
    }
}
