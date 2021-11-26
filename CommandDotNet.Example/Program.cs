using System.Globalization;

using PPlus.CommandDotNet;
using CommandDotNet.NameCasing;
using System;
using PPlus;

namespace CommandDotNet.Example
{
    public class Program
    {
        static int Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);
            PromptPlus.Clear();

            return new AppRunner<Examples>()
                .UseDefaultMiddleware()
                .UsePrompter()
                .UseNameCasing(Case.KebabCase)
                .UsePromptPlusAnsiConsole()
                .UsePromptPlusArgumentPrompter()
                .UsePromptPlusWizard()
                .Run(args);
        }
    }
}
