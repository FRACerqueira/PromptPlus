// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/WenceyWang/FIGlet.Net
// ***************************************************************************************

using PPlus.FIGlet;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PPlus.Controls
{
    internal class BannerControl : IBannerControl
    {

        private readonly IConsoleControl _console;
        private readonly ConfigControls _config;

        private BannerControl()
        {
            throw new PromptPlusException("BannerControl CTOR NotImplemented");
        }

        public BannerControl(ConfigControls config, IConsoleControl console, string value)
        {
            Text = value;
            _console = console;
            _config = config;
        }

        public string Text { get; private set; }
        public FIGletFont Font { get; private set; } = FIGletFont.Default;
        public CharacterWidth CharacterWidth { get; private set; } = CharacterWidth.Fitted;

        private string[] _result;
        private int Height => Font?.Height ?? 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0056:Use index operator", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1846:Prefer 'AsSpan' over 'Substring'", Justification = "<Pending>")]
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
                        for (var currentLine = 0; currentLine < Height; currentLine++)
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
                        for (var currentLine = 0; currentLine < Height; currentLine++)
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
                            _result[currentLine] = lineBuilder.ToString();
                        }
                        break;
                    }
            }
        }

        public IBannerControl LoadFont(Stream value)
        {
            if (value == null)
            {
                throw new PromptPlusException("BannerControl.LoadFont is null");
            }
            Font = new FIGletFont(value);
            return this;
        }

        public IBannerControl LoadFont(string value)
        {
            FIGletFont result;
            using (var fso = File.Open(value, FileMode.Open))
            {
                result = new FIGletFont(fso);
            }
            Font = result;
            return this;
        }

        public IBannerControl FIGletWidth(CharacterWidth value)
        {
            CharacterWidth = value;
            return this;
        }

        public void Run(Color? color = null, BannerDashOptions bannerDash = BannerDashOptions.None)
        {
            Color localcorlor = _console.ForegroundColor;
            if (color != null)
            {
                localcorlor = color.Value;
            }
            InitAsciiArt();
            var max = 0;
            foreach (var item in _result.Where(x => max < x.Length))
            {
                max = item.Length;
            }
            if (bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = _config.Symbols(SymbolType.SingleBorder).value[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = _config.Symbols(SymbolType.DoubleBorder).value[0];
                        break;
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = _config.Symbols(SymbolType.SingleBorder).unicode[0];
                        break;
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = _config.Symbols(SymbolType.DoubleBorder).unicode[0];
                        break;
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = _config.Symbols(SymbolType.HeavyBorder).unicode[0];
                        break;
                    default:
                        break;
                }
                if (!_console.IsUnicodeSupported && dach.HasValue)
                {
                    switch (bannerDash)
                    {
                        case BannerDashOptions.SingleBorderUpDown:
                            dach = _config.Symbols(SymbolType.SingleBorder).value[0];
                            break;
                        case BannerDashOptions.DoubleBorderUpDown:
                            dach = _config.Symbols(SymbolType.DoubleBorder).value[0];
                            break;
                        case BannerDashOptions.HeavyBorderUpDown:
                            dach = _config.Symbols(SymbolType.HeavyBorder).value[0];
                            break;
                        default:
                            break;
                    }
                }
                if (dach.HasValue)
                {
                    _console.WriteLine(new string(dach.Value, max), Style.Default.Foreground(localcorlor).Overflow(Overflow.Crop));
                }
            }
            foreach (var item in _result)
            {
                _console.WriteLine(item, Style.Default.Foreground(localcorlor).Overflow(Overflow.Crop));
            }
            if (bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderDown:
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = _config.Symbols(SymbolType.SingleBorder).value[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderDown:
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = _config.Symbols(SymbolType.DoubleBorder).value[0];
                        break;
                    case BannerDashOptions.SingleBorderDown:
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = _config.Symbols(SymbolType.SingleBorder).unicode[0];
                        break;
                    case BannerDashOptions.DoubleBorderDown:
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = _config.Symbols(SymbolType.DoubleBorder).unicode[0];
                        break;
                    case BannerDashOptions.HeavyBorderDown:
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = _config.Symbols(SymbolType.HeavyBorder).unicode[0];
                        break;
                    default:
                        break;
                }
                if (!_console.IsUnicodeSupported && dach.HasValue)
                {
                    switch (bannerDash)
                    {
                        case BannerDashOptions.SingleBorderDown:
                        case BannerDashOptions.SingleBorderUpDown:
                            dach = _config.Symbols(SymbolType.SingleBorder).value[0];
                            break;
                        case BannerDashOptions.DoubleBorderDown:
                        case BannerDashOptions.DoubleBorderUpDown:
                            dach = _config.Symbols(SymbolType.DoubleBorder).value[0];
                            break;
                        case BannerDashOptions.HeavyBorderDown:
                        case BannerDashOptions.HeavyBorderUpDown:
                            dach = _config.Symbols(SymbolType.HeavyBorder).value[0];
                            break;
                        default:
                            break;
                    }
                }
                if (dach.HasValue)
                {
                    _console.WriteLine(new string(dach.Value, max), Style.Default.Foreground(localcorlor).Overflow(Overflow.Crop));
                }
            }
        }
    }
}
