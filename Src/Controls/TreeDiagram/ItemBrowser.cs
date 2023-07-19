// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a file or folder item
    /// </summary>
    public class ItemBrowser
    {
        /// <summary>
        /// Get Name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Get if item is folder
        /// </summary>
        public bool IsFolder { get; internal set; }

        /// <summary>
        /// Get name of parent folder
        /// </summary>
        public string CurrentFolder { get; internal set; }

        /// <summary>
        /// Get fullpath of item
        /// </summary>
        public string FullPath { get; internal set; }


        /// <summary>
        /// Get Length of item. If a folder lenght represents number of item. If file lenght represents the size in bytes
        /// </summary>
        public long Length { get; internal set; }

    }
}
