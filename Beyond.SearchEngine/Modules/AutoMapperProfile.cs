using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;

namespace Beyond.SearchEngine.Modules;

public class AutoMapperProfile : MapperConfigurationExpression
{
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
    }
}