// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Text;

using PPlus.FIGlet;

using PPlus.Internal;

namespace PPlus.Controls
{

    internal class BannerControl : IFIGlet
    {
        private readonly ScreenRender _screenrender;
        private ConsoleColor _color = PromptPlus.ForegroundColor;

        public BannerControl(string value)
        {
            Text = value;
            _screenrender = new ScreenRender();
        }

        public string Text { get; private set; }
        public FIGletFont Font { get; private set; } = FIGletFont.Default;
        public CharacterWidth CharacterWidth { get; private set; } = CharacterWidth.Fitted;
        public string[] Result { get; private set; }
        public int Height => Font?.Height ?? 0;
        public int Width => Result.Max(line => line?.Length ?? 0);

        public void InitAsciiArt()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            Text = Text.Replace(Environment.NewLine, "");

            Result = new string[Font.Height];

            switch (CharacterWidth)
            {
                case CharacterWidth.Full:
                {
                    for (var currentLine = 0; currentLine < Height; currentLine++)
                    {
                        var lineBuilder = new StringBuilder();
                        foreach (var currentChar in Text)
                        {
                            lineBuilder.Append(Font.GetCharacter(currentChar, currentLine));
                            lineBuilder.Append(' ');
                        }
                        Result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
                case CharacterWidth.Fitted:
                {
                    for (var currentLine = 0; currentLine < Height; currentLine++)
                    {
                        var lineBuilder = new StringBuilder();
                        foreach (var currentChar in Text)
                        {
                            lineBuilder.Append(Font.GetCharacter(currentChar, currentLine));
                        }
                        Result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
                case CharacterWidth.Smush:
                {
                    for (var currentLine = 0; currentLine < Height; currentLine++)
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
                        Result[currentLine] = lineBuilder.ToString();
                    }
                    break;
                }
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
                LoadFont(fso);
            }
            return this;
        }

        public IFIGlet FIGletWidth(CharacterWidth value)
        {
            CharacterWidth = value;
            return this;
        }


        public void Run(ConsoleColor? color = null)
        {
            _color = color ?? PromptPlus.ForegroundColor;
            InitAsciiArt();
            _screenrender.InputRender(InputTemplate);
        }

        private void InputTemplate(ScreenBuffer screenBuffer)
        {
            foreach (var item in Result)
            {
                screenBuffer.WriteLine(item, _color);
            }
            screenBuffer.WriteLine();
            screenBuffer.PushCursor();
        }
    }
}
