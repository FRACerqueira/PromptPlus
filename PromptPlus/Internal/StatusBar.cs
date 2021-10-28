// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Internal
{
    internal struct StatusBar
    {
        public StatusBar(int heiht, int width)
        {
            Templates = new Dictionary<string, StatusBarTemplate>();
            LastTemplatesVisibles = 0;
            IsRunning = false;
            LastSize = new() { height = heiht, width = width };
        }

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
            UpdateText(template.Id, template.Columns[index].Id, value);
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

        public Dictionary<string, StatusBarTemplate> Templates { get; set; }
        public int LastTemplatesVisibles { get; set; }
        public bool IsRunning { get; set; }
        public (int width, int height) LastSize { get; set; }

        public void UpdateText(string idtemplate, string idcol, string value)
        {
            int index;
            if (string.IsNullOrEmpty(idcol))
            {
                index = Templates[idtemplate].Columns.FindIndex(x => !x.IsText);
            }
            else
            {
                index = Templates[idtemplate].Columns.FindIndex(x => x.Id == idcol);
            }
            var aux = Templates[idtemplate].Columns[index];
            Templates[idtemplate].Columns[index] = new StatusBarColumn(aux.Id, value, aux.Lenght, aux.IsText, aux.Changed, aux.LeftPos, aux.Alignment);
        }

        public void UpdateChanged(string idtemplate, string idcol, bool value)
        {
            int index;
            if (string.IsNullOrEmpty(idcol))
            {
                index = Templates[idtemplate].Columns.FindIndex(x => !x.IsText);
            }
            else
            {
                index = Templates[idtemplate].Columns.FindIndex(x => x.Id == idcol);
            }
            var aux = Templates[idtemplate].Columns[index];
            Templates[idtemplate].Columns[index] = new StatusBarColumn(aux.Id, aux.Text, aux.Lenght, aux.IsText, value, aux.LeftPos, aux.Alignment);
        }

    }

    internal struct StatusBarTemplate
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
        public List<StatusBarColumn> Columns { get; set; }
        public void AddColumn(string id, string text, int lenght, bool istext, bool changed, StatusBarColAlignment alignment)
        {
            var leftpos = Columns.Sum(x => x.IsText ? x.Text.Length : x.Lenght);
            Columns.Add(new StatusBarColumn(id, text, lenght, istext, changed, leftpos, alignment));
        }
    }

    internal struct StatusBarColumn
    {
        public StatusBarColumn(string id, string text, int lenght, bool istext, bool changed, int leftpos, StatusBarColAlignment alignment)
        {
            Id = id;
            Lenght = lenght;
            IsText = istext;
            Text = text ?? string.Empty;
            Changed = changed;
            LeftPos = leftpos;
            Alignment = alignment;
        }
        public string Text { get; set; }
        public bool Changed { get; set; }

        public string Id { get; private set; }
        public int Lenght { get; private set; }
        public bool IsText { get; private set; }
        public int LeftPos { get; private set; }
        public StatusBarColAlignment Alignment { get; private set; }

    }
}
