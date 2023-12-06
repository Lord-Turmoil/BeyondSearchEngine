using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

class InstitutionIndexer : OpenAlexIndexer
{
    public InstitutionIndexer(string dataPath, string tempPath, DateOnly beginDate)
        : base(dataPath, tempPath, beginDate)
    {
    }

    public List<InstitutionDto>? NextDataChunk()
    {
        if (NeedNextManifest())
        {
            currentManifestEntry = NextManifestEntry();
            if (currentManifestEntry == null)
            {
                return null;
            }
        }

        string archivePath = Path.Join(_dataPath, currentManifestEntry!.RelativePath);
        List<JObject> data = ExtractData(archivePath);

        return data.Select(InstitutionDtoBuilder.Build).ToList();
    }
}