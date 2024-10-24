using capital_gains;

namespace unit_tests;

public class CapitalGainsTests
{
    [Fact]
    public void GetTaxes_GivenSingleBuyOperation_ReturnsZeroTax()
    {
        var simulation = new List<TradeOperation>
        {
            new TradeOperation { Operation = "buy", UnitCost = 10.00m, Quantity = 100 }
        };

        var taxes = CapitalGains.GetTaxes(simulation);

        Assert.Single(taxes);
        Assert.Equal(0.00m, taxes.First().tax);
    }

    [Fact]
    public void GetTaxes_GivenSingleBuyOperation_ReturnsZeroTax()
    {
        var simulation = new List<TradeOperation>
        {
            new TradeOperation { Operation = "buy", UnitCost = 10.00m, Quantity = 100 }
        };

        var taxes = CapitalGains.GetTaxes(simulation);

        Assert.Single(taxes);
        Assert.Equal(0.00m, taxes.First().tax);
    }
}
