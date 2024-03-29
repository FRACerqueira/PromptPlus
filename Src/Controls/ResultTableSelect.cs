﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Table Select Result <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Typeof return</typeparam>
    public class ResultTableSelect<T>
    {
        /// <summary>
        /// Create a ResultPrompt
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultTableSelect()
        {
            throw new PromptPlusException("ResultTable CTOR NotImplemented");
        }

        /// <summary>
        /// Create ResultGrid instance.
        /// </summary>
        /// <param name="row">Row number</param>
        /// <param name="column">Column number</param>
        /// <param name="rowvalue">Row value</param>
        /// <param name="columnvalue">Column value</param>
        public ResultTableSelect(int row, int column,T rowvalue, object columnvalue)
        {
            Row = row;
            Column = column;
            RowValue = rowvalue;
            ColumnValue = columnvalue;
        }

        internal static ResultTableSelect<T> NullResult()
        {
            return new ResultTableSelect<T>(-1,-1,default,null);
        }

        /// <summary>
        /// Row number, base 0.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Column number, base 0.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// <typeparamref name="T"/> Row value
        /// </summary>
        public T RowValue { get; }

        /// <summary>
        /// <typeparamref name="T"/> Column value
        /// </summary>
        public object ColumnValue { get; }

    }
}
