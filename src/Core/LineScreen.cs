// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using EastAsianWidthDotNet;
using System.Collections.Generic;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents a line in the screen buffer with its content and metadata.
    /// </summary>
    internal sealed class LineScreen
    {
        private int _contentSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineScreen"/> class.
        /// </summary>
        /// <param name="line">The relative line number.</param>
        /// <param name="content">The content of the line as an array of segments.</param>
        public LineScreen(int line, Segment[] content)
        {
            Line = line;
            Content = [.. content];
            _contentSize = CalculateContentSize(Content);
        }

        /// <summary>
        /// Gets the relative line number.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the content of the line as an array of segments.
        /// </summary>
        public List<Segment> Content { get; }

        /// <summary>
        /// Gets the size of all content, precomputed for performance.
        /// </summary>
        public int ContentSize => _contentSize;

        public void AddContent(Segment segment)
        {
            Content.Add(segment);
            _contentSize = CalculateContentSize(Content);

        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                // Combine the hash code of the Line property
                hash = hash * 31 + Line.GetHashCode();

                // Combine the hash code of the Content array
                foreach (Segment segment in Content)
                {
                    hash = hash * 31 + (segment?.GetHashCode() ?? 0);
                }

                // Combine the hash code of the ContentSize property
                hash = hash * 31 + _contentSize.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Calculates the total size of the content.
        /// </summary>
        /// <param name="content">The array of segments.</param>
        /// <returns>The total size of the content.</returns>
        private static int CalculateContentSize(List<Segment> content)
        {
            int size = 0;
            foreach (Segment segment in content)
            {
                size += segment.Text.GetWidth();
            }
            return size;
        }
    }
}
