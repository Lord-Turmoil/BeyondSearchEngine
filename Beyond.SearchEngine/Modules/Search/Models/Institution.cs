using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Institution : OpenAlexStatisticsModel
{
    /***             Basics               ***/

    public string Type { get; set; }

    public string Country { get; set; }

    /*
     * These urls might be long.
     * Reference: https://stackoverflow.com/questions/20958/list-of-standard-lengths-for-database-fields
     */

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    /// <summary>
    ///     The same as <see cref="Work.Concepts" />.
    /// </summary>
    public string Concepts { get; set; }

    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);


    /// <summary>
    ///     Associated institutions. Each institution is represented as a
    ///     5 comma separated values. All institutions are separated
    ///     by semi-colon.
    ///     e.g. id,name,type,country,relation;...
    ///     relation is the relation of the associated institution to this.
    /// </summary>
    public string AssociatedInstitutions { get; set; }

    public List<AssociatedInstitutionData> AssociatedInstitutionList =>
        AssociatedInstitutionData.BuildList(AssociatedInstitutions);
}