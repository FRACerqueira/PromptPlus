using System;

using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class ItemHistoryTest
    {
        [Fact]
        public void Should_have_Separator()
        {
            // Given
            var tm = DateTime.Now.Ticks;
            var hv = "test";
            // When
            var h = new ItemHistory(hv, tm);
            // Then
            Assert.Equal(h.ToString(), $"{hv}{ItemHistory.Separator}{tm}");
        }
    }
}
