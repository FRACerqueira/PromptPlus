using System.Text.Json;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.SliderSwitch
{
    public class SliderSwitchTests: BaseTest
    {

        [Fact]
        public void Should_ValidInitControlPromptptEn1()
        {
            PromptPlus.Config.DefaultCulture  = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Off",init);
        }

        [Fact]
        public void Should_ValidInitControlPromptEn2()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus.SliderSwitch("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Off", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptptPt()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("pt-BR");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Desligado", init);
        }



        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(true);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("On", init);
        }

        [Fact]
        public void Should_ValidInitControOnValue()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .OnValue("XXXXXXXXXXXXX")
                .Default(true);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("XXXXXXXXXXXXX", init);
        }

        [Fact]
        public void Should_ValidFinishControOnValue()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .OnValue("XXXXXXXXXXXXX")
                .Default(true);
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, true, false);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("XXXXXXXXXXXXX"));
        }

        [Fact]
        public void Should_ValidInitControOffValue()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .OffValue("XXXXXXXXXXXXX");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("XXXXXXXXXXXXX", init);
        }

        [Fact]
        public void Should_ValidFinishControOffValue()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .OffValue("XXXXXXXXXXXXX");
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, false, false);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("XXXXXXXXXXXXX"));
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(true), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(false)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("On", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory2()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            //not exit file
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(false)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Off", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory3()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            //exit file with timeout
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory(JsonSerializer.Serialize(true),
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(false)
                .OverwriteDefaultFrom(namehist);

            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Off", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory4()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(false), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(true)
                .OverwriteDefaultFrom(namehist);

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, false, false);

            ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Default(false)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Off", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");
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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D", (cfg) =>
                {
                    cfg.ShowTooltip(false);
                });
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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D", (cfg) =>
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
        public void Should_AcceptInputTemplateCustomTooltipWithConfigMethod()
        {
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
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
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, true, false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("On"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort1()
        {
            PromptPlus.Config.DefaultCulture = new System.Globalization.CultureInfo("en-US");

            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, true, true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Trim() == "P:");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("On"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D");

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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
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
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .Width(50);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => string.IsNullOrWhiteSpace(x.Text) && x.Width == 25);
        }


        [Fact]
        public void Should_ChangeDescription()
        {
            var ctrl = (SliderSwitchControl)PromptPlus
                .SliderSwitch("P", "D")
                .ChangeDescription((input) =>
                {
                    if (input)
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow,false,false,false));
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }
    }
}
