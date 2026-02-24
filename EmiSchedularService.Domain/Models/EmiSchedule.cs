using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Domain.Models
{
    public class EmiSchedule
    {

        [Key]
        public long ScheduleId { get; set; }


        [Required]
        public long LoanId { get; set; }

        //[ForeignKey("LoanId")]
        //public LoanAccount? LoanAccount { get; set; }


        [Required]
        public int InstallmentNumber { get; set; }

        public DateTime DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EmiAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrincipalComponent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestComponent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ClosingBalance { get; set; }

       

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }

        public DateTime? PaidDate { get; set; }

        [NotMapped]
        public decimal PendingAmount => EmiAmount - PaidAmount;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PenaltyAmount { get; set; }

        [NotMapped]
        public decimal TotalDue => PendingAmount + PenaltyAmount;

        [NotMapped]
        public int DaysOverdue =>
            (PaymentStatus != PaymentStatus.Paid && DateTime.Today > DueDate)
                ? (DateTime.Today - DueDate).Days
                : 0;


        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? DeletedBy { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Overdue = 3,
        PartiallyPaid = 4
    }
}

