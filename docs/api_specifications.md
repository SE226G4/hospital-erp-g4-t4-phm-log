# API Specifications

## 1. Overview
Provides medication dispensing services, patient safety verification (against allergies and chronic diseases), inventory deduction, and automated billing integration to the Finance & Insurance module. It also communicates with the Inventory module for restock alerts.

## 2. Main Endpoints
*List the functions or endpoints your module shares with others.*

### Endpoint 1: Dispense Medication
* Method: `POST`
* What it does: Processes a medication dispensing request. It verifies patient safety (allergies/chronic diseases), checks stock availability, deducts the medication from inventory, logs the dispensing action (MAR), and triggers the billing process.
* Required Data: `prescription_item_id`, `patient_id`, `medication_id`, `requested_quantity`, `dispensed_by` (Pharmacist ID)
* Returned Data: `dispensing_log_id`, `status` (Success/Rejected), `rejection_reason` (if rejected), `cost_decimal`

### Endpoint 2: Send Dispensing Cost to Finance
* Method: `POST` (Internal call to Module 2 API)
* What it does: Automatically sends the financial details of a successfully dispensed medication to the Finance & Insurance module to be added to the patient's unified bill.
* Required Data: `patient_id`, `dispensing_log_id`, `medical_service_id`, `amount` (cost), `item_name`
* Returned Data: `billing_item_id`, `payment_status` (e.g., Covered by Insurance, Pending)

### Endpoint 3: Request Inventory Restock
* Method: `POST` (Internal call to Module 7 API)
* What it does: Sends an automated restock request to the Inventory & Supplies module when a medication's stock quantity falls to the minimum alert level.
* Required Data: `medication_id`, `medication_name`, `current_stock`, `requested_quantity`
* Returned Data: `restock_request_id`, `request_status`

### Endpoint 4: Verify Patient Medical Profile
* Method: `GET` (Internal call to Module 1 API)
* What it does: Fetches the patient's critical medical history (allergies, chronic diseases) from the Admission module to verify safety before dispensing any medication.
* Required Data: `patient_id` or `national_id`
* Returned Data: `allergies` (List), `chronic_diseases` (List), `blood_type`
