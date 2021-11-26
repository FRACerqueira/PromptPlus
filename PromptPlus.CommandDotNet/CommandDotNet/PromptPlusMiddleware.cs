﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private const string DefSourcePPlus = "PromptPlus.CommandDotNet.UsePromptPlusWizard";
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
        public static AppRunner UsePromptPlusAnsiConsole(this AppRunner appRunner, IConsoleDriver? ansiConsole = null)
        {
            return appRunner.Configure(c =>
            {
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
        public static AppRunner UsePromptPlusArgumentPrompter(this AppRunner appRunner, int pageSize = 5,
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
                        string.Format(Exceptions.Ex_RegisterConsole, nameof(UsePromptPlusAnsiConsole), nameof(IConsoleDriver)));
                }
                LoadConfigFromFile(foldername);
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
        /// <param name="runwizard">
        /// The hotkey to execute args from wizard. Default value : F5.
        /// </param>
        /// <param name="forecolorwizard">
        /// The fore-color text for args to wizard. Default value : PPlus.PromptPlus.ColorSchema.Select.
        /// </param>
        /// <param name="backcolorwizard">
        /// The back-color text for args to wizard. Default value : PPlus.PromptPlus.BackgroundColor
        /// </param>
        /// <param name="missingforecolorwizard">
        /// The fore-color text for operand missing to wizard. Default value : PPlus.PromptPlus.ColorSchema.Error.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusWizard(
            this AppRunner appRunner, string name = "wizard", int pageSize = 5, HotKey? runwizard = null, ConsoleColor? forecolorwizard=null, ConsoleColor? backcolorwizard = null, ConsoleColor? missingforecolorwizard = null)
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
            if (!missingforecolorwizard.HasValue)
            {
                missingforecolorwizard = ColorSchema.Error;
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
                            aux += string.Format(Messages.WizardHelpText, name).Replace("%\\n%", "\n");
                            args.CommandBuilder.Command.ExtendedHelpText = aux;
                        }
                    }
                };
                c.UseMiddleware((ctx, next) =>
                {
                    return CommandHooksBuildWizard(name, ctx, next, PromptPlusWizard, pageSize, runwizard, forecolorwizard.Value,backcolorwizard.Value, missingforecolorwizard.Value);
                }, MiddlewareStages.PostTokenizePreParseInput);
            });
        }

        private static void PromptPlusWizard(CommandContext context, int pageSize, HotKey? runwizard, ConsoleColor forecolorwizard, ConsoleColor backcolorwizard, ConsoleColor missingforecolorwizard)
        {
            context.Tokens = Tokenizer.Tokenize(Array.Empty<string>());
            var promptPlusargs = new List<WizardArgs>();

            var lstTokens = new List<IArgumentNode>();
            //add all commands
            lstTokens.AddRange(context.RootCommand.Subcommands);

            //add all options for RootCommand
            lstTokens.AddRange(context.RootCommand.Options);

            var sel = new WizardControl(new WizardOptions()
            {
                Build = runwizard?? new HotKey(ConsoleKey.F5),
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                MissingForeColor = missingforecolorwizard,
                IsRootControl = true,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = Select<IArgumentNode>(Messages.WizardNext)
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
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
                    promptPlusargs.Add(new WizardArgs($"-{opt.ShortName}", sel.Value, false,true));
                }
                else if (opt.LongName is not null)
                {
                    promptPlusargs.Add(new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                }
                PromptPlusWizardNext(ref promptPlusargs, context, context.RootCommand, pageSize, false, runwizard, forecolorwizard, backcolorwizard, missingforecolorwizard);
            }
            else if (sel.Value.GetType().Name == nameof(Command))
            {
                var cmd = (Command)sel.Value;
                promptPlusargs.Add(new WizardArgs(cmd.Name, sel.Value, false,true));
                PromptPlusWizardNext(ref promptPlusargs, context, cmd, pageSize, true, runwizard, forecolorwizard, backcolorwizard, missingforecolorwizard);
            }
            context.Tokens = Tokenizer.Tokenize(promptPlusargs.Select(a => a.ArgValue));
        }

        private static WizardArgs[] PromptPlusWizardNext(ref List<WizardArgs> promptargs,CommandContext context, Command currentCommand,int pageSize, bool isroot, HotKey? runwizard, ConsoleColor forecolorwizard, ConsoleColor backcolorwizard, ConsoleColor missingforecolorwizard)
        {
            var lstTokens = new List<IArgumentNode>();
            lstTokens.Clear();
            //add all sub-commands
            lstTokens.AddRange(currentCommand.Subcommands);
            if (currentCommand.Subcommands.Count == 0)
            {
                //add all Arguments for Command
                var args = currentCommand.AllArguments(true, false);
                var lstTokensOpe = args.Where(a => a.GetType() == typeof(Operand));
                lstTokens.AddRange(lstTokensOpe);
                lstTokens.AddRange(args
                    .Where(a => a.GetType() == typeof(Option))
                    .Where(a => ((Option)a).Parent.Name == currentCommand.Name));
            }
            else
            {
                //add all options to current command
                lstTokens.AddRange(currentCommand.AllOptions(true, false));
            }
            var sel = new WizardControl(new WizardOptions()
            {
                IsRootControl = isroot,
                Build = runwizard ?? new HotKey(ConsoleKey.F5),
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                MissingForeColor = missingforecolorwizard,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = Select<IArgumentNode>("Next")
                .AddItems(lstTokens)
                .TextSelector(x => x.Name)
                .PageSize(pageSize)
                .Config(ctx =>
                {
                    ctx.HideAfterFinish(true);
                })
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
                .Config(ctx =>
                {
                    ctx.HideAfterFinish(true);
                })
                .UpdateTokenArgs(promptargs.Select(a => new WizardArgs(a.ArgValue,a.IsSecret, a.IsEnabled)))
                .Run(context.CancellationToken);

            if (sel.IsAborted)
            {
                promptargs.Clear();
            }

            if (!sel.IsAborted && sel.Value is not null)
            {
                if (sel.Value.GetType().Name == nameof(Command))
                {
                    promptargs = RemovePreviusHelp(promptargs);
                    promptargs.Add(new WizardArgs(sel.Value.Name, sel.Value, false, true));
                    PromptPlusWizardNext(ref promptargs,context, (Command)sel.Value,pageSize,true, runwizard, forecolorwizard, backcolorwizard,missingforecolorwizard);
                }
                if (sel.Value.GetType().Name == nameof(Option))
                {
                    var opt = (Option)sel.Value;
                    promptargs = RemovePreviusHelp(promptargs);
                    if (opt.DefinitionSource == DefSourceHelp)
                    {
                        promptargs = RemovePreviusOptionsAndOperands(promptargs);
                        if (opt.ShortName is not null)
                        {
                            promptargs.Add(new WizardArgs($"-{opt.ShortName}", sel.Value, false, true));
                        }
                        else if (opt.LongName is not null)
                        {
                            promptargs.Add(new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                        }
                        PromptPlusWizardNext(ref promptargs, context, currentCommand, pageSize, false, runwizard, forecolorwizard, backcolorwizard,missingforecolorwizard);
                    }
                    else
                    {
                        var ispassword = opt.TypeInfo.UnderlyingType == typeof(Password);
                        var indexparent = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Command) && a.ArgNode.Name == opt.Parent.Name && a.ArgNode.DefinitionSource == opt.Parent.DefinitionSource);
                        var kindprompt = opt.EnsureValidPromptPlusType();
                        var iscancelrequest = false;
                        ICollection<string> resultope;

                        string[] lastvalue = null;
                        var findvalues = promptargs
                                .Where(a => a.ArgNode.GetType() == typeof(Option) && a.ArgNode.Name == opt.Name && a.ArgNode.DefinitionSource == opt.DefinitionSource)
                                .Skip(1);
                        if (findvalues.Count() > 0)
                        {
                            lastvalue = findvalues.Select(f => f.ArgValue).ToArray();
                        }

                        if (kindprompt == PromptPlusTypeKind.None)
                        {
                            resultope = UtilExtension.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, lastvalue, out iscancelrequest);
                        }
                        else
                        {
                            resultope = UtilExtension.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, kindprompt, lastvalue, out iscancelrequest);
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
                                promptargs.Insert(indexparent, new WizardArgs($"-{opt.ShortName}", sel.Value, false, true));
                            }
                            else if (opt.LongName is not null)
                            {
                                promptargs.Insert(indexparent, new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                            }
                            foreach (var item in resultope)
                            {
                                indexparent++;
                                promptargs.Insert(indexparent, new WizardArgs(item, sel.Value, ispassword, true));
                            }
                        }
                        PromptPlusWizardNext(ref promptargs, context, currentCommand, pageSize, false, runwizard, forecolorwizard, backcolorwizard,missingforecolorwizard);
                    }
                }
                if (sel.Value.GetType().Name == nameof(Operand))
                {
                    var ope = (Operand)sel.Value;
                    string[] lastvalue = null;
                    if (!ExistArgOperand(promptargs))
                    {
                        foreach (var item in lstTokens)
                        {
                            if (item.GetType() == typeof(Operand))
                            {

                                promptargs.Add(
                                    new WizardArgs("?",
                                        item,
                                        ((Operand)item).TypeInfo.UnderlyingType == typeof(Password),
                                        false));
                            }
                        }
                    }
                    else
                    {
                        var findvalues = promptargs.Where(a => a.IsEnabled && a.ArgNode.GetType() == typeof(Operand) && a.ArgNode.Name == ope.Name && a.ArgNode.DefinitionSource == ope.DefinitionSource);
                        if (findvalues.Count() > 0)
                        {
                            lastvalue = findvalues.Select(f => f.ArgValue).ToArray();
                        }
                    }
                    var ispassword = ope.TypeInfo.UnderlyingType == typeof(Password);
                    promptargs = RemovePreviusOptionsForOperand(promptargs);
                    var kindprompt = ope.EnsureValidPromptPlusType();
                    var iscancelrequest = false;
                    ICollection<string> resultope;
                    if (kindprompt == PromptPlusTypeKind.None)
                    {
                        resultope = UtilExtension.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize,lastvalue, out iscancelrequest);
                    }
                    else
                    {
                        resultope = UtilExtension.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, kindprompt,lastvalue, out iscancelrequest);
                    }
                    if (!iscancelrequest)
                    {
                        if (resultope.Count() > 1)
                        {
                            var indexope = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Operand) && a.ArgNode.Name == ope.Name && a.ArgNode.DefinitionSource == ope.DefinitionSource);
                            promptargs = promptargs.Take(indexope).ToList();
                            foreach (var item in resultope)
                            {
                                promptargs.Add(new WizardArgs(item, ope, ispassword, true));
                            }
                        }
                        else
                        {
                            var indexope = promptargs.FindIndex(a => a.ArgNode.GetType() == typeof(Operand) && a.ArgNode.Name == ope.Name && a.ArgNode.DefinitionSource == ope.DefinitionSource);
                            if (indexope != promptargs.Count - 1)
                            {
                                promptargs[indexope] = new WizardArgs(resultope.First(), ope, ispassword, true);
                            }
                            else
                            {
                                promptargs.RemoveAt(indexope);
                                foreach (var item in resultope)
                                {
                                    promptargs.Add(new WizardArgs(item, ope, ispassword, true));
                                }
                            }
                        }
                    }
                    else
                    {
                        promptargs = RemoveOperandIfAllEmpty(promptargs);
                    }
                    PromptPlusWizardNext(ref promptargs, context, currentCommand,pageSize,false,runwizard,forecolorwizard,backcolorwizard,missingforecolorwizard);
                }
            }
            return promptargs.ToArray();
        }

        private static bool ExistArgOperand(List<WizardArgs> args)
        {
            if (args[args.Count - 1].ArgNode.GetType() == typeof(Operand))
            {
                return true;
            }
            return false;
        }

        private static List<WizardArgs> RemovePreviusHelp(List<WizardArgs> args)
        {
            var lstdel = new List<WizardArgs>();
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].ArgNode.GetType() != typeof(Command) && args[i].ArgNode.DefinitionSource == DefSourceHelp)
                {
                    lstdel.Add(args[i]);
                    break;
                }
            }
            foreach (var item in lstdel)
            {
                args.Remove(item);
            }
            return args;
        }

        private static List<WizardArgs> RemoveOperandIfAllEmpty(List<WizardArgs> args)
        {
            var qtdenabled = 0;
            var qtdope = 0;
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].IsEnabled)
                {
                    if (args[i].ArgNode.GetType() != typeof(Command) && args[i].ArgNode.GetType() != typeof(Operand))
                    {
                        qtdenabled++;
                    }
                    else
                    {
                        break;
                    }
                }
                qtdope++;
            }
            if (qtdenabled == 0)
            {
                args = RemovePreviusOptionsAndOperands(args);
            }
            return args;
        }

        private static List<WizardArgs> RemovePreviusOptionsForOperand(List<WizardArgs> args)
        {
            var lstdel = new List<WizardArgs>();
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].IsEnabled)
                {
                    if (args[i].ArgNode.GetType() != typeof(Command) && args[i].ArgNode.GetType() != typeof(Operand))
                    {
                        lstdel.Add(args[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            foreach (var item in lstdel)
            {
                args.Remove(item);
            }
            return args;
        }

        private static List<WizardArgs> RemovePreviusOptionsAndOperands(List<WizardArgs> args)
        {
            var lstdel = new List<WizardArgs>();
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].ArgNode.GetType() != typeof(Command))
                {
                    lstdel.Add(args[i]);
                }
                else
                {
                    break;
                }
            }
            foreach (var item in lstdel)
            {
                args.Remove(item);
            }
            return args;
        }

        private static Task<int> CommandHooksBuildWizard(string  name,
            CommandContext context, ExecutionDelegate next,
            Action<CommandContext, int,HotKey?,ConsoleColor,ConsoleColor, ConsoleColor>? beforeCommandRun,
            int pageSize, HotKey?
            runwizard,
            ConsoleColor forecolorwizard,
            ConsoleColor backcolorwizard,
            ConsoleColor missingforecolorwizard)
        {
            if (context.Tokens.Directives.Count == 1)
            {
                if (context.Tokens.Directives.First().Value == name)
                {
                    beforeCommandRun?.Invoke(context,pageSize, runwizard,forecolorwizard,backcolorwizard, missingforecolorwizard);
                }
            }
            return next(context);
        }
    }
}