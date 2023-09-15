// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace PPlus.Drivers
{
    internal class TerminalDetector
    {
        public static bool Detect()
        {
            // Not Running on Windows?
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }
            try
            {
                //windows 11 22H2
                //https://devblogs.microsoft.com/commandline/windows-terminal-is-now-the-default-in-windows-11/
                if (Environment.OSVersion.Version.Build >= 22621)
                {
                    return true;
                }
                if (Environment.GetEnvironmentVariable("WT_SESSION") != null)
                { 
                    return true;
                }
                //if Running over windows legacy console over S.O Windows BufferHeight != WindowHeight
                return Console.BufferHeight == Console.WindowHeight;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
