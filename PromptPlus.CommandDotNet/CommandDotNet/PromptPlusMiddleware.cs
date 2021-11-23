using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using PPlus.Attributes;
using PPlus.CommandDotNet.Controls;
using PPlus.CommandDotNet.Resources;
using PPlus.Drivers;
using PPlus.Objects;

using static PPlus.PromptPlus;

namespace PPlus.CommandDotNet
{
    public static class PromptPlusMiddleware
    {
        private const string Defsource = "PromptPlus.CommandDotNet.UsePromptPlusWizard";
        private const string DefSourceHelp = "CommandDotNet.Help.HelpMiddleware";

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
        /// <param name="disableEscAbort">Disable esc key to abort. Default value : false.</param>
        /// <param name="getPromptTextCallback">Used to customize the generation of the prompt text.</param>
        /// <param name="argumentFilter">
        /// Filter the arguments that will be prompted. i.e. Create a [PromptWhenMissing] attribute, or only prompt for operands.<br/>
        /// Default filter includes only arguments where <see cref="IArgumentArity"/>.<see cref="IArgumentArity.Minimum"/> is greater than zero.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusArgumentPrompter(this AppRunner appRunner, int pageSize = 5, bool disableEscAbort = false,
            Func<CommandContext, IArgument, string>? getPromptTextCallback = null,
            Predicate<IArgument>? argumentFilter = null)
        {
            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        string.Format(Exceptions.Ex_RegisterConsole, nameof(UsePromptPlusAnsiConsole), nameof(IConsoleDriver)));
                }
                c.Services.Add<IArgumentPrompter>(new PromptPlusArgumentPrompter(pageSize,disableEscAbort, getPromptTextCallback));
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
                        string.Format(Exceptions.Ex_RegisterConsole, nameof(UsePromptPlusAnsiConsole), nameof(IConsoleDriver)));
                }
                PromptPlus.LoadConfigFromFile(foldername);
            });
        }



        /// <summary>
        /// Find commands/options and arguments with prompt
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="name">
        /// Required, name of Directive.
        /// </param>
        /// <param name="pageSize">
        /// The page size for selection lists.
        /// </param>
        /// <param name="disableEscAbort">
        /// Disable esc key to abort. Default value : false.
        /// </param>
        /// <param name="runwizard">
        /// The hotkey to execute args from wizard. Default value : F5.
        /// </param>
        /// <param name="forecolorwizard">
        /// The fore-color text for args to wizard. Default value : PPlus.PromptPlus.ColorSchema.Select.
        /// </param>
        /// <param name="backcolorwizard">
        /// The back-color text for args to wizard. Default value : PPlus.PromptPlus.BackgroundColor
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusWizard(
            this AppRunner appRunner, string name = "wizard", int pageSize = 5,bool disableEscAbort  = false, HotKey? runwizard = null, ConsoleColor? forecolorwizard=null, ConsoleColor? backcolorwizard = null)
        {
            if (pageSize < 1)
            {
                pageSize = 1;
            }
            if (!forecolorwizard.HasValue)
            {
                forecolorwizard = ColorSchema.Select;
            }
            if (!backcolorwizard.HasValue)
            {
                backcolorwizard = BackgroundColor;
            }
            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        string.Format(Exceptions.Ex_RegisterConsole, nameof(UsePromptPlusAnsiConsole), nameof(IConsoleDriver)));
                }
                c.BuildEvents.OnCommandCreated += (args) =>
                {
                    if (args.CommandContext.Tokens.Directives.Count == 1)
                    {
                        if (args.CommandContext.Tokens.Directives.First().Value == name)
                        {
                            var aux = args.CommandBuilder.Command.ExtendedHelpText ?? string.Empty;
                            if (!string.IsNullOrEmpty(aux))
                            {
                                aux += "\n\n";
                            }
                            aux += string.Format(Messages.WizardHelpText, name);
                            args.CommandBuilder.Command.ExtendedHelpText = aux;
                        }
                    }
                };
                c.UseMiddleware((ctx, next) =>
                {
                    return CommandHooksBuildWizard(name, ctx, next, PromptPlusWizard, pageSize, disableEscAbort, runwizard, forecolorwizard,backcolorwizard);
                }, MiddlewareStages.PostTokenizePreParseInput);
            });
        }

        private static void PromptPlusWizard(CommandContext context, int pageSize,bool disableEscAbort, HotKey? runwizard, ConsoleColor? forecolorwizard, ConsoleColor? backcolorwizard)
        {
            context.Tokens = Tokenizer.Tokenize(Array.Empty<string>());
            var promptPlusargs = new List<WizardArgs>();

            var lstTokens = new List<IArgumentNode>();
            //add all commands
            lstTokens.AddRange(context.RootCommand.Subcommands
                .Cast<IArgumentNode>());

            //add all options for RootCommand
            lstTokens.AddRange(context.RootCommand.Options.Cast<IArgumentNode>());

            var sel = new WizardControl(new WizardOptions()
            {
                Build = runwizard?? new HotKey(ConsoleKey.F5),
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                IsRootControl = true,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = PromptPlus.Select<IArgumentNode>(Messages.WizardNext)
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .DescriptionSelector(x =>
                {
                    var desc = x.Description;
                    if (string.IsNullOrEmpty(desc))
                    {
                        DescriptionAttribute att;
                        if (x as Command != null)
                        {
                            att = x.CustomAttributes.GetCustomAttributes(false)
                                .Where(a => a.GetType() == typeof(DescriptionAttribute))
                                .Cast<DescriptionAttribute>()
                                .FirstOrDefault();
                        }
                        else
                        {
                            att = ((IArgument)x).FindArgumentAttribute<DescriptionAttribute>();
                        }
                        if (att is not null)
                        {
                            desc = att.Description;
                        }
                        else
                        {
                            desc = Messages.WizardNoDescription;
                        }
                    }
                    if (x.GetType().Name == nameof(Command))
                    {
                        return string.Format(Messages.WizardCommandFor,context.RootCommand.Name,desc);
                    }
                    else
                    {
                        return string.Format(Messages.WizardOptionFor, context.RootCommand.Name, desc);
                    }
                })
                .ToPipe(null, context.RootCommand.Name),
            }).Config(ctx => ctx.HideAfterFinish(true))
              .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
              .Run(context.CancellationToken);

            if (sel.Value == null)
            {
                return;
            }
            //root option
            if (sel.Value.GetType().Name == nameof(Option))
            {
                var opt = (Option)sel.Value;
                if (opt.ShortName is not null)
                {
                    promptPlusargs.Add(new WizardArgs($"-{opt.ShortName}",sel.Value,false));
                }
                else if (opt.LongName is not null)
                {
                    promptPlusargs.Add(new WizardArgs($"--{opt.LongName}",sel.Value,false));
                }
                PromptPlusWizardNext(promptPlusargs, context, context.RootCommand, pageSize,disableEscAbort, false,runwizard,forecolorwizard,backcolorwizard);
                return;
            }
            if (sel.Value.GetType().Name == nameof(Command))
            {
                var cmd = (Command)sel.Value;
                promptPlusargs.Add( new WizardArgs(cmd.Name,sel.Value,false));
                PromptPlusWizardNext(promptPlusargs, context, cmd,pageSize,disableEscAbort,true, runwizard, forecolorwizard, backcolorwizard);
            }
            context.Tokens = Tokenizer.Tokenize(promptPlusargs.Select(a => a.ArgValue));
        }

        private static WizardArgs[] PromptPlusWizardNext(List<WizardArgs> promptargs,CommandContext context, Command currentCommand,int pageSize,bool disableEscAbort, bool isroot, HotKey? runwizard, ConsoleColor? forecolorwizard, ConsoleColor? backcolorwizard)
        {
            var lstTokens = new List<IArgumentNode>();
            lstTokens.Clear();
            //add all sub-commands
            lstTokens.AddRange(currentCommand.Subcommands);
            if (currentCommand.Subcommands.Count == 0)
            {
                //add all Arguments for Command
                var args = currentCommand.AllArguments(true, false);
                lstTokens.AddRange(args.Cast<IArgumentNode>());
            }
            else
            {
                //add all HideOptios
                var args = currentCommand.AllOptions(false, false)
                    .Where(o => o.IsInterceptorOption == false);
                lstTokens.AddRange(args.Cast<IArgumentNode>());
            }
            var sel = new WizardControl(new WizardOptions()
            {
                IsRootControl = isroot,
                Build = runwizard ?? new HotKey(ConsoleKey.F5),
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = PromptPlus.Select<IArgumentNode>("Next")
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .DescriptionSelector(x =>
                {
                    var desc = x.Description;
                    if (string.IsNullOrEmpty(desc))
                    {
                        DescriptionAttribute att;
                        if (x as Command != null)
                        {
                            att = x.CustomAttributes.GetCustomAttributes(false)
                                .Where(a => a.GetType() == typeof(DescriptionAttribute))
                                .Cast<DescriptionAttribute>()
                                .FirstOrDefault();
                        }
                        else
                        {
                            att = ((IArgument)x).FindArgumentAttribute<DescriptionAttribute>();
                        }
                        if (att is not null)
                        {
                            desc = att.Description;
                        }
                        else
                        {
                            desc = Messages.WizardNoDescription;
                        }
                    }
                    if (x.GetType().Name == nameof(Command))
                    {
                        return string.Format(Messages.WizardCommandFor,currentCommand.Name,desc);
                    }
                    else if (x.GetType().Name == nameof(Option))
                    {
                        return string.Format(Messages.WizardOptionFor, ((Option)x).Parent.Name, desc);
                    }
                    else
                    {
                        return string.Format(Messages.WizardOperandFor, currentCommand.Name, desc);
                    }
                })
                .ToPipe(null, context.RootCommand.Name),
            })
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .UpdateTokenArgs(promptargs.Select(a => new WizardArgs(a.ArgValue,null, a.IsSecret)))
                .Run(context.CancellationToken);

            if (sel.IsAborted)
            {
                promptargs.Clear();
            }

            if (!sel.IsAborted && sel.Value is not null)
            {
                if (sel.Value.GetType().Name == nameof(Command))
                {
                    promptargs.Add(new WizardArgs(sel.Value.Name, sel.Value, false));
                    PromptPlusWizardNext(promptargs,context, (Command)sel.Value,pageSize, disableEscAbort,true, runwizard, forecolorwizard, backcolorwizard);
                }
                if (sel.Value.GetType().Name == nameof(Option))
                {
                    var opt = (Option)sel.Value;

                    if (sel.Value.DefinitionSource == DefSourceHelp)
                    {
                        if (opt.ShortName is not null)
                        {
                            var index = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Option) && a.ArgNode.Name == opt.Name && a.ArgNode.DefinitionSource == opt.DefinitionSource);
                            if (index < 0)
                            {
                                promptargs.Add(new WizardArgs($"-{opt.ShortName}", sel.Value, false));
                            }
                            else
                            {
                                promptargs[index] = new WizardArgs($"-{opt.ShortName}", sel.Value, false);
                            }
                        }
                        else if (opt.LongName is not null)
                        {
                            var index = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Option) && a.ArgNode.Name == opt.Name && a.ArgNode.DefinitionSource == opt.DefinitionSource);
                            if (index < 0)
                            {
                                promptargs.Add(new WizardArgs($"--{opt.LongName}", sel.Value, false));
                            }
                            else
                            {
                                promptargs[index] = new WizardArgs($"--{opt.LongName}", sel.Value, false);
                            }
                        }
                    }
                    else
                    {
                        if (opt.Parent.Name != currentCommand.Name)
                        {
                            var ispassword = opt.TypeInfo.UnderlyingType == typeof(Password);
                            var indexparent = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Command)  && a.ArgNode.Name == opt.Parent.Name && a.ArgNode.DefinitionSource == opt.Parent.DefinitionSource);
                            var kindprompt = opt.EnsureValidPromptPlusType();
                            var iscancelrequest = false;
                            ICollection<string> resultope;
                            if (kindprompt == PromptPlusTypeKind.None)
                            {
                                resultope = UtilExtension.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize,disableEscAbort, out iscancelrequest);
                            }
                            else
                            {
                                resultope = UtilExtension.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize,kindprompt,disableEscAbort, out iscancelrequest);
                            }
                            if (!iscancelrequest)
                            {
                                //remove duplicates
                                var indexopt = -1;
                                do
                                {
                                    indexopt = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Option) && a.ArgNode.Name == opt.Name && a.ArgNode.DefinitionSource == opt.DefinitionSource);
                                    if (indexopt >= 0)
                                    {
                                        promptargs.RemoveAt(indexopt);
                                    }
                                } while (indexopt >= 0);

                                indexparent++;
                                if (opt.ShortName is not null)
                                {
                                    promptargs.Insert(indexparent, new WizardArgs($"-{opt.ShortName}",sel.Value, false));
                                }
                                else if (opt.LongName is not null)
                                {
                                    promptargs.Insert(indexparent, new WizardArgs($"--{opt.LongName}",sel.Value, false));
                                }
                                foreach (var item in resultope)
                                {
                                    indexparent++;
                                    promptargs.Insert(indexparent,new WizardArgs(item,sel.Value, ispassword));
                                }
                            }
                            PromptPlusWizardNext(promptargs, context, currentCommand, pageSize, disableEscAbort, false, runwizard, forecolorwizard, backcolorwizard);
                        }
                    }
                }
                if (sel.Value.GetType().Name == nameof(Operand))
                {
                    var ope = (Operand)sel.Value;
                    var kindprompt = ope.EnsureValidPromptPlusType();
                    var iscancelrequest = false;
                    ICollection<string> resultope;
                    var ispassword = ope.TypeInfo.UnderlyingType == typeof(Password);
                    if (kindprompt == PromptPlusTypeKind.None)
                    {
                        resultope = UtilExtension.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, disableEscAbort, out iscancelrequest);
                    }
                    else
                    {
                        resultope = UtilExtension.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, kindprompt, disableEscAbort, out iscancelrequest);
                    }
                    if (!iscancelrequest)
                    {
                        //remove duplicates
                        var indexope = -1;
                        do
                        {
                            indexope = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Operand) && a.ArgNode.Name == ope.Name && a.ArgNode.DefinitionSource == ope.DefinitionSource);
                            if (indexope >= 0)
                            {
                                promptargs.RemoveAt(indexope);
                            }
                        } while (indexope >= 0);
                        foreach (var item in resultope)
                        {
                            promptargs.Add(new WizardArgs(item,sel.Value, ispassword));
                        }
                    }
                    PromptPlusWizardNext(promptargs, context, currentCommand,pageSize, disableEscAbort,false,runwizard,forecolorwizard,backcolorwizard);
                }
            }
            return promptargs.ToArray();
        }

        private static Task<int> CommandHooksBuildWizard(string  name,
            CommandContext context, ExecutionDelegate next,
            Action<CommandContext, int,bool,HotKey?,ConsoleColor?,ConsoleColor?>? beforeCommandRun, int pageSize,bool disableEscAbort, HotKey? runwizard,ConsoleColor? forecolorwizard, ConsoleColor? backcolorwizard)
        {
            if (context.Tokens.Directives.Count == 1)
            {
                if (context.Tokens.Directives.First().Value == name)
                {
                    beforeCommandRun?.Invoke(context,pageSize,disableEscAbort, runwizard,forecolorwizard,backcolorwizard);
                }
            }
            return next(context);
        }
    }
}
