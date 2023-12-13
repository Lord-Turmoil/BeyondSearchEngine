using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer;

public class OpenAlexIndexer
{
    protected readonly string _dataPath;

    private readonly List<ManifestEntry> _manifest;
    private readonly string _tempPath;

    protected ManifestEntry? _currentManifestEntry;

    protected IndexLogger _logger;

    private int _nextManifestEntryIndex;

    /// <summary>
    ///     Index all data in a given path.
    /// </summary>
    /// <param name="dataPath">Should be an absolute path.</param>
    /// <param name="tempPath">Temporary path for extracted data.</param>
    protected OpenAlexIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate)
    {
        if (!Path.IsPathRooted(dataPath))
        {
            throw new IndexException($"Data path must be absolute: {dataPath}");
        }

        _dataPath = dataPath;
        _tempPath = tempPath;
        BeginDate = beginDate;
        EndDate = endDate;

        _logger = new IndexLogger("index.log");

        try
        {
            _manifest = Manifest.ReadManifest(_dataPath);
        }
        catch (Exception e)
        {
            _logger.Log($"Failed to read manifest: {e.Message}");
            throw new IndexException($"Failed to read manifest: {e.Message}", e);
        }

        _logger.Info($"Processing data from {BeginDate} to {EndDate}");
        _logger.LogSub($"Found {_manifest.Count} entries in manifest");
    }

    public DateOnly BeginDate { get; }
    public DateOnly EndDate { get; }

    public ManifestEntry? CurrentManifestEntry()
    {
        if (_currentManifestEntry == null)
        {
            return NextManifestEntry();
        }

        return _currentManifestEntry;
    }

    /// <summary>
    ///     Get next manifest entry. null if no more entries.
    /// </summary>
    /// <returns></returns>
    public ManifestEntry? NextManifestEntry()
    {
        if (_nextManifestEntryIndex >= _manifest.Count)
        {
            _currentManifestEntry = null;
        }
        else
        {
            ManifestEntry entry;
            do
            {
                entry = _manifest[_nextManifestEntryIndex++];
            } while (_nextManifestEntryIndex < _manifest.Count && !IsInDateRange(entry.UpdatedDate));

            _currentManifestEntry = IsInDateRange(entry.UpdatedDate) ? entry : null;
        }

        return _currentManifestEntry;
    }

    private bool IsInDateRange(DateOnly date)
    {
        return date >= BeginDate && date <= EndDate;
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

        string dataFilename = files.First();
        return File.ReadAllLines(dataFilename).Select(JObject.Parse).ToList();
    }

    protected bool NeedNextManifest()
    {
        return _currentManifestEntry == null;
    }
}