using System.Text.Json;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.SliderNumber
{
    public class SliderNumberTests : BaseTest
    {

        [Fact]
        public void Should_ValidInitControlPromptpt1()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P")
                .Range(-100,100);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0", init);
        }

        [Fact]
        public void Should_ValidInitControlPrompt2()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P","D")
                .Range(-100, 100);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("0", init);
        }


        [Fact]
        public void Should_ValidInitControlPromptDefaultValue1()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(20);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("20", init);
        }


        [Fact]
        public void Should_ValidInitControlPromptDefaultValue2()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(-20);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("-20", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(20), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(10)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("20", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory2()
        {
            //not exit file
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(10)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("10", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory3()
        {
            //exit file with timeout
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory(JsonSerializer.Serialize(20),
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(10)
                .OverwriteDefaultFrom(namehist);

            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("10", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory4()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(20), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);


            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(10)
                .OverwriteDefaultFrom(namehist);


            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, 30, false);

            ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100)
                .Default(10)
                .OverwriteDefaultFrom(namehist);

            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("30", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100);
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(-100, 100);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateWithoutTooltip()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D",(cfg) =>
                {
                    cfg.ShowTooltip(false);
                })
                .Range(-100, 100);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltip()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D", (cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                })
                .Range(-100, 100);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltipWithConfigMethod()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Config((cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                });
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, 30, false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("30"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort1()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Default(40)
                .Range(0, 100);
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, 40, true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Trim() == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("40"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F1, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            });
        }


        [Fact]
        public void Should_TryResulEnter()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResultAbort()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                var result = ctrl.TryResult(cts.Token);
                Assert.True(result.IsAborted);
            });
        }

        [Fact]
        public void Should_TryResulNotAcceptEsc()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Config((cfg) => cfg.EnabledAbortKey(false));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
            });
        }

        [Fact]
        public void Should_AcceptWidth()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Width(50);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => string.IsNullOrWhiteSpace(x.Text) && x.Width == 50);
        }


        [Fact]
        public void Should_ChangeDescription()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .ChangeDescription((input) =>
                {
                    if (input != 0)
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Tab, true, false, false));
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }

        [Fact]
        public void Should_FracionalDig()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .FracionalDig(2)
                .Step(0.1);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);

                Assert.Equal(0.1, result.Value);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(0, result.Value);
            });
        }


        [Fact]
        public void Should_CultureEN()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .FracionalDig(2)
                .Culture("en-us")
                .Default(10.34);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("10.34"));
        }

        [Fact]
        public void Should_CulturePT()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .FracionalDig(2)
                .Culture("pt-br")
                .Default(10.34);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("10,34"));
        }

        [Fact]
        public void Should_Step()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Step(5);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);

                Assert.Equal(5,result.Value);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(0, result.Value);
            });
        }


        [Fact]
        public void Should_LargeStep()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .LargeStep(15);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Tab, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);

                Assert.Equal(15, result.Value);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Tab, true, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(0, result.Value);
            });
        }


        [Fact]
        public void Should_MoveKeyPressUpDownMode1()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Layout(LayoutSliderNumber.UpDown)
                .Step(5);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);

                Assert.Equal(5, result.Value);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(0, result.Value);
            });
        }

        [Fact]
        public void Should_MoveKeyPressUpDownMode2()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Layout(LayoutSliderNumber.UpDown)
                .Step(5);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(0, result.Value);
            });
        }

        [Fact]
        public void Should_MoveKeyPressUpDownMode3()
        {
            var ctrl = (SliderNumberControl)PromptPlus
                .SliderNumber("P", "D")
                .Range(0, 100)
                .Default(10)
                .Layout(LayoutSliderNumber.UpDown)
                .Step(5);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.Equal(10, result.Value);
            });
        }
    }
}
