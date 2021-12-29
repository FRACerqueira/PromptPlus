using System.Globalization;

using PPlus.Internal;
using PPlus.Tests.Personas;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class MaskedBufferTimeOnlyTest
    {

        [Fact]
        internal void Should_have_accept_Load_validvalues_en_us()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34:56 PM", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("23:34:56", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34:68 PM", false));
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("23/34/68", false));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal("", maskedBuffer.SignalTimeInput);
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //then
            Assert.Equal("11:34:00 AM", maskedBuffer.ToMasked());
            Assert.Equal("", maskedBuffer.SignalTimeInput);
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues_pt_br_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("pt-BR"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("23:34", true));
            // then
            Assert.Equal("23:34", maskedBuffer.ToMasked());
            Assert.Equal("", maskedBuffer.SignalTimeInput);
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("pt-BR"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("23:34", true));
            // then
            Assert.Equal("23:34:00", maskedBuffer.ToMasked());
            Assert.Equal("", maskedBuffer.SignalTimeInput);
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Clear_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.Clear();
            //then
            Assert.Equal("00:00:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_ToEnd()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11/34", true));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_Forward()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11/34", true));
            //when
            maskedBuffer.ToStart();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(0, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.True(maskedBuffer.IsStart);
            //when
            maskedBuffer.Forward();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11/34", true));
            //when
            maskedBuffer.ToEnd();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Backward();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
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
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11/34", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            var aux1 = maskedBuffer.ToBackwardString();
            var aux2 = maskedBuffer.ToForwardString();
            //then
            Assert.Equal("11:", aux1);
            Assert.StartsWith("34", aux2);
        }


        [Fact]
        internal void Should_have_accept_Delete_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("11:4 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(3, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Delete_withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Forward();
            maskedBuffer.Forward();
            maskedBuffer.Delete();
            //then
            Assert.Equal("11:40:00 AM", maskedBuffer.ToMasked());
            Assert.Equal(2, maskedBuffer.Position);
            Assert.Equal(6, maskedBuffer.Length);
            Assert.True(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }



        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid_withoutFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.ToEnd();
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('X', out var ok);
            //then
            Assert.False(ok);
            Assert.Equal("11:34:2 AM", maskedBuffer.ToMasked());
            Assert.Equal(5, maskedBuffer.Position);
            Assert.Equal(5, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid__withFill()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null, true));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.Insert('2', out _);
            maskedBuffer.Insert('X', out var ok);
            //then
            Assert.False(ok);
            Assert.Equal("11:34:20 AM", maskedBuffer.ToMasked());
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.ToStart();
            maskedBuffer.Insert('0', out _);
            //then
            Assert.Equal("01:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(1, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.False(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues_home_and_sep_valid()
        {
            // Given
            var maskedBuffer = new MaskedBuffer(new OptionsForMaskeditTimeOnly(new CultureInfo("en-US"), null));
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34:56", false));
            //when
            maskedBuffer.ToStart();
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
            maskedBuffer.Load(maskedBuffer.PreparationDefaultValue("11:34", true));
            //when
            maskedBuffer.Insert('P', out _);
            //then
            Assert.Equal("11:34 PM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
            //when
            maskedBuffer.Insert('A', out _);
            //then
            Assert.Equal("11:34 AM", maskedBuffer.ToMasked());
            Assert.Equal(4, maskedBuffer.Position);
            Assert.Equal(4, maskedBuffer.Length);
            Assert.False(maskedBuffer.IsMaxInput);
            Assert.True(maskedBuffer.IsEnd);
            Assert.False(maskedBuffer.IsStart);
        }
    }
}
