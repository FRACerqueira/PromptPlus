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
    /// Defines which table border elements are hidden when rendering.
    /// <see cref="None"/> (default) renders all elements.
    /// Combine flags to hide multiple elements at once.
    /// </summary>
    [Flags]
    public enum HideTable
    {
        /// <summary>
        /// Show all border elements (default).
        /// </summary>
        None = 0,
        /// <summary>
        /// Hide horizontal separators between data rows.
        /// </summary>
        RowSeparator = 1,
        /// <summary>
        /// Hide the entire header row (column titles) and the separator line between header and data.
        /// When set, no header content is rendered; the top border connects directly to the data rows.
        /// </summary>
        Header = 2,
        /// <summary>
        /// Hide vertical separators between columns.
        /// </summary>
        ColumnSeparator = 4,
        /// <summary>
        /// Hide the outer frame border (top, bottom, left and right edges).
        /// </summary>
        OuterBorder = 8,
    }
}

