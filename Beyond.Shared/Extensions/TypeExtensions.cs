using Beyond.Shared.Indexer;

namespace Beyond.Shared.Extensions;

static class TypeExtensions
{
    public static string NotNull(this string? stringLiteral)
    {
        return stringLiteral ?? throw new IndexException("Unexpected null");
    }

    public static string NotNull(this string? stringLiteral, string key)
    {
        return stringLiteral ?? throw new IndexException($"Unexpected null of \"{key}\"");
    }

    public static string OpenAlexId(this string fullId)
    {
        return fullId[(fullId.LastIndexOf('/') + 1)..];
    }
}