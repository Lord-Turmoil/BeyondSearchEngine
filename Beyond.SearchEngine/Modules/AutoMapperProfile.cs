using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;

namespace Beyond.SearchEngine.Modules;

public class AutoMapperProfile : MapperConfigurationExpression
{
    public AutoMapperProfile()
    {
        // Update module.
        CreateMap<Institution, InstitutionDto>().ReverseMap();
    }
}