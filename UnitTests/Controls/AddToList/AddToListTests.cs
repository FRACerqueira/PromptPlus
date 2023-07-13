using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.AddToList
{
    public class AddToListTests : BaseTest
    {
        SugestionOutput SugestionInputSample(SugestionInput arg)
        {
            var result = new SugestionOutput();
            result.Add("sugestion 1");
            result.Add("sugestion 2");
            result.Add("sugestion 3");
            return result;
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptyAddtoList1()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", (cfg) => { });
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptyAddtoList2()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptNotEmptySelect()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("D"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2")
                .Default("item3");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("item3", init);
        }


        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (ListControl)PromptPlus.AddtoList("P", "D");
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (ListControl)PromptPlus.AddtoList("P", "D");
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
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Config((cfg) =>
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
            var ctrl = (ListControl)PromptPlus.AddtoList("P", "D", (cfg) =>
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
            var ctrl = (ListControl)PromptPlus
                 .AddtoList("P", "D")
                 .AddItems(new string[] {"item1","item2"});

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new string[] { "item1", "item2" }, false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort()
        {
            var ctrl = (ListControl)PromptPlus
                 .AddtoList("P", "D")
                 .AddItems(new string[] { "item1", "item2" });

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, new string[] { "item1", "item2" }, true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "item1");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "item2");
        }


        [Fact]
        public void Should_ValidInitControlPromptInteraction()
        {
            var ctrl = (ListControl)PromptPlus
                 .AddtoList("P", "D")

                .Interaction(new string[] { "item1", "item2" }, (ctrl, item) =>
                {
                    ctrl.AddItem(item);
                });
            var init = ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
        }


        [Fact]
        public void Should_ValidInitControlPromptPageSize()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2")
                .PageSize(2);
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item1"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item2"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("item3"));
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);

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
        public void Should_TryResultAbort()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D");

            var init = ctrl.InitControl(CancellationToken.None);

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
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Config((cfg) => cfg.EnabledAbortKey(false));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
            });
        }

        [Fact]
        public void Should_CtrlEnterEnd()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D");

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter,false,false,true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsRunning);
                Assert.False(result.IsAborted);
            });
        }

        [Fact]
        public void Should_TryResultMaxLenght()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .MaxLenght(3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("123456");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("123", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResultValidator1()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 0);
            });
        }

        [Fact]
        public void Should_TryResultValidator2()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddValidators(PromptValidators.MinLength(2));
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("AA");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_TryResultUppercase()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .InputToCase(CaseOptions.Uppercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("a");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("A", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResultLowercase()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .InputToCase(CaseOptions.Lowercase);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("a", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResultAcceptInput()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("AX");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResultAcceptInputUpperCase()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .InputToCase(CaseOptions.Uppercase)
                .AcceptInput((input) =>
                {
                    return input.Equals('X');
                });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("Ax");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("X", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResulSugestion1()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .SuggestionHandler(SugestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("sugestion 1", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResulSugestion2()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .SuggestionHandler(SugestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, true, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.Equal("sugestion 3", result.Value.First());
            });
        }

        [Fact]
        public void Should_TryResulCancelSugestion()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .SuggestionHandler(SugestionInputSample);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)9, ConsoleKey.Tab, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 0);
            });
        }


        [Fact]
        public void Should_TryResulNotAllowDuplicate()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_TryResulAllowDuplicate()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AllowDuplicate();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
            });
        }

        [Fact]
        public void Should_AllowDuplicate1()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AllowDuplicate()
                .AddItem("item1")
                .AddItem("item1");
            ctrl.InitControl(CancellationToken.None);
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
            var result = ctrl.TryResult(CancellationToken.None);
            Assert.False(result.IsAborted);
            Assert.False(result.IsRunning);
            Assert.True(result.Value.Count() == 2);
        }

        [Fact]
        public void Should_NotAllowDuplicate()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item1");
            ctrl.InitControl(CancellationToken.None);
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
            var result = ctrl.TryResult(CancellationToken.None);
            Assert.False(result.IsAborted);
            Assert.False(result.IsRunning);
            Assert.True(result.Value.Count() == 1);
        }

        [Fact]
        public void Should_Range0()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2")
                .AddItem("item3")
                .Range(0,2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
            });
        }

        [Fact]
        public void Should_Range1()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }


        [Fact]
        public void Should_Range2()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("b");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
            });
        }


        [Fact]
        public void Should_Range3()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Range(2, 3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("b");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("c");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("d");
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
            });
        }

        [Fact]
        public void Should_Range4()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .Range(2,3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer("A");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("b");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("c");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
            });
        }



        [Fact]
        public void Should_DeleteItem()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
                Assert.True(result.Value.First() == "item2");
            });
        }

        [Fact]
        public void Should_DeleteItem_With_ChangeHotKeyEditItem()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .HotKeyRemoveItem(new HotKey(ConsoleKey.F7))
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
                Assert.True(result.Value.First() == "item2");
            });
        }

        [Fact]
        public void Should_Edit()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("test");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First() == "item1test");
            });
        }

        [Fact]
        public void Should_Edit_With_ChangeHotKeyEditItem()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .HotKeyEditItem(new HotKey(ConsoleKey.F7))
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("test");
                PromptPlus.InputBuffer(Environment.NewLine);
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, true));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First() == "item1test");
            });
        }

        [Fact]
        public void Should_AbortEdit()
        {
            var ctrl = (ListControl)PromptPlus
                .AddtoList("P", "D")
                .AddItem("item1")
                .AddItem("item2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer("test");
                 PromptPlus.InputBuffer(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("test"));
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
                Assert.True(result.Value.First() == "item1");

            });
        }
    }
}
