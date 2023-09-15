using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PPlus;

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
