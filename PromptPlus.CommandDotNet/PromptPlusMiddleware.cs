using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommandDotNet;
using CommandDotNet.Builders;
using CommandDotNet.Execution;
using CommandDotNet.Prompts;
using CommandDotNet.Rendering;
using CommandDotNet.Tokens;

using PPlus;
using PPlus.Drivers;

namespace PromptPlusCommandDotNet
{
    public static class PromptPlusMiddleware
    {
        private const string Defsource = "PromptPlus.CommandDotNet.UsePromptPlusWizard";
        private static List<string>? s_promptPlusargs;

        /// <summary>
        /// Makes the <see cref="IConsoleDriver"/> available as a command parameter and will
        /// forward <see cref="IConsole"/>.Out to the <see cref="IConsoleDriver"/>.
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="ansiConsole">
        /// optionally, the <see cref="IConsoleDriver"/> to use,
        /// else will use <see cref="AnsiConsole"/>.<see cref="AnsiConsole.Console"/>
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusAnsiConsole(this AppRunner appRunner, IConsoleDriver? ansiConsole = null, CultureInfo? cultureInfo = null)
        {
            return appRunner.Configure(c =>
            {
                PromptPlus.DefaultCulture = cultureInfo ?? Thread.CurrentThread.CurrentUICulture;
                ansiConsole ??= PromptPlus.ConsoleDriver;
                c.Console = ansiConsole as IConsole ?? new AnsiConsoleForwardingConsole(ansiConsole);
                c.UseParameterResolver(_ => ansiConsole);
                c.Services.Add(ansiConsole);
            });
        }

        /// <summary>
        /// Adds support for prompting arguments.<br/>
        /// By default, prompts for arguments missing a required value.<br/>
        /// Missing is determined by <see cref="IArgumentArity"/>, not by any validation frameworks.
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="pageSize">the page size for selection lists.</param>
        /// <param name="getPromptTextCallback">Used to customize the generation of the prompt text.</param>
        /// <param name="argumentFilter">
        /// Filter the arguments that will be prompted. i.e. Create a [PromptWhenMissing] attribute, or only prompt for operands.<br/>
        /// Default filter includes only arguments where <see cref="IArgumentArity"/>.<see cref="IArgumentArity.Minimum"/> is greater than zero.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusArgumentPrompter(this AppRunner appRunner, int pageSize = 10,
            Func<CommandContext, IArgument, string>? getPromptTextCallback = null,
            Predicate<IArgument>? argumentFilter = null)
        {
            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        $"must register {nameof(UsePromptPlusAnsiConsole)} to ensure an {nameof(IConsoleDriver)} is available.");
                }
                c.Services.Add<IArgumentPrompter>(new PromptPlusArgumentPrompter(pageSize, getPromptTextCallback));
                appRunner.UseArgumentPrompter(argumentFilter: argumentFilter);
            });
        }

        /// <summary>
        /// Load custom config(Colors/hotkeys/and so on) for PromptPlus
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="foldername">
        /// Required, the folder name where file PromptPlus.config.json is placed.
        /// This method is only necessary when the file is in a custom folder. Prompt Plus automatically loads the file if the file is placed in the same folder as the binaries.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusConfig(this AppRunner appRunner, string foldername)
        {
            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        $"must register {nameof(UsePromptPlusAnsiConsole)} to ensure an {nameof(IConsoleDriver)} is available.");
                }
                PromptPlus.LoadConfigFromFile(foldername);
            });
        }



        private static AppRunner UsePromptPlusWizard(
            this AppRunner appRunner, string name, string? description = null)
        {

            var Cmd = new Command(name, definitionSource: Defsource)
            {
                Description = description
            };

            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        $"must register {nameof(UsePromptPlusAnsiConsole)} to ensure an {nameof(IConsoleDriver)} is available.");
                }
                c.BuildEvents.OnCommandCreated += (args) =>
                {
                    var aux = args.CommandBuilder.Command.ExtendedHelpText ?? String.Empty;
                    if (!string.IsNullOrEmpty(aux))
                    {
                        aux += "\n\n";
                    }
                    aux += "Directive for wizard prompt:\n" +
                    $"  [{name}] to wizard find commands/options and arguments with prompt! (Remark: Shouldn't have any more arguments)" +
                    $"\n\nExample: %AppName% [{name}].";
                    args.CommandBuilder.Command.ExtendedHelpText = aux;
                };
                c.UseMiddleware((ctx, next) =>
                    {
                        var args = ctx.Original.Args.ToList();
                        args.Insert(0, $"[{name}]");
                        var t = Tokenizer.Tokenize(args, true, Defsource);
                        ctx.Tokens = t;
                        return CommandHooks(ctx, next, PromptPlusWizard);
                    }, MiddlewareStages.PostTokenizePreParseInput);
            });

        }

        private static void PromptPlusWizard(CommandContext context)
        {
            s_promptPlusargs = new();

            var lstTokens = new List<IArgumentNode>();
            //add all commands
            lstTokens.AddRange(context.RootCommand.Subcommands.Cast<IArgumentNode>());

            //add all options for RootCommand
            lstTokens.AddRange(context.RootCommand.Options
                .Where(o => !o.Hidden)
                .Cast<IArgumentNode>());

            var sel = PromptPlus.Select<IArgumentNode>("Choose next")
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .DescriptionSelector(x =>
                {
                    var desc = x.Description ?? " No description";
                    if (x.GetType().Name == nameof(Command))
                    {
                        return $"Command : {desc}";
                    }
                    else
                    {
                        return $"Option : {desc}";
                    }
                })
                .HideAfterFinish(true)
                .Run();
            if (sel.IsAborted)
            {
                return;
            }
            //root option
            if (sel.Value.GetType().Name == nameof(Option))
            {
                var t = Tokenizer.Tokenize(new string[] { $"--{sel.Value.Name}" });
                context.Tokens = t;
                return;
            }
            if (sel.Value.GetType().Name == nameof(Command))
            {
                var cmd = (Command)sel.Value;
                s_promptPlusargs.Add(cmd.Name);
                PromptPlusWizardNext(context, cmd);
            }
        }

        private static void PromptPlusWizardNext(CommandContext context, Command currentCommand)
        {
            var lstTokens = new List<IArgumentNode>();
            lstTokens.Clear();
            //add all subcommands
            lstTokens.AddRange(currentCommand.Subcommands);
            //add all options for RootCommand
            var args = currentCommand.AllArguments(true, true);
            lstTokens.AddRange(args.Cast<IArgumentNode>());
            var sel = PromptPlus.Select<IArgumentNode>("Choose next")
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .DescriptionSelector(x =>
                {
                    var desc = x.Description ?? " No description";
                    if (x.GetType().Name == nameof(Command))
                    {
                        return $"Command : {desc}";
                    }
                    else if (x.GetType().Name == nameof(Option))
                    {
                        return $"Option : {desc}";
                    }
                    else
                    {
                        return $"Operands : {desc}";
                    }
                })
                .HideAfterFinish(true)
                .Run();
            if (sel.IsAborted)
            {
                return;
            }
            if (sel.Value.GetType().Name == nameof(Command))
            {
                var cmd = (Command)sel.Value;
                s_promptPlusargs.Add(cmd.Name);
                PromptPlusWizardNext(context, cmd);
            }
        }

        private static Task<int> CommandHooks(
            CommandContext context, ExecutionDelegate next,
            Action<CommandContext>? beforeCommandRun)
        {
            if (context.Tokens.Directives.Count == 1 && context.Tokens.Count == 1)
            {
                if (context.Tokens.Directives.First().SourceName == Defsource)
                {
                    var args = context.Original.Args.ToList();
                    var t = Tokenizer.Tokenize(args, true, Defsource);
                    context.Tokens = t;
                    beforeCommandRun?.Invoke(context);
                }
            }
            return next(context);
        }
    }
}
