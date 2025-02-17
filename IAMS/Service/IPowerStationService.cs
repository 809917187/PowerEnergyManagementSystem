using IAMS.Models.PowerStation;
using IAMS.MQTT.Model;
using IAMS.ViewModels.StationSystem;

namespace IAMS.Service {
    public interface IPowerStationService {
        public List<PowerStationInfo> GetAllPowerStationInfos();
        public PowerStationInfo GetPowerStationInfoById(int id);
        public List<string> GetAllStationImages(int PowerStationId);
        public List<string> GetAllStationInstallImages(int PowerStationId);
        public bool UpdateStationInfo(PowerStationInfo powerStationInfo);
        public bool AddPowerSatationInfo(PowerStationInfo powerStationInfo);
        public bool DeletePowerSatationInfo(int PowerStationId);
        public List<PowerStationInfo> GetAllPowerStationInfoByCabinetName(string cabinetName);//包含被选中信息
        public List<EnergyStorageCabinetInfo> GetAllEnergyStorageCabinetArray();

        public RootDataFromMqtt GetDataSourceCabinet(List<PowerStationInfo> powerStationInfos);
        public bool BindCabinetToPowerStation(int PowerStationId, List<int> CabinetIds);
        public bool BindPowerStationToUser(int PowerStationId, List<int> UserIds);
        public List<int> GetBindUserListByPowerStationId(int PowerStationId);
        public List<int> GetBindPowerStationListByUserId(int UserId);



    }
}
