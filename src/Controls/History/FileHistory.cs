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

namespace PromptPlusLibrary.Controls.History
{
    internal static class FileHistory
    {
        private const string Folderhistory = "PromptPlus.History";
        private static readonly string UserProfilePath = GetFolderPath(SpecialFolder.UserProfile);
        private static readonly string HistoryFolderPath = Path.Combine(UserProfilePath, Folderhistory);

        public static TimeSpan DefaultHistoryTimeout => TimeSpan.FromDays(365);

        public static IList<ItemHistory> LoadHistory(string filename, byte? maxitem = byte.MaxValue)
        {
            maxitem ??= byte.MaxValue;

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("FileHistory.LoadHistory Null Or Empty", nameof(filename));
            }

            string filePath = GetFilePath(filename);
            List<ItemHistory> result = [];

            if (File.Exists(filePath))
            {
                DateTime now = DateTime.Now;
                string[] rawLines = File.ReadAllLines(filePath);
                foreach (string line in rawLines)
                {
                    string[] parts = line.Split(ItemHistory.Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && long.TryParse(parts[1], out long dtTicks) && now < new DateTime(dtTicks))
                    {
                        result.Add(new ItemHistory(parts[0], dtTicks));
                    }
                }
            }

            return [.. result
                .OrderByDescending(x => x.TimeOutTicks)
                .Take(maxitem.Value)];
        }

        public static IList<ItemHistory> AddHistory(string value, TimeSpan timeout, IList<ItemHistory>? items)
        {
            items ??= [];

            if (string.IsNullOrWhiteSpace(value))
            {
                return items;
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].History.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    items.RemoveAt(i);
                }
            }

            items.Insert(0, ItemHistory.CreateItemHistory(value, timeout));
            return items;
        }

        public static void SaveHistory(string filename, IList<ItemHistory> items, byte? maxitem = byte.MaxValue)
        {
            maxitem ??= byte.MaxValue;

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("FileHistory.SaveHistory Null Or Empty", nameof(filename));
            }

            string filePath = GetFilePath(filename);

            if (!Directory.Exists(HistoryFolderPath))
            {
                Directory.CreateDirectory(HistoryFolderPath);
            }

            if (items.Count == 0)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return;
            }

            while (items.Count > maxitem.Value)
            {
                items.RemoveAt(items.Count - 1);
            }

            DateTime now = DateTime.Now;
            string[] lines = [.. items
                .Where(x => now < new DateTime(x.TimeOutTicks))
                .Select(x => x.ToString())];

            File.WriteAllLines(filePath, lines);
        }

        public static void ClearHistory(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("FileHistory.ClearHistory Null Or Empty", nameof(filename));
            }

            string filePath = GetFilePath(filename);

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

        private static string GetFilePath(string filename) =>
            Path.Combine(HistoryFolderPath, $"{UniqueDomain(filename)}.txt");
    }
}
