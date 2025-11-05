// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Specifies how the control should behave after press (SIGINT) in terminal (typically Ctrl+C) or Ctrl-Break is pressed.
    /// </summary>
    public enum AfterCancelKeyPress
    {
        /// <summary>
        /// Follows the default action after a cancellation.
        /// </summary>
        /// 
        Default,

        /// <summary>
        /// Abort the current control's operation when cancel key is pressed and continue with next operation.
        /// </summary>
        AbortCurrentControl,

        /// <summary>
        /// Abort the All control's operation when cancel key is pressed and continue with next operation.
        /// </summary>
        AbortAllControl,
    }
}