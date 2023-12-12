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
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Concept, ConceptDto>().ReverseMap();
        CreateMap<Institution, InstitutionDto>().ReverseMap();
        CreateMap<Publisher, PublisherDto>().ReverseMap();
        CreateMap<Source, SourceDto>().ReverseMap();
        CreateMap<Work, WorkDto>().ReverseMap();
    }
}