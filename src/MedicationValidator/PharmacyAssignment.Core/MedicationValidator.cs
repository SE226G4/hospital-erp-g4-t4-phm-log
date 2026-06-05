namespace PharmacyAssignment.Core
{
    public class MedicationValidator
    {
        public string CheckMedicationStatus(int stockQuantity, bool isExpired, bool hasAllergy, bool hasChronicDisease)
        {
            string stockStatus = CheckStock(stockQuantity, isExpired);
            if (stockStatus != "Valid") return stockStatus;

            string medicalStatus = CheckMedicalProfile(hasAllergy, hasChronicDisease);
            if (medicalStatus != "Valid") return medicalStatus;

            return "Approved: Can Dispense";
        }

        private string CheckStock(int stockQuantity, bool isExpired)
        {
            if (stockQuantity <= 0) return "Rejected: Out of Stock";
            if (isExpired) return "Rejected: Expired";
            return "Valid";
        }

        private string CheckMedicalProfile(bool hasAllergy, bool hasChronicDisease)
        {
            if (hasAllergy) return "Rejected: Allergy Conflict";
            if (hasChronicDisease) return "Rejected: Chronic Disease Conflict";
            return "Valid";
        }
    }
}