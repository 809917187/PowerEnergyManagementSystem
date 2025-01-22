using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;
using IAMS.ViewModels.StationSystem;

namespace IAMS.Service {
    public interface IStationSystemService {
        /*
         根据sn码查询站点对应的数据
         */
        /*public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn);*/
        public bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos);

        public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetName, DateTime today);

        public List<PCSInfo> GetPCSInfo(List<string> devNames, DateTime startDate, DateTime endDate = default);
        public List<EnergyStorageStackControlInfo> GetEnergyStorageStackControllerInfo(List<string> devNames, DateTime startDate, DateTime endDate = default);
        public List<GatewayTableModelInfo> GetGatewayTableModelInfo(List<string> devNames, DateTime startDate, DateTime endDate = default);
        public TotalActivePowerOfChart GetTotalActivePowerOfChart(string EnergyStorageCabinetArrayName, DateTime today);
        /*public PowerStationRootInfo? GetPowerStationRootInfoByName(string EnergyStorageCabinetArrayName);*/
        public List<SeriesData> GetRealTimeTrendOfChart(string EnergyStorageCabinetArrayName, DateTime today);
    }
}
