// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleEmacsReadLineControlsSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample Emacs ReadLine", extraLines: 1);

            PromptPlus.Console.Join()
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+T]] to transpose the previous two characters.[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+L]] to clears the content.[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+H]] to deletes the previous character (equivalent to the backspace key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+E]] to moves the cursor to the line end (equivalent to the end key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+A]] to moves the cursor to the line start (equivalent to the home key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+B]] to moves the cursor back one character (equivalent to the left arrow key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+F]] to Moves the cursor forward one character (equivalent to the right arrow key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+D]] to delete the current character (equivalent to the delete key).[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+U]] to clears the line content before the cursor.[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+K]] to clears the line content after the cursor.[/]")
                .WriteLine(@"[gray]Press [[CTRL+SHIFT+W]] to clear the word before the cursor.[/]")
                .WriteLine(@"[gray]Press [[ALT+L]]  to lowers the case of every character from the cursor's position to the end of the current word.[/]")
                .WriteLine(@"[gray]Press [[ALT+U]]  to upper the case of every character from the cursor's position to the end of the current word.[/]")
                .WriteLine(@"[gray]Press [[ALT+C]]  to capitalizes the character under the cursor and moves to the end of the word.[/]")
                .WriteLine(@"[gray]Press [[ALT+D]]  to clear the word after the cursor.[/]")
                .WriteLine(@"[gray]Press [[ALT+F]]  to moves the cursor forward one word.[/]")
                .WriteLine(@"[gray]Press [[ALT+B]]  to moves the cursor backward one word.[/]")
                .WriteLine(@"[gray]Press [[INSERT]] to toggle input replacement mode (default/started in insert mode).[/]")
                .WriteLine(@"[gray]Press [[ESC]] (feature optional) to abort input and return null.[/]")
                .WriteLine("")
                .Done();

            PromptPlus.Console.Write("[yellow]Try Input sample:[/] ");

            var input = PromptPlus.Controls.InputEmacs("Sample test value")
                .ValidateKey(MyKeyAccept)
                .EscAbort()
                .MaxWidth(20)
                .MaxLength(150)
                .CaseOptions(CaseOptions.Any)
                .ReadLine();

            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.Write("[yellow]Read only Input sample:[/] ");
            input = PromptPlus.Controls.InputEmacs("Sample test value")
                .EscAbort()
                .MaxWidth(8)
                .ReadOnly()
                .ReadLine();

        }

        private static bool MyKeyAccept(char currentinput)
        {
            //do something
            return true;
        }
    }
}
