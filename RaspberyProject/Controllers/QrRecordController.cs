using BLL.DTOs.QrRecordDtos;
using BLL.Services.AbstractServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace RaspberyProject.Controllers
{
    public class QrRecordController : Controller
    {
        private readonly IQrRecordService _qrRecordService;
        private readonly ILogger<QrRecordController> _logger;

        public QrRecordController(IQrRecordService qrRecordService,ILogger<QrRecordController> logger)
        {
            _qrRecordService = qrRecordService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            var getAllQrRecords = await _qrRecordService.GetAllAsync();
            

            ViewBag.TotalCount = await _qrRecordService.GetTotalCountAsync();


            return View(getAllQrRecords);
        }

        
        public async Task<ActionResult> Details(int id)
        {
            var qrRecord = await _qrRecordService.GetByIdAsync(id);

            if(qrRecord == null)
                return NotFound("Böyle id'ye ait bir QR kaydı bulunamadı!");
            
            return View(qrRecord);

        }        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QrRecordAddDto qrRecordAddDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(qrRecordAddDto);

                bool validateQrRecords = await _qrRecordService.ValidateQrContentAsync(qrRecordAddDto.Content);

                if (validateQrRecords)
                {
                    ModelState.AddModelError("Content", "Bu QR içeriği zaten sistemde kayıtlıdır!");
                    return View(qrRecordAddDto);
                }

                await _qrRecordService.AddAsync(qrRecordAddDto);

                return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"Bir hata oluştu ; {ex.Message}");
                return View();
            }
        }

        
        public async Task<ActionResult> Edit(int id)
        {
            var getQrRecord = await _qrRecordService.GetByIdAsync(id);

            if(getQrRecord == null)
            {
                return NotFound("Böyle id ile ilişkili QR kaydı bulunamadı!");
            }
            var updatedQrRecord = new QrRecordUpdateDto
            {
                DeviceName = getQrRecord.DeviceName,
                Content = getQrRecord.Content,
                ScannedAt = getQrRecord.ScannedAt
            };

            return View(updatedQrRecord);
        }
        
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirmed(int id, QrRecordUpdateDto qrRecordUpdateDto)
        {

            try
            {

                var editedQrRecord  = await _qrRecordService.GetByIdAsync(id);

                if (editedQrRecord == null)
                   return NotFound("Böyle bir QR kaydı bulunamadı!");                
                    
                await _qrRecordService.UpdateAsync(editedQrRecord.Id,qrRecordUpdateDto);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"Bir hata oluştu : {ex.Message}");
                return View(qrRecordUpdateDto);
            }
        }        
        public async Task<ActionResult> Delete(int id)
        {
            

            try
            {
                var getDeletedItemById = await _qrRecordService.GetByIdAsync(id);

                if (getDeletedItemById == null)
                    return NotFound("Böyle bir id'ye ait QR kaydı bulunamadı!");

                return View(getDeletedItemById);

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu : {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var getDeletedQrRecById = await _qrRecordService.GetByIdAsync(id);

                if (getDeletedQrRecById == null)
                    return NotFound("Böyle bir id'ye ait QR kaydı bulunamadı!");

                await _qrRecordService.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {

                ModelState.AddModelError(string.Empty,$"Bir hata oluştu : {ex.Message}");

                return RedirectToAction(nameof(Index));
            }            
        }
        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {

            var results = await _qrRecordService.SearchAsync(keyword);

            if (results == null || !results.Any())
            {
                ViewBag.Message = "Aranan kriterlere uygun QR kaydı bulunamadı.";
                return View("Index", new List<QrRecordListDto>());
            }

            return View("Index", results);

        }
        [HttpGet]
        public async Task<IActionResult> QrImage(int id)
        {
            var record = await _qrRecordService.GetByIdAsync(id);

            if (record == null)
                return NotFound();

            var base64Image = await _qrRecordService.GenerateQrCodeAsync(record);

            var imageData = base64Image.Split(',')[1];

            var imageBytes = Convert.FromBase64String(imageData);

            return File(imageBytes, "image/png");

        }
        [HttpPost]
        public async Task<IActionResult> GetQrRecordsByDateRange(DateTime start,DateTime end)
        {

            if (start.Date > end.Date)
            {
                ModelState.AddModelError(string.Empty, "Başlangıç tarihi bitiş tarihinden büyük olamaz!");
                return View("Index",new List<QrRecordListDto>());
            }

            var getQrRecordsByDate = await _qrRecordService.GetByDateRangeAsync(start,end);

            if (getQrRecordsByDate == null || !getQrRecordsByDate.Any())
            {
                ViewBag.Message = "Bu tarihler arasında kayıt bulunmadı.";
                return View("Index",new List<QrRecordListDto>());
            }                

            return View("Index",getQrRecordsByDate);

        }

    }
}
