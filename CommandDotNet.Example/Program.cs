using System.Globalization;

using PPlus.CommandDotNet;
using CommandDotNet.NameCasing;

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
                .UsePromptPlusWizard()
                .UseNameCasing(Case.KebabCase)
                .Run(args);
        }

    }
}
