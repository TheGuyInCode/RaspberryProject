using AutoMapper;
using BLL.DTOs.SensorDataDtos;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MapperProfiles
{
    public class SensorDataProfile : Profile
    {
        public SensorDataProfile()
        {
           
            CreateMap<SensorDataAddDto, SensorData>().ReverseMap();            
            CreateMap<SensorData, SensorDataDetailDto>().ReverseMap();            
            CreateMap<SensorData, SensorDataListDto>().ReverseMap();            
            CreateMap<SensorData, SensorDataDto>().ReverseMap();



        }
    }
}
