using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    public class DeleteSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for DeleteUser feature
        /// </summary>
        public DeleteSaleProfile()
        {
            CreateMap<int, DeleteSaleCommand>()
                .ConstructUsing(id => new DeleteSaleCommand(id));
        }
    }
}
