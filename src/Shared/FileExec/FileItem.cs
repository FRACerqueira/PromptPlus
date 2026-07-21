// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a file system entry (file or directory) selected by the File control.
    /// </summary>
    public sealed class FileItem
    {
        internal FileItem(string fullPath, string name, bool isDirectory, long length, DateTime lastWriteTime)
        {
            FullPath = fullPath;
            Name = name;
            IsDirectory = isDirectory;
            Length = length;
            LastWriteTime = lastWriteTime;
        }

        /// <summary>
        /// Gets the full path of the entry.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        /// Gets the display name of the entry.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets whether the entry is a directory.
        /// </summary>
        public bool IsDirectory { get; }

        /// <summary>
        /// Gets the file length in bytes. Zero for directories.
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Gets the last write time of the entry.
        /// </summary>
        public DateTime LastWriteTime { get; }

        /// <inheritdoc/>
        public override string ToString() => FullPath;
    }
}
