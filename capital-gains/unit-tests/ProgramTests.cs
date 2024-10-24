using capital_gains;

namespace unit_tests;

public class ProgramTests
{
    [Fact]
    public void GetSimulations_GivenSingleInput()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";

        var simulations = Program.GetSimulations(input);

        Assert.Single(simulations);

        AssertFirstSimulation(simulations.First());
    }

    [Fact]
    public void GetSimulations_GivenMultipleInput()
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
    public void Print_GivenOutput()
    {
        var output = new List<List<object>>
        {
            new() { new { tax = 0.00m }, new { tax = 0.00m } },
            new() { new { tax = 0.00m }, new { tax = 10000.00m } }
        };

        using var writer = new StringWriter();
        Console.SetOut(writer);

        Program.Print(output);

        var expected = "[{\"tax\":0.00},{\"tax\":0.00}]\r\n[{\"tax\":0.00},{\"tax\":10000.00}]\r\n";
        Assert.Equal(expected, writer.ToString());
    }






    [Fact]
    public void Main_Case1()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]", output);
    }

    [Fact]
    public void Main_Case2()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]", output);
    }

    [Fact]
    public void Main_Case1And2()
    {
        var input = "[[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}],[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]\r\n[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]", output);
    }

    [Fact]
    public void Main_Case3()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 3000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":1000.00}]", output);
    }

    [Fact]
    public void Main_Case4()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]", output);
    }

    [Fact]
    public void Main_Case5()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 5000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":10000.00}]", output);
    }

    [Fact]
    public void Main_Case6()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00}]", output);
    }

    [Fact]
    public void Main_Case7()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}, {\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}, {\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 5000}, {\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 4350}, {\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 650}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3700.00},{\"tax\":0.00}]", output);
    }

    [Fact]
    public void Main_Case8()
    {
        var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}, {\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}, {\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}]";

        using var inputReader = new StringReader(input + "\n");
        using var outputWriter = new StringWriter();
        Console.SetIn(inputReader);
        Console.SetOut(outputWriter);

        Program.Main();

        var output = outputWriter.ToString();
        Assert.Contains("[{\"tax\":0.00},{\"tax\":80000.00},{\"tax\":0.00},{\"tax\":60000.00}]", output);
    }
}
