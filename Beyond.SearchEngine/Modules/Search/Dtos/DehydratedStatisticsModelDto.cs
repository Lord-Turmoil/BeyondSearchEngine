using Beyond.SearchEngine.Modules.Search.Models;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Used for search preview. Can be used for all models that inherit
///     from OpenAlexStatisticsModel. <see cref="OpenAlexStatisticsModel" />
/// </summary>
public class DehydratedStatisticsModelDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }
}