using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Text.Json;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // HotKeyJsonConverter (Core/HotKeyJsonConverter.cs) — camada 1, unidade pura.
    public class HotKeyJsonConverterTests
    {
        private static JsonSerializerOptions Options() => new() { Converters = { new HotKeyJsonConverter() } };

        [Fact]
        public void Write_serializes_key_and_all_modifier_flags()
        {
            var key = new HotKey(ConsoleKey.F1, alt: true, ctrl: false, shift: true);

            string json = JsonSerializer.Serialize(key, Options());

            _ = json.Should().Be("""{"key":"F1","alt":true,"ctrl":false,"shift":true}""");
        }

        [Fact]
        public void Round_trip_preserves_key_and_modifiers()
        {
            var original = new HotKey(ConsoleKey.Escape, alt: false, ctrl: true, shift: false);
            var options = Options();

            string json = JsonSerializer.Serialize(original, options);
            HotKey roundTripped = JsonSerializer.Deserialize<HotKey>(json, options);

            _ = roundTripped.Should().Be(original);
        }

        [Fact]
        public void Read_is_case_insensitive_on_property_names()
        {
            HotKey key = JsonSerializer.Deserialize<HotKey>("""{"KEY":"A","CTRL":true}""", Options());

            _ = key.Key.Should().Be(ConsoleKey.A);
            _ = key.Ctrl.Should().BeTrue();
        }

        [Fact]
        public void Read_defaults_omitted_modifiers_to_false()
        {
            HotKey key = JsonSerializer.Deserialize<HotKey>("""{"key":"Enter"}""", Options());

            _ = key.Should().Be(new HotKey(ConsoleKey.Enter));
        }

        [Fact]
        public void Read_ignores_unknown_properties()
        {
            HotKey key = JsonSerializer.Deserialize<HotKey>("""{"key":"A","unknown":123}""", Options());

            _ = key.Key.Should().Be(ConsoleKey.A);
        }

        [Fact]
        public void Read_throws_when_the_key_property_is_missing()
        {
            Action act = () => JsonSerializer.Deserialize<HotKey>("""{"ctrl":true}""", Options());
            _ = act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Read_throws_for_an_invalid_ConsoleKey_name()
        {
            Action act = () => JsonSerializer.Deserialize<HotKey>("""{"key":"NotARealKey"}""", Options());
            _ = act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Read_throws_when_key_is_not_a_string()
        {
            Action act = () => JsonSerializer.Deserialize<HotKey>("""{"key":42}""", Options());
            _ = act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Read_throws_when_a_modifier_is_not_a_boolean()
        {
            Action act = () => JsonSerializer.Deserialize<HotKey>("""{"key":"A","ctrl":"yes"}""", Options());
            _ = act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Read_throws_when_the_root_token_is_not_an_object()
        {
            Action act = () => JsonSerializer.Deserialize<HotKey>("\"A\"", Options());
            _ = act.Should().Throw<JsonException>();
        }
    }
}
