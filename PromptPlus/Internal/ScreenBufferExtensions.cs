// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlusControls.Internal
{
    internal static class ScreenBufferExtensions
    {
        public static void WriteSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(PromptPlus.Symbols.Done, PromptPlus.ColorSchema.DoneSymbol, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteSymbolPrompt(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(PromptPlus.Symbols.Prompt, PromptPlus.ColorSchema.PromptSymbol, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PromptPlus.Symbols.Done, PromptPlus.ColorSchema.DoneSymbol, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeSelect(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PromptPlus.Symbols.Selected, PromptPlus.ColorSchema.Select, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeDisabled(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PromptPlus.Symbols.NotSelect, PromptPlus.ColorSchema.Disabled, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeSkiped(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PromptPlus.Symbols.Skiped, PromptPlus.ColorSchema.Error, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteDone(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(PromptPlus.Symbols.Done, PromptPlus.ColorSchema.DoneSymbol, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}: ");
        }

        public static void WritePrompt(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(PromptPlus.Symbols.Prompt, PromptPlus.ColorSchema.PromptSymbol, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}: ", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineError(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.Error} {message}", PromptPlus.ColorSchema.Error, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void ClearRestOfLine(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write("\x1b[0K");
        }

        public static void WriteAnswer(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PromptPlus.ColorSchema.Answer, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSelector(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.Selector} {message}", PromptPlus.ColorSchema.Select, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotSelector(this ScreenBuffer screenBuffer, string message)
        {
            var len = PromptPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineTaskRun(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.TaskRun} {message}", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFileBrowserSelected(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.Selector} {message}", PromptPlus.ColorSchema.Select, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFileBrowser(this ScreenBuffer screenBuffer, string message)
        {
            var len = PromptPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.Selector} {PromptPlus.Symbols.Selected} {message}", PromptPlus.ColorSchema.Select, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PromptPlus.Symbols.Selector} {PromptPlus.Symbols.NotSelect} {message}", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = PromptPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {PromptPlus.Symbols.Selected} {message}", PromptPlus.ColorSchema.Select, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = PromptPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {PromptPlus.Symbols.NotSelect} {message}", PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteSliderOn(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), PromptPlus.ColorSchema.SliderForecolor, PromptPlus.ColorSchema.SliderForecolor);
        }

        public static void WriteSliderOff(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), PromptPlus.ColorSchema.SliderBackcolor, PromptPlus.ColorSchema.SliderBackcolor);
        }

        public static void WriteLinePagination(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PromptPlus.ColorSchema.Pagination, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineProcessStandardHotKeys(this ScreenBuffer screenBuffer, bool overpipeline, bool enabledabortkey, int extraspace = 0)
        {
            var msg = Messages.EscCancel.Replace(",", "").Trim();
            msg = string.Format(msg, PromptPlus.AbortKeyPress.ToString());
            screenBuffer.WriteLine();
            if (extraspace != 0)
            {
                screenBuffer.Write(new string(' ', extraspace), PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
            }
            if (enabledabortkey)
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowProcessStandardHotKeysWithPipeline, PromptPlus.ResumePipesKeyPress, Messages.EscCancel), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(msg, PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
            }
            else
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowProcessStandardHotKeysWithPipeline, PromptPlus.ResumePipesKeyPress, ""), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
            }
        }

        public static void WriteLineStandardHotKeys(this ScreenBuffer screenBuffer, bool overpipeline, bool enabledabortkey, bool enabledabortAllpipes)
        {
            screenBuffer.WriteLine();
            if (enabledabortkey)
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, PromptPlus.TooltipKeyPress, PromptPlus.ResumePipesKeyPress, enabledabortAllpipes ? Messages.EscCancelWithPipeline : Messages.EscCancelWithPipeNotAll), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, PromptPlus.TooltipKeyPress, Messages.EscCancel), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
            }
            else
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, PromptPlus.TooltipKeyPress, PromptPlus.ResumePipesKeyPress, ""), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, PromptPlus.TooltipKeyPress, ""), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
                }
            }
        }

        public static void WriteHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineInputHit(this ScreenBuffer screenBuffer, bool haspasswordvisiblekey, string message)
        {
            if (haspasswordvisiblekey)
            {
                screenBuffer.WriteLine(string.Format(Messages.PasswordStandardHotkeys, message, PromptPlus.SwitchViewPassword), PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
            }
            else
            {
                screenBuffer.WriteLine(message, PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
            }
        }

        public static void WriteLineHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PromptPlus.ColorSchema.Hint, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PromptPlus.ColorSchema.Filter, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PromptPlus.ColorSchema.Filter, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text)
        {
            screenBuffer.Write(text, PromptPlus.ColorSchema.ForeColorSchema, PromptPlus.ColorSchema.BackColorSchema);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text, ConsoleColor color, ConsoleColor? colorbg = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            screenBuffer.Last().Add(new TextInfo(text, color, colorbg ?? PromptPlus.ColorSchema.BackColorSchema));
        }

        public static void WriteLine(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Add(new List<TextInfo>());
        }

        public static void WriteLine(this ScreenBuffer screenBuffer, string text, ConsoleColor color, ConsoleColor? colorbg = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            screenBuffer.WriteLine();
            screenBuffer.Last().Add(new TextInfo(text, color, colorbg ?? PromptPlus.ColorSchema.BackColorSchema));
        }

        public static void PushCursor(this ScreenBuffer screenBuffer)
        {
            if (!screenBuffer.Any(x => x.Any(x => x.SaveCursor)))
            {
                screenBuffer.Last().Last().SaveCursor = true;
            }
        }

        public static void PushCursor(this ScreenBuffer screenBuffer, InputBuffer buffer)
        {
            screenBuffer.WriteAnswer(buffer.ToBackwardString());
            screenBuffer.PushCursor();
            screenBuffer.WriteAnswer(buffer.ToForwardString());
        }

        public static void PushCursor(this ScreenBuffer screenBuffer, MaskedBuffer buffer)
        {
            screenBuffer.WriteAnswer(buffer.ToBackwardString());
            screenBuffer.PushCursor();
            screenBuffer.WriteAnswer(buffer.ToForwardString());
        }
    }
}
