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
                    case ExampleType.ConsoleCmd:
                        RunCommandsSample();
                        break;
                    case ExampleType.Screen:
                        RunScrennSample();
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

            if (PromptPlus.IsAlternateScreen)
            {
                if (PromptPlus.IsRunningTerminal)
                {
                    PromptPlus.Screen()
                        .StatusBar()
                        .Stop();
                }
                else
                {
                    PromptPlus.Screen().Switch();
                }
            }

            if (!quit)
            {
                Environment.ExitCode = -1;
            }
            _appLifetime.StopApplication();
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
                    var line = PromptPlus.CursorTop;
                    PromptPlus.WriteLines(3);
                    PromptPlus.KeyPress().Run();
                }
            }
        }

        private void RunScrennSample()
        {
            var quit = false;
            while (!_stopApp.IsCancellationRequested && !quit)
            {
                PromptPlus.Clear();
                PromptPlus.WriteLine("Current Screen is ", (PromptPlus.IsAlternateScreen ? "Alternate" : "Principal").Yellow());
                PromptPlus.WriteLine();
                var opc = PromptPlus.Select<string>("Select Screen Sample");
                if (!PromptPlus.IsAlternateScreen)
                {
                    opc.AddItem("1 - Switch to alternate screen");
                    opc.AddItem("2 - Switch to alternate screen and show Status bar");
                }
                else
                {
                    opc.AddItem("1 - Switch to principal screen");
                    opc.AddItem("2 - Show/change values StatusBar");
                    if (PromptPlus.IsStatusBarRunning)
                    {
                        opc.AddItem("3 - Hide StatusBar");
                    }
                }
                var cmd = opc.AddItem("X - End Samples")
                    .Run();

                if (cmd.IsAborted)
                {
                    continue;
                }
                if (cmd.Value[0] == '1')
                {
                    PromptPlus.Screen()
                        .Switch();
                }
                else if (cmd.Value[0] == '2')
                {
                    StatusBarSample();
                    PromptPlus.Clear();
                }
                else if (cmd.Value[0] == '3')
                {
                    PromptPlus.Screen()
                        .StatusBar()
                        .Hide();
                }
                else if (cmd.Value[0] == 'X')
                {
                    quit = true;
                }
            }
            if (PromptPlus.IsAlternateScreen)
            {
                PromptPlus.Screen()
                    .Switch();
            }
        }


        private void StatusBarSample()
        {
            if (!PromptPlus.IsStatusBarRunning)
            {
                PromptPlus.Screen()
                    .StatusBar()
                    .Reset()
                    .AddTemplate("Sample1", ConsoleColor.White, ConsoleColor.Blue)
                        .AddText("SampleText")
                        .AddSeparator()
                        .AddColumn("col1", 30)
                        .AddSeparator()
                        .AddColumn("col2", 200, StatusBarColAlignment.Right)
                        .Build()
                    .AddTemplate("Sample2", ConsoleColor.White, ConsoleColor.Green)
                        .Build()
                    .Show();
            }
            else
            {
                PromptPlus.Screen()
                    .StatusBar()
                    .Refresh();
            }

            var quit = false;
            while (!_stopApp.IsCancellationRequested && !quit)
            {
                PromptPlus.Clear();
                PromptPlus.WriteLine("Hello ".Yellow(), "this is a ", "alternate screen ".Cyan(), "with ", "StatusBar!".Cyan());
                PromptPlus.WriteLine();
                PromptPlus.WriteLine("* Nix style applications often utilize an ", "alternate screen buffer, ".Cyan(),
                    "so that they can modify the entire contents of the buffer, ", "without".Cyan(), " affecting the application " +
                    "that started them.");
                PromptPlus.WriteLine();
                PromptPlus.WriteLine("The alternate buffer is ", "exactly the dimensions of the window, without any scrollback region.".Cyan(),
                    "For an example of this behavior, consider when vim is launched from bash.Vim uses the entirety of the screen to edit the file, " +
                    "then returning to bash leaves the original buffer unchanged.");

                PromptPlus.CursorPosition(0, 10);

                var c1 = PromptPlus.Input("Col1 value to Statubar with color blue")
                    .Run();

                if (c1.IsAborted)
                {
                    quit = true;
                    continue;
                }
                var c2 = PromptPlus.Input("Col2 value to Statubar with color blue")
                    .Run();

                if (c2.IsAborted)
                {
                    quit = true;
                    continue;
                }
                var c3 = PromptPlus.Input("value of Statubar with color Green")
                    .Run();

                if (c3.IsAborted)
                {
                    quit = true;
                    continue;
                }

                PromptPlus.Screen()
                    .StatusBar()
                    .WithTemplate("Sample1")
                        .UpdateColumn("col1", c1.Value)
                        .UpdateColumn("col2", c2.Value)
                    .WithTemplate("Sample2")
                        .UpdateColumn(null, c3.Value)
                    .Show();

                var opc = PromptPlus.Confirm("new values?")
                    .Default(false)
                    .Run();

                if (!opc.IsAborted && !opc.Value)
                {
                    quit = true;
                }
            }
        }

        private void RunColorTextSample()
        {
            PromptPlus.WriteLine();
            PromptPlus.WriteLine(
                "PromptPlus".Cyan(),
                ".",
                "WriteLine".DarkYellow(),
                "(",
                "\"Now set\"".Red(),
                ".",
                "Yellow().OnBlue()".DarkYellow(),
                ", ",
                "\" the \", ",
                "\"background\"",
                ".",
                "Underline()".DarkYellow(),
                ", \" \" , ",
                "\"color too!\"".Red(),
                ".",
                "Cyan().OnBlue()".DarkYellow(),
                ");");

            PromptPlus.WriteLine();
            PromptPlus.WriteLine("Output:".Yellow());
            PromptPlus.WriteLine();
            PromptPlus.WriteLine("Now set".Yellow().OnBlue(), " the ", "background".Underline(), " ", "color too!".Cyan().OnBlue());
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

        private void RunMultiSelectGroupSample()
        {
            var options = PromptPlus.MultiSelect<string>("Which cities would you like to visit?")
                .AddGroup(new[] { "Seattle", "Boston", "New York" }, "North America")
                .AddGroup(new[] { "Tokyo", "Singapore", "Shanghai" }, "Asia")
                .AddItem("South America (Any)")
                .AddItem("Europe (Any)")
                .DisableItem("Boston")
                .AddDefault("New York")
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
                .DisableItem(MyEnum.Baz)
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
                .DisableItem(MyEnum.Baz)
                .Run(_stopApp);

            if (multvalue.IsAborted)
            {
                return;
            }
            Console.WriteLine($"You picked {string.Join(", ", multvalue.Value)}");
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
                            }
                        })
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
