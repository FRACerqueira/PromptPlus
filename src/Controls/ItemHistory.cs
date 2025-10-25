// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Controls
{
    internal struct ItemHistory
    {
        public static char Separator = (char)1;

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

        public string History { get; }
        public long TimeOutTicks { get; }

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            return $"{History}{Separator}{TimeOutTicks}";
        }
    }
}
