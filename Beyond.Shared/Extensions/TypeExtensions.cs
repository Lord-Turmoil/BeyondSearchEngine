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

    public static string OrcId(this string fullOrcId)
    {
        return fullOrcId[(fullOrcId.LastIndexOf('/') + 1)..];
    }

    public static string WikiDataId(this string fullWikiDataId)
    {
        return fullWikiDataId[(fullWikiDataId.LastIndexOf('/') + 1)..];
    }

    public static string Doi(this string fullDoi)
    {
        if (string.IsNullOrEmpty(fullDoi))
        {
            return string.Empty;
        }

        // https://doi.org/10.1103/physrevlett.77.3865
        return fullDoi[16..];
    }
}