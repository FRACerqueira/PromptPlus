using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.MultiSelect
{
    public class MultiSelectTests : BaseTest
    {
        private static IEnumerable<MyClass> LoadData()
        {
            var aux = new List<MyClass>
            {
                new MyClass { Id = 7, MyText = "Text4", MyDesc="Text4 for id=4", IsDisabled = false, IsHide = true , IsSeleted = true},
                new MyClass { Id = 4, MyText = "Text4", MyDesc="Text4 for id=4", IsDisabled = false, IsHide = false , IsSeleted = true},
                new MyClass { Id = 5, MyText = "Text5", MyDesc="Text5 for id=5", IsDisabled = false, IsHide = false },
                new MyClass { Id = 6, MyText = "Text6", MyDesc="Text6 for id=6", IsDisabled = false, IsHide = true },
                new MyClass { Id = 1, MyText = "Text1", MyDesc="Text1 for id=1", IsDisabled = false, IsHide = false },
                new MyClass { Id = 0, MyText = "Text1", MyDesc="Text1 for id=0", IsDisabled = false, IsHide = false },
                new MyClass { Id = 2, MyText = "Text2", MyDesc="Text2 for id=2", IsDisabled = true, IsSeleted = true },
                new MyClass { Id = 3, MyText = "Text3", MyDesc="Text3 for id=3", IsDisabled = true, IsHide = false }
            };
            return aux;
        }
        private class MyClass
        {
            public int Id { get; set; }
            public string? MyText { get; set; }
            public string? MyDesc { get; set; }
            public bool IsDisabled { get; set; }
            public bool IsSeleted { get; set; }
            public bool IsHide { get; set; }

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
                });
            }
            return result.ToArray();
        }

        private IEnumerable<string> TestCity()
        {
            return new string[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" };
        }

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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", (cfg) => { });
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_ValidInitControlPromptEmptySelect2()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }


        [Fact]
        public void Should_ValidInitControlPromptNotEmptySelect()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItem("1")
                .AddItem("2");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Empty(init);
        }

        [Fact]
        public void Should_FinalizeControl()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P", "D");
            ctrl.FinalizeControl(CancellationToken.None);
        }

        [Fact]
        public void Should_AcceptInputTemplateWithTooltip()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P", "D");
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
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
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P", "D", (cfg) =>
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
        public void Should_ValidInitControlPromptInteraction()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
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
        public void Should_ValidInitControlPromptPageSize()
        {
            var ctrl = (MultiSelectControl<MyClass>)PromptPlus
                .MultiSelect<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .AddItems(TestColletionMyClass(10))
                .PageSize(2);
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item0"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item1"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Item2"));
        }


        [Fact]
        public void Should_TryResulAcceptEsc()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItems(TestCity());
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItems(TestCity())
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
        public void Should_ChangeDescription()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
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
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.DownArrow, false, false, false));
                ctrl.TryResult(CancellationToken.None);
                sb = new ScreenBuffer();
                ctrl.InputTemplate(sb);
                Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("ChangeDescription=1"));

            });
        }


        [Fact]
        public void Should_FilterTypeContains1()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.InputBuffer("o");
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .FilterType(FilterMode.Contains)
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.InputBuffer("b");
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .FilterType(FilterMode.StartsWith)
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.InputBuffer("s");
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
        public void Should_EnterSelct()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                var sb = new ScreenBuffer();
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsRunning);
                Assert.False(result.IsAborted);
            });
        }

        [Fact]
        public void Should_SelectAll()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == TestCity().Count());
            });
        }

        [Fact]
        public void Should_HotKeySelectAll()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .HotKeySelectAll(new HotKey(ConsoleKey.F7))
                 .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F7"));

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == TestCity().Count());
            });
        }

        [Fact]
        public void Should_HotKeyInvertSelected()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .HotKeyInvertSelected(new HotKey(ConsoleKey.F7))
                 .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("F7"));

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F7, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == TestCity().Count());
            });
        }

        [Fact]
        public void Should_SelectOne()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1")
                 .AddItem("ITEM2")
                 .AddItem("ITEM3");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_UnSelectOne()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1",false,true)
                 .AddItem("ITEM2")
                 .AddItem("ITEM3");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 0);
            });
        }

        [Fact]
        public void Should_InvertSelectAll1()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItems(TestCity());
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == TestCity().Count());
            });
        }

        [Fact]
        public void Should_InvertSelectAll2()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2")
                 .AddItem("ITEM3");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
            });
        }

        [Fact]
        public void Should_Ranger1()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2")
                 .AddItem("ITEM3")
                 .AddItem("ITEM4")
                 .AddItem("ITEM5")
                 .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotEmpty(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_Ranger2()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2", false, true)
                 .AddItem("ITEM3")
                 .AddItem("ITEM4")
                 .AddItem("ITEM5")
                 .Range(2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_Ranger3()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2", false, true)
                 .AddItem("ITEM3")
                 .AddItem("ITEM4")
                 .AddItem("ITEM5")
                 .Range(2,3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_Ranger4()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2", false, true)
                 .AddItem("ITEM3", false, true)
                 .AddItem("ITEM4")
                 .AddItem("ITEM5")
                 .Range(2, 3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
            });
        }

        [Fact]
        public void Should_Ranger5()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1")
                 .AddItem("ITEM2", false, true)
                 .AddItem("ITEM3", false, true)
                 .AddItem("ITEM4", false, true)
                 .Range(2, 3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotEmpty(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
            });
        }


        [Fact]
        public void Should_Ranger6()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1", false, true)
                 .AddItem("ITEM2", false, true)
                 .AddItem("ITEM3", false, true)
                 .AddItem("ITEM4", false, true)
                 .Range(2, 3);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.Null(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 3);
                Assert.Contains(result.Value, x => (x ?? string.Empty) == "ITEM1");
                Assert.Contains(result.Value, x => (x ?? string.Empty) == "ITEM2");
                Assert.Contains(result.Value, x => (x ?? string.Empty) == "ITEM3");
                Assert.DoesNotContain(result.Value, x => (x ?? string.Empty).Contains("ITEM4"));
            });
        }

        [Fact]
        public void Should_AddDefault()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItem("ITEM1")
                 .AddItem("ITEM2")
                 .AddItem("ITEM3")
                 .AddItem("ITEM4")
                 .AddItem("ITEM5")
                 .AddDefault("ITEM1")
                 .AddDefault("ITEM2");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.True(result.Value.Count() == 2);
                Assert.Contains(result.Value, x => (x ?? string.Empty) == "ITEM1");
                Assert.Contains(result.Value, x => (x ?? string.Empty) == "ITEM2");
                Assert.DoesNotContain(result.Value, x => (x ?? string.Empty) == "ITEM3");
                Assert.DoesNotContain(result.Value, x => (x ?? string.Empty) == "ITEM4");
                Assert.DoesNotContain(result.Value, x => (x ?? string.Empty) == "ITEM5");
            });
        }

        [Fact]
        public void Should_ShowGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)")
                 .AppendGroupOnDescription();
            ctrl.InitControl(CancellationToken.None);

            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("North America"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Asia"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("South America (Any)"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Tokyo"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("New York"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Singapore"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Shanghai"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("Seattle"));
        }


        [Fact]
        public void Should_SelectGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)")
                 .AppendGroupOnDescription();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 3);
            });
        }


        [Fact]
        public void Should_SelectUnselectGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)")
                 .AppendGroupOnDescription();
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 3);
                PromptPlus.InputBuffer(" ");
                result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 0);
            });
        }


        [Fact]
        public void Should_ErrorSelectGroupWithRanger()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)")
                 .Range(1,2);
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 0);
            });
        }

        [Fact]
        public void Should_SelectAllGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" })
                 .AddItem("South America (Any)");
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 7);
            });
        }

        [Fact]
        public void Should_ErrorSelectAllGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .Range(0, 3)
                 .AddItem("South America (Any)")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 0);
            });
        }

        [Fact]
        public void Should_ErrorInvertSelectAllGrouped()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P")
                 .Range(0, 3)
                 .AddItem("South America (Any)")
                 .AddItemsGrouped("North America", new[] { "Seattle", "Boston", "New York" })
                 .AddItemsGrouped("Asia", new[] { "Tokyo", "Singapore", "Shanghai" });
            ctrl.InitControl(CancellationToken.None);

            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(" ");
                var result = ctrl.TryResult(CancellationToken.None);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.NotNull(ctrl.ValidateError);
                Assert.True(result.Value.Count() == 1);
            });
        }

        [Fact]
        public void Should_WithScoped()
        {
            var datasample = LoadData();
            var expectedsel = datasample.Where(x => x.IsSeleted).Count() - datasample.Where(x => x.IsHide && x.IsSeleted).Count();
            var ctrl = (MultiSelectControl<MyClass>)PromptPlus.MultiSelect<MyClass>("MultiSelect")
                .AddItems(datasample.Where(x => x.IsSeleted), selected: true)
                .AddItems(datasample.Where(x => !x.IsSeleted))
                .AddItemsTo(AdderScope.Disable, datasample.Where(x => x.IsDisabled))
                .AddItemsTo(AdderScope.Remove, datasample.Where(x => x.IsHide))
                .TextSelector(x => x.MyText!)
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .ChangeDescription(x => x.MyDesc!)
                .AddDefault(datasample.Where(x => x.IsSeleted).ToArray());
            ctrl.InitControl(CancellationToken.None);


            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(Environment.NewLine);
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.False(result.IsRunning);
                Assert.True(result.Value.Count() == expectedsel);
            });
        }

        [Fact]
        public void Should_WithFixedSelectedAndFixedUnselect()
        {
            var datasample = LoadData();
            var expectedsel = datasample.Where(x => x.IsSeleted).Count() - datasample.Where(x => x.IsHide && x.IsSeleted).Count();
            var ctrl = (MultiSelectControl<MyClass>)PromptPlus.MultiSelect<MyClass>("MultiSelect")
                .AddItems(datasample.Where(x => x.IsSeleted), selected: true)
                .AddItems(datasample.Where(x => !x.IsSeleted))
                .AddItemsTo(AdderScope.Disable, datasample.Where(x => x.IsDisabled))
                .AddItemsTo(AdderScope.Remove, datasample.Where(x => x.IsHide))
                .TextSelector(x => x.MyText!)
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .ChangeDescription(x => x.MyDesc!)
                .AddDefault(datasample.Where(x => x.IsSeleted).ToArray());
            ctrl.InitControl(CancellationToken.None);


            CompletesIn(100, () =>
            {
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F2, false, false, false));
                var result = ctrl.TryResult(CancellationToken.None);
                Assert.True(result.Value.Count() == 5);
                PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
                result = ctrl.TryResult(CancellationToken.None);
                Assert.False(result.IsAborted);
                Assert.True(result.IsRunning);
                Assert.True(result.Value.Count() == 1);
            });
        }


        [Fact]
        public void Should_SelectEnum()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select");
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("None"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option three"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option two"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option one"));

        }

        [Fact]
        public void Should_SelectScopeEnumRemove1()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select")
                .AddItemTo(AdderScope.Remove, MyEnum.None);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("None"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option three"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option two"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option one"));

        }

        [Fact]
        public void Should_SelectScopeEnumRemove2()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select")
                .AddItemsTo(AdderScope.Remove, new MyEnum[] { MyEnum.None, MyEnum.Opc1 });
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("None"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option three"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option two"));
            Assert.DoesNotContain(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option one"));

        }

        [Fact]
        public void Should_SelectScopeEnumDisable()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select")
                .AddItemTo(AdderScope.Disable, MyEnum.None);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("", init);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("None"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option three"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option two"));
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("option one"));
        }


        [Fact]
        public void Should_SelectEnumWithDefault()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select")
                .AddDefault(MyEnum.Opc3);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("option three", init);
        }

        [Fact]
        public void Should_SelectEnumWithTextSelector()
        {
            var ctrl = (MultiSelectControl<MyEnum>)PromptPlus.MultiSelect<MyEnum>("Select")
                .TextSelector((item) => item.ToString())
                .AddDefault(MyEnum.Opc3);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Opc3", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory1()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(new List<string> { "Seattle" }), TimeSpan.FromSeconds(30), null);
            FileHistory.SaveHistory(namehist, hist);

            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItems(TestCity())
                .AddDefault("London")
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
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItems(TestCity())
                .AddDefault("London")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("London", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory3()
        {
            //exit file with timeout
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var hist = FileHistory.AddHistory(JsonSerializer.Serialize(new List<string> { "Seattle" }), TimeSpan.FromMilliseconds(1), null);
            FileHistory.SaveHistory(namehist, hist);

            Thread.Sleep(2);

            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                .AddItems(TestCity())
                .AddDefault("London")
                .OverwriteDefaultFrom(namehist);
            var init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("London", init);
        }


        [Fact]
        public void Should_ValidInitControlPromptOverwriteDefaultHistory4()
        {
            var namehist = "InitInputOverwriteDefaultHistory1";
            FileHistory.ClearHistory(namehist);
            var ctrl = (MultiSelectControl<string>)PromptPlus
                .MultiSelect<string>("P", "D")
                 .AddItems(TestCity())
                 .AddDefault("London")
                 .OverwriteDefaultFrom(namehist);

            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("London", init);
            var sb = new ScreenBuffer();
            ctrl.FinishTemplate(sb,new string[] { "Seattle" }, false);

            ctrl = (MultiSelectControl<string>)PromptPlus
                 .MultiSelect<string>("P", "D")
                 .AddItems(TestCity())
                 .AddDefault("London")
                 .OverwriteDefaultFrom(namehist);

            init = ctrl.InitControl(CancellationToken.None);

            FileHistory.ClearHistory(namehist);

            Assert.Equal("Seattle", init);
        }


        [Fact]
        public void Should_FinishTemplate()
        {
            var ctrl = (MultiSelectControl<string>)PromptPlus.MultiSelect<string>("P", "D")
                .AddItems(TestCity())
                .AddDefault("London", "Seattle");
            ctrl.InitControl(CancellationToken.None);
            var sb = new ScreenBuffer();
            ctrl.InputTemplate(sb);
            ctrl.FinishTemplate(sb, new string[] { "London", "Seattle" }, false);
            Assert.Contains(sb.Buffer, x => (x.Text ?? string.Empty).Contains("London, Seattle"));
        }


        [Fact]
        public void Should_ValidInitControlPromptOrderBy()
        {
            var ctrl = (MultiSelectControl<MyClass>)PromptPlus.MultiSelect<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .OrderBy((item) => item.MyText!)
                .AddItems(TestColletionMyClass(3).OrderByDescending(x => x.MyText),false,true);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item0, Item1, Item2", init);
        }

        [Fact]
        public void Should_ValidInitControlPromptOrderByDescending()
        {
            var ctrl = (MultiSelectControl<MyClass>)PromptPlus.MultiSelect<MyClass>("P", "D")
                .EqualItems((item1, item2) => item1.Id == item2.Id)
                .TextSelector((item) => item.MyText!)
                .OrderByDescending((item) => item.MyText!)
                .AddItems(TestColletionMyClass(3),false,true);
            var init = ctrl.InitControl(CancellationToken.None);
            Assert.Equal("Item2, Item1, Item0", init);
        }

    }
}
