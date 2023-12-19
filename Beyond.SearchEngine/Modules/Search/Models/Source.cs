using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Source : OpenAlexStatisticsModel
{
    public string Type { get; set; }

    public string Country { get; set; }

    public string HomepageUrl { get; set; }


    /***              Relations               ***/

    public string HostId { get; set; }

    public string HostName { get; set; }

    public string Concepts { get; set; }

    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);
}