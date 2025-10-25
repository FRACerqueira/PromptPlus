// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the state of a Progress Bar.
    /// </summary>
    /// <param name="value">Last value of Progress Bar.</param>
    /// <param name="valuetext">Finished text of Progress Bar.</param>
    /// <param name="minvalue">Min value of Progress Bar.</param>
    /// <param name="maxvalue">Max value of Progress Bar.</param>
    /// <param name="elapsedtime">Elapsed Time of Progress Bar.</param>
    /// <param name="error">Exception of Progress Bar.</param>
    public readonly struct StateProgress(double value, string? valuetext, double minvalue, double maxvalue, TimeSpan elapsedtime, Exception? error = null)
    {
        /// <summary>
        /// Last value of Progress Bar
        /// </summary>
        public double? FinishedValue { get; } = value;

        /// <summary>
        /// Finished text of Progress Bar
        /// </summary>
        public string? FinishedText { get; } = valuetext;

        /// <summary>
        /// Min value of Progress Bar
        /// </summary>
        public double MinValue { get; } = minvalue;

        /// <summary>
        /// Max value of Progress Bar
        /// </summary>
        public double MaxValue { get; } = maxvalue;

        /// <summary>
        /// Exception of Progress Bar
        /// </summary>
        public Exception? ExceptionProgress { get; } = error;

        /// <summary>
        /// Elapsed Time of Progress Bar
        /// </summary>
        public TimeSpan ElapsedTime { get; } = elapsedtime;

    }
}
