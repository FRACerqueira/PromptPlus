// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.IO;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the Banner Control interface
    /// </summary>
    public interface IBannerControl
    {
        /// <summary>
        /// Load external font from file
        /// </summary>
        /// <param name="value">fullpath of file</param>
        /// <returns>IBannerControl</returns>
        IBannerControl LoadFont(string value);

        /// <summary>
        /// Load external font from <see cref="Stream"/>
        /// </summary>
        /// <param name="value">stream instance</param>
        /// <returns>IBannerControl</returns>
        IBannerControl LoadFont(Stream value);

        /// <summary>
        /// Set <see cref="CharacterWidth"/> for the banner
        /// </summary>
        /// <param name="value">CharacterWidth <see cref="CharacterWidth"/> </param>
        /// <returns>IBannerControl</returns>
        IBannerControl FIGletWidth(CharacterWidth value);

        /// <summary>
        /// Execute this control and show banner.
        /// </summary>
        /// <param name="color">The foregound <see cref="Color"/> text</param>
        /// <param name="bannerDash">The type of <see cref="BannerDashOptions"/></param>
        void Run(Color? color = null,BannerDashOptions bannerDash = BannerDashOptions.None);
    }
}
