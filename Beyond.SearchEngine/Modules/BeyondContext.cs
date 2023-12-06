using Microsoft.EntityFrameworkCore;

namespace Beyond.SearchEngine.Modules;

public class BeyondContext : DbContext
{
    public BeyondContext(DbContextOptions<BeyondContext> options) : base(options)
    {
    }
}