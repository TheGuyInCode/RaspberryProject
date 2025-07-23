using BLL.DTOs.SensorDataDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices
{
    public interface ISensorDataService
    {
        Task<SensorDataDto> SensorDataAddAsync(SensorDataAddDto sensorDataAddDto);
        Task<List<SensorDataListDto>> GetAllSensorData();
        Task<SensorDataDetailDto> GetSensorDataByIdAsync(int id);
        Task<List<SensorDataListDto>> GetSensorDataByDateRangeAsync(DateTime start,DateTime end);
        Task<int> GetTotalSensorDataCountAsync();
        Task<List<SensorDataListDto>> SearchSensorDataAsync(string keyword);
        Task<bool> DeleteSensorDataAsync(int id);
    }
}
