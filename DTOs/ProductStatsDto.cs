namespace StorageApi.DTOs;

public sealed class ProductStatsDto
{
    public int TotalProducts { get; init; }

    public decimal TotalInventoryValue { get; init; }

    public decimal AveragePrice { get; init; }
}
