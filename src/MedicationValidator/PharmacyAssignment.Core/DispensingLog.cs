using System;

namespace Module4.Core
{
    public class DispensingLog
    {
        public int Id { get; }
        public int PatientId { get; }
        public Medication Medication { get; }
        public int DispensedQuantity { get; }
        public decimal CostDecimal { get; private set; }
        public bool IsSentToFinance { get; private set; }

        public DispensingLog(int id, int patientId, Medication medication, int dispensedQuantity)
        {
            if (dispensedQuantity <= 0)
                throw new ArgumentException("Dispensed quantity must be greater than zero.", nameof(dispensedQuantity));

            Id = id;
            PatientId = patientId;
            Medication = medication ?? throw new ArgumentNullException(nameof(medication));
            DispensedQuantity = dispensedQuantity;
        }

        //  النسخة الجديدة بعد الـ Refactoring
        public void CalculateAndSendCost()
        {
            ValidateExpiry();
            ValidateStock();

            CalculateCost();
            SendCostToFinance();
            DeductMedicationStock();
        }

        //  التحقق من صلاحية الدواء
        private void ValidateExpiry()
        {
            if (Medication.IsExpired())
                throw new InvalidOperationException("Cannot dispense expired medication.");
        }

        //  التحقق من المخزون
        private void ValidateStock()
        {
            if (!Medication.IsStockSufficient(DispensedQuantity))
                throw new InvalidOperationException("Insufficient stock for dispensing.");
        }

        //  حساب التكلفة
        private void CalculateCost()
        {
            CostDecimal = Medication.UnitPrice * DispensedQuantity;
        }

        //  إرسال التكلفة للنظام المالي
        private void SendCostToFinance()
        {
            bool success = FinanceSystem.SendCost(PatientId, CostDecimal);

            if (success)
                IsSentToFinance = true;
        }

        //  خصم المخزون
        private void DeductMedicationStock()
        {
            Medication.DeductStock(DispensedQuantity);
        }
    }
}