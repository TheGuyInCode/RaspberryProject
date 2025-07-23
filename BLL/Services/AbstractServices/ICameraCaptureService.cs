using BLL.DTOs.CameraCaptureDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices
{
    public interface ICameraCaptureService
    {
        Task<CameraCaptureDto> AddAsync(CameraCaptureAddDto cameraCaptureAddDto);
        Task<List<CameraCaptureListDto>> GetAllAsync();
        Task<CameraCaptureDetailDto> GetCameraCaptureByIdAsync(int id);
        Task<bool> DeleteCameraCaptureAsync(int id);
        Task<List<CameraCaptureListDto>> SearchCameraCaptureAsync(string keyword);
        Task<int> GetTotalCountCameraCapturesAsync();
        Task<List<CameraCaptureListDto>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
