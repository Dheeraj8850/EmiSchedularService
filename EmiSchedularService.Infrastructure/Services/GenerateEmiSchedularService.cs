using Azure.Core;
using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Application.Interfaces;
using EmiSchedularService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Infrastructure.Services
{
    public class GenerateEmiSchedularService
    {
        private readonly IEmiSchedularRepository _repository;
        public GenerateEmiSchedularService(IEmiSchedularRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GenerateEmiScheduleResponse>> GetAllGeneratedEmisByLoanId(int loanId, PaymentStatus? status)
        {
            var listEmi =await _repository.GetByLoanIdAsync(loanId,status);
            if (!listEmi.Any())
            {
                return null;
            }
            return listEmi;
        }


        public async Task<List<EmiSchedule>> GenerateEmiSchedule(GenerateEmiScheduleRequest request)
        {
            var scheduleList = new List<EmiSchedule>();

            decimal balance = request.LoanAmount;
            decimal monthlyRate = request.InterestRate / 12 / 100;

            DateTime dueDate = request.DisbursementDate.AddMonths(1);

            for (int i = 1; i <= request.TenureMonths; i++)
            {
                decimal interest = balance * monthlyRate;
                decimal principal = request.EmiAmount - interest;
                decimal closingBalance = balance - principal;

                if (i == request.TenureMonths)
                {
                    principal = balance;
                    interest = request.EmiAmount - principal;
                    closingBalance = 0;
                }

                var schedule = new EmiSchedule
                {
                    LoanId = request.LoanId,
                    InstallmentNumber = i,
                    DueDate = dueDate,
                    EmiAmount = request.EmiAmount,

                    OpeningBalance = Math.Round(balance, 2),   
                    InterestComponent = Math.Round(interest, 2),
                    PrincipalComponent = Math.Round(principal, 2),
                    ClosingBalance = Math.Round(closingBalance, 2),

                    PaymentStatus = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                scheduleList.Add(schedule);

                balance = closingBalance;  
                dueDate = dueDate.AddMonths(1);
            }

            await _repository.AddAsync(scheduleList);

            return scheduleList;
        }


        public async Task<IEnumerable<GenerateEmiScheduleResponse>> GetUpcomingAsync(long lonId)
        {
            var result =await _repository.GetByUpcomingAsync(lonId);
            return result;
        }

        public async Task<GenerateEmiScheduleResponse> GetByInstllmentNumberAsync(long loanId , long number)
        {
            var result =await _repository.GetByInstallmentNumberAsync(loanId,number );
            return result;
        }

    }
}
