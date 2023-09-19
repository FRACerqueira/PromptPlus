using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PPlus;

namespace PromptPlusTemplate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                //add services
                services.AddSingleton<MainAsync>();
            })
            .ConfigureLogging((context, cfg) =>
            {
                cfg.ClearProviders();
                //add logprovider
            })
            .Build();

            using IServiceScope scope = host.Services.CreateScope();

            var main = scope.ServiceProvider.GetRequiredService<MainAsync>();
            var lt = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();

            //detect ctrl+c and trigger event to stop app (Graceful Shutdown)
            Console.CancelKeyPress += (sender, evtarg) =>
            {
                lt.StopApplication();
                evtarg.Cancel = true;
            };

            //initialize PromptPlus
            InitConfigPromptPlus();

            //start Application
            int exitcode = -2;
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                exitcode = await main.Run();
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.InnerExceptions)
                {
                    //do anything
                }
            }
            catch (Exception ex)
            {
                //do anything
            }
#pragma warning restore CS0168 // Variable is declared but never used

            //return to Environment exitcode (0: Normal termination, otherwhise abnormal termination)
            Environment.Exit(exitcode);
        }

        private static void InitConfigPromptPlus()
        {
            //*******************************************************************
            //Note: Sample(partial) Custom global set Environment for PromptPlus. 
            //*******************************************************************

            //Note: Disable key [ESC] to abort exec. for all controls. Default value : true
            PromptPlus.Config.EnabledAbortKey = false;

            //Note: Hide tooltips at statup for all controls. Default value : true
            PromptPlus.Config.ShowTooltip = false;

            //Note: DisableToggle Tooltip. Default value : false 
            PromptPlus.Config.DisableToggleTooltip = true;

            //Note: Show pagination only if exists. Default value : false 
            PromptPlus.Config.ShowOnlyExistingPagination = true;
        }
    }
}