// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Banner Control to Write to console AsciiArt(FIGlet). 
        /// </summary>
        /// <param name="value">The text to write</param>
        /// <returns><see cref="IBannerControl"/></returns>
        public static IBannerControl Banner(string value)
        {
            return new BannerControl(_consoledrive,value);
        }
    }
}
