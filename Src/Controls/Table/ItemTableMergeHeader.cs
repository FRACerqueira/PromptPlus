// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    internal readonly struct ItemTableMergeHeader
    {
        public ItemTableMergeHeader()
        {
            throw new PromptPlusException("ItemTableMergeHeader CTOR NotImplemented");
        }

        public ItemTableMergeHeader(string header, Alignment align, byte start,byte end)
        {
            Header = header;
            Align = align;
            StartColumn = start;
            EndColumn = end;
        }
        public string Header { get; }
        public Alignment Align { get; }
        public byte StartColumn { get; }
        public  byte EndColumn { get;}
    }
}
