using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Publisher : OpenAlexStatisticsModel
{
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }


    /***            Basics               ***/

    public string Countries { get; set; }

    [NotMapped]
    public List<string> CountryList => string.IsNullOrEmpty(Countries) ? [] : Countries.Split(';').ToList();

    [Column(TypeName = "varchar(2083)")]
    public string HomepageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ImageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    public string ParentPublisher { get; set; }

    [NotMapped]
    public PublisherData? ParentPublisherData => PublisherData.Build(ParentPublisher);

    public string Roles { get; set; }

    [NotMapped]
    public List<RoleData> RoleList => RoleData.BuildList(Roles);
}