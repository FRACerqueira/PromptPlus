// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Widgets.Banner.FIGlet;
using System;
using System.IO;
using System.Linq;

namespace PromptPlusLibrary.Widgets.Banner
{
    internal sealed class BannerWidget(IConsoleExtend console, PromptConfig promptConfig, string text, Style style) : IBanner
    {
        private static readonly FigletFont _fontDefault = new();
        private FigletFont? _fontloaded;
        private BannerDashOptions _bannerDash = BannerDashOptions.None;

        public IBanner Border(BannerDashOptions dashOptions)
        {
            _bannerDash = dashOptions;
            return this;
        }

        public IBanner FromFont(string filepathFont)
        {
            ArgumentException.ThrowIfNullOrEmpty(filepathFont);
            _fontloaded = new FigletFont(filepathFont);
            return this;
        }

        public IBanner FromFont(Stream streamFont)
        {
            ArgumentNullException.ThrowIfNull(streamFont);
            _fontloaded = new FigletFont(streamFont);
            return this;
        }

        public void Show()
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            string[] result = _fontloaded is not null ? _fontloaded.ToAsciiArt(text) : _fontDefault.ToAsciiArt(text);
            int max = 0;
            foreach (string? item in result.Where(x => max < x.Length))
            {
                max = item.Length;
            }
            if (_bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (_bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.SingleBorder, false)[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.DoubleBorder, false)[0];
                        break;
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.SingleBorder, true)[0];
                        break;
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.DoubleBorder, true)[0];
                        break;
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.HeavyBorder, true)[0];
                        break;
                    default:
                        break;
                }
                if (dach.HasValue)
                {
                    console.RawWriteLine(new string(dach.Value, max), style.Overflow(Overflow.Crop));
                }
            }
            foreach (string item in result)
            {
                console.RawWriteLine(item, style.Overflow(Overflow.Crop));
            }
            if (_bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (_bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderDown:
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.SingleBorder, false)[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderDown:
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.DoubleBorder, false)[0];
                        break;
                    case BannerDashOptions.SingleBorderDown:
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.SingleBorder, true)[0];
                        break;
                    case BannerDashOptions.DoubleBorderDown:
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.DoubleBorder, true)[0];
                        break;
                    case BannerDashOptions.HeavyBorderDown:
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = promptConfig.GetSymbol(SymbolType.HeavyBorder, true)[0];
                        break;
                    default:
                        break;
                }
                if (dach.HasValue)
                {
                    console.RawWriteLine(new string(dach.Value, max), style.Overflow(Overflow.Crop));
                }
            }
        }
    }
}
