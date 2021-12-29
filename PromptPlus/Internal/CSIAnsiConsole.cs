// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace PPlus.Internal
{
    internal static class CSIAnsiConsole
    {
        private const char EscapeCharacter = '\x1b';
        private enum State
        {
            Default,
            Escape,
            Csi,
            CsiParameterBytes,
            CsiIntermediateBytes,
            EndCsiCommand

        }

        public static string[] SplitCommands(string? value)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(value))
            {
                return result.ToArray();
            }
            var _buffer = new StringBuilder();
            var _currentState = State.Default;
            foreach (var item in value)
            {
                EvaluateChar(item, _buffer, _currentState, out _currentState);
                switch (_currentState)
                {
                    case State.Csi:
                    case State.CsiParameterBytes:
                    case State.CsiIntermediateBytes:
                    case State.Default:
                        break;
                    case State.Escape:
                        if (_buffer.Length > 0)
                        {
                            result.Add(_buffer.ToString());
                        }
                        _buffer = new StringBuilder();
                        _buffer.Append(EscapeCharacter);
                        break;
                    case State.EndCsiCommand:
                        result.Add(_buffer.ToString());
                        _buffer = new StringBuilder();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            if (_buffer.Length > 0)
            {
                result.Add(_buffer.ToString());
            }
            return result.ToArray();
        }

        private static void EvaluateChar(char c, StringBuilder _buffer, State currentstate, out State state)
        {
            state = currentstate;
            switch (currentstate)
            {
                case State.Escape:
                    switch (c)
                    {
                        case EscapeCharacter:
                            // Escape an escape character.
                            state = State.Default;
                            return;
                        case '[':
                            state = State.Csi;
                            break;
                        default:
                            state = State.Default;
                            break;
                    }
                    _buffer.Append(c);
                    break;

                case State.Csi:
                case State.CsiParameterBytes:
                    if (c >= 0x30 && c <= 0x3F)
                    {
                        _buffer.Append(c);
                        state = State.CsiParameterBytes;
                    }
                    else
                    {
                        goto case State.CsiIntermediateBytes;
                    }
                    break;

                case State.CsiIntermediateBytes:
                    if (c >= 0x20 && c <= 0x2F)
                    {
                        _buffer.Append(c);
                        state = State.CsiIntermediateBytes;
                    }
                    else if (c >= 0x40 && c <= 0x7E)
                    {
                        // Final byte.
                        _buffer.Append(c);
                        state = State.EndCsiCommand;
                    }
                    else
                    {
                        throw new Exception("Invalid character in CSI sequence.");
                    }
                    break;

                case State.EndCsiCommand:
                {
                    state = State.Default;
                    goto case State.Default;
                }
                case State.Default:
                    if (c == EscapeCharacter)
                    {
                        state = State.Escape;
                    }
                    else
                    {
                        _buffer.Append(c);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

