using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<UpdateCartCommand, Cart>();
            CreateMap<Cart, UpdateCartResult>();
            CreateMap<ProductCartDto, ProductCart>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IdProduct, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap()
                .ForPath(src => src.Quantity, opt => opt.MapFrom(dest => dest.Count))
                .ForPath(src => src.ProductId, opt => opt.MapFrom(dest => dest.IdProduct));
        }
    }
}
