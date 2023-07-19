// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace PPlus.Drivers
{
    internal class InputDriveMemory : IInputDrive
    {
        private readonly Queue<ConsoleKeyInfo> _input;
        private bool _isInputRedirected;

        public InputDriveMemory()
        {
            _input = new Queue<ConsoleKeyInfo>();
        }


        public bool KeyAvailable => _input.Count > 0;

        public bool IsInputRedirected => _isInputRedirected;

        public Encoding InputEncoding
        {
            get
            {
                return Encoding.UTF8;
            }
            set{}
        }

        public TextReader In => TextReader.Null;

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            if (_input.Count == 0)
            {
                throw new PromptPlusException("No input available.");
            }
            return _input.Dequeue();
        }

        public string? ReadLine()
        {
            ConsoleKeyInfo? key = null;
            var result = new StringBuilder();
            do
            {
                if (!KeyAvailable)
                {
                    break;
                }
                key = ReadKey();
                if (key.Value.Key != ConsoleKey.Enter)
                {
                    result.Append(key.Value.KeyChar);
                }
            }
            while (key.Value.Key != ConsoleKey.Enter);
            if (!key.HasValue)
            {
                return null;
            }
            return result.ToString();
        }

        public void SetIn(TextReader value)
        {
            _isInputRedirected = true;
        }

        public ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken)
        {
            cancellationToken ??= CancellationToken.None;
            if (cancellationToken.Value.IsCancellationRequested)
            { 
                return null;
            }
            if (_input.Count == 0)
            {
                while (_input.Count == 0)
                {
                    cancellationToken.Value.WaitHandle.WaitOne(30);
                }
            }
            return _input.Dequeue();
        }

        public void InputBuffer(string value)
        {
            foreach (var item in value)
            {
                _input.Enqueue(new ConsoleKeyInfo(item, (ConsoleKey)item, false, false, false));
            }
        }

        public void InputBuffer(ConsoleKeyInfo value)
        {
            _input.Enqueue(value);
        }

    }
}
