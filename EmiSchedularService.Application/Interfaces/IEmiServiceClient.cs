using EmiSchedularService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.Interfaces
{
    public interface IEmiServiceClient
    {
        Task CreateEmiScheduleAsync(GenerateEmiScheduleRequest request);
    }
}
