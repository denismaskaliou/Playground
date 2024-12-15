using AutoMapper;
using CosmosDb.Shared.Entities;
using MongoDB.AspNet.Models;

namespace CosmosDb.AspNet.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}