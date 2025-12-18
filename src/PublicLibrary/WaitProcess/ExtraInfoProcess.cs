// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{ 
    /// <summary>
    /// Provides functionality to update the extra information associated with a state process.
    /// </summary>
    /// <param name="stateProcess">The state process instance whose extra information will be updated. Cannot be null.</param>
    public sealed class ExtraInfoProcess(StateProcess stateProcess)
    {
        /// <summary>
        /// Updates the extra information associated with the current state process.
        /// </summary>
        /// <param name="extraInfo">The new extra information to associate with the state process. Can be null to clear the existing
        /// information.</param>
        public void Update(string? extraInfo)
        {
            stateProcess.DynamicInfo = extraInfo;
        }
    }
}
