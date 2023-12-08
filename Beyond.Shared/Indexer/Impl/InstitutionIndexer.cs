using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

public class InstitutionIndexer : OpenAlexIndexer
{
    public InstitutionIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate)
        : base(dataPath, tempPath, beginDate, endDate)
    {
        _logger.Info("Indexing institutions");
    }

    public List<InstitutionDto>? NextDataChunk()
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

        List<InstitutionDto> dtos = new List<InstitutionDto>();
        foreach (JObject obj in data)
        {
            try
            {
                InstitutionDto dto = InstitutionDtoBuilder.Build(obj);
                dtos.Add(dto);
            }
            catch (Exception e)
            {
                _logger.Log($"Failed to build institution: {e.Message}");
            }
        }

        return dtos;
    }
}