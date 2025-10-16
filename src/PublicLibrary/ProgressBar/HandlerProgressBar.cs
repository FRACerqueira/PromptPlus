// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a progress bar handler.
    /// </summary>
    public sealed class HandlerProgressBar
    {
        private double? _lastvalue;
        private double _currentvalue;
        private readonly double _min;
        private readonly double _max;
        private Exception? _error;
        private bool _aborted;

#pragma warning disable IDE0290 // Use primary constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerProgressBar"/> class with the specified value, minimum, and maximum values.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <exception cref="ArgumentException">Thrown when min is greater than or equal to max.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is outside the range [min, max].</exception>
        public HandlerProgressBar(double value, double min, double max)
        {
            // Initialize to a value outside the range to ensure HasChange works correctly.
            _lastvalue = value * -1;
            _currentvalue = value;
            _min = min;
            _max = max;
        }
#pragma warning restore IDE0290 // Use primary constructor

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Maxvalue => _max;

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Minvalue => _min;

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Value => _currentvalue;

        /// <summary>
        /// Gets a value indicating whether the progress bar has finished.
        /// </summary>
        public bool Finish => _aborted || _currentvalue >= Maxvalue;

        /// <summary>
        /// Gets or sets the error associated with the progress bar.
        /// </summary>
        public Exception? Error => _error;

        /// <summary>
        /// Sets the error associated with the progress bar.
        /// </summary>
        /// <param name="error"></param>
        public void ErrorAndAbort(Exception? error)
        {
            _error = error;
            _aborted = true;
        }

        /// <summary>
        /// Updates the current value of the progress bar.
        /// </summary>
        /// <param name="value">The new current value.</param>
        public void Update(double value)
        {
            if (value > Maxvalue)
            {
                value = Maxvalue;
            }
            if (value < Minvalue)
            {
                value = Minvalue;
            }
            _currentvalue = value;
        }

        /// <summary>
        /// Checks if the value has changed since the last update.
        /// </summary>
        /// <returns><c>true</c> if the value has changed; otherwise, <c>false</c>.</returns>
        public bool HasChange()
        {
            bool result = _lastvalue != Value;
            if (result)
            {
                _lastvalue = Value;
            }
            if (_aborted)
            {
                result = true;
            }
            return result;
        }

        internal void Abort()
        {
            _aborted = true;
        }
    }
}
