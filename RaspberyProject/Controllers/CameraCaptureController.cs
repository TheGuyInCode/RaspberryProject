using BLL.DTOs.CameraCaptureDtos;
using BLL.Services.AbstractServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RaspberyProject.Controllers
{
    public class CameraCaptureController : Controller
    {
        private readonly ICameraCaptureService _cameraCaptureService;
        private readonly ILogger<CameraCaptureController> _logger;

        public CameraCaptureController(ICameraCaptureService cameraCaptureService, ILogger<CameraCaptureController> logger)
        {
            _cameraCaptureService = cameraCaptureService;
            _logger = logger;
        }
        // GET: CameraCapture
        public async Task<ActionResult> Index()
        {

            var getAllCameraCaptures = await _cameraCaptureService.GetAllAsync();

            ViewBag.TotalCount = await _cameraCaptureService.GetTotalCountCameraCapturesAsync();

            return View(getAllCameraCaptures);

        }

        // GET: CameraCapture/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var getCameraCaptureById = await _cameraCaptureService.GetCameraCaptureByIdAsync(id);

            if (getCameraCaptureById == null)
            {
                TempData["Message"] = "Kamera kaydı bulunamadı.";
                RedirectToAction("Index");
            }

            return View(getCameraCaptureById);

        }

        // GET: CameraCapture/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CameraCapture/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CameraCaptureAddDto cameraCaptureAddDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(cameraCaptureAddDto);            

                var createdCameraCapture = await _cameraCaptureService.AddAsync(cameraCaptureAddDto);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"Bir hata oluştu : {ex.Message}");
                return View(cameraCaptureAddDto);
            }
        }
        // GET: CameraCapture/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var getCameraCaptureById = await _cameraCaptureService.GetCameraCaptureByIdAsync(id);

            if (getCameraCaptureById == null)
                return NotFound("Böyle bir kayıt bulunamadı!");

            return View(getCameraCaptureById);
        }

        // POST: CameraCapture/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var getCameraCaptureById = await _cameraCaptureService.GetCameraCaptureByIdAsync(id);
                
                if (getCameraCaptureById == null)
                    return NotFound("Böyle bir kayıt bulunamadı!");

                await _cameraCaptureService.DeleteCameraCaptureAsync(id);


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Silme sırasında hata oluştu.");
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCameraCapturesByDate(DateTime start, DateTime end)
        {
            if(start.Date > end.Date)
            {
                ModelState.AddModelError(string.Empty, "Başlangıç tarihi bitiş tarihinden büyük olamaz!");
                return View("Index",new List<CameraCaptureListDto>());
            }

            var getRelatedCaptures = await _cameraCaptureService.GetByDateRangeAsync(start,end);

            if (getRelatedCaptures == null || !getRelatedCaptures.Any())
            {
                ViewBag.Message = "Bu tarihler arasında hiçbir kamera kaydı yoktur!";
                return View("Index", new List<CameraCaptureListDto>());               
            }

            return View("Index", getRelatedCaptures);

        }

        [HttpGet]
        public async Task<IActionResult> SearchCamCaptures(string keyword)
        {
            var searchedItems = await _cameraCaptureService.SearchCameraCaptureAsync(keyword);

            if (searchedItems == null || !searchedItems.Any())
            {
                ViewBag.Message = "Kriterlere uygun arama bulunamadı!";
                return View("Index",new List<CameraCaptureListDto>());
            }

            return View("Index",searchedItems);

        }
    }
}
