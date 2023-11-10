// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using System.Text.Json;
using PipeFilterCore;
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

            var context = new MyClassPipeline();
            var pipeline = PipeAndFilter.New<MyClassPipeline>()
                .AddPipe(FistName, "FistName")
                .AddPipe(LastName, "LastName")
                    .WithCondition(ExistFirstName)
                .AddPipe(Confirm,"Confirm")
                    .WithCondition(ExistFirstName)
                .AddPipe(WriteResult)
                    .WithGotoCondition(TryAgain, "FistName")
                .BuildAndCreate()
                .Init(context)
                .Run().Result;

            
            if (!pipeline.Aborted)
            {
                PromptPlus.WriteLine($"You input is {pipeline.Value!.FirtName} {pipeline.Value!.LastName}");
            }
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }

        private static ValueTask<bool> TryAgain(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            var confirm = false;
            if (pipe.TrySavedValue("Confirm", out var value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    confirm = JsonSerializer.Deserialize<bool>(value);
                }
            }
            return new ValueTask<bool>(!confirm);    
        }

        private static Task WriteResult(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            if (pipe.TrySavedValue("FistName", out string? value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    pipe.ThreadSafeAccess((Contract) =>
                    {
                        Contract.FirtName = JsonSerializer.Deserialize<string>(value) ?? string.Empty;
                    });
                }
            }
            if (pipe.TrySavedValue("LastName", out value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    pipe.ThreadSafeAccess((Contract) =>
                    {
                        Contract.LastName = JsonSerializer.Deserialize<string>(value) ?? string.Empty;
                    });
                }
            }
            return Task.CompletedTask;
        }

        private static Task Confirm(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            var result = PromptPlus.Confirm("Confirm all input")
                .Run();
            if (!result.IsAborted)
            {
                pipe.SaveValueAtEnd("Confirm", result.Value.IsYesResponseKey());
            }
            else
            {
                pipe.EndPipeAndFilter();
            }
            return Task.CompletedTask;
        }

        private static Task FistName(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            if (pipe.TrySavedValue("FistName", out string? value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = JsonSerializer.Deserialize<string>(value) ?? string.Empty;
                }
            }

            var result = PromptPlus.Input("Your First Name", "If first name is empty not get lastname and not confim inputs")
                 .Default(value ?? string.Empty)
                 .Run();
            if (!result.IsAborted)
            {
                pipe.SaveValueAtEnd("FistName", result.Value);
            }
            else
            {
                pipe.EndPipeAndFilter();
            }
            return Task.CompletedTask;
        }

        private static Task LastName(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            if (pipe.TrySavedValue("LastName", out string? value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = JsonSerializer.Deserialize<string>(value) ?? string.Empty;
                }
            }
            var result = PromptPlus.Input("Your Last Name")
                 .Default(value ?? string.Empty)
                 .Run();
            if (!result.IsAborted)
            {
                pipe.SaveValueAtEnd("LastName", result.Value);
            }
            else
            {
                pipe.EndPipeAndFilter();
            }
            return Task.CompletedTask;
        }

        private static ValueTask<bool> ExistFirstName(EventPipe<MyClassPipeline> pipe, CancellationToken token)
        {
            if (pipe.TrySavedValue("FistName", out string? value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = JsonSerializer.Deserialize<string>(value) ?? string.Empty;
                }
            }
            return ValueTask.FromResult(!string.IsNullOrEmpty(value));
        }

    }
}