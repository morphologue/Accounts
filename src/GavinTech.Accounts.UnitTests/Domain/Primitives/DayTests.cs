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
            var dateTime = DateTime.Parse(dateString).ToUniversalTime();

            var result = new Day(dateTime);

            result.Offset.Should().Be(expectedOffset);
        }

        [Fact]
        public void ConstructorString_UnderstandsDateAsUtc()
        {
            var dateString = "2021-03-04";

            var result = new Day(dateString);

            result.Offset.Should().Be(18690);
        }

        [
            Theory,
            InlineData("2021-03-04Z"),
            InlineData(" 2021-03-04"),
            InlineData("2021-03-04 "),
            InlineData("2021-3-04"),
            InlineData("2021-03-4"),
            InlineData("2021-03-04T23:11:10Z")
        ]
        public void ConstructorString_ThrowsFormatException_GivenAnyDeviationFromTheFormat(string deviation)
        {
            Func<Day> action = () => new Day(deviation);

            action.Should().Throw<FormatException>();
        }

    }
}
