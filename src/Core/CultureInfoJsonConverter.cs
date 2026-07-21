// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Converts a <see cref="CultureInfo"/> to and from JSON using the culture name.
    /// </summary>
    internal sealed class CultureInfoJsonConverter : JsonConverter<CultureInfo>
    {
        /// <inheritdoc/>
        public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing CultureInfo. Expected String.");
            }

            var cultureName = reader.GetString();
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return CultureInfo.InvariantCulture;
            }

            try
            {
                return new CultureInfo(cultureName);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.InvariantCulture;
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Name);
        }
    }
}
