// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Text;

namespace PromptPlusLibrary.Core.Markup
{
    /// <summary>
    /// Tokenizes markup text.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MarkupTokenizer"/> class.
    /// </remarks>
    /// <param name="text">The text to tokenize.</param>
    /// <exception cref="ArgumentNullException">Thrown when the text is null.</exception>
    internal sealed class MarkupTokenizer(string text) : IDisposable
    {
        private readonly StringBuffer _reader = new(text ?? throw new ArgumentNullException(nameof(text), "MarkupTokenizer with text null"));
        private bool _disposed;
        private readonly StringBuilder _builder = new();

        /// <summary>
        /// Gets the current token.
        /// </summary>
        public MarkupToken? Current { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="MarkupTokenizer"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
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

        /// <summary>
        /// Moves to the next token.
        /// </summary>
        /// <returns><c>true</c> if there is a next token; otherwise, <c>false</c>.</returns>
        public bool MoveNext()
        {
            if (_reader.Eof)
            {
                return false;
            }

            char current = _reader.Peek();
            return current == '[' ? ReadMarkup() : ReadText();
        }

        private bool ReadText()
        {
            int position = _reader.Position;
            _builder.Clear();

            bool encounteredClosing = false;
            while (!_reader.Eof)
            {
                char current = _reader.Peek();
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
                        throw new InvalidOperationException($"Encountered unescaped ']' token at position {_reader.Position}");
                    }
                }

                _builder.Append(_reader.Read());
            }

            if (encounteredClosing)
            {
                throw new InvalidOperationException($"Encountered unescaped ']' token at position {_reader.Position}");
            }

            Current = new MarkupToken(MarkupTokenKind.Text, _builder.ToString(), position);
            return true;
        }

        private bool ReadMarkup()
        {
            int position = _reader.Position;

            _reader.Read();

            if (_reader.Eof)
            {
                throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
            }

            char current = _reader.Peek();
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
                        throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    current = _reader.Peek();
                    if (current != ']')
                    {
                        throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    _reader.Read();
                    Current = new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
                    return true;
            }

            // Read the "content" of the markup until we find the end-of-markup
            _builder.Clear();
            bool encounteredOpening = false;
            bool encounteredClosing = false;
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
                    throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position - 1}.");
                }

                _builder.Append(_reader.Read());
            }

            if (_reader.Eof)
            {
                throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
            }

            Current = new MarkupToken(MarkupTokenKind.Open, _builder.ToString(), position);
            return true;
        }

        private sealed class StringBuffer : IDisposable
        {
            private readonly StringReader _reader;
            private readonly int _length;
            private bool _disposed;

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
                if (!_disposed)
                {
                    _reader.Dispose();
                    _disposed = true;
                }
                GC.SuppressFinalize(this);
            }

            public char Expect(char character)
            {
                char read = Read();
                if (read != character)
                {
                    throw new InvalidOperationException($"Expected '{character}', but found '{read}'");
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
