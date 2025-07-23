using System.Diagnostics;
using BLL.Services.AbstractServices;
using BLL.Services.ConcreteServices;
using Microsoft.AspNetCore.Mvc;
using RaspberyProject.Models;

namespace RaspberyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQrRecordService _qrRecordService;
        private readonly ICameraCaptureService _cameraCaptureService;
        private readonly ISensorDataService _sensorDataService;

        public HomeController(ILogger<HomeController> logger, IQrRecordService qrRecordService, ICameraCaptureService cameraCaptureService, ISensorDataService sensorDataService)
        {
            _logger = logger;
            _qrRecordService = qrRecordService;
            _cameraCaptureService = cameraCaptureService;
            _sensorDataService = sensorDataService;
        }

        public async Task<IActionResult> Index()
        {
            
            ViewBag.TotalQr = await _qrRecordService.GetTotalCountAsync();
            ViewBag.TotalCamera = await _cameraCaptureService.GetTotalCountCameraCapturesAsync();
            ViewBag.TotalSensor = await _sensorDataService.GetTotalSensorDataCountAsync();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
