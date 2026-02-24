using AutoMapper;
using EmiSchedularService.Application.DTOs;
using EmiSchedularService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiSchedularService.Application.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<EmiSchedule, GenerateEmiScheduleResponse>().ReverseMap();
            CreateMap<EmiSchedule, GenerateEmiScheduleRequest>().ReverseMap();
        }
    }
}
