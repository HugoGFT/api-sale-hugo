using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale
{
    public class ListSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for ListSale feature
        /// </summary>
        public ListSaleProfile()
        {
            CreateMap<ListSaleResult, ListSaleResponse>();
            CreateMap<GetSaleResult, GetSaleResponse>();
        }
    }
}
