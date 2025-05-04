namespace Ambev.DeveloperEvaluation.Domain.Dto.SaleDto
{
    public class ListSaleResultDto
    {
        public ListSaleResultDto(int totalItems, int totalPages, int currentPage, IEnumerable<Entities.Sale> sales)
        {
            TotalItems = totalItems;
            Data = sales;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public IEnumerable<Entities.Sale> Data { get; set; } = new List<Entities.Sale>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
