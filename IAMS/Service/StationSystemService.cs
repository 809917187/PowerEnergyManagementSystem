using Dapper;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.MQTT.Model;
using IAMS.MQTT;
using IAMS.ViewModels.StationSystem;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;
using System;
using IAMS.Common;
using IAMS.Models.DeviceInfo;

namespace IAMS.Service {
    public class StationSystemService : IStationSystemService {
        private string _connectionString;
        private IPowerStationService _powerStationService;
        private IClickHouseService _clickHouseService;
        public StationSystemService(IConfiguration configuration, IPowerStationService powerStationService, IClickHouseService clickHouseService) {
            _connectionString = configuration.GetConnectionString("ems");
            _powerStationService = powerStationService;
            _clickHouseService = clickHouseService;
        }

        



        public List<SeriesData> GetRealTimeTrendOfChart(string EnergyStorageCabinetArraySn, DateTime today) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            List<DeviceBaseInfo> deviceBaseInfos = _powerStationService.GetDeviceBaseInfoByEmsSn(EnergyStorageCabinetArraySn);
            var gatewayTableTotalActivePower = _clickHouseService.GetPccModel001s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), today).GroupBy(s => s.Sn).Select(
                g => new SeriesData() {
                    Name = g.Key + "有功功率",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.TotalActivePower }).ToList()
                }).ToList();//关口表总有功功率

            var pcsTotalActivePower = _clickHouseService.GetPcsModel005s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS).Select(s => s.Sn).ToList(), today).GroupBy(s => s.Sn).Select(
                g => new SeriesData() {
                    Name = g.Key + "有功功率",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.TotalActivePower }).ToList()
                }).ToList();//PCS

            var energyStorageStackControlGroupedByName = _clickHouseService.GetBsuModel003s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.BSU).Select(s => s.Sn).ToList(), today).GroupBy(s => s.Sn);
            var energyStorageStackControlSoc = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "SOC",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.SOC }).ToList()
                }).ToList();//堆控SOC
            var energyStorageStackControlHighestCellVoltage = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "单体最高电压",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.HighestSingleCellVoltage }).ToList()
                }).ToList();//堆控单体最高电压
            var energyStorageStackControlLowestCellVoltage = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "单体最低电压",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.LowestSingleCellVoltage }).ToList()
                }).ToList();//堆控单体最低电压

            seriesDatas = seriesDatas.Concat(gatewayTableTotalActivePower).Concat(pcsTotalActivePower).Concat(energyStorageStackControlSoc).Concat(energyStorageStackControlHighestCellVoltage).Concat(energyStorageStackControlLowestCellVoltage).ToList();

            return seriesDatas;
        }
        public TotalActivePowerOfChart GetTotalActivePowerOfChart(string EnergyStorageCabinetArraySn, DateTime today) {
            TotalActivePowerOfChart model = new TotalActivePowerOfChart();
            var deviceBaseInfos = _powerStationService.GetDeviceBaseInfoByEmsSn(EnergyStorageCabinetArraySn);
            //电网有功功率就是关口表上的和
            model.ChartDataOfElectricGrid = _clickHouseService.GetPccModel001s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), today).GroupBy(m => m.UploadTime).Select(g => new object[]{
                g.Key.ToString("yyyy-MM-ddTHH:mm:ss"),
                g.Sum(m => m.TotalActivePower)
            }).OrderBy(s => s[0]).ToList();
            //储能功率从PCS总有功取
            model.ChartDataOfEnergyStorage = _clickHouseService.GetPcsModel005s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS).Select(s => s.Sn).ToList(), today).GroupBy(m => m.UploadTime).Select(g => new object[]{
                g.Key.ToString("yyyy-MM-ddTHH:mm:ss"),
                g.Sum(m => m.TotalActivePower)
            }).OrderBy(s => s[0]).ToList();

            return model;
        }



        public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetSn, DateTime dateTime) {
            StationSystemIndexViewModel model = new StationSystemIndexViewModel();
            try {
                model.PowerStationInfos = _powerStationService.GetAllPowerStationInfos(0, energyStorageCabinetSn);

                var deviceBaseInfos = _powerStationService.GetDeviceBaseInfoByEmsSn(energyStorageCabinetSn);

                //PCS devType=5
                var pcsInfos = _clickHouseService.GetPcsModel005s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCS).Select(s => s.Sn).ToList(), dateTime);
                //储能堆控
                var energyStorageStackControlInfos = _clickHouseService.GetBsuModel003s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.BSU).Select(s => s.Sn).ToList(), dateTime);
                //关口表
                var gatewayTableModelInfos = _clickHouseService.GetPccModel001s(deviceBaseInfos.Where(s => s.DeviceType == (int)DeviceCode.PCC).Select(s => s.Sn).ToList(), dateTime);


                model.DailyChargeAmount = pcsInfos.Sum(s => s.DailyChargeAmount).ToString("F2");
                model.DailyDischargeAmount = pcsInfos.Sum(s => s.DailyDischargeAmount).ToString("F2");
                model.ACCumulativeChargeAmount = pcsInfos.Sum(s => s.ACAccumulatedChargeAmount).ToString("F2");
                model.ACCumulativeDischargeAmount = pcsInfos.Sum(s => s.ACAccumulatedDischargeAmount).ToString("F2");
                model.SOC = energyStorageStackControlInfos.Average(s => s.SOC).ToString("F2");
                model.PowerGrid = gatewayTableModelInfos.Select(s => s.TotalActivePower).Sum().ToString("F2");
                model.EnergyStorage = pcsInfos.Select(s => s.TotalActivePower).Sum().ToString("F2");
                model.Load = (Convert.ToDouble(model.PowerGrid) - Convert.ToDouble(model.EnergyStorage)).ToString("F2");

                foreach (var device in pcsInfos) {
                    model.deviceStatus.Add((device.Sn, device.OnlineStatus ? "在线" : "离线"));
                }
                foreach (var device in energyStorageStackControlInfos) {
                    model.deviceStatus.Add((device.Sn, device.OnlineStatus ? "在线" : "离线"));
                }
                foreach (var device in gatewayTableModelInfos) {
                    model.deviceStatus.Add((device.Sn, device.OnlineStatus ? "在线" : "离线"));
                }
            }catch (Exception ex) {


            }
            

            return model;
        }


        


    }
}
