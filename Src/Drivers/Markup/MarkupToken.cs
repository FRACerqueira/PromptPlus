// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
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
