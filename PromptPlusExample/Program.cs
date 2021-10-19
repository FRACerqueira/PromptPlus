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

            PromptPlus.DefaultCulture = new CultureInfo("en-US");

            Console.ForegroundColor = PromptPlus.ColorSchema.ForeColorSchema;
            Console.BackgroundColor = PromptPlus.ColorSchema.BackColorSchema;
            Console.Clear();
            //Console.WriteLine("Attach process to debug..");
            //Console.ReadKey(false);

            var quit = false;


            while (!_stopApp.IsCancellationRequested && !quit)
            {

                var type = PromptPlus.Select<ExampleType>("Choose prompt example")
                    .Run(_stopApp);

                if (type.IsAborted)
                {
                    continue;
                }

                //Console.Clear();

                switch (type.Value)
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

        private void RunSaveLoadSample()
        {
            PromptPlus.AbortAllPipesKeyPress = new HotKey(ConsoleKey.X, false, true, false);
            PromptPlus.AbortKeyPress = new HotKey(ConsoleKey.Escape, false, false, false);
            PromptPlus.TooltipKeyPress = new HotKey(ConsoleKey.F1, false, false, false);
            PromptPlus.ResumePipesKeyPress = new HotKey(ConsoleKey.F2, false, false, false);

            var filecfg = PromptPlus.SaveConfigToFile();
            PromptPlus.LoadConfigFromFile();
            Console.WriteLine($"PromptPlus file saved and readed. Location: {filecfg}");
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
            Console.WriteLine($"You selected {envalue.Value}");
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
            var masksignal = PromptPlus.SliderSwitche("Accept negative")
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
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
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
            var masfill = PromptPlus.SliderSwitche("Fill with zeros")
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
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
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
            var masfill = PromptPlus.SliderSwitche("Fill with zeros")
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
                Console.WriteLine($"your input was empty!");
            }
            else
            {
                Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
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
            var masfill = PromptPlus.SliderSwitche("Fill with zeros")
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
            var mask = PromptPlus.MaskEdit(MaskedType.DateOnly, "Date")
                .Culture(opccult.Value[0] == 'E' ? new CultureInfo("en-US") : new CultureInfo("pt-BR"))
                .FormatYear(opcyear.Value[0] == 'F' ? FormatYear.Y4 : FormatYear.Y2)
                .FillZeros(masfill.Value)
                .Run(_stopApp);

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
            var mask = PromptPlus.MaskEdit(MaskedType.Generic, "Inventory Number")
                .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .Run(_stopApp);

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
            var key = PromptPlus.KeyPress()
                    .Run(_stopApp);
            if (key.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Hello, key pressed");
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
            Console.WriteLine($"Hello,  key Ctrl-B pressed");
        }

        private void RunImportValidatorsSample()
        {
            var inst = new MylCass();

            Console.WriteLine("Imported Validators of Myclass, property MyInput:");
            Console.WriteLine("private class MylCass \n{\n   [Required(ErrorMessage = \"{0} is required!\")] \n   [MinLength(3, ErrorMessage = \"Min. Length = 3.\")] \n   [MaxLength(5, ErrorMessage = \"Max. Length = 5.\")] \n   [Display(Prompt = \"My Input\")]\n   public string MyInput { get; set; }\n}");
            var name = PromptPlus.Input("Input Value for MyInput")
                .Addvalidators(inst.ImportValidators(x => x.MyInput))
                .Run(_stopApp);

            if (name.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your input: {name.Value}!");
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
                .Addvalidator(PromptValidators.Required())
                .Addvalidator(PromptValidators.MinLength(3))
                .Run(_stopApp);
            if (name.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Hello, {name.Value}!");
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
                    Console.WriteLine($"Sua resposta é Yes");
                }
                else
                {
                    Console.WriteLine($"Sua resposta é No");
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

            var process = PromptPlus.WaitProcess("phase 1")
                .AddProcess(new SingleProcess
                {
                    ProcessToRun = (_stopApp) =>
                    {
                        _stopApp.WaitHandle.WaitOne(4000);
                        if (_stopApp.IsCancellationRequested)
                        {
                            return Task.FromResult<object>("canceled");
                        }
                        return Task.FromResult<object>("Done");
                    },
                })
                .Run(_stopApp);

            var aux = process.Value.First();

            Console.WriteLine($"Result task ({aux.ProcessId}) : {aux.TextResult}. Property IsCanceled = {aux.IsCanceled}");
        }

        private void RunWaitManyProcessSample()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            var Process = PromptPlus.WaitProcess("My Tasks(3) Async")
                .AddProcess(new SingleProcess
                {
                    ProcessId = "Task1",
                    ProcessToRun = (_stopApp) =>
                    {
                        _stopApp.WaitHandle.WaitOne(10000);
                        if (_stopApp.IsCancellationRequested)
                        {
                            return Task.FromResult<object>("canceled");
                        }
                        return Task.FromResult<object>("Done");
                    }
                })
                .AddProcess(new SingleProcess
                {
                    ProcessId = "Task2",
                    ProcessToRun = (_stopApp) =>
                    {
                        _stopApp.WaitHandle.WaitOne(5000);
                        if (_stopApp.IsCancellationRequested)
                        {
                            return Task.FromResult<object>(-1);
                        }
                        return Task.FromResult<object>(1);
                    }
                })
                .AddProcess(new SingleProcess
                {
                    ProcessId = "Task3",
                    ProcessToRun = (_stopApp) =>
                    {
                        _stopApp.WaitHandle.WaitOne(7000);
                        if (_stopApp.IsCancellationRequested)
                        {
                            return Task.FromResult<object>("Canceled");
                        }
                        return Task.FromResult<object>("Done");
                    }
                })
                .Run(_stopApp);

            foreach (var item in Process.Value)
            {
                Console.WriteLine($"Result tasks ({item.ProcessId}) : {item.ValueProcess}");
            }
        }

        private void RunProgressbarSample()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            var progress = PromptPlus.Progressbar("Processing Tasks")
                .UpdateHandler(UpdateSampleHandlerAsync)
                .Run(_stopApp);

            if (progress.IsAborted)
            {
                Console.WriteLine($"Your result is: {progress.Value.Message} Canceled!");
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
            var number = PromptPlus.NumberUpDown("Select a number")
                .Default(5.5)
                .Ranger(0, 10)
                .Step(0.1)
                .FracionalDig(1)
                .Run(_stopApp);

            if (number.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderNumberSample()
        {
            var number = PromptPlus.SliderNumber("Select a number")
                .Default(5.5)
                .Ranger(0, 10)
                .Step(0.1)
                .FracionalDig(1)
                .Run(_stopApp);

            if (number.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {number.Value}");
        }

        private void RunSliderSwitcheSample()
        {
            var slider = PromptPlus.SliderSwitche("Turn on/off")
                .Run(_stopApp);

            if (slider.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Your answer is: {slider.Value}");
        }

        private void RunPasswordSample()
        {
            var pwd = PromptPlus.Input("Type new password")
                .IsPassword(true)
                .Addvalidator(PromptValidators.Required())
                .Addvalidator(PromptValidators.MinLength(8))
                .Run(_stopApp);

            if (pwd.IsAborted)
            {
                return;
            }
            Console.WriteLine($"Password OK : {pwd.Value}");
        }

        private void RunListSample()
        {
            var lst = PromptPlus.List<string>("Please add item(s)")
                .PageSize(3)
                .UpperCase(true)
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
        }

        private void RunListMaskedSample()
        {
            var lst = PromptPlus.ListMasked("Please add item(s)")
                .MaskType(MaskedType.Generic, @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
                .UpperCase(true)
                .Run(_stopApp);

            if (lst.IsAborted)
            {
                return;
            }
            foreach (var item in lst.Value)
            {
                Console.WriteLine($"You picked {item.ObjectValue}");

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
                Console.WriteLine($"Hello, {city.Value}!");
            }
            else
            {
                Console.WriteLine("You chose nothing!");
            }
        }

        private void RunMultiSelectSample()
        {
            var options = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                .AddItems(new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" })
                .AddDefault("Tokyo")
                .PageSize(3)
                .Run(_stopApp);

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
            var envalue = PromptPlus.Select<MyEnum>("Select enum value")
                .Default(MyEnum.Bar)
                .Run(_stopApp);

            if (envalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You selected {envalue.Value}");
        }

        private void RunMultiSelectEnumSample()
        {
            var multvalue = PromptPlus.MultiSelect<MyEnum>("Select enum value")
                .AddDefault(MyEnum.Bar)
                .Run(_stopApp);

            if (multvalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", multvalue.Value)}");
        }

        private void RunPipeLineSample()
        {
            var steps = new List<IFormPlusBase>
            {
                PromptPlus.Input("Your first name (empty = skip lastname)")
                .AddPipe(null,"First Name"),

                PromptPlus.Input("Your last name")
                .Condition((res,context) =>
                {
                    return !string.IsNullOrEmpty( ((ResultPromptPlus<string>)res[0].ValuePipe).Value);
                })
                .AddPipe(null,  "Last Name"),

                PromptPlus.MaskEdit(MaskedType.DateOnly,"Your birth date")
                    .AddPipe(null,"birth date"),

                PromptPlus.WaitProcess("phase 1")
                    .AddProcess(new SingleProcess{ ProcessToRun = (_stopApp) =>
                            {
                                _stopApp.WaitHandle.WaitOne(4000);
                                if (_stopApp.IsCancellationRequested)
                                {
                                    return Task.FromResult<object>("canceled");
                                }
                                return Task.FromResult<object>("Done");
                            } })
                    .AddPipe(null,"Update phase 1"),

                PromptPlus.Progressbar("Processing Tasks ")
                    .UpdateHandler(UpdateSampleHandlerAsync)
                    .AddPipe(null,"Update phase 2")

            };
            _ = PromptPlus.Pipeline(steps, _stopApp);
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
            var file = PromptPlus.Browser("Select/New file")
                .PageSize(10)
                .AllowNotSelected(true)
                .PrefixExtension(".cs")
                .Run(_stopApp);

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
