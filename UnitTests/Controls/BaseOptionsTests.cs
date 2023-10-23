// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{
    
    public class BaseOptionsTests : BaseTest
    {
        public BaseOptionsTests()
        {
            if (Directory.Exists("PPlus.Tests.Controls"))
            {
                Directory.Delete("PPlus.Tests.Controls", true);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Should_InitCursorOptions(bool cursor, bool expected)
        {
            var kp = new OptBaseTest(cursor);
            Assert.Equal(expected, kp.OptShowCursor);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Should_InitHideAfterFinish(bool value, bool expected)
        {
            var kp = new OptBaseTest(false);
            Assert.Equal(PromptPlus.Config.HideAfterFinish, kp.OptHideAfterFinish);
            kp.HideAfterFinish(value);
            Assert.Equal(expected, kp.OptHideAfterFinish);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Should_InitShowTooltip(bool value, bool expected)
        {
            var kp = new OptBaseTest(false);
            Assert.Equal(PromptPlus.Config.ShowTooltip, kp.OptShowTooltip);
            kp.ShowTooltip(value);
            Assert.Equal(expected, kp.OptShowTooltip);
        }
        [Theory]
        [InlineData(StageControl.OnStartControl)]
        [InlineData(StageControl.OnInputRender)]
        [InlineData(StageControl.OnTryAcceptInput)]
        [InlineData(StageControl.OnFinishControl)]
        public void Should_InitAddExtraAction(StageControl value)
        {
            var kp = new OptBaseTest(false);
            string result = ""; 
            kp.AddExtraAction(value, (item1,item2) => result = item1.ToString()!);
            kp.OptUserActions[value].Invoke(value, null);
            Assert.Equal(value.ToString()!, result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(10, 10)]
        [InlineData("teste","teste")]
        public void Should_InitSetContext(object value, object expected)
        {
            var kp = new OptBaseTest(false);
            kp.SetContext(value);
            Assert.Equal(expected, kp.OptContext);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("teste", "teste")]
        public void Should_InitDescriptionString(string value, string expected)
        {
            var kp = new OptBaseTest(false);
            kp.Description(value);
            Assert.Equal(expected, kp.OptDescription);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Should_InitEnabledAbortKey(bool value, bool expected)
        {
            var kp = new OptBaseTest(false);
            Assert.Equal(PromptPlus.Config.EnabledAbortKey, kp.OptEnabledAbortKey);
            kp.EnabledAbortKey(value);
            Assert.Equal(expected, kp.OptEnabledAbortKey);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Should_InitHideOnAbort(bool value, bool expected)
        {
            var kp = new OptBaseTest(false);
            Assert.Equal(PromptPlus.Config.HideOnAbort, kp.OptHideOnAbort);
            kp.HideOnAbort(value);
            Assert.Equal(expected, kp.OptHideOnAbort);
        }



        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("teste", "teste")]
        public void Should_InitTooltipsString(string value, string expected)
        {
            var kp = new OptBaseTest(false);
            kp.Tooltips(value);
            Assert.Equal(expected, kp.OptToolTip);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("teste", "teste")]
        public void Should_InitPromptString(string value, string expected)
        {
            var kp = new OptBaseTest(false);
            kp.Prompt(value);
            Assert.Equal(expected, kp.OptPrompt);
        }


        [Fact]
        public void Should_DefaultSymbolsUnicodeSupported()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = true;
            });
            var kp = new OptBaseTest(false);
            foreach (var item in PromptPlus.Config._globalSymbols.Keys)
            {
                var syb = kp.Symbol(item);
                Assert.Equal(PromptPlus.Config.Symbols(item).unicode,syb);
            }
        }

        [Fact]
        public void Should_DefaultSymbolsNotUnicodeSupported()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = false;
            });
            var kp = new OptBaseTest(false);
            foreach (var item in PromptPlus.Config._globalSymbols.Keys)
            {
                var syb = kp.Symbol(item);
                Assert.Equal(PromptPlus.Config.Symbols(item).value, syb);
            }
        }

        [Fact]
        public void Should_SetSymbolsIsUnicodeSupported()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = true;
            });
            var kp = new OptBaseTest(false);
            kp.Symbols(SymbolType.MaskEmpty,"a","b");
            var syb = kp.Symbol(SymbolType.MaskEmpty);
            Assert.Equal("b", syb);
        }

        [Fact]
        public void Should_SetSymbolsNotIsUnicodeSupported()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = false;
            });
            var kp = new OptBaseTest(false);
            kp.Symbols(SymbolType.MaskEmpty, "a", "b");
            var syb = kp.Symbol(SymbolType.MaskEmpty);
            Assert.Equal("a", syb);
        }

        private class OptBaseTest : BaseOptions
        {
            public OptBaseTest(bool showcursor) : base(PromptPlus.StyleSchema, PromptPlus.Config, PromptPlus._consoledrive,  showcursor)
            {
            }
        }
    }
}
