using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;

namespace Beyond.SearchEngine.Modules;

public class AutoMapperProfile : MapperConfigurationExpression
{
    /// <summary>
    ///     It seems that the auto mapper is not that smart. So we have to
    ///     manually set properties that is to be ignored.
    /// </summary>
    public AutoMapperProfile()
    {
        // Update module.
        CreateMap<InstitutionDto, Institution>()
            .ForMember(inst => inst.AssociatedInstitutionList, act => act.Ignore())
            .ForMember(inst => inst.ConceptList, act => act.Ignore())
            .ReverseMap();

        CreateMap<AuthorDto, Author>()
            .ForMember(author => author.ConceptList, act => act.Ignore())
            .ForMember(author => author.InstitutionData, act => act.Ignore())
            .ReverseMap();

        CreateMap<WorkDto, Work>()
            .ForMember(work => work.ConceptList, act => act.Ignore())
            .ForMember(work => work.KeywordList, act => act.Ignore())
            .ForMember(work => work.RelatedWorkList, act => act.Ignore())
            .ForMember(work => work.ReferencedWorkList, act => act.Ignore())
            .ForMember(work => work.AuthorList, act => act.Ignore())
            .ReverseMap();

        CreateMap<ConceptDto, Concept>()
            .ForMember(concept => concept.RelatedConceptList, act => act.Ignore())
            .ForMember(concept => concept.CountsByYearList, act => act.Ignore())
            .ReverseMap();
    }
}