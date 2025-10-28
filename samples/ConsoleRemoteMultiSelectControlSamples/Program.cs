// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using PromptPlusLibrary;

namespace ConsoleRemoteMultiSelectControlSamples
{
    public class MyRemoteControl
    {
        public int LastItem { get; set; }
        public bool EndList { get; set; }
    }

    public class OddNumber
    {
        public int Value { get; set; }
    }

    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample Remote Selector");

            var resultclass = PromptPlus.Controls.RemoteMultiSelect<OddNumber, MyRemoteControl>("Select : ")
                .TextSelector(item => item.Value.ToString())
                .UniqueId(item => item.Value.ToString())
                .DefaultWhenLoad([new OddNumber{ Value=71 }, new OddNumber { Value = 73 }])
                .PredicateSearchItems(new MyRemoteControl(), GetOddNumbers, (err) => err.Message)
                .PageSize(7)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {resultclass.IsAborted}, Value count: {resultclass.Content.Length!}");

        }

        private static (bool, MyRemoteControl, IEnumerable<OddNumber>) GetOddNumbers(MyRemoteControl control)
        {
            if (control.EndList)
            {
                return (control.EndList, control, []);
            }
            var endlist = false;
            var ini = control.LastItem;
            var max = control.LastItem;
            if (control.LastItem + 35 > int.MaxValue - 1)
            {
                max = control.LastItem + (int.MaxValue - 1 - control.LastItem);
                endlist = true;
            }
            else
            {
                max = control.LastItem + 35;
            }
            var lst = new List<OddNumber>();
            for (int i = ini; i < max; i++)
            {
                lst.Add(new OddNumber { Value = i * 2 + 1 });
            }
            var status = new MyRemoteControl { LastItem = max, EndList = endlist };

            Thread.Sleep(5000);

            return (status.EndList, status, lst);
        }
    }

}
