using IAMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class MultiStationOverviewController : Controller {
        private readonly IMultiSatationOverviewService _multiSatationOverviewService;
        public MultiStationOverviewController(IMultiSatationOverviewService multiSatationOverviewService) {
            _multiSatationOverviewService = multiSatationOverviewService;
        }
        public IActionResult Index() {
            return View(_multiSatationOverviewService.GetMultiStationOverviewViewModel());
        }
    }
}
