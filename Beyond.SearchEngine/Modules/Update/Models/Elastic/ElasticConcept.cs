namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticConcept : ElasticStatisticsModel
{
    public string WikiDataId { get; set; }

    /***             Basics               ***/

    public string Name { get; set; }

    public int Level { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relation               ***/

    public string RelatedConcepts { get; set; }
}