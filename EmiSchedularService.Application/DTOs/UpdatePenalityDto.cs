using EmiSchedularService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.DTOs
{
    public class UpdatePenalityDto
    {
        public long ScheduleId { get; set; }

        public long LoanId { get; set; }

        //[ForeignKey("LoanId")]
        //public LoanAccount? LoanAccount { get; set; }

        public int InstallmentNumber { get; set; }

        public DateTime DueDate { get; set; }

        public decimal EmiAmount { get; set; }

        public decimal PrincipalComponent { get; set; }

        public decimal InterestComponent { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal ClosingBalance { get; set; }

        public string PaymentStatus { get; set; }
        public decimal PaidAmount { get; set; }

        public DateTime? PaidDate { get; set; }

        public decimal PendingAmount => EmiAmount - PaidAmount;

        public decimal PenaltyAmount { get; set; }
        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }


        public string? CreatedBy { get; set; }


        public string? DeletedBy { get; set; }

    }
}
