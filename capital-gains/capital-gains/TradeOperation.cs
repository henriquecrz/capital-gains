using Newtonsoft.Json;

namespace capital_gains;

public class TradeOperation
{
    public required string Operation { get; set; }

    [JsonProperty("unit-cost")]
    public decimal UnitCost { get; set; }

    public int Quantity { get; set; }
}
