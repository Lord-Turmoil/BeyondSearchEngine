﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Update.Models;

public class UpdateHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "varchar(15)")]
    public string Type { get; set; }

    [Column(TypeName = "char(12)")]
    public string UpdatedTime { get; set; }

    public int RecordCount { get; set; } = 0;

    public DateTime? Completed { get; set; }

    public DateTime Created { get; set; }

    public bool IsCompleted => Completed != null;
}