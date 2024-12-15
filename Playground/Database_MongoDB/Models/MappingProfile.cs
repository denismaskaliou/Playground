using AutoMapper;
using Domain.Entities;

namespace Database_MongoDB.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, Product>().ReverseMap();
    }
}