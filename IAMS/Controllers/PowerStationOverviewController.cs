using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationOverviewController : Controller {
        private readonly IPowerStationOverviewService _powerStationOverviewService;
        public PowerStationOverviewController(IPowerStationOverviewService powerStationOverviewService) {
            _powerStationOverviewService = powerStationOverviewService;
        }
        public IActionResult Index(int PowerstationId) {
            return View(_powerStationOverviewService.GetPowerStationOverviewViewModel(PowerstationId));
        }

        [HttpGet]
        public IActionResult GetEarnChartData(int powerStationId, string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_powerStationOverviewService.GetEarnSummaryData(powerStationId, start, end));
            } else {
				return Ok(_powerStationOverviewService.GetEarnSummaryData(powerStationId));
			}
            
        }
    }
}
