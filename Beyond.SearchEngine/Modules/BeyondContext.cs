﻿using Beyond.SearchEngine.Modules.Search.Models;
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

    // Search Module
    public DbSet<Author> Authors { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Work> Works { get; set; }
}