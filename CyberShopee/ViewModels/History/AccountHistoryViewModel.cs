// File: ViewModels/AccountHistoryViewModel.cs
namespace CyberShopee.ViewModels
{
    public class OrderHistoryItem
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total => Quantity * UnitCost; // Calculated property
    }

    public class AccountHistoryViewModel
    {
        public IEnumerable<OrderHistoryItem> HistoryItems { get; set; }
    }
}