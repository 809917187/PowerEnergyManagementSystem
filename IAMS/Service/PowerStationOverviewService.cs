using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.PowerStationOverview;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Org.BouncyCastle.Ocsp;

namespace IAMS.Service {
    public class PowerStationOverviewService : IPowerStationOverviewService {
        private IPowerStationService _powerStationService;
        private IStationSystemService _stationSystemService;
        private ITemplateService _templateService;
        public PowerStationOverviewService(IPowerStationService powerStationService, IStationSystemService stationSystemService, ITemplateService templateService) {
            _powerStationService = powerStationService;
            _stationSystemService = stationSystemService;
            _templateService = templateService;
        }

        public SeriesData GetEarnSummaryData(int powerStationId, DateTime startDate = default, DateTime endDate = default) {
            SeriesData seriesData = new SeriesData() {
                Name = "收益统计"
            };
            if (startDate == default || endDate == default) {
                startDate = DateTime.Now.AddDays(-7);
                endDate = DateTime.Now;
            }

            PowerStationInfo selectedPs = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0).First(s => s.Id == powerStationId);
            PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(selectedPs.Id);
            Dictionary<DateTime, decimal> date2earn = new Dictionary<DateTime, decimal>();
            foreach (EnergyStorageCabinetInfo cabinet in selectedPs.EnergyStorageCabinetRootDataList) {
                List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
                List<GatewayTableModelInfo> gatewayTableModelInfos = _stationSystemService.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), startDate, endDate)
                    .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                for (DateTime current = startDate; current < endDate; current = current.AddDays(1)) {
                    decimal earn = 0;
                    List<GatewayTableModelInfo> currentGatewayTableModelInfo = gatewayTableModelInfos.FindAll(s => s.UploadTime.Date == current.Date);
                    if (currentGatewayTableModelInfo != null) {
                        earn =
                        (decimal)currentGatewayTableModelInfo.Sum(s => s.PeakForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[1] - templateInfo.TimeFrame2SalePrice[1]) +
                        (decimal)currentGatewayTableModelInfo.Sum(s => s.FlatForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[2] - templateInfo.TimeFrame2SalePrice[2]) +
                        (decimal)currentGatewayTableModelInfo.Sum(s => s.NormalForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[3] - templateInfo.TimeFrame2SalePrice[3]) +
                        (decimal)currentGatewayTableModelInfo.Sum(s => s.ValleyForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[4] - templateInfo.TimeFrame2SalePrice[4]);
                    }
                    if (date2earn.ContainsKey(current)) {
                        date2earn[current] += earn;
                    } else {
                        date2earn[current] = earn;
                    }
                }
            }

            seriesData.Data = date2earn.OrderBy(kv => kv.Key).Select(kv => new object[] { kv.Key, kv.Value }).ToList();
            return seriesData;
        }

        public PowerStationOverviewViewModel GetPowerStationOverviewViewModel(int id) {
            PowerStationOverviewViewModel ret = new PowerStationOverviewViewModel();
            ret.PowerStationInfos = _powerStationService.GetAllPowerStationInfos().FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            if (id == 0) {
                ret.PowerStationInfos.First().IsSelected = true;
            } else {
                ret.PowerStationInfos.First(s => s.Id == id).IsSelected = true;
            }

            PowerStationInfo selectedPs = ret.PowerStationInfos.FirstOrDefault(s => s.IsSelected);
            if (selectedPs == null) {
                return ret;
            }

            foreach (EnergyStorageCabinetInfo cabinet in selectedPs.EnergyStorageCabinetRootDataList) {
                List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.PCS, 1);
                List<PCSInfo> pcsInfos_today = _stationSystemService.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), DateTime.Today).GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                List<PCSInfo> pcsInfos_yesterday = _stationSystemService.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), DateTime.Today.AddDays(-1)).GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();

                ret.ChargeAmountToday += pcsInfos_today.Sum(s => s.DailyChargeAmount);
                ret.ChargeAmountRaise += pcsInfos_today.Sum(s => s.DailyChargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyChargeAmount);
                ret.DischargeAmountToday += pcsInfos_today.Sum(s => s.DailyDischargeAmount);
                ret.DischargeAmountRaise += pcsInfos_today.Sum(s => s.DailyDischargeAmount) - pcsInfos_yesterday.Sum(s => s.DailyDischargeAmount);

                pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(cabinet.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);


                //根据ps查到绑定的电价模板
                PriceTemplateInfo templateInfo = _templateService.GetTemplateByPowerStationId(selectedPs.Id);
                List<GatewayTableModelInfo> gatewayTableModelInfos = _stationSystemService.GetGatewayTableModelInfo(pcsRootInfos.Select(S => S.name).ToList(), DateTime.Today)
                    .GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningToday +=
                    (decimal)gatewayTableModelInfos.Sum(s => s.PeakForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[1] - templateInfo.TimeFrame2SalePrice[1]) +
                    (decimal)gatewayTableModelInfos.Sum(s => s.FlatForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[2] - templateInfo.TimeFrame2SalePrice[2]) +
                    (decimal)gatewayTableModelInfos.Sum(s => s.NormalForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[3] - templateInfo.TimeFrame2SalePrice[3]) +
                    (decimal)gatewayTableModelInfos.Sum(s => s.ValleyForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[4] - templateInfo.TimeFrame2SalePrice[4]);
                List<GatewayTableModelInfo> gatewayTableModelInfos_yesterday = _stationSystemService.GetGatewayTableModelInfo(pcsRootInfos.Select(S => S.name).ToList(), DateTime.Today.AddDays(-1))
                    .GroupBy(s => s.DevName).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningRaise +=
                    ret.EarningToday -
                    (decimal)gatewayTableModelInfos_yesterday.Sum(s => s.PeakForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[1] - templateInfo.TimeFrame2SalePrice[1]) +
                    (decimal)gatewayTableModelInfos_yesterday.Sum(s => s.FlatForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[2] - templateInfo.TimeFrame2SalePrice[2]) +
                    (decimal)gatewayTableModelInfos_yesterday.Sum(s => s.NormalForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[3] - templateInfo.TimeFrame2SalePrice[3]) +
                    (decimal)gatewayTableModelInfos_yesterday.Sum(s => s.ValleyForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[4] - templateInfo.TimeFrame2SalePrice[4]);

                //获取当月每天的所有的GatewayTableModelInfo
                List<GatewayTableModelInfo> gatewayTableModelInfo_month = _stationSystemService.GetGatewayTableModelInfo(pcsRootInfos.Select(S => S.name).ToList(), Utility.GetStartOfMonth(DateTime.Now), Utility.GetEndOfMonth(DateTime.Now))
                    .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.EarningThisMonth +=
                    (decimal)gatewayTableModelInfo_month.Sum(s => s.PeakForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[1] - templateInfo.TimeFrame2SalePrice[1]) +
                    (decimal)gatewayTableModelInfo_month.Sum(s => s.FlatForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[2] - templateInfo.TimeFrame2SalePrice[2]) +
                    (decimal)gatewayTableModelInfo_month.Sum(s => s.NormalForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[3] - templateInfo.TimeFrame2SalePrice[3]) +
                    (decimal)gatewayTableModelInfo_month.Sum(s => s.ValleyForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[4] - templateInfo.TimeFrame2SalePrice[4]);
                //获取历史的所有的GatewayTableModelInfo
                List<GatewayTableModelInfo> gatewayTableModelInfo_total = _stationSystemService.GetGatewayTableModelInfo(pcsRootInfos.Select(S => S.name).ToList(), DateTime.MinValue, DateTime.MaxValue)
                    .GroupBy(m => new { m.DevName, Date = m.UploadTime.Date }).Select(g => g.OrderByDescending(m => m.UploadTime).First()).ToList();
                ret.TotalEarning +=
                    (decimal)gatewayTableModelInfo_total.Sum(s => s.PeakForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[1] - templateInfo.TimeFrame2SalePrice[1]) +
                    (decimal)gatewayTableModelInfo_total.Sum(s => s.FlatForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[2] - templateInfo.TimeFrame2SalePrice[2]) +
                    (decimal)gatewayTableModelInfo_total.Sum(s => s.NormalForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[3] - templateInfo.TimeFrame2SalePrice[3]) +
                    (decimal)gatewayTableModelInfo_total.Sum(s => s.ValleyForwardActiveEnergy) * (templateInfo.TimeFrame2BuyPrice[4] - templateInfo.TimeFrame2SalePrice[4]);

            }


            return ret;
        }

    }
}
