// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Statistics.Models;
using Beyond.SearchEngine.Modules.Update.Models;
using Microsoft.EntityFrameworkCore;

namespace Beyond.SearchEngine.Modules;

public class BeyondContext : DbContext
{
    public BeyondContext(DbContextOptions<BeyondContext> options) : base(options)
    {
    }

    // Update Module
    public DbSet<User> Users { get; set; }

    public DbSet<UpdateHistory> UpdateHistories { get; set; }
    // public DbSet<UpdateConfiguration> UpdateConfigurations { get; set; }

    // Search Module
    // public DbSet<Author> Authors { get; set; }
    // public DbSet<Concept> Concepts { get; set; }
    // public DbSet<Funder> Funders { get; set; }
    // public DbSet<Institution> Institutions { get; set; }
    // public DbSet<Publisher> Publishers { get; set; }
    // public DbSet<Source> Sources { get; set; }
    // public DbSet<Work> Works { get; set; }

    // Statistics Module
    public DbSet<WorkStatistics> WorkStatistics { get; set; }
    public DbSet<UserLikeRecord> UserLikeRecords { get; set; }
}