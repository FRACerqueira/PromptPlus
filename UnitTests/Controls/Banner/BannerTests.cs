// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls.Banner
{
    public  class BannerTests : BaseTest
    {
        [Fact]
        public void Should_LoadFontWithNoError()
        {
            var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
            ctrl.LoadFont("starwars.flf");
        }

        [Fact]
        public void Should_LoadFontStreamWithNoError()
        {
            var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
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
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = true;
            });
            var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
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
            PromptPlus.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported= false;
            });
            var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
            ctrl.Run(null, value);
        }

        [Fact]
        public void Should_Runoutput1()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = true;
            });
            var output = PromptPlus.RecordOutput(() =>
            {
                var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
                ctrl.Run();
            });
            Assert.Equal(Expectations.GetVerifyAnsi("Banner.txt"), output);
        }


        [Fact]
        public void Should_Runoutput2()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            var output = PromptPlus.RecordOutput(() =>
            {
                var ctrl = new BannerControl(PromptPlus.Config, PromptPlus._consoledrive, "Test");
                ctrl.Run();
            });
            Assert.Equal(Expectations.GetVerifyStd("Banner.txt"), output);
        }
    }
}
