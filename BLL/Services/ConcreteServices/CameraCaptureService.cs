using AutoMapper;
using BLL.DTOs.CameraCaptureDtos;
using BLL.Services.AbstractServices;
using DAL.Entities;
using DAL.Repositories.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ConcreteServices
{
    public class CameraCaptureService : ICameraCaptureService
    {
        private readonly IGenericRepository<CameraCapture> _genericRepository;
        private readonly IMapper _mapper;

        public CameraCaptureService(IGenericRepository<CameraCapture> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public async Task<CameraCaptureDto> AddAsync(CameraCaptureAddDto cameraCaptureAddDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cameraCaptureAddDto.Base64ImageData))
                    throw new Exception("Fotoğraf verisi boştur!");

                var entity = _mapper.Map<CameraCapture>(cameraCaptureAddDto);

                entity.ImageData = Convert.FromBase64String(cameraCaptureAddDto.Base64ImageData);

                var added = await _genericRepository.AddAsync(entity);

                return _mapper.Map<CameraCaptureDto>(added);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kayıt sırasında hata oluştu: {ex.Message}");
            }
        }
        public async Task<bool> DeleteCameraCaptureAsync(int id)
        {

            var deletedCameraCapture = await _genericRepository.GetByIdAsync(id);

            if (deletedCameraCapture == null)
            {
                throw new KeyNotFoundException("Silinecek bir kayıt bulunamadı!");
                            
            }

            await _genericRepository.DeleteAsync(deletedCameraCapture.Id);

            return true;

        }

        public async Task<List<CameraCaptureListDto>> GetAllAsync()
        {
            return _mapper.Map<List<CameraCaptureListDto>>(await _genericRepository.GetAllAsync());
        }

        public async Task<List<CameraCaptureListDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var startDate = start.Date.AddDays(-1);
            var endDate = end.Date.AddDays(1);

            var getDateCameraCaptures = await _genericRepository.WhereAsync(x=>x.CapturedAt >= start.Date && x.CapturedAt <= end.Date);

            return _mapper.Map<List<CameraCaptureListDto>>(getDateCameraCaptures);
        }

        public async Task<CameraCaptureDetailDto> GetCameraCaptureByIdAsync(int id)
        {
            return _mapper.Map<CameraCaptureDetailDto>(await _genericRepository.GetByIdAsync(id));
        }

        public async Task<int> GetTotalCountCameraCapturesAsync()
        {
            var totalCountCameraCaptures =await _genericRepository.GetAllAsync();

            var count = totalCountCameraCaptures.Count();

            return count;
        }

        public async Task<List<CameraCaptureListDto>> SearchCameraCaptureAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<CameraCaptureListDto>();

            return _mapper.Map<List<CameraCaptureListDto>>(await _genericRepository.WhereAsync(x=>x.Description != null && x.Description.ToLower().Contains(keyword.ToLower()) ||
                                                                                               x.FileName != null && x.FileName.ToLower().Contains(keyword.ToLower())));        
        }
    }
}
