using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.TaskWaitControl
{
    public class WaitControlTests : BaseTest
    {
        [Fact]
        public void Should_ValidInitControlPrompt1()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P")
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPrompt2()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_OverwriteCultureTaskName()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P", "D")
                .TaskTitle("XXXXXXXXXXXX")
                .AddStep(StepMode.Sequential, "task1", "desc task1", (_, cts) =>
                {
                    Thread.Sleep(500);
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                    Assert.False(result.IsRunning);
                    Assert.True(result.Value.States.Count() == 1);
                });
            });
            Assert.Contains("XXXXXXXXXXXX", output);
        }

        [Fact]
        public void Should_Interaction()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P", "D")
                .Interaction(new string[2] { "", "" }, (ctrl, _) =>
                {
                    ctrl.AddStep(StepMode.Sequential, (_, cts) =>
                     {
                     });
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(3000, () =>
            {
                ctrl.InputTemplate(new ScreenBuffer());
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(!result.IsRunning);
                Assert.True(result.Value.States.Count() == 2);
                Assert.True(result.Value.States.First().Status == TaskStatus.RanToCompletion);
            });
        }


        [Fact]
        public void Should_Runtask1()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, "task1", "desc task1", (_, cts) =>
                {
                    Thread.Sleep(250);
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                    Assert.True(!result.IsRunning);
                    Assert.True(result.Value.States.Count() == 1);
                    Assert.True(result.Value.States.First().Status == TaskStatus.RanToCompletion);
                    Assert.True(result.Value.States.First().ElapsedTime > TimeSpan.FromMilliseconds(250));
                });
            });
            Assert.Contains("desc task1", output);
        }


        [Fact]
        public void Should_RuntaskWithException()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                .WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, "task1", "desc task1", (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(400);
                    throw new Exception();
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                ctrl.InputTemplate(new ScreenBuffer());
                ResultPrompt<ResultWaitProcess<object>>? result = null;
                CompletesIn(3000, () =>
                {
                    result = ctrl.TryResult(CancellationToken.None);
                }, true);
                Assert.False(result!.Value.IsRunning);
                Assert.True(result!.Value.Value.States.Count() == 1);
                Assert.True(result!.Value.Value.States.First().Status == TaskStatus.Faulted);
            });
            Assert.Contains("desc task1", output);
        }

        [Fact]
        public void Should_showAcceptInputEsc()
        {
            var ctrl = (WaitControl<object>)PromptPlus
                 .WaitProcess("P", "D")
                 .AddStep(StepMode.Parallel, (_, _) => { });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                });
            });
            Assert.Contains(string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress), output);
        }

        [Fact]
        public void Should_notshowAcceptInputEsc()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D", (cfg) =>
                {
                    cfg.EnabledAbortKey(false);
                })
                .AddStep(StepMode.Parallel, (_, _) => { });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                });
            });
            Assert.DoesNotContain(string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress), output);
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltip()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D", (cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                })
                .AddStep(StepMode.Parallel, (_, _) => { });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                });
            });
            Assert.Contains("CustomTooltip", output);
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(5000);
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(3000, () =>
            {
                ctrl.InputTemplate(new ScreenBuffer());
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                Thread.Sleep(500);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResulNotAcceptEsc()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(2000);
                })
                .Config((cfg) =>
                {
                    cfg.EnabledAbortKey(false);
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(3000, () =>
            {
                ctrl.InputTemplate(new ScreenBuffer());
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                Thread.Sleep(500);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(!result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.True(result.Value.States.First().ElapsedTime > TimeSpan.FromSeconds(2));
            });
        }

        [Fact]
        public void Should_TryResultAbort()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(5000);
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);


            using var cts = new CancellationTokenSource();

            ctrl.InitControl(cts.Token);

            ctrl.InputTemplate(new ScreenBuffer());

            CompletesIn(3000, () =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    cts.Cancel();
                });
                var result = ctrl.TryResult(cts.Token);
                Assert.True(result.IsAborted);
            });
        }

        [Fact]
        public void Should_EventAbortSequential()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .MaxDegreeProcess(2)
                .AddStep(StepMode.Sequential, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(1000);
                })
                .AddStep(StepMode.Sequential, (evt, cts) =>
                 {
                    evt.CancelAllNextTasks = true;
                    cts.WaitHandle.WaitOne(2000);
           
                 })
                .AddStep(StepMode.Sequential, (_, cts) =>
                 {
                     cts.WaitHandle.WaitOne(5000);
                 })
                .AddStep(StepMode.Sequential, (_, cts) =>
                 {
                     cts.WaitHandle.WaitOne(5000);
                 });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);


            using var cts = new CancellationTokenSource();

            ctrl.InitControl(cts.Token);

            ctrl.InputTemplate(new ScreenBuffer());

            CompletesIn(8000, () =>
            {
                var result = ctrl.TryResult(cts.Token);
                Assert.False(result.IsAborted);
                Assert.Equal(2,result.Value.States.Count(x => x.Status == TaskStatus.Canceled));
            });
        }

        [Fact]
        public void Should_EventChangeConext()
        {
            var ctrl = (WaitControl<int>)PromptPlus.WaitProcess<int>("P", "D")
                .MaxDegreeProcess(2)
                .Context(0)
                .AddStep(StepMode.Parallel, (evt, cts) =>
                {
                    evt.Context++;
                })
                .AddStep(StepMode.Parallel, (evt, cts) =>
                {
                    evt.Context++;
                })
                .AddStep(StepMode.Parallel, (evt, cts) =>
                {
                    evt.Context++;
                })
                .AddStep(StepMode.Parallel, (evt, cts) =>
                {
                    evt.Context++;
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);


            using var cts = new CancellationTokenSource();

            ctrl.InitControl(cts.Token);

            ctrl.InputTemplate(new ScreenBuffer());

            CompletesIn(1000, () =>
            {
                var result = ctrl.TryResult(cts.Token);
                Assert.False(result.IsAborted);
                Assert.Equal(4, result.Value.Context);
            });
        }

        [Fact]
        public void Should_EventAbortParallel()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .MaxDegreeProcess(2)
                .AddStep(StepMode.Parallel, (evt, cts) =>
                {
                    evt.CancelAllNextTasks = true;
                    cts.WaitHandle.WaitOne(1000);
                })
                .AddStep(StepMode.Parallel, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(2000);

                })
                .AddStep(StepMode.Parallel, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(5000);
                })
                .AddStep(StepMode.Parallel, (_, cts) =>
                {
                    cts.WaitHandle.WaitOne(5000);
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);


            using var cts = new CancellationTokenSource();

            ctrl.InitControl(cts.Token);

            ctrl.InputTemplate(new ScreenBuffer());

            CompletesIn(8000, () =>
            {
                var result = ctrl.TryResult(cts.Token);
                Assert.False(result.IsAborted);
                Assert.Equal(2, result.Value.States.Count(x => x.Status == TaskStatus.Canceled));
            });
        }


        [Fact]
        public void Should_AcceptCustomFinish()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .Finish("FinishTest")
                .AddStep(StepMode.Parallel, (_, _) => { });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultWaitProcess<object>(new object(), Array.Empty<StateProcess>()), false);
                });
            });
            Assert.Contains("FinishTest", output);
        }

        [Fact]
        public void Should_AcceptFinishAbort()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .AddStep(StepMode.Parallel, (_, _) => { });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultWaitProcess<object>(new object(), Array.Empty<StateProcess>()), true);
                });
            });
            var partmsg = Messages.CanceledKey.Split()[0];
            Assert.Contains(partmsg, output);
        }

        [Fact]
        public void Should_WaitTime()
        {
            var opt = new WaitOptions<object>(false)
            {
                WaitTime = true,
                TimeDelay = TimeSpan.FromSeconds(1),
                ShowCountdown = true,
                OptPrompt = "P"
            };
            var ctrl = new WaitControl<object>((IConsoleControl)PromptPlus.Console, opt);
            ctrl.Spinner(SpinnersType.Ascii);
            ctrl.AddStep(StepMode.Sequential, (_, cts) =>
            {
                cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(5000, () =>
            {
                ctrl.InputTemplate(new ScreenBuffer());
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.True(result.Value.States.First().ElapsedTime > TimeSpan.Zero);
            });
        }


        [Fact]
        public void Should_MaxDegreeProcess()
        {
            var ctrl = (WaitControl<object>)PromptPlus.WaitProcess("P", "D")
                .MaxDegreeProcess(1)
                .AddStep(StepMode.Parallel, (_, _) => { })
                .AddStep(StepMode.Parallel, (_, _) => { })
                .AddStep(StepMode.Parallel, (_, _) => { });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    var result = ctrl.TryResult(CancellationToken.None);
                    Assert.True(result.Value.States.Count() == 3);
                    Assert.True(!result.IsRunning);
                });
            });
        }
    }
}
