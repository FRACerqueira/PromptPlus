// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines how the Timer control displays the running time value.
    /// </summary>
    public enum TimerDisplayMode
    {
        /// <summary>
        /// Displays the remaining time counting down to zero. This is the control's default.
        /// </summary>
        Countdown,
        /// <summary>
        /// Displays the elapsed time counting up from zero to the configured duration.
        /// </summary>
        Elapsed
    }
}
