using Xunit;
using PharmacyAssignment.Core;

namespace PharmacyAssignment.Tests
{
    public class InventoryManagerTests
    {
        // 1. المخزون كافٍ
        [Fact]
        public void Test_StockSufficient()
        {
            var manager = new InventoryManager();
            var med = new Medication { StockQuantity = 100, MinStockAlert = 10 };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Success", result.Status);
        }
        // 2. المخزون منخفض ويوجد طلب معلق
        [Fact]
        public void Test_LowStock_WithPendingRequest()
        {
            var manager = new InventoryManager();
            var med = new Medication { StockQuantity = 5, MinStockAlert = 10 };
            var result = manager.EvaluateRestockNeed(med, true);
            Assert.Equal("Alert Only", result.Status);
        }
        // 3. مخزون صفر، دواء مراقب، كمية مطلوبة أكبر من 50 (يجب أن يحد الكمية بـ 50)
        [Fact]
        public void Test_ZeroStock_Controlled_NeedsMoreThan50()
        {
            var manager = new InventoryManager();
            var med = new Medication { Id="M1", StockQuantity = 0, MinStockAlert = 10, MaxCapacity = 200, IsControlledSubstance = true };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Restock Required - Special Approval", result.Status);
            Assert.Equal(50, result.RequestedQuantity);
        }
        // 4. مخزون منخفض، دواء مراقب، كمية مطلوبة أقل من 50
        [Fact]
        public void Test_LowStock_Controlled_NeedsLessThan50()
        {
            var manager = new InventoryManager();
            var med = new Medication { Id="M2", StockQuantity = 10, MinStockAlert = 10, MaxCapacity = 50, IsControlledSubstance = true };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Restock Required - Special Approval", result.Status);
            Assert.Equal(40, result.RequestedQuantity); // 50 - 10 = 40
        }
        // 5. مخزون منخفض، دواء عادي (موافقة تلقائية)
        [Fact]
        public void Test_LowStock_NormalMedication()
        {
            var manager = new InventoryManager();
            var med = new Medication { Id="M3", StockQuantity = 5, MinStockAlert = 10, MaxCapacity = 100, IsControlledSubstance = false };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Restock Required - Auto Approved", result.Status);
            Assert.Equal(95, result.RequestedQuantity); // 100 - 5 = 95
        }
        // 6. مخزون صفر، دواء عادي
        [Fact]
        public void Test_ZeroStock_NormalMedication()
        {
            var manager = new InventoryManager();
            var med = new Medication { Id="M4", StockQuantity = 0, MinStockAlert = 10, MaxCapacity = 150, IsControlledSubstance = false };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Restock Required - Auto Approved", result.Status);
            Assert.Equal(150, result.RequestedQuantity);
        }
        // 7. مخزون صفر، دواء مراقب، والمطلوب بالضبط 50
        [Fact]
        public void Test_ZeroStock_Controlled_NeedsExactly50()
        {
            var manager = new InventoryManager();
            var med = new Medication { Id="M5", StockQuantity = 0, MinStockAlert = 10, MaxCapacity = 50, IsControlledSubstance = true };
            var result = manager.EvaluateRestockNeed(med, false);
            Assert.Equal("Restock Required - Special Approval", result.Status);
            Assert.Equal(50, result.RequestedQuantity);
        }
    }
}