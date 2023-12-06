using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer;

class ManifestEntry
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
        string url = (string)json["url"];
        int pos = url.IndexOf("updated_date=");
        RelativePath = url.Substring(pos, 23);
        UpdatedDate = DateOnly.ParseExact(RelativePath.Substring(pos + 13, 10), "yyyy-MM-dd", null);

        var meta = json["meta"] as JObject;
        RecordCount = (int)meta["record_count"];
    }

    public string RelativePath { get; }
    public int RecordCount { get; }
    public DateOnly UpdatedDate { get; }


    public override string ToString()
    {
        return $"{RelativePath} ({RecordCount} records) {UpdatedDate.ToString()}";
    }
}