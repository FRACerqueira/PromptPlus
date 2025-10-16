// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using System;

namespace PromptPlusLibrary.Core.Markup
{
    internal sealed class MarkupToken(MarkupTokenKind kind, string value, int position)
    {
        public MarkupTokenKind Kind { get; } = kind;
        public string Value { get; } = value ?? throw new ArgumentNullException(nameof(value), "Color MarkupToken with value null");
        public int Position { get; } = position;
    }
}
