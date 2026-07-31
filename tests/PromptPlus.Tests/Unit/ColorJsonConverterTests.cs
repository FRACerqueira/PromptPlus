using ConsolePlusLibrary;
using FluentAssertions;
using PromptPlusLibrary.Core;
using System;
using System.Text.Json;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // PromptPlus/src/Core/ColorJsonConverter.cs — pure unit-level. Exact duplicate of
    // ConsolePlus's own ColorJsonConverter (different assembly/namespace, compiled independently),
    // carrying the SAME latent bug found there: fixing ConsolePlus's copy does not fix this one.
    public class ColorJsonConverterTests
    {
        private static JsonSerializerOptions Options() => new() { Converters = { new ColorJsonConverter() } };

        [Fact]
        public void Write_serializes_the_color_as_a_hash_prefixed_hex_string()
        {
            string json = JsonSerializer.Serialize(new Color(255, 0, 128), Options());
            _ = json.Should().Be("\"#FF0080\"");
        }

        [Fact]
        public void Round_trip_preserves_the_color()
        {
            var original = new Color(37, 201, 88);
            var options = Options();

            string json = JsonSerializer.Serialize(original, options);
            Color roundTripped = JsonSerializer.Deserialize<Color>(json, options);

            _ = roundTripped.Should().Be(original);
        }

        // Regression for the same bug fixed in ConsolePlus's ColorJsonConverter, duplicated here
        // because this is a separate class in a separate assembly: Color.FromHex throws
        // FormatException for non-hex digits (via byte.Parse), but Read only caught
        // ArgumentException. Fixed to also catch FormatException.
        [Fact]
        public void Read_wraps_invalid_hex_digits_in_a_JsonException_instead_of_leaking_FormatException()
        {
            Action act = () => JsonSerializer.Deserialize<Color>("\"#GGGGGG\"", Options());

            _ = act.Should().Throw<JsonException>()
                .WithInnerException<FormatException>();
        }

        [Fact]
        public void Read_throws_JsonException_for_an_empty_string()
        {
            Action act = () => JsonSerializer.Deserialize<Color>("\"\"", Options());
            _ = act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Read_throws_JsonException_for_a_non_string_token()
        {
            Action act = () => JsonSerializer.Deserialize<Color>("123", Options());
            _ = act.Should().Throw<JsonException>();
        }
    }
}
