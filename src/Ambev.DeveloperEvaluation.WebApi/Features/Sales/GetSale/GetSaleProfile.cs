using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetSale feature
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<int, GetSaleCommand>()
                .ConstructUsing(id => new GetSaleCommand { Id = id });
            CreateMap<GetSaleResult, GetSaleResponse>();
        }
    }
}
