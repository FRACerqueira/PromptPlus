// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a stage when Controls is Running.
    /// </summary>
    public enum StageControl
    {
        /// <summary>
        /// Start stage.Represents the stage after then control stated.
        /// </summary>
        OnStartControl = 1,
        /// <summary>
        /// Input stage.Represents the stage after then control show input render.
        /// </summary>
        OnInputRender = 2,
        /// <summary>
        /// Input stage.Represents the stage after then control try accept input.
        /// </summary>
        OnTryAcceptInput = 3,
        /// <summary>
        /// Finish stage.Represents the stage after then control finished render.
        /// </summary>
        OnFinishControl = 4,
    }
}
