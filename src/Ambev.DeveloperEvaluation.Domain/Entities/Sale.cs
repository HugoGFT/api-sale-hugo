using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string Date { get; set; }
        public int CustomerId { get; set; }
        public string Branch { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public bool IsCancelled { get; set; }
    }
}
