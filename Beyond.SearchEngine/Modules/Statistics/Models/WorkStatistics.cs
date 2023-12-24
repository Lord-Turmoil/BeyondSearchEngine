// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Statistics.Models;

public class WorkStatistics
{
    [Key]
    [Column(TypeName = "varchar(12)")]
    public string Id { get; set; }

    /// <summary>
    ///     How many reader like the work.
    /// </summary>
    public int Likes { get; set; }

    /// <summary>
    ///     How many people view the work.
    /// </summary>
    public int Views { get; set; }
}