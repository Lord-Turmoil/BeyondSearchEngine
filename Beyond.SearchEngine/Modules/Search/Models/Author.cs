using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Author : OpenAlexStatisticsModel
{
    /// <summary>
    ///     ORC ID without prefix. e.g. 0000-0002-4465-7034
    ///     of https://orcid.org/0000-0002-4465-7034
    /// </summary>
    public string OrcId { get; set; }

    public string FullOrcId => "https://orcid.org/" + OrcId;

    /***                Relation               ***/

    /// <summary>
    ///     Institution is stored as a quadruple of comma separated values.
    ///     id,name,type,country
    /// </summary>
    public string Institution { get; set; }

    public InstitutionData? InstitutionData => InstitutionData.Build(Institution);

    /// <summary>
    ///     The same as <see cref="Work.Concepts" />.
    /// </summary>
    public string Concepts { get; set; }

    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);
}