using IAMS.Models.PowerStation;
using IAMS.Models.StationSystem;

namespace IAMS.Service {
    public interface IStationSystemService {
        /*
         根据sn码查询站点对应的数据
         */
        public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn);
        public bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos);

    }
}
