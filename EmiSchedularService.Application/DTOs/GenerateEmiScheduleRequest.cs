using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.DTOs
{
    public class GenerateEmiScheduleRequest
    {
        public long LoanId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TenureMonths { get; set; }
        public decimal EmiAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
    }
}
