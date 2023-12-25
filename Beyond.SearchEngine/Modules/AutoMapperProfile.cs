// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Modules.Deprecated.Dtos;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Statistics.Dtos;
using Beyond.SearchEngine.Modules.Statistics.Models;
using Beyond.SearchEngine.Modules.Update.Models.Elastic;
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
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.InstitutionData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());
        CreateMap<AuthorDto, ElasticAuthor>();
        CreateMap<Author, DehydratedStatisticsModelDto>();

        CreateMap<Concept, ConceptDto>();
        CreateMap<ConceptDto, Concept>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedConceptList, opt => opt.Ignore());
        CreateMap<ConceptDto, ElasticConcept>();
        CreateMap<Concept, DehydratedStatisticsModelDto>();

        CreateMap<Funder, FunderDto>();
        CreateMap<FunderDto, Funder>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<FunderDto, ElasticFunder>();
        CreateMap<Funder, DehydratedStatisticsModelDto>();

        CreateMap<Institution, InstitutionDto>();
        CreateMap<InstitutionDto, Institution>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.AssociatedInstitutionList, opt => opt.Ignore());
        CreateMap<InstitutionDto, ElasticInstitution>();
        CreateMap<Institution, DehydratedStatisticsModelDto>();

        CreateMap<Publisher, PublisherDto>();
        CreateMap<PublisherDto, Publisher>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.CountryList, opt => opt.Ignore())
            .ForMember(dest => dest.ParentPublisherData, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<PublisherDto, ElasticPublisher>();
        CreateMap<Publisher, DehydratedStatisticsModelDto>();

        CreateMap<Source, SourceDto>();
        CreateMap<SourceDto, Source>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());
        CreateMap<SourceDto, ElasticSource>();
        CreateMap<Source, DehydratedStatisticsModelDto>();

        CreateMap<Work, WorkDto>();
        CreateMap<WorkDto, Work>()
            .ForMember(dest => dest.SourceData, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.KeywordList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.ReferencedWorkList, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorList, opt => opt.Ignore())
            .ForMember(dest => dest.FunderList, opt => opt.Ignore());
        CreateMap<WorkDto, ElasticWork>();
        CreateMap<Work, DehydratedWorkDto>();
        CreateMap<Work, BriefWorkDto>();
        CreateMap<Work, WorkSearchDto>()
            .ForMember(dest => dest.Source, opt => opt.Ignore());


        CreateMap<WorkStatistics, WorkStatisticsDto>().ReverseMap();


        // Deprecated
        CreateMap<Author, DeprecatedAuthorDto>()
            .ForMember(dest => dest.Institution, opt => opt.Ignore());
        CreateMap<Work, DeprecatedBriefWorkDto>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore());
        CreateMap<Work, DeprecatedWorkDto>();
    }
}