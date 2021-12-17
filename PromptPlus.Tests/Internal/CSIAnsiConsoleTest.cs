using PPlus.Internal;
using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class CSIAnsiConsoleTest
    {

        [Theory]
        [InlineData("teste", false)]
        [InlineData("teste\n\t\b\r", false)]
        [InlineData("",true)]
        [InlineData(null,true)]
        public void Should_have_value_normal(string value,bool empty)
        {
            // Given
            var csi = CSIAnsiConsole.SplitCommands(value);
            // Then
            if (!empty)
            {
                Assert.True(csi.Length == 1);
                Assert.True(csi[0] == value);
            }
            else
            {
                Assert.True(csi.Length == 0);
            }
        }
        [Theory]
        [InlineData("teste\x1b[1;3mOutrotexto",new string[] { "teste", "\x1b[1;3m", "Outrotexto" })]
        [InlineData("teste\x1b[1;3mOutrotexto\x1b\x1b", new string[] { "teste", "\x1b[1;3m", "Outrotexto", "\x1b" })]
        [InlineData("teste\x1b[1;3mOutrotexto\x1bx", new string[] { "teste", "\x1b[1;3m", "Outrotexto", "\x1bx" })]
        public void Should_have_value_Ansivalue(string value, string[] resuts)
        {
            // Given
            var csi = CSIAnsiConsole.SplitCommands(value);
            // Then
            Assert.True(csi.Length == resuts.Length);
            for (var i = 0; i < csi.Length; i++)
            {
                Assert.True(csi[i] == resuts[i]);
            }
        }

        [Theory]
        [InlineData("teste\x1b[\x2f;10")]
        [InlineData("teste\x1b[0\x1f;10")]
        [InlineData("teste\x1b[10;0\x7f")]
        public void Should_have_Ansivalue_exception(string value)
        {
            // Given
            var ex = Record.Exception(() =>
            {
                CSIAnsiConsole.SplitCommands(value);
            });
            // Then
            Assert.NotNull(ex);
        }
    }
}
