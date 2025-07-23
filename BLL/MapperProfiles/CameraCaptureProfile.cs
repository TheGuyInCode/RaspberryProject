using AutoMapper;
using BLL.DTOs.CameraCaptureDtos;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MapperProfiles
{
    public class CameraCaptureProfile :Profile
    {
        public CameraCaptureProfile()
        {

            CreateMap<CameraCaptureAddDto, CameraCapture>().ReverseMap();
            
            CreateMap<CameraCapture, CameraCaptureListDto>()
                .ForMember(dest => dest.Base64ImageData, opt => opt.MapFrom(src =>
                    $"data:image/png;base64,{Convert.ToBase64String(src.ImageData ?? Array.Empty<byte>())}")).ReverseMap();
           
            CreateMap<CameraCapture, CameraCaptureDetailDto>()
                .ForMember(dest => dest.Base64ImageData, opt => opt.MapFrom(src =>
                    $"data:image/png;base64,{Convert.ToBase64String(src.ImageData ?? Array.Empty<byte>())}")).ReverseMap();

            CreateMap<CameraCapture, CameraCaptureDto>()
                .ForMember(dest => dest.Base64ImageData, opt => opt.MapFrom(src =>
                    $"data:image/png;base64,{Convert.ToBase64String(src.ImageData ?? Array.Empty<byte>())}"))
                .ReverseMap()
                .ForMember(dest => dest.ImageData, opt => opt.Ignore());

        }
    }
}
