using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommandDotNet.Prompts;
using CommandDotNet.Rendering;


using PPlus;
using PPlus.Drivers;
using PPlus.Objects;
using PPlus.CommandDotNet;
using PPlus.Attributes;

namespace CommandDotNet.Example
{
    [Command(
        Description = "Demonstrates prompting (including passwords) and interceptor method to define options common to subcommands",
        ExtendedHelpText = "Uses an interceptor method ",
        Usage = "prompts ")]
    public class Prompts
    {
        [SubCommand]
        [Description("Secure download")]
        public class Secure
        {
            public Task<int> Intercept(InterceptorExecutionDelegate next,
                [MinLength(5)]
                [Description("password to secure download")]
                Password password,
                IConsole console,
                [Option(Description = "username to secure download")]
                string username = "admin",
                [PromptPlusTypeMasked("TK-C[ABC]9{3}-L{2}",true)]
                string extratoken = "TK-A333-QQ")
            {
                // mimic auth

                if (username == null)
                {
                    console.Out.WriteLine("username not provided");
                    return ExitCodes.Error;
                }

                var pwd = password?.GetPassword();
                if (string.IsNullOrWhiteSpace(pwd))
                {
                    console.Out.WriteLine("password not provided");
                    return ExitCodes.Error;
                }

                console.Out.WriteLine($"authenticated as user:{username} with password:{password}  (actual password:{pwd})");

                return next();
            }

            public void Download(
                [Description("Url (http/https) to download")]
                [PromptValidatorUri(UriKind.Absolute,"http;https")]
                string url,
                [Description("Destination File Path")]
                [PromptPlusTypeBrowser(BrowserFilter.OnlyFolder)]
                string filepath,
                [Description("Date reference to download")]
                DateTime refdate,
                IConsole console)
            {
                console.Out.WriteLine($"Pretending to download {url} to {filepath} by ref: {refdate}");
            }

        }

        [Command(Description = "Echos the given text, demonstrating prompting for a single item")]
        public void Echo(
            IConsole console,
            [Operand(Description = "the text to echo")]
            string text)
        {
            console.Out.WriteLine(text);
        }

        [Command(Description = "sums the list of numbers, demonstrating prompting for a list")]
        public void Sum(
            IConsole console,
            [Description("Numbers to sum")]
            ICollection<int> numbers)
        {
            console.Out.WriteLine(numbers == null
                ? "no numbers were entered"
                : $"{string.Join(" + ", numbers)} = {numbers.Sum()}");
        }

        [Command(Description = "Echos the list of items")]
        public void List(
            IConsole console,
            [Description("Items to list")]
            ICollection<string> items)
        {
            console.Out.WriteLine(string.Join(Environment.NewLine, items));
        }


        [Command(Description = "Confirms required boolean arguments")]
        public void Confirm(
            IConsoleDriver console,
            [Description("Sample confirm")]
            bool @continue)
        {
            console.WriteLine($"{nameof(@continue)} {@continue}");
        }

        [Command(Description = "knock-knock joke, demonstrating use of IAnsiConsole")]
        public void Choose(IConsoleDriver console, CancellationToken cancellationToken, int pageSize = 5)
        {
            var answer = PromptPlus.Select<string>(
                "What is your favorite color ?",
                GetType().CopyCallerDescription()?? string.Empty)
                    .PageSize(pageSize)
                    .AddItems(new string[] { "blue", "purple", "red", "orange", "yellow", "green" })
                    .Run(cancellationToken);
            if (answer.IsAborted)
            {
                console.WriteLine("Choose canceled");
            }
            else
            {
                console.WriteLine(answer.Value);
            }
        }


        [Command(Description = "knock-knock joke, demonstrating use of IPrompter")]
        public void Knock(IConsole console, IPrompter prompter)
        {
            if (prompter.TryPromptForValue("who's there?", out var answer1, out var isCancellationRequested) && !isCancellationRequested)
            {
                var answer2 = prompter.PromptForValue($"{answer1} who?", out _);
                console.Out.WriteLine($"{answer1} {answer2}");
                console.Out.WriteLine("lulz");
            }
        }
    }
}
