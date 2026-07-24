// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// JSON converter for serializing and deserializing Color as hexadecimal string.
    /// </summary>
    internal sealed class ColorJsonConverter : JsonConverter<Color>
    {
        /// <summary>
        /// Reads a Color from JSON hex string format.
        /// </summary>
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing Color. Expected string in hex format (#RRGGBB).");
            }

            string? hexValue = reader.GetString();
            if (string.IsNullOrEmpty(hexValue))
            {
                throw new JsonException("Color hex value cannot be null or empty.");
            }

            try
            {
                return Color.FromHex(hexValue);
            }
            catch (Exception ex) when (ex is ArgumentException or FormatException)
            {
                throw new JsonException($"Invalid Color hex format: {hexValue}. Expected format: #RRGGBB", ex);
            }
        }

        /// <summary>
        /// Writes a Color to JSON as hexadecimal string.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            string hexValue = $"#{value.ToHex()}";
            writer.WriteStringValue(hexValue);
        }
    }
}
