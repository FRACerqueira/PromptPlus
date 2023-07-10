using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PPlus;
using PPlus.Controls;

using PromptPlusExample.Models;

using static System.Environment;

namespace PromptPlusExample
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                    .UseConsoleLifetime()
                    .UseEnvironment("CLI")
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddLogging(
                          builder =>
                          {
                              builder.SetMinimumLevel(LogLevel.Information);
                              builder.AddFilter("Microsoft", LogLevel.Warning)
                                     .AddFilter("System", LogLevel.Warning);
                          });
                        services.AddHostedService<MainProgram>();
                    }).Build();
            await host.RunAsync();
        }
    }

    internal class MainProgram : IHostedService
    {

        private readonly IHostApplicationLifetime _appLifetime;
        private readonly CancellationToken _stopApp;
        private Task _menutask;
        public MainProgram(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _stopApp = _appLifetime.ApplicationStopping;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _menutask = Task.Run(() =>
            {
                try
                {
                    ShowMenu();
                }
                catch (Exception ex)
                {
                    PromptPlus.WriteLine(ex);
                    throw;
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            if (_menutask != null)
            {
                await _menutask;
                _menutask.Dispose();
            }
        }

        public void ShowMenu()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); ;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);
            PromptPlus.Clear();

            PromptPlus.Banner("PromptPlus").FIGletWidth(CharacterWidth.Fitted).Run();
 
            var quit = false;
            while (!_stopApp.IsCancellationRequested && !quit)
            {
                var type = PromptPlus.Select<ExampleType>("Select Example")
                    .Run(_stopApp);

                if (type.IsAborted)
                {
                    continue;
                }
                switch (type.Value)
                {
                    case ExampleType.Readline:
                        //RunReadlineSample();
                        break;
                    case ExampleType.AutoComplete:
                        RunAutoCompleteSample();
                        break;
                    case ExampleType.ConsoleCmd:
                        RunCommandsSample();
                        break;
                    case ExampleType.ColorText:
                        RunColorTextSample();
                        break;
                    case ExampleType.Banner:
                        RunBannerSample();
                        break;
                    case ExampleType.SaveLoadConfig:
                        //RunSaveLoadSample();
                        break;
                    case ExampleType.ChooseLanguage:
                        RunChooseLanguageSample();
                        break;
                    case ExampleType.AnyKey:
                        RunAnyKeySample();
                        break;
                    case ExampleType.KeyPress:
                        RunKeyPressSample();
                        break;
                    case ExampleType.Input:
                        RunInputSample();
                        break;
                    case ExampleType.InputWithHistoric:
                        //RunInputWithHistoricSample();
                        break;
                    case ExampleType.InputWithsuggestions:
                        //RunInputWithsuggestionsSample();
                        break;
                    case ExampleType.Confirm:
                        //RunConfirmSample();
                        break;
                    case ExampleType.MaskEditGeneric:
                        RunMaskEditGenericSample();
                        break;
                    case ExampleType.MaskEditDate:
                        RunMaskEditDateSample();
                        break;
                    case ExampleType.MaskEditTime:
                        RunMaskEditTimeSample();
                        break;
                    case ExampleType.MaskEditDateTime:
                        RunMaskEditDateTimeSample();
                        break;
                    case ExampleType.MaskEditNumber:
                        RunMaskEditNumberSample("N");
                        break;
                    case ExampleType.MaskEditCurrrency:
                        RunMaskEditNumberSample("C");
                        break;
                    case ExampleType.Password:
                        RunPasswordSample();
                        break;
                    case ExampleType.Select:
                        RunSelectSample();
                        break;
                    case ExampleType.MultiSelectGroup:
                        RunMultiSelectGroupSample();
                        break;
                    case ExampleType.MultiSelect:
                        RunMultiSelectSample();
                        break;
                    case ExampleType.SelectWithEnum:
                        RunSelectEnumSample();
                        break;
                    case ExampleType.SelectWithAutoSelect:
                        RunSelectWithAutoSelectSample();
                        break;
                    case ExampleType.MultiSelectWithEnum:
                        RunMultiSelectEnumSample();
                        break;
                    case ExampleType.List:
                        RunListSample();
                        break;
                    case ExampleType.ListWithsuggestions:
                        ListWithsuggestionsSample();
                        break;
                    case ExampleType.ListMasked:
                        RunListMaskedSample();
                        break;
                    case ExampleType.FolderBrowser:
                        RunFolderSample();
                        break;
                    case ExampleType.FileBrowser:
                        RunFileSample();
                        break;
                    case ExampleType.SliderNumber:
                        RunSliderNumberSample();
                        break;
                    case ExampleType.NumberUpDown:
                        RunNumberUpDownSample();
                        break;
                    case ExampleType.SliderSwitche:
                        RunSliderSwitcheSample();
                        break;
                    case ExampleType.ProgressbarAsync:
                        //RunProgressbarSample();
                        break;
                    case ExampleType.WaitSingleProcess:
                        //RunWaitSingleProcessSample();
                        break;
                    case ExampleType.WaitManyProcess:
                        //RunWaitManyProcessSample();
                        break;
                    case ExampleType.PipeLine:
                        //RunPipeLineSample();
                        break;
                    case ExampleType.ImportValidators:
                        RunImportValidatorsSample();
                        break;
                    case ExampleType.Quit:
                        quit = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (!_stopApp.IsCancellationRequested && !quit)
                {
                    if (type.Value != ExampleType.AnyKey)
                    {
                        PromptPlus.KeyPress()
                            .Run(_stopApp);
                    }
                }
            }

            if (!quit)
            {
                ExitCode = -1;
            }
            _appLifetime.StopApplication();
        }

        //private void RunReadlineSample()
        //{
        //    var enterHist = PromptPlus.SliderSwitch("Finish When History Enter")
        //        .Default(true)
        //        .Run(_stopApp);
        //    if (enterHist.IsAborted)
        //    {
        //        return;
        //    }

        //    var enabledhis = PromptPlus.SliderSwitch("Enabled History")
        //        .Default(true)
        //        .Run(_stopApp);
        //    if (enabledhis.IsAborted)
        //    {
        //        return;
        //    }
        //    var timeout = 0;
        //    if (enabledhis.Value)
        //    {
        //        var timeouthis = PromptPlus.Select<int>("Timeout(seconds) History")
        //                .AddItem(5)
        //                .AddItem(10)
        //                .AddItem(600)
        //                .Run(_stopApp);
        //        if (enabledhis.IsAborted)
        //        {
        //            return;
        //        }
        //        timeout = timeouthis.Value;
        //    }

        //    var ctrlreadline = PromptPlus.Readline("Readline>", "Sample Readline control")
        //        .AddValidator(PromptPlusValidators.Required())
        //        .PageSize(5)
        //        .EnabledHistory(enabledhis.Value)
        //        .FileNameHistory("RunReadlineSample")
        //        .TimeoutHistory(new TimeSpan(0, 0, timeout))
        //        .FinisWhenHistoryEnter(enterHist.Value)
        //        .SuggestionHandler(mysugestion)
        //        .Run(_stopApp);

        //    if (ctrlreadline.IsAborted)
        //    {
        //        return;
        //    }
        //    PromptPlus.WriteLine($"Result : [cyan]{ctrlreadline.Value}[/]!");
        //}

        //private SugestionOutput mysugestion(SugestionInput arg)
        //{
        //    var aux = new SugestionOutput();
        //    var word = arg.CurrentWord();
        //    if (word.ToLowerInvariant() == "prompt")
        //    {
        //        aux.Add("choose");
        //        aux.Add("secure");
        //        aux.Add("help", true);
        //        return aux;
        //    }
        //    var random = new Random();
        //    for (var i = 0; i < 3; i++)
        //    {
        //        var c1 = (char)random.Next(65, 90);
        //        var c2 = (char)random.Next(97, 122);
        //        var c3 = (char)random.Next(97, 122);
        //        aux.Add($"Opc {c1}{c2}{c3}");
        //    }
        //    aux.Add("opc Clearline -test a b c", true);
        //    return aux;
        //}


        private void RunAutoCompleteSample()
        {
            //var macth = PromptPlus.SliderSwitch("Accept Without Match sugestions")
            //    .Run(_stopApp);
            //if (macth.IsAborted)
            //{
            //    return;
            //}
            var ctrlinput = PromptPlus.AutoComplete("Input value", "Sample autocomplete control")
                .AddValidators(PromptValidators.Required())
                .AddValidators(PromptValidators.MinLength(3))
                //.AddValidator(PromptPlusValidators.Required())
                //.AddValidator(PromptPlusValidators.MinLength(3))
                .CompletionMaxCount(10)
                .ValidateOnDemand()
                .PageSize(5)
                //.CompletionWithDescriptionAsyncService(MYServiceCompleteAsync);
                .CompletionAsyncService(MYServiceCompleteAsync);
            //if (macth.Value)
            //{
            //    ctrlinput.AcceptWithoutMatch();
            //}
            var input = ctrlinput.Run(_stopApp);
            if (input.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Result : [cyan]{input.Value}[/]!");
        }

        private static async Task<string[]> MYServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
        {
            if (count == 0)
            {
                count = 10;
            }
            var random = new Random();
            var items = new List<string>();
            for (var i = 0; i < count; i++)
            {
                var c1 = (char)random.Next(65, 90);
                var c2 = (char)random.Next(97, 122);
                var c3 = (char)random.Next(97, 122);

                items.Add(prefixText + c1 + c2 + c3);
            }
            //delay for sample purpose only
            cancellationToken.WaitHandle.WaitOne(2000);
            return await Task.FromResult(items.ToArray());
        }
        //private async Task<ValueDescription<string>[]> MYServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
        //{
        //    if (count == 0)
        //    {
        //        count = 10;
        //    }
        //    var random = new Random();
        //    var items = new List<ValueDescription<string>>();
        //    for (var i = 0; i < count; i++)
        //    {
        //        var c1 = (char)random.Next(65, 90);
        //        var c2 = (char)random.Next(97, 122);
        //        var c3 = (char)random.Next(97, 122);

        //        items.Add(new ValueDescription<string>(prefixText + c1 + c2 + c3, $"Description {i}"));
        //    }
        //    return await Task.FromResult(items.ToArray());
        //}


        private void RunCommandsSample()
        {
            var quit = false;
            var oldbg = PromptPlus.BackgroundColor;
            while (!_stopApp.IsCancellationRequested && !quit)
            {
                PromptPlus.Clear();
                var opc = PromptPlus.Select<string>("Select command Sample")
                    .AddItem("1 - Clear")
                    .AddItem("2 - ClearLine")
                    .AddItem("3 - ClearRestOfLine")
                    .AddItem("4 - WriteLines")
                    .AddItem("X - End Samples")
                    .AutoSelect()
                    //.AutoSelectIfOne()
                    .Run();

                if (opc.IsAborted)
                {
                    continue;
                }
                if (opc.Value[0] == 'X')
                {
                    quit = true;
                    continue;
                }
                if (opc.Value[0] == '1')
                {
                    var cor = PromptPlus.Select<string>("Select color to clear")
                        .AddItem("None")
                        .AddItem("Blue")
                        .AddItem("Red")
                        .Run();
                    if (cor.IsAborted)
                    {
                        continue;
                    }
                    else if (cor.Value[0] == 'B')
                    {
                        PromptPlus.Clear(ConsoleColor.Blue);
                        PromptPlus.KeyPress()
                            .Run();
                    }
                    else if (cor.Value[0] == 'R')
                    {
                        PromptPlus.Clear(ConsoleColor.Red);
                        PromptPlus.KeyPress()
                            .Run();
                        PromptPlus.Clear(oldbg);

                    }
                    PromptPlus.Clear(oldbg);
                }
                if (opc.Value[0] == '2')
                {
                    PromptPlus.WriteLine("LINE1");
                    PromptPlus.WriteLine("LINE2");
                    PromptPlus.WriteLine("LINE3");
                    PromptPlus.WriteLine("LINE4");
                    PromptPlus.WriteLine("LINE5");
                    PromptPlus.WriteLine("after pressing the key, The LINE 3 and LINE4 will be erased");
                    PromptPlus.KeyPress().Run();
                    var line = PromptPlus.CursorTop;
                    PromptPlus.ClearLine(line - 4);
                    PromptPlus.ClearLine(line - 3);
                    PromptPlus.SetCursorPosition(0, line);
                    PromptPlus.KeyPress().Run();
                }
                if (opc.Value[0] == '3')
                {
                    PromptPlus.WriteLine("LINE1 AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    PromptPlus.WriteLine("after pressing the key, over LINE 1 rest of line will be erased with backgroundcolor red");
                    PromptPlus.KeyPress().Run();
                    var line = PromptPlus.CursorTop;
                    PromptPlus.SetCursorPosition(5, line - 2);
                    //PromptPlus.ClearRestOfLine(ConsoleColor.Red);
                    PromptPlus.SetCursorPosition(0, line);
                    PromptPlus.KeyPress().Run();
                }
                if (opc.Value[0] == '4')
                {
                    PromptPlus.WriteLine("LINE REF");
                    PromptPlus.WriteLine("after pressing the key, 3 new lines will be added");
                    PromptPlus.KeyPress().Run();
                    PromptPlus.WriteLines(3);
                    PromptPlus.KeyPress().Run();
                }
            }
        }

        private void RunColorTextSample()
        {
            PromptPlus.WriteLine();
            PromptPlus.WriteLine("PromptPlus.WriteLine(\"Output1:\",ConsoleColor.White, ConsoleColor.Blue, true)");
            PromptPlus.WriteLine("PromptPlus.WriteLine(\"Output2:\".White().OnBlue().Underline())");
            PromptPlus.WriteLine();
            PromptPlus.WriteLine("Output1:", new Style(ConsoleColor.White, ConsoleColor.Blue), true);
            PromptPlus.WriteLine("This [cyan]is [red]a [white:blue]simples[/] line with [yellow!u]color[/]. End [/]line.");
            //PromptPlus.WriteLine("Output2:".White().OnBlue().Underline());
            //PromptPlus.WriteLine("[cyan]This[/] is another [white:blue]simples[/] line using [red!u]Mask[/].".Mask(ConsoleColor.Yellow));
            PromptPlus.WriteLine();
        }

        private void RunBannerSample()
        {
            var colorsel = PromptPlus.Select<ConsoleColor>("Select a color")
                .AddItemTo(AdderScope.Remove, PromptPlus.BackgroundColor)
                //.HideItem(PromptPlus.BackgroundColor)
                .Run(_stopApp);

            if (colorsel.IsAborted)
            {
                return;
            }

            var widthsel = PromptPlus.Select<CharacterWidth>("Select a Character Width")
                .Run(_stopApp);
            if (widthsel.IsAborted)
            {
                return;
            }

            var fontsel = PromptPlus.Select<string>("Select a font")
                  .AddItem("Default")
                  .AddItem("Starwars")
                  .Run(_stopApp);
            if (fontsel.IsAborted)
            {
                return;
            }

            if (fontsel.Value[0] == 'D')
            {
                PromptPlus.Banner("PromptPlus")
                    .FIGletWidth(widthsel.Value)
                    .Run(colorsel.Value);
            }
            else
            {
                PromptPlus.Banner("PromptPlus")
                    .FIGletWidth(widthsel.Value)
                    .LoadFont("starwars.flf")
                    .Run(colorsel.Value);
            }
        }

        //private void RunSaveLoadSample()
        //{
        //    PromptPlus.TooltipKeyPress = new HotKey(UserHotKey.F1, false, false, false);
        //    PromptPlus.ResumePipesKeyPress = new HotKey(UserHotKey.F2, false, false, false);

        //    var filecfg = PromptPlus.SaveConfigToFile();
        //    PromptPlus.LoadConfigFromFile();
        //    PromptPlus.WriteLine($"PromptPlus file [cyan]saved and readed[/]. Location: {filecfg}");
        //}

        private void RunChooseLanguageSample()
        {
            var envalue = PromptPlus.Select<LanguageOptions>("Select a language")
                .Default(LanguageOptions.English)
                .Run(_stopApp);

            if (envalue.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You selected [cyan]{envalue.Value}[/]");
            switch (envalue.Value)
            {
                case LanguageOptions.English:
                    PromptPlus.Config.DefaultCulture = new CultureInfo("en");
                    break;
                case LanguageOptions.PortugueseBrazil:
                    PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
                    break;
                default:
                    break;
            }
        }

        private void RunMaskEditNumberSample(string numtype)
        {
            var maskqtdint = PromptPlus.MaskEdit("Max integer pos.(empty = 5)")
                    .Mask("C[0123456789]")
                    .Default("5")
                    .Run(_stopApp);
            if (maskqtdint.IsAborted)
            {
                return;
            }
            var maskqtddec = PromptPlus.MaskEdit("Max decimal pos.(empty = 2)")
                    .Mask("C[0123]")
                    .Default("2")
                    .Run(_stopApp);
            if (maskqtddec.IsAborted)
            {
                return;
            }
            var masksignal = PromptPlus.SliderSwitch("Accept negative")
                .Default(true)
                .Run(_stopApp);

            if (masksignal.IsAborted)
            {
                return;
            }
            var opccult = PromptPlus.Select<string>("Select format language")
                .AddItems(new List<string> { "English", "Portuguese Brazil", "French" })
                .Run(_stopApp);

            if (opccult.IsAborted)
            {
                return;
            }
            var qtdint = 5;
            if (maskqtdint.Value.Input.ToString().Length > 0)
            {
                qtdint = int.Parse(maskqtdint.Value.Input.ToString());
            }
            var qtddec = 2;
            if (maskqtddec.Value.Input.ToString().Length > 0)
            {
                qtddec = int.Parse(maskqtddec.Value.Input.ToString());
            }
            var mask = PromptPlus.MaskEdit("Number")
                    .Mask(numtype == "N" ? MaskedType.Number : MaskedType.Currency)
                    .AmmoutPositions(qtdint, qtddec, masksignal.Value)
                    .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : opccult.Value[0] == 'P' ? new CultureInfo("pt-BR") : new CultureInfo("fr-FR"))
                    //.AcceptSignal(masksignal.Value)
                    .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.Masked}[/]!");
            }

        }

        private void RunMaskEditDateTimeSample()
        {
            var opcyear = PromptPlus.Select<string>("Select format year")
                .AddItems(new List<string> { "Four digits", "Two digits" })
                .Run(_stopApp);

            if (opcyear.IsAborted)
            {
                return;
            }
            var opctime = PromptPlus.Select<string>("Select format time")
                .AddItems(new List<string> { "1-Hour,minute,Second", "2-Hour,minute", "3-hour" })
                .Run(_stopApp);

            if (opctime.IsAborted)
            {
                return;
            }
            var masfill = PromptPlus.SliderSwitch("Fill with zeros")
                .Default(true)
                .Run(_stopApp);

            if (masfill.IsAborted)
            {
                return;
            }
            var opccult = PromptPlus.Select<string>("Select format language")
                .AddItems(new List<string> { "English", "Portuguese Brazil" })
                .Run(_stopApp);

            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PromptPlus.MaskEdit("Date and Time")
                .Mask(MaskedType.DateTime)
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatYear(opcyear.Value[0] == 'F' ? FormatYear.Long : FormatYear.Short)
                .FormatTime(opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH)
                //.FillZeros(masfill.Value)
                .FillZeros()
                //.Default(new DateTime(2021,12,31,21,34,56))
                .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.Masked}[/]!");
            }
        }

        private void RunMaskEditTimeSample()
        {
            var opctime = PromptPlus.Select<string>("Select format time")
                .AddItems(new List<string> { "1-Hour,minute,Second", "2-Hour,minute", "3-hour" })
                .Run(_stopApp);

            if (opctime.IsAborted)
            {
                return;
            }
            var masfill = PromptPlus.SliderSwitch("Fill with zeros")
                .Default(true)
                .Run(_stopApp);

            if (masfill.IsAborted)
            {
                return;
            }
            var opccult = PromptPlus.Select<string>("Select format language")
                .AddItems(new List<string> { "English", "Portuguese Brazil" })
                .Run(_stopApp);

            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PromptPlus.MaskEdit("Time")
                .Mask(MaskedType.DateTime)
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatTime(opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH)
                .FillZeros()
                .Run(_stopApp);


            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.Masked}[/]!");
            }
        }

        private void RunMaskEditDateSample()
        {
            var opcyear = PromptPlus.Select<string>("Select format year")
                .AddItems(new List<string> { "Four digits", "Two digits" })
                .Run(_stopApp);

            if (opcyear.IsAborted)
            {
                return;
            }
            var masfill = PromptPlus.SliderSwitch("Fill with zeros")
                .Default(true)
                .Run(_stopApp);

            if (masfill.IsAborted)
            {
                return;
            }

            var week = PromptPlus.SliderSwitch("Show day week")
                .Default(true)
                .Run(_stopApp);

            if (week.IsAborted)
            {
                return;
            }


            var opccult = PromptPlus.Select<string>("Select format language")
                .AddItems(new List<string> { "English", "Portuguese Brazil" })
                .Run(_stopApp);

            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PromptPlus.MaskEdit("Date")
                .Mask(MaskedType.DateOnly)
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatYear(opcyear.Value[0] == 'F' ? FormatYear.Long : FormatYear.Short)
                .FillZeros()
                .DescriptionWithInputType(week.Value ? FormatWeek.Short : FormatWeek.None)
                //.ShowDayWeek(week.Value ? FormatWeek.Short : FormatWeek.None)
                .ValidateOnDemand()
                .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.Masked}[/]!");
            }
        }

        private void RunMaskEditGenericSample()
        {
            var mask = PromptPlus.MaskEdit("Inventory Number")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .ChangeDescription(MyDescMaskedirGeneric)
                //.DescriptionSelector(MyDescMaskedirGeneric)
                .Run(_stopApp);

            if (mask.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.Masked}[/]!");
            }
        }

        private string MyDescMaskedirGeneric(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return null;
            if (arg.Length < 3)
            {
                return "Code of Region";
            }
            if (arg.Length < 6)
                return "Code of Coutry";
            if (arg.Length == 6)
                return "Code of type";
            if (arg.Length == 7)
                return "Code of type-level";
            return "item Id";
        }

        private void RunAnyKeySample()
        {
            var key = PromptPlus.KeyPress()
                    .Run(_stopApp);
            if (key.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, key [cyan]pressed[/]");
        }

        private void RunKeyPressSample()
        {
            //var key = PromptPlus.KeyPress('B', ConsoleModifiers.Control)
            //    .Prompt("Press Ctrl-B to continue")
            //    .Run(_stopApp);

            var key = PromptPlus.KeyPress("Press Ctrl-B to continue")
                .AddKeyValid(ConsoleKey.B, ConsoleModifiers.Control)
                .Run(_stopApp);

            if (key.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, key [cyan]Ctrl-B pressed[/]");
        }

        private void RunImportValidatorsSample()
        {
            var inst = new MylCass();

            PromptPlus.WriteLine("Imported Validators of Myclass, property MyInput:");
            PromptPlus.WriteLine("private class MylCass \n{\n   [Required(ErrorMessage = \"{0} is required!\")] \n   [MinLength(3, ErrorMessage = \"Min. Length = 3.\")] \n   [MaxLength(5, ErrorMessage = \"Max. Length = 5.\")] \n   [Display(Prompt = \"My Input\")]\n   public string MyInput { get; set; }\n}");
            var name = PromptPlus.Input("Input Value for MyInput")
                .AddValidators(PromptValidators.ImportValidators(inst, x => x.MyInput))
                //.AddValidators(inst.ImportValidators(x => x.MyInput))
                .Run(_stopApp);

            if (name.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your input: [cyan]{name.Value}[/]!");
        }

        private class MylCass
        {
            [Required(ErrorMessage = "{0} is required!")]
            [MinLength(3, ErrorMessage = "Min. Length = 3.")]
            [MaxLength(5, ErrorMessage = "Min. Length = 5.")]
            [Display(Prompt = "My Input")]
            public string MyInput { get; set; }
        }

        private void RunInputSample()
        {
            var name = PromptPlus.Input("What's your name?")
                .Default("Peter Parker")
                //.AddValidator(PromptPlusValidators.Required())
                //.AddValidator(PromptPlusValidators.MinLength(3))
                .Run(_stopApp);
            if (name.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, [cyan]{name.Value}[/]!");
        }

        //private void RunInputWithHistoricSample()
        //{
        //    var name = PromptPlus.Input("What's your name?")
        //        .Default("Peter Parker")
        //        //.AddValidator(PromptPlusValidators.Required())
        //        //.AddValidator(PromptPlusValidators.MinLength(3))
        //        .SuggestionHandler(SugestionInputSample, true)
        //        .Config((ctx) =>
        //        {
        //            ctx.AddExtraAction(StageControl.OnStartControl, LoadSampleHistInputSugestion)
        //               .AddExtraAction(StageControl.OnFinishControl, SaveSampleHistSugestion);
        //        })
        //        .Run(_stopApp);
        //    if (name.IsAborted)
        //    {
        //        return;
        //    }
        //    PromptPlus.WriteLine($"Hello, [cyan]{name.Value}[/]!");
        //}
        //private void RunInputWithsuggestionsSample()
        //{
        //    var name = PromptPlus.Input("what is your favorite color?")
        //         .SuggestionHandler(SugestionInputColorSample, true)
        //        .Run(_stopApp);
        //    if (name.IsAborted)
        //    {
        //        return;
        //    }
        //    PromptPlus.WriteLine($"[cyan]{name.Value}[/]!");
        //}


        //private IList<ItemHistory> _itemsInputSampleHistory;

        //private SugestionOutput SugestionInputColorSample(SugestionInput arg)
        //{
        //    var result = new SugestionOutput();
        //    result.Add("Red");
        //    result.Add("Blue");
        //    result.Add("Green");
        //    return result;
        //}

        //private SugestionOutput SugestionInputSample(SugestionInput arg)
        //{
        //    var result = new SugestionOutput();
        //    if (_itemsInputSampleHistory.Count > 0)
        //    {
        //        foreach (var item in _itemsInputSampleHistory
        //            .OrderByDescending(x => x.TimeOutTicks))
        //        {
        //            result.Add(item.History, true);
        //        }
        //    }
        //    return result;
        //}

        //private void LoadSampleHistInputSugestion(object ctx, string value)
        //{
        //    _itemsInputSampleHistory  = FileHistory
        //        .LoadHistory($"{AppDomain.CurrentDomain.FriendlyName}_SampleHistInputSugestion");
        //}

        //private void SaveSampleHistSugestion(object ctx, string value)
        //{
        //    if (value is null)
        //    {
        //        return;
        //    }
        //    var localnewhis = value.Trim();
        //    var found = _itemsInputSampleHistory
        //        .Where(x => x.History.ToLowerInvariant() == localnewhis.ToLowerInvariant())
        //        .ToArray();
        //    if (found.Length > 0)
        //    {
        //        foreach (var item in found)
        //        {
        //            _itemsInputSampleHistory.Remove(item);
        //        }
        //    }
        //    if (_itemsInputSampleHistory.Count >= byte.MaxValue)
        //    {
        //        _itemsInputSampleHistory.RemoveAt(_itemsInputSampleHistory.Count - 1);
        //    }
        //    _itemsInputSampleHistory.Insert(0,
        //        ItemHistory.CreateItemHistory(localnewhis, new TimeSpan(1, 0, 0, 0)));

        //    FileHistory.SaveHistory(
        //        $"{AppDomain.CurrentDomain.FriendlyName}_SampleHistInputSugestion",
        //        _itemsInputSampleHistory);
        //}

        //private void RunConfirmSample()
        //{
        //    var opccult = PromptPlus.Select<string>("Select format language")
        //        .AddItems(new List<string> { "English", "Portuguese Brazil" })
        //        .Run(_stopApp);

        //    if (opccult.IsAborted)
        //    {
        //        return;
        //    }
        //    if (opccult.Value[0] == 'E')
        //    {
        //        PromptPlus.Config.DefaultCulture = new CultureInfo("en-US");
        //        var answer = PromptPlus.Confirm("Are you ready?")
        //            //.Default(true)
        //            .Run(_stopApp);
        //        if (answer.IsAborted)
        //        {
        //            return;
        //        }
        //        if (answer.Value)
        //        {
        //            PromptPlus.WriteLine($"Sua resposta é [cyan]Yes[/]");
        //        }
        //        else
        //        {
        //            PromptPlus.WriteLine($"Sua resposta é [cyan]No[/]");
        //        }
        //    }
        //    else
        //    {
        //        PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
        //        var answer = PromptPlus.Confirm("Você esta pronto?")
        //            .Default(true)
        //            .Run(_stopApp);

        //        if (answer.IsAborted)
        //        {
        //            return;
        //        }
        //        if (answer.Value)
        //        {
        //            PromptPlus.WriteLine($"Sua resposta é [cyan]Sim[/]");
        //        }
        //        else
        //        {
        //            PromptPlus.WriteLine($"Sua resposta é [cyan]Não[/]");
        //        }
        //    }
        //}

        //private void RunWaitSingleProcessSample()
        //{
        //    var process = PromptPlus.WaitProcess("phase 1")
        //         .AddProcess(new SingleProcess((_stopApp) =>
        //             {
        //                 _stopApp.WaitHandle.WaitOne(4000);
        //                 if (_stopApp.IsCancellationRequested)
        //                 {
        //                     return Task.FromResult<object>("canceled");
        //                 }
        //                 return Task.FromResult<object>("Done");
        //             }))
        //         .Run(_stopApp);

        //    var aux = process.Value.First();

        //    PromptPlus.WriteLine($"Result task ({aux.ProcessId}) : {aux.TextResult}. Property IsCanceled = {aux.IsCanceled}");
        //}

        //private void RunWaitManyProcessSample()
        //{
        //    var Process = PromptPlus.WaitProcess("My Tasks(3) Async")
        //         .AddProcess(new SingleProcess((_stopApp) =>
        //             {
        //                 _stopApp.WaitHandle.WaitOne(10000);
        //                 if (_stopApp.IsCancellationRequested)
        //                 {
        //                     return Task.FromResult<object>("canceled");
        //                 }
        //                 return Task.FromResult<object>("Done");
        //             }, "Task1"))
        //         .AddProcess(new SingleProcess((_stopApp) =>
        //             {
        //                 _stopApp.WaitHandle.WaitOne(5000);
        //                 if (_stopApp.IsCancellationRequested)
        //                 {
        //                     return Task.FromResult<object>(-1);
        //                 }
        //                 return Task.FromResult<object>(1);
        //             }, "Task2"))
        //         .AddProcess(new SingleProcess((_stopApp) =>
        //             {
        //                 _stopApp.WaitHandle.WaitOne(7000);
        //                 if (_stopApp.IsCancellationRequested)
        //                 {
        //                     return Task.FromResult<object>("Canceled");
        //                 }
        //                 return Task.FromResult<object>("Done");
        //             }, "Task3"))
        //         .Run(_stopApp);

        //    foreach (var item in Process.Value)
        //    {
        //        PromptPlus.WriteLine($"Result tasks ({item.ProcessId}) : {item.ValueProcess}");
        //    }
        //}

        //private void RunProgressbarSample()
        //{
        //    var progress = PromptPlus.Progressbar("Processing Tasks", "My Process")
        //        .UpdateHandler(UpdateSampleHandlerAsync)
        //        .Run(_stopApp);

        //    if (progress.IsAborted)
        //    {
        //        PromptPlus.WriteLine($"Your result is: {progress.Value.Message} Canceled!");
        //        return;
        //    }
        //    PromptPlus.WriteLine($"Your result is: {progress.Value.Message}");
        //}

        //private async Task<ProgressBarInfo> UpdateSampleHandlerAsync(ProgressBarInfo status, CancellationToken cancellationToken)
        //{
        //    await Task.Delay(10);
        //    var aux = (int)status.InterationId + 1;
        //    var endupdate = true;
        //    if (aux < 100)
        //    {
        //        endupdate = false;
        //    }
        //    return new ProgressBarInfo(aux, endupdate, $"Interation {aux}", aux);
        //}

        private void RunNumberUpDownSample()
        {
            var number = PromptPlus.SliderNumber("Select a number")
                .MoveKeyPress(SliderNumberType.UpDown)
                .Default(5.5)
                .Range(0, 10)
                .Step(0.1)
                .FracionalDig(1)
                .Run(_stopApp);

            if (number.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderNumberSample()
        {
            var number = PromptPlus.SliderNumber("Select a number")
                .MoveKeyPress(SliderNumberType.LeftRight)
                .Default(5.5)
                .Range(0, 10)
                .Step(0.1)
                .FracionalDig(1)
                .Run(_stopApp);

            if (number.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderSwitcheSample()
        {
            var slider = PromptPlus.SliderSwitch("Turn on/off")
                .Run(_stopApp);

            if (slider.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your answer is: {slider.Value}");

            var slider1 = PromptPlus.SliderSwitch("Custom")
                .OnValue("My On-Value")
                .OffValue("My Off-Value")
                .Run(_stopApp);

            if (slider1.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your answer is: {slider1.Value}");
        }
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }


        public static PasswordScore CheckStrength(string password)
        {
            var score = 1;
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 5)
                score++;
            if (password.Length >= 7)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))   //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            return (PasswordScore)score;
        }


        private void RunPasswordSample()
        {
            var pwd = PromptPlus.Input("Type new password")
                .IsSecret()
                //.AddValidator(PromptPlusValidators.Required())
                //.AddValidator(PromptPlusValidators.MinLength(5))
                .ValidateOnDemand()
                .ChangeDescription(x =>
                {
                    return $"password has {CheckStrength(x)}";
                })
                .Run(_stopApp);

            if (pwd.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Password OK : {pwd.Value}");
        }

        private void RunListSample()
        {
            var lst = PromptPlus.AddtoList("Please add item(s)", "Sample List")
                .PageSize(3)
                .InputToCase(CaseOptions.Uppercase)
                //.UpperCase(true)
                .AddItem("aaa")
                .AddItem("bbb")
                .AddItem("ccc")
                //.InitialValue("teste")
                //.AddValidator(PromptPlusValidators.MinLength(3))
                //.DescriptionSelector(x =>
                //{
                //    var result = x;
                //    var random = new Random();
                //    for (var i = 0; i < 5; i++)
                //    {
                //        var c1 = (char)random.Next(65, 90);
                //        var c2 = (char)random.Next(97, 122);
                //        var c3 = (char)random.Next(97, 122);
                //        result = result + c1 + c2 + c3;
                //    }
                //    return result;
                //})
                //.ValidateOnDemand()
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private void ListWithsuggestionsSample()
        {
            var lst = PromptPlus.AddtoList("Please add item(s)", "Sample List")
                .PageSize(3)
                //.UpperCase(true)
                .AddItem("aaa")
                .AddItem("bbb")
                .AddItem("ccc")
                //.InitialValue("teste")
                //.AddValidator(PromptPlusValidators.MinLength(3))
                .AllowDuplicate()
                //.SuggestionHandler(SugestionListSample, true)
                //.DescriptionSelector(x =>
                //{
                //    var result = x;
                //    var random = new Random();
                //    for (var i = 0; i < 5; i++)
                //    {
                //        var c1 = (char)random.Next(65, 90);
                //        var c2 = (char)random.Next(97, 122);
                //        var c3 = (char)random.Next(97, 122);
                //        result = result + c1 + c2 + c3;
                //    }
                //    return result;
                //})
                //.ValidateOnDemand()
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private SugestionOutput SugestionListSample(SugestionInput arg)
        {
            var result = new SugestionOutput();
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var c1 = new string((char)random.Next(65, 90), 5);
                if (arg.Context != null)
                {
                    var ctx = (IEnumerable<string>)arg.Context;
                    if (!ctx.Contains(c1))
                    {
                        result.Add(c1);
                    }
                }
                else
                {
                    result.Add(c1);
                }
            }
            return result;
        }

        private void RunListMaskedSample()
        {
            var lst = PromptPlus.AddtoMaskEditList("Please add item(s)")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                //.MaskType(MaskedType.Generic, @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                //.UpperCase(true)
                .AddItem("XYZ 123-EUA-A1-AAA")
                .AddItem("XYZ 123-EUA-A2-AAA")
                //.InitialValue("XYZ 123", true)
                //.DescriptionSelector(MyDescMaskedirGeneric)
                //.ValidateOnDemand()
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            foreach (var item in lst.Value)
            {
                PromptPlus.WriteLine($"You picked {item.Masked}");

            }
        }

        private void RunSelectWithAutoSelectSample()
        {
            var city = PromptPlus.Select<string>("Select your city")
                .AddItems(new[] { "1 - Seattle", "2 - London", "3 - Tokyo", "4 - New York", "5 - Singapore", "6 - Shanghai" })
                .PageSize(3)
                //.AutoSelectIfOne()
                .Run(_stopApp);

            if (city.IsAborted)
            {
                return;
            }
            if (!string.IsNullOrEmpty(city.Value))
            {
                PromptPlus.WriteLine($"Hello, {city.Value}!");
            }
            else
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
        }

        private void RunSelectSample()
        {
            var city = PromptPlus.Select<string>("Select your city")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .PageSize(3)
                .Run(_stopApp);

            if (city.IsAborted)
            {
                return;
            }
            if (!string.IsNullOrEmpty(city.Value))
            {
                PromptPlus.WriteLine($"Hello, {city.Value}!");
            }
            else
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
        }

        private void RunMultiSelectGroupSample()
        {
            var options = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                .AddItemsGrouped("North America",new[] { "Seattle", "Boston", "New York" })
                //.AddGroup(new[] { "Seattle", "Boston", "New York" }, "North America")
                ///.AddGroup(new[] { "Tokyo", "Singapore", "Shanghai" }, "Asia")
                .AddItem("South America (Any)")
                .AddItem("Europe (Any)")
                //.DisableItem("Boston")
                .AddItemTo(AdderScope.Disable,"Boston")
                .AddDefault("New York")
                //.DescriptionSelector(x => x)
                //.ShowGroupOnDescription("No group")
                .AppendGroupOnDescription()
                .Range(1, 3)
                .Run(_stopApp);

            if (options.IsAborted)
            {
                return;
            }
            if (options.Value.Any())
            {
                PromptPlus.WriteLine($"You picked {string.Join(", ", options.Value)}");
            }
            else
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
        }

        private void RunMultiSelectSample()
        {
            var options = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                .AddItems(new[] { "Seattle", "Boston", "New York", "Tokyo", "Singapore", "Shanghai" })
                .AddDefault("New York")
                .PageSize(3)
                .Run(_stopApp);

            if (options.IsAborted)
            {
                return;
            }
            if (options.Value.Any())
            {
                PromptPlus.WriteLine($"You picked {string.Join(", ", options.Value)}");
            }
            else
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
        }

        private void RunSelectEnumSample()
        {
            var envalue = PromptPlus.Select<MyEnum>("Select enum value")
                .Default(MyEnum.Bar)
                //.DisableItem(MyEnum.Baz)
                .Run(_stopApp);

            if (envalue.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You selected {envalue.Value}");
        }

        private void RunMultiSelectEnumSample()
        {
            var multvalue = PromptPlus.MultiSelect<MyEnum>("Select enum value")
                .AddDefault(MyEnum.Bar)
                //.DisableItem(MyEnum.Baz)
                .Run(_stopApp);

            if (multvalue.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You picked {string.Join(", ", multvalue.Value)}");
        }

        //private void RunPipeLineSample()
        //{
        //    _ = PromptPlus.Pipeline()
        //            .AddPipe(PromptPlus.Input("Your first name (empty = skip lastname)")
        //                    .Config(ctx =>
        //                    {
        //                        ctx
        //                        .AddExtraAction(StageControl.OnStartControl, (ctx, value) =>
        //                        {
        //                        })
        //                        .AddExtraAction(StageControl.OnFinishControl, (ctx, value) =>
        //                       {
        //                       });
        //                    })
        //                    .ToPipe(null, "First Name"))
        //            .AddPipe(PromptPlus.Input("Your last name")
        //                .PipeCondition((res, context) =>
        //                {
        //                    return !string.IsNullOrEmpty(((ResultPromptPlus<string>)res[0].ValuePipe).Value);
        //                })
        //                .ToPipe(null, "Last Name"))
        //            .AddPipe(PromptPlus.MaskEdit(MaskedType.DateOnly, "Your birth date")
        //                .ToPipe(null, "birth date"))
        //            .AddPipe(
        //                PromptPlus.WaitProcess("phase 1")
        //                .AddProcess(new SingleProcess((_stopApp) =>
        //                    {
        //                        _stopApp.WaitHandle.WaitOne(4000);
        //                        if (_stopApp.IsCancellationRequested)
        //                        {
        //                            return Task.FromResult<object>("canceled");
        //                        }
        //                        return Task.FromResult<object>("Done");
        //                    }))
        //                .ToPipe(null, "Update phase 1"))
        //            .AddPipe(PromptPlus.Progressbar("Processing Tasks ")
        //                .UpdateHandler(UpdateSampleHandlerAsync)
        //                .ToPipe(null, "Update phase 2"))
        //            .Run(_stopApp);
        //}

        private void RunFolderSample()
        {
            var folder = PromptPlus.Browser("Select/New folder")
                //.Filter(BrowserFilter.OnlyFolder)
                .OnlyFolders(true)
                .PageSize(5)
                .Default(Directory.GetDirectoryRoot(AppDomain.CurrentDomain.BaseDirectory))
                //.PromptCurrentPath(false)
                .ShowCurrentFolder(true)
                .Run(_stopApp);

            if (folder.IsAborted)
            {
                return;
            }
            //if (string.IsNullOrEmpty(folder.Value.SelectedValue))
            //{
            //    PromptPlus.WriteLine("You chose nothing!");
            //}
            //else
            //{
            //    var dirfound = folder.Value.NotFound ? "not found" : "found";
            //    PromptPlus.WriteLine($"You picked, {Path.Combine(folder.Value.PathValue, folder.Value.SelectedValue)} and {dirfound}");
            //}
        }

        private void RunFileSample()
        {
            var file = PromptPlus.Browser("Select/New file")
                .PageSize(5)
                //.AllowNotSelected(true)
                //.PrefixExtension(".cs")
                .Run(_stopApp);

            if (file.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(file.Value.Name))
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
            //else
            //{
            //    var filefound = file.Value.NotFound ? "not found" : "found";
            //    PromptPlus.WriteLine($"You picked, {Path.Combine(file.Value.PathValue, file.Value.SelectedValue)} and {filefound}");
            //}
        }
    }
}
