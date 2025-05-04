using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleProfile : Profile
    {
        public ListSaleProfile()
        {
            CreateMap<ListSaleCommand, ListSaleFilter>();
            CreateMap<Sale, GetSaleResult>();
            CreateMap<ListSaleResult, ListSaleResultDto>().ReverseMap();
            CreateMap<SaleItemDto, SaleItem>().ReverseMap();
        }
    }
}
