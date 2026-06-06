using System;
using Module4.Core;
using Xunit;

namespace Module4.Tests
{
    public class DispensingLogTests
    {
        // 1) المسار الطبيعي
        [Fact]
        public void CalculateAndSendCost_Should_Calculate_Correct_Cost_And_Set_IsSentToFinance()
        {
            var med = new Medication(
                id: 1,
                name: "Paracetamol",
                unitPrice: 10m,
                stockQuantity: 100,
                expiryDate: DateTime.Now.AddDays(30));

            var log = new DispensingLog(
                id: 1,
                patientId: 123,
                medication: med,
                dispensedQuantity: 3);

            log.CalculateAndSendCost();

            Assert.Equal(30m, log.CostDecimal);
            Assert.True(log.IsSentToFinance);
            Assert.Equal(97, med.StockQuantity);
        }

        // 2) كمية غير صالحة (≤ 0)
        [Fact]
        public void CalculateAndSendCost_Should_ThrowException_When_Quantity_Is_Invalid()
        {
            var med = new Medication(
                id: 1,
                name: "Paracetamol",
                unitPrice: 10m,
                stockQuantity: 50,
                expiryDate: DateTime.Now.AddDays(30));

            Assert.Throws<ArgumentException>(() =>
            {
                var log = new DispensingLog(
                    id: 1,
                    patientId: 123,
                    medication: med,
                    dispensedQuantity: 0);
            });
        }

        // 3) دواء منتهي الصلاحية
        [Fact]
        public void CalculateAndSendCost_Should_ThrowException_When_Medication_Is_Expired()
        {
            var med = new Medication(
                id: 1,
                name: "ExpiredMed",
                unitPrice: 20m,
                stockQuantity: 50,
                expiryDate: DateTime.Now.AddDays(-1));

            var log = new DispensingLog(
                id: 1,
                patientId: 123,
                medication: med,
                dispensedQuantity: 2);

            Assert.Throws<InvalidOperationException>(() => log.CalculateAndSendCost());
        }

        // 4) مخزون غير كافٍ
        [Fact]
        public void CalculateAndSendCost_Should_ThrowException_When_Stock_Is_Insufficient()
        {
            var med = new Medication(
                id: 1,
                name: "Ibuprofen",
                unitPrice: 15m,
                stockQuantity: 1,
                expiryDate: DateTime.Now.AddDays(30));

            var log = new DispensingLog(
                id: 1,
                patientId: 123,
                medication: med,
                dispensedQuantity: 5);

            Assert.Throws<InvalidOperationException>(() => log.CalculateAndSendCost());
        }

        // 5) أقل كمية صحيحة (مسار إضافي)
        [Fact]
        public void CalculateAndSendCost_Should_Work_With_Minimum_Valid_Quantity()
        {
            var med = new Medication(
                id: 1,
                name: "VitaminC",
                unitPrice: 8m,
                stockQuantity: 10,
                expiryDate: DateTime.Now.AddDays(10));

            var log = new DispensingLog(
                id: 2,
                patientId: 456,
                medication: med,
                dispensedQuantity: 1);

            log.CalculateAndSendCost();

            Assert.Equal(8m, log.CostDecimal);
            Assert.True(log.IsSentToFinance);
            Assert.Equal(9, med.StockQuantity);
        }
    }
}