using System;

namespace PharmacyAssignment.Core
{
    public class PatientBillingItem
    {
        public decimal BaseAmount { get; set; }
        public decimal PatientShare { get; set; }
        public string? RejectionReason { get; set; }

        public bool ProcessInsuranceCoverage(bool isInsuranceActive, string coverageTier, bool isMedicationCovered, decimal maxLimit)
        {
            if (!isInsuranceActive)
            {
                PatientShare = BaseAmount;
                RejectionReason = "Insurance policy is inactive";
                return false;
            }

            if (!isMedicationCovered)
            {
                PatientShare = BaseAmount;
                RejectionReason = "Medication not covered by this plan";
                return false;
            }

            decimal coveragePercentage = 0.50m;
            if (coverageTier == "Premium")
            {
                coveragePercentage = 0.10m;
            }
            else if (coverageTier == "Standard")
            {
                coveragePercentage = 0.30m;
            }

            decimal calculatedShare = BaseAmount * coveragePercentage;

            if (calculatedShare > maxLimit)
            {
                PatientShare = maxLimit;
            }
            else
            {
                PatientShare = calculatedShare;
            }

            return true;
        }
    }
}