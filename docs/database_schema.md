# Database Schema

## 1. Entity-Relationship Diagram (ERD)
[ERD4.png]

## 2. Tables List
*List the main tables in your database.*

| Table Name | Purpose / Description |
| :--- | :--- |
| `Patient` | المرجع الأساسي للمريض (يأتي من Module 1)، يحتوي على بيانات التدقيق الطبي الحرجة (الحساسيات والأمراض المزمنة) لمنع صرف أدوية متعارضة. |
| `Medication` | كتالوج الأدوية وإدارة المخزون، يتضمن الكميات المتاحة، تاريخ الانتهاء، الحد الأدنى للتنبيه، وسعر الوحدة. |
| `Prescription` | رأس الوصفة الطبية، يربط المريض بالزيارة (Visit) والطبيب الذي كتب الوصفة. |
| `PrescriptionItem` | تفاصيل عناصر الوصفة (الأدوية المطلوبة)، تشمل الكمية، الجرعة، حالة الصرف، وسبب الرفض إن وجد. |
| `DispensingLog` | سجل التدقيق (Audit Log / MAR)، يثبت عملية صرف الدواء فعلياً للمريض ويحسب تكلفتها. |
| `MedicalService` | جدول الخدمات الطبية (مشترك مع Module 2)، يحدد اسم الخدمة، سعرها، والقسم التابع له (مثل صيدلية، عمليات). |
| `PatientBillingItem` | بنود فواتير المريض، الجدول الوسيط الذي يربط الخدمات المقدمة (مثل الصرف الطبي) بالنظام المالي والتأمين. |

## 3. Shared Data (Integration Points)
*Which tables or data do you share with other teams?*

*   **Shared Table/ID:** `Patient` (Specifically `patient_id`, `national_id`, `allergies`, `chronic_diseases`)
    *   **Shared With:** Module 1 (Admission & Medical Coding) - نستقبل بيانات المريض وملف المخاطر منهم للتدقيق الطبي قبل الصرف.
*   **Shared Table/ID:** `PatientBillingItem` & `MedicalService` (Specifically `medical_service_id`, `dispensing_log_id`, `amount`, `status`)
    *   **Shared With:** Module 2 (Finance & Insurance) - نرسل تكلفة الأدوية المصروفة تلقائياً كبنود فواتير مرتبطة بالخدمات الطبية لتحسبها ضمن الفاتورة الشاملة للمريض.
*   **Shared Table/ID:** `Medication` (Specifically `stock_quantity`, `min_stock_alert`)
    *   **Shared With:** Module 7 (Inventory & Supplies Management) - نرسل تنبيهات عند وصول مخزون الدواء للحد الأدنى لطلب تزويد، ونتلقى تحديثات الكميات منهم.
*   **Shared Table/ID:** `Prescription` (Specifically `visit_id`, `doctor_id`)
    *   **Shared With:** Module 1 / Clinic Module - نستقبل رقم الزيارة والطبيب لربط الوصفة الطبية بالزيارة الحالية للمريض.
