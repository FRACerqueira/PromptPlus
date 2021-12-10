// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Logging;

using PPlus.FIGlet;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Controls
{

    internal class BannerControl : IFIGlet
    {
        private const string SourceLog = "PromptPlus.Banner";

        private readonly ScreenRender _screenrender;
        private ConsoleColor _color = PromptPlus.ForegroundColor;
        private readonly ControlLog _logs = new();

        public BannerControl(string value)
        {
            Text = value;
            _screenrender = new ScreenRender();
        }

        public string Text { get; private set; }
        public FIGletFont Font { get; private set; } = FIGletFont.Default;
        public CharacterWidth CharacterWidth { get; private set; } = CharacterWidth.Fitted;

        private string[] _result;
        private int _height => Font?.Height ?? 0;

        public void InitAsciiArt()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            Text = Text.Replace(Environment.NewLine, "");

            _result = new string[Font.Height];

            switch (CharacterWidth)
            {
                case CharacterWidth.Full:
                {
                    for (var currentLine = 0; currentLine < _height; currentLine++)
                    {
                        var lineBuilder = new StringBuilder();
                        foreach (var currentChar in Text)
                        {
                            lineBuilder.Append(Font.GetCharacter(currentChar, currentLine));
                            lineBuilder.Append(' ');
                        }
                        _result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
                case CharacterWidth.Fitted:
                {
                    for (var currentLine = 0; currentLine < _height; currentLine++)
                    {
                        var lineBuilder = new StringBuilder();
                        foreach (var currentChar in Text)
                        {
                            lineBuilder.Append(Font.GetCharacter(currentChar, currentLine));
                        }
                        _result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
                case CharacterWidth.Smush:
                {
                    for (var currentLine = 0; currentLine < _height; currentLine++)
                    {
                        var lineBuilder = new StringBuilder();
                        lineBuilder.Append(Font.GetCharacter(Text[0], currentLine));
                        var lastChar = Text[0];
                        for (var currentCharIndex = 1; currentCharIndex < Text.Length; currentCharIndex++)
                        {
                            var currentChar = Text[currentCharIndex];
                            var currentCharacterLine = Font.GetCharacter(currentChar, currentLine);
                            if (lastChar != ' ' && currentChar != ' ')
                            {
                                if (lineBuilder[lineBuilder.Length - 1] == ' ')
                                {
                                    lineBuilder[lineBuilder.Length - 1] = currentCharacterLine[0];
                                }
                                lineBuilder.Append(currentCharacterLine.Substring(1));
                            }
                            else
                            {
                                lineBuilder.Append(currentCharacterLine);
                            }
                            lastChar = currentChar;
                        }
                        _result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
            }
        }

        private void LoadFont(Stream value, bool withlog)
        {
            LoadFont(value);
            if (withlog)
            {
                _logs.Add(LogLevel.Debug, "LoadFont", "stream", SourceLog, LogKind.Property);
            }
        }

        public IFIGlet LoadFont(Stream value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            Font = new FIGletFont(value);
            return this;
        }

        public IFIGlet LoadFont(string value)
        {
            using (var fso = File.Open(value, FileMode.Open))
            {
                LoadFont(fso,false);
                _logs.Add(LogLevel.Debug, "LoadFont", value, SourceLog, LogKind.Property);

            }
            return this;
        }

        public IFIGlet FIGletWidth(CharacterWidth value)
        {
            CharacterWidth = value;
            return this;
        }


        public ResultPromptPlus<string> Run(ConsoleColor? color = null)
        {
            _color = color ?? PromptPlus.ForegroundColor;
            InitAsciiArt();
            _logs.Add(LogLevel.Debug, "CharacterWidth", CharacterWidth.ToString(), SourceLog, LogKind.Property);
            _logs.Add(LogLevel.Debug, "Color", _color.ToString(), SourceLog, LogKind.Property);

            _screenrender.ClearBuffer();
            _screenrender.InputRender(InputTemplate);

            PromptPlus.WriteLog(_logs);

            return new ResultPromptPlus<string>(Text, false, false, _logs);
        }

        private void InputTemplate(ScreenBuffer screenBuffer)
        {
            foreach (var item in _result)
            {
                screenBuffer.WriteLine(item, _color);
            }
            screenBuffer.WriteLine();
            screenBuffer.PushCursor();
        }
    }
}
