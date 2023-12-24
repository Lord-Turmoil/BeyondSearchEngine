using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Statistics.Models;

[PrimaryKey(nameof(UserId), nameof(WorkId))]
public class UserLikeRecord
{
    public int UserId { get; set; }

    [Column(TypeName = "varchar(12)")]
    public string WorkId { get; set; }

    public DateTime Created { get; set; }
}