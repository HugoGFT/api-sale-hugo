using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Dto.ProductDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for ListProduct operation
        /// </summary>
        public ListProductProfile()
        {
            CreateMap<ListProductCommand, ListProductFilter>();
            CreateMap<ListProductResultDto, ListProductResult>();
            CreateMap<Product, GetProductResult>();
        }
    }
}
