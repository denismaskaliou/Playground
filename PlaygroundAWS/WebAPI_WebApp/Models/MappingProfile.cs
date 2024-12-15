using AutoMapper;
using Database_MongoDB.Models;
using Domain.Entities;

namespace WebAPI_WebApp.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}