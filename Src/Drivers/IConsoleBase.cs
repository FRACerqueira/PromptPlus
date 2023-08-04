// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <inheritdoc cref="ICursorDrive"/>
    /// <inheritdoc cref="IInputDrive"/>
    /// <inheritdoc cref="IOutputDrive"/>
    /// <inheritdoc cref="IBackendTextWrite"/> 
    /// <inheritdoc cref="IProfileDrive"/>
    /// <inheritdoc cref="IConsoleExtendDrive"/>
    /// <summary>
    /// Represents the interface for any console.
    /// </summary>
    public interface IConsoleBase : ICursorDrive, IInputDrive, IOutputDrive, IBackendTextWrite, IProfileDrive, IConsoleExtendDrive
    {
    }

    internal interface IConsoleControl : IConsoleBase
    {
        bool IsControlText { get; set; }
        bool EnabledRecord { get; set; }
        string RecordConsole();
        string CaptureRecord(bool clearrecord);
 
    }
}
