using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PromptPlusControls;
using PromptPlusControls.FIGlet;
using PromptPlusControls.ValueObjects;

using PromptPlusExample.Models;

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
            Console.OutputEncoding = Encoding.UTF8;

            PromptPlus.DefaultCulture = new CultureInfo("en-US");
            PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);
            PromptPlus.DefaultCulture = new CultureInfo("en-US");
            PromptPlus.Clear();

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
                        RunSaveLoadSample();
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
                    case ExampleType.Confirm:
                        RunConfirmSample();
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
                        RunProgressbarSample();
                        break;
                    case ExampleType.WaitSingleProcess:
                        RunWaitSingleProcessSample();
                        break;
                    case ExampleType.WaitManyProcess:
                        RunWaitManyProcessSample();
                        break;
                    case ExampleType.PipeLine:
                        RunPipeLineSample();
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
                    //Console.ReadKey(true);
                    if (type.Value != ExampleType.AnyKey)
                    {
                        PromptPlus.KeyPress()
                            .Run(_stopApp);
                    }
                }
            }

            if (!quit)
            {
                Environment.ExitCode = -1;
            }
            _appLifetime.StopApplication();
        }

        private void RunAutoCompleteSample()
        {
            var macth = PromptPlus.SliderSwitch("Accept Without Match sugestions")
                .Run(_stopApp);
            if (macth.IsAborted)
            {
                return;
            }
            var ctrlinput = PromptPlus.AutoComplete("Input value", "Sample autocomplete control")
                .AddValidator(PromptValidators.Required())
                .AddValidator(PromptValidators.MinLength(3))
                .CompletionInterval(1000)
                .CompletionMaxCount(10)
                .ValidateOnDemand()
                .PageSize(5)
                .CompletionAsyncService(MYServiceCompleteAsync);
            if (macth.Value)
            {
                ctrlinput.AcceptWithoutMatch();
            }
            var input = ctrlinput.Run(_stopApp);
            if (input.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Result : [cyan]{input.Value}[/cyan]!");
        }

        private async Task<string[]> MYServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
        {
            if (count == 0)
            {
                count = 10;
            }
            var random = new Random();
            var items = new List<string>(count);
            for (var i = 0; i < count; i++)
            {
                var c1 = (char)random.Next(65, 90);
                var c2 = (char)random.Next(97, 122);
                var c3 = (char)random.Next(97, 122);

                items.Add(prefixText + c1 + c2 + c3);
            }
            return await Task.FromResult(items.ToArray());
        }


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
                    .AutoSelectIfOne()
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
                    PromptPlus.CursorPosition(0, line);
                    PromptPlus.KeyPress().Run();
                }
                if (opc.Value[0] == '3')
                {
                    PromptPlus.WriteLine("LINE1 AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    PromptPlus.WriteLine("after pressing the key, over LINE 1 rest of line will be erased with backgroundcolor red");
                    PromptPlus.KeyPress().Run();
                    var line = PromptPlus.CursorTop;
                    PromptPlus.CursorPosition(5, line - 2);
                    PromptPlus.ClearRestOfLine(ConsoleColor.Red);
                    PromptPlus.CursorPosition(0, line);
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
            PromptPlus.WriteLineSkipColors("PromptPlus.WriteLine(\"[cyan]This[/cyan] is a [white:blue]simples[/white:blue] line with [red!u]color[/red!u].\")");
            PromptPlus.WriteLine("PromptPlus.WriteLine(\"Output2:\".White().OnBlue().Underline())");
            PromptPlus.WriteLineSkipColors("PromptPlus.WriteLine(\"[cyan]This[/cyan] is another [white:blue]simples[/white:blue] line using [red!u]Mask[/red!u].\".Mask(ConsoleColor.DarkRed))");
            PromptPlus.WriteLine();
            PromptPlus.WriteLine("Output1:", ConsoleColor.White, ConsoleColor.Blue, true);
            PromptPlus.WriteLine("[cyan]This[/cyan] is a [white:blue]simples[/white:blue] line with [red!u]color[/red!u].");
            PromptPlus.WriteLine("Output2:".White().OnBlue().Underline());
            PromptPlus.WriteLine("[cyan]This[/cyan] is another [white:blue]simples[/white:blue] line using [red!u]Mask[/red!u].".Mask(ConsoleColor.DarkRed));
            PromptPlus.WriteLine();
        }

        private void RunBannerSample()
        {
            var colorsel = PromptPlus.Select<ConsoleColor>("Select a color")
                .HideItem(PromptPlus.BackgroundColor)
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

        private void RunSaveLoadSample()
        {
            PromptPlus.AbortAllPipesKeyPress = new HotKey(ConsoleKey.X, false, true, false);
            PromptPlus.AbortKeyPress = new HotKey(ConsoleKey.Escape, false, false, false);
            PromptPlus.TooltipKeyPress = new HotKey(ConsoleKey.F1, false, false, false);
            PromptPlus.ResumePipesKeyPress = new HotKey(ConsoleKey.F2, false, false, false);

            var filecfg = PromptPlus.SaveConfigToFile();
            PromptPlus.LoadConfigFromFile();
            PromptPlus.WriteLine($"PromptPlus file [cyan]saved and readed[/cyan]. Location: {filecfg}");
        }

        private void RunChooseLanguageSample()
        {
            var envalue = PromptPlus.Select<LanguageOptions>("Select a language")
                .Default(LanguageOptions.English)
                .Run(_stopApp);

            if (envalue.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You selected [cyan]{envalue.Value}[/cyan]");
            switch (envalue.Value)
            {
                case LanguageOptions.English:
                    PromptPlus.DefaultCulture = new CultureInfo("en");
                    break;
                case LanguageOptions.PortugueseBrazil:
                    PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
                    break;
                default:
                    break;
            }
        }

        private void RunMaskEditNumberSample(string numtype)
        {
            var maskqtdint = PromptPlus.MaskEdit(MaskedType.Generic, "Max integer pos.(empty = 5)")
                    .Mask("C[0123456789]")
                    .Default("5")
                    .Run(_stopApp);
            if (maskqtdint.IsAborted)
            {
                return;
            }
            var maskqtddec = PromptPlus.MaskEdit(MaskedType.Generic, "Max decimal pos.(empty = 2)")
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
            if (maskqtdint.Value.ObjectValue.ToString().Length > 0)
            {
                qtdint = int.Parse(maskqtdint.Value.ObjectValue.ToString());
            }
            var qtddec = 2;
            if (maskqtddec.Value.ObjectValue.ToString().Length > 0)
            {
                qtddec = int.Parse(maskqtddec.Value.ObjectValue.ToString());
            }
            var mask = PromptPlus.MaskEdit(numtype == "N" ? MaskedType.Number : MaskedType.Currency, "Number")
                    .AmmoutPositions(qtdint, qtddec)
                    .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : opccult.Value[0] == 'P' ? new CultureInfo("pt-BR") : new CultureInfo("fr-FR"))
                    .AcceptSignal(masksignal.Value)
                    .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.ObjectValue}[/cyan]!");
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
            var mask = PromptPlus.MaskEdit(MaskedType.DateTime, "Date and Time")
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatYear(opcyear.Value[0] == 'F' ? FormatYear.Y4 : FormatYear.Y2)
                .FormatTime(opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH)
                .FillZeros(masfill.Value)
                .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.ObjectValue}[/cyan]!");
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
            var mask = PromptPlus.MaskEdit(MaskedType.TimeOnly, "Time")
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatTime(opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH)
                .FillZeros(masfill.Value)
                .Run(_stopApp);


            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.ObjectValue}[/cyan]!");
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
            var mask = PromptPlus.MaskEdit(MaskedType.DateOnly, "Date")
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatYear(opcyear.Value[0] == 'F' ? FormatYear.Y4 : FormatYear.Y2)
                .FillZeros(masfill.Value)
                .ShowDayWeek(week.Value ? FormatWeek.Short : FormatWeek.None)
                .ValidateOnDemand()
                .Run(_stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                PromptPlus.WriteLine($"your input was empty!");
            }
            else
            {
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.ObjectValue}[/cyan]!");
            }
        }

        private void RunMaskEditGenericSample()
        {
            var mask = PromptPlus.MaskEdit(MaskedType.Generic, "Inventory Number")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
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
                PromptPlus.WriteLine($"your input was [cyan]{mask.Value.ObjectValue}[/cyan]!");
            }
        }

        private void RunAnyKeySample()
        {
            var key = PromptPlus.KeyPress()
                    .Run(_stopApp);
            if (key.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, key [cyan]pressed[/cyan]");
        }

        private void RunKeyPressSample()
        {
            var key = PromptPlus.KeyPress('B', ConsoleModifiers.Control)
                .Prompt("Press Ctrl-B to continue")
                .Run(_stopApp);
            if (key.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, key [cyan]Ctrl-B pressed[/cyan]");
        }

        private void RunImportValidatorsSample()
        {
            var inst = new MylCass();

            PromptPlus.WriteLine("Imported Validators of Myclass, property MyInput:");
            PromptPlus.WriteLine("private class MylCass \n{\n   [Required(ErrorMessage = \"{0} is required!\")] \n   [MinLength(3, ErrorMessage = \"Min. Length = 3.\")] \n   [MaxLength(5, ErrorMessage = \"Max. Length = 5.\")] \n   [Display(Prompt = \"My Input\")]\n   public string MyInput { get; set; }\n}");
            var name = PromptPlus.Input("Input Value for MyInput")
                .AddValidators(inst.ImportValidators(x => x.MyInput))
                .Run(_stopApp);

            if (name.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Your input: [cyan]{name.Value}[/cyan]!");
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
                .AddValidators(PromptValidators.Required())
                .AddValidators(PromptValidators.MinLength(3))
                .Run(_stopApp);
            if (name.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Hello, [cyan]{name.Value}[/cyan]!");
        }

        private void RunConfirmSample()
        {
            var opccult = PromptPlus.Select<string>("Select format language")
                .AddItems(new List<string> { "English", "Portuguese Brazil" })
                .Run(_stopApp);

            if (opccult.IsAborted)
            {
                return;
            }
            if (opccult.Value[0] == 'E')
            {
                PromptPlus.DefaultCulture = new CultureInfo("en-US");
                var answer = PromptPlus.Confirm("Are you ready?")
                    .Default(true)
                    .Run(_stopApp);
                if (answer.IsAborted)
                {
                    return;
                }
                if (answer.Value)
                {
                    PromptPlus.WriteLine($"Sua resposta é [cyan]Yes[/cyan]");
                }
                else
                {
                    PromptPlus.WriteLine($"Sua resposta é [cyan]No[/cyan]");
                }
            }
            else
            {
                PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
                var answer = PromptPlus.Confirm("Você esta pronto?")
                    .Default(true)
                    .Run(_stopApp);

                if (answer.IsAborted)
                {
                    return;
                }
                if (answer.Value)
                {
                    PromptPlus.WriteLine($"Sua resposta é [cyan]Sim[/cyan]");
                }
                else
                {
                    PromptPlus.WriteLine($"Sua resposta é [cyan]Não[/cyan]");
                }
            }
        }

        private void RunWaitSingleProcessSample()
        {
            var process = PromptPlus.WaitProcess("phase 1")
                 .AddProcess(new SingleProcess((_stopApp) =>
                     {
                         _stopApp.WaitHandle.WaitOne(4000);
                         if (_stopApp.IsCancellationRequested)
                         {
                             return Task.FromResult<object>("canceled");
                         }
                         return Task.FromResult<object>("Done");
                     }))
                 .Run(_stopApp);

            var aux = process.Value.First();

            PromptPlus.WriteLine($"Result task ({aux.ProcessId}) : {aux.TextResult}. Property IsCanceled = {aux.IsCanceled}");
        }

        private void RunWaitManyProcessSample()
        {
            var Process = PromptPlus.WaitProcess("My Tasks(3) Async")
                 .AddProcess(new SingleProcess((_stopApp) =>
                     {
                         _stopApp.WaitHandle.WaitOne(10000);
                         if (_stopApp.IsCancellationRequested)
                         {
                             return Task.FromResult<object>("canceled");
                         }
                         return Task.FromResult<object>("Done");
                     }, "Task1"))
                 .AddProcess(new SingleProcess((_stopApp) =>
                     {
                         _stopApp.WaitHandle.WaitOne(5000);
                         if (_stopApp.IsCancellationRequested)
                         {
                             return Task.FromResult<object>(-1);
                         }
                         return Task.FromResult<object>(1);
                     }, "Task2"))
                 .AddProcess(new SingleProcess((_stopApp) =>
                     {
                         _stopApp.WaitHandle.WaitOne(7000);
                         if (_stopApp.IsCancellationRequested)
                         {
                             return Task.FromResult<object>("Canceled");
                         }
                         return Task.FromResult<object>("Done");
                     }, "Task3"))
                 .Run(_stopApp);

            foreach (var item in Process.Value)
            {
                PromptPlus.WriteLine($"Result tasks ({item.ProcessId}) : {item.ValueProcess}");
            }
        }

        private void RunProgressbarSample()
        {
            var progress = PromptPlus.Progressbar("Processing Tasks","My Process")
                .UpdateHandler(UpdateSampleHandlerAsync)
                .Run(_stopApp);

            if (progress.IsAborted)
            {
                PromptPlus.WriteLine($"Your result is: {progress.Value.Message} Canceled!");
                return;
            }
            PromptPlus.WriteLine($"Your result is: {progress.Value.Message}");
        }

        private async Task<ProgressBarInfo> UpdateSampleHandlerAsync(ProgressBarInfo status, CancellationToken cancellationToken)
        {
            await Task.Delay(10);
            var aux = (int)status.InterationId + 1;
            var endupdate = true;
            if (aux < 100)
            {
                endupdate = false;
            }
            return new ProgressBarInfo(aux, endupdate, $"Interation {aux}", aux);
        }

        private void RunNumberUpDownSample()
        {
            var number = PromptPlus.SliderNumber(SliderNumberType.UpDown, "Select a number")
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
            var number = PromptPlus.SliderNumber(SliderNumberType.LeftRight, "Select a number")
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

        private void RunPasswordSample()
        {
            var pwd = PromptPlus.Input("Type new password")
                .IsPassword(true)
                .AddValidators(PromptValidators.Required())
                .AddValidators(PromptValidators.MinLength(8))
                .ValidateOnDemand()
                .Run(_stopApp);

            if (pwd.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"Password OK : {pwd.Value}");
        }

        private void RunListSample()
        {
            var lst = PromptPlus.List<string>("Please add item(s)")
                .PageSize(3)
                .UpperCase(true)
                .AddValidator(PromptValidators.MinLength(3))
                .ValidateOnDemand()
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private void RunListMaskedSample()
        {
            var lst = PromptPlus.ListMasked("Please add item(s)")
                .MaskType(MaskedType.Generic, @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .UpperCase(true)
                .AddValidator(PromptValidators.MinLength(6))
                .ValidateOnDemand()
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            foreach (var item in lst.Value)
            {
                PromptPlus.WriteLine($"You picked {item.ObjectValue}");

            }
        }

        private void RunSelectWithAutoSelectSample()
        {
            var city = PromptPlus.Select<string>("Select your city")
                .AddItems(new[] { "1 - Seattle", "2 - London", "3 - Tokyo", "4 - New York", "5 - Singapore", "6 - Shanghai" })
                .PageSize(3)
                .AutoSelectIfOne()
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
                .AddGroup(new[] { "Seattle", "Boston", "New York" }, "North America")
                .AddGroup(new[] { "Tokyo", "Singapore", "Shanghai" }, "Asia")
                .AddItem("South America (Any)")
                .AddItem("Europe (Any)")
                .DisableItem("Boston")
                .AddDefault("New York")
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
                .DisableItem(MyEnum.Baz)
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
                .DisableItem(MyEnum.Baz)
                .Run(_stopApp);

            if (multvalue.IsAborted)
            {
                return;
            }
            PromptPlus.WriteLine($"You picked {string.Join(", ", multvalue.Value)}");
        }

        private void RunPipeLineSample()
        {
            _ = PromptPlus.Pipeline()
                    .AddPipe(PromptPlus.Input("Your first name (empty = skip lastname)")
                            .ToPipe(null, "First Name"))
                    .AddPipe(PromptPlus.Input("Your last name")
                        .PipeCondition((res, context) =>
                        {
                            return !string.IsNullOrEmpty(((ResultPromptPlus<string>)res[0].ValuePipe).Value);
                        })
                        .ToPipe(null, "Last Name"))
                    .AddPipe(PromptPlus.MaskEdit(MaskedType.DateOnly, "Your birth date")
                        .ToPipe(null, "birth date"))
                    .AddPipe(
                        PromptPlus.WaitProcess("phase 1")
                        .AddProcess(new SingleProcess((_stopApp) =>
                            {
                                _stopApp.WaitHandle.WaitOne(4000);
                                if (_stopApp.IsCancellationRequested)
                                {
                                    return Task.FromResult<object>("canceled");
                                }
                                return Task.FromResult<object>("Done");
                            }))
                        .ToPipe(null, "Update phase 1"))
                    .AddPipe(PromptPlus.Progressbar("Processing Tasks ")
                        .UpdateHandler(UpdateSampleHandlerAsync)
                        .ToPipe(null, "Update phase 2"))
                    .Run(_stopApp);
        }

        private void RunFolderSample()
        {
            var folder = PromptPlus.Browser("Select/New folder")
                .Filter(BrowserFilter.OnlyFolder)
                .PageSize(5)
                .PromptCurrentPath(false)
                .Run(_stopApp);

            if (folder.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(folder.Value.SelectedValue))
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
            else
            {
                var dirfound = folder.Value.NotFound ? "not found" : "found";
                PromptPlus.WriteLine($"You picked, {Path.Combine(folder.Value.PathValue, folder.Value.SelectedValue)} and {dirfound}");
            }
        }

        private void RunFileSample()
        {
            var file = PromptPlus.Browser("Select/New file")
                .PageSize(5)
                .AllowNotSelected(true)
                .PrefixExtension(".cs")
                .Run(_stopApp);

            if (file.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(file.Value.SelectedValue))
            {
                PromptPlus.WriteLine("You chose nothing!");
            }
            else
            {
                var filefound = file.Value.NotFound ? "not found" : "found";
                PromptPlus.WriteLine($"You picked, {Path.Combine(file.Value.PathValue, file.Value.SelectedValue)} and {filefound}");
            }
        }
    }
}
