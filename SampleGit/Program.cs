using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PromptPlusControls;
using PromptPlusControls.Options;
using PromptPlusControls.ValueObjects;

namespace SampleGit
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

        public MainProgram(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _stopApp = _appLifetime.ApplicationStopping;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run((Action)(() =>
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                while (!_stopApp.IsCancellationRequested)
                {
                    Console.Clear();
                    var lng = PromptPlus.Select("Select language", new List<string> { "English", "Portuguese" });
                    if (lng.Value[0] == 'E')
                    {
                        PromptPlus.DefaultCulture = new CultureInfo("en-US");
                    }
                    else
                    {
                        PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
                    }
                    var steps = new List<IFormPlusBase>
                    {
                        PromptPlus.Pipe.Input<string>(new InputOptions { Message = "Your first name (empty = skip lastname)" })
                            .Step("First Name"),
                        PromptPlus.Pipe.Input<string>(new InputOptions { Message = "Your last name" })
                            .Step("Last Name", (ResultPipe[] res, object context) =>
                        {
                            return !string.IsNullOrEmpty( ((ResultPromptPlus<string>)res[0].ValuePipe).Value);
                        }),
                        PromptPlus.Pipe.MaskEdit(PromptPlus.MaskTypeDateOnly, "Your birth date")
                             .Step("birth date"),
                        PromptPlus.Pipe.Progressbar("Processing Tasks ", UpdateSampleHandlerAsync,interationId:80)
                             .Step("Update")
                    };
                    _ = PromptPlus.Pipeline(steps);
                    Console.Write("Done");
                    Console.ReadKey();
                }
            }), stoppingToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task<ProgressBarInfo> UpdateSampleHandlerAsync(ProgressBarInfo status, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            var aux = (int)status.InterationId + 1;
            var endupdate = true;
            if (aux < 100)
            {
                endupdate = false;
            }
            return new ProgressBarInfo(aux, endupdate, $"Interation {aux}", aux);
        }
    }
}
