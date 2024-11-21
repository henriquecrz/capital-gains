using capital_gains;

namespace unit_tests;

public class ProgramTests
{
    [Theory]
    [MemberData(nameof(GetCasesData))]
    public void Main_GivenCases_ShouldPrintOutputs(string input, string expected)
    {
        using var inputReader = new StringReader(input + Environment.NewLine);
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains(expected, output);
    }

    [Fact]
    public void GetSimulations_GivenSingleInput_ReturnSimulations()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";

        var simulations = Program.GetSimulations(input);

        Assert.Single(simulations);

        AssertFirstSimulation(simulations.First());
    }

    [Fact]
    public void GetSimulations_GivenMultipleInput_ReturnSimulations()
    {
        var input = "[[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}],[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}]]";

        var simulations = Program.GetSimulations(input);

        Assert.Equal(2, simulations.Count);

        AssertFirstSimulation(simulations[0]);
        AssertSecondSimulation(simulations[1]);
    }

    private static void AssertFirstSimulation(List<TradeOperation> operations)
    {
        var firstOperation = operations[0];
        Assert.Equal("buy", firstOperation.Operation);
        Assert.Equal(10.00m, firstOperation.UnitCost);
        Assert.Equal(100, firstOperation.Quantity);

        var secondOperation = operations[1];
        Assert.Equal("sell", secondOperation.Operation);
        Assert.Equal(15.00m, secondOperation.UnitCost);
        Assert.Equal(50, secondOperation.Quantity);
    }

    private static void AssertSecondSimulation(List<TradeOperation> operations)
    {
        var firstOperation = operations[0];
        Assert.Equal("buy", firstOperation.Operation);
        Assert.Equal(10.00m, firstOperation.UnitCost);
        Assert.Equal(10000, firstOperation.Quantity);

        var secondOperation = operations[1];
        Assert.Equal("sell", secondOperation.Operation);
        Assert.Equal(20.00m, secondOperation.UnitCost);
        Assert.Equal(5000, secondOperation.Quantity);
    }

    [Fact]
    public void GetOutputs_GivenSimulations_ShouldReturnOutputs()
    {
        var simulations = new List<List<TradeOperation>>
        {
            new()
            {
                new TradeOperation { Operation = "buy", UnitCost = 10.00m, Quantity = 100 },
                new TradeOperation { Operation = "sell", UnitCost = 15.00m, Quantity = 50 }
            },
            new()
            {
                new TradeOperation { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
                new TradeOperation { Operation = "sell", UnitCost = 20.00m, Quantity = 5000 }
            }
        };

        var outputs = Program.GetOutputs(simulations);

        Assert.Equal(2, outputs.Count);

        AssertFirstOutput(outputs[0]);
        AssertSecondOutput(outputs[1]);
    }

    private static void AssertFirstOutput(List<object> outputs)
    {
        Assert.Equivalent(new { tax = 0.00m }, outputs[0]);
        Assert.Equivalent(new { tax = 0.00m }, outputs[1]);
    }

    private static void AssertSecondOutput(List<object> outputs)
    {
        Assert.Equivalent(new { tax = 0.00 }, outputs[0]);
        Assert.Equivalent(new { tax = 10000.00 }, outputs[1]);
    }

    [Fact]
    public void Print_GivenOutput_ShouldPrintOutputs()
    {
        using var writer = new StringWriter();
        Console.SetOut(writer);

        var output = new List<List<object>>
        {
            new() { new { tax = 0.00m }, new { tax = 0.00m } },
            new() { new { tax = 0.00m }, new { tax = 10000.00m } }
        };

        Program.Print(output);

        var expected = "[{\"tax\":0.00},{\"tax\":0.00}]" + Environment.NewLine + "[{\"tax\":0.00},{\"tax\":10000.00}]" + Environment.NewLine;
        Assert.Equal(expected, writer.ToString());
    }

    public static TheoryData<string, string> GetCasesData() => new()
    {
        // Case 1
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]"
        },

        // Case 2
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]",
            "[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]"
        },

        // Case 1 + Case 2
        {
            "[[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}],[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]" + Environment.NewLine + "[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]"
        },

        // Case 3
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 3000}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":1000.00}]"
        },

        // Case 4
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]"
        },

        // Case 5
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 5000}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":10000.00}]"
        },

        // Case 6
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00}]"
        },

        // Case 7
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}, {\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 4350}, {\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 650}]",
            "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3700.00},{\"tax\":0.00}]"
        },

        // Case 8
        {
            "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}]",
            "[{\"tax\":0.00},{\"tax\":80000.00},{\"tax\":0.00},{\"tax\":60000.00}]"
        },
    };
}
