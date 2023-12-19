using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Funder : OpenAlexStatisticsModel
{
    /***             Basics               ***/

    public string Country { get; set; }

    public string Description { get; set; }

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***             Relations                ***/

    public string Roles { get; set; }

    public List<RoleData> RoleList => RoleData.BuildList(Roles);


    /***             Statistics                ***/

    public int GrantsCount { get; set; }
}