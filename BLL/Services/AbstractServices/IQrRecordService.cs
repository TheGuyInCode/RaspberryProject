using BLL.DTOs.QrRecordDtos;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices
{
    public interface IQrRecordService
    {
        Task<QrRecordDto> AddAsync(QrRecordAddDto qrRecordAddDto);
        Task<bool> DeleteAsync(int id);
        Task<QrRecordDetailDto> UpdateAsync(int id, QrRecordUpdateDto qrRecordUpdateDto);
        Task<QrRecordDetailDto> GetByIdAsync(int id);
        Task<List<QrRecordListDto>> GetAllAsync();        
        Task<List<QrRecordListDto>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<List<QrRecordListDto>> SearchAsync(string keyword);
        Task<string> GenerateQrCodeAsync(QrRecordDetailDto dto);
        Task<bool> ValidateQrContentAsync(string qrText);
        Task<int> GetTotalCountAsync();

    }
}
