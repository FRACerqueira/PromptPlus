// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

using TypeInfo = CommandDotNet.TypeInfo;

namespace PPlus.CommandDotNet
{
    public static class PromptPlusMiddleware
    {
        public static string CopyCallerDescription(this object source, [CallerMemberName] string? caller = null)
        {
            var m = source.GetType().GetMethod(caller);
            var att = m.GetCustomAttributes().FirstOrDefault(a => a as INameAndDescription != null);
            if (att != null)
            {
                return ((INameAndDescription)att).Description;
            }
            att = m.GetCustomAttributes().FirstOrDefault(a => a as DescriptionAttribute != null);
            if (att != null)
            {
                return ((DescriptionAttribute)att).Description;
            }
            return null;
        }

        /// <summary>
        /// Makes the <see cref="IConsoleDriver"/> available as a command parameter and will
        /// forward <see cref="IConsole"/>.Out to the <see cref="IConsoleDriver"/>.
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="ansiConsole">
        /// optionally, the <see cref="IConsoleDriver"/> to use,
        /// else will use else will use PromptPlus console driver/>
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusAnsiConsole(this AppRunner appRunner, IConsoleDriver? ansiConsole = null)
        {
            return appRunner.Configure(c =>
            {
                ansiConsole ??= PPlusConsole;
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
        /// <param name="backcommand">
        /// The hotkey to returns the wizard to the previous command. Default value : ConsoleKey.Backspace.
        /// </param>
        /// <param name="forecolorwizard">
        /// The forecolor text for args to wizard. Default value : PPlus.PromptPlus.ColorSchema.Select.
        /// </param>
        /// <param name="backcolorwizard">
        /// The backcolor text for args to wizard. Default value : PPlus.PromptPlus.BackgroundColor
        /// </param>
        /// <param name="missingforecolorwizard">
        /// The forecolor text for operand missing to wizard. Default value : PPlus.PromptPlus.ColorSchema.Error.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusWizard(
            this AppRunner appRunner, string name = "wizard", byte pageSize = 5, HotKey? runwizard = null, HotKey? backcommand = null, ConsoleColor? forecolorwizard = null, ConsoleColor? backcolorwizard = null, ConsoleColor? missingforecolorwizard = null)
        {
            if (pageSize < 1)
            {
                pageSize = 1;
            }
            if (runwizard is null)
            {
                runwizard = new HotKey(UserHotKey.F5);
            }
            if (backcommand is null)
            {
                backcommand = new HotKey(ConsoleKey.Backspace);
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

                c.UseMiddleware((ctx, next) =>
                {
                    return CommandHooksBuildWizard(name, ctx, next, PromptPlusWizard, pageSize, runwizard.Value, backcommand.Value, forecolorwizard.Value, backcolorwizard.Value, missingforecolorwizard.Value);
                }, MiddlewareStages.PostTokenizePreParseInput);

                c.BuildEvents.OnCommandCreated += (args) =>
                {
                    var aux = args.CommandBuilder.Command.ExtendedHelpText ?? string.Empty;
                    if (!string.IsNullOrEmpty(aux))
                    {
                        aux += "\n\n";
                    }
                    aux += string.Format(Messages.WizardHelpText, name)
                    .Replace("%\\n%", "\n")
                    .Replace("%\\t%", "\t");
                    args.CommandBuilder.Command.ExtendedHelpText = aux;
                };
            });
        }

        /// <summary>
        /// Start interative session with readline prompt, Sugestions and History
        /// </summary>
        /// <param name="appRunner">the <see cref="AppRunner"/> instance</param>
        /// <param name="enabledsuggestion">
        /// Flag to run suggestionHandler when keep track of interactive sessions.  Default value 'true'
        /// </param>
        /// /// <param name="suggestionHandler">
        /// Function to load and send sugestion .if null and enabledsuggestion will be using the default suggestion routine.
        /// </param>
        /// <param name="sugestionEnterTryFininsh">
        /// When select sugestion Try Fininsh input.Default 'false'.
        /// </param>
        /// <param name="usesugestionargumenttype">
        /// If true use type of argument to sugestion, If false use name of argument to sugestion. default 'true'
        /// </param>
        /// <param name="useoptionsintercept">
        /// If true use intercept argument to sugestion. Default 'true'
        /// </param>
        /// <param name="shortoptionname">
        /// Short(Char) option alias to enter interative session. Default value 'i'. Short or long name must be filled in.
        /// </param>
        /// <param name="longoptionname">
        /// Long(string) option alias to enter interative session. Default value 'interactive'.  Short or long name must be filled in.
        /// </param>
        /// <param name="description">
        /// Description for interative session option.Default value 'enter an interactive session'
        /// </param>
        /// <param name="enabledhistory">
        /// Flag to keep track of interactive sessions. Default value 'true'
        /// </param>
        /// <param name="pageSize">
        /// The page size for history lists. If page less than 1 will be set to 1
        /// </param>
        /// <param name="timeouthistory">
        /// Timeout for history items. Default value '30' days.
        /// </param>
        /// <param name="argumentstreatment">
        /// Function for handling arguments before save history. E.g.: masked password. Default value null.
        /// </param>
        /// <param name="colorizeSessionInitMessage">
        /// Function to user colorize initial title session interative. Default value null.
        /// </param>
        /// <returns></returns>
        public static AppRunner UsePromptPlusRepl(
                this AppRunner appRunner,
                bool enabledSugestion = true,
                Func<SugestionInput, SugestionOutput> suggestionHandler = null,
                bool sugestionEnterTryFininsh = false,
                bool usesugestionargumenttype = true,
                bool useoptionsintercept = true,
                char? shortoptionname = 'i',
                string longoptionname = "interactive",
                string description = "enter an interactive session",
                bool enabledhistory = true,
                byte pageSize = 5,
                TimeSpan? timeouthistory = null,
                Func<string[], string[]> argumentstreatment = null,
                Func<string, ColorToken> colorizeSessionInitMessage = null)
        {
            if (!timeouthistory.HasValue)
            {
                timeouthistory = new TimeSpan(30, 0, 0, 0, 0);
            }
            if (pageSize < 1)
            {
                pageSize = 1;
            }
            if (argumentstreatment is null)
            {
                argumentstreatment = (args) => args;
            }
            if (!shortoptionname.HasValue && string.IsNullOrEmpty(longoptionname))
            {
                throw new InvalidConfigurationException(
                    string.Format(Exceptions.Ex_ReplControlOptions, $"{nameof(shortoptionname)} / {nameof(longoptionname)}"));
            }

            return appRunner.Configure(c =>
            {
                if (!c.Services.Contains<IConsoleDriver>())
                {
                    throw new InvalidConfigurationException(
                        string.Format(Exceptions.Ex_RegisterConsole, nameof(UsePromptPlusAnsiConsole), nameof(IConsoleDriver)));
                }

                var option = new Option(longoptionname, shortoptionname, TypeInfo.Flag, ArgumentArity.Zero, definitionSource: nameof(UsePromptPlusRepl))
                {
                    Description = description
                };

                var config = new ReplConfig(appRunner, null, option, enabledSugestion, suggestionHandler, sugestionEnterTryFininsh, usesugestionargumenttype,useoptionsintercept, enabledhistory, pageSize, timeouthistory.Value, argumentstreatment, colorizeSessionInitMessage);

                c.Services.Add(config);

                c.UseMiddleware(ReplSessionHooks, MiddlewareSteps.ParseInput);

                c.BuildEvents.OnCommandCreated += (args) =>
                {
                    var builder = args.CommandBuilder;
                    var command = builder.Command;
                    if (!config.InSession && command.IsRootCommand())
                    {
                        if (command.IsExecutable)
                        {
                            throw new InvalidConfigurationException($"Root command {command.Name} has been defined as executable using [DefaultCommand] on method {command.DefinitionSource}. This is not suitable for hosting an interactive session.");
                        }
                        builder.AddArgument(option);
                    }
                };
            });
        }

        private static Task<int> ReplSessionHooks(CommandContext ctx, ExecutionDelegate next)
        {
            var parseResult = ctx.ParseResult!;
            var cmd = parseResult.TargetCommand;

            var config = ctx.AppConfig.Services.GetOrThrow<ReplConfig>();
            config.ReplContext = ctx;

            if (config.InSession && config.InSessionParse)
            {
                if (parseResult.ParseError is not null)
                {
                    config.ReplContext = ctx;
                    return Task.FromResult(-1);
                }
                return Task.FromResult(0);
            }

            if (config.InSession || (parseResult.ParseError is not null && parseResult.HelpWasRequested()))
            {
                return next(ctx);
            }

            if (cmd.GetRootCommand().HasInputValues(config.Option.Name))
            {
                config.InSession = true;
                new ReplSession(config).Start();
                config.InSession = false;
                return ExitCodes.Success;
            }

            return next(ctx);
        }


        private static void PromptPlusWizard(CommandContext context, int pageSize, HotKey runwizard, HotKey backcommand, ConsoleColor forecolorwizard, ConsoleColor backcolorwizard, ConsoleColor missingforecolorwizard)
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
                Build = runwizard,
                BackCommand = backcommand,
                RootCommand = context.RootCommand,
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                MissingForeColor = missingforecolorwizard,
                IsRootControl = true,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = Select<IArgumentNode>(Messages.WizardNext)
                .AddItems(lstTokens)
                .TextSelector(x => x?.Name ?? string.Empty)
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
                        return string.Format(Messages.WizardCommandFor, context.RootCommand.Name, desc);
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
            if (sel.Value.GetType().Name == nameof(Option))
            {
                var opt = (Option)sel.Value;
                if (opt.ShortName is not null)
                {
                    promptPlusargs.Add(new WizardArgs($"-{opt.ShortName}", sel.Value, false, true));
                }
                else if (opt.LongName is not null)
                {
                    promptPlusargs.Add(new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                }
            }
            else if (sel.Value.GetType().Name == nameof(Command))
            {
                var cmd = (Command)sel.Value;
                promptPlusargs.Add(new WizardArgs(cmd.Name, sel.Value, false, true));
            }

            var finishwizard = false;
            var localcommandContext = context;
            var localcommand = context.RootCommand;
            var localisroot = true;
            if (sel.Value.GetType().Name == nameof(Command))
            {
                localcommand = (Command)sel.Value;
            }
            while (!finishwizard)
            {
                var args = PromptPlusWizardNext(out finishwizard, out localisroot, localisroot, out localcommand, localcommand, promptPlusargs, localcommandContext, pageSize, runwizard, backcommand, forecolorwizard, backcolorwizard, missingforecolorwizard);
                promptPlusargs.Clear();
                promptPlusargs.AddRange(args);
            }
            context.Tokens = Tokenizer.Tokenize(promptPlusargs.Select(a => a.ArgValue));
        }

        private static WizardArgs[] PromptPlusWizardNext(out bool finishwizard, out bool isroot, bool iniroot, out Command currentCommand, Command inicommand, List<WizardArgs> promptargs, CommandContext context, int pageSize, HotKey runwizard, HotKey backcommand, ConsoleColor forecolorwizard, ConsoleColor backcolorwizard, ConsoleColor missingforecolorwizard)
        {
            finishwizard = false;
            isroot = iniroot;
            currentCommand = inicommand;

            var lstTokens = new List<IArgumentNode>();
            lstTokens.Clear();
            //add all sub-commands
            lstTokens.AddRange(inicommand.Subcommands);
            if (inicommand.Subcommands.Count == 0)
            {
                //add all Arguments for Command
                var args = inicommand.AllArguments(true, false);
                var lstTokensOpe = args.Where(a => a.GetType() == typeof(Operand));
                lstTokens.AddRange(lstTokensOpe);
                lstTokens.AddRange(args
                    .Where(a => a.GetType() == typeof(Option))
                    .Where(a => ((Option)a).Parent.Name == inicommand.Name));
            }
            else
            {
                //add all options to current command
                lstTokens.AddRange(inicommand.AllOptions(true, false));
            }
            var sel = new WizardControl(new WizardOptions()
            {
                IsRootControl = iniroot,
                Build = runwizard,
                BackCommand = backcommand,
                RootCommand = context.RootCommand,
                EnabledBackCommand = inicommand.Parent is not null,
                ForeColor = forecolorwizard,
                BackColor = backcolorwizard,
                MissingForeColor = missingforecolorwizard,
                Message = AppDomain.CurrentDomain.FriendlyName,
                WizardControl = Select<IArgumentNode>("Next")
                .AddItems(lstTokens)
                .TextSelector(x => x?.Name ?? string.Empty)
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
                        return string.Format(Messages.WizardCommandFor, inicommand.Name, desc);
                    }
                    else if (x.GetType().Name == nameof(Option))
                    {
                        return string.Format(Messages.WizardOptionFor, ((Option)x).Parent.Name, desc);
                    }
                    else
                    {
                        return string.Format(Messages.WizardOperandFor, inicommand.Name, desc);
                    }
                })
                .ToPipe(null, context.RootCommand.Name),
            })
                .Config(ctx =>
                {
                    ctx.HideAfterFinish(true);
                })
                .UpdateTokenArgs(promptargs.Select(a => new WizardArgs(a.ArgValue, a.IsSecret, a.IsEnabled)))
                .Run(context.CancellationToken);

            if (sel.IsAborted)
            {
                if (sel.IsAllAborted)
                {
                    promptargs = RemoveLastCommand(promptargs);
                    currentCommand = inicommand.Parent;
                    isroot = false;
                    finishwizard = false;
                    return promptargs.ToArray();
                }
                else
                {
                    promptargs.Clear();
                }
            }

            if (!sel.IsAborted && sel.Value is not null)
            {
                if (sel.Value.GetType().Name == nameof(Command))
                {
                    if (((Command)sel.Value).IsRootCommand())
                    {
                        currentCommand = (Command)sel.Value;
                        isroot = false;
                        finishwizard = true;
                        return promptargs.ToArray();
                    }
                    promptargs.Add(new WizardArgs(sel.Value.Name, sel.Value, false, true));
                    currentCommand = (Command)sel.Value;
                    isroot = true;
                    finishwizard = false;
                    return RemoveMidleOptions(promptargs).ToArray(); ;
                }
                if (sel.Value.GetType().Name == nameof(Option))
                {
                    var opt = (Option)sel.Value;
                    promptargs = RemoveMidleOptions(promptargs);
                    if (opt.IsMiddlewareOption)
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
                        currentCommand = inicommand;
                        isroot = false;
                        finishwizard = false;
                        return promptargs.ToArray();
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
                        if (findvalues.Any())
                        {
                            lastvalue = findvalues.Select(f => f.ArgValue).ToArray();
#if NET5_0_OR_GREATER
                            if (opt.Split.HasValue)
                            {
                                lastvalue = findvalues.Select(f => f.ArgValue).First().Split(opt.Split.Value);
                            }
#endif
                        }

                        if (kindprompt == PromptPlusTypeKind.None)
                        {
                            resultope = Common.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, lastvalue, out iscancelrequest);
                        }
                        else
                        {
                            resultope = Common.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, kindprompt, lastvalue, out iscancelrequest);
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

#if NETSTANDARD2_1_OR_GREATER
                            foreach (var item in resultope)
                            {
                                indexparent++;
                                if (opt.ShortName is not null)
                                {
                                    promptargs.Insert(indexparent, new WizardArgs($"-{opt.ShortName}", sel.Value, false, true));
                                }
                                else if (opt.LongName is not null)
                                {
                                    promptargs.Insert(indexparent, new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                                }
                                indexparent++;
                                promptargs.Insert(indexparent, new WizardArgs(item, sel.Value, ispassword, true));
                            }
#endif
#if NET5_0_OR_GREATER
                            indexparent++;
                            if (opt.ShortName is not null)
                            {
                                promptargs.Insert(indexparent, new WizardArgs($"-{opt.ShortName}", sel.Value, false, true));
                            }
                            else if (opt.LongName is not null)
                            {
                                promptargs.Insert(indexparent, new WizardArgs($"--{opt.LongName}", sel.Value, false, true));
                            }
                            if (opt.Split.HasValue)
                            {
                                var joinresult = string.Join(opt.Split.Value, resultope);
                                indexparent++;
                                promptargs.Insert(indexparent, new WizardArgs(joinresult, sel.Value, ispassword, true));
                            }
                            else
                            {
                                foreach (var item in resultope)
                                {
                                    indexparent++;
                                    promptargs.Insert(indexparent, new WizardArgs(item, sel.Value, ispassword, true));
                                }
                            }
#endif
                        }
                        currentCommand = inicommand;
                        isroot = false;
                        finishwizard = false;
                        return promptargs.ToArray();
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
                        if (findvalues.Any())
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
                        resultope = Common.PromptForTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, lastvalue, out iscancelrequest);
                    }
                    else
                    {
                        resultope = Common.PromptForPromptPlusTypeArgumentValues(context, (IArgument)sel.Value, sel.Value.Description, pageSize, kindprompt, lastvalue, out iscancelrequest);
                    }
                    if (!iscancelrequest)
                    {
                        if (resultope.Count > 1)
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
                    currentCommand = inicommand;
                    isroot = false;
                    finishwizard = false;
                    return promptargs.ToArray();
                }
            }
            currentCommand = inicommand;
            isroot = false;
            finishwizard = true;
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

        private static List<WizardArgs> RemoveMidleOptions(List<WizardArgs> args)
        {
            var lstdel = new List<WizardArgs>();
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].ArgNode.GetType() == typeof(Option) && ((Option)args[i].ArgNode).IsMiddlewareOption)
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
            var qtdenabled = false;
            for (var i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].ArgNode.GetType() == typeof(Command))
                {
                    break;
                }
                if (args[i].IsEnabled && args[i].ArgNode.GetType() == typeof(Operand))
                {
                    qtdenabled = true;
                    break;

                }
            }
            if (!qtdenabled)
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

        private static List<WizardArgs> RemoveLastCommand(List<WizardArgs> args)
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

        private static Task<int> CommandHooksBuildWizard(string name,
            CommandContext context, ExecutionDelegate next,
            Action<CommandContext, int, HotKey, HotKey, ConsoleColor, ConsoleColor, ConsoleColor>? beforeCommandRun,
            int pageSize,
            HotKey runwizard,
            HotKey backcommand,
            ConsoleColor forecolorwizard,
            ConsoleColor backcolorwizard,
            ConsoleColor missingforecolorwizard)
        {
            if (context.Tokens.Directives.Count == 1)
            {
                if (context.Tokens.Directives.First().Value == name)
                {
                    beforeCommandRun?.Invoke(context, pageSize, runwizard, backcommand, forecolorwizard, backcolorwizard, missingforecolorwizard);
                }
            }
            return next(context);
        }
    }
}
