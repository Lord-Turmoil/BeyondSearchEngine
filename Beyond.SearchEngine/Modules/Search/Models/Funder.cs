using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Funder : OpenAlexStatisticsModel
{
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }


    /***             Basics               ***/

    [Column(TypeName = "char(3)")]
    public string Country { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string HomepageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ImageUrl { get; set; }

    [Column(TypeName = "varchar(2083)")]
    public string ThumbnailUrl { get; set; }


    /***             Relations                ***/

    public string Roles { get; set; }

    [NotMapped]
    public List<RoleData> RoleList => RoleData.BuildList(Roles);


    /***             Statistics                ***/

    public int GrantsCount { get; set; }
}