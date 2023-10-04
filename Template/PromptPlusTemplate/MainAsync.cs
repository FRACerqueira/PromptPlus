using Microsoft.Extensions.Hosting;
using PPlus;
using PPlus.Controls;

namespace PromptPlusTemplate
{
    internal class MainAsync
    {
        private readonly CancellationToken _cancellationToken;
        public MainAsync(IHostApplicationLifetime hostApplicationLifetime)
        {
            //set token to abort promptplus control
            _cancellationToken = hostApplicationLifetime.ApplicationStopping;
        }

        public async ValueTask<int> Run()
        {
            PromptPlus.DoubleDash("Choose a option");
            var result = PromptPlus
                .Select<string>("") // no show prompt
                .AddItem("Opc1")
                .AddItem("Opc2")
                .AddItem("Opc3")
                .FilterType(FilterMode.Disabled) // disable filter feature
                .Run(_cancellationToken);

            //check if result is not valid (AKA: Aborted by CTRL+C (_cancellationToken.IsCancellationRequested = true) or [ESC])
            if (result.IsAborted)
            {
                //abnormal termination
                //do anything (Graceful Shutdown)
                return await ValueTask.FromResult(-1);
            }
            //get valid result
            var Inputcontent = result.Value;
            //do anything with Inputcontent.

            PromptPlus.DoubleDash("Choose a option with overwrite global setting");
            result = PromptPlus
                .Select<string>("Seleted") // show prompt
                .Config(cfg => 
                {
                    cfg.ShowOnlyExistingPagination(false)
                      .DisableToggleTooltip(false)
                      .ShowTooltip(true);
                })
                .AddItem("Opc1")
                .AddItem("Opc2")
                .AddItem("Opc3")
                .FilterType(FilterMode.Contains) // this is the default filter if this command is omitted
                .Run(_cancellationToken);
            if (result.IsAborted)
            {
                //abnormal termination
                //do anything (Graceful Shutdown)
                return await ValueTask.FromResult(-1);
            }



            //Check if user Aborted by CTRL+C
            //Must be setted Console.CancelKeyPress event.See comment code in Program.cs
            //if (_cancellationToken.IsCancellationRequested)
            //{
            //  do anything (Graceful Shutdown)
            //  return await ValueTask.FromResult(-1);
            //}


            //Normal termination
            return await ValueTask.FromResult(0);
        }
    }
}
