using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

public class AuthorIndexer : OpenAlexIndexer
{
    public AuthorIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate)
        : base(dataPath, tempPath, beginDate, endDate)
    {
    }

    public List<AuthorDto>? NextDataChunk()
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

        var dtos = new List<AuthorDto>();
        foreach (JObject obj in data)
        {
            try
            {
                AuthorDto dto = AuthorDtoBuilder.Build(obj);
                dtos.Add(dto);
            }
            catch (Exception e)
            {
                _logger.Log($"Failed to build author: {e.Message}");
            }
        }

        return dtos;
    }
}