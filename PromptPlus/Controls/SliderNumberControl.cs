// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Linq.Expressions;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;
using PromptPlusControls.Resources;

namespace PromptPlusControls.Controls
{
    internal class SliderNumberControl<T> : ControlBase<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        private T _currentValue;
        private readonly SliderNumberOptions<T> _options;
        private readonly double _stepSlider;
        private readonly T _largestep;
        private readonly T _shortstep;
        private readonly T _zerovalue = (T)Convert.ChangeType(0, typeof(T));

        public SliderNumberControl(SliderNumberOptions<T> options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            if (!IsValidType())
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_SliderNumberType, typeof(T).UnderlyingSystemType));
            }
            if (options.Min.CompareTo(_zerovalue) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Min), string.Format(Exceptions.Ex_MinArgumentOutOfRange, options.Min));
            }
            if (options.Min.CompareTo(options.Max) >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Max), string.Format(Exceptions.Ex_MaxArgumentOutOfRange, options.Max, options.Min));
            }
            if (options.Value.CompareTo(options.Max) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Value), string.Format(Exceptions.Ex_ValueArgumentOutOfRangeMax, options.Value, options.Max, options.Max));
            }
            if (options.Value.CompareTo(options.Min) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Value), string.Format(Exceptions.Ex_ValueArgumentOutOfRangeMin, options.Value, options.Min, options.Max));
            }
            if (options.ShortStep.CompareTo(_zerovalue) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.ShortStep), Exceptions.Ex_ShortvalueArgumentOutOfRangeMin);
            }
            if (options.ShortStep.CompareTo(options.Max) >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.ShortStep), string.Format(Exceptions.Ex_ShortvalueArgumentOutOfRangeMax, options.Max));
            }

            _options = options;
            _shortstep = _options.ShortStep;
            if (_options.LargeStep.HasValue)
            {
                _largestep = _options.LargeStep.Value;
            }
            else
            {
                _largestep = (T)Convert.ChangeType(int.Parse(_options.Max.ToString()) / 10, typeof(T));
            }
            if (_largestep.Equals(_zerovalue))
            {
                _largestep = _shortstep;
            }
            _currentValue = _options.Value;
            _stepSlider = TicketStep;
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out T result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);

                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        result = TypeHelper<T>.ConvertTo(ValueToString(_currentValue));
                        return true;
                    }
                    case ConsoleKey.DownArrow when keyInfo.Modifiers == 0 && _options.Type == SliderNumberType.UpDown:
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && _options.Type == SliderNumberType.LeftRight:
                    {
                        if (_currentValue.CompareTo(_options.Min) == 0)
                        {
                            isvalidhit = null;
                            break;
                        }
                        var aux = Substract(_currentValue, _shortstep);
                        if (aux.CompareTo(_options.Min) < 0)
                        {
                            aux = _options.Min;
                        }
                        _currentValue = aux;
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == ConsoleModifiers.Control && _options.Type == SliderNumberType.LeftRight:
                    {
                        if (_currentValue.CompareTo(_options.Min) == 0)
                        {
                            isvalidhit = null;
                            break;
                        }
                        var aux = Substract(_currentValue, _largestep);
                        if (aux.CompareTo(_options.Min) < 0)
                        {
                            aux = _options.Min;
                        }
                        _currentValue = aux;
                        break;
                    }
                    case ConsoleKey.UpArrow when keyInfo.Modifiers == 0 && _options.Type == SliderNumberType.UpDown:
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && _options.Type == SliderNumberType.LeftRight:
                    {
                        if (_currentValue.CompareTo(_options.Max) == 0)
                        {
                            isvalidhit = null;
                            break;
                        }
                        var aux = Add(_currentValue, _shortstep);
                        if (aux.CompareTo(_options.Max) > 0)
                        {
                            aux = _options.Max;
                        }
                        _currentValue = aux;
                        break;
                    }
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == ConsoleModifiers.Control && _options.Type == SliderNumberType.LeftRight:
                    {
                        if (_currentValue.CompareTo(_options.Max) == 0)
                        {
                            isvalidhit = null;
                            break;
                        }
                        var aux = Add(_currentValue, _largestep);
                        if (aux.CompareTo(_options.Max) > 0)
                        {
                            aux = _options.Max;
                        }
                        _currentValue = aux;
                        break;
                    }
                    default:
                    {
                        isvalidhit = null;
                        break;
                    }
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);

            result = default;

            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.Type == SliderNumberType.UpDown)
            {
                screenBuffer.WritePrompt($"{_options.Message} [{_options.Min},{_options.Max}]");
            }
            else
            {
                screenBuffer.WritePrompt(_options.Message);
            }

            var valuestep = CurrentValueStep(_currentValue);

            screenBuffer.WriteAnswer(ValueToString(_currentValue));

            if (_options.Type == SliderNumberType.LeftRight)
            {
                var difflen = ValueToString(_options.Max).Length - ValueToString(_currentValue).Length;
                if (difflen > 0)
                {
                    screenBuffer.Write(new string(' ', difflen));
                }

                screenBuffer.WriteHint($" | {_options.Min} ");
                if (valuestep > 0)
                {
                    screenBuffer.WriteSliderOn(valuestep);
                    screenBuffer.WriteSliderOff(_options.Witdth - valuestep);
                }
                else
                {
                    screenBuffer.WriteSliderOff(_options.Witdth);
                }
                screenBuffer.WriteHint($" {_options.Max} |");
            }

            screenBuffer.PushCursor();

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    if (_options.Type == SliderNumberType.LeftRight)
                    {
                        screenBuffer.WriteLineHint(Messages.SliderNumberKeyNavigator);
                    }
                    else if (_options.Type == SliderNumberType.UpDown)
                    {
                        screenBuffer.WriteLineHint(Messages.NumberUpDownKeyNavigator);
                    }
                }
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T result)
        {
            screenBuffer.WriteDone(_options.Message);
            screenBuffer.WriteAnswer(ValueToString(result));
        }

        private string ValueToString(T value)
        {
            var tmp = value.ToString();
            var decsep = PromptPlus.DefaultCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp.Substring(index + 1);
                if (dec.Length > _options.FracionalDig)
                {
                    dec = dec.Substring(0, _options.FracionalDig);
                }
                if (dec.Length < _options.FracionalDig)
                {
                    dec += new string('0', _options.FracionalDig - dec.Length);
                }
                if (_options.FracionalDig > 0)
                {
                    tmp = tmp.Substring(0, index) + decsep + dec;
                }
                else
                {
                    tmp = tmp.Substring(0, index);
                }
            }
            else
            {
                if (_options.FracionalDig > 0)
                {
                    tmp += decsep + new string('0', _options.FracionalDig);
                }
            }
            return tmp;
        }

        private static T Add(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            // Add the parameters together
            var body = Expression.Add(paramA, paramB);

            // Compile it
            var add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            // Call it
            return add(a, b);
        }

        private static T Substract(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            // Add the parameters together
            var body = Expression.Subtract(paramA, paramB);

            // Compile it
            var substract = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            // Call it
            return substract(a, b);
        }

        private int CurrentValueStep(T value) => (int)Math.Round((_stepSlider * double.Parse(value.ToString())), _options.FracionalDig);

        private double TicketStep => double.Parse(_options.Witdth.ToString()) / (int.Parse(_options.Max.ToString()) - int.Parse(_options.Min.ToString()));

        private bool IsValidType()
        {
            var type = typeof(T).UnderlyingSystemType;
            var result = false;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    result = true;
                    break;
            }
            return result;
        }
    }
}
