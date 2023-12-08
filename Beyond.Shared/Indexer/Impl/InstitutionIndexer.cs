using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

public class InstitutionIndexer : GenericIndexer<InstitutionDtoBuilder, InstitutionDto>
{
    public InstitutionIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate)
        : base(dataPath, tempPath, beginDate, endDate)
    {
    }
}
