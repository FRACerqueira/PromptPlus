// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Drivers.Colors;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PPlus.Drivers
{
    internal static class StyleParser
    {
        public static Style Parse(string text, Overflow overflow = Overflow.None)
        {
            var style = Parse(text, overflow, out var error);
            if (error != null)
            {
                throw new PromptPlusException(error);
            }

            if (!style.HasValue)
            {
                // This should not happen, but we need to please the compiler
                // which cannot know that style isn't null here.
                throw new PromptPlusException("Could not parse style.");
            }

            return style.Value;
        }

        private static Style? Parse(string text,Overflow overflow,  out string? error)
        {
            var effectiveForeground = (Color?)null;
            var effectiveBackground = (Color?)null;

            var parts = text.Split(new[] { ' ' });
            if (parts.Length == 1 && parts[0].Contains(':'))
            {
                var index = parts[0].IndexOf(':');
                parts = new string[] { parts[0][..index], "on", parts[0][(index + 1)..] };
            }
            var foreground = true;
            foreach (var part in parts)
            {
                if (part.Equals("on", StringComparison.OrdinalIgnoreCase))
                {
                    foreground = false;
                    continue;
                }

                var color = ColorTable.GetColor(part);
                if (color == null)
                {
                    if (part.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                    {
                        color = ParseHexColor(part, out error);
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            return null;
                        }
                    }
                    else if (part.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
                    {
                        color = ParseRgbColor(part, out error);
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            return null;
                        }
                    }
                    else if (int.TryParse(part, out var number))
                    {
                        if (number < 0)
                        {
                            error = $"Color number must be greater than or equal to 0 (was {number})";
                            return null;
                        }
                        else if (number > 255)
                        {
                            error = $"Color number must be less than or equal to 255 (was {number})";
                            return null;
                        }

                        color = number;
                    }
                    else
                    {
                        error = !foreground
                            ? $"Could not find color '{part}'."
                            : $"Could not find color or style '{part}'.";

                        return null;
                    }
                }

                if (foreground)
                {
                    if (effectiveForeground != null)
                    {
                        error = "A foreground color has already been set.";
                        return null;
                    }

                    effectiveForeground = color;
                }
                else
                {
                    if (effectiveBackground != null)
                    {
                        error = "A background color has already been set.";
                        return null;
                    }

                    effectiveBackground = color;
                }
            }

            error = null;
            return new Style(
                effectiveForeground ?? Color.DefaultForecolor,
                effectiveBackground ?? Color.DefaultBackcolor,
                overflow);
        }

        [SuppressMessage("Style", "IDE0057:Use range operator", Justification = "<Pending>")]
        private static Color? ParseHexColor(string hex, out string? error)
        {
            error = null;

            hex ??= string.Empty;
            hex = hex.Replace("#", string.Empty, StringComparison.Ordinal).Trim();

            try
            {
                if (!string.IsNullOrWhiteSpace(hex))
                {
                    if (hex.Length == 6)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(hex.Substring(0, 2), 16),
                            (byte)Convert.ToUInt32(hex.Substring(2, 2), 16),
                            (byte)Convert.ToUInt32(hex.Substring(4, 2), 16));
                    }
                    else if (hex.Length == 3)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(new string(hex[0], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[1], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[2], 2), 16));
                    }
                }
            }
            catch (Exception ex)
            {
                error = $"Invalid hex color '#{hex}'. {ex.Message}";
                return null;
            }

            error = $"Invalid hex color '#{hex}'.";
            return null;
        }

        [SuppressMessage("Style", "IDE0057:Use range operator", Justification = "<Pending>")]
        private static Color? ParseRgbColor(string rgb, out string? error)
        {
            try
            {
                error = null;

                var normalized = rgb ?? string.Empty;
                if (normalized.Length >= 3)
                {
                    // Trim parentheses
                    normalized = normalized.Substring(3).Trim();

                    if (normalized.StartsWith("(", StringComparison.OrdinalIgnoreCase) &&
                       normalized.EndsWith(")", StringComparison.OrdinalIgnoreCase))
                    {
                        normalized = normalized.Trim('(').Trim(')');

                        var parts = normalized.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 3)
                        {
                            return new Color(
                                (byte)Convert.ToInt32(parts[0], CultureInfo.InvariantCulture),
                                (byte)Convert.ToInt32(parts[1], CultureInfo.InvariantCulture),
                                (byte)Convert.ToInt32(parts[2], CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = $"Invalid RGB color '{rgb}'. {ex.Message}";
                return null;
            }

            error = $"Invalid RGB color '{rgb}'.";
            return null;
        }
    }
}
