﻿using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
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

        CreateMap<Concept, ConceptDto>();
        CreateMap<ConceptDto, Concept>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RelatedConceptList, opt => opt.Ignore());
        CreateMap<ConceptDto, ElasticConcept>();
        CreateMap<Concept, DehydratedConceptDto>();

        CreateMap<Funder, FunderDto>();
        CreateMap<FunderDto, Funder>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<FunderDto, ElasticFunder>();

        CreateMap<Institution, InstitutionDto>();
        CreateMap<InstitutionDto, Institution>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore())
            .ForMember(dest => dest.AssociatedInstitutionList, opt => opt.Ignore());
        CreateMap<InstitutionDto, ElasticInstitution>();

        CreateMap<Publisher, PublisherDto>();
        CreateMap<PublisherDto, Publisher>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.CountryList, opt => opt.Ignore())
            .ForMember(dest => dest.ParentPublisherData, opt => opt.Ignore())
            .ForMember(dest => dest.RoleList, opt => opt.Ignore());
        CreateMap<PublisherDto, ElasticPublisher>();

        CreateMap<Source, SourceDto>();
        CreateMap<SourceDto, Source>()
            .ForMember(dest => dest.CountsByYearList, opt => opt.Ignore())
            .ForMember(dest => dest.ConceptList, opt => opt.Ignore());
        CreateMap<SourceDto, ElasticSource>();

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
    }
}