// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using static PPlus.PromptPlus;

namespace PPlus.Internal
{
    internal static class ScreenBufferExtensions
    {
        public static void WriteSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(Symbols.Done, ColorSchema.DoneSymbol);
        }

        public static void WriteSymbolPrompt(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(Symbols.Prompt, ColorSchema.PromptSymbol);
        }

        public static void WriteLineSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(Symbols.Done, ColorSchema.DoneSymbol);
        }

        public static void WriteLinePipeSelect(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(Symbols.Selected, ColorSchema.Select);
        }

        public static void WriteLinePipeDisabled(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(Symbols.NotSelect, ColorSchema.Disabled);
        }

        public static void WriteLinePipeSkiped(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(Symbols.Skiped, ColorSchema.Error);
        }

        public static void WriteDone(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(Symbols.Done, ColorSchema.DoneSymbol);
            screenBuffer.Write($" {message}: ");
        }

        public static void WritePrompt(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(Symbols.Prompt, ColorSchema.PromptSymbol);
            screenBuffer.Write($" {message}: ");
        }

        public static void WriteLineError(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Error} {message}", ColorSchema.Error);
        }

        public static void ClearRestOfLine(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write("\x1b[0K");
        }

        public static void WriteAnswer(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, ColorSchema.Answer);
        }

        public static void WriteLineSelector(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} {message}", ColorSchema.Select);
        }

        public static void WriteLineSelectorDisabled(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} ", ColorSchema.Select);
            screenBuffer.Write(message, ColorSchema.Disabled);
        }

        public static void WriteLineNotSelector(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {message}");
        }

        public static void WriteLineNotSelectorDisabled(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {message}", ColorSchema.Disabled);
        }

        public static void WriteLineTaskRun(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.TaskRun} {message}");
        }

        public static void WriteLineFileBrowserSelected(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} {message}", ColorSchema.Select);
        }

        public static void WriteLineFileBrowser(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {message}");
        }

        public static void WriteLineMarkSelectIndent(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} {Symbols.Selected} ", ColorSchema.Select);
            screenBuffer.Write(message);
        }

        public static void WriteLineMarkNotSelectIndent(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} ", ColorSchema.Select);
            screenBuffer.Write($"{Symbols.NotSelect} ");
            screenBuffer.Write(message);
        }

        public static void WriteLineGroupIndent(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} ", ColorSchema.Select);
            screenBuffer.Write($"{Symbols.SymbGroup} ");
            screenBuffer.Write(message);
        }

        public static void WriteLineGroupUnselectIndent(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($"{Symbols.SymbGroup} ");
            screenBuffer.Write(message);
        }

        public static void WriteLineMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} {Symbols.Selected} {message}", ColorSchema.Select);
        }

        public static void WriteMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, ColorSchema.Select);
        }

        public static void WriteLineNotMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{Symbols.Selector} {Symbols.NotSelect} {message}");
        }

        public static void WriteLineNotMarkSelectIndent(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {Symbols.Selected} ", ColorSchema.Select);
            screenBuffer.Write(message);
        }

        public static void WriteLineNotMarkUnSelectIndent(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {Symbols.NotSelect} ");
            screenBuffer.Write(message);
        }


        public static void WriteNotMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message);
        }

        public static void WriteNotMarkSelectDisabled(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, ColorSchema.Disabled);
        }

        public static void WriteLineSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {Symbols.Selected} {message}", ColorSchema.Select);
        }

        public static void WriteLineNotSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len));
            screenBuffer.Write($" {Symbols.NotSelect} {message}");
        }

        public static void WriteSliderOn(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), ColorSchema.SliderForecolor, ColorSchema.SliderForecolor);
        }

        public static void WriteSliderOff(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), ColorSchema.SliderBackcolor, ColorSchema.SliderBackcolor);
        }

        public static void WriteLinePagination(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, ColorSchema.Pagination);
        }

        public static void WriteLineProcessStandardHotKeys(this ScreenBuffer screenBuffer, bool overpipeline, bool enabledabortkey, bool hasdescription, int extraspace = 0)
        {
            var msg = string.Empty;
            if (enabledabortkey)
            {
                msg = Messages.EscCancel.Replace(",", "").Trim();
                msg = string.Format(msg, AbortKeyPress.ToString());
            }
            if (hasdescription && !overpipeline)
            {
                msg = $"{msg}, {string.Format(Messages.HotKeyDescription, ToggleVisibleDescription)}";
            }
            screenBuffer.WriteLine();
            if (extraspace != 0)
            {
                screenBuffer.Write(new string(' ', extraspace));
            }
            if (overpipeline)
            {
                screenBuffer.Write(string.Format(Messages.ShowProcessStandardHotKeysWithPipeline, msg, ResumePipesKeyPress), ColorSchema.Hint);
            }
            else
            {
                screenBuffer.Write(msg, ColorSchema.Hint);
            }
        }

        public static void WriteLineDescription(this ScreenBuffer screenBuffer, string description)
        {
            screenBuffer.WriteLine(description, ColorSchema.Description);
        }

        public static void WriteLineStandardHotKeys(this ScreenBuffer screenBuffer, bool overpipeline, bool enabledabortkey, bool enabledabortAllpipes, bool hidedescription)
        {
            screenBuffer.WriteLine();
            if (enabledabortkey)
            {
                if (overpipeline)
                {
                    if (!hidedescription)
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipelineDesc, TooltipKeyPress, ToggleVisibleDescription, ResumePipesKeyPress, enabledabortAllpipes ? Messages.EscCancelWithPipeline : Messages.EscCancelWithPipeNotAll), ColorSchema.Hint, PPlusConsole.BackgroundColor);
                    }
                    else
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, TooltipKeyPress, ResumePipesKeyPress, enabledabortAllpipes ? Messages.EscCancelWithPipeline : Messages.EscCancelWithPipeNotAll), ColorSchema.Hint, PPlusConsole.BackgroundColor);
                    }
                }
                else
                {
                    if (IsRunningWithCommandDotNet)
                    {
                        if (!hidedescription)
                        {
                            screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysDesc, TooltipKeyPress, ToggleVisibleDescription, Messages.EscCancelWizard), ColorSchema.Hint);
                        }
                        else
                        {
                            screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, TooltipKeyPress, Messages.EscCancelWizard), ColorSchema.Hint);
                        }
                    }
                    else
                    {
                        if (!hidedescription)
                        {
                            screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysDesc, TooltipKeyPress, ToggleVisibleDescription, Messages.EscCancel), ColorSchema.Hint);
                        }
                        else
                        {
                            screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, TooltipKeyPress, Messages.EscCancel), ColorSchema.Hint);
                        }
                    }
                }
            }
            else
            {
                if (overpipeline)
                {
                    if (!hidedescription)
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipelineDesc, TooltipKeyPress, ToggleVisibleDescription, ResumePipesKeyPress, ""), ColorSchema.Hint);
                    }
                    else
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, TooltipKeyPress, ResumePipesKeyPress, ""), ColorSchema.Hint);
                    }
                }
                else
                {
                    if (!hidedescription)
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysDesc, TooltipKeyPress, ToggleVisibleDescription, ""), ColorSchema.Hint);
                    }
                    else
                    {
                        screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, TooltipKeyPress, ""), ColorSchema.Hint);
                    }
                }
            }
        }

        public static void WriteHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, ColorSchema.Hint);
        }

        public static void WriteLineInputHit(this ScreenBuffer screenBuffer, bool haspasswordvisiblekey, string message)
        {
            if (haspasswordvisiblekey)
            {
                screenBuffer.WriteLine(string.Format(Messages.PasswordStandardHotkeys, message, SwitchViewPassword), ColorSchema.Hint);
            }
            else
            {
                screenBuffer.WriteLine(message, ColorSchema.Hint);
            }
        }

        public static void WriteLineHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, ColorSchema.Hint);
        }

        public static void WriteFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, ColorSchema.Filter);
        }

        public static void WriteLineFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, ColorSchema.Filter);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text)
        {
            screenBuffer.Write(text, PPlusConsole.ForegroundColor, PPlusConsole.BackgroundColor);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text, ConsoleColor color, ConsoleColor? colorbg = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            screenBuffer.Last().Add(new TextInfo(text, color, colorbg ?? PPlusConsole.BackgroundColor));
        }

        public static void WriteLine(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Add(new List<TextInfo>() { new TextInfo("", PPlusConsole.ForegroundColor, PPlusConsole.BackgroundColor) });
        }

        public static void WriteLine(this ScreenBuffer screenBuffer, string text, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            screenBuffer.WriteLine();
            screenBuffer.Last().Add(new TextInfo(text ?? string.Empty, color ?? PPlusConsole.ForegroundColor, colorbg ?? PPlusConsole.BackgroundColor));
        }

        public static void PushCursor(this ScreenBuffer screenBuffer)
        {
            if (!screenBuffer.Any(x => x.Any(x => x.SaveCursor)))
            {
                if (screenBuffer.Last().Any())
                {
                    screenBuffer.Last().Last().SaveCursor = true;
                }
            }
        }

        public static void PushCursor(this ScreenBuffer screenBuffer, ReadLineBuffer buffer)
        {
            screenBuffer.WriteAnswer(buffer.ToBackward());
            screenBuffer.PushCursor();
            screenBuffer.WriteAnswer(buffer.ToForward());
        }

        public static void PushCursor(this ScreenBuffer screenBuffer, MaskedBuffer buffer)
        {
            screenBuffer.WriteAnswer(buffer.ToBackwardString());
            screenBuffer.PushCursor();
            screenBuffer.WriteAnswer(buffer.ToForwardString());
        }
    }
}
