using IAMS.Models.PowerStation;

namespace IAMS.Service {
    public interface IPowerStationService {
        public List<PowerStationInfo> GetAllPowerStationInfos();
        public PowerStationInfo GetPowerStationInfoById(int id);
        public List<string> GetAllStationImages(int PowerStationId );
        public List<string> GetAllStationInstallImages(int PowerStationId);
        public bool UpdateStationInfo(PowerStationInfo powerStationInfo);
        public bool AddPowerSatationInfo(PowerStationInfo powerStationInfo);
        public bool DeletePowerSatationInfo(int PowerStationId);

    }
}
