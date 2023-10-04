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

    public class MaskedBufferTimeOnlyTests : BaseTest
    {
        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34:56 PM", false));
            //then
            Assert.Equal("11:34:56 PM", maskedBuffer.ToMasked());
            Assert.Equal("PM", maskedBuffer.SignalTimeInput);
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Load_validvalues_pt_br()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("23:34:56", false));
            //then
            Assert.Equal("23:34:56", maskedBuffer.ToMasked());
            Assert.Equal("", maskedBuffer.SignalTimeInput);
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_not_accept_Load_invalidvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34:68 PM", false));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("23/34/68", false));
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Clear_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", true));
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
        internal void Should_have_accept_Clear_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", false));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", false));
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
        internal void Should_have_accept_Delete_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", true));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("11:40:0 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(5, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Delete_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("11:40:0 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(5, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }



        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", true));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('X', out var ok);
            //then
            Assert.False(ok);
            Assert.Equal("11:34:02 AM", maskedBuffer.ToMasked());
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_insert_valid_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", true));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('0', out _);
            //then
            Assert.Equal("01:34:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_sep_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34:56", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert(':', out _);
            //then
            Assert.Equal("11:34:56 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert(':', out _);
            //then
            Assert.Equal("11:34:56 AM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert(':', out _);
            //then
            Assert.Equal("11:34:56 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_insert_valid_PM_AM()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("11:34", true));
            //when
            maskedBuffer.Insert('P', out _);
            //then
            Assert.Equal("11:34:00 PM", maskedBuffer.ToMasked());
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('A', out _);
            //then
            Assert.Equal("11:34:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        internal class OptionsForMaskeditTimeOnly : MaskEditOptions
        {
            public OptionsForMaskeditTimeOnly(CultureInfo culture, string? defaultvalue) : base(PromptPlus.StyleSchema, PromptPlus.Config, PromptPlus._consoledrive, false)
            {
                Type = ControlMaskedType.TimeOnly;
                DefaultValue = defaultvalue;
                ShowDayWeek = FormatWeek.Short;
                CurrentCulture = culture;
            }
        }
    }
}
