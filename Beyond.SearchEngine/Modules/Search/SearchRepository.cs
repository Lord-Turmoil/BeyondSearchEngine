using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Search.Models;

namespace Beyond.SearchEngine.Modules.Search;

public class AuthorRepository : Repository<Author>
{
    public AuthorRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class ConceptRepository : Repository<Concept>
{
    public ConceptRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class InstitutionRepository : Repository<Institution>
{
    public InstitutionRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class PublisherRepository : Repository<Publisher>
{
    public PublisherRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class SourceRepository : Repository<Source>
{
    public SourceRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class WorkRepository : Repository<Work>
{
    public WorkRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}