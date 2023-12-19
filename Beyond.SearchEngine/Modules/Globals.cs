namespace Beyond.SearchEngine.Modules;

public static class Globals
{
    public const int DefaultPageSize = 20;
    public const int DefaultPage = 1;
    public const int MaxPageSize = 100;
    public const int MaxPagePressure = 1000;

    public static readonly HashSet<string> AvailableTypes = [
        "authors",
        "concepts",
        "funders",
        "institutions",
        "publishers",
        "sources",
        "works"
    ];
}