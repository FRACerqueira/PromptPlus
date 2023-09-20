// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    internal readonly struct ItemTableHeader
    {
        public ItemTableHeader()
        {
            throw new PromptPlusException("ItemHeader CTOR NotImplemented");
        }

        public ItemTableHeader(string header, Alignment align)
        {
            Header = header;
            Align = align;
            StartColumn = 0;
            EndColumn = 0;
        }

        public ItemTableHeader(string header, Alignment align, byte start,byte end)
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
