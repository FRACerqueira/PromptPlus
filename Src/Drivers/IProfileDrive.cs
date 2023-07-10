﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus
{
    /// <summary>
    /// Represents the interface for profile setup for console.
    /// </summary>
    public interface IProfileDrive
    {

        /// <summary>
        /// Get provider mode.
        /// </summary>
        string Provider { get; }

        /// <summary>
        /// Get Terminal mode.
        /// </summary>
        bool IsTerminal { get; }

        /// <summary>
        /// Get Unicode Supported.
        /// </summary>
        bool IsUnicodeSupported { get; }

        /// <summary>
        /// Get SupportsAnsi mode.
        /// </summary>
        bool SupportsAnsi { get; }

        /// <summary>
        /// Get Color capacity.<see cref="ColorSystem"/>
        /// </summary>
        ColorSystem ColorDepth { get; }

        /// <summary>
        /// Get default <see cref="Style"/> console.
        /// </summary>
        Style DefaultStyle { get; }

        /// <summary>
        /// Get screen margin left
        /// </summary>
        byte PadLeft { get; }

        /// <summary>
        /// Get screen margin right
        /// </summary>
        byte PadRight { get; }

        /// <summary>
        /// Gets the width of the buffer area.
        /// </summary>
        int BufferWidth { get; }

        /// <summary>
        /// Gets the height of the buffer area.
        /// </summary>
        int BufferHeight { get; }

        /// <summary>
        /// Get/Set Foreground console with color.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Get/set BackgroundColor console with color.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Get write Overflow Strategy.
        /// </summary>
        Overflow OverflowStrategy { get; }

        /// <summary>
        /// Reset colors to default values.
        /// </summary>
        void ResetColor();

    }
}