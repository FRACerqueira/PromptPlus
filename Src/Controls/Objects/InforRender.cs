// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls.Objects
{
    internal struct InforRender
    {
        public InforRender(int top, int qtdlines)
        {
            CursorTop = top;
            QtdLines = qtdlines;
        }
        public int CursorTop { get; set; }
        public int QtdLines { get; }
    }
}
