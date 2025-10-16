// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Controls.MaskEdit
{
    internal sealed class MaskElement(ElementType type, char token, char promptchar)
    {
        public static readonly char Emptyinputchar = '\u0001';
        public char Token { get; } = token;
        public ElementType Type { get; } = type;
        public string Validchars { get; init; } = string.Empty;
        public string Customchars { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public char Outputchar { get; set; } = promptchar;
        public char Inputchar { get; set; } = Emptyinputchar;
    }
}
