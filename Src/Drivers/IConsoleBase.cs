// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Represents the interface for any console.
    /// </summary>
    public interface IConsoleBase : ICursorDrive, IInputDrive, IOutputDrive, IBackendTextWrite, IProfileDrive
    {
    }

    /// <summary>
    /// Represents the custom interface for record console.
    /// </summary>
    internal interface IConsoleControl : IConsoleBase
    {
        bool IsControlText { get; set; }
        bool EnabledRecord { get; set; }
        string RecordConsole();
        string CaptureRecord(bool clearrecord);
    }
}
