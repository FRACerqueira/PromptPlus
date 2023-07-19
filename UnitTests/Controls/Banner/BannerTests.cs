using PPlus.Controls;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.Banner
{
    public  class BannerTests : BaseTest
    {
        [Theory]
        [InlineData(CharacterWidth.Full)]
        [InlineData(CharacterWidth.Smush)]
        [InlineData(CharacterWidth.Fitted)]
        public void Should_InitAsciiArtWithNoError(CharacterWidth value)
        {
            var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
            ctrl.FIGletWidth(value);
            ctrl.InitAsciiArt();
        }

        [Fact]
        public void Should_LoadFontWithNoError()
        {
            var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
            ctrl.InitAsciiArt();
            ctrl.LoadFont("starwars.flf");
        }

        [Fact]
        public void Should_LoadFontStreamWithNoError()
        {
            var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
            ctrl.InitAsciiArt();
            using var sr = new FileStream("starwars.flf", FileMode.Open);
            ctrl.LoadFont(sr);
        }

        [Theory]
        [InlineData(BannerDashOptions.AsciiDoubleBorderDown)]
        [InlineData(BannerDashOptions.AsciiDoubleBorderUpDown)]
        [InlineData(BannerDashOptions.AsciiSingleBorderDown)]
        [InlineData(BannerDashOptions.AsciiSingleBorderUpDown)]
        [InlineData(BannerDashOptions.DoubleBorderDown)]
        [InlineData(BannerDashOptions.DoubleBorderUpDown)]
        [InlineData(BannerDashOptions.HeavyBorderDown)]
        [InlineData(BannerDashOptions.HeavyBorderUpDown)]
        [InlineData(BannerDashOptions.SingleBorderDown)]
        [InlineData(BannerDashOptions.SingleBorderUpDown)]
        [InlineData(BannerDashOptions.None)]
        public void Should_RunWithNoErrorUnicode(BannerDashOptions value)
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = true;
            });
            var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
            ctrl.InitAsciiArt();
            ctrl.Run(null, value);
        }

        [Theory]
        [InlineData(BannerDashOptions.AsciiDoubleBorderDown)]
        [InlineData(BannerDashOptions.AsciiDoubleBorderUpDown)]
        [InlineData(BannerDashOptions.AsciiSingleBorderDown)]
        [InlineData(BannerDashOptions.AsciiSingleBorderUpDown)]
        [InlineData(BannerDashOptions.DoubleBorderDown)]
        [InlineData(BannerDashOptions.DoubleBorderUpDown)]
        [InlineData(BannerDashOptions.HeavyBorderDown)]
        [InlineData(BannerDashOptions.HeavyBorderUpDown)]
        [InlineData(BannerDashOptions.SingleBorderDown)]
        [InlineData(BannerDashOptions.SingleBorderUpDown)]
        [InlineData(BannerDashOptions.None)]
        public void Should_RunWithNoErrorNotUnicode(BannerDashOptions value)
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported= false;
            });
            var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
            ctrl.InitAsciiArt();
            ctrl.Run(null, value);
        }

        [Fact]
        public void Should_Runoutput1()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = true;
            });
            var output = PromptPlus.RecordOutput(() =>
            {
                var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
                ctrl.InitAsciiArt();
                ctrl.Run();
            });
            Assert.Equal(Expectations.GetVerifyAnsi("Banner.txt"), output);
        }


        [Fact]
        public void Should_Runoutput2()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            var output = PromptPlus.RecordOutput(() =>
            {
                var ctrl = new BannerControl((IConsoleControl)PromptPlus.Console, "Test");
                ctrl.InitAsciiArt();
                ctrl.Run();
            });
            Assert.Equal(Expectations.GetVerifyStd("Banner.txt"), output);
        }
    }
}
