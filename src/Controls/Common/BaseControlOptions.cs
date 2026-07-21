// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls.Common
{
    internal sealed class BaseControlOptions(PromptConfig promptConfig) : IControlOptions
    {
        private string? _description;
        private string? _prompt;

        private bool _showMessageAbortKey = promptConfig.ShowMessageAbortKey;
        private bool _enabledAbortKey = promptConfig.EnabledAbortKey;
        private bool _hideAfterFinish = promptConfig.HideAfterFinish;
        private bool _hideOnAbort = promptConfig.HideOnAbort;
        private bool _showTooltip = promptConfig.ShowTooltip;
        private string _sufixAfterPrompt = promptConfig.SufixAfterPrompt;
        private string _prefixExtraInfo = promptConfig.PrefixExtraInfo;
        private string _suffixExtraInfo = promptConfig.SuffixExtraInfo;


        public string? PromptValue => _prompt;

        public string? DescriptionValue => _description;

        public bool EnabledAbortKeyValue => _enabledAbortKey;

        public bool ShowMessageAbortKeyValue => _showMessageAbortKey;

        public bool HideAfterFinishValue => _hideAfterFinish;

        public bool HideOnAbortValue => _hideOnAbort;

        public bool ShowTooltipValue => _showTooltip;

        public string SufixAfterPromptValue => _sufixAfterPrompt;

        public string PrefixExtraInfoValue => _prefixExtraInfo;

        public string SuffixExtraInfoValue => _suffixExtraInfo;

        public IControlOptions Prompt(string prompt)
        {
            _prompt = prompt;
            return this;
        }

        public IControlOptions SufixAfterPrompt(string sufix = ": ")
        {
            _sufixAfterPrompt = sufix;
            return this;
        }

        public IControlOptions PrefixExtraInfo(string prefix = "(")
        {
            _prefixExtraInfo = prefix;
            return this;
        }

        public IControlOptions SuffixExtraInfo(string suffix = ")")
        {
            _suffixExtraInfo = suffix;
            return this;
        }

        public IControlOptions Description(string description)
        {
            _description = description;
            return this;
        }

        public IControlOptions ShowMessageAbortKey(bool isshow = true)
        {
            _showMessageAbortKey = isshow;
            return this;
        }

        public IControlOptions EnabledAbortKey(bool isEnabled = true)
        {
            _enabledAbortKey = isEnabled;
            return this;
        }

        public IControlOptions HideAfterFinish(bool shouldHide = true)
        {
            _hideAfterFinish = shouldHide;
            return this;
        }

        public IControlOptions HideOnAbort(bool shouldHide = true)
        {
            _hideOnAbort = shouldHide;
            return this;
        }

        public IControlOptions ShowTooltip(bool isVisible = true)
        {
            _showTooltip = isVisible;
            return this;
        }

        public Dictionary<TS, Style> LoadStyle<TS>(Style currentStyle) where TS : struct, Enum
        {
            TS[] values = Enum.GetValues<TS>();
            Dictionary<TS, Style> result = new(values.Length);
            foreach (TS item in values)
            {
                ComponentStyles styleDefault = Enum.Parse<ComponentStyles>(item.ToString()!);
                Style styleItem = FindStyle(styleDefault, currentStyle);

                if (promptConfig.ContrastRatio == 0)
                {
                    result.TryAdd(item, styleItem);
                }
                else
                {
                    Color adjusted = styleItem.Foreground.AdjustForegroundColorForContrast(styleItem.Background, promptConfig.ContrastRatio);
                    result.TryAdd(item, styleItem.Colors(adjusted));
                }
            }
            return result;
        }

        /// <summary>
        /// Resolves the default style for a component and ensures the resulting foreground has
        /// adequate contrast against its background. The suggested color is preserved when it is
        /// already readable; otherwise it is adjusted to the nearest palette color that reaches
        /// the minimum contrast, keeping the visual identity of the original color.
        /// </summary>
        public static Style FindStyle(ComponentStyles componentStyles, Style currentStyle)
        {
            Style style = ResolveStyle(componentStyles, currentStyle);
            Color adjusted = style.Foreground.AdjustForegroundColorForContrast(style.Background);
            return style.Colors(adjusted, style.Background);
        }

        private static Style ResolveStyle(ComponentStyles componentStyles, Style currentStyle) =>
            componentStyles switch
            {
                ComponentStyles.Prompt            => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.Answer            => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.NegativeValue     => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.PositiveValue     => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.Description       => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.Suggestion        => currentStyle.Colors(ConsoleColor.Yellow),
                ComponentStyles.Selected          => currentStyle.Colors(ConsoleColor.Green),
                ComponentStyles.UnSelected        => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.Disabled          => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.Error             => currentStyle.Colors(ConsoleColor.Red),
                ComponentStyles.Pagination        => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.TaggedInfo        => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.Tooltips          => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.Spinner           => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.ElapsedTime       => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.Ranger            => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.Slider            => new Style(ConsoleColor.White, ConsoleColor.DarkGray),
                ComponentStyles.SwitchOn           => new Style(ConsoleColor.White, ConsoleColor.DarkGray),
                ComponentStyles.SwitchOff          => new Style(ConsoleColor.DarkGray, ConsoleColor.DarkGray),
                ComponentStyles.Lines             => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.CalendarDay       => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.CalendarHighlight => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.CalendarMonth     => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.CalendarWeekDay   => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.CalendarYear      => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.ChartLabel        => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.ChartOrder        => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.ChartPercent      => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.ChartTitle        => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.ChartValue        => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.GroupTip          => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.TableTitle        => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.TableHeader       => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.TableContent      => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.HeaderText        => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.HeaderBorder      => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.SelectedCell      => currentStyle.Colors(ConsoleColor.Green),
                ComponentStyles.UnselectedCell    => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.DisabledRow       => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.BorderLines       => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.FileRoot          => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.FileSize          => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.FileTypeFile      => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.FileTypeFolder    => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.ExpandSymbol      => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.Root              => currentStyle.Colors(ConsoleColor.White),
                ComponentStyles.Node              => currentStyle.Colors(ConsoleColor.Gray),
                ComponentStyles.ChildsCount       => currentStyle.Colors(ConsoleColor.DarkYellow),
                ComponentStyles.WaitingTask       => currentStyle.Colors(ConsoleColor.DarkGray),
                ComponentStyles.RunningTask       => currentStyle.Colors(ConsoleColor.Cyan),
                ComponentStyles.SuccessTask       => currentStyle.Colors(ConsoleColor.Green),
                ComponentStyles.FailedTask        => currentStyle.Colors(ConsoleColor.Red),
                _                                 => currentStyle,
            };
    }
}
