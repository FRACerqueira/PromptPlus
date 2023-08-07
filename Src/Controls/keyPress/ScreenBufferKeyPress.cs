// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Text;

namespace PPlus.Controls
{
    internal static class ScreenBufferKeyPress
    {
        public static void WriteDoneKeyPress(this ScreenBuffer screenBuffer, BaseOptions options, string input)
        {
            if (!string.IsNullOrEmpty(options.OptPrompt))
            {
                screenBuffer.AddBuffer(options.OptPrompt, options.OptStyleSchema.Prompt());
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(": ", options.OptStyleSchema.Prompt(), true, !string.IsNullOrEmpty(options.OptPrompt));
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Answer(), true, false);
            }
        }

        public static void WritePromptKeyPress(this ScreenBuffer screenBuffer, KeyPressOptions options, string input, bool modeconfirm)
        {
            if (!string.IsNullOrEmpty(options.OptPrompt))
            {
                screenBuffer.AddBuffer(options.OptPrompt, options.OptStyleSchema.Prompt());
                if (modeconfirm)
                {
                    var aux = new StringBuilder();
                    for (int i = 0; i < options.KeyValids.Count; i++)
                    {
                        if (options.TextKey != null)
                        {
                            var txt = options.TextKey(options.KeyValids[i]);
                            if (string.IsNullOrEmpty(txt))
                            {
                                txt = options.KeyValids[i].Key.ToString();
                            }
                            aux.Append(txt);
                        }
                        else
                        {
                            aux.Append(options.KeyValids[i].Key);
                        }
                        if (i < options.KeyValids.Count - 1)
                        {
                            aux.Append('/');
                        }
                    }
                    if (aux.Length > 0)
                    {
                        screenBuffer.AddBuffer($" ({aux})", options.OptStyleSchema.TaggedInfo(), true, false);
                    }
                }
            }
            if (!string.IsNullOrEmpty(input))
            {
                screenBuffer.AddBuffer(": ", options.OptStyleSchema.Prompt(), true, !string.IsNullOrEmpty(options.OptPrompt));
                screenBuffer.AddBuffer(input, options.OptStyleSchema.Answer(), true, false);
            }
        }

        public static void WriteLineTooltipsKeyPress(this ScreenBuffer screenBuffer, KeyPressOptions options)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipKeypress(options);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), smk);
                }
            }
        }

        public static void WriteLineDescriptionKeyPress(this ScreenBuffer screenBuffer, KeyPressOptions options, bool showkeyvalids)
        {
            if (!string.IsNullOrEmpty(options.OptDescription))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(options.OptDescription, options.OptStyleSchema.Description());
            }
            if (showkeyvalids)
            {
                var aux = new StringBuilder();
                for (int i = 0; i < options.KeyValids.Count; i++)
                {
                    var usedefaulttxt = true;
                    if (options.TextKey != null)
                    {
                        var txt = options.TextKey(options.KeyValids[i]);
                        if (!string.IsNullOrEmpty(txt))
                        {
                            aux.Append(txt);
                            usedefaulttxt = false;
                        }
                    }
                    if (usedefaulttxt)
                    {
                        var modifiers = string.Empty;
                        if (options.KeyValids[i].Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            modifiers += "Crtl+";
                        }
                        if (options.KeyValids[i].Modifiers.HasFlag(ConsoleModifiers.Shift))
                        {
                            modifiers += "Shift+";
                        }
                        if (options.KeyValids[i].Modifiers.HasFlag(ConsoleModifiers.Alt))
                        {
                            modifiers += "Alt+";
                        }
                        aux.Append(modifiers);
                        aux.Append(options.KeyValids[i].Key);
                    }
                    if (i < options.KeyValids.Count - 1)
                    {
                        aux.Append(", ");
                    }
                }
                if (aux.Length > 0) 
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer($"{Messages.Validkeys}: ", options.OptStyleSchema.TaggedInfo());
                    screenBuffer.AddBuffer(aux.ToString(), options.OptStyleSchema.TaggedInfo());
                }
            }
        }



        private static string DefaultToolTipKeypress(KeyPressOptions baseOptions)      
        {
            if (baseOptions.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}",
                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress));
            }
            else
            {
                return string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress);
            }
        }
    }
}
