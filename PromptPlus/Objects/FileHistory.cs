// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static System.Environment;

namespace PPlus.Objects
{
    public static class FileHistory
    {
        private const string Folderhistory = "PromptPlus.History";
        private const string Filehistory = "{0}.txt";

        public static IList<ItemHistory> LoadHistory(string filename)
        {
            var file = string.Format(Filehistory, filename);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            var result = new List<ItemHistory>();
            if (File.Exists(Path.Combine(userProfile, Folderhistory, file)))
            {
                var aux = File.ReadAllLines(Path.Combine(userProfile, Folderhistory, file));
                foreach (var item in aux)
                {
                    var itemhist = item.Split(ItemHistory.Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (itemhist.Length == 2)
                    {
                        if (long.TryParse(itemhist[1], out var dtTicks))
                        {
                            if (DateTime.Now < new DateTime(dtTicks))
                            {
                                result.Add(new ItemHistory(itemhist[0], dtTicks));
                            }
                        }
                    }
                }
            }
            return result
                .OrderByDescending(x => x.TimeOutTicks)
                .ToList();
        }

        public static IList<ItemHistory> AddHistory(string value, TimeSpan timeout, IList<ItemHistory> items)
        {
            var localnewhis = value.Trim();
            var found = items
                .Where(x => x.History.ToLowerInvariant() == localnewhis.ToLowerInvariant())
                .ToArray();
            if (found.Length > 0)
            {
                foreach (var item in found)
                {
                    items.Remove(item);
                }
            }
            if (items.Count >= byte.MaxValue)
            {
                items.RemoveAt(items.Count - 1);
            }
            items.Insert(0,
                ItemHistory.CreateItemHistory(localnewhis, timeout));
            return items;
        }

        public static void SaveHistory(string filename, IList<ItemHistory> items)
        {
            var file = string.Format(Filehistory, filename);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            if (!Directory.Exists(Path.Combine(userProfile, Folderhistory)))
            {
                Directory.CreateDirectory(Path.Combine(userProfile, Folderhistory));
            }

            File.WriteAllLines(Path.Combine(userProfile, Folderhistory, file),
                items.Where(x => DateTime.Now < new DateTime(x.TimeOutTicks))
                    .Select(x => x.ToString()), Encoding.UTF8);
        }

        public static void UpdateHistory(string filename, TimeSpan timeout)
        {
            var file = string.Format(Filehistory, filename);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            if (!Directory.Exists(Path.Combine(userProfile, Folderhistory)))
            {
                Directory.CreateDirectory(Path.Combine(userProfile, Folderhistory));
            }
            if (timeout.Ticks == 0)
            {
                File.Delete(Path.Combine(userProfile, Folderhistory, file));
                return;
            }
            var items = LoadHistory(filename);
            IList<ItemHistory> newitems = new List<ItemHistory>();
            foreach (var item in items)
            {
                if (DateTime.Now < new DateTime(item.TimeOutTicks))
                {
                    var diff = new DateTime(item.TimeOutTicks).Subtract(DateTime.Now);
                    if (diff < timeout)
                    {
                        newitems = AddHistory(item.History, diff, newitems);
                    }
                    else
                    {
                        newitems = AddHistory(item.History, timeout, newitems);
                    }
                }
            }
            File.WriteAllLines(Path.Combine(userProfile, Folderhistory, file),
                newitems.Select(x => x.ToString()), Encoding.UTF8);

        }
    }
}
