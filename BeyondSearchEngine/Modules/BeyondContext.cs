using Microsoft.EntityFrameworkCore;

namespace BeyondSearchEngine.Modules
{
    public class BeyondContext : DbContext
    {
        public BeyondContext(DbContextOptions<BeyondContext> options) : base(options) { }
    }
}
