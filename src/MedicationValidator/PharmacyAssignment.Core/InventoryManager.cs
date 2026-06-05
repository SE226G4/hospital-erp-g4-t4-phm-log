using System;
namespace PharmacyAssignment.Core
{
    public class Medication 
    { 
        public string Id { get; set; } = string.Empty; 
        public int StockQuantity { get; set; } 
        public int MinStockAlert { get; set; } 
        public int MaxCapacity { get; set; }
        public bool IsControlledSubstance { get; set; } 
    }
    public class RestockResult 
    { 
        public string Status { get; set; } = string.Empty; 
        public string Message { get; set; } = string.Empty; 
        public string MedicationId { get; set; } = string.Empty; 
        public int RequestedQuantity { get; set; } 
    }

    public class InventoryManager
    {
        public RestockResult EvaluateRestockNeed(Medication medication, bool hasPendingRequest)
        {
            if (!NeedsRestock(medication))
            {
                return new RestockResult { Status = "Success", Message = "Sufficient stock" };
            }

            if (hasPendingRequest)
            {
                return new RestockResult { Status = "Alert Only", Message = "Low stock but pending" };
            }
            return CreateRestockRequest(medication);
        }
        private bool NeedsRestock(Medication medication)
        {
            return medication.StockQuantity <= medication.MinStockAlert;
        }
        private RestockResult CreateRestockRequest(Medication medication)
        {
            int requestedQuantity = medication.MaxCapacity - medication.StockQuantity;
            string approvalStatus;

            if (medication.IsControlledSubstance)
            {
                approvalStatus = "Restock Required - Special Approval";
                requestedQuantity = Math.Min(requestedQuantity, 50); 
            }
            else
            {
                approvalStatus = "Restock Required - Auto Approved";}
            return new RestockResult 
            { 
                Status = approvalStatus, 
                MedicationId = medication.Id, 
                RequestedQuantity = requestedQuantity 
            };
        }
    }
}