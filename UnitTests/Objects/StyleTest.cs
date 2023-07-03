using PPlus.Drivers;
using PPlus.Tests.Util;
using Shouldly;

namespace PPlus.Tests.Objects
{

    public sealed class StyleTests : BaseTest
    {
        [Fact]
        public void Should_DefaultStyle()
        {
            // Given
            var result = new Style();
            // Then
            result.Foreground.ShouldBe(Color.DefaultForecolor);
            result.Background.ShouldBe(Color.DefaultBackcolor);
            result.OverflowStrategy.ShouldBe(Overflow.None);
        }

        [Fact]
        public void Should_OverflowCrop()
        {
            // Given
            var result = Style.OverflowCrop;
            // Then
            result.Foreground.ShouldBe(Style.Plain.Foreground);
            result.Background.ShouldBe(Style.Plain.Background);
            result.OverflowStrategy.ShouldBe(Overflow.Crop);
        }

        [Fact]
        public void Should_OverflowEllipsis()
        {
            // Given
            var result = Style.OverflowEllipsis;
            // Then
            result.Foreground.ShouldBe(Style.Plain.Foreground);
            result.Background.ShouldBe(Style.Plain.Background);
            result.OverflowStrategy.ShouldBe(Overflow.Ellipsis);
        }

        [Fact]
        public void Should_ApplyOverflowStrategyUnicode()
        {
            // Given
            var result = Style.ApplyOverflowStrategy(0,3,Overflow.Ellipsis,"abcdefgh",true);
            // Then
            Assert.EndsWith(Style.UnicodeEllipsis, result);
        }

        [Fact]
        public void Should_ApplyOverflowStrategyNoUnicode()
        {
            // Given
            var result = Style.ApplyOverflowStrategy(0, 3, Overflow.Ellipsis, "abcdefgh", false);
            // Then
            Assert.EndsWith(Style.AsciiEllipsis, result);
        }

        [Fact]
        public void Should_change_overflow()
        {
            // Given
            var result = Style.Plain.Overflow(Overflow.Ellipsis);
            // Then
            result.OverflowStrategy.ShouldBe(Overflow.Ellipsis);
        }

        [Fact]
        public void Should_change_Foreground()
        {
            // Given
            var result = Style.Plain.Foreground(Color.Red);
            // Then
            result.OverflowStrategy.ShouldBe(Overflow.None);
            result.Foreground.ShouldBe(Color.Red);
        }

        [Fact]
        public void Should_change_Background()
        {
            // Given
            var result = Style.Plain.Background(Color.Red);
            // Then
            result.Background.ShouldBe(Color.Red);
        }


        [Fact]
        public void Should_Combine_Two_Styles_As_Expected()
        {
            // Given
            var first = new Style(Color.White, Color.Yellow);
            var other = new Style(Color.Green, Color.Silver);

            // When
            var result = first.Combine(other);

            // Then
            result.Foreground.ShouldBe(Color.Green);
            result.Background.ShouldBe(Color.Silver);
        }

        [Fact]
        public void Should_Consider_Two_Identical_Styles_Equal()
        {
            // Given
            var first = new Style(Color.White, Color.Yellow);
            var second = new Style(Color.White, Color.Yellow);

            // When
            var result = first.Equals(second);

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Consider_Two_Styles_With_Different_Foreground_Colors_Equal()
        {
            // Given
            var first = new Style(Color.White, Color.Yellow);
            var second = new Style(Color.Blue, Color.Yellow);

            // When
            var result = first.Equals(second);

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Consider_Two_Styles_With_Different_Background_Colors_Equal()
        {
            // Given
            var first = new Style(Color.White, Color.Yellow);
            var second = new Style(Color.White, Color.Blue);

            // When
            var result = first.Equals(second);

            // Then
            result.ShouldBeFalse();
        }


        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Foreground_Is_Set_Twice()
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse("green yellow"));

                // Then
                result.ShouldBeOfType<PromptPlusException>();
                result.Message.ShouldBe("A foreground color has already been set.");
            }

            [Fact]
            public void Should_Throw_If_Background_Is_Set_Twice()
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse("green on blue yellow"));

                // Then
                result.ShouldBeOfType<PromptPlusException>();
                result.Message.ShouldBe("A background color has already been set.");
            }

            [Fact]
            public void Should_Throw_If_Color_Name_Could_Not_Be_Found()
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse("lol"));

                // Then
                result.ShouldBeOfType<PromptPlusException>();
                result.Message.ShouldBe("Could not find color or style 'lol'.");
            }

            [Fact]
            public void Should_Throw_If_Background_Color_Name_Could_Not_Be_Found()
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse("blue on lol"));

                // Then
                result.ShouldBeOfType<PromptPlusException>();
                result.Message.ShouldBe("Could not find color 'lol'.");
            }

            [Theory]
            [InlineData("#FF0000 on #0000FF")]
            [InlineData("#F00 on #00F")]
            public void Should_Parse_Hex_Colors_Correctly(string style)
            {
                // Given, When
                var result = StyleParser.Parse(style);

                // Then
                result.Foreground.ShouldBe(Color.Red);
                result.Background.ShouldBe(Color.Blue);
            }

            [Theory]
            [InlineData("#", "Invalid hex color '#'.")]
            [InlineData("#FF00FF00FF", "Invalid hex color '#FF00FF00FF'.")]
            [InlineData("#FOO", "Invalid hex color '#FOO'. Could not find any recognizable digits.")]
            public void Should_Return_Error_If_Hex_Color_Is_Invalid(string style, string expected)
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse(style));

                // Then
                result.ShouldNotBeNull();
                result.Message.ShouldBe(expected);
            }

            [Theory]
            [InlineData("rgb(255,0,0) on rgb(0,0,255)")]
            public void Should_Parse_Rgb_Colors_Correctly(string style)
            {
                // Given, When
                var result = StyleParser.Parse(style);

                // Then
                result.Foreground.ShouldBe(Color.Red);
                result.Background.ShouldBe(Color.Blue);
            }

            [Theory]
            [InlineData("12 on 24")]
            public void Should_Parse_Colors_Numbers_Correctly(string style)
            {
                // Given, When
                var result = StyleParser.Parse(style);

                // Then
                result.Foreground.ShouldBe(Color.Blue);
                result.Background.ShouldBe(Color.DeepSkyBlue4_1);
            }

            [Theory]
            [InlineData("-12", "Color number must be greater than or equal to 0 (was -12)")]
            [InlineData("256", "Color number must be less than or equal to 255 (was 256)")]
            public void Should_Return_Error_If_Color_Number_Is_Invalid(string style, string expected)
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse(style));

                // Then
                result.ShouldNotBeNull();
                result.Message.ShouldBe(expected);
            }

            [Theory]
            [InlineData("rgb()", "Invalid RGB color 'rgb()'.")]
            [InlineData("rgb(", "Invalid RGB color 'rgb('.")]
            [InlineData("rgb(255)", "Invalid RGB color 'rgb(255)'.")]
            [InlineData("rgb(255,255)", "Invalid RGB color 'rgb(255,255)'.")]
            [InlineData("rgb(255,255,255", "Invalid RGB color 'rgb(255,255,255'.")]
            [InlineData("rgb(A,B,C)", "Invalid RGB color 'rgb(A,B,C)'.")]
            public void Should_Return_Error_If_Rgb_Color_Is_Invalid(string style, string expected)
            {
                // Given, When
                var result = Record.Exception(() => StyleParser.Parse(style));

                // Then
                result.ShouldNotBeNull();
                result.Message.ShouldStartWith(expected);
            }
        }
    }
}
