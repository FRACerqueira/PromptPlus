// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;

namespace PipelineSamples
{
    internal class Program
    {
        private enum Mypipes
        {
            FirstName,
            LastName,
            Confirm
        }

        private class MyClassPipeline
        {
            public string FirtName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            PromptPlus.Clear();

            PromptPlus.DoubleDash("Control Pipeline");

            var pl = PromptPlus.Pipeline(new MyClassPipeline())
                .AddPipe("First Name",
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Input(eventpipe.CurrentPipe,"If first name is empty not get lastname and not confim inputs")
                            .Default(eventpipe.Input.FirtName ?? string.Empty)
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            eventpipe.Input.FirtName = result.Value;
                        }
                    })
                .AddPipe("Last Name",
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Input(eventpipe.CurrentPipe)
                            .Default(eventpipe.Input.LastName ?? string.Empty)
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            eventpipe.Input.LastName = result.Value;
                        }
                    },
                    (eventpipe, stoptoken) =>
                    {
                        if (eventpipe.Input.FirtName?.Length > 0)
                        {
                            return true;
                        }
                        return false;
                    })
                .AddPipe("Confirm",
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Confirm(eventpipe.CurrentPipe)
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            if (!result.Value.IsYesResponseKey())
                            {
                                eventpipe.NextPipe("First Name");
                            }
                        }
                    })
                .Run();

            
            if (!pl.IsAborted)
            {
                if ((pl.Value.Context.FirtName!.Trim() + pl.Value.Context.LastName!.Trim()).Length > 0)
                {
                    var last = pl.Value.Context.LastName!.Trim();
                    if (last.Length > 0)
                    {
                        last = $", {last}";
                    }
                    PromptPlus.WriteLine($"You input is {pl.Value.Context.FirtName!}{last}");
                }
                else
                {
                    PromptPlus.WriteLine($"You input is empty");
                }
            }

            PromptPlus.DoubleDash("Control Pipeline by enum");

            var pl1 = PromptPlus.Pipeline(new MyClassPipeline())
                .AddPipe(Mypipes.FirstName,
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Input("Your First Name", "If first name is empty not get lastname and not confim inputs")
                            .Default(eventpipe.Input.FirtName ?? string.Empty)
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            eventpipe.Input.FirtName = result.Value;
                        }
                    })
                .AddPipe(Mypipes.LastName,
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Input("Your Last Name")
                            .Default(eventpipe.Input.LastName ?? string.Empty)
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            eventpipe.Input.LastName = result.Value;
                        }
                    },
                    (eventpipe, stoptoken) =>
                    {
                        if (eventpipe.Input.FirtName?.Length > 0)
                        {
                            return true;
                        }
                        return false;
                    })
                .AddPipe(Mypipes.Confirm,
                    (eventpipe, stoptoken) =>
                    {
                        var result = PromptPlus.Confirm("Confirm all inputs")
                            .Run();
                        if (result.IsAborted)
                        {
                            eventpipe.AbortPipeline();
                        }
                        else
                        {
                            if (!result.Value.IsYesResponseKey())
                            {
                                eventpipe.NextPipe(Mypipes.FirstName);
                            }
                        }
                    },
                    (eventpipe, stoptoken) =>
                    {
                        if (eventpipe.Input.FirtName?.Length > 0)
                        {
                            return true;
                        }
                        return false;
                    })
                .Run();

            if (!pl1.IsAborted)
            {
                if ((pl1.Value.Context.FirtName!.Trim() + pl1.Value.Context.LastName!.Trim()).Length > 0)
                {
                    var last = pl1.Value.Context.LastName!.Trim();
                    if (last.Length > 0)
                    {
                        last = $", {last}";
                    }
                    PromptPlus.WriteLine($"You input is {pl1.Value.Context.FirtName!}{last}");
                }
                else
                {
                    PromptPlus.WriteLine($"You input is empty");
                }
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}