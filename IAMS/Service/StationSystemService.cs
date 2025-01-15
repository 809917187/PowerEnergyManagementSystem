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
using System;
using IAMS.Common;

namespace IAMS.Service {
    public class StationSystemService : IStationSystemService {
        private string _connectionString;
        private IPowerStationService _powerStationService;
        public StationSystemService(IConfiguration configuration, IPowerStationService powerStationService) {
            _connectionString = configuration.GetConnectionString("gq");
            _powerStationService = powerStationService;
        }
        /*public EnergyStorageStackControlInfo GetEnergyStorageStackControlInfo(string sn) {
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
        }*/

        public List<GatewayTableModelInfo> GetGatewayTableModelInfo(List<string> names, DateTime dateTime) {
            List<GatewayTableModelInfo> ret = new List<GatewayTableModelInfo>();

            try {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string sql = "SELECT * FROM device_gateway_table_model WHERE dev_name IN @Names AND DATE(upload_time) = @TargetDate";
                    ret = connection.Query<GatewayTableModelInfo>(sql, new { Names = names, TargetDate = dateTime.Date }).AsList();
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }
        public List<EnergyStorageStackControlInfo> GetEnergyStorageStackControllerInfo(List<string> names, DateTime dateTime) {
            List<EnergyStorageStackControlInfo> ret = new List<EnergyStorageStackControlInfo>();

            try {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string sql = "SELECT * FROM device_energy_storage_stack_control_info WHERE dev_name IN @Names AND DATE(upload_time) = @TargetDate";
                    ret = connection.Query<EnergyStorageStackControlInfo>(sql, new { Names = names, TargetDate = dateTime.Date }).AsList();
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
        public PowerStationInfo? GetPowerStationRootInfoByName(string EnergyStorageCabinetArrayName) {
            return _powerStationService.GetAllPowerStationInfos().Find(s => s.EnergyStorageCabinetRootDataList.FindAll(m => m.rootDataFromMqtt.structure.name == EnergyStorageCabinetArrayName).Count > 0);
        }
        public List<SeriesData> GetRealTimeTrendOfChart(string EnergyStorageCabinetArrayName, DateTime today) {
            List<SeriesData> seriesDatas = new List<SeriesData>();
            PowerStationInfo powerStationInfo = this.GetPowerStationRootInfoByName(EnergyStorageCabinetArrayName);
            if (powerStationInfo == null) {
                return seriesDatas;
            }
            EnergyStorageCabinetInfo energyStorageCabinetInfo = powerStationInfo.EnergyStorageCabinetRootDataList.FirstOrDefault(s => s.rootDataFromMqtt.structure.name == EnergyStorageCabinetArrayName);

            List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
            var gatewayTableTotalActivePower = this.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), today).GroupBy(s => s.DevName).Select(
                g => new SeriesData() {
                    Name = g.Key + "有功功率",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.TotalActivePower }).ToList()
                }).ToList();//关口表总有功功率

            List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
            var pcsTotalActivePower = this.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), today).GroupBy(s => s.DevName).Select(
                g => new SeriesData() {
                    Name = g.Key + "有功功率",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.TotalActivePower }).ToList()
                }).ToList();//PCS

            List<Structure> energyStorageStackControlRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(energyStorageCabinetInfo.rootDataFromMqtt.structure, (int)DeviceCode.EnergyStorageStackControl, 1);
            var energyStorageStackControlGroupedByName = this.GetEnergyStorageStackControllerInfo(energyStorageStackControlRootInfos.Select(S => S.name).ToList(), today).GroupBy(s => s.DevName);
            var energyStorageStackControlSoc = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "SOC",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.StateOfCharge }).ToList()
                }).ToList();//堆控SOC
            var energyStorageStackControlHighestCellVoltage = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "单体最高电压",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.HighestCellVoltage }).ToList()
                }).ToList();//堆控单体最高电压
            var energyStorageStackControlLowestCellVoltage = energyStorageStackControlGroupedByName.Select(
                g => new SeriesData() {
                    Name = g.Key + "单体最低电压",
                    Data = g.Select(m => new object[] { m.UploadTime.ToString("yyyy-MM-ddTHH:mm:ss"), m.LowestCellVoltage }).ToList()
                }).ToList();//堆控单体最低电压

            seriesDatas = seriesDatas.Concat(gatewayTableTotalActivePower).Concat(pcsTotalActivePower).Concat(energyStorageStackControlSoc).Concat(energyStorageStackControlHighestCellVoltage).Concat(energyStorageStackControlLowestCellVoltage).ToList();

            return seriesDatas;
        }
        public TotalActivePowerOfChart GetTotalActivePowerOfChart(string EnergyStorageCabinetArrayName, DateTime today) {
            TotalActivePowerOfChart model = new TotalActivePowerOfChart();
            PowerStationInfo powerStationInfo = this.GetPowerStationRootInfoByName(EnergyStorageCabinetArrayName);
            if (powerStationInfo == null) {
                return model;
            }

            EnergyStorageCabinetInfo rootDataFromMqtt = powerStationInfo.EnergyStorageCabinetRootDataList.FirstOrDefault(s => s.rootDataFromMqtt.structure.name == EnergyStorageCabinetArrayName);

            //电网有功功率就是关口表上的和
            List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(rootDataFromMqtt.rootDataFromMqtt.structure, (int)DeviceCode.GatewayTableModel, 1);
            model.ChartDataOfElectricGrid = this.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), today).GroupBy(m => m.UploadTime).Select(g => new object[]{
                g.Key.ToString("yyyy-MM-ddTHH:mm:ss"),
                Math.Round(g.Sum(m => m.TotalActivePower), 2)
            }).OrderBy(s => s[0]).ToList();
            //储能功率从PCS总有功取
            List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(rootDataFromMqtt.rootDataFromMqtt.structure, (int)DeviceCode.PCS, 1);
            model.ChartDataOfEnergyStorage = this.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), today).GroupBy(m => m.UploadTime).Select(g => new object[]{
                g.Key.ToString("yyyy-MM-ddTHH:mm:ss"),
                Math.Round(g.Sum(m => m.TotalActivePower), 2)
            }).OrderBy(s => s[0]).ToList();

            return model;
        }

        

        public StationSystemIndexViewModel GetStationSystemIndexViewModel(string energyStorageCabinetName, DateTime dateTime) {
            StationSystemIndexViewModel model = new StationSystemIndexViewModel();
            model.PowerStationInfos = _powerStationService.GetAllPowerStationInfoByCabinetName(energyStorageCabinetName);

            RootDataFromMqtt viewDataSourceCabinet = _powerStationService.GetDataSourceCabinet(model.PowerStationInfos);

            //PCS devType=5
            List<Structure> pcsRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.PCS, 1);
            var pcsInfos = this.GetPCSInfo(pcsRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();
            //储能堆控
            List<Structure> bsuRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.EnergyStorageStackControl, 1);
            var energyStorageStackControlInfos = this.GetEnergyStorageStackControllerInfo(bsuRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();
            //关口表
            List<Structure> gatewayTableRootInfos = MQTTHelper.FindStructuresBydevTypeAndmenuTree(viewDataSourceCabinet.structure, (int)DeviceCode.GatewayTableModel, 1);
            var gatewayTableModelInfos = this.GetGatewayTableModelInfo(gatewayTableRootInfos.Select(S => S.name).ToList(), dateTime).GroupBy(m => m.DevName).Select(g => g.OrderByDescending(m => m.Id).First()).ToList();


            model.DailyChargeAmount = pcsInfos.Sum(s => s.DailyChargeAmount).ToString("F2");
            model.DailyDischargeAmount = pcsInfos.Sum(s => s.DailyDischargeAmount).ToString("F2");
            model.ACCumulativeChargeAmount = pcsInfos.Sum(s => s.ACCumulativeChargeAmount).ToString("F2");
            model.ACCumulativeDischargeAmount = pcsInfos.Sum(s => s.ACCumulativeDischargeAmount).ToString("F2");
            model.SOC = energyStorageStackControlInfos.Average(s => s.StateOfCharge).ToString("F2");
            model.PowerGrid = gatewayTableModelInfos.Select(s => s.TotalActivePower).Sum().ToString("F2");
            model.EnergyStorage = pcsInfos.Select(s => s.TotalActivePower).Sum().ToString("F2");
            model.Load = (Convert.ToDouble(model.PowerGrid) - Convert.ToDouble(model.EnergyStorage)).ToString("F2");

            foreach (var device in pcsInfos) {
                model.deviceStatus.Add((device.DevName, device.IsOnline ? "在线" : "离线"));
            }
            foreach (var device in energyStorageStackControlInfos) {
                model.deviceStatus.Add((device.DevName, device.IsOnline ? "在线" : "离线"));
            }
            foreach (var device in gatewayTableModelInfos) {
                model.deviceStatus.Add((device.DevName, device.IsOnline ? "在线" : "离线"));
            }

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
