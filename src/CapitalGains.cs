namespace capital_gains;

public class CapitalGains
{
    const string BUY = "buy";
    const string SELL = "sell";
    const decimal TAX_RATE = 0.2m;

    private int _quantity;
    private decimal _weightedAveragePrice;
    private decimal _balance;
    private int _errorCount = 0;

    public List<object> GetTaxes(List<TradeOperation> simulation)
    {
        return simulation.Select(trade =>
        {
            var error = ValidateOperation(trade);

            if (error is not null)
            {
                return error;
            }

            if (trade.Operation == BUY)
            {
                return GetBuyTax(trade);
            }

            return GetSellOutput(trade);

        }).ToList();
    }

    public object GetBuyTax(TradeOperation trade)
    {
        if (_quantity == 0)
        {
            _balance = 0m;
            _weightedAveragePrice = trade.UnitCost;
        }
        else
        {
            _weightedAveragePrice = GetNewWeightedAveragePrice(trade);
        }

        _quantity += trade.Quantity;

        return new { tax = 0.00m };
    }

    public decimal GetNewWeightedAveragePrice(TradeOperation trade) =>
        (_weightedAveragePrice * _quantity + trade.UnitCost * trade.Quantity) / (_quantity + trade.Quantity);

    public object? ValidateOperation(TradeOperation trade)
    {
        if (IsAccountBlocked())
        {
            return new { error = "Your account is blocked" };
        }

        if (trade.Operation == SELL)
        {
            if (!HasEnoughQuantity(trade))
            {
                _errorCount++;
                return new { error = "Can't sell more stocks than you have" };
            }
        }

        return null;
    }

    public bool IsAccountBlocked() => _errorCount >= 3;

    public bool HasEnoughQuantity(TradeOperation trade)
    {
        return trade.Quantity <= _quantity;
    }

    public object GetSellOutput(TradeOperation trade)
    {
        _quantity -= trade.Quantity;

        var saleValue = trade.UnitCost * trade.Quantity;

        if (trade.UnitCost > _weightedAveragePrice)
        {
            if (saleValue <= 20000)
            {
                return new { tax = 0.00m };
            }

            var profit = saleValue - (trade.Quantity * _weightedAveragePrice);

            _balance += profit;

            if (_balance > 0)
            {
                var tax = Math.Round(_balance * TAX_RATE, 2);

                return new { tax };
            }
        }

        if (trade.UnitCost < _weightedAveragePrice)
        {
            var prejudice = (trade.Quantity * _weightedAveragePrice) - saleValue;

            _balance -= prejudice;
        }

        return new { tax = 0.00m };
    }
}
