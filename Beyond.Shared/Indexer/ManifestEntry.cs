using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer;

public class ManifestEntry
{
    private const string UrlPrefix = "s3://openalex/data/";

    /// <summary>
    ///     {
    ///     "url": "s3://openalex/data/institutions/updated_date=2023-10-07/part_000.gz",
    ///     "meta": {
    ///     "content_length": 8959,
    ///     "record_count": 15
    ///     }
    ///     },
    /// </summary>
    /// <param name="json"></param>
    public ManifestEntry(JObject json)
    {
        string url = json["url"].ToStringNotNull();
        int pos = url.IndexOf("updated_date=", StringComparison.Ordinal);
        RelativePath = url.Substring(pos);
        UpdatedDate = DateOnly.ParseExact(RelativePath.Substring(13, 10), "yyyy-MM-dd", null);

        string filename = Path.GetFileNameWithoutExtension(url);
        PartId = int.Parse(filename.Substring(filename.Length - 3));

        var meta = json["meta"] as JObject;
        RecordCount = meta["record_count"].ToIntNotNull();
    }

    public string RelativePath { get; }
    public int RecordCount { get; }
    public DateOnly UpdatedDate { get; }
    public int PartId { get; }


    public override string ToString()
    {
        return $"{RelativePath} ({RecordCount} records) {UpdatedDate.ToString()}[{PartId}]";
    }
}