using Dapper;
using IAMS.Models.PowerStation;
using IAMS.Models.PriceTemplate;
using IAMS.Models.StationSystem;
using IAMS.MQTT.Model;
using IAMS.MQTT;
using IAMS.ViewModels.StationSystem;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;

namespace IAMS.Service {
	public class StationSystemService : IStationSystemService {
		private string _connectionString;
		private IPowerStationService _powerStationService;
		public StationSystemService(IConfiguration configuration, IPowerStationService powerStationService) {
			_connectionString = configuration.GetConnectionString("gq");
			_powerStationService = powerStationService;
		}
		public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn) {
			EnergyStorageStackControlInfo ret = new EnergyStorageStackControlInfo() {
				Sn = sn
			};

			try {
				// 创建 MySQL 连接
				using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
					connection.Open();

					// 定义 SQL 查询
					string query = "SELECT " +
						"id,name,device_type,device_id,sn,is_active,daily_charge_capacity,daily_discharge_capacity,total_charge_capacity," +
						"total_discharge_capacity,soc,max_charge_power,max_discharge_power,max_load_power,upload_time,power_station_id " +
						"FROM " +
						"device_energy_storage_stack_control_info " +
						"WHERE sn=@sn " +
						"order by upload_time desc " +
						"LIMIT 1 ";
					using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
						cmd.Parameters.AddWithValue("@sn", sn);
						var read = cmd.ExecuteReader();
						while (read.Read()) {
							ret.Id = read.GetInt32("id");
							ret.DevName = read.GetString("name");
							ret.DevId = read.GetString("device_id");
							ret.DeviceOnline = read.GetBoolean("is_active");
							ret.DailyChargeCapacity = read.GetDouble("daily_charge_capacity");
							ret.DailyDischargeCapacity = read.GetDouble("daily_discharge_capacity");
							ret.CumulativeChargeCapacity = read.GetDouble("total_charge_capacity");
							ret.CumulativeDischargeCapacity = read.GetDouble("total_discharge_capacity");
							ret.StateOfCharge = read.GetDouble("soc");
							ret.MaximumAllowedChargePower = read.GetDouble("max_charge_power");
							ret.MaximumAllowedDischargePower = read.GetDouble("max_discharge_power");
							ret.UploadTime = read.GetDateTime("upload_time");
							ret.PowerStationId = read.GetInt32("power_station_id");
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			return ret;
		}

		public List<PCSInfo> GetPCSInfo(List<string> pcsName, DateTime dateTime) {
			List<PCSInfo> ret = new List<PCSInfo>();

			try {
				Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
				// 创建 MySQL 连接
				using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
					connection.Open();

					// 定义 SQL 查询
					string sql = "SELECT * FROM device_pcs_info WHERE dev_name IN @Names AND DATE(upload_time) = @TargetDate";
					ret = connection.Query<PCSInfo>(sql, new { Names = pcsName, TargetDate = dateTime.Date }).AsList();
				}
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			return ret;
		}

		public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetName, DateTime dateTime) {
			StationSystemIndexViewModel model = new StationSystemIndexViewModel();
			model.EnergyStorageCabinets = _powerStationService.GetAllEnergyStorageCabinetArray();//功下拉列表选择
			List<int> bindPowerStationIds = model.EnergyStorageCabinets.Select(s => s.PowerStationId).Distinct().ToList();
			model.PowerStationInfos = _powerStationService.GetAllPowerStationInfos().FindAll(s => bindPowerStationIds.Contains(s.Id));//功下拉列表选择

			if (string.IsNullOrEmpty(energyStorageCabinetName)) { //默认的
				model.selectedPowerStation = model.PowerStationInfos[0];
				model.selectedEnergyStorageCabinet = model.EnergyStorageCabinets.Find(s => s.PowerStationId == model.PowerStationInfos[0].Id);
			} else {
				model.selectedEnergyStorageCabinet = model.EnergyStorageCabinets.Find(s => s.rootDataFromMqtt.structure.name == energyStorageCabinetName);
				model.selectedPowerStation = model.PowerStationInfos.FirstOrDefault(s => s.Id == model.selectedEnergyStorageCabinet.PowerStationId);
			}
			RootDataFromMqtt viewDataSourceCabinet = model.selectedEnergyStorageCabinet.rootDataFromMqtt;
			//PCS devType=5
			List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, 5, 1);
			model.pcsInfos = this.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();
			return model;
		}

		public bool SaveEnergyStorageStackControlInfo(List<EnergyStorageStackControlInfo> energyStorageStackControlInfos) {
			if (energyStorageStackControlInfos == null || energyStorageStackControlInfos.Count == 0) {
				return false;
			}

			try {
				using (var connection = new MySqlConnection(_connectionString)) {
					connection.Open();
					const string insertQuery = @"
                        INSERT INTO EnergyStorageStackControlInfo (
upload_time,dev_type,dev_name,dev_id,sn,is_in_use,is_active
                            daily_charge_capacity, daily_discharge_capacity, total_charge_capacity, total_discharge_capacity,
                            soc, max_charge_power, max_discharge_power, max_load_power, total_voltage, max_voltage,
                            min_voltage, total_current, soh, soe, remaining_charge, remaining_discharge,
                            average_temperature, average_voltage, insulation, positive_insulation, negative_insulation,
                            max_temperature, min_temperature
                        )
                        VALUES (
@UploadTime,@DevType,@DevName,@DevId,@Sn,@IsInUse,@IsActive,
                            @DailyChargeCapacity, @DailyDischargeCapacity, @TotalChargeCapacity, @TotalDischargeCapacity,
                            @SOC, @MaxChargePower, @MaxDischargePower, @MaxLoadPower, @TotalVoltage, @MaxVoltage,
                            @MinVoltage, @TotalCurrent, @SOH, @SOE, @RemainingCharge, @RemainingDischarge,
                            @AverageTemperature, @AverageVoltage, @Insulation, @PositiveInsulation, @NegativeInsulation,
                            @MaxTemperature, @MinTemperature
                        );";
					using (var transaction = connection.BeginTransaction()) {
						connection.Execute(insertQuery, energyStorageStackControlInfos, transaction: transaction);
						transaction.Commit();
					}
					return true;
				}
			} catch (Exception ex) {
				return false;
			}

		}


	}
}
