using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Concept : OpenAlexStatisticsModel
{
    /// <summary>
    ///     Wikidata ID without prefix. e.g. Q21198
    ///     of https://www.wikidata.org/wiki/Q21198
    /// </summary>
    [Column(TypeName = "varchar(15)")]
    public string WikiDataId { get; set; }

    [NotMapped]
    public string FullWikiDataId => "https://www.wikidata.org/wiki/" + WikiDataId;

    
    /***             Basics               ***/

    public string Name { get; set; }

    public int Level { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ImageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ThumbnailUrl { get; set; }


    /***              Relation               ***/

    public string RelatedConcepts { get; set; }

    [NotMapped]
    public List<ConceptData> RelatedConceptList => ConceptData.BuildList(RelatedConcepts);
}