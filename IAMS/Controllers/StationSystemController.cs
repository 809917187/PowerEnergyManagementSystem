using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class StationSystemController : Controller {
        private readonly IStationSystemService _stationSystemService;
        public StationSystemController(IStationSystemService stationSystemService) {
            _stationSystemService = stationSystemService;
        }
        public IActionResult Index(string sn="BBBB") {
            return View(_stationSystemService.GetEnergyStorageStackControlInfo(sn));
        }
    }
}
