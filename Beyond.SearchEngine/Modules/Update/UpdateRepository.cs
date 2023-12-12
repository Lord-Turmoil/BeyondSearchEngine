using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Update.Models;

namespace Beyond.SearchEngine.Modules.Update;

public class UserRepository : Repository<User>
{
    public UserRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class UpdateHistoryRepository : Repository<UpdateHistory>
{
    public UpdateHistoryRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class UpdateConfigurationRepository : Repository<UpdateConfiguration>
{
    public UpdateConfigurationRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}