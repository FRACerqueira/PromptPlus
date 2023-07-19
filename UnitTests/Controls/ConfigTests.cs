using PPlus.Controls;
using PPlus.Tests.Util;
using System.Globalization;

namespace PPlus.Tests.Controls
{
    
    public class ConfigTests : BaseTest
    {
        public ConfigTests()
        {
            if (Directory.Exists("PPlus.Tests.Controls"))
            {
                Directory.Delete("PPlus.Tests.Controls", true);
            }
        }

        [Theory]
        [MemberData(nameof(DataCulture))]
        public void Should_IsImplementedResource(CultureInfo cultureInfo, bool expected)
        {
            // given
            var controlconfig = PromptPlus.Config;
            // When
            controlconfig.DefaultCulture = cultureInfo;
            //THEN
            Assert.Equal(expected, controlconfig.IsImplementedResource);
        }


        [Fact]
        public void Should_SetSymbols()
        {
            PromptPlus.Config.Symbols(SymbolType.MaskEmpty, "a", "b");
            Assert.Equal("a", PromptPlus.Config.Symbols(SymbolType.MaskEmpty).value);
            Assert.Equal("b", PromptPlus.Config.Symbols(SymbolType.MaskEmpty).unicode);
        }

        [Fact]
        public void Should_SetNoChar()
        {
            PromptPlus.Config.NoChar = 'x';
            Assert.Equal('x', PromptPlus.Config.NoChar);
            PromptPlus.Config.NoChar = null;
           PromptPlus.Config.DefaultCulture = new CultureInfo("fr-fr");
            Assert.Equal('N', PromptPlus.Config.NoChar);
        }

        [Fact]
        public void Should_SetSecretChar()
        {
            PromptPlus.Config.SecretChar = 'x';
            Assert.Equal('x', PromptPlus.Config.SecretChar);
            PromptPlus.Config.SecretChar = null;
            PromptPlus.Config.DefaultCulture = new CultureInfo("fr-fr");
            Assert.Equal('#', PromptPlus.Config.SecretChar);
        }

        [Fact]
        public void Should_SetYesChar()
        {
            PromptPlus.Config.YesChar = 'x';
            Assert.Equal('x', PromptPlus.Config.YesChar);
            PromptPlus.Config.YesChar = null;
            PromptPlus.Config.DefaultCulture = new CultureInfo("fr-fr");
            Assert.Equal('Y', PromptPlus.Config.YesChar);
        }

        [Fact]
        public void Should_DefaultHotkeysTooltipKeyPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F1), PromptPlus.Config.TooltipKeyPress);
        }

        [Fact]
        public void Should_DefaultHotkeysPasswordViewPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F2), PromptPlus.Config.PasswordViewPress);
        }


        [Fact]
        public void Should_DefaultHotkeysSelectAllPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F2), PromptPlus.Config.SelectAllPress);
        }


        [Fact]
        public void Should_DefaultHotkeysFullPathPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F2), PromptPlus.Config.FullPathPress);
        }

        [Fact]
        public void Should_DefaultHotkeysEditItemPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F2), PromptPlus.Config.EditItemPress);
        }


        [Fact]
        public void Should_DefaultHotkeysRemoveItemPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F3), PromptPlus.Config.RemoveItemPress);
        }

        [Fact]
        public void Should_DefaultHotkeysInvertSelectedPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F3), PromptPlus.Config.InvertSelectedPress);
        }

        [Fact]
        public void Should_DefaultHotkeysToggleExpandPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F3), PromptPlus.Config.ToggleExpandPress);
        }

        [Fact]
        public void Should_DefaultHotkeysToggleExpandAllPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.F4), PromptPlus.Config.ToggleExpandAllPress);
        }

        [Fact]
        public void Should_DefaultHotkeysAbortKeyPress()
        {
            Assert.Equal(new HotKey(ConsoleKey.Escape), PromptPlus.Config.AbortKeyPress);
        }

        [Fact]
        public void Should_DefaulPropertiesAppCulture()
        {
            Assert.Equal(Thread.CurrentThread.CurrentCulture, PromptPlus.Config.AppCulture);
        }

        [Fact]
        public void Should_DefaulPropertiesCompletionMaxCount()
        {
            Assert.Equal(1000, PromptPlus.Config.CompletionMaxCount);
            PromptPlus.Config.CompletionMaxCount = 0;
            Assert.Equal(1, PromptPlus.Config.CompletionMaxCount);
        }

        [Fact]
        public void Should_DefaulPropertiesCompletionMinimumPrefixLength()
        {
            Assert.Equal(3, PromptPlus.Config.CompletionMinimumPrefixLength);
            PromptPlus.Config.CompletionMinimumPrefixLength = -1;
            Assert.Equal(0, PromptPlus.Config.CompletionMinimumPrefixLength);
        }

        [Fact]
        public void Should_DefaulPropertiesCompletionWaitToStart()
        {
            Assert.Equal(1000, PromptPlus.Config.CompletionWaitToStart);
            PromptPlus.Config.CompletionWaitToStart = 9;
            Assert.Equal(10, PromptPlus.Config.CompletionWaitToStart);
        }

        [Fact]
        public void Should_DefaulPropertiesEnabledAbortKey()
        {
            Assert.True(PromptPlus.Config.EnabledAbortKey);
        }

        [Fact]
        public void Should_DefaulPropertiesHideAfterFinish()
        {
            Assert.False(PromptPlus.Config.HideAfterFinish);
        }

        [Fact]
        public void Should_DefaulPropertiesHideOnAbort()
        {
            Assert.False(PromptPlus.Config.HideOnAbort);
        }

        [Fact]
        public void Should_DefaulPropertiesHistoryTimeout()
        {
            Assert.Equal(TimeSpan.FromDays(365),PromptPlus.Config.HistoryTimeout);
        }

        [Fact]
        public void Should_DefaulPropertiesPageSize()
        {
            Assert.Equal(10, PromptPlus.Config.PageSize);
            PromptPlus.Config.PageSize = 0;
            Assert.Equal(1, PromptPlus.Config.PageSize);
        }

        [Fact]
        public void Should_DefaulPropertiesShowTooltip()
        {
            Assert.True(PromptPlus.Config.ShowTooltip);
        }

        public static IEnumerable<object[]> DataCulture =>
            new List<object[]>
            {
                new object[] { new CultureInfo("en-US"), true },
                new object[] { new CultureInfo("pt-BR"), true },
                new object[] { new CultureInfo("fr-FR"), false},
            };
    }
}
