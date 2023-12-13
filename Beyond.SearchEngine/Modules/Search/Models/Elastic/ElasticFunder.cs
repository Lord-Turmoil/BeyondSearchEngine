namespace Beyond.SearchEngine.Modules.Search.Models.Elastic;

public class ElasticFunder : ElasticStatisticsModel
{
    public string Name { get; set; }


    /***             Basics               ***/

    public string Country { get; set; }

    public string Description { get; set; }

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***             Relations                ***/

    public string Roles { get; set; }


    /***             Statistics                ***/

    public int GrantsCount { get; set; }
}