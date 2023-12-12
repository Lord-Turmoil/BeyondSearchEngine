using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Institution : OpenAlexStatisticsModel
{
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }


    /***             Basics               ***/

    [Column(TypeName = "varchar(15)")]
    public string Type { get; set; }

    [Column(TypeName = "char(8)")]
    public string Country { get; set; }

    /*
     * These urls might be long.
     * Reference: https://stackoverflow.com/questions/20958/list-of-standard-lengths-for-database-fields
     */

    [Column(TypeName = "varchar(2083)")]
    public string HomepageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ImageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    /// <summary>
    ///     The same as <see cref="Work.Concepts" />.
    /// </summary>
    public string Concepts { get; set; }

    [NotMapped]
    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);


    /// <summary>
    ///     Associated institutions. Each institution is represented as a
    ///     5 comma separated values. All institutions are separated
    ///     by semi-colon.
    ///     e.g. id,name,type,country,relation;...
    ///     relation is the relation of the associated institution to this.
    /// </summary>
    public string AssociatedInstitutions { get; set; }

    [NotMapped]
    public List<AssociatedInstitutionData> AssociatedInstitutionList =>
        AssociatedInstitutionData.BuildList(AssociatedInstitutions);
}