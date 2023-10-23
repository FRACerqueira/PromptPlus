// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls.Objects
{
    internal static class ScreenBufferExtensions
    {
        public static void WriteLinePagination(this ScreenBuffer screenBuffer,BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(message, options.StyleContent(StyleControls.Pagination), true);
            }
        }

        public static void WritePrompt(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (options.OptMinimalRender)
            {
                return;
            }
            var prompt = options.OptPrompt ?? string.Empty;
            if (!string.IsNullOrEmpty(prompt))
            {
                screenBuffer.AddBuffer(prompt, options.StyleContent(StyleControls.Prompt),false,false);
                screenBuffer.AddBuffer(": ", options.StyleContent(StyleControls.Prompt));
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.Answer), true);
            }
        }

        public static void WriteAnswer(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.Answer), true);
            }
        }

        public static void WriteSuggestion(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.Suggestion), true);
            }
        }

        public static void WriteFilterMatch(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.FilterMatch).Overflow(Overflow.Crop), true);
            }
        }

        public static void WriteFilterUnMatch(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.FilterUnMatch).Overflow(Overflow.Crop), true);
            }
        }

        public static void WriteTaggedInfo(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.TaggedInfo), true);
            }
        }
        public static void WriteDone(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(options.OptPrompt))
            {
                screenBuffer.AddBuffer(options.OptPrompt, options.StyleContent(StyleControls.Prompt));
                if (!string.IsNullOrEmpty(input))
                {
                    screenBuffer.AddBuffer(": ", options.StyleContent(StyleControls.Prompt), true);
                }
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.StyleContent(StyleControls.Answer).Overflow(Overflow.Crop), true);
            }
        }

        public static string EscTooltip(BaseOptions options)
        {
            if (options.OptEnabledAbortKey)
            {
                return string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress);
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool WriteLineValidate(this ScreenBuffer screenBuffer, string error, BaseOptions options)
        {
            if (!string.IsNullOrEmpty(error))
            {
               screenBuffer.NewLine();
                screenBuffer.AddBuffer(error, options.StyleContent(StyleControls.Error), true);
                return true;
            }
            return false;
        }

    }
}
