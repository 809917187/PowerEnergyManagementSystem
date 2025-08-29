using IAMS.Models.DeviceInfo;
using IAMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace IAMS.Controllers {
	public class DeviceMonitorController : Controller {
		private readonly IDeviceMonitorService _deviceMonitorService;
		private readonly IPowerStationService _powerStationService;
		private IClickHouseService _clickHouseService;
		public DeviceMonitorController(IDeviceMonitorService deviceMonitorService, IPowerStationService powerStationService) {
			_deviceMonitorService = deviceMonitorService;
			_powerStationService = powerStationService;

		}
		public IActionResult Index(string energyStorageCabinetSn) {
			if (string.IsNullOrWhiteSpace(energyStorageCabinetSn)) {
				energyStorageCabinetSn = _powerStationService.GetDefaultSelectedEmsSn();
			}
			return View(_deviceMonitorService.GetDeviceMonitorViewModel(energyStorageCabinetSn));
		}

		public static byte[] GenerateCsv<T>(List<T> data) {
			var sb = new StringBuilder();

			// 获取属性
			var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// 表头：Display(Name) 优先，其次属性名
			var header = properties.Select(p => {
				var displayAttr = p.GetCustomAttribute<DisplayAttribute>();
				return displayAttr != null ? displayAttr.Name : p.Name;
			});

			sb.AppendLine(string.Join(",", header));

			// 内容
			foreach (var item in data) {
				var values = properties.Select(p => {
					var value = p.GetValue(item);
					return value != null ? value.ToString() : "";
				});
				sb.AppendLine(string.Join(",", values));
			}

			// 返回 UTF-8 带 BOM，避免中文乱码
			return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
		}

		[HttpGet]
		public IActionResult DownloadHistoryData(string energyStorageCabinetSn) {
			if (string.IsNullOrWhiteSpace(energyStorageCabinetSn)) {
				energyStorageCabinetSn = _powerStationService.GetDefaultSelectedEmsSn();
			}

			var model = _deviceMonitorService.GetDeviceMonitorHistory(energyStorageCabinetSn);

			using var zipStream = new MemoryStream();
			using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true)) {
				// BCU
				var bcuCsv = archive.CreateEntry("BCU.csv");
				using (var stream = bcuCsv.Open()) {
					var bytes = GenerateCsv(model.BcuInfos);
					stream.Write(bytes, 0, bytes.Length);
				}

				// PCC
				var pccCsv = archive.CreateEntry("PCC.csv");
				using (var stream = pccCsv.Open()) {
					var bytes = GenerateCsv(model.GatewayTableModelInfos);
					stream.Write(bytes, 0, bytes.Length);
				}

				// PCS
				var pcsCsv = archive.CreateEntry("PCS.csv");
				using (var stream = pcsCsv.Open()) {
					var bytes = GenerateCsv(model.PcsInfos);
					stream.Write(bytes, 0, bytes.Length);
				}
			}

			zipStream.Seek(0, SeekOrigin.Begin);
			return File(zipStream.ToArray(), "application/zip", $"历史数据_{DateTime.Now:yyyyMMddHHmmss}.zip");

		}
	}
}
