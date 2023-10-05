// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.Input
{
    public class InputTests : BaseTest
    {
        SuggestionOutput SuggestionInputSample(SuggestionInput arg)
        {
            var result = new SuggestionOutput();
            result.Add("suggestion 1");
            result.Add("suggestion 2");
            result.Add("suggestion 3");
            return result;
        }

        [Fact]
        public void Should_ValidInitControlPrompt1()
        {
            var ctrl = (InputControl)PromptPlus.Input("P");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }


        [Fact]
        public void Should_ValidInitControlPrompt2()
        {
            var ctrl = (InputControl)PromptPlus.Input("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }


        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Default", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory("OverWriteDefault", TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("OverWriteDefault", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory2()
        {
            //not exit file
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Default", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory3()
        {
            //exit file with timeout
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("OverWriteDefault",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Default", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory4()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory("OverWriteDefault", TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default")
                .OverwriteDefaultFrom(namehist);
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "AA", false);

            ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .Default("Default")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("AA", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (InputControl)PromptPlus.Input("P", "D");
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (InputControl)PromptPlus.Input("P", "D");
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
            var ctrl = (InputControl)PromptPlus.Input("P", "D", (cfg) =>
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
            var ctrl = (InputControl)PromptPlus.Input("P", "D", (cfg) =>
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
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
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
        public void Should_AcceptInputTemplatePassword()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F2"));
        }

        [Fact]
        public void Should_AcceptInputTemplatePasswordViewSecret1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .EnabledViewSecret()
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F2"));
        }

        [Fact]
        public void Should_AcceptInputTemplatePasswordViewSecret2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .EnabledViewSecret(new HotKey(ConsoleKey.F10))
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F10"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbort()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Result", false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateNoAbortPassword()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Result", false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("######"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Result", true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Result", true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("######"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
        }

        [Fact]
        public void Should_TryResultDefaultSetting1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultDefaultSetting2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("A", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultMaxLength()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .MaxLength(3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("123456");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("123", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultValidator1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultValidator2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("AA");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("AA", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultDefaultSettingEmpty()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .DefaultIfEmpty("ISEMPTY");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("ISEMPTY", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultUppercase()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .InputToCase(CaseOptions.Uppercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("a");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value);
            });
        }


        [Fact]
        public void Should_TryResultLowercase()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .InputToCase(CaseOptions.Lowercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("a", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultAcceptInput()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("AX");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultAcceptInputUpperCase()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .InputToCase(CaseOptions.Uppercase)
                .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("Ax");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultSecret1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("ABC");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("ABC", result.Value);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => x.SaveCursor);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("###"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ABC"));

            });
        }

        [Fact]
        public void Should_TryResultSecret2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret('*');
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("ABC");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("ABC", result.Value);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => x.SaveCursor);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("***"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ABC"));

            });
        }


        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
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
        public void Should_TryResultEnabledViewSecret()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .IsSecret()
                .EnabledViewSecret();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("ABC");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F2"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ABC"));
            });
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
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
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D");
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
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
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
        public void Should_ValidInitControlHistoryEnabled()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultHistoryMinimumPrefixLength()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(10), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);


            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("1");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("1", result.Value);
            });

            FileHistory.ClearHistory(namehist);

        }


        [Fact]
        public void Should_ValidTryResultHistoryNotShowHistory()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("12");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal("12", result.Value);
            });

            FileHistory.ClearHistory(namehist);

        }


        [Fact]
        public void Should_ValidTryResultHistoryShowHistory1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST3"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST2"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("HIST1"));
            });


            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST2", result.Value);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory3()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMaxItems(1)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory4()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryPageSize(1)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.PageDown, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST2", result.Value);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultHistoryShowHistory5()
        {
            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryTimeout(TimeSpan.FromMilliseconds(1))
                .HistoryEnabled("HistoryEnabled");

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.FinishTemplate(sb, result.Value, result.IsAborted);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);


                Assert.Equal("HI", result.Value);
            });

            Thread.Sleep(2);

            ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryTimeout(TimeSpan.FromMilliseconds(1))
                .HistoryEnabled("HistoryEnabled");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("H");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("H", result.Value);
            });

            FileHistory.ClearHistory(namehist);
        }

        [Fact]
        public void Should_ValidTryResultAbortHistory()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("HIST3", result.Value);
            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_ValidTryResultClearHistory()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory("HIST1",
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.AddHistory("HIST2",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.AddHistory("HIST3",
                TimeSpan.FromSeconds(60), hist);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("HI");
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Delete, false, false, true));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.UpArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal(0, FileHistory.LoadHistory(namehist).Count);

            });

            FileHistory.ClearHistory(namehist);

        }

        [Fact]
        public void Should_SaveHistory()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .HistoryMinimumPrefixLength(2)
                .HistoryEnabled("HistoryEnabled");

            var namehist = "HistoryEnabled";
            FileHistory.ClearHistory(namehist);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("NEWHISTORY");
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                ctrl.FinishTemplate(sb, result.Value, result.IsAborted);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Equal(1, FileHistory.LoadHistory(namehist).Count);
                Assert.Equal("NEWHISTORY", FileHistory.LoadHistory(namehist)[0].History);

            });

            FileHistory.ClearHistory(namehist);

        }


        [Fact]
        public void Should_TryResulSuggestion1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("suggestion 1", result.Value);
            });
        }

        [Fact]
        public void Should_TryResulSuggestion2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, true, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("suggestion 3", result.Value);
            });
        }


        [Fact]
        public void Should_TryResulCancelSuggestion()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .SuggestionHandler(SuggestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("suggestion 1", result.Value);
            });
        }

        [Fact]
        public void Should_ValidateOnDemand1()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .ValidateOnDemand(true)
                .AddValidators(PromptValidators.MinLength(5));

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.NotNull(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_ValidateOnDemand2()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .ValidateOnDemand(false)
                .AddValidators(PromptValidators.MinLength(5));

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Null(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_ChangeDescription()
        {
            var ctrl = (InputControl)PromptPlus
                .Input("P", "D")
                .ChangeDescription((input) =>
                {
                    if (input.Length == 1)
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }
    }
}
