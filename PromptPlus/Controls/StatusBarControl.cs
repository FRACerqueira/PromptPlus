// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Linq;

using PromptPlusControls.Drivers;
using PromptPlusControls.Internal;
using PromptPlusControls.Resources;

namespace PromptPlusControls.Controls
{
    internal class StatusBarControl : IStatusBar, IStatusbarColumn, IStatusBarActions
    {
        private readonly IConsoleDriver _consoleDriver;
        private string _idtemplate;

        public StatusBarControl()
        {
            _consoleDriver = new ConsoleDriver();
        }

        public IStatusbarColumn AddTemplate(string id, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Exceptions.Ex_InvalidValue, nameof(id));
            }
            if (!foregroundColor.HasValue)
            {
                foregroundColor = PromptPlus.ColorSchema.ForeColorSchema;
            }
            if (!backgroundColor.HasValue)
            {
                backgroundColor = PromptPlus.ColorSchema.BackColorSchema;
            }
            _idtemplate = id;
            PromptPlus.s_topBarInfo.Templates.Add(id, new StatusBarTemplate(id, foregroundColor.Value, backgroundColor.Value));
            return this;
        }

        public IStatusbarColumn AddSeparator()
        {
            PromptPlus.s_topBarInfo.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), "|", 1, true, true);
            return this;
        }

        public IStatusbarColumn AddColumn(string id, int lenght)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(id)));
            }
            PromptPlus.s_topBarInfo.Templates[_idtemplate].AddColumn(id, null, lenght, false, false);
            return this;
        }

        public IStatusbarColumn AddText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            PromptPlus.s_topBarInfo.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), text, text.Length, true, true);
            return this;
        }

        public IStatusBar Build(params string[] values)
        {
            if (PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns.Count == 0)
            {
                PromptPlus.s_topBarInfo.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), null, -1, false, false);
            }
            UpdateColumns(values);
            foreach (var item in PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns)
            {
                item.Changed = true;
            }
            _idtemplate = null;
            return this;
        }

        public IStatusBarActions WithTemplate(string id)
        {
            if (!PromptPlus.s_topBarInfo.Templates.ContainsKey(id))
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(id)));
            }
            _idtemplate = id;
            return this;
        }

        public IStatusBarActions TryValue(string idcolumn, out string value)
        {
            var index = -1;
            if (!string.IsNullOrEmpty(idcolumn))
            {
                index = PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns.FindIndex(x => x.Id == idcolumn);
            }
            if (index < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(idcolumn)));
            }
            value = PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns[index].Text;
            return this;
        }

        public IStatusBarActions UpdateColumn(string value, string idcolumn = null)
        {
            var index = -1;
            if (string.IsNullOrEmpty(idcolumn) && PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns.Count == 1)
            {
                index = 0;
            }
            if (!string.IsNullOrEmpty(idcolumn))
            {
                index = PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns.FindIndex(x => x.Id == idcolumn);
            }
            if (index < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(idcolumn)));
            }
            if (PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns[index].Text != value)
            {
                PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns[index].Changed = true;
                PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns[index].Text = value;
            }
            return this;
        }

        public IStatusBarActions UpdateColumns(params string[] values)
        {
            if (values.Length > 0)
            {
                if (values.Length != PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns.Count(x => !x.IsText))
                {
                    throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(values)));
                }
                var i = 0;
                foreach (var item in PromptPlus.s_topBarInfo.Templates[_idtemplate].Columns)
                {
                    if (!item.IsText)
                    {
                        item.Text = values[i];
                        item.Changed = true;
                        i++;
                    }
                }
            }
            return this;
        }

        public void End()
        {
            PromptPlus.s_topBarInfo.LastTemplatesVisibles = -1;
            PromptPlus.s_topBarInfo.Clear();
            var l = _consoleDriver.CursorLeft;
            var t = _consoleDriver.CursorTop;
            _consoleDriver.Write("\x1b[;r", PromptPlus.ColorSchema.ForeColorSchema);
            _consoleDriver.SetCursorPosition(l, t);
        }

        public void Run()
        {
            var oldcur = _consoleDriver.CursorVisible;
            _consoleDriver.CursorVisible = false;
            var l = _consoleDriver.CursorLeft;
            var t = _consoleDriver.CursorTop;

            var qtd = PromptPlus.s_topBarInfo.Templates.Count;
            if (PromptPlus.s_topBarInfo.LastTemplatesVisibles < 0)
            {
                _consoleDriver.Write($"\x1b[;{_consoleDriver.BufferHeight-qtd}r", PromptPlus.ColorSchema.ForeColorSchema);
            }
            var line = 0;
            foreach (var item in PromptPlus.s_topBarInfo.Templates)
            {
                _consoleDriver.SetCursorPosition(0, _consoleDriver.BufferHeight - qtd + line);
                var left = 0;
                foreach (var itemcol in item.Value.Columns)
                {
                    var len = itemcol.Lenght;
                    if (len <= 0)
                    {
                        len = _consoleDriver.BufferWidth - 1;
                    }
                    if (itemcol.Changed)
                    {
                        WriteColumn(item.Value, itemcol.Text, left, len);
                        itemcol.Changed = false;
                    }
                    left += len;
                }
                _consoleDriver.SetCursorPosition(left, _consoleDriver.BufferHeight - qtd + line);
                _consoleDriver.ClearRestOfLine(item.Value.BackgroundColor);
                line++;
            }
            if (PromptPlus.s_topBarInfo.LastTemplatesVisibles < 0)
            {
                PromptPlus.s_topBarInfo.LastTemplatesVisibles = qtd;
           }
            if (t >= _consoleDriver.BufferHeight - qtd)
            {
                t = _consoleDriver.BufferHeight - qtd - 1;
            }
            _consoleDriver.SetCursorPosition(l, t);
            _consoleDriver.CursorVisible = oldcur;
        }


        private void WriteColumn(StatusBarTemplate template, string item, int left, int len)
        {
            var result = (item.PadRight(len, ' ')).Substring(0, len);
            _consoleDriver.SetCursorPosition(left, _consoleDriver.CursorTop);
            _consoleDriver.Write(result,
                template.ForegroundColor,
                template.BackgroundColor);
        }

    }
}
