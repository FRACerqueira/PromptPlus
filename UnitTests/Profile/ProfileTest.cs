using PPlus.Tests.Util;

namespace PPlus.Tests.Profile
{
    
    public class ProfileTest : BaseTest
    {

        [Fact]
        public void Should_Emulator_Running()
        {
            Assert.True(PromptPlus.Console.Provider == "Memory");
        }


        [Fact]
        public void Should_Change_DefaultForegroundColor()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.ForegroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal(ConsoleColor.Red, PromptPlus.Console.ForegroundColor);
        }

        [Fact]
        public void Should_Change_BackgroundColor()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.BackgroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal(ConsoleColor.Red, PromptPlus.Console.BackgroundColor);
        }

        [Fact]
        public void Should_default_DefaultStyle()
        {
            Assert.Equal<ConsoleColor>(PromptPlus.Console.ForegroundColor, PromptPlus.Console.DefaultStyle.Foreground);
            Assert.Equal<ConsoleColor>(PromptPlus.Console.BackgroundColor, PromptPlus.Console.DefaultStyle.Background);
        }

        [Fact]
        public void Should_change_DefaultStyle()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.ForegroundColor = ConsoleColor.Red;
                cfg.BackgroundColor = ConsoleColor.Red;
            });
            // Then
            Assert.Equal<ConsoleColor>(ConsoleColor.Red, PromptPlus.Console.DefaultStyle.Foreground);
            Assert.Equal<ConsoleColor>(ConsoleColor.Red, PromptPlus.Console.DefaultStyle.Background);
        }


        [Fact]
        public void Should_default_BufferHeight()
        {
            Assert.Equal(80, PromptPlus.Console.BufferHeight);
        }

        [Fact]
        public void Should_default_BufferWidth()
        {
            Assert.Equal(132, PromptPlus.Console.BufferWidth);
        }

        [Fact]
        public void Should_EmulatorIsUnicodeSupported()
        {
            Assert.True(PromptPlus.Console.IsUnicodeSupported);
        }

        [Fact]
        public void Should_can_ResetColor()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.OverflowStrategy = Overflow.Ellipsis;
            });
            var def = PromptPlus.Console.DefaultStyle;
            //when
            PromptPlus.Console.ForegroundColor = Color.Blue;
            PromptPlus.Console.BackgroundColor = Color.Red;
            PromptPlus.Console.ResetColor();
            // Then
            Assert.Equal(def, PromptPlus.Console.DefaultStyle);

        }

        [Theory]
        [MemberData(nameof(DataStyle))]
        public void Should_Change_Style(Style expected)
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.OverflowStrategy = expected.OverflowStrategy;
            });
            //when
            PromptPlus.Console.ForegroundColor = expected.Foreground;
            PromptPlus.Console.BackgroundColor = expected.Background;
            // Then
            Assert.Equal<Color>(expected.Foreground, PromptPlus.Console.ForegroundColor);
            Assert.Equal<Color>(expected.Background, PromptPlus.Console.BackgroundColor);
        }

        public static IEnumerable<object[]> DataStyle =>
            new List<object[]>
            {
                        new object[] { Style.Plain },
                        new object[] { Style.Plain.Foreground(Color.Blue).Background(Color.Red).Overflow(Overflow.Ellipsis) },
            };

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_SupportsAnsi(bool expected)
        {
            // When
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = expected;
            });
            // Then
            Assert.Equal(expected, PromptPlus.Console.SupportsAnsi);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_IsTerminal(bool expected)
        {
            // When
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.IsTerminal = expected;
            });
            // Then
            Assert.Equal(expected, PromptPlus.Console.IsTerminal);
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
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.ColorDepth = expected;
            });

            // Then
            Assert.Equal(expected, PromptPlus.Console.ColorDepth);
        }

        [Fact]
        public void Should_Default_OverflowStrategy()
        {
            Assert.Equal(Overflow.None, PromptPlus.Console.OverflowStrategy);
        }


        [Theory]
        [InlineData(Overflow.Crop)]
        [InlineData(Overflow.Ellipsis)]
        [InlineData(Overflow.None)]
        public void Should_Change_OverflowStrategy(Overflow expected)
        {
            // When
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.OverflowStrategy = expected;
            });

            // Then
            Assert.Equal(expected, PromptPlus.Console.OverflowStrategy);
        }

        [Fact]
        public void Should_Default_PadLeft()
        {
            Assert.Equal(0, PromptPlus.Console.PadLeft);
        }

        [Fact]
        public void Should_PadLeft_set()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.PadLeft = 1;
            });
            // Then
            Assert.Equal(1, PromptPlus.Console.PadLeft);
        }


        [Fact]
        public void Should_Default_PadRight()
        {
            Assert.Equal(0, PromptPlus.Console.PadRight);
        }

        [Fact]
        public void Should_Default_PadRight_set()
        {
            // given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.PadRight = 1;
            });
            // Then
            Assert.Equal(1, PromptPlus.Console.PadRight);
        }
    }
}