// **********************************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// **********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlusLibrary.Controls
{
    internal sealed class Spinners
    {
        private readonly List<string> _frames;
        private readonly int _interval;
        private int _index;
        private DateTime _dateref;

        public Spinners(IEnumerable<string> frames, int interval)
        {
            if (frames == null || !frames.Any())
            {
                throw new ArgumentException("Frames cannot be null or empty.", nameof(frames));
            }
            if (interval < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater or equal than 100.");
            }
            _frames = [.. frames];
            _interval = interval;
            _dateref = DateTime.MinValue;
        }

        public Spinners(SpinnersType spinnersType)
        {
            switch (spinnersType)
            {
                case SpinnersType.Ascii:
                    _frames = ["-", "\\", "|", "/", "-", "\\", "|", "/"];
                    _interval = 200;
                    break;
                case SpinnersType.Dots:
                    _frames = [".  ", ".. ", "...", "   "];
                    _interval = 200;
                    break;
                case SpinnersType.DotsScrolling:
                    _frames = [".  ", ".. ", "...", " ..", "  .", "   "];
                    _interval = 200;
                    break;
                case SpinnersType.DotArrow:
                    _frames = [">....", ".>...", "..>..", "...>.", "....>"];
                    _interval = 200;
                    break;
                case SpinnersType.Star:
                    _frames = ["+", "x", "*"];
                    _interval = 200;
                    break;
                case SpinnersType.Flip:
                    _frames = ["_", "_", "_", "-", "`", "`", "'", "´", "-", "_", "_", "_"];
                    _interval = 200;
                    break;
                case SpinnersType.Bounce:
                    _frames = ["o     ", " o    ", "  o   ", "   o  ", "    o ", "     o", "    o ", "   o  ", "  o   ", " o    ", "o     "];
                    _interval = 200;
                    break;
                case SpinnersType.BouncingBar:
                    _frames = ["    ", "=   ", "==  ", "=== ", " ===", "  ==", "   =", "    ", "   =", "  ==", " ===", "====", "=== ", "==  ", "=   "];
                    _interval = 200;
                    break;
                case SpinnersType.Arrow:
                    _frames = [">    ", ">>   ", ">>>  ", ">>>> ", ">>>>>"];
                    _interval = 200;
                    break;
                case SpinnersType.Toggle:
                    _frames = ["=", "*", "-"];
                    _interval = 200;
                    break;
                default:
                    throw new NotImplementedException($"SpinnersType: {spinnersType} Not Implemented");
            }
            _dateref = DateTime.MinValue;
        }

        public bool HasNextFrame(out string? frame)
        {
            if (_dateref == DateTime.MinValue)
            {
                _index = 0;
                frame = _frames[_index];
                _dateref = DateTime.Now.AddMilliseconds(_interval);
                return true;
            }
            if (DateTime.Now > _dateref)
            {
                _index++;
                if (_index > _frames.Count - 1)
                {
                    _index = 0;
                }
                frame = _frames[_index];
                _dateref = DateTime.Now.AddMilliseconds(_interval);
                return true;
            }
            frame = null;
            return false;
        }
    }
}
