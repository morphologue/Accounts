using System;
using FluentAssertions;
using GavinTech.Accounts.Domain.Primitives;
using Xunit;

namespace GavinTech.Accounts.UnitTests.Domain.Primitives
{
    public class DayTests
    {
        [
            Theory,
            InlineData(0, "1970-01-01"),
            InlineData(-1, "1969-12-31"),
            InlineData(18690, "2021-03-04")
        ]
        public void ToString_FormatsToIsoish(int dayNumber, string expectedResult)
        {
            var patient = new Day(dayNumber);

            var result = patient.ToString();

            result.Should().Be(expectedResult);
        }

        [
            Theory,
            InlineData("2021-03-04T23:11:10Z", 18690),
            InlineData("2021-03-05T00:00:00Z", 18691)
        ]
        public void ConstructorDateTime_RoundsDownToNearestDay(string dateString, int expectedOffset)
        {
            var towardsEndOfUtcDay = DateTime.Parse(dateString).ToUniversalTime();

            var result = new Day(towardsEndOfUtcDay);

            result.Offset.Should().Be(expectedOffset);
        }
    }
}
