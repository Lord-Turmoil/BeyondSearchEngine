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
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.InstitutionData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());
        CreateMap<AuthorDto, Author>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.InstitutionData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());

        CreateMap<Concept, ConceptDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedConceptList, opt => opt.Ignore());
        CreateMap<ConceptDto, Concept>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedConceptList, opt => opt.Ignore());

        CreateMap<Funder, FunderDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<FunderDto, Funder>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());

        CreateMap<Institution, InstitutionDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.AssociatedInstitutionList, opt => opt.Ignore());
        CreateMap<InstitutionDto, Institution>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.AssociatedInstitutionList, opt => opt.Ignore());

        CreateMap<Publisher, PublisherDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.CountryList, opt => opt.Ignore())
            .ForMember(dest => dest.ParentPublisherData, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<PublisherDto, Publisher>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.CountryList, opt => opt.Ignore())
            .ForMember(dest => dest.ParentPublisherData, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());

        CreateMap<Source, SourceDto>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());
        CreateMap<SourceDto, Source>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());

        CreateMap<Work, WorkDto>()
            .ForMember(dest => dest.SourceData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.KeywordList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.ReferencedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorList, opt => opt.Ignore())
            .ForMember(dest => dest.FunderList, opt => opt.Ignore());
        CreateMap<WorkDto, Work>()
            .ForMember(dest => dest.SourceData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.KeywordList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.ReferencedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorList, opt => opt.Ignore())
            .ForMember(dest => dest.FunderList, opt => opt.Ignore());
    }
}