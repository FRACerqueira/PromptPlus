using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PromptPlus.Example.Models;
using PromptPlus.Options;
using PromptPlus.ValueObjects;

namespace PromptPlus.Example
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
                    Console.WriteLine(ex);
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            //Console.WriteLine("Attach process to debug..");
            //Console.ReadKey(false);

            var quit = false;

            PPlus.DefaultCulture = new CultureInfo("en-US");

            while (!_stopApp.IsCancellationRequested && !quit)
            {

                var type = PPlus.Select<ExampleType>("Choose prompt example", cancellationToken: _stopApp);
                if (type.IsAborted)
                {
                    continue;
                }

                //Console.Clear();

                switch (type.Value.Value)
                {
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
                    case ExampleType.MultiSelect:
                        RunMultiSelectSample();
                        break;
                    case ExampleType.SelectWithEnum:
                        RunSelectEnumSample();
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
                    case ExampleType.Quit:
                        quit = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (!_stopApp.IsCancellationRequested && !quit)
                {
                    //Console.ReadKey(true);
                    if (type.Value.Value != ExampleType.AnyKey)
                    {
                        PPlus.AnyKey(_stopApp);
                    }
                }
            }
            if (!quit)
            {
                Environment.ExitCode = -1;
            }
            _appLifetime.StopApplication();
        }

        private void RunSaveLoadSample()
        {
            PPlus.AbortAllPipesKeyPress = new HotKey(ConsoleKey.X, false, true, false);
            PPlus.AbortKeyPress = new HotKey(ConsoleKey.Escape, false, false, false);
            PPlus.TooltipKeyPress = new HotKey(ConsoleKey.F1, false, false, false);
            PPlus.ResumePipesKeyPress = new HotKey(ConsoleKey.F2, false, false, false);

            var filecfg = PPlus.SaveConfigToFile();
            PPlus.LoadConfigFromFile();
            Console.WriteLine($"PromptPlus file saved and readed. Location: {filecfg}");
        }

        private void RunChooseLanguageSample()
        {
            var envalue = PPlus.Select<LanguageOptions>("Select a language", defaultValue: LanguageOptions.English, cancellationToken: _stopApp);
            if (envalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You selected {envalue.Value.Value}");
            switch (envalue.Value.Value)
            {
                case LanguageOptions.English:
                    PPlus.DefaultCulture = new CultureInfo("en");
                    break;
                case LanguageOptions.PortugueseBrazil:
                    PPlus.DefaultCulture = new CultureInfo("pt-BR");
                    break;
                default:
                    break;
            }
        }

        private void RunMaskEditNumberSample(string numtype)
        {
            var maskqtdint = PPlus.MaskEdit(PPlus.MaskTypeGeneric, "Max integer pos.(empty = 5)", @"C[123456789]", "5", cancellationToken: _stopApp);
            if (maskqtdint.IsAborted)
            {
                return;
            }
            var maskqtddec = PPlus.MaskEdit(PPlus.MaskTypeGeneric, "Max decimal pos.(empty = 2)", @"C[0123]", "2", cancellationToken: _stopApp);
            if (maskqtddec.IsAborted)
            {
                return;
            }
            var masksignal = PPlus.SliderSwitche("Accept negative", true, cancellationToken: _stopApp);
            if (masksignal.IsAborted)
            {
                return;
            }
            var opccult = PPlus.Select("Select format language", new List<string> { "English", "Portuguese Brazil", "French" }, cancellationToken: _stopApp);
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
            ResultPPlus<ResultMasked> mask;
            if (numtype == "N")
            {
                mask = PPlus.MaskEdit(PPlus.MaskTypeNumber,
                    "Number",
                    qtdint,
                    qtddec,
                    null,
                    opccult.Value[0] == 'E' ? new CultureInfo("en-US") : opccult.Value[0] == 'P' ? new CultureInfo("pt-BR") : new CultureInfo("fr-FR"),
                    masksignal.Value ? MaskedSignal.Enabled : MaskedSignal.None, cancellationToken: _stopApp);
            }
            else
            {
                mask = PPlus.MaskEdit(PPlus.MaskTypeCurrency,
                    "Currency",
                    qtdint,
                    qtddec,
                    null,
                    opccult.Value[0] == 'E' ? new CultureInfo("en-US") : opccult.Value[0] == 'P' ? new CultureInfo("pt-BR") : new CultureInfo("fr-FR"),
                    masksignal.Value ? MaskedSignal.Enabled : MaskedSignal.None, cancellationToken: _stopApp);
            }

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
            }

        }

        private void RunMaskEditDateTimeSample()
        {
            var opcyear = PPlus.Select("Select format year", new List<string> { "Four digits", "Two digits" }, cancellationToken: _stopApp);
            if (opcyear.IsAborted)
            {
                return;
            }
            var opctime = PPlus.Select("Select format time", new List<string> { "1-Hour,minute,Second", "2-Hour,minute", "3-hour" }, cancellationToken: _stopApp);
            if (opctime.IsAborted)
            {
                return;
            }
            var masfill = PPlus.SliderSwitche("Fill with zeros", true, cancellationToken: _stopApp);
            if (masfill.IsAborted)
            {
                return;
            }
            var opccult = PPlus.Select("Select format language", new List<string> { "English", "Portuguese Brazil" }, cancellationToken: _stopApp);
            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PPlus.MaskEdit(PPlus.MaskTypeDateTime, "Date and Time",
                cultureinfo: opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"),
                fyear: opcyear.Value[0] == 'F' ? FormatYear.Y4 : FormatYear.Y2,
                ftime: opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH,
                fillzeros: masfill.Value,
                cancellationToken: _stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
            }

        }

        private void RunMaskEditTimeSample()
        {
            var opctime = PPlus.Select("Select format time", new List<string> { "1-Hour,minute,Second", "2-Hour,minute", "3-hour" }, cancellationToken: _stopApp);
            if (opctime.IsAborted)
            {
                return;
            }
            var masfill = PPlus.SliderSwitche("Fill with zeros", true, cancellationToken: _stopApp);
            if (masfill.IsAborted)
            {
                return;
            }
            var opccult = PPlus.Select("Select format language", new List<string> { "English", "Portuguese Brazil" }, cancellationToken: _stopApp);
            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PPlus.MaskEdit(PPlus.MaskTypeTimeOnly, "Time",
                cultureinfo: opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"),
                ftime: opctime.Value[0] == '1' ? FormatTime.HMS : opctime.Value[0] == '2' ? FormatTime.OnlyHM : FormatTime.OnlyH,
                fillzeros: masfill.Value,
                cancellationToken: _stopApp);


            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
            }
        }

        private void RunMaskEditDateSample()
        {
            var opcyear = PPlus.Select("Select format year", new List<string> { "Four digits", "Two digits" }, cancellationToken: _stopApp);
            if (opcyear.IsAborted)
            {
                return;
            }
            var masfill = PPlus.SliderSwitche("Fill with zeros", true, cancellationToken: _stopApp);
            if (masfill.IsAborted)
            {
                return;
            }
            var opccult = PPlus.Select("Select format language", new List<string> { "English", "Portuguese Brazil" }, cancellationToken: _stopApp);
            if (opccult.IsAborted)
            {
                return;
            }
            var mask = PPlus.MaskEdit(PPlus.MaskTypeDateOnly, "Date",
                cultureinfo: opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"),
                fyear: opcyear.Value[0] == 'F' ? FormatYear.Y4 : FormatYear.Y2,
                fillzeros: masfill.Value,
                cancellationToken: _stopApp);

            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
            }
        }

        private void RunMaskEditGenericSample()
        {
            var mask = PPlus.MaskEdit(PPlus.MaskTypeGeneric, "Inventory Number", @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}", cancellationToken: _stopApp);
            if (mask.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(mask.Value.Input))
            {
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
            }
        }

        private void RunAnyKeySample()
        {
            var key = PPlus.AnyKey(_stopApp);
            if (key.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Hello, key pressed");
        }

        private void RunKeyPressSample()
        {
            var key = PPlus.KeyPress("Press Ctrl-B to continue", 'B', ConsoleModifiers.Control, _stopApp);
            if (key.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Hello,  key Ctrl-B pressed");
        }

        private void RunInputSample()
        {
            var name = PPlus.Input<string>("What's your name?", "Peter Parker", validators: new[] { Validators.Required(), Validators.MinLength(3) }, cancellationToken: _stopApp);
            if (name.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Hello, {name.Value}!");
        }

        private void RunConfirmSample()
        {
            var opccult = PPlus.Select("Select format language", new List<string> { "English", "Portuguese Brazil" }, cancellationToken: _stopApp);
            if (opccult.IsAborted)
            {
                return;
            }
            if (opccult.Value[0] == 'E')
            {
                PPlus.DefaultCulture = new CultureInfo("en-US");
                var answer = PPlus.Confirm("Are you ready?", true, _stopApp);
                if (answer.IsAborted)
                {
                    return;
                }
                if (answer.Value)
                {
                    Console.WriteLine($"Sua resposta é Yes");
                }
                else
                {
                    Console.WriteLine($"Sua resposta é No");
                }
            }
            else
            {
                PPlus.DefaultCulture = new CultureInfo("pt-BR");
                var answer = PPlus.Confirm("Você esta pronto?", true, _stopApp);
                if (answer.IsAborted)
                {
                    return;
                }
                if (answer.Value)
                {
                    Console.WriteLine($"Sua resposta é Sim");
                }
                else
                {
                    Console.WriteLine($"Sua resposta é Não");
                }
            }
        }

        private void RunWaitSingleProcessSample()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            var progress = PPlus.WaitProcess("My Task", async () =>
                {
                    await Task.Delay(10000);
                    return "Done";
                }, cancellationToken: _stopApp);
            if (progress.IsAborted)
            {
                Console.WriteLine($"Your task aborted.");
            }
        }

        private void RunWaitManyProcessSample()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            var progress = PPlus.WaitProcess("My Tasks(3) Async", new List<SingleProcess<string>>
            {
                new SingleProcess<string>
                {
                     ProcessId = "Task1",
                     ProcessToRun = async () =>
                     {
                         await Task.Delay(10000);
                         return "Done";
                     }
                },
                new SingleProcess<string>
                {
                     ProcessId = "Task2",
                     ProcessToRun = async () =>
                     {
                         await Task.Delay(5000);
                         return "Done";
                     }
                },
                new SingleProcess<string>
                {
                     ProcessId = "Task3",
                     ProcessToRun = async () =>
                     {
                         await Task.Delay(7000);
                         return "Done";
                     }
                },
            }
            , cancellationToken: _stopApp);
            if (progress.IsAborted)
            {
                return;
            }
        }

        private void RunProgressbarSample()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            var progress = PPlus.Progressbar("Processing Tasks", UpdateSampleHandlerAsync, 0, cancellationToken: _stopApp);
            if (progress.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your result is: {progress.Value.Message}");
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
            var number = PPlus.NumberUpDown("Select a number", 5.5, 0, 10, 0.1, fracionalDig: 1, cancellationToken: _stopApp);
            if (number.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderNumberSample()
        {
            var number = PPlus.SliderNumber("Select a number", 5.5, 0, 10, 0.1, fracionalDig: 1, cancellationToken: _stopApp);
            if (number.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderSwitcheSample()
        {
            var slider = PPlus.SliderSwitche("Turn on/off", false, cancellationToken: _stopApp);
            if (slider.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {slider.Value}");
        }

        private void RunPasswordSample()
        {
            var pwd = PPlus.Password("Type new password", true, new[] { Validators.Required(), Validators.MinLength(8) }, _stopApp);
            if (pwd.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Password OK : {pwd.Value}");
        }

        private void RunListSample()
        {
            var lst = PPlus.List<string>("Please add item(s)", uppercase: true, cancellationToken: _stopApp);
            if (lst.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private void RunListMaskedSample()
        {
            var lst = PPlus.ListMasked<string>("Please add item(s)", @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}", uppercase: true, cancellationToken: _stopApp);
            if (lst.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private void RunSelectSample()
        {
            var city = PPlus.Select("Select your city", new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" }, pageSize: 3, cancellationToken: _stopApp);
            if (city.IsAborted)
            {
                return;
            }
            if (!string.IsNullOrEmpty(city.Value))
            {
                Console.WriteLine($"Hello, {city.Value}!");
            }
            else
            {
                Console.WriteLine("You chose nothing!");
            }
        }

        private void RunMultiSelectSample()
        {
            var options = PPlus.MultiSelect("Which cities would you like to visit?", new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" }, pageSize: 3, defaultValues: new[] { "Tokyo" }, minimum: 0, cancellationToken: _stopApp);
            if (options.IsAborted)
            {
                return;
            }
            if (options.Value.Any())
            {
                Console.WriteLine($"You picked {string.Join(", ", options.Value)}");
            }
            else
            {
                Console.WriteLine("You chose nothing!");
            }
        }

        private void RunSelectEnumSample()
        {
            var envalue = PPlus.Select<MyEnum>("Select enum value", defaultValue: MyEnum.Bar, cancellationToken: _stopApp);
            if (envalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You selected {envalue.Value.DisplayName}");
        }

        private void RunMultiSelectEnumSample()
        {
            var multvalue = PPlus.MultiSelect("Select enum value", defaultValues: new[] { MyEnum.Bar }, cancellationToken: _stopApp);
            if (multvalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", multvalue.Value.Select(x => x.DisplayName))}");
        }

        private void RunPipeLineSample()
        {
            var steps = new List<IFormPPlusBase>
            {
                PPlus.Pipe.Input<string>(new InputOptions { Message = "Your first name (empty = skip lastname)" })
                .Step("First Name"),

                PPlus.Pipe.Input<string>(new InputOptions { Message = "Your last name" })
                .Step("Last Name",(res,context) =>
                {
                    return !string.IsNullOrEmpty( ((ResultPPlus<string>)res[0].ValuePipe).Value);
                }),

                PPlus.Pipe.MaskEdit(PPlus.MaskTypeDateOnly, "Your birth date",cancellationToken: _stopApp)
                .Step("birth date"),

                PPlus.Pipe.Progressbar("Processing Tasks ",  UpdateSampleHandlerAsync, 30)
                .Step("Update")
            };
            _ = PPlus.Pipeline(steps, _stopApp);
        }

        private void RunFolderSample()
        {
            var folder = PPlus.Browser(BrowserFilter.OnlyFolder, "Select/New folder", cancellationToken: _stopApp, pageSize: 5, promptCurrentPath: false);
            if (folder.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(folder.Value.SelectedValue))
            {
                Console.WriteLine("You chose nothing!");
            }
            else
            {
                var dirfound = folder.Value.NotFound ? "not found" : "found";
                Console.WriteLine($"You picked, {Path.Combine(folder.Value.PathValue, folder.Value.SelectedValue)} and {dirfound}");
            }
        }

        private void RunFileSample()
        {
            var file = PPlus.Browser(BrowserFilter.None, "Select/New file", cancellationToken: _stopApp, pageSize: 10, allowNotSelected: true, prefixExtension: ".cs", supressHidden: false);
            if (file.IsAborted)
            {
                return;
            }
            if (string.IsNullOrEmpty(file.Value.SelectedValue))
            {
                Console.WriteLine("You chose nothing!");
            }
            else
            {
                var filefound = file.Value.NotFound ? "not found" : "found";
                Console.WriteLine($"You picked, {Path.Combine(file.Value.PathValue, file.Value.SelectedValue)} and {filefound}");
            }
        }
    }
}
