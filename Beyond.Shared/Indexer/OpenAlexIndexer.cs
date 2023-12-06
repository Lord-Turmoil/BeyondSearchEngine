using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer;

class OpenAlexIndexer
{
    private readonly DateOnly _beginDate;

    private readonly List<ManifestEntry> _manifest;
    private readonly string _tempPath;
    protected string _dataPath;

    private int _nextManifestEntryIndex;

    protected ManifestEntry? currentManifestEntry;

    /// <summary>
    ///     Index all data in a given path.
    /// </summary>
    /// <param name="dataPath">Should be an absolute path.</param>
    /// <param name="tempPath">Temporary path for extracted data.</param>
    public OpenAlexIndexer(string dataPath, string tempPath, DateOnly beginDate)
    {
        if (dataPath.StartsWith("/"))
        {
            throw new IndexException($"Data path must be absolute: {dataPath}");
        }

        _dataPath = dataPath;
        _tempPath = tempPath;
        _beginDate = beginDate;

        _manifest = Manifest.ReadManifest(_dataPath);
    }

    /// <summary>
    ///     Get next manifest entry. null if no more entries.
    /// </summary>
    /// <returns></returns>
    protected ManifestEntry? NextManifestEntry()
    {
        if (_nextManifestEntryIndex >= _manifest.Count)
        {
            return null;
        }

        ManifestEntry entry;
        do
        {
            entry = _manifest[_nextManifestEntryIndex++];
        } while (_nextManifestEntryIndex < _manifest.Count && entry.UpdatedDate < _beginDate);

        return entry.UpdatedDate < _beginDate ? null : entry;
    }


    /// <summary>
    ///     Extract archive to temp path.
    /// </summary>
    /// <param name="archivePath"></param>
    /// <returns>Data file name.</returns>
    protected List<JObject> ExtractData(string archivePath)
    {
        Extractor.Extract(archivePath, _tempPath);
        IEnumerable<string> files = Directory.EnumerateFiles(_tempPath).ToList();
        if (files.Count() != 1)
        {
            throw new IndexException($"Expected 1 file in archive, found {files.Count()}");
        }

        var data = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(files.First()));
        return data ?? throw new IndexException($"Empty data file: {files.First()}");
    }

    protected bool NeedNextManifest()
    {
        return currentManifestEntry == null;
    }

    protected T NotNull<T>(T? value) where T : class
    {
        return value ?? throw new IndexException("Unexpected null value");
    }
}