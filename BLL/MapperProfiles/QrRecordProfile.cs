using AutoMapper;
using BLL.DTOs.QrRecordDtos;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MapperProfiles
{
    public class QrRecordProfile : Profile
    {
        public QrRecordProfile()
        {
            CreateMap<QrRecordAddDto, QrRecord>().ReverseMap();

            CreateMap<QrRecord, QrRecordDto>().ReverseMap();
            
            CreateMap<QrRecord, QrRecordListDto>().ReverseMap();
           
            CreateMap<QrRecord, QrRecordDetailDto>().ReverseMap();
           
            CreateMap<QrRecord, QrRecordDeleteDto>().ReverseMap();
            
            CreateMap<QrRecord, QrRecordDto>().ReverseMap();

            CreateMap<QrRecordUpdateDto, QrRecord>().ReverseMap();


        }
    }
}
