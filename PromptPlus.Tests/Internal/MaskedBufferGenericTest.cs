using System;

using PPlus.Drivers;
using PPlus.Internal;
using PPlus.Tests.Personas;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class MaskedBufferGenericTest
    {
        private const string MaskTest = @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}";
        private MaskedBuffer _maskedBuffer;

        public MaskedBufferGenericTest()
        {
            _maskedBuffer = new MaskedBuffer(new OptionsForMaskeditGeneric(MaskTest, null));
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA-AX-QQQ", false));
            //then
            Assert.Equal("XYZ 222-AAA-AX-QQQ",_maskedBuffer.ToMasked());
            Assert.Equal(10, _maskedBuffer.Position);
            Assert.Equal(11, _maskedBuffer.Length);
            Assert.True(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_not_accept_Load_invalidvalues()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ AAA-AAA-AX-QQQ", false));
            //then
            Assert.Equal("", _maskedBuffer.ToMasked());
            Assert.Equal(0, _maskedBuffer.Position);
            Assert.Equal(0, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.True(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_Load_partial_validvalues()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(6, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Clear()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.Clear();
            //then
            Assert.Equal("", _maskedBuffer.ToMasked());
            Assert.Equal(0, _maskedBuffer.Position);
            Assert.Equal(0, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.True(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_ToEnd()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToStart();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(0, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.True(_maskedBuffer.IsStart);
            //when
            _maskedBuffer.ToEnd();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(6, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToStart_Forward()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToStart();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(0, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.True(_maskedBuffer.IsStart);
            //when
            _maskedBuffer.Forward();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(1, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_ToEnd_Backward()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToEnd();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(6, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
            //when
            _maskedBuffer.Backward();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(5, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_ToStart_ToForwardString()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToStart();
            _maskedBuffer.Forward();
            _maskedBuffer.Forward();
            _maskedBuffer.Forward();
            //then
            Assert.Equal("XYZ 222-AAA", _maskedBuffer.ToMasked());
            Assert.Equal(3, _maskedBuffer.Position);
            Assert.Equal(6, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
            //when
            var aux1 = _maskedBuffer.ToBackwardString();
            var aux2 = _maskedBuffer.ToForwardString();
            //then
            Assert.Equal("XYZ 222-", aux1);
            Assert.StartsWith("AAA", aux2);
        }


        [Fact]
        internal void Should_have_accept_Delete()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToStart();
            _maskedBuffer.Forward();
            _maskedBuffer.Forward();
            _maskedBuffer.Forward();
            _maskedBuffer.Delete();
            //then
            Assert.Equal("XYZ 222-AA", _maskedBuffer.ToMasked());
            Assert.Equal(3, _maskedBuffer.Position);
            Assert.Equal(5, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }

        [Fact]
        internal void Should_have_accept_insert_valid_reject_invalid()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA", true));
            //when
            _maskedBuffer.ToEnd();
            _maskedBuffer.Insert('B', out _);
            _maskedBuffer.Insert('+', out var ok);
            _maskedBuffer.Insert('X', out _);
            //then
            Assert.False(ok);
            Assert.Equal("XYZ 222-AAA-BX", _maskedBuffer.ToMasked());
            Assert.Equal(8, _maskedBuffer.Position);
            Assert.Equal(8, _maskedBuffer.Length);
            Assert.False(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }


        [Fact]
        internal void Should_have_accept_Load_validvalues_and_insert_valid()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA-AX-QQQ", false));
            //when
            _maskedBuffer.Insert('W', out _);
            //then
            Assert.Equal("XYZ 222-AAA-AX-QQW", _maskedBuffer.ToMasked());
            Assert.Equal(10, _maskedBuffer.Position);
            Assert.Equal(11, _maskedBuffer.Length);
            Assert.True(_maskedBuffer.IsMaxInput);
            Assert.True(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }
        [Fact]
        internal void Should_have_accept_Load_validvalues__home_and_insert_valid()
        {
            // Given
            _maskedBuffer.Load(_maskedBuffer.PreparationDefaultValue("XYZ 222-AAA-AX-QQQ", false));
            //when
            _maskedBuffer.ToStart();
            _maskedBuffer.Insert('3', out _);
            //then
            Assert.Equal("XYZ 322-AAA-AX-QQQ", _maskedBuffer.ToMasked());
            Assert.Equal(1, _maskedBuffer.Position);
            Assert.Equal(11, _maskedBuffer.Length);
            Assert.True(_maskedBuffer.IsMaxInput);
            Assert.False(_maskedBuffer.IsEnd);
            Assert.False(_maskedBuffer.IsStart);
        }


    }
}
