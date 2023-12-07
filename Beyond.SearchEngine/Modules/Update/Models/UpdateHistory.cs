using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Update.Models;

public class UpdateHistory
{
    public DateTime? Completed;

    public DateTime Created;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id;

    [Column(TypeName = "varchar(15)")]
    public string Type;

    public DateOnly UpdatedTime;

    public bool IsCompleted => Completed != null;
}