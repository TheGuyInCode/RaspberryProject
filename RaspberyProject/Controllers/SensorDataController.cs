using BLL.DTOs.CameraCaptureDtos;
using BLL.DTOs.SensorDataDtos;
using BLL.Services.AbstractServices;
using BLL.Services.ConcreteServices;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RaspberyProject.Controllers
{
    public class SensorDataController : Controller
    {
        private readonly ISensorDataService _sensorDataService;
        private readonly ILogger<SensorDataController> _logger;

        public SensorDataController(ISensorDataService sensorDataService, ILogger<SensorDataController> logger)
        {
            _sensorDataService = sensorDataService;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var getAllSensorData = await _sensorDataService.GetAllSensorData();

            ViewBag.TotalCount = await _sensorDataService.GetTotalSensorDataCountAsync();

            return View(getAllSensorData);
           
        }

        // GET: SensorDataController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var getSensorDataById = await _sensorDataService.GetSensorDataByIdAsync(id);

            if (getSensorDataById == null)
            {
                ViewBag.Message = "Sensör kaydı yoktur!";
                return RedirectToAction("Index");
            }
                

            return View(getSensorDataById);
        }

        // GET: SensorDataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SensorDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SensorDataAddDto sensorDataAddDto)
        {
            try
            {

                if (!ModelState.IsValid)
                    return View(sensorDataAddDto);

                var addedSensorData = await _sensorDataService.SensorDataAddAsync(sensorDataAddDto);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"Bir hata oluştu : {ex.Message}");
                return View(sensorDataAddDto);
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            var getSensorById = await _sensorDataService.GetSensorDataByIdAsync(id);

            if (getSensorById == null)
                return NotFound("Sensör verisine ait id bulunamadı!");


            return View(getSensorById);
        }

        // POST: SensorDataController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deletedSensorData = await _sensorDataService.GetSensorDataByIdAsync(id);

                if (deletedSensorData == null)
                    return NotFound("Sensör verisine ait id bulunamadı!");

                await _sensorDataService.DeleteSensorDataAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"{ex.Message}");
                return View(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSensorDataByDate(DateTime start, DateTime end)
        {
            if (start.Date > end.Date)
            {
                ModelState.AddModelError(string.Empty, "Başlangıç tarihi bitiş tarihinden büyük olamaz!");
                return View("Index", new List<SensorDataListDto>());
            }

            var getRelatedSensorData = await _sensorDataService.GetSensorDataByDateRangeAsync(start, end);

            if (getRelatedSensorData == null || !getRelatedSensorData.Any())
            {
                ViewBag.Message = "Bu tarihler arasında hiçbir sensör kaydı yoktur!";
                return View("Index", new List<SensorDataListDto>());
            }

            return View("Index", getRelatedSensorData);

        }

        [HttpGet]
        public async Task<IActionResult> SearchSensorData(string keyword)
        {
            var searchedItems = await _sensorDataService.SearchSensorDataAsync(keyword);

            if (searchedItems == null || !searchedItems.Any())
            {
                ViewBag.Message = "Kriterlere uygun arama bulunamadı!";
                return View("Index", new List<SensorDataListDto>());
            }

            return View("Index", searchedItems);

        }
    }
}
