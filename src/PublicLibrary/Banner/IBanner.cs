// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.IO;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a banner that can be customized and displayed.
    /// </summary>
    public interface IBanner
    {
        /// <summary>
        /// Load a font from a file.
        /// </summary>
        /// <param name="filepathFont">The path to the font file.</param>
        /// <returns>The current <see cref="IBanner"/> instance for method chaining.</returns>
        IBanner FromFont(string filepathFont);

        /// <summary>
        /// Set border options for the banner.
        /// </summary>
        /// <param name="dashOptions">The <see cref="BannerDashOptions"/> to apply.</param>
        /// <returns>The current <see cref="IBanner"/> instance for method chaining.</returns>
        IBanner Border(BannerDashOptions dashOptions);

        /// <summary>
        /// Load a font from a stream.
        /// </summary>
        /// <param name="streamFont">The stream containing the font data.</param>
        /// <returns>The current <see cref="IBanner"/> instance for method chaining.</returns>
        IBanner FromFont(Stream streamFont);

        /// <summary>
        /// Displays the Banner widget.
        /// </summary>
        void Show();
    }
}
