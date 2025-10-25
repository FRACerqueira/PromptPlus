// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Environment;

namespace PromptPlusLibrary.Controls
{
    internal static class FileHistory
    {
        private const string Folderhistory = "PromptPlus.History";
        private const string Filehistory = "{0}.txt";

        public static TimeSpan DefaultHistoryTimeout => TimeSpan.FromDays(365);

        public static IList<ItemHistory> LoadHistory(string filename, byte? maxitem = byte.MaxValue)
        {
            maxitem ??= byte.MaxValue;

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "FileHistory.LoadHistory Null Or Empty");
            }

            string localfilename = UniqueDomain(filename);
            string file = string.Format(Filehistory, localfilename);
            string userProfile = GetFolderPath(SpecialFolder.UserProfile);
            string filePath = Path.Combine(userProfile, Folderhistory, file);
            List<ItemHistory> result = [];

            if (File.Exists(filePath))
            {
                string[] aux = File.ReadAllLines(filePath);
                foreach (string item in aux)
                {
                    string[] itemhist = item.Split(ItemHistory.Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (itemhist.Length == 2 && long.TryParse(itemhist[1], out long dtTicks))
                    {
                        if (DateTime.Now < new DateTime(dtTicks))
                        {
                            result.Add(new ItemHistory(itemhist[0], dtTicks));
                        }
                    }
                }
            }

            return [.. result
                .OrderByDescending(x => x.TimeOutTicks)
                .Take(maxitem.Value)];
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1309:Use ordinal string comparison", Justification = "By design")]
        public static IList<ItemHistory> AddHistory(string value, TimeSpan timeout, IList<ItemHistory>? items)
        {
            items ??= [];

            if (string.IsNullOrWhiteSpace(value))
            {
                return items;
            }

            ItemHistory[] found = [.. items.Where(x => x.History.Equals(value, StringComparison.InvariantCultureIgnoreCase))];

            foreach (ItemHistory item in found)
            {
                items.Remove(item);
            }

            items.Insert(0, ItemHistory.CreateItemHistory(value, timeout));
            return items;
        }

        public static void SaveHistory(string filename, IList<ItemHistory> items, byte? maxitem = byte.MaxValue)
        {
            maxitem ??= byte.MaxValue;

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "FileHistory.SaveHistory Null Or Empty");
            }

            string localfilename = UniqueDomain(filename);
            string file = string.Format(Filehistory, localfilename);
            string userProfile = GetFolderPath(SpecialFolder.UserProfile);
            string folderPath = Path.Combine(userProfile, Folderhistory);
            string filePath = Path.Combine(folderPath, file);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (items == null || items.Count == 0)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return;
            }

            while (items.Count > maxitem)
            {
                items.RemoveAt(items.Count - 1);
            }

            string[] lines = [.. items
                .Where(x => DateTime.Now < new DateTime(x.TimeOutTicks))
                .Select(x => x.ToString())];

            File.WriteAllLines(filePath, lines);
        }

        public static void ClearHistory(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "FileHistory.ClearHistory Null Or Empty");
            }

            string localfilename = UniqueDomain(filename);
            string file = string.Format(Filehistory, localfilename);
            string userProfile = GetFolderPath(SpecialFolder.UserProfile);
            string filePath = Path.Combine(userProfile, Folderhistory, file);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string UniqueDomain(string value)
        {
            string name = Assembly.GetEntryAssembly()!.GetName().Name!;
            return $"{name}.{value}";
        }
    }
}
