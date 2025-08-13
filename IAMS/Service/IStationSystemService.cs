using IAMS.Common;
using IAMS.Models.PowerStation;
using IAMS.ViewModels.StationSystem;

namespace IAMS.Service {
    public interface IStationSystemService {
        /*
         根据sn码查询站点对应的数据
         */
        /*public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn);*/

        public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetName, DateTime today);

        public TotalActivePowerOfChart GetTotalActivePowerOfChart(string EnergyStorageCabinetArrayName, DateTime today);
        /*public PowerStationRootInfo? GetPowerStationRootInfoByName(string EnergyStorageCabinetArrayName);*/
        public List<SeriesData> GetRealTimeTrendOfChart(string EnergyStorageCabinetArraySn, DateTime today);
    }
}
