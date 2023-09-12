// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.Select
{
    public class SelectTests :BaseTest
    {
        private class MyClass
        {
            public int Id { get; set; }
            public string? MyText { get; set; }
            public string? MyDesc { get; set; }
        }

        private IEnumerable<MyClass> TestColletionMyClass(int max = 10)
        {
            var result = new List<MyClass>();
            for (int i = 0; i < max; i++)
            {
                result.Add(new MyClass 
                { 
                    Id = i, 
                    MyDesc = $"Desc{i}",
                    MyText = $"Item{i}",
                } );
            }
            return result.ToArray();
        }

        private IEnumerable<string> TestCity()
        {
            return new string[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" };
        }
        private const string DefaultCity = "New York";

        private enum MyEnum
        {
            None,
            [Display(Name = "option one")]
            Opc1,
            [Display(Name = "option two")]
            Opc2,
            [Display(Name = "option three")]
            Opc3
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptySelect1()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P",(cfg) => { });
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptySelect2()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptNotEmptySelect()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItem("1")
                .AddItem("2");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("1",init);
        }

        [Fact]
        public void Should_ValidInitControlPromptDefaultValue()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity())
                .Default(DefaultCity);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(DefaultCity, init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize("Seattle"), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity())
                .Default(DefaultCity)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Seattle", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory2()
        {
            //not exit file
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity())
                .Default(DefaultCity)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal(DefaultCity, init);
        }
        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory3()
        {
            //exit file with timeout
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory
                .AddHistory(JsonSerializer.Serialize("Seattle"),
                    TimeSpan.FromMilliseconds(1), null);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity())
                .Default(DefaultCity)
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal(DefaultCity, init);
        }


        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory4()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity())
                 .Default(DefaultCity)
                 .OverwriteDefaultFrom(namehist);

            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, "Seattle", false);

            ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity())
                 .Default(DefaultCity)
                 .OverwriteDefaultFrom(namehist);

            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Seattle", init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (SelectControl<string>)PromptPlus.Select<string>("P", "D");
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (SelectControl<string>)PromptPlus.Select<string>("P", "D");
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
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .Config( (cfg) =>
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
            var ctrl = (SelectControl<string>)PromptPlus.Select<string>("P", "D", (cfg) =>
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
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity());

           var init =  ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, init, false);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("P"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains(init));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "Tokyo");
        }

        [Fact]
        public void Should_AcceptInputFinishTemplateAbort()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity());

            var init = ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb, init, true);
            Assert.Contains(sb.Buffer, x => !x.SaveCursor);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == "P");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "D");
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains(init));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty) == Messages.CanceledKey);
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty) == "Tokyo");
        }


        [Fact]
        public void Should_ValidInitControlPromptInteraction()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .Interaction(TestCity(), (ctrl, item) =>
                {
                    ctrl.AddItem(item);
                });
            var init = ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Seattle"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Tokyo"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Singapore"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Shanghai"));
        }

        [Fact]
        public void Should_ValidInitControlPromptEqualItemsAndTextSelector()
        {
            var ctrl = (SelectControl<MyClass>)PromptPlus
                .Select<MyClass>("P", "D")
                .EqualItems((item1,item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .AddItems(TestColletionMyClass(3));
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item0", init);
        }

        [Fact]
        public void Should_SelectEnum()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus.Select<MyEnum>("Select");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("None", init);
        }

        [Fact]
        public void Should_SelectScopeRemove1()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus
                .Select<MyEnum>("Select")
                .AddItemsTo(AdderScope.Remove,MyEnum.None);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option one", init);
        }

        [Fact]
        public void Should_SelectScopeRemove2()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus
                .Select<MyEnum>("Select")
                .AddItemsTo(AdderScope.Remove, new MyEnum[] { MyEnum.None, MyEnum.Opc1 });
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option two", init);
        }

        [Fact]
        public void Should_SelectScopeDisable()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus
                .Select<MyEnum>("Select")
                .AddItemsTo(AdderScope.Disable, MyEnum.None);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option one", init);
        }


        [Fact]
        public void Should_SelectEnumWithDefault()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus.Select<MyEnum>("Select")
                .Default(MyEnum.Opc3);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option three", init);
        }

        [Fact]
        public void Should_SelectEnumWithTextSelector()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus.Select<MyEnum>("Select")
                .TextSelector((item) => item.ToString())
                .Default(MyEnum.Opc3);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Opc3", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptPageSize()
        {
            var ctrl = (SelectControl<MyClass>)PromptPlus
                .Select<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .AddItems(TestColletionMyClass(10))
                .PageSize(2);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item0", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item0"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item1"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item2"));
        }


        [Fact]
        public void Should_ValidInitControlPromptOrderBy()
        {
            var ctrl = (SelectControl<MyClass>)PromptPlus
                .Select<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .OrderBy((item) => item.MyText!)
                .AddItems(TestColletionMyClass(3).OrderByDescending(x => x.MyText));
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item0", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOrderByDescending()
        {
            var ctrl = (SelectControl<MyClass>)PromptPlus
                .Select<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .OrderByDescending((item) => item.MyText!)
                .AddItems(TestColletionMyClass(3));
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item2", init);
        }

        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity());
            var init = ctrl.InitControl(CancellationToken.None);

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
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity());
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
            var ctrl = (SelectControl<string>)PromptPlus
                 .Select<string>("P", "D")
                 .AddItems(TestCity())
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
        public void Should_ChangeDescription()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity())
                .ChangeDescription((input) =>
                {
                    if (input == "Seattle")
                    {
                        return "ChangeDescription=1";
                    }
                    return "";
                });

            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));
                PromptPlus.MemoryInputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow,false,false,false));
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }


        [Fact]
        public void Should_FilterTypeContains1()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer("o");
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Tokyo"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("New York"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Singapore"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Shanghai"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Seattle"));
            });
        }


        [Fact]
        public void Should_FilterTypeContains2()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .FilterType(FilterMode.Contains)
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer("b");
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Tokyo"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("New York"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Singapore"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Shanghai"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Seattle"));
            });
        }

        [Fact]
        public void Should_FilterTypeStartsWith()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .FilterType(FilterMode.StartsWith)
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer("s");
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Tokyo"));
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("New York"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Singapore"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Shanghai"));
                Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Seattle"));
            });
        }

        [Fact]
        public void Should_SelectShowDisable()
        {
            var ctrl = (SelectControl<MyEnum>)PromptPlus
                .Select<MyEnum>("Select")
                .AddItemsTo(AdderScope.Disable, MyEnum.None);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option one", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("None"));

        }


        [Fact]
        public void Should_AutoSelect()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .FilterType(FilterMode.StartsWith)
                .AutoSelect()
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer("t");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsRunning);
                Assert.False(result.IsAborted);
                Assert.Equal("Tokyo", result.Value);
            });
        }

        [Fact]
        public void Should_EnterSelct()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D")
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsRunning);
                Assert.False(result.IsAborted);
            });
        }

        [Fact]
        public void Should_EnterSelectNotSelect()
        {
            var ctrl = (SelectControl<string>)PromptPlus
                .Select<string>("P", "D");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.MemoryInputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.IsRunning);
                Assert.False(result.IsAborted);
            });
        }
    }
}
