using Application.ViewModels.Products;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<CreateProductViewModel, Product>().ReverseMap();
        }
    }
}