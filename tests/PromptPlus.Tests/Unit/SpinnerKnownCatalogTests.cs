using FluentAssertions;
using PromptPlusLibrary.Core;
using System;
using System.Reflection;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // SpinnerBase.Known (Core/SpinnerInstance.Known.cs) — mostly static data, but a
    // cheap reflective sweep over every catalog entry catches a copy-paste data mistake (empty
    // frame list, zero/negative interval) for free.
    public class SpinnerKnownCatalogTests
    {
        [Fact]
        public void Every_known_spinner_has_at_least_one_frame_and_a_positive_interval()
        {
            PropertyInfo[] props = typeof(SpinnerBase.Known).GetProperties(BindingFlags.Public | BindingFlags.Static);
            _ = props.Should().NotBeEmpty();

            foreach (PropertyInfo prop in props)
            {
                var spinner = (SpinnerBase)prop.GetValue(null)!;
                _ = spinner.Frames.Should().NotBeNullOrEmpty(because: $"{prop.Name} should define at least one frame");
                _ = spinner.Interval.Should().BePositive(because: $"{prop.Name} should have a positive interval");
            }
        }
    }
}
