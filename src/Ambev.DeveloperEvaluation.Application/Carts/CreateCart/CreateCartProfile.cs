using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartCommand, Cart>();
            CreateMap<Cart, CreateCartResult>();
            CreateMap<ProductCartDto, ProductCart>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap()
                .ForPath(src => src.Quantity, opt => opt.MapFrom(dest => dest.Count))
                .ForPath(src => src.ProductId, opt => opt.MapFrom(dest => dest.IdProduct));
        }
    }
}
