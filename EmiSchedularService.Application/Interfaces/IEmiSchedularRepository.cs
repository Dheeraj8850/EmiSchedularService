using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.Interfaces
{
    public interface IEmiSchedularRepository
    {
        Task<IEnumerable<GenerateEmiScheduleResponse>> GetByLoanIdAsync(int loanId , PaymentStatus? status);

        Task<GenerateEmiScheduleResponse> GetByScheduleIdAsync(long scheduleId);

        Task AddAsync(List<EmiSchedule> emiSchedules);

        Task<IEnumerable<GenerateEmiScheduleResponse>> GetByUpcomingAsync(long loanId);

        Task<GenerateEmiScheduleResponse> GetByInstallmentNumberAsync(long loanId , long number);


        Task<List<GenerateEmiScheduleResponse>> getCurrentPendingEmiAsync();

        Task UpdatepenalityAmountAsync(UpdatePenalityDto dto);
        Task UpdateEMISchedule(UpdatePenalityDto dto);
        Task<List<GenerateEmiScheduleResponse>> GetOverDueEmi();
        
    }
}
