using FluentAssertions;
using GavinTech.Accounts.Domain.Primitives;
using Xunit;

namespace GavinTech.Accounts.UnitTests.Domain.Primitives;

public class AmountTests
{
    [
        Theory,
        InlineData(0, "0.00"),
        InlineData(-1, "-0.01"),
        InlineData(-9999999, "-99999.99"),
        InlineData(1, "0.01"),
        InlineData(110, "1.10"),
        InlineData(9999900, "99999.00")
    ]
    public void ToString_FormatsTo2Dp(int centCount, string expectedString)
    {
        var patient = new Amount(centCount);

        var result = patient.ToString();

        result.Should().Be(expectedString);
    }

    [Fact]
    public void ToDecimal_ReturnsOneHundredthOfCentCount()
    {
        var patient = new Amount(-10067);

        var result = patient.ToDecimal();

        result.Should().Be(-100.67m);
    }

    [Fact]
    public void DefaultConstructor_SetsCentCountToZero()
    {
        var patient = new Amount();

        var result = patient.CentCount;

        result.Should().Be(0);
    }
}