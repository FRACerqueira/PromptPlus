using System;
using System.Collections.Generic;

namespace PPlus.Objects
{
    public struct ItemHistory: IComparer<ItemHistory>
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

        public ItemHistory(string history, long dateTicks)
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
