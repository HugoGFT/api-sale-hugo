using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;

/// <summary>
/// Profile for mapping between Application and API ListProduct responses
/// </summary>
public class ListProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProduct feature
    /// </summary>
    public ListProductProfile()
    {
        CreateMap<ListProductRequest, ListProductCommand>();
        CreateMap<ListProductResult, ListProductResponse>();
        CreateMap<GetProductResult, GetProductResponse>();
    }
}
