using EmiSchedular.Application.Common;
using EmiSchedularService.Application.Common.Exceptions;
using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Application.Interfaces;
using EmiSchedularService.Domain.Models;
using EmiSchedularService.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmiSchedularService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmiController : ControllerBase
    {
        private readonly GenerateEmiSchedularService _service;
        private readonly IEmiSchedularRepository _repo;
        public EmiController(GenerateEmiSchedularService service, IEmiSchedularRepository repo)
        {
            _service = service;
            _repo = repo;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate(GenerateEmiScheduleRequest request)
        {
            var result = await _service.GenerateEmiSchedule(request);
            if (result == null)
            {
                return NotFound("something went wrong");
            }
            var response = ApiResponse<List<EmiSchedule>>.SuccessResponse(result, "Generate successfullly");
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmiList(int loanId, PaymentStatus? paymentStatus)
        {
            var result = await _service.GetAllGeneratedEmisByLoanId(loanId, paymentStatus);
            if (result == null)
            {

                var error = ApiResponse<IEnumerable<GenerateEmiScheduleResponse>>.SuccessResponse(result, "Not Data present");
                return Ok(error);
            }

            var response = ApiResponse<IEnumerable<GenerateEmiScheduleResponse>>.SuccessResponse(result, "Fetch successfullly");
            return Ok(response);
        }

        [HttpGet("{scheduleId:long}")]
        public async Task<IActionResult> GetById(long scheduleId)
        {
            var result = await _repo.GetByScheduleIdAsync(scheduleId);
            if (result == null)
                return NotFound();

            return Ok(ApiResponse<GenerateEmiScheduleResponse>.SuccessResponse(result, "done"));
        }


        [HttpGet("{loanId:long}/upcoming")]
        public async Task<IActionResult> GetUpcomingEmi(long loanId)
        {
            var result = await _service.GetUpcomingAsync(loanId);
            if (result == null)
            {
                throw new Exception("Data Not Found");
            }
            var response = ApiResponse<IEnumerable<GenerateEmiScheduleResponse>>.SuccessResponse(result, "Fecth Successfully");
            return Ok(response);
        }

        [HttpGet("/{loanId:long}/installment/{number:int}")]
        public async Task<IActionResult> GetByInstallmentNumber(long loanId , long number)
        {
            var result =await _service.GetByInstllmentNumberAsync(loanId, number);
            if(result == null)
            {
                throw new NotFoundException("Loan", result.LoanId);
            }
            var response = ApiResponse<GenerateEmiScheduleResponse>.SuccessResponse(result, "Fecth successfully");
            return Ok(response);
        }


        [HttpGet("currentEmis")]
        public async Task<IActionResult> GetByInstallmentNumber()
        {
            var data =  await _repo.getCurrentPendingEmiAsync();
            return Ok(ApiResponse<List<GenerateEmiScheduleResponse>>.SuccessResponse(data, "success"));
        }

        [HttpPut("update/penality")]
        public async Task<IActionResult> GetByInstallmentNumber(UpdatePenalityDto dto)
        {
              await _repo.UpdatepenalityAmountAsync(dto);
            return Ok(ApiResponse<string>.SuccessResponse("penality updated ", "success"));
        }
        [HttpPut("updateFromLoanPayment")]
        public async Task<IActionResult> UpdateEmiSchedule(UpdatePenalityDto dto)
        {
            await _repo.UpdateEMISchedule(dto);
            return Ok(ApiResponse<string>.SuccessResponse("EMI schedule Updated", "success"));
        }
        [HttpGet("getOverDueEMI")]
        public async Task<IActionResult> GetOverDueEmi()
        {
            var datalist =await _repo.GetOverDueEmi();
            return Ok(ApiResponse<List<GenerateEmiScheduleResponse>>.SuccessResponse(datalist, "success"));
        }
        [HttpPut("updatePenalty")]
        public async Task<IActionResult> updatePenaltyAmountForScheduleEMI(UpdatePenalityDto dto)
        {
            await _repo.UpdatepenalityAmountAsync(dto);
            return Ok(ApiResponse<string>.SuccessResponse("EMI schedule Penalty Updated", "success"));
        }
        [HttpGet("GetPenaltyByLoanID")]
        public async Task<IActionResult> GetPenalty(int loanid)
        {
            return Ok();
        }

    }
}
