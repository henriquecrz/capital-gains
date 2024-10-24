using Newtonsoft.Json;

namespace capital_gains;

public class Program
{
    public static void Main()
    {
        string? input;

        do
        {
            input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                break;
            }

            var simulations = GetSimulations(input);
            var outputs = GetOutputs(simulations);

            Print(outputs);
        }
        while (!string.IsNullOrEmpty(input));
    }

    public static List<List<TradeOperation>> GetSimulations(string input)
    {
        var simulations = new List<List<TradeOperation>>();

        if (input.StartsWith("[["))
        {
            var inputSimulations = JsonConvert.DeserializeObject<List<List<TradeOperation>>>(input)!;

            simulations.AddRange(inputSimulations);
        }
        else
        {
            var inputSimulation = JsonConvert.DeserializeObject<List<TradeOperation>>(input)!;

            simulations.Add(inputSimulation);
        }

        return simulations;
    }

    public static List<List<object>> GetOutputs(List<List<TradeOperation>> simulations)
    {
        return simulations.Select(s =>
        {
            var capitalGains = new CapitalGains();

            return capitalGains.GetTaxes(s);
        }).ToList();
    }

    public static void Print(List<List<object>> outputs)
    {
        foreach (var output in outputs)
        {
            Console.WriteLine(JsonConvert.SerializeObject(output));
        }
    }
}
