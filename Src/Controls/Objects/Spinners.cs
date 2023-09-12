// **********************************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// **********************************************************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace PPlus.Controls.Objects
{
    internal class Spinners
    {
        private IReadOnlyList<string> _frames;
        private bool _isUnicode;
        private int _interval;
        private int _index;
        private readonly object _root;

        public Spinners(SpinnersType type,bool isunicode, int? interval= null, IEnumerable<string> frames = null)
        {
            _root = new();
            switch (type)
            {
                case SpinnersType.Ascii:
                    Init(SpinnersData.AsciiSpinner, interval);
                    break;
                case SpinnersType.Dots:
                    Init(SpinnersData.Dots, interval);
                    break;
                case SpinnersType.DotArrow:
                    Init(SpinnersData.DotArrow, interval);
                    break;
                case SpinnersType.DotArrowHeavy:
                    Init(SpinnersData.DotArrowHeavy, interval);
                    break;
                case SpinnersType.DotsScrolling:
                    Init(SpinnersData.DotsScrolling, interval);
                    break;
                case SpinnersType.Star:
                    Init(SpinnersData.Star, interval);
                    break;
                case SpinnersType.Flip:
                    Init(SpinnersData.Flip, interval);
                    break;
                case SpinnersType.Balloon:
                    Init(SpinnersData.Balloon, interval);
                    break;
                case SpinnersType.Noise:
                    Init(SpinnersData.Noise, interval);
                    break;
                case SpinnersType.Bounce:
                    Init(SpinnersData.Bounce, interval);
                    break;
                case SpinnersType.BouncingBar:
                    Init(SpinnersData.BouncingBar, interval);
                    break;
                case SpinnersType.BoxHeavy:
                    Init(SpinnersData.BoxHeavy, interval);
                    break;
                case SpinnersType.Arrow:
                    Init(SpinnersData.DotArrow, interval);
                    break;
                case SpinnersType.ArrowHeavy:
                    Init(SpinnersData.ArrowHeavy, interval);
                    break;
                case SpinnersType.DoubleArrow:
                    Init(SpinnersData.DoubleArrow, interval);
                    break;
                case SpinnersType.RightArrow:
                    Init(SpinnersData.RightArrow, interval);
                    break;
                case SpinnersType.LeftArrow:
                    Init(SpinnersData.LeftArrow, interval);
                    break;
                case SpinnersType.Pipe:
                    Init(SpinnersData.Pipe, interval);
                    break;
                case SpinnersType.Toggle:
                    Init(SpinnersData.Toggle, interval);
                    break;
                case SpinnersType.Custom:
                    _frames = new ReadOnlyCollection<string>(frames.ToArray());
                    _interval = interval ?? SpinnersData.FallBackSpinner.Interval;
                    _isUnicode = isunicode;
                    break;
                default:
                    throw new PromptPlusException($"SpinnersType: {type} Not Implemented");
            }
            if (!isunicode && _isUnicode)
            {
                _frames = SpinnersData.FallBackSpinner.Frames;
                _interval = interval ?? SpinnersData.FallBackSpinner.Interval;
                _isUnicode = SpinnersData.FallBackSpinner.IsUnicode;
            }
            _index = -1;
        }

        public string NextFrame(CancellationToken cancellationToken)
        {
            lock (_root)
            {
                _index++;
                if (_index > _frames.Count - 1)
                {
                    _index = 0;
                }
                cancellationToken.WaitHandle.WaitOne(_interval);
                return _frames[_index];
            }
        }

        public int Length => _frames.Count;
        public int Interval => _interval;
        public bool IsReseted => _index == -1;
        public void Reset()
        {
            lock (_root)
            {
                _index = -1;
            }
        }
        private void Init((IReadOnlyList<string> Frames, bool IsUnicode, int Interval) value, int? interval)
        {
            _frames = value.Frames;
            _interval = interval ?? value.Interval;
            _isUnicode = value.IsUnicode;
        }
    }




}
