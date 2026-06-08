namespace StorageApi.DTOs;

public sealed class ProductDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int Price { get; init; }

    public int Count { get; init; }

    public long InventoryValue => (long)Price * Count;
}
