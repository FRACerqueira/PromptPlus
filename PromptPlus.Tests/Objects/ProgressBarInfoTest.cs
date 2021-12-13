using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Objects
{
    public class CSIAnsiConsoleTest
    {

        [Theory]
        [InlineData(0, false, "msg", null)]
        [InlineData(100, true, "msg", 10)]
        public void Should_have_value(int percentValue, bool finished, string message, object interationId)
        {
            // Given
            var pbi = new ProgressBarInfo(percentValue,finished,message,interationId);
            // When
            //none
            // Then
            Assert.True(pbi.PercentValue == percentValue);
            Assert.True(pbi.Finished == finished);
            Assert.True(pbi.Message == message);
            Assert.True(pbi.InterationId == interationId);
        }

        [Theory]
        [InlineData(-1, false, "msg", null)]
        [InlineData(101, true, "msg", 10)]
        public void Should_have_exception(int percentValue, bool finished, string message, object interationId)
        {
            // Given When
            var ex = Record.Exception(() =>
            {
                var pbi = new ProgressBarInfo(percentValue, finished, message, interationId);
            });
            // Then
            Assert.NotNull(ex);
        }

    }
}
