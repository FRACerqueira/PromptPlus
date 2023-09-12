// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.FIGletCore;
using System.IO;
using System.Linq;

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
            Font = new FigletFont();
        }

        public string Text { get; private set; }

        public FigletFont Font { get; private set; }

        public IBannerControl LoadFont(Stream value)
        {
            if (value == null)
            {
                throw new PromptPlusException("BannerControl.LoadFont is null");
            }
            Font = new FigletFont(value);
            return this;
        }

        public IBannerControl LoadFont(string value)
        {
            if (value == null)
            {
                throw new PromptPlusException("BannerControl.LoadFont is null");
            }
            Font = new FigletFont(value);
            return this;
        }


        public void Run(Color? color = null, BannerDashOptions bannerDash = BannerDashOptions.None)
        {
            Color localcorlor = _console.ForegroundColor;
            if (color != null)
            {
                localcorlor = color.Value;
            }
            var result = Font.ToAsciiArt(Text); 
            var max = 0;
            foreach (var item in result.Where(x => max < x.Length))
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
            foreach (var item in result)
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
