// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Inspired by the work https://github.com/shibayan/Sharprompt
// ***************************************************************************************

using System.Text;

namespace PromptPlusControls.Internal
{
    internal class InputBuffer
    {
        private readonly StringBuilder _inputBuffer = new();

        public int Position { get; private set; }

        public int Length => _inputBuffer.Length;

        public bool IsStart => Position == 0;

        public bool IsEnd => Position == _inputBuffer.Length;

        public InputBuffer Insert(char value)
        {
            _inputBuffer.Insert(Position++, value);
            return this;
        }

        public InputBuffer Load(string value)
        {
            _inputBuffer.Append(value);
            Position += value.Length;
            return this;
        }

        public InputBuffer Backward()
        {
            Position--;
            return this;
        }

        public InputBuffer Forward()
        {
            Position++;
            return this;
        }

        public InputBuffer Backspace()
        {
            _inputBuffer.Remove(--Position, 1);
            return this;
        }

        public InputBuffer Delete()
        {
            _inputBuffer.Remove(Position, 1);
            return this;
        }

        public InputBuffer Clear()
        {
            Position = 0;
            _inputBuffer.Clear();
            return this;
        }

        public string ToBackward() => _inputBuffer.ToString(0, Position);

        public string ToForward() => _inputBuffer.ToString(Position, _inputBuffer.Length - Position);

        public override string ToString() => _inputBuffer.ToString();

    }
}
