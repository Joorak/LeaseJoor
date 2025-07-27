using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // جدول افراد (مشتری، ضامن، تامین‌کننده و ...)
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public bool IsLegalEntity { get; set; } // حقیقی یا حقوقی

        [Required, MaxLength(10)]
        public string NationalId { get; set; } // کدملی یا شناسه ملی

        [Required]
        public DateTime BirthDate { get; set; } // تاریخ تولد یا تاسیس

        [Required, MaxLength(20)]
        public string MobileNumber { get; set; }

        [MaxLength(200)]
        public string ImageUrl { get; set; } // تصویر یا لوگو

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    // جدول استعلامات خارج‌سازمانی
    public class ThirdPartyService
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string WebApiUrl { get; set; }

        [Required, MaxLength(50)]
        public string ServiceType { get; set; } // نوع استعلام (اعتبارسنجی، هویت‌سنجی و ...)

        [Required]
        public string Parameters { get; set; } // پارامترها به‌صورت JSON

        [MaxLength(200)]
        public string AuthToken { get; set; } // توکن احراز هویت

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // جدول فرد-استعلام
    public class PersonInquiry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [Required]
        public Guid ThirdPartyServiceId { get; set; }

        [ForeignKey("ThirdPartyServiceId")]
        public ThirdPartyService ThirdPartyService { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; } // نقش فرد (مشتری، ضامن، تامین‌کننده)

        [Required]
        public string Result { get; set; } // نتیجه استعلام به‌صورت JSON

        public DateTime InquiryDate { get; set; } = DateTime.UtcNow;
    }

    // جدول مشخصات اضافی افراد
    public class PersonAttribute
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [Required, MaxLength(50)]
        public string AttributeType { get; set; } // نوع مشخصه (مشتری، ضامن، تامین‌کننده)

        [Required]
        public string AttributeValue { get; set; } // مقادیر به‌صورت JSON
    }

    // جدول طبقه‌بندی کالاها
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public Guid? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public ProductCategory ParentCategory { get; set; }

        public bool IsLeaf { get; set; } // آیا نود انتهایی است
    }

    // جدول طرح‌های تسهیلاتی
    public class LoanScheme
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public Guid? SupplierId { get; set; } // تامین‌کننده مرتبط (اختیاری)

        [ForeignKey("SupplierId")]
        public Person Supplier { get; set; }

        public Guid? ProductCategoryId { get; set; } // طبقه کالا (اختیاری)

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }

        [Required]
        public decimal MaxLoanAmount { get; set; } // حداکثر مبلغ تسهیلات

        [Required]
        public int MaxInstallments { get; set; } // حداکثر تعداد اقساط

        public int GracePeriodDays { get; set; } // روزهای تنفس

        [Required]
        public decimal InterestRate { get; set; } // نرخ سود

        [Required]
        public decimal EffectiveInterestRate { get; set; } // نرخ سود واقعی

        [Required]
        public string PenaltyFormula { get; set; } // فرمول جریمه

        public string RequiredGuarantees { get; set; } // وثایق/تضامین موردنیاز (JSON)
    }

    // جدول اعتبارسنجی‌های طرح
    public class LoanSchemeValidation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid LoanSchemeId { get; set; }

        [ForeignKey("LoanSchemeId")]
        public LoanScheme LoanScheme { get; set; }

        [Required]
        public Guid ThirdPartyServiceId { get; set; }

        [ForeignKey("ThirdPartyServiceId")]
        public ThirdPartyService ThirdPartyService { get; set; }

        [Required]
        public int WizardStep { get; set; } // مرحله ویزارد

        [Required]
        public string ValidResultCriteria { get; set; } // معیارهای مجاز (JSON)
    }

    // جدول سفارش
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string OrderNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Person Customer { get; set; }

        public Guid? LoanSchemeId { get; set; }

        [ForeignKey("LoanSchemeId")]
        public LoanScheme LoanScheme { get; set; }

        public Guid? ContractId { get; set; }

        [ForeignKey("ContractId")]
        public Contract Contract { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } // وضعیت سفارش (جدید، تکمیل، در حال بررسی و ...)
    }

    // جدول اقلام سفارش
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Person Supplier { get; set; }

        [Required, MaxLength(200)]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } // تخفیف به درصد

        [MaxLength(200)]
        public string ImageUrl { get; set; } // آدرس تصویر محصول
    }

    // جدول وضعیت سفارش
    public class OrderStatusLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        public DateTime StatusDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string Description { get; set; }
    }

    // جدول ضامن‌های سفارش
    public class OrderGuarantor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public Guid GuarantorId { get; set; }

        [ForeignKey("GuarantorId")]
        public Person Guarantor { get; set; }
    }

    // جدول وثایق/تضامین سفارش
    public class OrderCollateral
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public string CollateralDetails { get; set; } // جزئیات وثیقه (JSON)
    }

    // جدول مدارک فرد
    public class PersonDocument
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; } // نقش فرد

        [Required, MaxLength(200)]
        public string DocumentUrl { get; set; }

        [Required, MaxLength(100)]
        public string DocumentType { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }

    // جدول قرارداد
    public class Contract
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string ContractNumber { get; set; }

        [Required]
        public DateTime ContractDate { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string ContractDetails { get; set; } // جزئیات قرارداد (JSON)
    }

    // جدول اسناد مالی
    public class FinancialRecord
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [Required, MaxLength(50)]
        public string TransactionType { get; set; } // بدهکار/بستانکار

        [Required]
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        public string ReferenceNumber { get; set; } // شماره پیگیری

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string Description { get; set; }
    }

    // جدول اقساط
    public class Installment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ContractId { get; set; }

        [ForeignKey("ContractId")]
        public Contract Contract { get; set; }

        [Required]
        public int InstallmentNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } // وضعیت قسط (پرداخت‌شده، در انتظار و ...)
    }

    // جدول پارامترهای فرمول
    public class FormulaParameter
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string OutputType { get; set; } // نوع خروجی (عدد، تاریخ و ...)

        [Required]
        public string Query { get; set; } // کوئری SQL

        public bool HasAdditionalParameters { get; set; } // آیا پارامتر اضافی دارد
    }

    // جدول پیام‌ها (Message)
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        [MaxLength(50)]
        public string EventType { get; set; } // نوع رویداد (قبول سفارش، سررسید پرداخت و ...)

        public bool IsRead { get; set; } // خوانده‌شده یا خیر

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
    }

    // جدول تنظیمات کاربر (UserSetting)
    public class UserSetting
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        public string ProfileJson { get; set; } // اطلاعات پروفایلی قابل تغییر (پسورد، روش ورود و ...)

        public string IdentityJson { get; set; } // اطلاعات هویتی غیرقابل ویرایش (از ثبت احوال)

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
    }

    public class CountriesTurnoverStat
    {
        //[Key]
        //public int Id { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public double LeasingVolume { get; set; }
        public double MarketShare { get; set; }
    }
}