using System;

using PPlus.Objects;

namespace PPlus.Drivers
{
    public static class ConsoleDriveExtension
    {
        public static void WriteAnsiConsole(this IConsoleDriver console, string text)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }

            console.Write(text);
        }
        public static void WriteAnsiConsole(this IConsoleDriver console, params ColorToken[] tokens)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }

            console.Write(tokens);
        }


        public static void WriteLineAnsiConsole(this IConsoleDriver console, string text)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }

            console.WriteLine(text);
        }

        public static void WriteLineAnsiConsole(this IConsoleDriver console, params ColorToken[] tokens)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }

            console.WriteLine(tokens);
        }

    }
}
