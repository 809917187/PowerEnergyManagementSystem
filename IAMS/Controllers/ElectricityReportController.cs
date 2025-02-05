using IAMS.Common;
using IAMS.Service;
using IAMS.ViewModels.ElectricityReport;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class ElectricityReportController : Controller {
        private readonly IElectricityReportService _electricityReportService;
        public ElectricityReportController(IElectricityReportService electricityReportService) {
            _electricityReportService = electricityReportService;
        }

        public IActionResult Index() {
            return View(_electricityReportService.GetElectricityReportViewModel());
        }

        [HttpGet]
        public IActionResult LoadStationSummaryTableData(DateTime startDate, DateTime endDate) {
            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue) {
                startDate = Utility.GetMinDate();
                endDate = Utility.GetMaxDate();
            }
            var ret = _electricityReportService.GetElectricityReportStationSummaryData(startDate, endDate);

            return Ok(ret);
        }


        [HttpGet]
        public IActionResult LoadSingleStationReportByDay(int powerStationId) {

            var ret = _electricityReportService.GetSingleStationReportByDayData(powerStationId, DateTime.Now.AddDays(-31), DateTime.Now);

            return Ok(ret);
        }
    }
}
