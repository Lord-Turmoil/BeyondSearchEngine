using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Author : ElasticModel
{
    /// <summary>
    ///     ORC ID without prefix. e.g. 0000-0002-4465-7034
    ///     of https://orcid.org/0000-0002-4465-7034
    /// </summary>
    [Column(TypeName = "char(20)")]
    public string OrcId { get; set; }

    [NotMapped]
    public string FullOrcId => "https://orcid.org/" + OrcId;

    /// <summary>
    ///     Only store one display name.
    /// </summary>
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }


    /***              Statistics               ***/

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }

    public string CountsByYears { get; set; }

    [NotMapped]
    public List<CountsByYearData> CountsByYearList => CountsByYearData.BuildList(CountsByYears);


    /***                Relation               ***/

    /// <summary>
    ///     Institution is stored as a quadruple of comma separated values.
    ///     id,name,type,country
    /// </summary>
    public string Institution { get; set; }

    [NotMapped]
    public InstitutionData? InstitutionData => InstitutionData.Build(Institution);

    /// <summary>
    ///     The same as <see cref="Work.Concepts" />.
    /// </summary>
    public string Concepts { get; set; }

    [NotMapped]
    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);
}