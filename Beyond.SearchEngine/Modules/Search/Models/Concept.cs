using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Concept : ElasticModel
{
    /// <summary>
    /// Wikidata ID without prefix. e.g. Q21198
    /// of https://www.wikidata.org/wiki/Q21198
    /// </summary>
    [Column(TypeName = "varchar(8)")]
    public string WikiDataId { get; set; }

    [NotMapped]
    public string FullWikiDataId => "https://www.wikidata.org/wiki/" + WikiDataId;

    /***             Basics               ***/

    [Column(TypeName = "varchar(63)")]
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
    public List<ConceptData> RelatedConceptList => RelatedConcepts.Split(';').Select(c => new ConceptData(c)).ToList();


    /***              Statistics               ***/

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }

    public string CountsByYears { get; set; }

    [NotMapped]
    public List<CountsByYearData> CountsByYearList => CountsByYears.Split(';').Select(c => new CountsByYearData(c)).ToList();
}