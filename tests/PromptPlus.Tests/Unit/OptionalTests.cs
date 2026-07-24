using FluentAssertions;
using PromptPlusLibrary.Controls.Common;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // Optional<T> (Controls/Common/Optional.cs) — camada 1, unidade pura. Support type used to
    // distinguish "no default value provided" from an actual default value across most controls'
    // constructors and Paginator<T>.
    public class OptionalTests
    {
        [Fact]
        public void Set_wraps_a_value_with_HasValue_true()
        {
            var opt = Optional<int>.Set(5);
            _ = opt.HasValue.Should().BeTrue();
            _ = opt.Value.Should().Be(5);
        }

        [Fact]
        public void Empty_has_no_value()
        {
            _ = Optional<int>.Empty().HasValue.Should().BeFalse();
        }

        [Fact]
        public void Two_Set_instances_with_the_same_value_are_equal()
        {
            _ = (Optional<int>.Set(5) == Optional<int>.Set(5)).Should().BeTrue();
        }

        [Fact]
        public void Two_Set_instances_with_different_values_are_not_equal()
        {
            _ = (Optional<int>.Set(5) == Optional<int>.Set(6)).Should().BeFalse();
        }

        [Fact]
        public void Two_Empty_instances_are_equal_regardless_of_the_underlying_default()
        {
            _ = (Optional<int>.Empty() == Optional<int>.Empty()).Should().BeTrue();
        }

        [Fact]
        public void Set_and_Empty_are_never_equal_even_when_the_set_value_is_the_types_default()
        {
            _ = (Optional<int>.Set(0) == Optional<int>.Empty()).Should().BeFalse();
        }

        [Fact]
        public void Implicit_cast_to_T_returns_the_wrapped_value()
        {
            int value = Optional<int>.Set(7);
            _ = value.Should().Be(7);
        }

        [Fact]
        public void Implicit_cast_of_Empty_returns_the_types_default()
        {
            int value = Optional<int>.Empty();
            _ = value.Should().Be(0);
        }

        // Regression for a real (latent, unused-in-production) bug found while writing this suite:
        // operator ==(T, Optional<T>) compared the raw value against right.Value without checking
        // right.HasValue, so an Empty() optional spuriously equaled the type's default value (e.g.
        // 0 == Optional<int>.Empty() was true). Fixed to require HasValue first. Confirmed via
        // exploration that no production code exercises this operator today (always .HasValue/.Value).
        [Fact]
        public void Raw_value_never_equals_an_Empty_optional_even_when_it_matches_the_type_default()
        {
            _ = (0 == Optional<int>.Empty()).Should().BeFalse();
            _ = (0 != Optional<int>.Empty()).Should().BeTrue();

            string? nullStr = null;
            _ = (nullStr == Optional<string?>.Empty()).Should().BeFalse();
        }

        [Fact]
        public void Raw_value_equals_a_Set_optional_holding_the_same_value()
        {
            _ = (5 == Optional<int>.Set(5)).Should().BeTrue();
            _ = (6 == Optional<int>.Set(5)).Should().BeFalse();
        }

        [Fact]
        public void Equals_object_overload_handles_boxed_Optional_and_boxed_raw_value()
        {
            object boxedOptional = Optional<int>.Set(5);
            object boxedRaw = 5;

            _ = Optional<int>.Set(5).Equals(boxedOptional).Should().BeTrue();
            _ = Optional<int>.Set(5).Equals(boxedRaw).Should().BeTrue();
            _ = Optional<int>.Empty().Equals(boxedRaw).Should().BeFalse();
            _ = Optional<int>.Set(5).Equals("not an int").Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_is_consistent_with_Equals_for_Set_values()
        {
            _ = Optional<int>.Set(5).GetHashCode().Should().Be(Optional<int>.Set(5).GetHashCode());
        }
    }
}
