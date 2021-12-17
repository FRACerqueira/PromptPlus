using System;
using System.Globalization;

using PPlus.Drivers;
using PPlus.Internal;
using PPlus.Tests.Personas;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class MaskedBufferDateOnlyTest
    {

        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12/2021", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/31/2021", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/31/2021", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_en_us_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //then
            Assert.Equal("12/31/0000", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12", true));
            //then
            Assert.Equal("31/12", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("pt-BR"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12", true));
            //then
            Assert.Equal("31/12/0000", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Clear_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.Clear();
            //then
            Assert.Equal("00/00/0000", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_ToEnd_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_ToEnd_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31/0000", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31/0000", maskedBuffer.ToMasked());
            Assert.Equal(7, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_Forward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.Forward();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToEnd_Backward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Backward();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(3, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_ToForwardString()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            //then
            Assert.Equal("12/31", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
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
        internal void Should_have_accept_Delete_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/1", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(3, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Delete_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/10/0000", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31", true));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('A', out var ok);
            maskedBuffer.Insert('0', out _);
            //then
            Assert.False(ok);
            Assert.Equal("12/31/20", maskedBuffer.ToMasked());
            Assert.Equal(6, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null,true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("01/31", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Insert('1', out _);
            maskedBuffer.Insert('x', out var ok);
            maskedBuffer.Insert('0', out _);
            //then
            Assert.False(ok);
            Assert.Equal("10/31/0000", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(8, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_and_insert_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021", false));
            //when
            maskedBuffer.ToStart();
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021", false));
            //when
            maskedBuffer.ToStart();
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
    }
}
