using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

public class GenericIndexer<TBuilder, TDto> : OpenAlexIndexer where TBuilder : IDtoBuilder<TDto>, new()
{
    protected GenericIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath,
        tempPath, beginDate, endDate)
    {
        _logger.Info($"Indexing {typeof(TDto).Name}s");
    }

    public List<TDto>? NextDataChunk()
    {
        if (NeedNextManifest())
        {
            if (NextManifestEntry() == null)
            {
                _logger.Info("No more manifest entries");
                return null;
            }
        }

        string archivePath = Path.Join(_dataPath, _currentManifestEntry!.RelativePath);
        List<JObject> data = ExtractData(archivePath);
        _currentManifestEntry = null;

        var dtos = new List<TDto>();
        var builder = new TBuilder();
        foreach (JObject json in data)
        {
            try
            {
                TDto? dto = builder.Build(json);
                if (dto != null)
                {
                    dtos.Add(dto);
                }
            }
            catch (Exception e)
            {
                _logger.Log($"Failed to build {typeof(TDto).Name}: {e.Message}");
            }
        }

        return dtos;
    }
}