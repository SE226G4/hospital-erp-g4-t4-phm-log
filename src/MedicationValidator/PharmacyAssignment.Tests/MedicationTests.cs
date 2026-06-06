using Module4.Core;
using Xunit;

namespace Module4.Tests
{
    public class MedicationTests
    {
        [Fact]
        public void CheckLowStock_Should_Return_Critical_When_Stock_Is_Zero()
        {
            var med = new Medication(1, "TestMed", 10m, 0, 5, true);
            string result = med.CheckLowStock();
            Assert.Contains("CRITICAL", result);
        }

        [Fact]
        public void CheckLowStock_Should_Return_Urgent_When_Stock_Is_Low_And_Critical()
        {
            var med = new Medication(2, "CriticalMed", 10m, 3, 5, true);
            string result = med.CheckLowStock();
            Assert.Contains("URGENT", result);
        }

        [Fact]
        public void CheckLowStock_Should_Return_Warning_When_Stock_Is_Low_And_Not_Critical()
        {
            var med = new Medication(3, "NormalMed", 10m, 3, 5, false);
            string result = med.CheckLowStock();
            Assert.Contains("WARNING", result);
        }

        [Fact]
        public void CheckLowStock_Should_Return_Notice_When_Stock_Is_Moderate()
        {
            var med = new Medication(4, "ModerateMed", 10m, 9, 5, false);
            string result = med.CheckLowStock();
            Assert.Contains("NOTICE", result);
        }

        [Fact]
        public void CheckLowStock_Should_Return_OK_When_Stock_Is_Sufficient()
        {
            var med = new Medication(5, "GoodMed", 10m, 20, 5, false);
            string result = med.CheckLowStock();
            Assert.Contains("OK", result);
        }
    }
}