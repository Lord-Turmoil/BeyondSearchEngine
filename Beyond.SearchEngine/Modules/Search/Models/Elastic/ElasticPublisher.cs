namespace Beyond.SearchEngine.Modules.Search.Models.Elastic;

public class ElasticPublisher : ElasticStatisticsModel
{
    public string Name { get; set; }


    /***            Basics               ***/

    public string Countries { get; set; }

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    public string ParentPublisher { get; set; }

    public string Roles { get; set; }
}