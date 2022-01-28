// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Drivers
{
    public static class ConsoleDriveExtension
    {
        private const string ParamName = "console";

        public static void WriteAnsiConsole(this IConsoleDriver console, string text)
        {
            if (console == null)
            {
                throw new ArgumentNullException(ParamName, Messages.Invalid);
            }

            console.Write(text);
        }

        public static void WriteAnsiConsole(this IConsoleDriver console, params ColorToken[] tokens)
        {
            if (console == null)
            {
                throw new ArgumentNullException(ParamName, Messages.Invalid);
            }

            console.Write(tokens);
        }

        public static void WriteLineAnsiConsole(this IConsoleDriver console, string text)
        {
            if (console == null)
            {
                throw new ArgumentNullException(ParamName, Messages.Invalid);
            }

            console.WriteLine(text);
        }

        public static void WriteLineAnsiConsole(this IConsoleDriver console, params ColorToken[] tokens)
        {
            if (console == null)
            {
                throw new ArgumentNullException(ParamName, Messages.Invalid);
            }

            console.WriteLine(tokens);
        }

    }
}
