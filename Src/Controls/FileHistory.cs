// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Environment;

namespace PPlus.Controls
{
    internal static class FileHistory
    {
        private const string Folderhistory = "PromptPlus.History";
        private const string Filehistory = "{0}.txt";

        public static TimeSpan DefaultHistoryTimeout => TimeSpan.FromDays(365);
        public static IList<ItemHistory> LoadHistory(string filename, byte? maxitem = byte.MaxValue)
        {
            if (!maxitem.HasValue)
            {
                maxitem = byte.MaxValue;
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new PromptPlusException("FileHistory.LoadHistory Null Or Empty");
            }
            var localfilename = UniqueDomain(filename);
            var file = string.Format(Filehistory, localfilename);
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
                .Take(maxitem.Value)
                .ToList();
        }

        public static IList<ItemHistory> AddHistory(string value, TimeSpan timeout, IList<ItemHistory>? items)
        {
            items ??= new List<ItemHistory>();
            if (string.IsNullOrEmpty(value?.Trim() ?? string.Empty))
            {
                return items;
            }
            var found = items
                .Where(x => x.History.ToLowerInvariant() == value.ToLowerInvariant())
                .ToArray();
            if (found.Length > 0)
            {
                foreach (var item in found)
                {
                    items.Remove(item);
                }
            }
            items.Insert(0,
                ItemHistory.CreateItemHistory(value, timeout));
            return items;
        }

        public static void SaveHistory(string filename, IList<ItemHistory> items, byte? maxitem = byte.MaxValue)
        {
            if (!maxitem.HasValue)
            {
                maxitem = byte.MaxValue;
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new PromptPlusException("FileHistory.SaveHistory Null Or Empty");
            }

            var localfilename = UniqueDomain(filename);
            var file = string.Format(Filehistory, localfilename);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            if (!Directory.Exists(Path.Combine(userProfile, Folderhistory)))
            {
                Directory.CreateDirectory(Path.Combine(userProfile, Folderhistory));
            }
            if (items is null || items.Count == 0)
            {
                if (File.Exists(Path.Combine(userProfile, Folderhistory, file)))
                {
                    File.Delete(Path.Combine(userProfile, Folderhistory, file));
                }
                return;
            }

            while (items.Count > maxitem)
            {
                items.RemoveAt(items.Count - 1);
            }

            File.WriteAllLines(Path.Combine(userProfile, Folderhistory, file),
                items.Where(x => DateTime.Now < new DateTime(x.TimeOutTicks))
                    .Select(x => x.ToString()));
        }

        public static void ClearHistory(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new PromptPlusException("FileHistory.ClearHistory Null Or Empty");
            }
            var localfilename = UniqueDomain(filename);
            var file = string.Format(Filehistory, localfilename);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            if (File.Exists(Path.Combine(userProfile, Folderhistory, file)))
            {
                File.Delete(Path.Combine(userProfile, Folderhistory, file));
            }
        }
 
        private static string UniqueDomain(string value)
        {
            string name = Assembly.GetEntryAssembly().GetName().Name;
            return $"{name}.{value}";
        }
    }
}
