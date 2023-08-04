using PPlus.Controls;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{
    
    public class StyleSchemaTest : BaseTest
    {
        public StyleSchemaTest()
        {
            if (Directory.Exists("PPlus.Tests.Controls"))
            {
                Directory.Delete("PPlus.Tests.Controls", true);
            }
        }

        [Fact]
        public void Should_InitDefaultStyleSchema()
        {
            var ss = new StyleSchema();
            Assert.Equal(Style.Default.Foreground(ConsoleColor.White), ss.Prompt());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Cyan), ss.Answer());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.DarkYellow), ss.Description());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Yellow), ss.Sugestion());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Gray), ss.UnSelected());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Green), ss.Selected());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.DarkGray), ss.Disabled());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Red), ss.Error());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.DarkGray), ss.Pagination());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.DarkYellow), ss.TaggedInfo());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.DarkGray), ss.Tooltips());
            Assert.Equal(Style.Default.Foreground(ConsoleColor.Cyan).Background(ConsoleColor.DarkGray), ss.Slider());
        }

        [Fact]
        public void Should_InitApplyStyleSchema()
        {
            var ss = new StyleSchema();
            ss.ApplyStyle(StyleControls.Prompt, new Style(Color.Aqua, Color.Aquamarine1, Overflow.Ellipsis));
            Assert.Equal(Color.Aqua, ss.Prompt().Foreground);
            Assert.Equal(Color.Aquamarine1, ss.Prompt().Background);
            Assert.Equal(Overflow.Ellipsis, ss.Prompt().OverflowStrategy);
        }

        [Fact]
        public void Should_UpdateBackgoundStyleSchema()
        {
            var ss = new StyleSchema();
            ss.UpdateBackgoundColor(Color.Aquamarine1);

            Assert.Equal(Color.Aquamarine1, ss.Answer().Background);
            Assert.Equal(Color.Aquamarine1, ss.Description().Background);
            Assert.Equal(Color.Aquamarine1, ss.Sugestion().Background);
            Assert.Equal(Color.Aquamarine1, ss.UnSelected().Background);
            Assert.Equal(Color.Aquamarine1, ss.Selected().Background);
            Assert.Equal(Color.Aquamarine1, ss.Disabled().Background);
            Assert.Equal(Color.Aquamarine1, ss.Error().Background);
            Assert.Equal(Color.Aquamarine1, ss.Pagination().Background);
            Assert.Equal(Color.Aquamarine1, ss.TaggedInfo().Background);
            Assert.Equal(Color.Aquamarine1, ss.Tooltips().Background);
            Assert.Equal(Color.Aquamarine1, ss.Slider().Background);
        }
    }
}