using AutoMapper;
using BLL.DTOs.SensorDataDtos;
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
    public class SensorDataService : ISensorDataService
    {
        private readonly IGenericRepository<SensorData> _genericRepository;
        private readonly IMapper _mapper;

        public SensorDataService(IGenericRepository<SensorData> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteSensorDataAsync(int id)
        {
            var deletedSensorData = await _genericRepository.GetByIdAsync(id);

            if (deletedSensorData == null)
                throw new Exception("Silinecek sensör verisine ait id bulunamadı!");

            await _genericRepository.DeleteAsync(id);

            return true;
        }

        public async Task<List<SensorDataListDto>> GetAllSensorData()
        {
            return _mapper.Map<List<SensorDataListDto>>(await _genericRepository.GetAllAsync());
        }

        public async Task<List<SensorDataListDto>> GetSensorDataByDateRangeAsync(DateTime start, DateTime end)
        {
            return _mapper.Map<List<SensorDataListDto>>(await _genericRepository.WhereAsync(x=>x.CreatedDate >= start.Date && x.CreatedDate<=end.Date));
        }

        public async Task<SensorDataDetailDto> GetSensorDataByIdAsync(int id)
        {
            return _mapper.Map<SensorDataDetailDto>(await _genericRepository.GetByIdAsync(id));
        }

        public async Task<int> GetTotalSensorDataCountAsync()
        {
            var getAllSensorData = await _genericRepository.GetAllAsync();

            int totalSensorDataCount = getAllSensorData.Count();

            return totalSensorDataCount;
        }

        public async Task<List<SensorDataListDto>> SearchSensorDataAsync(string keyword)
        {
            //return _mapper.Map<List<SensorDataListDto>>(await _genericRepository.WhereAsync(x => x.Temperature.ToString().Contains(keyword.ToLower()) ? true : x.Temperature==0 || x.Humidity.ToString().Contains(keyword.ToLower()) ? true : x.Humidity == 0)); 

            // Ufak bir bilgi birikimi denemesi!

            if (string.IsNullOrWhiteSpace(keyword))
                return new List<SensorDataListDto>();

            var lowerKeyword = keyword.ToLower();

            var result = await _genericRepository.WhereAsync(x =>
                x.Temperature.ToString().ToLower().Contains(lowerKeyword) ||
                x.Humidity.ToString().ToLower().Contains(lowerKeyword));

            return _mapper.Map<List<SensorDataListDto>>(result);

        }

        public async Task<SensorDataDto> SensorDataAddAsync(SensorDataAddDto sensorDataAddDto)
        {
            var addedSensorData =  await _genericRepository.AddAsync(_mapper.Map<SensorData>(sensorDataAddDto));


            return _mapper.Map<SensorDataDto>(addedSensorData);
        }
    }
}
