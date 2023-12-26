// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Beyond.SearchEngine.Modules.Statistics.Models;

[PrimaryKey(nameof(UserId), nameof(WorkId))]
public class UserLikeRecord
{
    public int UserId { get; set; }

    [Column(TypeName = "varchar(12)")]
    public string WorkId { get; set; }

    public DateTime Created { get; set; }
}