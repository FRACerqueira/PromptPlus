using PPlus.Tests.Util;

namespace PPlus.Tests.Profile
{
    
    public class ProfileTest : BaseTest
    {

        [Fact]
        public void Should_Emulator_Running()
        {
            Assert.True(PromptPlus.Provider == "Memory");
        }


        [Fact]
        public void Should_Change_DefaultForegroundColor()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.ForegroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal(ConsoleColor.Red, PromptPlus.ForegroundColor);
        }

        [Fact]
        public void Should_Change_BackgroundColor()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.BackgroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal(ConsoleColor.Red, PromptPlus.BackgroundColor);
        }

        [Fact]
        public void Should_default_DefaultStyle()
        {
            Assert.Equal<ConsoleColor>(PromptPlus.ForegroundColor, Style.Default.Foreground);
            Assert.Equal<ConsoleColor>(PromptPlus.BackgroundColor, Style.Default.Background);
        }

        [Fact]
        public void Should_change_DefaultStyle()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.ForegroundColor = ConsoleColor.Red;
                cfg.BackgroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal<ConsoleColor>(ConsoleColor.Red, Style.Default.Foreground);
            Assert.Equal<ConsoleColor>(ConsoleColor.Red, Style.Default.Background);
        }


        [Fact]
        public void Should_default_BufferHeight()
        {
            Assert.Equal(80, PromptPlus.BufferHeight);
        }

        [Fact]
        public void Should_default_BufferWidth()
        {
            Assert.Equal(132, PromptPlus.BufferWidth);
        }

        [Fact]
        public void Should_EmulatorIsUnicodeSupported()
        {
            Assert.True(PromptPlus.IsUnicodeSupported);
        }

        [Fact]
        public void Should_can_ResetColor()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.OverflowStrategy = Overflow.Ellipsis;
            });
            var def = Style.Default;
            //when
            PromptPlus.ForegroundColor = Color.Blue;
            PromptPlus.BackgroundColor = Color.Red;
            PromptPlus.ResetColor();
            // Then
            Assert.Equal(def, Style.Default);

        }

        [Theory]
        [MemberData(nameof(DataStyle))]
        public void Should_Change_Style(Style expected)
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.OverflowStrategy = expected.OverflowStrategy;
            });
            //when
            PromptPlus.ForegroundColor = expected.Foreground;
            PromptPlus.BackgroundColor = expected.Background;
            // Then
            Assert.Equal<Color>(expected.Foreground, PromptPlus.ForegroundColor);
            Assert.Equal<Color>(expected.Background, PromptPlus.BackgroundColor);
        }

        public static IEnumerable<object[]> DataStyle =>
            new List<object[]>
            {
                        new object[] { Style.Default },
                        new object[] { Style.Default.Foreground(Color.Blue).Background(Color.Red).Overflow(Overflow.Ellipsis) },
            };

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_SupportsAnsi(bool expected)
        {
            // When
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = expected;
            });
            // Then
            Assert.Equal(expected, PromptPlus.SupportsAnsi);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_IsTerminal(bool expected)
        {
            // When
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsTerminal = expected;
            });
            // Then
            Assert.Equal(expected, PromptPlus.IsTerminal);
        }


        [Theory]
        [InlineData(ColorSystem.EightBit)]
        [InlineData(ColorSystem.Legacy)]
        [InlineData(ColorSystem.NoColors)]
        [InlineData(ColorSystem.Standard)]
        [InlineData(ColorSystem.TrueColor)]
        public void Should_Change_ColorDepth(ColorSystem expected)
        {
            // When
            PromptPlus.Setup((cfg) =>
            {
                cfg.ColorDepth = expected;
            });

            // Then
            Assert.Equal(expected, PromptPlus.ColorDepth);
        }

        [Fact]
        public void Should_Default_OverflowStrategy()
        {
            Assert.Equal(Overflow.None, PromptPlus.OverflowStrategy);
        }


        [Theory]
        [InlineData(Overflow.Crop)]
        [InlineData(Overflow.Ellipsis)]
        [InlineData(Overflow.None)]
        public void Should_Change_OverflowStrategy(Overflow expected)
        {
            // When
            PromptPlus.Setup((cfg) =>
            {
                cfg.OverflowStrategy = expected;
            });

            // Then
            Assert.Equal(expected, PromptPlus.OverflowStrategy);
        }

        [Fact]
        public void Should_Default_PadLeft()
        {
            Assert.Equal(0, PromptPlus.PadLeft);
        }

        [Fact]
        public void Should_PadLeft_set()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 1;
            });
            // Then
            Assert.Equal(1, PromptPlus.PadLeft);
        }


        [Fact]
        public void Should_Default_PadRight()
        {
            Assert.Equal(0, PromptPlus.PadRight);
        }

        [Fact]
        public void Should_Default_PadRight_set()
        {
            // given
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadRight = 1;
            });
            // Then
            Assert.Equal(1, PromptPlus.PadRight);
        }
    }
}