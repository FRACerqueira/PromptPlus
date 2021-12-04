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
#if NETCOREAPP3_1
        [SubCommand]
#else
        [Subcommand]
#endif
        [Description("Secure download")]
        public class Secure
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
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

                console.Out.WriteLine($"authenticated as user:{username} with password:{password} and token {extratoken} (actual password:{pwd})");

                return next();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
            public void Download(
                [Description("Url (http/https) to download")]
                [PromptInitialValue("https://")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
        [Command(Description = "Echos the given text, demonstrating prompting for a single item")]
        public void Echo(
            IConsole console,
#if NETCOREAPP3_1
        [Option(LongName = "texts",Description = "list text to show")]
#else
        [Option(longName:"texts",Description = "list text to show separate by comma", Split = ',')]
#endif
            IEnumerable<string> texts
            )
        {
            console.Out.WriteLine(string.Join(",", texts));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
        [Command(Description = "Echos the list of items string")]
        public void List(
            IConsole console,
            [Description("string to list")]
            ICollection<string> items)
        {
            console.Out.WriteLine(string.Join(Environment.NewLine, items));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
        [Command(Description = "Echos the list of items Dates")]
        public void ListDates(
            IConsole console,
            [Description("date to list")]
            ICollection<DateTime> items)
        {
            console.Out.WriteLine(string.Join(Environment.NewLine, items));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
        [Command(Description = "Echos the list of items boolean")]
        public void ListBoolean(
            IConsole console,
            [Description("boolean to list")]
            ICollection<bool> items)
        {
            console.Out.WriteLine(string.Join(Environment.NewLine, items));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
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
                this.CopyCallerDescription()?? string.Empty)
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


        [Command(Description = "demonstrating prompt with enum")]
        public void ChooseColor(
            IConsole console,
            [Description("My Colors Preference")]
            IEnumerable<ColorPreference> mycolor)
        {
            console.Out.WriteLine(mycolor);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "by design")]
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
