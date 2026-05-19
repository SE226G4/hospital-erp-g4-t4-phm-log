using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalERP.PharmacyModule
{
    // ==========================================
    // 1. جدول المرضى (يأتي من Module 1)
    // ==========================================
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string NationalId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(10)]
        public string BloodType { get; set; }

        public string Allergies { get; set; } // خاصية حرجة للتدقيق
        public string ChronicDiseases { get; set; } // خاصية حرجة للتدقيق

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties (العلاقات)
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<DispensingLog> DispensingLogs { get; set; }
        public virtual ICollection<PatientBillingItem> BillingItems { get; set; }
    }

    // ==========================================
    // 2. جدول الأدوية والمخزون
    // ==========================================
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        public int StockQuantity { get; set; } // حرج: يُخصم منه تلقائياً

        public int MinStockAlert { get; set; } // للتنبيه بطلب تزويد

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // لإرسال التكلفة للمالية

        [Required]
        public DateTime ExpiryDate { get; set; } // حرج: لمنع الصرف إذا منتهي

        [MaxLength(20)]
        public string Status { get; set; }

        // Navigation Properties
        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; }
        public virtual ICollection<DispensingLog> DispensingLogs { get; set; }
    }

    // ==========================================
    // 3. جدول الوصفات الطبية (رأس الوصفة)
    // ==========================================
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        public int VisitId { get; set; }
        public int DoctorId { get; set; }
        public DateTime IssueDate { get; set; }
        
        [MaxLength(20)]
        public string Status { get; set; } // قيد التنفيذ، مكتملة

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        
        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; }
    }

    // ==========================================
    // 4. جدول عناصر الوصفة (أدوية الوصفة)
    // ==========================================
    public class PrescriptionItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        [Required]
        public int MedicationId { get; set; }

        [Required]
        public int RequestedQuantity { get; set; }

        [MaxLength(100)]
        public string Dosage { get; set; } // الجرعة

        [MaxLength(20)]
        public string Status { get; set; } // تم الصرف، مرفوض

        public string RejectionReason { get; set; } // حرج: سبب الرفض

        // Navigation Properties
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }

        [ForeignKey("MedicationId")]
        public virtual Medication Medication { get; set; }

        // العلاقة 1 to 0..1 مع سجل الصرف
        public virtual DispensingLog DispensingLog { get; set; } 
    }

    // ==========================================
    // 5. جدول سجل صرف الأدوية (MAR - التدقيق)
    // ==========================================
    public class DispensingLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int PrescriptionItemId { get; set; }

        [Required]
        public int MedicationId { get; set; }

        [Required]
        public int DispensedQuantity { get; set; }
        public int DispensedBy { get; set; } // موظف الصيدلية
        public DateTime DispensedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostDecimal { get; set; } // التكلفة الإجمالية

        public bool IsSentToFinance { get; set; } = false; // حرج: هل أُرسلت للمالية؟

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("PrescriptionItemId")]
        public virtual PrescriptionItem PrescriptionItem { get; set; }

        [ForeignKey("MedicationId")]
        public virtual Medication Medication { get; set; }

        // العلاقة 1 to 0..1 مع الفاتورة
        public virtual PatientBillingItem PatientBillingItem { get; set; }
    }

    // ==========================================
    // 6. جدول الخدمات الطبية (الخاص بـ Module 2)
    // ==========================================
    public class MedicalService
    {
        [Key]
        public int ServiceId { get; set; }

        [Required, MaxLength(150)]
        public string ServiceName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ServicePrice { get; set; }

        [MaxLength(50)]
        public string Department { get; set; } // القسم (صيدلية، عمليات، إلخ)

        // Navigation Property
        public virtual ICollection<PatientBillingItem> PatientBillingItems { get; set; }
    }

    // ==========================================
    // 7. جدول بنود فواتير المريض (الربط مع Module 2)
    // ==========================================
    public class PatientBillingItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        // هذا الحقل يربط الفاتورة بعملية الصرف في الصيدلية (0..1)
        public int? DispensingLogId { get; set; } 
        
        // ⚠️ هذا هو الحقل الجديد الذي يربط الفاتورة بجدول الخدمات (Module 2)
        [Required]
        public int MedicalServiceId { get; set; } 

        [MaxLength(150)]
        public string ItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } // غير مدفوع، مغطى بالتأمين

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DispensingLogId")]
        public virtual DispensingLog DispensingLog { get; set; }

        // علاقة الفاتورة بالخدمة الطبية
        [ForeignKey("MedicalServiceId")]
        public virtual MedicalService MedicalService { get; set; }
    }
}
