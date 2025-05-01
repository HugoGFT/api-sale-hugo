using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int UserID { get; set; }
        public string Date { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal TotalDescount { get; set; }
        public decimal TotalPriceWithDescount { get; set; }
    }
}
