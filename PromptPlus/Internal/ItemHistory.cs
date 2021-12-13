using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPlus.Internal
{
    internal struct ItemHistory: IComparer<ItemHistory>
    {
        public const string Separator = "|%PPlus.TimeOut%|";


        public static ItemHistory CreateItemHistory(string history, TimeSpan timeout)
        {
            return new ItemHistory(history, DateTime.Now.Add(timeout).Ticks);
        }

        public ItemHistory()
        {
            History = "";
            TimeOutTicks = DateTime.Now.Ticks;
        }

        internal ItemHistory(string history, long dateTicks)
        {
            History = history;
            TimeOutTicks = dateTicks;
        }

        public string History { get;  }
        public long TimeOutTicks { get; }

        public override string ToString()
        {
            return $"{History}{Separator}{TimeOutTicks}";
        }

        public int Compare(ItemHistory x, ItemHistory y) => throw new NotImplementedException();
    }


}
