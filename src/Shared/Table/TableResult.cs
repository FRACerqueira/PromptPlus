// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the result returned by table controls.
    /// </summary>
    /// <typeparam name="T">Type of selected row value.</typeparam>
    /// <param name="value">The selected row value.</param>
    /// <param name="rowIndex">The selected row index.</param>
    /// <param name="columnIndex">The selected column index.</param>
    public readonly struct TableResult<T>(T value, int rowIndex, int columnIndex)
    {
        /// <summary>
        /// Selected row value.
        /// </summary>
        public T Value => value;

        /// <summary>
        /// Selected row index.
        /// </summary>
        public int RowIndex => rowIndex;

        /// <summary>
        /// Selected column index.
        /// </summary>
        public int ColumnIndex => columnIndex;

        /// <summary>
        /// Deconstructs the <see cref="TableResult{T}"/> into components.
        /// </summary>
        /// <param name="valueResult">Selected row value.</param>
        /// <param name="row">Selected row index.</param>
        /// <param name="column">Selected column index.</param>
        public void Deconstruct(out T valueResult, out int row, out int column)
        {
            valueResult = Value;
            row = RowIndex;
            column = ColumnIndex;
        }
    }
}
