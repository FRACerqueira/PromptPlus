using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.InputAutoComplete
{
    public class InputAutoCompleteTests : BaseTest
    {
        private async Task<string[]> MYServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
        {
            if (count == 0)
            {
                count = 5;
            }
            var items = new List<string>();
            for (var i = 0; i < count; i++)
            {
                items.Add($"{prefixText}:{i}");
            }
            return await Task.FromResult(items.ToArray());
        }

        [Fact]
        public void Should_ValidInitControlPromptShowAutoComplete()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionMaxCount(5)
                .MinimumPrefixLength(1)
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);


            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(3000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                var max = 2000;
                while (!ctrl.IsAutoCompleteFinish)
                {
                    Thread.Sleep(100);
                    max -= 100;
                    if (max < 0)
                    {
                        break;
                    }
                }
                Assert.True(ctrl.IsAutoCompleteFinish);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA", result.Value);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptShowAutoCompletePageSize1()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionMaxCount(5)
                .PageSize(2)
                .MinimumPrefixLength(1)
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(3000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                var max = 2000;
                while (!ctrl.IsAutoCompleteFinish)
                {
                    Thread.Sleep(100);
                    max -= 100;
                    if (max < 0)
                    {
                        break;
                    }
                }
                Assert.True(ctrl.IsAutoCompleteFinish);
                ctrl.TryResult(CancellationToken.None);

                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA", result.Value);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptShowAutoCompletePageSize2()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionMaxCount(5)
                .PageSize(2)
                .MinimumPrefixLength(1)
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(3000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                var result = ctrl.TryResult(CancellationToken.None);
                var max = 2000;
                while (!ctrl.IsAutoCompleteFinish)
                {
                    Thread.Sleep(100);
                    max -= 100;
                    if (max < 0)
                    {
                        break;
                    }
                }
                Assert.True(ctrl.IsAutoCompleteFinish);
                ctrl.TryResult(CancellationToken.None);

                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA", result.Value);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.PageDown, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));

            });
        }


        [Fact]
        public void Should_ValidInitControlPromptSelectAutoComplete()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionMaxCount(5)
                .MinimumPrefixLength(2)
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(3000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var max = 2000;
                while (!ctrl.IsAutoCompleteFinish)
                {
                    Thread.Sleep(100);
                    max -= 100;
                    if (max < 0)
                    {
                        break;
                    }
                }
                Assert.True(ctrl.IsAutoCompleteFinish);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA:0", result.Value);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));
            });
        }

        [Fact]
        public void Should_ValidInitControlPromptSelectAndClearAutoComplete()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionMaxCount(5)
                .MinimumPrefixLength(2)
                .CompletionAsyncService(MYServiceCompleteAsync);
 
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            CompletesIn(3000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                var max = 2000;
                while (!ctrl.IsAutoCompleteFinish)
                {
                    Thread.Sleep(100);
                    max -= 100;
                    if (max < 0)
                    {
                        break;
                    }
                }
                Assert.True(ctrl.IsAutoCompleteFinish);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA:0", result.Value);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:0"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));
                PromptPlus.MemoryInputBuffer("B");
                result = ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Equal("AA:0B", result.Value);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:1"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:2"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:3"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:4"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("AA:5"));

            });
        }


        [Fact]
        public void Should_ValidInitControlPrompt1()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P")
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPrompt2()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionAsyncService(MYServiceCompleteAsync);

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default");

            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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

            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default")
              .OverwriteDefaultFrom(namehist);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default")
              .OverwriteDefaultFrom(namehist);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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

            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default")
              .OverwriteDefaultFrom(namehist);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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

            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default")
              .OverwriteDefaultFrom(namehist);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "AA", false);

            ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync)
              .Default("Default")
              .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("AA", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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
            var ctrl = (AutoCompleteControl)PromptPlus.AutoComplete("P", "D", (cfg) =>
            {
                cfg.ShowTooltip(false);
            })
            .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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
            var ctrl = (AutoCompleteControl)PromptPlus.AutoComplete("P", "D", (cfg) =>
            {
                cfg.Tooltips("CustomTooltip");
            })
            .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Result", false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Result"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

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
        public void Should_TryResultDefaultSetting1()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(10000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
              .AutoComplete("P", "D")
              .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(10000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .MaxLength(2);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(10000, () =>
            {
                PromptPlus.MemoryInputBuffer("1");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("2");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("3");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("12", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultValidator1()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .AddValidators(PromptValidators.MinLength(2));
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .AddValidators(PromptValidators.MinLength(2));
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .DefaultIfEmpty("ISEMPTY");
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .InputToCase(CaseOptions.Uppercase);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .InputToCase(CaseOptions.Lowercase);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("X");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value);
            });
        }

        [Fact]
        public void Should_TryResultAcceptInputUpperCase()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .InputToCase(CaseOptions.Uppercase)
               .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
            {
                PromptPlus.MemoryInputBuffer("A");
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.MemoryInputBuffer("x");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value);

            });
        }

        [Fact]
        public void Should_TryResultHideTooltips()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync);
            ctrl.InitControl(CancellationToken.None);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .Config((cfg) => cfg.EnabledAbortKey(false));
            ctrl.InitControl(CancellationToken.None);
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            CompletesIn(1000, () =>
            {
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
            });
        }

        [Fact]
        public void Should_ValidateOnDemand1()
        {
            var ctrl = (AutoCompleteControl)PromptPlus
               .AutoComplete("P", "D")
               .CompletionAsyncService(MYServiceCompleteAsync)
               .ValidateOnDemand(true)
               .AddValidators(PromptValidators.MinLength(5));
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .ValidateOnDemand(false)
                .AddValidators(PromptValidators.MinLength(5));
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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
            var ctrl = (AutoCompleteControl)PromptPlus
                .AutoComplete("P", "D")
                .CompletionAsyncService(MYServiceCompleteAsync)
                .ChangeDescription((input) =>
                {
                    if (input.Length == 1)
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });
            ActionOnDispose = () => ctrl.FinalizeControl(CancellationToken.None);

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(1000, () =>
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