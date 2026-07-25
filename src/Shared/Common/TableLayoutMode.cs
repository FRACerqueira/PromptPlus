// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines the visual character set and rendering mode used by table borders and separators.
    /// </summary>
    public enum TableLayoutMode
    {
        /// <summary>
        /// Uses single Unicode box-drawing characters.
        /// </summary>
        SingleBox,
        /// <summary>
        /// Uses double Unicode box-drawing characters.
        /// </summary>
        DoubleBox,
        /// <summary>
        /// Uses single plain ASCII characters.
        /// </summary>
        SingleASCII,
        /// <summary>
        /// Uses double plain ASCII characters.
        /// </summary>
        DoubleASCII,
        /// <summary>
        /// Disables table border characters.
        /// </summary>
        None
    }
}
