
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PPlus.Drivers
{
    internal class MemoryConsoleReader : TextReader
    {

        private readonly Queue<char> _currentLine = new();

        public void LoadInput(string value)
        {
            foreach (var item in value)
            {
                _currentLine.Enqueue(item);
            }
        }
        public void LoadInput(ConsoleKeyInfo value)
        {
            _currentLine.Enqueue(value.KeyChar);
        }

        public void LoadInput(char value)
        {
            _currentLine.Enqueue(value);
        }

        public override int Peek()
        {
            if (!_currentLine.Any())
            {
                return -1;
            }
            return _currentLine.Peek();
        }


        public override int Read()
        {
            if (!_currentLine.Any())
            {
                return -1; 
            }
            return _currentLine.Dequeue();
        }
    }
}
