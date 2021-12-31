using System;
using System.Globalization;
using System.Runtime.InteropServices;

using PPlus.Internal;
using PPlus.Tests.Personas;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class MaskedBufferDateTimeTest
    {

        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11:34:56 PM", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12/2021 21:34:56", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("13/31/2021 10:34:56 PM", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/13/2021 11:34:56 PM", false));
            //then
            Assert.Equal("", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(0, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_en_us_without_fill()
        {
            // Given
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_en_us_with_fill()
        {
            // Given
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //then
            Assert.Equal("12/31/2021 11:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12/2021 23", false));
            //then
            Assert.Equal("31/12/2021 23", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);

            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("pt-BR"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("31/12/2021 23", false));
            //then
            Assert.Equal("31/12/2021 23:00:00", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);

            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Clear_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.Clear();
            //then
            Assert.Equal("00/00/0000 00:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_ToEnd_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_accept_ToStart_ToEnd_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31/2021 11:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31/2021 11:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(13, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_Forward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.Forward();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);

            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToEnd_Backward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Backward();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(9, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_ToForwardString()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            var aux1 = maskedBuffer.ToBackwardString();
            var aux2 = maskedBuffer.ToForwardString();
            //then
            Assert.Equal("12/", aux1);
            Assert.StartsWith("31/2021 11", aux2);
        }


        [Fact]
        internal void Should_have_accept_Delete_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/12/0211 1 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(9, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Delete_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("12/12/0211 10:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(14, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_without_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('x', out var ok);
            maskedBuffer.Insert('0', out _);
            //then
            Assert.False(ok);
            Assert.Equal("12/31/2021 11:20 AM", maskedBuffer.ToMasked());
            Assert.Equal(12, maskedBuffer.Position);
            Assert.Equal(12, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_with_fill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Insert('0', out _);
            maskedBuffer.Insert('x', out var ok);
            maskedBuffer.Insert('1', out _);
            //then
            Assert.False(ok);
            Assert.Equal("01/31/2021 11:00:00 AM", maskedBuffer.ToMasked());
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Insert('0', out _);
            maskedBuffer.Insert('1', out _);
            //then
            Assert.Equal("01/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_sep_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditDateTime(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11:34:56 PM", false));
            //when
            maskedBuffer.ToStart();
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("12/31/2021 11", false));
            //when
            maskedBuffer.Insert('P', out _);
            //then
            Assert.Equal("12/31/2021 11 PM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('A', out _);
            //then
            Assert.Equal("12/31/2021 11 AM", maskedBuffer.ToMasked());
            Assert.Equal(10, maskedBuffer.Position);
            Assert.Equal(10, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
    }
}
