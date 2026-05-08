# Module Name: Pharmacy Logistics & MAR 
## Project: Hospital ERP
Module Code: PHM-LOG
---

## 📝 Module Overview
This module manages pharmacy medication dispensing to ensure patient safety. It automatically deducts medications from inventory when dispensed, prevents dispensing if the patient has allergies or drug interactions, checks for sufficient stock, and sends billing information to the financial system.

---

## 👥 Team Members & Responsibilities
*This table is flexible. Assign tasks based on team size (4 to 6 members).*

| Member Name | Primary Responsibility | Assigned Tasks (Examples) | GitHub Profile |
| :--- | :--- | :--- | :--- |
| **shahd ibraheem (Leader)** | Integration & Architecture | Component Diagrams, API Specs, Team Coordination | [https://github.com/shahd-ibraheem] |
| **asmaa machal** | Requirements & Analysis | Functional Requirements, Use Case Diagrams | [https://github.com/AsmaaMachal2004] |
| **seidra zedan** | Process Modeling | Activity Diagrams, Business Rules Validation | [https://github.com/siedra-ziedan] |
| **ghaidaa machal** | Data Design | ERD, Database Schema, Class Diagrams | [https://github.com/GhaidaaMachal2005] |
| **lama izz aldeen** | Interaction Design | Sequence Diagrams, Logic Flow | [https://github.com/Lamaizzaldeen] |
| **miray daher** | UI/UX & Frontend | Wireframes, Interface Logic, User Stories | [https://github.com/miray-daher] |

---

## 🚀 Analysis & Design Progress
- [ ] **Requirement Elicitation:** Completed list of FRs/NFRs.
- [ ] **UML Behavioral Diagrams:** Use Case and Activity Diagrams.
- [ ] **UML Structural Diagrams:** ERD and Class Diagrams.
- [ ] **Dynamic Modeling:** Sequence Diagrams for core processes.
- [ ] **Interface Design:** Low-fidelity Wireframes.

---

## 🔗 Integration Points
*How this module communicates with others:*

###  Inbound (Data Received)
| Source Module | Data Description |
| ADM-MC (Admission & Medical Coding) | Unified patient record (National ID, digital ID, blood type), Risk profile (allergies, chronic diseases) |


###  Outbound (Data Sent)
| Destination Module | Data Description |
| FIN-INS (Finance & Insurance) | Medication cost + Patient ID + Dispensing date + Transaction ID |
| SEC-GOV (Data Security & Governance) | Dispensing log (who, when, what, patient) |

###  Internal (Inventory Check)
- Stock availability check before dispensing
- Automatic stock deduction after dispensing

---
## 🛠 Tools Used
* **Modeling:** e.g., StarUML / Draw.io.
* **Documentation:** Word / GitHup(README.md).
* **Version Control:** GitHub.
