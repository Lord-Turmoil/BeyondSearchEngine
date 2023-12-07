using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Update.Models;

/// <summary>
///     Generate a token to synchronize update operations.
/// </summary>
public class User
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "char(63)")]
    public string Username { get; set; }

    /// <summary>
    ///     Forget that, we are not going to encrypt the password.
    /// </summary>
    [Column(TypeName = "char(63)")]
    public string Password { get; set; }
}