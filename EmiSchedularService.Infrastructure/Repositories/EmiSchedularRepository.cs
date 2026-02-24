using AutoMapper;
using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Application.Interfaces;
using EmiSchedularService.Domain.Models;
using EmiSchedularService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Infrastructure.Repositories
{
    public class EmiSchedularRepository : IEmiSchedularRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmiSchedularRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(List<EmiSchedule> emiSchedules)
        {

           await _context.EmiSchedules.AddRangeAsync(emiSchedules);
           await  _context.SaveChangesAsync();

        }

        public async Task<GenerateEmiScheduleResponse> GetByInstallmentNumberAsync(long loanId,long number)
        {
            var result =await _context.EmiSchedules.FirstOrDefaultAsync(x => x.LoanId == loanId && x.InstallmentNumber == number && x.DeletedAt == null);

            var mapData = _mapper.Map<GenerateEmiScheduleResponse>(result);
            return mapData;
        }

        public async Task<IEnumerable<GenerateEmiScheduleResponse>> GetByLoanIdAsync(int loanId , PaymentStatus? status)
        {
            var Emi = _context.EmiSchedules.Where(l => l.LoanId ==loanId && l.DeletedAt ==null);
            if (status.HasValue)
            {
                Emi = Emi.Where( l => l.PaymentStatus == status.Value);
            }
            var result =await Emi.OrderBy(l => l.InstallmentNumber).ToListAsync();
            return _mapper.Map<List<GenerateEmiScheduleResponse>>(result);
            
        }

        public async Task<GenerateEmiScheduleResponse> GetByScheduleIdAsync(long scheduleId )
        {
            var Emi =await _context.EmiSchedules.FirstOrDefaultAsync(q =>  q.ScheduleId == scheduleId&& q.DeletedAt == null);
            if (Emi == null)
            {
                return null;
            }

            return _mapper.Map<GenerateEmiScheduleResponse>(Emi);
        }

        public async Task<IEnumerable<GenerateEmiScheduleResponse>> GetByUpcomingAsync(long loanId)
        {
            var Emi =await _context.EmiSchedules.Where(x => x.LoanId == loanId && x.DeletedAt == null&&
            (x.PaymentStatus ==PaymentStatus.Pending || x.PaymentStatus == PaymentStatus.Overdue)).OrderBy(x  => x.InstallmentNumber).ToListAsync();

            var mapEMi = _mapper.Map<IEnumerable<GenerateEmiScheduleResponse>>(Emi);
            return mapEMi;
        }

        public async Task<List<GenerateEmiScheduleResponse>> getCurrentPendingEmiAsync()
        {
            var today = DateTime.UtcNow.Date;

            var emi = await _context.EmiSchedules
                
                .Where(e =>


                    e.PaymentStatus == PaymentStatus.Pending &&
                    e.DueDate >= today
                    )
                .GroupBy(x=>x.LoanId)
                .Select(g=>g.OrderBy(e => e.DueDate)
                    .FirstOrDefault())
                
                .ToListAsync();

            if (emi == null)
                return null;

            return _mapper.Map<List<GenerateEmiScheduleResponse>>(emi);
        }

        public async Task<List<GenerateEmiScheduleResponse>> GetOverDueEmi()
        {
            var today = DateTime.Now;
            var emi =await _context.EmiSchedules.Where(e => e.DueDate <= today && e.PaymentStatus == PaymentStatus.Pending).ToListAsync();
            if (emi == null)
            {
                return null;
            }
            return _mapper.Map<List<GenerateEmiScheduleResponse>>(emi);

        }

        public async Task UpdateEMISchedule(UpdatePenalityDto dto)
        {
            var data = await _context.EmiSchedules.FindAsync(dto.ScheduleId);
            data.PaidAmount = dto.PaidAmount;
            data.PaidDate = dto.PaidDate;
            data.ModifiedAt = dto.ModifiedAt;
            data.PaymentStatus = dto.PaymentStatus=="Paid"?PaymentStatus.Paid:PaymentStatus.PartiallyPaid;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatepenalityAmountAsync(UpdatePenalityDto dto)
        {
          var emi = await _context.EmiSchedules.Where(e => e.LoanId == dto.LoanId && e.ScheduleId == dto.ScheduleId && e.DeletedAt==null).FirstOrDefaultAsync();

            if (emi == null)
                throw new KeyNotFoundException("EMI schedule not found.");

            if (dto.PenaltyAmount < 0)
                throw new ArgumentException("Penalty amount cannot be negative.");

            emi.PenaltyAmount = dto.PenaltyAmount;
            emi.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        

    }
}
