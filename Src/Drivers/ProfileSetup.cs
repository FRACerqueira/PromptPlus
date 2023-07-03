// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PPlus
{
    /// <summary>
    /// Represents Profile Setup for console.
    /// </summary>
    public class ProfileSetup 
    {
        internal ProfileSetup()
        {
        }

        /// <summary>
        /// Get/Set Default Culture for console
        /// <br>Culture is global set fro threads</br>  
        public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;


        /// <summary>
        /// Get/Set Terminal mode. if Running over Terminal mode or not.
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// Get/Set Unicode Supported.
        /// </summary>
        public bool IsUnicodeSupported { get; set; }

        /// <summary>
        /// Get/Set SupportsAnsi mode commands.
        /// </summary>
        public bool SupportsAnsi { get; set; }

        /// <summary>
        /// Get/Set Color capacity.<see cref="ColorSystem"/>
        /// </summary>
        public ColorSystem ColorDepth { get; set; }

        /// <summary>
        ///Get/Set screen margin left
        /// </summary>
        public byte PadLeft { get; set; }

        /// <summary>
        /// Get/Set screen margin right
        /// </summary>
        public byte PadRight { get; set; }

        /// <summary>
        /// Get/Set write Overflow Strategy.
        /// </summary>
        public Overflow OverflowStrategy { get; set; }


        /// <summary>
        /// Get/Set Foreground console with color.
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Get/Set BackgroundColor console with color.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

    }
}
