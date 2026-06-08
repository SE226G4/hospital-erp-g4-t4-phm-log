using Xunit;
using PharmacyAssignment.Core;

namespace PharmacyAssignment.Tests
{
    public class PatientBillingItemTests
    {
        [Fact]
        public void ProcessInsurance_InactiveInsurance_ReturnsFalseAndFullAmount()
        {
            // Arrange
            var item = new PatientBillingItem { BaseAmount = 100m };

            // Act
            bool result = item.ProcessInsuranceCoverage(false, "Premium", true, 50m);

            // Assert
            Assert.False(result);
            Assert.Equal(100m, item.PatientShare);
            Assert.Equal("Insurance policy is inactive", item.RejectionReason);
        }

        [Fact]
        public void ProcessInsurance_MedicationNotCovered_ReturnsFalse()
        {
            // Arrange
            var item = new PatientBillingItem { BaseAmount = 200m };

            // Act
            bool result = item.ProcessInsuranceCoverage(true, "Standard", false, 50m);

            // Assert
            Assert.False(result);
            Assert.Equal(200m, item.PatientShare);
            Assert.Equal("Medication not covered by this plan", item.RejectionReason);
        }

        [Fact]
        public void ProcessInsurance_PremiumTierWithinLimit_CalculatesTenPercent()
        {
            // Arrange
            var item = new PatientBillingItem { BaseAmount = 100m };

            // Act
            bool result = item.ProcessInsuranceCoverage(true, "Premium", true, 50m);

            // Assert
            Assert.True(result);
            Assert.Equal(10m, item.PatientShare);
        }

        [Fact]
        public void ProcessInsurance_StandardTierExceedingLimit_CapsAtMaxLimit()
        {
            // Arrange
            var item = new PatientBillingItem { BaseAmount = 500m };

            // Act
            bool result = item.ProcessInsuranceCoverage(true, "Standard", true, 40m);

            // Assert
            Assert.True(result);
            Assert.Equal(40m, item.PatientShare);
        }
    }
}