// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlus.Internal
{
    internal static class ScreenBufferExtensions
    {
        public static void WriteSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(PPlus.Symbols.Done, PPlus.ColorSchema.DoneSymbol, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteSymbolPrompt(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(PPlus.Symbols.Prompt, PPlus.ColorSchema.PromptSymbol, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSymbolsDone(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PPlus.Symbols.Done, PPlus.ColorSchema.DoneSymbol, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeSelect(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PPlus.Symbols.Selected, PPlus.ColorSchema.Select, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeDisabled(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PPlus.Symbols.NotSelect, PPlus.ColorSchema.Disabled, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLinePipeSkiped(this ScreenBuffer screenBuffer)
        {
            screenBuffer.WriteLine(PPlus.Symbols.Skiped, PPlus.ColorSchema.Error, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteDone(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(PPlus.Symbols.Done, PPlus.ColorSchema.DoneSymbol, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}: ");
        }

        public static void WritePrompt(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(PPlus.Symbols.Prompt, PPlus.ColorSchema.PromptSymbol, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}: ", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineError(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.Error} {message}", PPlus.ColorSchema.Error, PPlus.ColorSchema.BackColorSchema);
        }

        public static void ClearRestOfLine(this ScreenBuffer screenBuffer)
        {
            screenBuffer.Write("\x1b[0K");
        }

        public static void WriteAnswer(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PPlus.ColorSchema.Answer, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSelector(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.Selector} {message}", PPlus.ColorSchema.Select, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotSelector(this ScreenBuffer screenBuffer, string message)
        {
            var len = PPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineTaskRun(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.TaskRun} {message}", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFileBrowserSelected(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.Selector} {message}", PPlus.ColorSchema.Select, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFileBrowser(this ScreenBuffer screenBuffer, string message)
        {
            var len = PPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {message}", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.Selector} {PPlus.Symbols.Selected} {message}", PPlus.ColorSchema.Select, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotMarkSelect(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine($"{PPlus.Symbols.Selector} {PPlus.Symbols.NotSelect} {message}", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = PPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {PPlus.Symbols.Selected} {message}", PPlus.ColorSchema.Select, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineNotSelect(this ScreenBuffer screenBuffer, string message)
        {
            var len = PPlus.Symbols.Selector.ToString().Length;
            screenBuffer.WriteLine(new string(' ', len), PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
            screenBuffer.Write($" {PPlus.Symbols.NotSelect} {message}", PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteSliderOn(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), PPlus.ColorSchema.SliderForecolor, PPlus.ColorSchema.SliderForecolor);
        }

        public static void WriteSliderOff(this ScreenBuffer screenBuffer, int lenght)
        {
            screenBuffer.Write(new string(' ', lenght), PPlus.ColorSchema.SliderBackcolor, PPlus.ColorSchema.SliderBackcolor);
        }

        public static void WriteLinePagination(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PPlus.ColorSchema.Pagination, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineProcessStandardHotKeys(this ScreenBuffer screenBuffer, bool overpipeline, bool enabledabortkey, int extraspace = 0)
        {
            var msg = Messages.EscCancel.Replace(",", "").Trim();
            msg = string.Format(msg, PPlus.AbortKeyPress.ToString());
            screenBuffer.WriteLine();
            if (extraspace != 0)
            {
                screenBuffer.Write(new string(' ', extraspace), PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
            }
            if (enabledabortkey)
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowProcessStandardHotKeysWithPipeline, PPlus.ResumePipesKeyPress, Messages.EscCancel), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(msg, PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
            }
            else
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowProcessStandardHotKeysWithPipeline, PPlus.ResumePipesKeyPress, ""), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
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
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, PPlus.TooltipKeyPress, PPlus.ResumePipesKeyPress, enabledabortAllpipes ? Messages.EscCancelWithPipeline : Messages.EscCancelWithPipeNotAll), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, PPlus.TooltipKeyPress, Messages.EscCancel), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
            }
            else
            {
                if (overpipeline)
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeysWithPipeline, PPlus.TooltipKeyPress, PPlus.ResumePipesKeyPress, ""), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
                else
                {
                    screenBuffer.Write(string.Format(Messages.ShowStandardHotKeys, PPlus.TooltipKeyPress, ""), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
                }
            }
        }

        public static void WriteHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineInputHit(this ScreenBuffer screenBuffer, bool haspasswordvisiblekey, string message)
        {
            if (haspasswordvisiblekey)
            {
                screenBuffer.WriteLine(string.Format(Messages.PasswordStandardHotkeys, message, PPlus.SwitchViewPassword), PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
            }
            else
            {
                screenBuffer.WriteLine(message, PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
            }
        }

        public static void WriteLineHint(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PPlus.ColorSchema.Hint, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.Write(message, PPlus.ColorSchema.Filter, PPlus.ColorSchema.BackColorSchema);
        }

        public static void WriteLineFilter(this ScreenBuffer screenBuffer, string message)
        {
            screenBuffer.WriteLine(message, PPlus.ColorSchema.Filter, PPlus.ColorSchema.BackColorSchema);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text)
        {
            screenBuffer.Write(text, PPlus.ColorSchema.ForeColorSchema, PPlus.ColorSchema.BackColorSchema);
        }

        public static void Write(this ScreenBuffer screenBuffer, string text, ConsoleColor color, ConsoleColor? colorbg = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            screenBuffer.Last().Add(new TextInfo(text, color, colorbg ?? PPlus.ColorSchema.BackColorSchema));
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
            screenBuffer.Last().Add(new TextInfo(text, color, colorbg ?? PPlus.ColorSchema.BackColorSchema));
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
