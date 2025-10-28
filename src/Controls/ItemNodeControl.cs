// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Controls
{
    /// <summary>
    /// Create a instance
    /// </summary>
    internal sealed class ItemNodeControl<T>(string id)
    {
        public string UniqueId => id;

        /// <summary>
        /// Gets or sets the unique identifier of the parent entity, if available.
        /// </summary>
        public string? ParentUniqueId { get; init; }

        /// <summary>
        /// Gets or sets the item file associated with this instance.
        /// </summary>
        public required T Value { get; init; }

        /// <summary>
        /// Get fullpath of item
        /// </summary>
        public string? FullPath { get; init; }

        /// <summary>
        /// Get Text of item
        /// </summary>
        public string? Text { get; init; }

        /// <summary>
        /// Get number of Children
        /// </summary>
        public int CountChildren { get; init; }
        
        /// <summary>
        /// Gets or sets a value indicating whether additional child elements can be loaded.
        /// </summary>
        public bool MoreLoadChildren { get; set; }

        /// <summary>
        /// Node Level 
        /// </summary>
        public int Level { get; init; }

        /// <summary>
        /// Node First item
        /// </summary>
        public bool FirstItem { get; init; }

        /// <summary>
        /// Node First item
        /// </summary>
        public bool LastItem { get; init; }

        /// <summary>
        /// Node Status
        /// </summary>
        public NodeStatus Status { get; set; }

        /// <summary>
        /// Node expandend
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Node disabled for select return
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Node Marked 
        /// </summary>
        public bool IsMarked { get; set; }

        public override string ToString()
        {
            return UniqueId;
        }
    }
}
