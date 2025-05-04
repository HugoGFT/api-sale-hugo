namespace Ambev.DeveloperEvaluation.Domain.Dto.SaleDto
{
    public class SaleItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal Total { get; set; }
        public decimal TotalAmount { get; set; }
        public void ApplyDiscount()
        {
            if (Quantity >= 4 && Quantity < 10)
            {
                Discount = UnitPrice * Quantity * 0.10m;
            }
            else if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = UnitPrice * Quantity * 0.20m;
            }
            else if (Quantity > 20)
            {
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");
            }
            else
            {
                Discount = 0;
            }
            Total = (UnitPrice * Quantity);
            TotalAmount = Total - Discount; // Atualiza o backing field
        }
    }
}
