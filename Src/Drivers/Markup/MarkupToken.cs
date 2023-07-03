// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/spectreconsole/spectre.console
// ***************************************************************************************

namespace PPlus.Drivers.Markup
{
    internal sealed class MarkupToken
    {
        public MarkupTokenKind Kind { get; }
        public string Value { get; }
        public int Position { get; set; }

        public MarkupToken(MarkupTokenKind kind, string value, int position)
        {
            Kind = kind;
            Value = value ?? throw new PromptPlusException("MarkupToken with value null");
            Position = position;
        }
    }
}
