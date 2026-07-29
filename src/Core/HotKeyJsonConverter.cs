// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Provides JSON serialization and deserialization for the <see cref="HotKey"/> struct.
    /// </summary>
    /// <remarks>
    /// This converter handles the conversion of <see cref="HotKey"/> objects to and from JSON format.
    /// The JSON representation uses a compact object format with the key and modifier flags.
    /// </remarks>
    internal sealed class HotKeyJsonConverter : JsonConverter<HotKey>
    {
        private const string KeyPropertyName = "key";
        private const string AltPropertyName = "alt";
        private const string CtrlPropertyName = "ctrl";
        private const string ShiftPropertyName = "shift";

        /// <summary>
        /// Reads and converts the JSON to type <see cref="HotKey"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="JsonException">Thrown when the JSON is invalid or missing required properties.</exception>
        public override HotKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing HotKey. Expected StartObject.");
            }

            ConsoleKey key = default;
            bool alt = false;
            bool ctrl = false;
            bool shift = false;
            bool hasKey = false;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Unexpected token {reader.TokenType} when parsing HotKey properties.");
                }

                string? propertyName = reader.GetString();

                reader.Read();

                switch (propertyName?.ToLowerInvariant())
                {
                    case KeyPropertyName:
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException($"Expected string for '{KeyPropertyName}' property, but got {reader.TokenType}.");
                        }
                        string? keyString = reader.GetString();
                        if (!Enum.TryParse<ConsoleKey>(keyString, ignoreCase: true, out key))
                        {
                            throw new JsonException($"Invalid ConsoleKey value: '{keyString}'.");
                        }
                        hasKey = true;
                        break;

                    case AltPropertyName:
                        if (reader.TokenType != JsonTokenType.True && reader.TokenType != JsonTokenType.False)
                        {
                            throw new JsonException($"Expected boolean for '{AltPropertyName}' property, but got {reader.TokenType}.");
                        }
                        alt = reader.GetBoolean();
                        break;

                    case CtrlPropertyName:
                        if (reader.TokenType != JsonTokenType.True && reader.TokenType != JsonTokenType.False)
                        {
                            throw new JsonException($"Expected boolean for '{CtrlPropertyName}' property, but got {reader.TokenType}.");
                        }
                        ctrl = reader.GetBoolean();
                        break;

                    case ShiftPropertyName:
                        if (reader.TokenType != JsonTokenType.True && reader.TokenType != JsonTokenType.False)
                        {
                            throw new JsonException($"Expected boolean for '{ShiftPropertyName}' property, but got {reader.TokenType}.");
                        }
                        shift = reader.GetBoolean();
                        break;

                    default:
                        break;
                }
            }

            if (!hasKey)
            {
                throw new JsonException($"Missing required property '{KeyPropertyName}' when parsing HotKey.");
            }

            return new HotKey(key, alt, ctrl, shift);
        }

        /// <summary>
        /// Writes a <see cref="HotKey"/> value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, HotKey value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(KeyPropertyName, value.Key.ToString());
            writer.WriteBoolean(AltPropertyName, value.Alt);
            writer.WriteBoolean(CtrlPropertyName, value.Ctrl);
            writer.WriteBoolean(ShiftPropertyName, value.Shift);
            writer.WriteEndObject();
        }
    }
}
