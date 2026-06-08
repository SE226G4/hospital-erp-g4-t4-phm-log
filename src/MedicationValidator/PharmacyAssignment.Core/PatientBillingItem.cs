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
                SetRejectionFields(BaseAmount, "Insurance policy is inactive");
                return false;
            }

            if (!isMedicationCovered)
            {
                SetRejectionFields(BaseAmount, "Medication not covered by this plan");
                return false;
            }

            decimal coveragePercentage = GetCoveragePercentage(coverageTier);
            decimal calculatedShare = BaseAmount * coveragePercentage;

            PatientShare = Math.Min(calculatedShare, maxLimit);
            return true;
        }

        private void SetRejectionFields(decimal share, string reason)
        {
            PatientShare = share;
            RejectionReason = reason;
        }

        private decimal GetCoveragePercentage(string tier)
        {
            return tier switch
            {
                "Premium" => 0.10m,
                "Standard" => 0.30m,
                _ => 0.50m
            };
        }
    }
}