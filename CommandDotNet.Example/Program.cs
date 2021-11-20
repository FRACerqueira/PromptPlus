using System.Globalization;

using CommandDotNet.NameCasing;

using PromptPlusCommandDotNet;

namespace CommandDotNet.Example
{
    public class Program
    {
        static int Main(string[] args)
        {
            return new AppRunner<Examples>()
                .UseDefaultMiddleware()
                .UsePrompter()
                .UsePromptPlusAnsiConsole(cultureInfo: new CultureInfo("en"))
                .UsePromptPlusArgumentPrompter()
                .UseNameCasing(Case.KebabCase)
                .Run(args);
        }

    }
}
