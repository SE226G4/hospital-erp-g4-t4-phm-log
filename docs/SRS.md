# Software Requirements Specification (SRS)
## Project: [Hospital Enterprise Resource Planning (Hospital ERP) System]
## Module/Subsystem: [Pharmacy Logistics and Dispensing Audit Subsystem (PHM-LOG)]
**Version:** 1.0  
**Date:** [2026-05-20]

---

## 1. Introduction
### 1.1 Purpose
The purpose of this document is to specify the comprehensive functional and non-functional requirements for the Pharmacy Logistics and Dispensing Audit Subsystem (Module 4). The intended audience includes software developers, database administrators, quality assurance engineers, and the hospital integration management team. For this subsystem team, this document establishes the precise behavioral logic, validation rules, and integration contracts governing safe medication dispensing.

### 1.2 Scope
This subsystem manages the clinical validation, real-time stock control, and audit tracking of the medication dispensing workflow. 
* What the system WILL do:
    * Validate patient identity digitally through national ID integration with the Admission System (Module 1).
    * Automate real-time clinical verification by cross-referencing requested medications against patient allergies and medical risks fetched from the Unified Patient Record (Module 1).
    * Perform real-time physical inventory validation and auto-deduction upon successful dispensing.
    * Enforce drug expiration date checks to block out-of-date stock.
    * Log immutable dispensing records into the Medication Administration Record (MAR) audit trail.
    * Automatically transmit financial cost structures to the Billing and Financial System (Module 2).
    * Trigger automated electronic restock requests to the Central Warehouse (Module 7).
* What the system WILL NOT do:
    * It will not allow manual modification of patient clinical risk files (handled exclusively by Module 1).
    * It will not process direct financial payments or manage insurance claims (handled exclusively by Module 2).
    * It will not manage procurement contracts or physical tracking inside the Central Warehouse (handled exclusively by Module 7).

### 1.3 Definitions, Acronyms, and Abbreviations
| Term/Acronym | Definition |
| :--- | :--- |
| ERP | Enterprise Resource Planning. |
| SRS | Software Requirements Specification. |
| MAR | Medication Administration Record. |
| PHM-LOG | Pharmacy Health Management - Logistics and Auditing. |

### 1.4 References
* IEEE 830-1998 Standard for Software Requirements Specifications.
* Master Architectural Integration Framework Contract – Shared API Contracts between Modules 1, 2, 4, and 7.

### 1.5 Overview
This SRS is organized into four main sections: Section 1 outlines the organizational parameters and subsystem scope; Section 2 offers a high-level operational perspective, user attributes, and core constraints; Section 3 delivers detailed technical specifications written as Agile User Stories tied to Acceptance Criteria; Section 4 contains architectural diagrams and structural traceability checklists.

---

## 2. Overall Description
### 2.1 Product Perspective
The Pharmacy Logistics and Dispensing Audit Subsystem is a core module within the larger Hospital ERP ecosystem. It acts as an intermediary clinical and logistical validation engine. It does not operate in isolation; it dynamically consumes web services exposed by the Admission/Risk system and feeds real-time transactional data down to the Financial and Inventory frameworks.

*   **2.1.1 System Interfaces:**  Consumes Restful JSON APIs from Module 1 (/api/patient/validate and /api/patient/risk-profile). Exposes transactional billing hooks to Module 2 (/api/finance/charge-item). Emits supply-chain triggers to Module 7 (/api/warehouse/restock).
*   **2.1.2 User Interfaces:** A web-based desktop application optimized for pharmacy hardware layout, complying with the unified hospital ERP typography and color schema.
*   **2.1.3 Hardware Interfaces:** Integrated with peripheral USB barcode scanners for instant drug serialization and batch identification.
*   **2.1.4 Software Interfaces:** Deployed on cross-platform .NET 8.0/9.0 runtime environments with an underlying enterprise relational database schema.
*   **2.1.5 Communications Interfaces:**  All inter-module communication is conducted via HTTP/REST utilizing TLS 1.3 security protocols.
*   **2.1.6 Memory & Operational Constraints:** Minimum 8GB RAM local workstation deployment; transaction payload execution requires a state persistence window under high database concurrency.

### 2.2 Product Functions
* Digital National ID lookup and validation.
* Automated clinical risk profile ingestion.
* Drug-allergy interaction cross-matching.
* Physical stock volume and expiry verification.
* Real-time stock ledger auto-deduction.
* Immutable audit logging (MAR).
* Automated financial billing transmission.
* Automated low-stock threshold alert and restock routing.

### 2.3 User Characteristics
* Hospital Pharmacists: Highly trained medical professionals with moderate technical capability. They require an efficient interface that emphasizes clinical safety warnings and eliminates redundant data input.

### 2.4 Constraints, Assumptions, and Dependencies
* Regulatory Constraint: Medical data privacy standards mandate 0% compromise on medical records encryption.
* Technical Dependency: The module assumes constant network availability of Module 1's Risk Database. If Module 1 is down, a secure local fail-safe read-only cache must prevent complete operational blackout.
* Critical Assumption: Patient risk logs and allergies are accurately updated by the clinical team in Module 1 prior to pharmacy arrivals.

---

## 3. Specific Requirements (Agile Approach)
* **Instruction:** This section translates traditional functional requirements into Agile User Stories. Every feature must be traceable to the project management board.

### 3.1 External Interface Requirements
All inter-module communication payloads must use strict JSON formatted contracts. Example endpoint verification parameters:
* POST /api/dispense/validate-clinical: Payload containing {PatientID, DrugID, Quantity}. Returns {Status: Allowed/Blocked, ConflictDetails: String}.

### 3.2 System Features & User Stories

#### 3.2.1 Feature: [Digital Identity and Clinical Risk Validation]
* Description: Validates patient credentials and checks clinical risk parameters prior to dispensing.
* Priority: High.
* User Stories:
    * Story 1 (FR1 & FR2): As a pharmacist, I want to search for a patient using their National ID so that I can automatically pull up their digital identity and load their clinical risk profile from Module 1.
        * *Acceptance Criteria:* The system must display patient name and risk profile within 2 seconds of entering a valid National ID. It must block further processing if the ID is invalid.
        * *GitHub Issue:* #101
    * Story 2 (FR3 & FR4): As a pharmacist, I want the system to automatically cross-match the chemical components of the requested medication against the patient's registered allergy profile so that I am prevented from dispensing dangerous contraindications.
        * *Acceptance Criteria:* If a conflict is discovered, the system must trigger a distinct hard-block screen alert, display the conflict detail, and physically disable the "Approve Dispense" button. Safety margin error tolerance is exactly 0%.
        * *GitHub Issue:* #102

#### 3.2.2 Feature: [Real-time Stock Control and Expiry Management]
* Description: Governs inventory integrity, checks batch parameters, and executes stock movements.
* Priority: High.
* User Stories:
    * Story 1 (FR5 & FR7): As a pharmacist, I want the system to verify local stock availability before processing a request so that I can be blocked from issuing orders that exceed physical shelf capacity.
  * *Acceptance Criteria:* The system must check current stock quantities. If requested amount > available shelf stock, it must halt execution and throw a "Stock Insufficient" warning.
        * *GitHub Issue:* #103
    * Story 2 (FR6): As a pharmacist, I want the system to execute an immediate, real-time stock deduction upon confirming a transaction so that inventory visibility remains accurate across the hospital.
  * *Acceptance Criteria:* Local stock ledgers must deduct quantities instantaneously on commit.
        * *GitHub Issue:* #104
    * Story 3 (FR11): As a pharmacist, I want the system to check the batch expiration date of the selected medicine container so that I am blocked from issuing expired materials.
  * *Acceptance Criteria:* System must block transaction if current date >= drug expiration date recorded in database, displaying an "Expired Stock" error code.
        * *GitHub Issue:* #105

#### 3.2.2 Feature: [Integration and Transaction Auditing]
* Description: Manages downstream data synchronization and auditing logs.
* Priority: High.
* User Stories:
    * Story 1 (FR8): As a hospital administrator, I want every completed dispensing transaction to be immutably tied to the patient's record, including time stamps and pharmacist credentials, so that we retain an unalterable audit trail.
        * *Acceptance Criteria:* Write a non-modifiable entry into the MAR database partition upon transaction commitment.
        * *GitHub Issue:* #106
    * Story 2 (FR9): As a financial officer, I want the system to automatically calculate and push transaction costs to Module 2 so that patient billing updates transparently.
        * *Acceptance Criteria:* An asynchronous event must securely post cost payloads to Module 2 APIs immediately upon dispensing completion.
        * *GitHub Issue:* #107
    * Story 3 (FR10): As a pharmacy inventory manager, I want the system to automatically submit a restock requisition to the central warehouse (Module 7) when stock falls below minimum reorder marks.
        * *Acceptance Criteria:* When local shelf count reaches its designated minimum safety threshold, an automated replenishment ticket must be generated and piped to Module 7.
        * *GitHub Issue:* #108
     
 
### 3.3 Performance Requirements
* Response Time: Clinical risk cross-matching and verification logic loops must complete processing in under 2 seconds under a benchmark load of 50 concurrent validation threads.
* Data Synchronization: Cost transmission updates to the financial systems and ledger updates must achieve atomic commit sequences within 1 second of transactional clearance.

### 3.4 Logical Database Requirements
The subsystem manages structural entities including DrugItem, StockLedger, DispensingRecord, and RestockTicket. It maintains a read-only transactional dependency mapping against the external PatientProfile and ClinicalRisk entities managed within Module 1.

### 3.5 Software System Attributes
* Reliability: The subsystem must maintain 99.99% operational uptime. Clinical allergy check operations must achieve a 0% failure execution record.
* Security: Access is restricted to authenticated pharmacists via Role-Based Access Control (RBAC). Patient identity schemas, clinical allergy records, and payload parameters must be encrypted using AES-256 standard protocols at rest and TLS 1.3 protocols in transit.
* Maintainability: The solution code architecture must conform to strict N-Tier separation standards separating Data Access, Business Operations, and Web presentation components. All public interface domains require structural C# XML notation styling.

---

## 4. Appendices
### Appendix A: Glossary & Models
* Entity-Relationship Diagrams :
* 1- ERD [Entity_Relationship_Diagram](https://drive.google.com/file/d/1TyI8rrRpJfXZMJcAsVg8CS-2iefUVVH2/view?usp=sharing)
* 2- Class diagram [Class_Diagram](https://drive.google.com/file/d/1vSGnf0jOWZXRykZoZu9CGO8NCvCxL3bA/view?usp=sharing)
* 3- Sequence diagram [Sequence_Diagram](https://drive.google.com/file/d/14zgPyvjjMNqEp_h8Hj1dgmm8RTGVCUIO/view?usp=drivesdk)
* 4- UseCase Diagram [UseCase_Diagram](https://drive.google.com/file/d/1iP0OeoImrvcAbfynnFjGfS3f3qDuGUEE/view?usp=drivesdk)

### Appendix B: GitHub Traceability Checklist
* [x] Every User Story in Section 3.2 has a corresponding GitHub Issue (#101 through #108).
* [x] Every GitHub Issue has an appropriate label matching functional requirements criteria (clinical-validation, inventory-deduction, financial-integration).
* [x] Pull Requests reference the relevant Issue IDs to ensure clean automation close patterns. 
