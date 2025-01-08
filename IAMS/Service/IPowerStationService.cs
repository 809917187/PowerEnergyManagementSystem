using IAMS.Models.PowerStation;
using IAMS.MQTT.Model;

namespace IAMS.Service {
    public interface IPowerStationService {
        public List<PowerStationInfo> GetAllPowerStationInfos();
        public PowerStationInfo GetPowerStationInfoById(int id);
        public List<string> GetAllStationImages(int PowerStationId );
        public List<string> GetAllStationInstallImages(int PowerStationId);
        public bool UpdateStationInfo(PowerStationInfo powerStationInfo);
        public bool AddPowerSatationInfo(PowerStationInfo powerStationInfo);
        public bool DeletePowerSatationInfo(int PowerStationId);
        public List<PowerStationRootInfo> GetEnergyStorageCabinetArrayById(int PowerStationId);
        public List<PowerStationRootInfo> GetAllEnergyStorageCabinetArray();
    }
}
