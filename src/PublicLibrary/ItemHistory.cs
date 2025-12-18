// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the history of an item with a timeout.
    /// </summary>
    public struct ItemHistory
    {
        /// <summary>
        /// Separator character used in the string representation.
        /// </summary>
        public const char Separator = (char)1;

        /// <summary>
        /// Creates a new instance of the ItemHistory class with the specified history and a timeout applied to its
        /// expiration.
        /// </summary>
        /// <param name="history">The history data to associate with the item. Cannot be null.</param>
        /// <param name="timeout">The duration to add to the current time to determine the item's expiration.</param>
        /// <returns>The same <see cref="ItemHistory"/> instance for chaining.</returns>
        public static ItemHistory CreateItemHistory(string history, TimeSpan timeout)
        {
            return new ItemHistory(history, DateTime.Now.Add(timeout).Ticks);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHistory"/> struct with default values.
        /// </summary>
        public ItemHistory()
        {
            History = "";
            TimeOutTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHistory"/> struct with the specified history and timeout ticks.
        /// </summary>
        /// <param name="history">
        /// The history data to associate with the item. Cannot be null.
        /// </param>
        /// <param name="dateTicks">
        /// The timeout ticks for the item.
        /// </param>
        public ItemHistory(string history, long dateTicks)
        {
            History = history;
            TimeOutTicks = dateTicks;
        }

        /// <summary>
        /// The history data associated with the item.
        /// </summary>
        public string History { get; }

        /// <summary>
        /// Gets the timeout duration, in ticks, for the associated operation.
        /// </summary>
        public long TimeOutTicks { get; }

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            return $"{History}{Separator}{TimeOutTicks}";
        }
    }
}
