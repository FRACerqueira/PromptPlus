using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;
using System.Globalization;

namespace PPlus.Tests.Controls.KeyPress
{

    public class KeyPressTests : BaseTest
    {

        [Fact]
        public void Should_KeyPressRunoutputbasic1()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = true;
            });
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(100, () =>
                {
                    PromptPlus.InputBuffer("A");
                    PromptPlus.KeyPress("P", "D")
                         .Run();
                });
            });
            Assert.Equal(Expectations.GetVerifyAnsi("Keypress.txt"), output);
        }


        [Fact]
        public void Should_KeyPressRunoutputbasic2()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = true;
            });
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(100, () =>
                {
                    PromptPlus.InputBuffer("S");
                    PromptPlus.Confirm("P", "D")
                         .Run();
                });
            });
            Assert.Equal(Expectations.GetVerifyAnsi("Confirm.txt"), output);
        }

        [Fact]
        public void Should_KeyPressRunoutputbasic3()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(100, () =>
                {
                    PromptPlus.InputBuffer("A");
                    PromptPlus.KeyPress("P", "D")
                         .Run();
                });
            });
            Assert.Equal(Expectations.GetVerifyControlStd("Keypress.txt"), output);
        }

        [Fact]
        public void Should_KeyPressRunoutputbasic4()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var output = PromptPlus.RecordOutput(() =>
            {
                CompletesIn(100, () =>
                {
                    PromptPlus.InputBuffer("S");
                    PromptPlus.Confirm("P", "D")
                         .Run();
                });
            });
            Assert.Equal(Expectations.GetVerifyControlStd("Confirm.txt"), output);
        }

        [Fact]
        public void Should_ValidInitControlPrompt()
        {
            var opt = new KeyPressOptions(false);
            var ctrl = new KeyPressControl((IConsoleControl)PromptPlus.Console, opt);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(Messages.AnyKey, opt.OptPrompt);
            Assert.Null(init);
        }


        [Fact]
        public void Should_FinalizeControl()
        {
            var opt = new KeyPressOptions(false);
            var ctrl = new KeyPressControl((IConsoleControl)PromptPlus.Console, opt);
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_Dispose()
        {
            var opt = new KeyPressOptions(false);
            var ctrl = new KeyPressControl((IConsoleControl)PromptPlus.Console, opt);
            ctrl.Dispose();
            ctrl.Dispose();
        }


        [Fact]
        public void Should_ValidInitControlPromptValidkey()
        {
            var opt = new KeyPressOptions(false);
            var ctrl = new KeyPressControl((IConsoleControl)PromptPlus.Console, opt);
            ctrl.AddKeyValid(ConsoleKey.M);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(Messages.ValidAnyKey, opt.OptPrompt);
            Assert.Null(init);
        }

        [Fact]
        public void Should_AcceptFinalizeControl()
        {
            var opt = new KeyPressOptions(false);
            var ctrl = new KeyPressControl((IConsoleControl)PromptPlus.Console, opt);
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (KeyPressControl)PromptPlus.KeyPress("P", "D");
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateWithoutTooltip()
        {
            var ctrl = (KeyPressControl)PromptPlus.KeyPress("P", "D", (cfg) =>
            {
                cfg.ShowTooltip(false);
            });
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltip()
        {
            var ctrl = (KeyPressControl)PromptPlus.KeyPress("P", "D", (cfg) =>
            {
                cfg.Tooltips("CustomTooltip");
            });
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputTemplateCustomTooltipWithConfigMethod()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D")
                .Config((cfg) =>
                {
                    cfg.Tooltips("CustomTooltip");
                });
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "CustomTooltip");
        }

        [Fact]
        public void Should_AcceptInputTemplateConfirmodeEn()
        {
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-US");
            var ctrl = (KeyPressControl)PromptPlus.Confirm("P", "D");
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("(Y/N)"));
        }


        [Fact]
        public void Should_AcceptInputTemplateConfirmodePt()
        {
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var ctrl = (KeyPressControl)PromptPlus.Confirm("P", "D");
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("(S/N)"));
        }

        [Fact]
        public void Should_AcceptInputTemplateConfirmodeCustom1()
        {
            var ctrl = (KeyPressControl)PromptPlus.Confirm("P", ConsoleKey.C, ConsoleKey.D, "D");
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("(C/D)"));
        }

        [Fact]
        public void Should_AcceptInputTemplateConfirmodeCustom2()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .Confirm("P", ConsoleKey.D1, ConsoleKey.D2, "D")
                .TextKeyValid((kp) =>
                {
                    if (kp.Key == ConsoleKey.D1)
                    {
                        return "1";
                    }
                    else if (kp.Key == ConsoleKey.D2)
                    {
                        return "2";
                    }
                    return null;
                });
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("(1/2)"));
        }

        [Fact]
        public void Should_AcceptInputTemplateKeyPressCustom()
        {
            var ctrl = (KeyPressControl)PromptPlus.KeyPress("P", "D")
                .AddKeyValid(ConsoleKey.F6)
                .AddKeyValid(ConsoleKey.F13);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F6"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F13"));
        }


        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort1()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D")
                .AddKeyValid(ConsoleKey.A,ConsoleModifiers.Shift | ConsoleModifiers.Alt | ConsoleModifiers.Control);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb,new ConsoleKeyInfo('A',ConsoleKey.A,true,true,true),false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Crtl+Shift+Alt+A"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort2()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D");
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new ConsoleKeyInfo('A', ConsoleKey.A, true, true, true), false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Crtl+Shift+Alt+A"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.Pressedkey);
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D");
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new ConsoleKeyInfo('A', ConsoleKey.A, true, true, true), true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Crtl+Shift+Alt+A"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_TryResultDefaultSetting1()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D");
            CompletesIn(100, () => 
            {
                PromptPlus.InputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal('A', result.Value.KeyChar);
            });
        }


        [Fact]
        public void Should_TryResultDefaultSetting2()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D")
                .AddKeyValid(ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Alt | ConsoleModifiers.Control);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo('A', ConsoleKey.A, true, true, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal('A', result.Value.KeyChar);
                Assert.True(result.Value.Modifiers.HasFlag(ConsoleModifiers.Shift));
                Assert.True(result.Value.Modifiers.HasFlag(ConsoleModifiers.Alt));
                Assert.True(result.Value.Modifiers.HasFlag(ConsoleModifiers.Control));
            });
        }


        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D")
                .AddKeyValid(ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Alt | ConsoleModifiers.Control);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F1,false,false,false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            });
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResulWithSpinner1()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D").Spinner(SpinnersType.Ascii);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResulWithSpinner2()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D").Spinner(SpinnersType.Ascii)
                .AddKeyValid(ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Alt | ConsoleModifiers.Control);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(600, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F1, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                Thread.Sleep(300);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                Assert.True(result.IsAborted);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResulIgnoreEsc()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D")
                .Config((cfg) => cfg.EnabledAbortKey(false));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                PromptPlus.InputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.Equal('A', result.Value.KeyChar);
                Assert.False(result.IsRunning);
            });
        }

        [Fact]
        public void Should_TryResultAbort()
        {
            var ctrl = (KeyPressControl)PromptPlus
                .KeyPress("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                using var cts = new CancellationTokenSource();
                cts.Cancel();
                var result = ctrl.TryResult(cts.Token);
                Assert.True(result.IsAborted);
            });
        }

        [Theory]
        [InlineData("Y")]
        [InlineData("N")]
        public void Should_TryResulIgnoreInvalidKeyEN(string inputchar)
        {
            PromptPlus.Config.DefaultCulture = new CultureInfo("en-US");
            var ctrl = (KeyPressControl)PromptPlus
                .Confirm("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(inputchar);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.Equal(inputchar, result.Value.KeyChar.ToString());
                Assert.False(result.IsRunning);
            });
        }

        [Theory]
        [InlineData("S")]
        [InlineData("N")]
        public void Should_TryResulIgnoreInvalidKeyPT(string inputchar)
        {
            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            var ctrl = (KeyPressControl)PromptPlus
                .Confirm("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(inputchar);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.Equal(inputchar, result.Value.KeyChar.ToString());
                Assert.False(result.IsRunning);
            });
        }

        [Theory]
        [InlineData("C")]
        [InlineData("D")]
        public void Should_TryResulIgnoreInvalidKeyCustom1(string inputchar)
        {
            var ctrl = (KeyPressControl)PromptPlus
                .Confirm("P",ConsoleKey.C,ConsoleKey.D);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(inputchar);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.Equal(inputchar, result.Value.KeyChar.ToString());
                Assert.False(result.IsRunning);
            });
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public void Should_TryResulIgnoreInvalidKeyCustom2(string inputchar)
        {
            var ctrl = (KeyPressControl)PromptPlus
                .Confirm("P", ConsoleKey.D1, ConsoleKey.D2)
                .TextKeyValid((kp) =>
                {
                    if (kp.Key == ConsoleKey.D1)
                    {
                        return "1";
                    }
                    else if (kp.Key == ConsoleKey.D2)
                    {
                        return "2";
                    }
                    return null;
                });
            
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(inputchar);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.Equal(inputchar, result.Value.KeyChar.ToString());
                Assert.False(result.IsRunning);
            });
        }
    }
}
