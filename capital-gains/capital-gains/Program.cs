using Newtonsoft.Json;

namespace capital_gains;

internal class Program
{
    const string BUY = "buy";
    const decimal TAX_RATE = 0.2m;

    static void Main(string[] args)
    {
        string input;

        do
        {
            input = Console.ReadLine();
            var inputSimulations = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var inputSimulation in inputSimulations)
            {
                var simulation = JsonConvert.DeserializeObject<List<TradeOperation>>(inputSimulation);

                Analyze(simulation);
            }

        }
        while (!string.IsNullOrEmpty(input));
    }

    public static void Analyze(List<TradeOperation> simulation)
    {
        var weightedAveragePrice = 0m;
        var quantity = 0;
        var balance = 0m;

        var taxes = simulation.Select<TradeOperation, object>(s =>
        {
            if (s.Operation == BUY)
            {
                if (weightedAveragePrice == 0)
                {
                    weightedAveragePrice = s.UnitCost;
                }
                else
                {
                    weightedAveragePrice = (weightedAveragePrice * quantity + s.UnitCost * s.Quantity) / (quantity + s.Quantity);
                }

                quantity += s.Quantity;

                return new { tax = 0.00m };
            }

            if (s.UnitCost > weightedAveragePrice)
            {
                var saleValue = s.UnitCost * s.Quantity;

                if (saleValue <= 20000)
                {
                    return new { tax = 0.00m };
                }

                var profit = saleValue - (s.Quantity * weightedAveragePrice);
                balance += profit;
                var tax = Math.Round(profit * TAX_RATE, 2);

                return new { tax };
            }

            if (s.UnitCost < weightedAveragePrice)
            {
                var saleValue = s.UnitCost * s.Quantity;
                var prejudice = saleValue - (s.Quantity * weightedAveragePrice);

                balance -= prejudice;
            }

            return new { tax = 0.00m };
        });

        Console.WriteLine(JsonConvert.SerializeObject(taxes));
    }
}
