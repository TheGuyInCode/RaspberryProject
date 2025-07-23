using AutoMapper;
using BLL.DTOs.QrRecordDtos;
using BLL.Services.AbstractServices;
using DAL.Entities;
using DAL.Repositories.AbstractRepositories;
using QRCoder;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZXing.QrCode.Internal;

namespace BLL.Services.ConcreteServices
{
    public class QrRecordService : IQrRecordService
    {
        private readonly IGenericRepository<QrRecord> _genericRepository;
        private readonly IMapper _mapper;

        public QrRecordService(IGenericRepository<QrRecord> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<QrRecordDto> AddAsync(QrRecordAddDto qrRecordAddDto)
        {

            //var added = _mapper.Map<QrRecord>(qrRecordAddDto);

            var addedQrRecord = await _genericRepository.AddAsync(_mapper.Map<QrRecord>(qrRecordAddDto)); //entitiy ceviriyor

            return _mapper.Map<QrRecordDto>(addedQrRecord); // entity dto çeviriyor          

        }

        public async Task<bool> DeleteAsync(int id)
        {

            var deletedQrRecord = await _genericRepository.GetByIdAsync(id);

            if (deletedQrRecord == null)
            {
                throw new KeyNotFoundException("Silinecek QrRecord bulunamadı!");
            }

            await _genericRepository.DeleteAsync(deletedQrRecord.Id);

            return true;

        }

        public async Task<string> GenerateQrCodeAsync(QrRecordDetailDto dto)
        {
            var generator = new QRCodeGenerator();

            var data = generator.CreateQrCode(dto.Content, QRCodeGenerator.ECCLevel.Q);

            var base64Qr = new Base64QRCode(data).GetGraphic(20);

            return await Task.FromResult($"data:image/png;base64,{base64Qr}");
        }

        public async Task<List<QrRecordListDto>> GetAllAsync()
        {

            return _mapper.Map<List<QrRecordListDto>>(await _genericRepository.GetAllAsync());            
            
        }

        public async Task<List<QrRecordListDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            

            var dateBetween = await _genericRepository.WhereAsync(x => x.ScannedAt >= start.Date && x.ScannedAt <= end.Date);

            return _mapper.Map<List<QrRecordListDto>>(dateBetween);
        }

        public async Task<QrRecordDetailDto> GetByIdAsync(int id)
        {
            var getByIdRecords = await _genericRepository.GetByIdAsync(id);

            return _mapper.Map<QrRecordDetailDto>(getByIdRecords);
        }

        public async Task<int> GetTotalCountAsync()
        {
            var allQr = await _genericRepository.GetAllAsync();

            var totalCountQr = allQr.Count();

            return totalCountQr;
        }

        public async Task<List<QrRecordListDto>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<QrRecordListDto>();
            
            var searchingData = await _genericRepository.WhereAsync(x=>x.DeviceName.Contains(keyword.ToLower()) || x.Content.Contains(keyword.ToLower()));

            return _mapper.Map<List<QrRecordListDto>>(searchingData);

        }

        public async Task<QrRecordDetailDto> UpdateAsync(int id, QrRecordUpdateDto qrRecordUpdateDto)
        {

            var updatedQrRecord = await _genericRepository.GetByIdAsync(id);

            if (updatedQrRecord == null)
            {
                throw new KeyNotFoundException($"QR record ID {id} bulunamadı!");
            }

            _mapper.Map(qrRecordUpdateDto,updatedQrRecord);

            await _genericRepository.UpdateAsync(updatedQrRecord.Id,updatedQrRecord);

            return _mapper.Map<QrRecordDetailDto>(updatedQrRecord);

        }

        public async Task<bool> ValidateQrContentAsync(string qrText)
        {

            if (string.IsNullOrWhiteSpace(qrText))            
                return false;            

            var getAllQrCodes = await _genericRepository.GetAllAsync();

            var contentValidate = getAllQrCodes.Where(x => x.Content.ToLower() == qrText.ToLower());

            return contentValidate.Any();

        }

    }
}
