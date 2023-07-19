// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/WenceyWang/FIGlet.Net
// ***************************************************************************************

using PPlus.Controls;
using System.IO;

namespace PPlus.FIGlet
{
    internal interface IFIGlet
    {
        /// <summary>
        /// Load font for AsciiArt(FIGlet)
        /// </summary>
        /// <param name="value">Full path to  file font</param>
        /// <returns><see cref="IFIGlet"/></returns>
        IFIGlet LoadFont(string value);

        /// <summary>
        /// Load font for AsciiArt(FIGlet)
        /// </summary>
        /// <param name="value"><see cref="Stream"/> for font</param>
        /// <returns><see cref="IFIGlet"/></returns>
        IFIGlet LoadFont(Stream value);

        /// <summary>
        /// Set <see cref="CharacterWidth"/> for AsciiArt(FIGlet)
        /// </summary>
        /// <param name="value">Enum <see cref="CharacterWidth"/> value</param>
        /// <returns><see cref="IFIGlet"/></returns>
        IFIGlet FIGletWidth(CharacterWidth value);
    }
}
