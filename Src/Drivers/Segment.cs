﻿// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Drivers.Markup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Drivers
{
    /// <summary>
    /// Represents a renderable part text.
    /// </summary>
    internal class Segment
    {
        //cache style to ansicontrol
        private static readonly Style _styletoAnsicontrol = Style.Default;

        private Segment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="text">The segment text.</param>
        /// <param name="style">The <see cref="Style"/> text.</param>
        public Segment(string text, Style style)
        {
            Text = text.NormalizeNewLines();
            Style = style;
        }

        /// <summary>
        /// Gets the segment style.
        /// </summary>
        public Style Style { get; }

        /// <summary>
        /// Gets the segment text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the segment text.
        /// </summary>
        public bool IsAnsiControl { get; private set; }

        public static Segment[] ParseAnsiControl(string text)
        {
            if (text is null)
            {
                throw new PromptPlusException("ParseAnsiControl with test null");
            }
            return new Segment[] { new Segment(text, _styletoAnsicontrol) { IsAnsiControl = true } };
        }

        public static Segment[] Parse(string text, Style style)
        {
            if (text is null)
            {
                throw new PromptPlusException("ParseAnsiControl with test null");
            }
            var localtext = text.NormalizeNewLines();
            var result = new List<Segment>();
            if (localtext.Length == 0)
            {
                result.Add(new Segment("", style));
                return result.ToArray();
            }

            if (PromptPlus.IgnoreColorTokens)
            {
                result.Add(new Segment(text, style));
                return result.ToArray();
            }

            using var tokenizer = new MarkupTokenizer(localtext);

            var stack = new Stack<Style>();

            while (tokenizer.MoveNext())
            {
                var token = tokenizer.Current;
                if (token == null)
                {
                    break;
                }

                if (token.Kind == MarkupTokenKind.Open)
                {
                    var parsedStyle = StyleParser.Parse(token.Value, style.OverflowStrategy);
                    stack.Push(parsedStyle);
                }
                else if (token.Kind == MarkupTokenKind.Close)
                {
                    if (stack.Count == 0)
                    {
                        throw new PromptPlusException($"Encountered closing tag when none was expected near position {token.Position}.");
                    }

                    stack.Pop();
                }
                else if (token.Kind == MarkupTokenKind.Text)
                {
                    // Get the effective style.
                    var effectiveStyle = Combine(style, stack.Reverse());
                    result.Add(new Segment(token.Value, effectiveStyle));
                }
                else
                {
                    throw new PromptPlusException("Encountered unknown markup token.");
                }
            }
            if (stack.Count > 0)
            {
                throw new PromptPlusException("Unbalanced markup stack. Did you forget to close a tag?");
            }

            return result.ToArray();
        }

        private static Style Combine(Style style, IEnumerable<Style> source)
        {
            var current = style;
            foreach (var item in source)
            {
                current = current.Combine(item);
            }

            return current;
        }
    }
}
