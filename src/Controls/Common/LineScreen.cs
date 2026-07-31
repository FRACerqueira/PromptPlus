// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlusLibrary.Controls.Common
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
        /// <param name="content">The content of the line as an array of segments.</param>
        public LineScreen(Fragment[] content)
        {
            Content = new List<Fragment>(content.Length);
            foreach (Fragment s in content)
            {
                Content.Add(s);
                if (s.Type == FragmentKind.ContentText)
                {
                    _contentSize += s.Text.GetDisplayLength().Sum();
                }
            }
        }

        /// <summary>
        /// Gets the content of the line as an array of segments.
        /// </summary>
        public List<Fragment> Content { get; }

        /// <summary>
        /// Gets the size of all content, precomputed for performance.
        /// </summary>
        public int ContentSize => _contentSize;

        public bool ContentEquals(LineScreen? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_contentSize != other._contentSize || Content.Count != other.Content.Count)
            {
                return false;
            }

            for (int i = 0; i < Content.Count; i++)
            {
                if (!Content[i].Equals(other.Content[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void AddContent(Fragment segment)
        {
            Content.Add(segment);
            if (segment.Type == FragmentKind.ContentText)
            {
                _contentSize += segment.Text.GetDisplayLength().Sum();
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hash = new();
            foreach (Fragment segment in Content)
            {
                hash.Add(segment.GetHashCode());
            }
            return hash.ToHashCode();
        }

    }
}
