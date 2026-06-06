using System;

namespace Module4.Core
{
    public class Medication
    {
        public int Id { get; }
        public string Name { get; }
        public decimal UnitPrice { get; }
        public int StockQuantity { get; private set; }
        public int MinStockAlert { get; }
        public bool IsCritical { get; }
        public DateTime ExpiryDate { get; }

        public Medication(int id, string name, decimal unitPrice, int stockQuantity, int minStockAlert, bool isCritical)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
            StockQuantity = stockQuantity;
            MinStockAlert = minStockAlert;
            IsCritical = isCritical;
            ExpiryDate = DateTime.Now.AddMonths(6);
        }

        public bool IsExpired()
        {
            return DateTime.Now > ExpiryDate;
        }

        public bool IsStockSufficient(int requestedQty)
        {
            return StockQuantity >= requestedQty;
        }

        public void DeductStock(int qty)
        {
            StockQuantity -= qty;
        }

        //  الدالة المطلوبة AFTRER الـ Refactor (CC = 5)
        private string GetLowStockMessage(string level)
        {
            return level switch
            {
                "CRITICAL" => $"CRITICAL: {Name} is out of stock!",
                "URGENT" => $"URGENT: {Name} stock is low ({StockQuantity}) and this is a critical medication!",
                "WARNING" => $"WARNING: {Name} stock is low ({StockQuantity}). Please reorder soon.",
                "NOTICE" => $"NOTICE: {Name} stock is moderate ({StockQuantity}). Monitor levels.",
                _ => $"OK: {Name} stock is sufficient ({StockQuantity})."
            };
        }
        public string CheckLowStock()
        {
            if (StockQuantity <= 0)
                return GetLowStockMessage("CRITICAL");

            if (StockQuantity <= MinStockAlert)
                return GetLowStockMessage(IsCritical ? "URGENT" : "WARNING");

            if (StockQuantity <= MinStockAlert * 2)
                return GetLowStockMessage("NOTICE");

            return GetLowStockMessage("OK");
        }
    }
}