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
                screenBuffer.AddBuffer(message, options.OptStyleSchema.Pagination(), true);
            }
        }

        public static void WritePrompt(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            var prompt = options.OptPrompt ?? string.Empty;
            screenBuffer.AddBuffer(prompt, options.OptStyleSchema.Prompt());
            if (!string.IsNullOrEmpty(prompt))
            {
                screenBuffer.AddBuffer(": ", options.OptStyleSchema.Prompt());
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Answer(), true);
            }
        }

        public static void WriteAnswer(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Answer(), true);
            }
        }

        public static void WriteSuggestion(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Suggestion(), true);
            }
        }

        public static void WriteTaggedInfo(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.OptStyleSchema.TaggedInfo(), true);
            }
        }
        public static void WriteDone(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(options.OptPrompt))
            {
                screenBuffer.AddBuffer(options.OptPrompt, options.OptStyleSchema.Prompt());
                screenBuffer.AddBuffer(": ", options.OptStyleSchema.Prompt(), true);
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Answer(), true);
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
               screenBuffer.AddBuffer(error, options.OptStyleSchema.Error(),true);
               return true;
            }
            return false;
        }

    }
}
