// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a file or folder selected
    /// </summary>
    public sealed class ItemFile
    {
        /// <summary>
        /// Get Name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Get fullpath of item
        /// </summary>
        public required string FullPath { get; init; }

        /// <summary>
        /// Get if item is folder
        /// </summary>
        public bool IsFolder { get; init; }

        /// <summary>
        /// Get Length of item. Length represents the size in bytes. If <see cref="IsFolder"/>, sise is number of entries.
        /// </summary>
        public long Length { get; init; }
    }
}
