using System.IO;

using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Objects
{
    public class ResultBrowserTest
    {

        [Fact]
        public void Should_have_folder_value()
        {
            // Given
            var rb = new ResultBrowser("folder", "file", false);
            // Then
            Assert.True(rb.AliasSelected == rb.SelectedValue);
        }

        [Theory]
        [InlineData(true, "folder", "file")]
        [InlineData(false, "folder", "file")]
        public void Should_have_file_value(bool showfile,string folder,string file)
        {
            // Given
            var rb = new ResultBrowser(folder, file, false,false,showfile);
            // Then
            Assert.True(rb.SelectedValue == "file");
            if (!showfile)
            {
                Assert.EndsWith(file, rb.AliasSelected);
            }
            else
            {
                Assert.EndsWith(Path.Combine(folder, file), rb.AliasSelected);
            }
        }




        [Theory]
        [InlineData(-1, false, "msg", null)]
        [InlineData(101, true, "msg", 10)]
        public void Should_have_exception(int percentValue, bool finished, string message, object interationId)
        {
            // Given
            var ex = Record.Exception(() =>
            {
                var pbi = new ProgressBarInfo(percentValue, finished, message, interationId);
            });
            // Then
            Assert.NotNull(ex);
        }

    }
}
