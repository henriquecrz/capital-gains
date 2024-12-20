﻿using capital_gains;

namespace unit_tests;

public class CapitalGainsTests
{
    [Fact]
    public void GetTaxes_GivenSimulation_ShouldReturnsRelatedTaxes()
    {
        var capitalGains = new CapitalGains();
        var simulations = new List<TradeOperation>
        {
            new() { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new() { Operation = "sell", UnitCost = 20.00m, Quantity = 5000 },
            new() { Operation = "sell", UnitCost = 0.00m, Quantity = 5000 }
        };

        var taxes = capitalGains.GetTaxes(simulations);

        Assert.Equivalent(new { tax = 0.00m }, taxes[0]);
        Assert.Equivalent(new { tax = 10000.00m }, taxes[1]);
        Assert.Equivalent(new { tax = 0.00m }, taxes[2]);
    }

    [Fact]
    public void GetTaxes_GivenBlockAccountSimulation_ShouldReturnsAccountBlocked()
    {
        var capitalGains = new CapitalGains();
        var simulations = new List<TradeOperation>
        {
            new() { Operation = "sell", UnitCost = 20.00m, Quantity = 10000 },
            new() { Operation = "sell", UnitCost = 20.00m, Quantity = 10000 },
            new() { Operation = "sell", UnitCost = 20.00m, Quantity = 10000 },
            new() { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new() { Operation = "sell", UnitCost = 20.00m, Quantity = 10000 }
        };

        var taxes = capitalGains.GetTaxes(simulations);

        Assert.Equivalent(new { error = "Can't sell more stocks than you have" }, taxes[0]);
        Assert.Equivalent(new { error = "Can't sell more stocks than you have" }, taxes[1]);
        Assert.Equivalent(new { error = "Can't sell more stocks than you have" }, taxes[2]);
        Assert.Equivalent(new { error = "Your account is blocked" }, taxes[3]);
        Assert.Equivalent(new { error = "Your account is blocked" }, taxes[4]);
    }

    [Fact]
    public void GetBuyTax_WhenQuantityZero_ShouldResetValuesAndReturnTaxZero()
    {
        var capitalGains = new CapitalGains();
        var trade = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 20.00m,
            Quantity = 50
        };

        var tax = capitalGains.GetBuyTax(trade);

        Assert.Equivalent(new { tax = 0.00m }, tax);
    }

    [Fact]
    public void GetBuyTax_WhenQuantityNotZero_ShouldSetNewWeightedAveragePriceAndReturnTaxZero()
    {
        var capitalGains = new CapitalGains();
        var trade = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 20.00m,
            Quantity = 50
        };

        capitalGains.GetBuyTax(trade);

        var tax = capitalGains.GetBuyTax(trade);

        Assert.Equivalent(new { tax = 0.00m }, tax);
    }

    [Fact]
    public void GetNewWeightedAveragePrice_GivenTradeOperation_ShouldReturnNewWeightedAveragePrice()
    {
        var capitalGains = new CapitalGains();
        var trade = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 20.00m,
            Quantity = 50
        };

        var newWeightedAveragePrice = capitalGains.GetNewWeightedAveragePrice(trade);

        Assert.Equal(20.00m, newWeightedAveragePrice);
    }

    [Fact]
    public void GetSellTax_WhenUnitCostEqualThanWeightedAveragePrice_ShouldReturnZeroTax()
    {
        var capitalGains = new CapitalGains();
        var buy = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 10.00m,
            Quantity = 100
        };

        capitalGains.GetBuyTax(buy);

        var sell = new TradeOperation
        {
            Operation = "sell",
            UnitCost = 15.00m,
            Quantity = 50
        };

        var tax = capitalGains.GetSellOutput(sell);

        Assert.Equivalent(new { tax = 0.00m }, tax);
    }

    [Fact]
    public void GetSellTax_WhenSaleValueLessOrEqualThan20000_ShouldReturnZeroTax()
    {
        var capitalGains = new CapitalGains();
        var buy = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 10.00m,
            Quantity = 100
        };

        capitalGains.GetBuyTax(buy);

        var sell = new TradeOperation
        {
            Operation = "sell",
            UnitCost = 15.00m,
            Quantity = 50
        };

        var tax = capitalGains.GetSellOutput(sell);

        Assert.Equivalent(new { tax = 0.00m }, tax);
    }

    [Fact]
    public void GetSellTax_WhenBalanceGreaterThanZero_ShouldReturnCalculatedTax()
    {
        var capitalGains = new CapitalGains();
        var buy = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 10.00m,
            Quantity = 10000
        };

        capitalGains.GetBuyTax(buy);

        var sell = new TradeOperation
        {
            Operation = "sell",
            UnitCost = 20.00m,
            Quantity = 5000
        };

        var tax = capitalGains.GetSellOutput(sell);

        Assert.Equivalent(new { tax = 10000.00m }, tax);
    }

    [Fact]
    public void GetSellTax_WhenUnitCostLessThanWeightedAveragePrice_ShouldReturnZeroTax()
    {
        var capitalGains = new CapitalGains();
        var buy = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 10.00m,
            Quantity = 10000
        };

        capitalGains.GetBuyTax(buy);

        var sell = new TradeOperation
        {
            Operation = "sell",
            UnitCost = 5.00m,
            Quantity = 5000
        };

        var tax = capitalGains.GetSellOutput(sell);

        Assert.Equivalent(new { tax = 0.00m }, tax);
    }

    [Fact]
    public void GetSellTax_WhenTradeQuantityGreaterThanExistingQuantity_ShouldReturnError()
    {
        //var capitalGains = new CapitalGains();
        //var buy = new TradeOperation
        //{
        //    Operation = "buy",
        //    UnitCost = 20.00m,
        //    Quantity = 10000
        //};

        //capitalGains.GetBuyTax(buy);

        //var sell = new TradeOperation
        //{
        //    Operation = "sell",
        //    UnitCost = 2000m,
        //    Quantity = 11000
        //};

        //var error = capitalGains.GetSellOutput(sell);

        //Assert.Equivalent(new { error = "Can't sell more stocks than you have" }, error);
    }

    [Fact]
    public void HasEnoughQuantity_WhenTradeQuantityGreaterThanExistingQuantity_ShouldReturn()
    {
        var capitalGains = new CapitalGains();
        var buy = new TradeOperation
        {
            Operation = "buy",
            UnitCost = 20.00m,
            Quantity = 10000
        };
        capitalGains.GetBuyTax(buy);

        var sell = new TradeOperation
        {
            Operation = "sell",
            UnitCost = 2000m,
            Quantity = 11000
        };
        var hasEnoughQuantity = capitalGains.HasEnoughQuantity(sell);

        Assert.False(hasEnoughQuantity);
    }
}
