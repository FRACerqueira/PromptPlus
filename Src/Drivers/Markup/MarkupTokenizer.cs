// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// ***************************************************************************************


using System;
using System.IO;
using System.Text;

namespace PPlus.Drivers.Markup
{
    internal class MarkupTokenizer : IDisposable
    {
        private readonly StringBuffer _reader;
        private bool _disposed = false;

        public MarkupToken? Current { get; set; }

        public MarkupTokenizer(string text)
        {
            _reader = new StringBuffer(text ?? throw new PromptPlusException("MarkupTokenizer with text null"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }
                _disposed = true;
            }
        }

        public bool MoveNext()
        {
            if (_reader.Eof)
            {
                return false;
            }

            var current = _reader.Peek();
            return current == '[' ? ReadMarkup() : ReadText();
        }

        private bool ReadText()
        {
            var position = _reader.Position;
            var builder = new StringBuilder();

            var encounteredClosing = false;
            while (!_reader.Eof)
            {
                var current = _reader.Peek();
                if (current == '[')
                {
                    // markup encountered. Stop processing.
                    break;
                }

                // If we find a closing tag (']') there must be two of them.
                if (current == ']')
                {
                    if (encounteredClosing)
                    {
                        _reader.Read();
                        encounteredClosing = false;
                        continue;
                    }

                    encounteredClosing = true;
                }
                else
                {
                    if (encounteredClosing)
                    {
                        throw new PromptPlusException(
                            $"Encountered unescaped ']' token at position {_reader.Position}");
                    }
                }

                builder.Append(_reader.Read());
            }

            if (encounteredClosing)
            {
                throw new PromptPlusException($"Encountered unescaped ']' token at position {_reader.Position}");
            }

            Current = new MarkupToken(MarkupTokenKind.Text, builder.ToString(), position);
            return true;
        }

        private bool ReadMarkup()
        {
            var position = _reader.Position;

            _reader.Read();

            if (_reader.Eof)
            {
                throw new PromptPlusException($"Encountered malformed markup tag at position {_reader.Position}.");
            }

            var current = _reader.Peek();
            switch (current)
            {
                case '[':
                    // No markup but instead escaped markup in text.
                    _reader.Read();
                    Current = new MarkupToken(MarkupTokenKind.Text, "[", position);
                    return true;
                case '/':
                    // Markup closed.
                    _reader.Read();

                    if (_reader.Eof)
                    {
                        throw new PromptPlusException(
                            $"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    current = _reader.Peek();
                    if (current != ']')
                    {
                        throw new PromptPlusException(
                            $"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    _reader.Read();
                    Current = new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
                    return true;
            }

            // Read the "content" of the markup until we find the end-of-markup
            var builder = new StringBuilder();
            var encounteredOpening = false;
            var encounteredClosing = false;
            while (!_reader.Eof)
            {
                current = _reader.Peek();
                switch (current)
                {
                    case ']':
                        _reader.Read();
                        encounteredClosing = true;
                        break;
                    case '[':
                        _reader.Read();
                        encounteredOpening = true;
                        break;
                }

                if (encounteredClosing)
                {
                    break;
                }

                if (encounteredOpening)
                {
                    throw new PromptPlusException(
                        $"Encountered malformed markup tag at position {_reader.Position - 1}.");
                }

                builder.Append(_reader.Read());
            }

            if (_reader.Eof)
            {
                throw new PromptPlusException($"Encountered malformed markup tag at position {_reader.Position}.");
            }

            Current = new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
            return true;
        }

        private class StringBuffer : IDisposable
        {
            private readonly StringReader _reader;
            private readonly int _length;
            private bool _disposed = false;

            public int Position { get; private set; }
            public bool Eof => Position >= _length;

            public StringBuffer(string text)
            {
                text ??= string.Empty;

                _reader = new StringReader(text);
                _length = text.Length;

                Position = 0;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _reader.Dispose();
                    }
                    _disposed = true;
                }
            }

            public char Expect(char character)
            {
                var read = Read();
                if (read != character)
                {
                    throw new PromptPlusException($"Expected '{character}', but found '{read}'");
                }

                return read;
            }

            public char Peek()
            {
                if (Eof)
                {
                    return '\0';
                }

                return (char)_reader.Peek();
            }

            public char Read()
            {
                if (Eof)
                {
                    return '\0';
                }

                Position++;
                return (char)_reader.Read();
            }
        }

    }
}
