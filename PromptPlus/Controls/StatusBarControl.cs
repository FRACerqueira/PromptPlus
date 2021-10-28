// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Linq;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class StatusBarControl : IStatusBar, IStatusbarColumn, IStatusBarActions
    {
        private string _idtemplate;

        public IStatusBar Reset()
        {
            if (PromptPlus._statusBar.IsRunning)
            {
                throw new InvalidOperationException(Exceptions.Ex_StatusBarRunning);
            }
            PromptPlus._statusBar.Clear();
            return this;
        }

        public IStatusbarColumn AddTemplate(string id, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (PromptPlus._statusBar.IsRunning)
            {
                throw new InvalidOperationException(Exceptions.Ex_StatusBarRunning);
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Exceptions.Ex_InvalidValue, nameof(id));
            }
            if (!foregroundColor.HasValue)
            {
                foregroundColor = PromptPlus.ForeColor;
            }
            if (!backgroundColor.HasValue)
            {
                backgroundColor = PromptPlus.BackColor;
            }
            _idtemplate = id;
            PromptPlus._statusBar.Templates.Add(id, new StatusBarTemplate(id, foregroundColor.Value, backgroundColor.Value));
            return this;
        }

        public IStatusbarColumn AddSeparator()
        {
            PromptPlus._statusBar.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), "|", 1, true, true, StatusBarColAlignment.Left);
            return this;
        }

        public IStatusbarColumn AddColumn(string id, int lenght, StatusBarColAlignment alignment = StatusBarColAlignment.Left)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(id)));
            }
            PromptPlus._statusBar.Templates[_idtemplate].AddColumn(id, null, lenght, false, false, alignment);
            return this;
        }

        public IStatusbarColumn AddText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            PromptPlus._statusBar.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), text, text.Length, true, true, StatusBarColAlignment.Left);
            return this;
        }

        public IStatusBar Build(params string[] values)
        {
            if (PromptPlus._statusBar.Templates[_idtemplate].Columns.Count == 0)
            {
                PromptPlus._statusBar.Templates[_idtemplate].AddColumn(Guid.NewGuid().ToString(), null, -1, false, false, StatusBarColAlignment.Left);
            }
            UpdateColumns(values);
            foreach (var item in PromptPlus._statusBar.Templates[_idtemplate].Columns.ToArray())
            {
                PromptPlus._statusBar.UpdateChanged(_idtemplate, item.Id, true);
            }
            _idtemplate = null;
            return this;
        }

        public IStatusBarActions WithTemplate(string id)
        {
            if (!PromptPlus._statusBar.Templates.ContainsKey(id))
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
                index = PromptPlus._statusBar.Templates[_idtemplate].Columns.FindIndex(x => x.Id == idcolumn);
            }
            if (index < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(idcolumn)));
            }
            value = PromptPlus._statusBar.Templates[_idtemplate].Columns[index].Text;
            return this;
        }

        public IStatusBarActions UpdateColumn(string idcolumn, string value)
        {
            var index = -1;
            if (string.IsNullOrEmpty(idcolumn) && PromptPlus._statusBar.Templates[_idtemplate].Columns.Count == 1)
            {
                index = 0;
            }
            if (!string.IsNullOrEmpty(idcolumn))
            {
                index = PromptPlus._statusBar.Templates[_idtemplate].Columns.FindIndex(x => x.Id == idcolumn);
            }
            if (index < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(idcolumn)));
            }
            if (PromptPlus._statusBar.Templates[_idtemplate].Columns[index].Text != value)
            {
                PromptPlus._statusBar.UpdateChanged(_idtemplate, idcolumn, true);
                PromptPlus._statusBar.UpdateText(_idtemplate, idcolumn, value);
            }
            return this;
        }

        public IStatusBarActions UpdateColumns(params string[] values)
        {
            if (values.Length > 0)
            {
                if (values.Length != PromptPlus._statusBar.Templates[_idtemplate].Columns.Count(x => !x.IsText))
                {
                    throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, nameof(values)));
                }
                var i = 0;
                foreach (var item in PromptPlus._statusBar.Templates[_idtemplate].Columns)
                {
                    if (!item.IsText)
                    {
                        PromptPlus._statusBar.UpdateChanged(_idtemplate, item.Id, true);
                        PromptPlus._statusBar.UpdateText(_idtemplate, item.Id, values[i]);
                        i++;
                    }
                }
            }
            return this;
        }

        public void End()
        {
            PromptPlus._consoleDriver.Write("\x1b[;r".DefautColor());
            PromptPlus._consoleDriver.Write("\x1b[?1049l".DefautColor());
            PromptPlus._statusBar.LastTemplatesVisibles = 0;
            PromptPlus._statusBar.IsRunning = false;
        }

        public void Refresh()
        {
            End();
            foreach (var item in PromptPlus._statusBar.Templates.ToArray())
            {
                foreach (var itemcol in item.Value.Columns.ToArray())
                {
                    PromptPlus._statusBar.UpdateChanged(item.Value.Id, itemcol.Id, true);
                }
            }
            Run();
        }

        public void Run()
        {
            PromptPlus._statusBar.LastSize = new() { width = PromptPlus._consoleDriver.BufferWidth, height = PromptPlus._consoleDriver.BufferHeight };

            var oldcur = PromptPlus._consoleDriver.CursorVisible;
            PromptPlus._consoleDriver.CursorVisible = false;
            var qtd = PromptPlus._statusBar.Templates.Count;
            if (!PromptPlus._statusBar.IsRunning)
            {
                PromptPlus._consoleDriver.Write("\x1b[?1049h".DefautColor());
                if (!PromptPlus._consoleDriver.IsRunningTerminal)
                {
                    Console.BufferHeight = Console.WindowHeight;
                    Console.BufferWidth = Console.WindowWidth;
                }
                PromptPlus._consoleDriver.Write($"\x1b[0;{PromptPlus._consoleDriver.BufferHeight - qtd}r".DefautColor());
                PromptPlus._statusBar.IsRunning = true;
                PromptPlus._statusBar.LastTemplatesVisibles = qtd;
            }
            var l = PromptPlus._consoleDriver.CursorLeft;
            var t = PromptPlus._consoleDriver.CursorTop;

            var line = 0;
            foreach (var item in PromptPlus._statusBar.Templates)
            {
                PromptPlus._consoleDriver.SetCursorPosition(0, PromptPlus._consoleDriver.BufferHeight - qtd + line);
                var left = 0;
                foreach (var itemcol in item.Value.Columns.ToArray())
                {
                    var len = itemcol.Lenght;
                    if (len < 0)
                    {
                        len = PromptPlus._consoleDriver.BufferWidth - 1;
                    }
                    if (itemcol.LeftPos + len > PromptPlus._consoleDriver.BufferWidth - 1)
                    {
                        len -= itemcol.LeftPos + len - PromptPlus._consoleDriver.BufferWidth + 1;
                    }
                    if (itemcol.Changed && itemcol.LeftPos < PromptPlus._consoleDriver.BufferWidth)
                    {
                        WriteColumn(item.Value, itemcol.Text, itemcol.LeftPos, len, itemcol.Alignment);
                        PromptPlus._statusBar.UpdateChanged(item.Value.Id, itemcol.Id, false);
                    }
                    left += len;
                }
                PromptPlus._consoleDriver.SetCursorPosition(left, PromptPlus._consoleDriver.BufferHeight - qtd + line);
                PromptPlus._consoleDriver.ClearRestOfLine(item.Value.BackgroundColor);
                line++;
            }
            if (t >= PromptPlus._consoleDriver.BufferHeight - qtd)
            {
                t = PromptPlus._consoleDriver.BufferHeight - qtd - 1;
            }
            PromptPlus._consoleDriver.SetCursorPosition(l, t);
            PromptPlus._consoleDriver.CursorVisible = oldcur;
        }

        private void WriteColumn(StatusBarTemplate template, string item, int left, int len, StatusBarColAlignment alignment)
        {
            string result;
            switch (alignment)
            {
                case StatusBarColAlignment.Right:
                {
                    if (item.Length > len)
                    {
                        result = item.Substring(0, len);
                    }
                    else
                    {
                        result = (new string(' ', len - item.Length) + item).Substring(0, len);
                    }
                }
                break;
                case StatusBarColAlignment.Center:
                {
                    if (item.Length > len)
                    {
                        result = item.Substring(0, len);
                    }
                    else
                    {
                        var mid = (len - item.Length) / 2;
                        result = new string(' ', mid) + item;
                        result += new string(' ', mid + 1);
                        result = result.Substring(0, len);
                    }
                }
                break;
                default:
                    result = (item.PadRight(len, ' ')).Substring(0, len);
                    break;
            }
            PromptPlus._consoleDriver.SetCursorPosition(left, PromptPlus._consoleDriver.CursorTop);
            PromptPlus._consoleDriver.Write(result.Color(template.ForegroundColor, template.BackgroundColor));
        }

    }
}
