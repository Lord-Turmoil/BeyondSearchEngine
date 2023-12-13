using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Search.Models;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Search;

public class SearchModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddCustomRepository<Author, AuthorRepository>()
            .AddCustomRepository<Concept, ConceptRepository>()
            .AddCustomRepository<Funder, FunderRepository>()
            .AddCustomRepository<Institution, InstitutionRepository>()
            .AddCustomRepository<Publisher, PublisherRepository>()
            .AddCustomRepository<Source, SourceRepository>()
            .AddCustomRepository<Work, WorkRepository>();

        return services;
    }
}