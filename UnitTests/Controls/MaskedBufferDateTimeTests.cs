// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus.Controls;
using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{

    public class MaskedBufferDateTimeTests : BaseTest
    {
        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 11:34:56 PM", false));
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Load_validvalues_pt_br()
        {
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/12/2021 21:34:56", false));
            //then
            Assert.Equal("31/12/2021 21:34:56", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);

        }

        [Fact]
        internal void Should_have_not_accept_Load_invalidvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("13/31/2021 10:34:56 PM", false));
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_not_accept_Load_invalidvalues_pt_br()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/13/2021 11:34:56 PM", false));
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Clear_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 11", false));
            //when
            maskedBuffer.Clear();
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_accept_Clear_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 11", false));
            //when
            maskedBuffer.Clear();
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToHome_ToEnd()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021", false));
            //when
            maskedBuffer.ToHome();
            //then
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.True(maskedBuffer.IsEnd);
        }


        [Fact]
        internal void Should_have_accept_Delete_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 10:23:45 PM", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/12/0211 02:34:5 PM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(13, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 10:23:45 PM", false));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('x', out var ok);
            maskedBuffer.Insert('0', out _);
            //then
            Assert.False(ok);
            Assert.Equal("12/31/2021 10:23:40 PM", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 10:23:45 PM", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('0', out _);
            maskedBuffer.Insert('x', out var ok);
            maskedBuffer.Insert('1', out _);
            //then
            Assert.False(ok);
            Assert.Equal("01/31/2021 10:23:45 PM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_insert_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 10:23:45 PM", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('0', out _);
            maskedBuffer.Insert('1', out _);
            //then
            Assert.Equal("01/31/2021 10:23:45 PM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_sep_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 11:34:56 PM", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert(':', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(8, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            maskedBuffer.Insert(':', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //then
            maskedBuffer.Insert(':', out _);
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(12, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //then
            maskedBuffer.Insert(':', out _);
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(8, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021 11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_PM_AM()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021 11:00:00", false));
            //when
            maskedBuffer.Insert('P', out _);
            //then
            Assert.Equal("12/31/2021 11:00:00 PM", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('A', out _);
            //then
            Assert.Equal("12/31/2021 11:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
        internal class OptionsForMaskeditDateTime : MaskEditOptions
        {
            public OptionsForMaskeditDateTime(CultureInfo culture, string? defaultvalue) : base(PromptPlus.StyleSchema, PromptPlus.Config, PromptPlus._consoledrive, false)
            {
                Type = ControlMaskedType.DateTime;
                DefaultValue = defaultvalue;
                ShowDayWeek = FormatWeek.Short;
                CurrentCulture = culture;
            }
        }
    }
}
