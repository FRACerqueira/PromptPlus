// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PPlus.Drivers
{
    internal class MemoryConsoleReader : TextReader
    {

        private readonly Queue<ConsoleKeyInfo> _currentLine = new();

        public void LoadInput(string value)
        {
            foreach (var item in value)
            {
                LoadInput(item);
            }
        }

        public void LoadInput(ConsoleKeyInfo value)
        {
            _currentLine.Enqueue(value);
        }

        public void WaitRender()
        {
            _currentLine.Enqueue(new ConsoleKeyInfo((char)0, 0, true, true, true));
        }

        public void LoadInput(char value)
        {
            if (Enum.TryParse<ConsoleKey>(value.ToString(), true, out var result))
            {
                LoadInput(new ConsoleKeyInfo(value, result, char.IsLetter(value) && char.IsUpper(value), false, false));
            }
            else
            {
                if (Enum.IsDefined(typeof(ConsoleKey), (int)value))
                {
                    var ck = (ConsoleKey)value;
                    LoadInput(new ConsoleKeyInfo(value, ck, false, false, false));
                }
            }
        }

        public override int Peek()
        {
            if (!_currentLine.Any())
            {
                return -1;
            }
            var aux = _currentLine.Peek();
            if (aux.Key == 0 && aux.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift | ConsoleModifiers.Alt))
            {
                return -2;
            }
            return aux.KeyChar;
        }

        public ConsoleKeyInfo ReadLoadInput()
        {
            if (!_currentLine.Any())
            {
                return new ConsoleKeyInfo();
            }
            return _currentLine.Dequeue();
        }

    }
}
