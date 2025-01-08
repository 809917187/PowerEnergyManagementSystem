using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;
using IAMS.ViewModels.StationSystem;

namespace IAMS.Service {
    public interface IStationSystemService {
        /*
         根据sn码查询站点对应的数据
         */
        public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn);
        public bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos);

        public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetName, DateTime today);

        public List<PCSInfo> GetPCSInfo(List<string> pcsName, DateTime today);

    }
}
