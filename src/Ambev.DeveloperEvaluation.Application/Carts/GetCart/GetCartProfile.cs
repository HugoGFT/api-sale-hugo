using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartProfile : Profile
    {
        public GetCartProfile()
        {
            CreateMap<GetCartCommand, Cart>();
            CreateMap<Cart, GetCartResult>();
            CreateMap<ProductCartDto, ProductCart>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap()
                .ForPath(src => src.Quantity, opt => opt.MapFrom(dest => dest.Count))
                .ForPath(src => src.ProductId, opt => opt.MapFrom(dest => dest.IdProduct));
        }
    }
}
