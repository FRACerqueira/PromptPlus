// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using CommandDotNet;
using CommandDotNet.Builders;
using CommandDotNet.Tokens;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.CommandDotNet
{
    internal class ReplSession
    {
        private readonly ReplConfig _replConfig;

        public ReplSession(ReplConfig replConfig)
        {
            _replConfig = replConfig ?? throw new ArgumentNullException(nameof(replConfig));
            if (replConfig.EnabledSugestion)
            {
                replConfig.SuggestionHandler ??= PPLusSugestion;
            }
            else
            {
                replConfig.SuggestionHandler = null;
            }
        }

        public void Start()
        {
            var sessionInitMessage = _replConfig.DefaultSessionInit();
            while (!_replConfig.ReplContext.CancellationToken.IsCancellationRequested)
            {
                var initialvalue = string.Empty;
                string initialerror;
                if (_replConfig.ReplContext.Original.Args.Count == 1 && string.IsNullOrEmpty(_replConfig.ReplContext.Original.Args.First()))
                {
                    initialerror = null;
                }
                else
                {
                    initialerror = _replConfig.ReplContext.ParseResult.ParseError?.Message;
                }

                //skip option interative when found
                initialvalue = InitialValueWidthoutInterativeOption();

                if (!_replConfig.InSessionParse)
                {
                    PromptPlus.WriteLine();
                    if (_replConfig.ColorizeSessionInitMessage is not null)
                    {
                        PromptPlus.WriteLine(_replConfig.ColorizeSessionInitMessage(sessionInitMessage));
                    }
                    else
                    {
                        PromptPlus.WriteLine(sessionInitMessage);
                    }
                    if (_replConfig.EnabledHistory)
                    {
                        PromptPlus.WriteLine(Resources.Messages.Repl_history_update, PromptPlus.ColorSchema.Hint);
                        PromptPlus.WriteLine(string.Format(Resources.Messages.Repl_history_note, _replConfig.TimeoutHistory.Days), PromptPlus.ColorSchema.Hint);
                    }
                    PromptPlus.WriteLine();
                }

                _replConfig.InSessionParse = false;

                var ctrlreadline = PromptPlus.Readline($"{_replConfig.AppName} >>>", Resources.Messages.Repl_default_description)
                    .Config((ctx) =>
                    {
                        ctx.EnabledAbortKey(false)
                           .HideAfterFinish(true);
                    })
                    .InitialValue(initialvalue, initialerror)
                    .PageSize(_replConfig.PagesizeHistory)
                    .FileNameHistory(_replConfig.DefaultFileHistory)
                    .EnabledHistory(_replConfig.EnabledHistory)
                    .SaveHistoryAtFinish(false)
                    .MinimumPrefixLength(1)
                    .TimeoutHistory(_replConfig.TimeoutHistory)
                    .FinisWhenHistoryEnter(_replConfig.SugestionEnterTryFininsh)
                    .SuggestionHandler(_replConfig.SuggestionHandler)
                    .Run(_replConfig.ReplContext.CancellationToken);

                //if host CancellationToken = true
                if (ctrlreadline.IsAborted)
                {
                    return;
                }

                var args = new CommandLineStringSplitter().Split(ctrlreadline.Value).ToList();
                if (args.Count == 1)
                {
                    var singleArg = args[0].ToLowerInvariant();
                    if (singleArg == "quit")
                    {
                        return;
                    }
                }
                else if (_replConfig.EnabledHistory && args.Count == 2 && args[0].ToLowerInvariant() == "replhistory")
                {
                    var param = args[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var days = 0;
                    var hours = 0;
                    var mins = 0;
                    var ok = true;
                    for (var i = 0; i < Math.Min(param.Length, 3); i++)
                    {
                        if (i == 0)
                        {
                            ok = int.TryParse(param[0], out days);
                        }
                        else if (i == 1 && ok)
                        {
                            ok = int.TryParse(param[1], out hours);
                        }
                        if (i == 2 && ok)
                        {
                            ok = int.TryParse(param[2], out mins);
                        }
                    }

                    if (ok)
                    {
                        //history
                        FileHistory.UpdateHistory(_replConfig.DefaultFileHistory, new TimeSpan(days, hours, mins, 0));
                        if (days + hours + mins == 0)
                        {
                            PromptPlus.WriteLine(Resources.Messages.Repl_clear_history);
                        }
                        else
                        {
                            PromptPlus.WriteLine(string.Format(Resources.Messages.Repl_Ok_history, days, hours, mins));
                        }
                    }
                    else
                    {
                        PromptPlus.WriteLine(Resources.Messages.Repl_invalid_values_history);
                    }
                    //reset conext to root-command for new interation
                    //when true command pipeline not continue. Purpose only availble parse
                    initialvalue = string.Empty;
                    _replConfig.InSessionParse = true;
                    _replConfig.AppRunner.Run(initialvalue);
                    _replConfig.InSessionParse = false;
                    continue;
                }

                //when true command pipeline not continue. Purpose only availble parse
                _replConfig.InSessionParse = true;
                var resultrepl = _replConfig.AppRunner.Run(args.ToArray());

                //parse ok then run again with full command pipeline
                if (resultrepl == 0)
                {
                    _replConfig.InSessionParse = false;

                    if (_replConfig.EnabledHistory)
                    {
                        var lastvalue = string
                            .Join(" ",
                            _replConfig.ArgumentsTreatment.Invoke(_replConfig.ReplContext.Original.Args.ToArray()))
                            .Trim();
                        //save history
                        var hist = FileHistory.LoadHistory(_replConfig.DefaultFileHistory);
                        hist = FileHistory.AddHistory(lastvalue, _replConfig.TimeoutHistory, hist);
                        FileHistory.SaveHistory(_replConfig.DefaultFileHistory, hist);
                    }

                    //show readline to run
                    PromptPlus.WriteLine($".{Path.AltDirectorySeparatorChar}{_replConfig.AppName} {ctrlreadline.Value}", PromptPlus.ColorSchema.Answer);
                    PromptPlus.WriteLine();
                    //run readline full pipeline
                    _ = _replConfig.AppRunner.Run(args.ToArray());

                    //reset conext to root-command for new interation
                    initialvalue = string.Empty;
                    _replConfig.InSessionParse = true;
                    _replConfig.AppRunner.Run(initialvalue);
                    _replConfig.InSessionParse = false;
                }
            }
        }

        private string InitialValueWidthoutInterativeOption()
        {
            var result = string.Empty;
            var shortrepl = string.Empty;
            if (_replConfig.Option.ShortName.HasValue)
            {
                shortrepl = $"-{_replConfig.Option.ShortName.Value}";
            }
            var longtrepl = string.Empty;
            if (_replConfig.Option.LongName is not null)
            {
                longtrepl = $"--{_replConfig.Option.LongName}";
            }

            foreach (var item in _replConfig.ReplContext.Tokens)
            {
                if (item.Value != shortrepl && item.Value != longtrepl && item.Value.Length > 0)
                {
                    result += item.Value + " ";
                }
            }
            return result.Trim();
        }

        private SugestionOutput PPLusSugestion(SugestionInput arg)
        {
            var result = new SugestionOutput();
            var pos = arg.CursorPrompt;
            var cmdref = _replConfig.ReplContext.RootCommand;
            var lstTokens = new List<IArgumentNode>();
            var cleartext = false;

            if (string.IsNullOrEmpty(arg.PromptText))
            {
                //add all commands
                lstTokens.AddRange(_replConfig.ReplContext.RootCommand.Subcommands);
                //add all options for Command and remove options repl
                lstTokens.AddRange(_replConfig.ReplContext.RootCommand.Options.Where(x => x.Name != _replConfig.Option.Name));
                //add all operands for Command 
                lstTokens.AddRange(_replConfig.ReplContext.RootCommand.Operands);
            }
            else
            {
                cmdref = null;
                string newcontext;
                if (pos < arg.PromptText.Length - 1)
                {
                    cleartext = true;
                    if (arg.PromptText[pos] != ' ')
                    {
                        while (pos < arg.PromptText.Length)
                        {
                            if (arg.PromptText[pos] == ' ')
                            {
                                break;
                            }
                            pos++;
                        }
                    }
                    else
                    {
                        while (pos <= 0)
                        {
                            if (arg.PromptText[pos] != ' ')
                            {
                                break;
                            }
                            pos--;
                        }
                    }
                    newcontext = arg.PromptText.Substring(0, pos);
                }
                else
                {
                    newcontext = arg.PromptText;
                }
                var newargs = new CommandLineStringSplitter().Split(newcontext).ToList();
                _replConfig.InSessionParse = true;
                var parseresult = _replConfig.AppRunner.Run(newargs.ToArray());
                _replConfig.InSessionParse = false;
                if (parseresult != 0)
                {
                    if (!_replConfig.ReplContext.ParseResult.TargetCommand.IsExecutable)
                    {
                        result.SetMsgError(_replConfig.ReplContext.ParseResult.ParseError.Message);
                    }
                    return result;
                }

                result.SetCursorPrompt(newcontext.Length);
                //add all subcommands
                lstTokens.AddRange(_replConfig.ReplContext.ParseResult.TargetCommand.Subcommands);
                var hassubcmd = lstTokens.Count != 0;
                //add all options for Command and remove options repl
                var opc = _replConfig.ReplContext.ParseResult.TargetCommand.Options.Where(x => x.Name != _replConfig.Option.Name);
                foreach (var item in opc)
                {
                    if (item.InputValues.Count == 0)
                    {
                        lstTokens.Add(item);
                    }
                }
                //add all operands for Command 
                lstTokens.AddRange(_replConfig.ReplContext.ParseResult.TargetCommand.Operands);
            }

            var fistope = true;
            foreach (var item in lstTokens)
            {
                if (item is Option option && (cmdref == null || option.Parent == cmdref))
                {
                    if (!_replConfig.UseOptionsIntercept && option.IsInterceptorOption)
                    {
                        continue;
                    }
                    var desc = option.Description;
                    if (string.IsNullOrEmpty(desc))
                    {
                        var aux = option.CustomAttributes.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (aux != null && aux.Length == 1)
                        {
                            desc = ((DescriptionAttribute)aux.First()).Description;
                        }
                    }
                    if (option.ShortName.HasValue)
                    {
                        result.Add($"-{option.ShortName}", cleartext, desc);
                    }
                    else
                    {
                        result.Add($"--{option.LongName}", cleartext, desc);
                    }
                }
                else if (item is Command command && (cmdref == null || command.Parent == cmdref))
                {
                    var desc = command.Description;
                    if (string.IsNullOrEmpty(desc))
                    {
                        var aux = command.CustomAttributes.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (aux != null && aux.Length == 1)
                        {
                            desc = ((DescriptionAttribute)aux.First()).Description;
                        }
                    }
                    result.Add(command.Aliases.First(), true, desc);
                }
                else if (item is Operand operand)
                {
                    var typeinf = operand.TypeInfo.DisplayName ?? string.Empty;

                    var desc = operand.Description;
                    if (string.IsNullOrEmpty(desc))
                    {
                        var aux = operand.CustomAttributes.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (aux != null && aux.Length == 1)
                        {
                            desc = ((DescriptionAttribute)aux.First()).Description;
                        }
                    }
                    if (operand.AllowedValues.Count > 0 && (cmdref == null || operand.Parent == cmdref))
                    {
                        if (operand.Arity.Maximum > operand.InputValues.Count)
                        {
                            if (_replConfig.UseSugestionArgumentType)
                            {
                                desc = $"{desc}.{typeinf}";
                            }
                            foreach (var itemope in operand.AllowedValues)
                            {
                                result.Add(itemope, false, desc);
                            }
                        }
                    }
                    else if (operand.AllowedValues.Count == 0 && (cmdref == null || operand.Parent == cmdref))
                    {
                        if (operand.Arity.Maximum > operand.InputValues.Count && fistope)
                        {
                            fistope = false;
                            var sug = operand.Name;
                            if (_replConfig.UseSugestionArgumentType)
                            {
                                sug = typeinf;
                            }
                            else
                            {
                                desc = $"{desc}.{typeinf}";
                            }
                            result.Add(sug, false, desc);
                        }
                    }
                }
            }

            if (cmdref != null)
            {
                result.Add("replhistory 30,0,0", true, Resources.Messages.Repl_history_description);
                result.Add("quit", true, Resources.Messages.Repl_exit);
            }
            else
            {
                //restore context
                var args = new CommandLineStringSplitter().Split(arg.PromptText).ToList();
                _replConfig.InSessionParse = true;
                _replConfig.AppRunner.Run(args.ToArray());
                _replConfig.InSessionParse = false;
            }

            return result;
        }
    }
}
