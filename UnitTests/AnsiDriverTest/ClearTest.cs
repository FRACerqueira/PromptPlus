using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class ClearTest : BaseTest
    {
        [Fact]
        public void Should_Position_Default()
        {
            int cursorleft = 0;
            int cursortop = 0;
            int afterclearcursorleft = 0;
            int afterclearcursortop = 0;

            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Write("TEST\nNEWLINE");
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
                PromptPlus.Clear();
                afterclearcursorleft = PromptPlus.CursorLeft;
                afterclearcursortop = PromptPlus.CursorTop;

            });
            // Then
            Assert.Equal(7, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal(0, afterclearcursorleft);
            Assert.Equal(0, afterclearcursortop);
        }

        [Fact]
        public void Should_Position_padleft()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
            });
            // When
            int cursorleft = 0;
            int cursortop = 0;
            int afterclearcursorleft = 0;
            int afterclearcursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Write("TEST\nNEWLINE");
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
                PromptPlus.Clear();
                afterclearcursorleft = PromptPlus.CursorLeft;
                afterclearcursortop = PromptPlus.CursorTop;
            });
            // Then
            Assert.Equal(9, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal(2, afterclearcursorleft);
            Assert.Equal(0, afterclearcursortop);

        }
    }
}
