﻿using IAMS.Common;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.Service;
using IAMS.ViewModels.StationSystem;
using Microsoft.AspNetCore.Mvc;

namespace IAMS.Controllers {
    public class StationSystemController : Controller {
        private readonly IStationSystemService _stationSystemService;
        private readonly IPowerStationService _powerStationService;
        public StationSystemController(IStationSystemService stationSystemService, IPowerStationService powerStationService) {
            _stationSystemService = stationSystemService;
            _powerStationService = powerStationService;
        }
        public IActionResult Index(string energyStorageCabinetName) {//储能柜集

            //根据储能柜集的name把页面所有的数据都查出来
            var model = _stationSystemService.GetStationSystemIndexViewModel(energyStorageCabinetName, DateTime.Now);

            return View(model);
        }
        [HttpGet]
        public IActionResult GetRealTimeChartData(string energyStorageCabinetName) {
            return Ok(_stationSystemService.GetRealTimeTrendOfChart(energyStorageCabinetName, DateTime.Now));
        }

        [HttpGet]
        public IActionResult GetPowerTrendChartData(string energyStorageCabinetName) {
            return Ok(_stationSystemService.GetTotalActivePowerOfChart(energyStorageCabinetName, DateTime.Now));
            
        }
    }
}
