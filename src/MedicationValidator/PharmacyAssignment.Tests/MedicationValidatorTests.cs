using Xunit;
using PharmacyAssignment.Core;

namespace PharmacyAssignment.Tests
{
    public class MedicationValidatorTests
    {
        private readonly MedicationValidator _validator;

        public MedicationValidatorTests()
        {
            _validator = new MedicationValidator();
        }

        [Fact]
        public void CheckStatus_Should_Return_OutOfStock_When_QuantityIsZero()
        {
            var result = _validator.CheckMedicationStatus(0, false, false, false);
            Assert.Equal("Rejected: Out of Stock", result);
        }

        [Fact]
        public void CheckStatus_Should_Return_Expired_When_IsExpiredTrue()
        {
            var result = _validator.CheckMedicationStatus(10, true, false, false);
            Assert.Equal("Rejected: Expired", result);
        }

        [Fact]
        public void CheckStatus_Should_Return_Allergy_When_HasAllergyTrue()
        {
            var result = _validator.CheckMedicationStatus(10, false, true, false);
            Assert.Equal("Rejected: Allergy Conflict", result);
        }

        [Fact]
        public void CheckStatus_Should_Return_Chronic_When_HasChronicTrue()
        {
            var result = _validator.CheckMedicationStatus(10, false, false, true);
            Assert.Equal("Rejected: Chronic Disease Conflict", result);
        }

        [Fact]
        public void CheckStatus_Should_Return_Approved_When_AllChecksPass()
        {
            var result = _validator.CheckMedicationStatus(10, false, false, false);
            Assert.Equal("Approved: Can Dispense", result);
        }
    }
}