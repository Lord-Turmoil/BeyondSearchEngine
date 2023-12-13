namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticInstitution : ElasticStatisticsModel
{
    public string Name { get; set; }


    /***             Basics               ***/

    public string Type { get; set; }

    public string Country { get; set; }

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    public string Concepts { get; set; }

    public string AssociatedInstitutions { get; set; }
}