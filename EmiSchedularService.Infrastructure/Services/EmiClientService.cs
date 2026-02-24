using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Infrastructure.Services
{
    public class EmiClientService : IEmiServiceClient
    {
        private readonly HttpClient _httpClient;

        public EmiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateEmiScheduleAsync(GenerateEmiScheduleRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("schedule", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to generate EMI schedule");
            }
        }
    }
}
