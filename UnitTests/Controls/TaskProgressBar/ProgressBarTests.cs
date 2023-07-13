using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;
using Shouldly;

namespace PPlus.Tests.Controls.TaskProgressBar
{
    public class ProgressBarTests : BaseTest
    {
        [Fact]
        public void Should_ValidInitControlPromptpt1()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P")
                .UpdateHandler((_,_) => { });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0", init);
        }


        [Fact]
        public void Should_ValidInitControlPrompt2()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P","D")
                .UpdateHandler((_, _) => { });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValue1()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .Default(20)
                .UpdateHandler((_, _) => { });


            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("20", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((_, _) => { });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_showAcceptInputEsc()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, _) => { prgbar.Finish = true; });

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
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar( ProgressBarType.Fill, "P",new object(), "D", (cfg) =>
                {
                    cfg.EnabledAbortKey(false);
                })
                .UpdateHandler((prgbar, _) => { prgbar.Finish = true; });

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
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(ProgressBarType.Fill, "P", new object(), "D", (cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                })
                .UpdateHandler((prgbar, _) => { prgbar.Finish = true; });

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
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, _) => { Thread.Sleep(1000); });

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
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, _) => { Thread.Sleep(5000); prgbar.Finish = true; })
                .Config((cfg) =>
                {
                    cfg.EnabledAbortKey(false);
                });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            ctrl.InputTemplate(new ScreenBuffer());

            // Given, When
            var result = Record.Exception(() => 
            {
                CompletesIn(3000, () =>
                {
                    PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                    var result = ctrl.TryResult(CancellationToken.None);
                });
            });

            // Then
            result.ShouldBeOfType<TimeoutException>();
        }

        [Fact]
        public void Should_TryResultAbort()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, cts) => { cts.WaitHandle.WaitOne(5000); prgbar.Finish = true; });


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
        public void Should_AcceptCustomFinish()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; })
                .Finish("FinishTest");
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), false);
                });
            });
            Assert.Contains("FinishTest", output);
        }


        [Fact]
        public void Should_AcceptFinishAbort()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(),100), true);
                });
            });
            var partmsg = Messages.CanceledKey.Split()[0];
            Assert.Contains(partmsg, output);
        }

        [Fact]
        public void Should_UpdateValue()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar("P", "D")
                .UpdateHandler((prgbar, cts) => { prgbar.Update(99); prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    var result = ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), result.Value.Lastvalue), true);
                });
            });
            var partmsg = Messages.CanceledKey.Split()[0];
            Assert.Contains(partmsg, output);
            Assert.Contains("99", output);
        }


        [Theory]
        [InlineData(ProgressBarType.AsciiDouble)]
        [InlineData(ProgressBarType.AsciiSingle)]
        [InlineData(ProgressBarType.Dot)]
        [InlineData(ProgressBarType.Light)]
        [InlineData(ProgressBarType.Heavy)]
        [InlineData(ProgressBarType.Bar)]
        [InlineData(ProgressBarType.Char)]
        [InlineData(ProgressBarType.Fill)]
        public void Should_ProgressBarType(ProgressBarType progressBarType)
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(progressBarType, "P", new object(), "D")
                .CharBar('+')
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), true);
                });
            });
            var partmsg = Messages.CanceledKey.Split()[0];
            Assert.Contains(partmsg, output);

        }



        [Fact]
        public void Should_AcceptWidth()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(ProgressBarType.Char, "P", new object(), "D")
                .Width(87)
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), true);
                });
            });
            Assert.Contains(new string('#',87), output);
        }


        [Fact]
        public void Should_Spinner()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(ProgressBarType.Char, "P", new object(), "D")
                .Spinner(SpinnersType.ArrowHeavy)
                .UpdateHandler((prgbar, cts) => { Thread.Sleep(1000); prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.InputTemplate(new ScreenBuffer());
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), true);
                });
            });
            Assert.Contains('►', output);
        }


        [Fact]
        public void Should_FracionalDigEn()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(ProgressBarType.Char, "P", new object(), "D")
                .Culture("en-US")
                .Default(20.2)
                .FracionalDig(2)
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), true);
                });
            });
            Assert.Contains("20.20", output);
        }

        [Fact]
        public void Should_FracionalDigPt()
        {
            var ctrl = (ProgressBarControl<object>)PromptPlus
                .ProgressBar(ProgressBarType.Char, "P", new object(), "D")
                .Culture("pt-BR")
                .Default(20.2)
                .FracionalDig(2)
                .UpdateHandler((prgbar, cts) => { prgbar.Finish = true; });

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            ctrl.InputTemplate(new ScreenBuffer());

            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(3000, () =>
                {
                    ctrl.TryResult(CancellationToken.None);
                    ctrl.FinishTemplate(new ScreenBuffer(), new ResultProgessBar<object>(new object(), 100), true);
                });
            });
            Assert.Contains("20,20", output);
        }
    }
}
