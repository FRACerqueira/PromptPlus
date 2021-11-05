// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

using PromptPlusControls.Controls;
using PromptPlusControls.FIGlet;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {

        #region Console Commands

        public static ResultPromptPlus<ConsoleKeyInfo> WaitKeypress(CancellationToken cancellationToken)
        {
            return new ResultPromptPlus<ConsoleKeyInfo>(_consoleDriver.WaitKeypress(cancellationToken), cancellationToken.IsCancellationRequested);
        }

        public static void ClearLine(int top)
        {
            if (top < 0)
            {
                top = 0;
            }
            if (top > _consoleDriver.BufferHeight - 1)
            {
                top = _consoleDriver.BufferHeight - 1;
            }
            _consoleDriver.ClearLine(top);
        }

        public static void ClearRestOfLine(ConsoleColor? color = null) => _consoleDriver.ClearRestOfLine(color ?? BackgroundColor);

        public static void Beep() => _consoleDriver.Beep();

        public static int CursorLeft => _consoleDriver.CursorLeft;

        public static int CursorTop => _consoleDriver.CursorTop;

        public static int BufferWidth => Console.BufferWidth;

        public static int BufferHeight => Console.BufferHeight;

        public static int WindowWidth => Console.WindowWidth;

        public static int WindowHeight => Console.WindowHeight;

        public static bool IsRunningTerminal => _consoleDriver.IsRunningTerminal;

        public static ConsoleColor ForegroundColor => _consoleDriver.ForegroundColor;

        public static ConsoleColor BackgroundColor => _consoleDriver.BackgroundColor;

        public static void CursorPosition(int left, int top)
        {
            lock (_lockobj)
            {
                if (top < 0)
                {
                    top = 0;
                }
                if (left < 0)
                {
                    left = 0;
                }
                if (top > _consoleDriver.BufferHeight - 1)
                {
                    top = _consoleDriver.BufferHeight - 1;
                }
                _consoleDriver.SetCursorPosition(left, top);
            }
        }

        public static void ConsoleDefaultColor(ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
        {
            lock (_lockobj)
            {
                if (forecolor.HasValue)
                {
                    _consoleDriver.ForegroundColor = forecolor.Value;
                }
                if (backcolor.HasValue)
                {
                    _consoleDriver.BackgroundColor = backcolor.Value;
                }
            }
        }

        public static void Clear(ConsoleColor? backcolor = null)
        {
            lock (_lockobj)
            {
                if (backcolor.HasValue)
                {
                    _consoleDriver.BackgroundColor = backcolor.Value;
                }
                _consoleDriver.Clear();
            }
        }

        public static void WriteLineSkipColors(string value)
        {
            _consoleDriver.WriteLine(value);
        }

        public static void WriteLine(Exception value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
        {
            WriteLine(value.ToString().Color(forecolor ?? PromptPlus.ForegroundColor, backcolor));
        }

        public static void WriteLine(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
        {
            if (underline)
            {
                var aux = ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor);
                foreach (var item in aux)
                {
                    WriteLine(item.Underline());
                }
            }
            else
            {
                WriteLine(ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor));
            }
        }

        public static void Write(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
        {
            if (underline)
            {
                var aux = ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor);
                foreach (var item in aux)
                {
                    Write(item.Underline());
                }
            }
            else
            {
                Write(ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor));
            }
        }

        public static void WriteSkipColors(string value)
        {
            _consoleDriver.Write(value);
        }

        public static void Write(params ColorToken[] tokens)
        {
            _consoleDriver.Write(tokens);
        }

        public static void WriteLine(params ColorToken[] tokens)
        {
            _consoleDriver.WriteLine(tokens);
        }

        public static void WriteLines(int value = 1)
        {
            if (value < 1)
            {
                return;
            }
            for (var i = 0; i < value; i++)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Allows a string to be written with embedded color values using:
        /// This is [red]Red[/red] text and this is [white:blue!u]Blue[/white:blue!u] text
        /// </summary>
        /// <param name="text">Text to display</param>
        private static ColorToken[] ConvertEmbeddedColorLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new[] { new ColorToken(text) };
            }

            var at = text.IndexOf("[");
            var at2 = text.IndexOf("]");
            if (at == -1 || at2 <= at)
            {
                return new[] { new ColorToken(text) };
            }

            var result = new List<ColorToken>();
            var match = colorBlockRegEx.Value.Match(text);
            while (match.Length >= 1)
            {
                // write up to expression
                result.Add(new ColorToken(text.Substring(0, match.Index)));

                // strip out the expression
                var highlightText = match.Groups["text"].Value;
                var colvalfc = _consoleDriver.ForegroundColor;
                var colvalbc = _consoleDriver.BackgroundColor;

                var underline = false;
                var mathgrp = match.Groups["color"].Value;
                if (mathgrp.StartsWith("!!"))
                {
                    result.Add(new ColorToken(highlightText));
                }
                else
                {
                    //find underline tolen
                    if (mathgrp.IndexOf("!u", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        underline = true;
                        mathgrp = mathgrp.Replace("!u", "", StringComparison.InvariantCultureIgnoreCase);
                    }
                    //split color

                    var colors = mathgrp.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (colors.Length == 1)
                    {
                        Enum.TryParse(colors[0], true, out colvalfc);
                    }
                    else if (colors.Length == 2)
                    {
                        Enum.TryParse(colors[0], true, out colvalfc);
                        Enum.TryParse(colors[1], true, out colvalbc);
                    }
                    result.Add(new ColorToken(highlightText, colvalfc, colvalbc, underline));
                }

                // remainder of string
                text = text.Substring(match.Index + match.Value.Length);
                match = colorBlockRegEx.Value.Match(text);
                if (match.Length < 1 && text.Length > 0)
                {
                    result.Add(new ColorToken(text));
                }
            }
            return result.ToArray();
        }

        private static readonly Lazy<Regex> colorBlockRegEx = new(
            () => new Regex("\\[(?<color>.*?)\\](?<text>[^[]*)\\[/\\k<color>\\]", RegexOptions.IgnoreCase),
            isThreadSafe: true);

        #endregion

        #region controls

        public static IFIGlet Banner(string value)
        {
            return new BannerControl(value);
        }

        public static IControlKeyPress KeyPress(char? Keypress = null, ConsoleModifiers? keymodifiers = null)
        {
            return new keyPressControl(new KeyPressOptions { KeyPress = Keypress, KeyModifiers = keymodifiers });
        }

        public static IControlMaskEdit MaskEdit(MaskedType type, string prompt = null)
        {
            return type switch
            {
                MaskedType.Generic => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Generic, Message = prompt }),
                MaskedType.DateOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateOnly, Message = prompt }),
                MaskedType.TimeOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.TimeOnly, Message = prompt }),
                MaskedType.DateTime => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateTime, Message = prompt }),
                MaskedType.Number => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Number, Message = prompt }),
                MaskedType.Currency => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Currency, Message = prompt }),
                _ => throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, type))
            };
        }

        public static IControlInput Input(string prompt = null)
        {
            return new InputControl(new InputOptions() { Message = prompt });
        }

        public static IControlConfirm Confirm(string prompt = null)
        {
            return new ConfirmControl(new ConfirmOptions() { Message = prompt });
        }

        public static IControlSliderNumber SliderNumber(SliderNumberType type, string prompt = null)
        {
            return new SliderNumberControl(new SliderNumberOptions() { Message = prompt, Type = type });
        }

        public static IControlSliderSwitche SliderSwitche(string prompt = null)
        {
            return new SliderSwitcheControl(new SliderSwitcheOptions() { Message = prompt });
        }

        public static IControlProgressbar Progressbar(string prompt = null)
        {
            return new ProgressBarControl(new ProgressBarOptions() { Message = prompt });
        }

        public static IControlWaitProcess WaitProcess(string prompt = null)
        {
            return new WaitProcessControl(new WaitProcessOptions() { Message = prompt });
        }

        public static IControlSelect<T> Select<T>(string prompt = null)
        {
            return new SelectControl<T>(new SelectOptions<T>() { Message = prompt });
        }

        public static IControlMultiSelect<T> MultiSelect<T>(string prompt = null)
        {
            return new MultiSelectControl<T>(new MultiSelectOptions<T>() { Message = prompt });
        }

        public static IControlBrowser Browser(string prompt = null)
        {
            return new BrowserControl(new BrowserOptions() { Message = prompt });
        }

        public static IControlList<T> List<T>(string prompt = null)
        {
            return new ListControl<T>(new ListOptions<T>() { Message = prompt });
        }

        public static IControlListMasked ListMasked(string prompt = null)
        {
            return new MaskedListControl(new ListOptions<string>() { Message = prompt });
        }

        public static IControlPipeLine Pipeline()
        {
            return new PipeLineControl();
        }

        #endregion
    }
}

