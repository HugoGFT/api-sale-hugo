using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCart;

/// <summary>
/// Profile for mapping between Application and API ListCart responses
/// </summary>
public class ListCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListCart feature
    /// </summary>
    public ListCartProfile()
    {
        CreateMap<ListCartRequest, ListCartCommand>();
        CreateMap<ListCartResult, ListCartResponse>();
    }
}
