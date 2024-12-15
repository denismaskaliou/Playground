using AutoMapper;
using MongoDB.Shared.Entities;

namespace MongoDB.AspNet.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}