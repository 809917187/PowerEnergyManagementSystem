using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class PowerStationOverviewController : Controller {
        private readonly IPowerStationOverviewService _powerStationOverviewService;
        public PowerStationOverviewController(IPowerStationOverviewService powerStationOverviewService) {
            _powerStationOverviewService = powerStationOverviewService;
        }
        public IActionResult Index(int PowerstationId) {
            var ret = _powerStationOverviewService.GetPowerStationOverviewViewModel(PowerstationId);
            return View(ret);
        }

        [HttpGet]
        public IActionResult GetEarnChartData(int powerStationId, string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_powerStationOverviewService.GetEarnSummaryData(powerStationId, start, end));
            } else {
				return Ok(_powerStationOverviewService.GetEarnSummaryData(powerStationId));
			}
            
        }

        [HttpGet]
        public IActionResult GetChargeAdnDischargeChartData(int powerStationId, string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_powerStationOverviewService.GetChargeAdnDischargeChartData(powerStationId, start, end));
            } else {
                return Ok(_powerStationOverviewService.GetChargeAdnDischargeChartData(powerStationId));
            }

        }
    }
}
