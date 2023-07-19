using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{


    public class EmacsBufferTests : BaseTest
    {
        private readonly EmacsBuffer _readlinedefault;

        public EmacsBufferTests()
        {
            _readlinedefault = new EmacsBuffer(PPlus.Controls.CaseOptions.Any);
        }

        [Fact]
        internal void Should_have_accept_TryAcceptedReadlineConsoleKey_validvalues()
        {
            // Given
            PromptPlus.InputBuffer("abc" + (char)13);
            //when
            var qtdnotaccept = 0;
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                var ok = _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
                if (!ok)
                {
                    qtdnotaccept++;
                }
            }
            // Then
            Assert.Equal(3, _readlinedefault.Length);
            Assert.Equal(3, _readlinedefault.Position);
            Assert.Equal(1, qtdnotaccept);
            Assert.Equal("abc", _readlinedefault.ToString());
        }

        [Fact]
        internal void Should_have_accept_Load_validvalues()
        {
            // Given
            _readlinedefault
                .LoadPrintable("abc" + (char)13);
            // Then
            Assert.Equal(3, _readlinedefault.Length);
            Assert.Equal(3, _readlinedefault.Position);
            Assert.Equal("abc", _readlinedefault.ToString());
        }

        [Fact]
        internal void Should_have_accept_Clear_command()
        {
            // Given
            _readlinedefault
                .LoadPrintable("abc" + (char)13);
            //when
            _readlinedefault.Clear();
            // Then
            Assert.Equal(0, _readlinedefault.Length);
        }

        [Fact]
        internal void Should_have_not_accept_Load_with_noprintable()
        {

            // Given
            _readlinedefault
                .LoadPrintable(((char)3).ToString());
            // Then
            Assert.Equal(0, _readlinedefault.Length);
        }

        [Fact]
        internal void Should_have_not_accept_TryAcceptedReadlineConsoleKey_validvalues()
        {
            // Given
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, 0, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Length);
        }

        [Fact]
        internal void Should_have_accept_ToBackward_and_ToForward_and_leftarrow()
        {
            // Given
            PromptPlus.InputBuffer("abc");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(2, _readlinedefault.Position);
            Assert.Equal("ab", _readlinedefault.ToBackward());
            Assert.Equal("c", _readlinedefault.ToForward());
            Assert.Equal("abc", _readlinedefault.ToString());
        }

        [Fact]
        internal void Should_have_accept_Home_and_Rightarrow()
        {
            // Given
            PromptPlus.InputBuffer("abc");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(1, _readlinedefault.Position);
            Assert.Equal("abc", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text with lenght > 1
        //Transpose the previous two characters
        internal void Should_have_accept_ctrl_T()
        {
            // Given
            PromptPlus.InputBuffer("abc");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.T, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(3, _readlinedefault.Position);
            Assert.Equal("acb", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut, when when have any text
        // Clears the content
        internal void Should_have_accept_ctrl_L()
        {
            // Given
            PromptPlus.InputBuffer("abc");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.L, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Position);
            Assert.Equal(0, _readlinedefault.Length);
        }

        [Fact]
        //Emacs keyboard shortcut  when when have any text
        //Lowers the case of every character from the cursor's position to the end of the current word
        internal void Should_have_accept_Alt_L()
        {
            // Given
            PromptPlus.InputBuffer("ABC DEF");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.L, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(4, _readlinedefault.Position);
            Assert.Equal(7, _readlinedefault.Length);
            Assert.Equal("abc DEF", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut  when when have any text
        // Clears the line content before the cursor
        internal void Should_have_accept_ctrl_U()
        {
            // Given
            PromptPlus.InputBuffer("abc");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.U, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Position);
            Assert.Equal(1, _readlinedefault.Length);
            Assert.Equal("c", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut  when when have any text
        //Upper the case of every character from the cursor's position to the end of the current word
        internal void Should_have_accept_Alt_U()
        {
            // Given
            PromptPlus.InputBuffer("abc def");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.U, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(4, _readlinedefault.Position);
            Assert.Equal(7, _readlinedefault.Length);
            Assert.Equal("ABC def", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Clears the line content after the cursor
        internal void Should_have_accept_Ctrl_K()
        {
            // Given
            PromptPlus.InputBuffer("abc d");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.K, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(1, _readlinedefault.Position);
            Assert.Equal(1, _readlinedefault.Length);
            Assert.Equal("a", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Clears the word before the cursor
        internal void Should_have_accept_Ctrl_W()
        {
            // Given
            PromptPlus.InputBuffer("abc def g");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.W, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(4, _readlinedefault.Position);
            Assert.Equal(6, _readlinedefault.Length);
            Assert.Equal("abc  g", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Capitalizes the character under the cursor and moves to the end of the word
        internal void Should_have_accept_Alt_C()
        {
            // Given
            PromptPlus.InputBuffer("abc def");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.C, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(3, _readlinedefault.Position);
            Assert.Equal(7, _readlinedefault.Length);
            Assert.Equal("aBc def", _readlinedefault.ToString());
        }


        [Fact]
        //Emacs keyboard shortcut when when have any text
        // Cuts the word after the cursor
        internal void Should_have_accept_Alt_D()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.D, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(1, _readlinedefault.Position);
            Assert.Equal(9, _readlinedefault.Length);
            Assert.Equal("a def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        // (forward) moves the cursor forward one word.
        internal void Should_have_accept_Alt_F()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(4, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //(backward) moves the cursor backward one word.
        internal void Should_have_accept_Alt_B()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.B, false, true, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(4, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Deletes the previous character (same as backspace).
        internal void Should_have_accept_Ctrl_h_And_bakspace()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.H, false, false, true));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Backspace, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(9, _readlinedefault.Position);
            Assert.Equal(9, _readlinedefault.Length);
            Assert.Equal("abc def g", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Moves the cursor to the line start (equivalent to the key Home).
        //(end) moves the cursor to the line end (equivalent to the key End).
        internal void Should_have_accept_Ctrl_A_then_Ctrl_E()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.A, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
            // Given
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.E, false, false, true));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(11, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Moves the cursor to the line start (equivalent to the key Home).
        internal void Should_have_accept_home_then_end()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
            // Given
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.End, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(11, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Moves the cursor back one character (equivalent to the key ←).
        internal void Should_have_accept_ctrl_B_and_LeftArrow()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.B, false, false, true));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.LeftArrow, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(9, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }


        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Moves the cursor forward one character (equivalent to the key →).
        internal void Should_have_accept_Home_then_ctrl_F_and_RightArrow()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.F, false, false, true));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.RightArrow, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(2, _readlinedefault.Position);
            Assert.Equal(11, _readlinedefault.Length);
            Assert.Equal("abc def ghi", _readlinedefault.ToString());
        }

        [Fact]
        //Emacs keyboard shortcut when when have any text
        //Delete the current character (then equivalent to the key Delete).
        internal void Should_have_accept_Home_then_ctrl_D_and_Delete()
        {
            // Given
            PromptPlus.InputBuffer("abc def ghi");
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Home, false, false, false));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.D, false, false, true));
            PromptPlus.InputBuffer(new ConsoleKeyInfo((char)0, ConsoleKey.Delete, false, false, false));
            //when
            while (PromptPlus.Console.KeyAvailable)
            {
                var keyinfo = PromptPlus.Console.ReadKey(false);
                _readlinedefault.TryAcceptedReadlineConsoleKey(keyinfo);
            }
            // Then
            Assert.Equal(0, _readlinedefault.Position);
            Assert.Equal(9, _readlinedefault.Length);
            Assert.Equal("c def ghi", _readlinedefault.ToString());
        }
    }
}
