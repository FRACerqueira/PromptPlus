// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusControls.Internal
{
    internal class StatusBar
    {
        public StatusBar()
        {
            Templates = new Dictionary<string, StatusBarTemplate>();
            LastTemplatesVisibles = -1;
        }
        public Dictionary<string, StatusBarTemplate> Templates { get; set; }
        public bool TryValue(StatusBarTemplate template, string column, out string value)
        {
            var index = template.Columns.FindIndex(x => x.Id == column);
            if (index < 0)
            {
                value = null;
                return false;
            }
            value = template.Columns[index].Text ?? string.Empty;
            return true;
        }
        public bool TryUpdate(StatusBarTemplate template, string column, string value)
        {
            var index = template.Columns.FindIndex(x => x.Id == column);
            if (index < 0)
            {
                return false;
            }
            template.Columns[index].Text = value;
            return true;
        }
        public void AddTemplate(StatusBarTemplate value)
        {
            if (Templates.ContainsKey(value.Id))
            {
                Templates[value.Id] = value;
            }
            else
            {
                Templates.Add(value.Id, value);
            }
        }
        public bool RemoveTemplate(string id)
        {
            return Templates.Remove(id);
        }
        public void Clear()
        {
            Templates.Clear();
        }
        public int LastTemplatesVisibles { get; internal set; }
    }

    internal class StatusBarTemplate
    {
        public StatusBarTemplate(string id, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Id = id;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Columns = new List<StatusBarColumn>();
        }
        public string Id { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }
        public List<StatusBarColumn> Columns { get; private set; }
        public void AddColumn(string id, string text, int lenght, bool istext, bool changed)
        {
            Columns.Add(new StatusBarColumn(id, text, lenght, istext, changed));
        }
    }

    internal class StatusBarColumn
    {
        public StatusBarColumn(string id, string text, int lenght, bool istext, bool changed)
        {
            Id = id;
            Lenght = lenght;
            IsText = istext;
            Text = text ?? string.Empty;
            Changed = changed;
        }
        public string Id { get; private set; }
        public string Text { get; set; }
        public int Lenght { get; private set; }
        public bool IsText { get; private set; }
        public bool Changed { get; set; }
    }
}
