using IAMS.Models;
using IAMS.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IAMS.Controllers {
    [Authorize]
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IMultiSatationOverviewService _multiSatationOverviewService;
        public HomeController(ILogger<HomeController> logger, IMultiSatationOverviewService multiSatationOverviewService) {
            _logger = logger;
            _multiSatationOverviewService = multiSatationOverviewService;
        }

        public IActionResult Index() {
            return View(_multiSatationOverviewService.GetMultiStationOverviewViewModel());
        }


        [HttpGet]
        public IActionResult GetEarnChartData(string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_multiSatationOverviewService.GetEarnSummaryData(start, end));
            } else {
                return Ok(_multiSatationOverviewService.GetEarnSummaryData());
            }
        }

        [HttpGet]
        public IActionResult GetEarnRankChartData(string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_multiSatationOverviewService.GetEarnRankData(start, end));
            } else {
                return Ok(_multiSatationOverviewService.GetEarnRankData());
            }
        }
        [HttpGet]
        public IActionResult GetChargeAdnDischargeChartData(string startDate, string endDate) {
            if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end)) {
                return Ok(_multiSatationOverviewService.GetChargeAdnDischargeChartData(start, end));
            } else {
                return Ok(_multiSatationOverviewService.GetChargeAdnDischargeChartData());
            }

        }

        public IActionResult Privacy() {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}