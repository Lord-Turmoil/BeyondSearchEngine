using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Institution
{
    /// <summary>
    ///     Institution ID without prefix. e.g. I4210087620
    ///     of https://openalex.org/I4210087620
    /// </summary>
    [Key]
    [Column(TypeName = "char(12)")]
    public string Id { get; set; }

    [Column(TypeName = "varchar(127)")]
    public string Name { get; set; }


    /***             Basics               ***/

    [Column(TypeName = "varchar(15)")]
    public string Type { get; set; }

    [Column(TypeName = "char(8)")]
    public string Country { get; set; }

    [Column(TypeName = "varchar(63)")]
    public string HomepageUrl { get; set; }

    [Column(TypeName = "varchar(127)")]
    public string ImageUrl { get; set; }

    [Column(TypeName = "varchar(127)")]
    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    /// <summary>
    ///     The same as <see cref="Work.Concepts" />.
    /// </summary>
    public string Concepts { get; set; }

    public List<ConceptData> ConceptList => Concepts.Split(';').Select(c => new ConceptData(c)).ToList();


    /// <summary>
    ///     Associated institutions. Each institution is represented as a
    ///     5 comma separated values. All institutions are separated
    ///     by semi-colon.
    ///     e.g. id,name,type,country,relation;...
    ///     relation is the relation of the associated institution to this.
    /// </summary>
    public string AssociatedInstitutions { get; set; }

    public List<AssociatedInstitutionData> AssociatedInstitutionList =>
        AssociatedInstitutions.Split(';').Select(i => new AssociatedInstitutionData(i)).ToList();


    /***              Statistics               ***/

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }


    /***               Other                   ***/

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}