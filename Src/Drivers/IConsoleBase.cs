// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

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
        /// <summary>
        /// Get/set console ForegroundColor
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Get/set console BackgroundColor
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Reset colors to default values.
        /// </summary>
        void ResetColor();
    }

    internal interface IConsoleControl : IConsoleBase
    {
        bool WriteToErroOutput { get; set; }
        bool IsControlText { get; set; }
        bool EnabledRecord { get; set; }
        string RecordConsole();
        string CaptureRecord(bool clearrecord);
        void UpdateProfile(ProfileSetup value);


    }
}
