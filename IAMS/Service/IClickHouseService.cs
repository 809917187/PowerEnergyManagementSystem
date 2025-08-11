using IAMS.Models;
using IAMS.Models.DeviceInfo;

namespace IAMS.Service {
    public interface IClickHouseService {
        public List<OrignialClickHouseData> GetOrignialClickHouseDatasBySn(int devType, List<string> snList, DateTime startDateTime, DateTime endDateTime = default);

        public List<T> PraseDeviceInfo<T>(List<OrignialClickHouseData> orignialBatteryClusterDatas) where T : new();

        public List<PccModel001> GetPccModel001s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default);
        public List<PcsModel005> GetPcsModel005s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default);
        public List<BsuModel003> GetBsuModel003s(List<string> snList, DateTime startDateTime, DateTime endDateTime = default);
    }
}
