namespace Beyond.SearchEngine.Modules.Search.Models.Elastic;

public class ElasticSource : ElasticStatisticsModel
{
    public string Name { get; set; }

    public string Type { get; set; }

    public string Country { get; set; }

    public string HomepageUrl { get; set; }


    /***              Relations               ***/

    public string HostId { get; set; }

    public string HostName { get; set; }

    public string Concepts { get; set; }
}