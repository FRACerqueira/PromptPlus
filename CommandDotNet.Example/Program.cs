using System.Globalization;

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
                .Run(args);
        }

    }
}
