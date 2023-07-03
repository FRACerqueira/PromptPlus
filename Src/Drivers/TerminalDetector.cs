// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Drivers
{
    internal class TerminalDetector
    {
        public static bool Detect()
        {
            try
            {
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
