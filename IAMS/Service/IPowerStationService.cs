using IAMS.Models.DeviceInfo;
using IAMS.Models.PowerStation;
using IAMS.MQTT.Model;
using IAMS.ViewModels.StationSystem;

namespace IAMS.Service {
    public interface IPowerStationService {
        public List<PowerStationInfo> GetAllPowerStationInfos(int ps_id, string emsSn);
        public PowerStationInfo GetPowerStationInfoById(int id);
        public List<string> GetAllStationImages(int PowerStationId);
        public List<string> GetAllStationInstallImages(int PowerStationId);
        public bool UpdateStationInfo(PowerStationInfo powerStationInfo);
        public bool AddPowerSatationInfo(PowerStationInfo powerStationInfo);
        public bool DeletePowerSatationInfo(int PowerStationId);
        /* public List<PowerStationInfo> GetAllPowerStationInfoByCabinetName(string cabinetName);//包含被选中信息*/
        public List<EnergyStorageCabinetInfo> GetAllEnergyStorageCabinetArray();
        public List<DeviceBaseInfo> GetDeviceBaseInfosByPowerStationId(List<int> psId);
        public List<DeviceBaseInfo> GetDeviceBaseInfoByEmsSn(string emsSn);
        public bool BindCabinetToPowerStation(int PowerStationId, List<string> CabinetIds);
        public bool BindPowerStationToUser(int PowerStationId, List<int> UserIds);
        public List<int> GetBindUserListByPowerStationId(int PowerStationId);
        public List<int> GetBindPowerStationListByUserId(int UserId);

        public string GetDefaultSelectedEmsSn();
        public int GetDefaultSelectedPowerStationId();

    }
}
