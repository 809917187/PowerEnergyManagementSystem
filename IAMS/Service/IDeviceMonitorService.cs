using IAMS.ViewModels.DeviceMonitor;

namespace IAMS.Service {
    public interface IDeviceMonitorService {
        public DeviceMonitorViewModel GetDeviceMonitorViewModel(string cabinetName);
        public DeviceMonitorViewModel GetDeviceMonitorHistory(string cabinetName);

    }
}
