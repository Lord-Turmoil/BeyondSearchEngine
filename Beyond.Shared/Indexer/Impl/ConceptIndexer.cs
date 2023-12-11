using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class ConceptIndexer : GenericIndexer<ConceptDtoBuilder, ConceptDto>
{
    public ConceptIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath, tempPath, beginDate, endDate)
    {
    }
}