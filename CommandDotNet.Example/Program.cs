using System;
using System.Globalization;

using CommandDotNet.NameCasing;

using PPlus;
using PPlus.CommandDotNet;

namespace CommandDotNet.Example
{
    public class Program
    {
        static int Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);

            return new AppRunner<Examples>()
                .UseDefaultMiddleware()
                .UsePrompter()
                .UseNameCasing(Case.KebabCase)
                .UsePromptPlusAnsiConsole()
                .UsePromptPlusArgumentPrompter()
                .UsePromptPlusWizard()
                .UsePromptPlusRepl(
                    colorizeSessionInitMessage: (msg) => msg.Yellow().Underline(),
                    argumentstreatment: 
                    (args) =>
                    {
                        //masked password when saved history
                        var maskvalue = false;
                        for (var i = 0; i < args.Length; i++)
                        {
                            if (maskvalue)
                            {
                                args[i] = "####";
                                maskvalue = false;
                            }
                            else if (args[i].ToLowerInvariant() == "--password")
                            {
                                maskvalue = true;
                            }
                        }
                        return args;
                    })
                .Run(args);
        }
    }
}
