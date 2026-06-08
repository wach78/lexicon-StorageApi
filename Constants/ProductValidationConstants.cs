namespace StorageApi.Constants;

public static class ProductValidationConstants
{
    public const int NameMinimumLength = 2;
    public const int NameMaximumLength = 100;

    public const int CategoryMinimumLength = 2;
    public const int CategoryMaximumLength = 50;

    public const int ShelfMinimumLength = 1;
    public const int ShelfMaximumLength = 20;

    public const int DescriptionMaximumLength = 500;

    public const int MinimumPrice = 0;
    public const int MaximumPrice = 1_000_000;

    public const int MinimumCount = 0;
    public const int MaximumCount = 100_000;
}
