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

    public class MaskedBufferDateOnlyTests : BaseTest
    {
        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021", false));
            //then
            Assert.Equal("12/31/2021", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Load_validvalues_pt_br()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/12/2021", false));
            //then
            Assert.Equal("31/12/2021", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_not_accept_Load_invalidvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/31/2021", false));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/31/2021", false));
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_en_us_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", false));
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("31/12", false));
            //then
            Assert.Equal($"31/12/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Clear()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToHome();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToHome_Forward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToHome();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.Forward();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToEnd_Backward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Backward();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(6, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToHome_ToForwardString()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            //then
            Assert.Equal($"12/31/{DateTime.Now.Year}", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            var aux1 = maskedBuffer.ToBackwardString();
            var aux2 = maskedBuffer.ToForwardString();
            //then
            Assert.Equal("12/", aux1);
            Assert.StartsWith("31", aux2);
        }


        [Fact]
        internal void Should_have_accept_Delete()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/12/023", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(7, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31", true));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('A', out var ok);
            maskedBuffer.Insert('0', out _);
            //then
            Assert.False(ok);
            Assert.Equal("12/31/2020", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

 
        [Fact]
        internal void Should_have_accept_Load_validvalues_and_insert_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021", false));
            maskedBuffer.ToEnd();
            //when
            maskedBuffer.Insert('0', out _);
            //then
            Assert.Equal("12/31/2020", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_insert_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('0', out _);
            //then
            Assert.Equal("02/31/2021", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_sep_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.RemoveMask("12/31/2021", false));
            //when
            maskedBuffer.ToHome();
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('/', out _);
            //then
            Assert.Equal("12/31/2021", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        internal class OptionsForMaskeditDateOnly : MaskEditOptions
        {
            public OptionsForMaskeditDateOnly(CultureInfo culture, string defaultvalue) : base(PromptPlus.StyleSchema, PromptPlus.Config, PromptPlus._consoledrive, false)
            {
                Type = ControlMaskedType.DateOnly;
                DefaultValue = defaultvalue;
                ShowDayWeek = FormatWeek.Short;
                CurrentCulture = culture;
            }
        }
    }
}
