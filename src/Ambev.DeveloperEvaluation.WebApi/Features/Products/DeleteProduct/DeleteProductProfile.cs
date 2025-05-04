using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteProduct;

/// <summary>
/// Profile for mapping DeleteUser feature requests to commands
/// </summary>
public class DeleteProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteUser feature
    /// </summary>
    public DeleteProductProfile()
    {
        CreateMap<int, DeleteProductCommand>()
            .ConstructUsing(id => new DeleteProductCommand(id));
    }
}
