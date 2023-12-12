using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Source : ElasticModel
{
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(15)")]
    public string Type { get; set; }

    [Column(TypeName = "char(8)")]
    public string Country { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string HomepageUrl { get; set; }


    /***              Relations               ***/

    [Column(TypeName = "char(12)")]
    public string HostId { get; set; }

    [Column(TypeName = "varchar(127)")]
    public string HostName { get; set; }

    public string Concepts { get; set; }

    [NotMapped]
    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);


    /***              Statistics               ***/

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }

    public string CountsByYears { get; set; }

    [NotMapped]
    public List<CountsByYearData> CountsByYearList => CountsByYears.Split(';').Select(c => new CountsByYearData(c)).ToList();
}