// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents a spinner that can be used to indicate progress in console applications.
    /// </summary>
    /// <remarks>
    /// The concrete spinner implementations live in <c>SpinnerBase.Instances.cs</c> and the
    /// catalog of well-known spinners is exposed through <see cref="Known"/> in
    /// <c>SpinnerInstance.Known.cs</c>.
    /// </remarks>
    internal abstract partial class SpinnerBase
    {
        private int indexframe;
        private string _currentFrame = string.Empty;
#if NET9_0_OR_GREATER
        private readonly System.Threading.Lock _nextFrameSync = new();
#else
        private readonly object _nextFrameSync = new();
#endif
        /// <summary>
        /// Gets the update interval for the spinner.
        /// </summary>
        public abstract TimeSpan Interval { get; }

        /// <summary>
        /// Gets a value indicating whether or not the spinner
        /// uses Unicode characters.
        /// </summary>
        public abstract bool IsUnicode { get; }

        /// <summary>
        /// Gets the spinner frames.
        /// </summary>
        public abstract IReadOnlyList<string> Frames { get; }

        /// <summary>
        /// Gets the current frame without advancing the spinner.
        /// </summary>
        public string CurrentFrame
        {
            get => System.Threading.Volatile.Read(ref _currentFrame);
        }

        /// <summary>
        /// Gets the next frame of the spinner.
        /// </summary>
        /// <remarks>
        /// Advances the internal cursor in a circular way and updates <see cref="CurrentFrame"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the spinner frames are null.
        /// </exception>
        public void NextFrame()
        {
            IReadOnlyList<string> frames = Frames ?? throw new InvalidOperationException("Spinner frames cannot be null.");
            int frameCount = frames.Count;
            if (frameCount == 0)
            {
                System.Threading.Volatile.Write(ref _currentFrame, string.Empty);
                return;
            }

            lock (_nextFrameSync)
            {
                // Keep the cursor in-range even if the spinner definition changes dynamically.
                if ((uint)indexframe >= (uint)frameCount)
                {
                    indexframe = 0;
                }

                _currentFrame = frames[indexframe];
                indexframe++;
                if (indexframe >= frameCount)
                {
                    indexframe = 0;
                }
            }
        }
    }
}
