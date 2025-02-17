using Dapper;
using IAMS.Models.PowerStation;
using IAMS.MQTT;
using IAMS.MQTT.Model;
using IAMS.ViewModels.StationSystem;
using MySql.Data.MySqlClient;
using System.Text;

namespace IAMS.Service {
    public class PowerStationService : IPowerStationService {
        private string _connectionString;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PowerStationService(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) {
            _connectionString = configuration.GetConnectionString("gq");
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool AddPowerSatationInfo(PowerStationInfo powerStationInfo) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string sql_main = "INSERT INTO " +
                            "power_station " +
                            "(name,owner,phone,installed_power,installed_capacity,start_time,country,state," +
                            "city,region,location_details,longitude,latitude,transformer_capacity,transformer_info,network_info,installer,installer_phone) " +
                            "VALUES " +
                            "(@Name,@Owner,@Phone,@InstalledPower,@InstalledCapacity,@StartTime,@Country,@State," +
                            "@City,@Region,@LocationDetails,@Longitude,@Latitude,@TransformerCapacity,@TransformerInfo,@NetworkInfo,@Installer,@InstallerPhone) ;" +
                            "SELECT LAST_INSERT_ID();";
                        powerStationInfo.Id = conn.ExecuteScalar<int>(sql_main, powerStationInfo, transaction);
                        InsertStationImages(powerStationInfo, conn, transaction);
                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        public bool DeletePowerSatationInfo(int PowerStationId) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Transaction = transaction;

                        // 执行删除操作
                        cmd.CommandText = "DELETE FROM power_station WHERE id=@id";
                        cmd.Parameters.AddWithValue("@id", PowerStationId);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM power_station_images WHERE power_station_id=@id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM power_station_install_images WHERE power_station_id=@id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE energy_storage_cabinet_array SET power_station_id = null WHERE power_station_id=@id";
                        cmd.ExecuteNonQuery();

                        // 提交事务
                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        public PowerStationInfo GetPowerStationInfoById(int id) {

            return this.GetAllPowerStationInfos().FirstOrDefault(s => s.Id == id);
            /*
                        PowerStationInfo powerStationInfo = new PowerStationInfo();

                        try {
                            // 创建 MySQL 连接
                            using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                                connection.Open();

                                // 定义 SQL 查询
                                string query = "SELECT " +
                                    "id AS Id,name AS Name,owner AS Owner,phone AS Phone," +
                                    "installed_power AS InstalledPower, installed_capacity AS InstalledCapacity," +
                                    "start_time AS StartTime,country AS Country,state AS State,city AS City,region AS Region," +
                                    "location_details AS LocationDetails,longitude AS Longitude,latitude AS Latitude," +
                                    "transformer_capacity AS TransformerCapacity,transformer_info AS TransformerInfo,network_info AS NetworkInfo," +
                                    "installer AS Installer,installer_phone AS InstallerPhone " +
                                    "FROM " +
                                    "power_station " +
                                    "WHERE id=" + id;
                                powerStationInfo = connection.Query<PowerStationInfo>(query).First();
                                powerStationInfo.StationImagesFilePath = this.GetAllStationImages(powerStationInfo.Id);
                                powerStationInfo.StationInstallImagesFilePath = this.GetAllStationInstallImages(powerStationInfo.Id);
                                // 访问 HttpContext 获取 Request.Scheme 和 Request.Host
                                var scheme = _httpContextAccessor.HttpContext?.Request.Scheme;
                                var host = _httpContextAccessor.HttpContext?.Request.Host.Value;
                                powerStationInfo.StationImagesFilePath = powerStationInfo.StationImagesFilePath.Select(s => $"{scheme}://{host}/{s}").ToList();
                                powerStationInfo.StationInstallImagesFilePath = powerStationInfo.StationInstallImagesFilePath.Select(s => $"{scheme}://{host}/{s}").ToList();
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"Error: {ex.Message}");
                        }

                        return powerStationInfo;*/
        }

        public List<PowerStationInfo> GetAllPowerStationInfos() {
            List<PowerStationInfo> powerStationInfos = new List<PowerStationInfo>();

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询，关联两个表
                    string query = @"
            SELECT 
                ps.id AS Id, ps.name AS Name, ps.owner AS Owner, ps.phone AS Phone,
                ps.installed_power AS InstalledPower, ps.installed_capacity AS InstalledCapacity,
                ps.start_time AS StartTime, ps.country AS Country, ps.state AS State, 
                ps.city AS City, ps.region AS Region, ps.location_details AS LocationDetails,
                ps.longitude AS Longitude, ps.latitude AS Latitude, 
                ps.transformer_capacity AS TransformerCapacity, ps.transformer_info AS TransformerInfo,
                ps.network_info AS NetworkInfo, ps.installer AS Installer, ps.installer_phone AS InstallerPhone,
                esc.json_structure AS JsonStructure
            FROM 
                power_station ps
            LEFT JOIN 
                energy_storage_cabinet_array esc
            ON 
                ps.id = esc.power_station_id;";
                    var lookup = new Dictionary<int, PowerStationInfo>();

                    connection.Query<PowerStationInfo, string, PowerStationInfo>(
                        query,
                        (powerStation, jsonStructure) => {
                            if (!lookup.TryGetValue(powerStation.Id, out var powerStationInfo)) {
                                powerStationInfo = powerStation;
                                powerStationInfo.EnergyStorageCabinetRootDataList = new List<EnergyStorageCabinetInfo>();
                                lookup[powerStation.Id] = powerStationInfo;
                            }

                            if (!string.IsNullOrEmpty(jsonStructure)) {
                                var energyCabinetInfo = new EnergyStorageCabinetInfo {
                                    IsSelected = false, 
                                    rootDataFromMqtt = MQTTHelper.ConvertRootInfoToObject(jsonStructure)
                                };
                                powerStationInfo.EnergyStorageCabinetRootDataList.Add(energyCabinetInfo);
                            }

                            return powerStationInfo;
                        },
                        splitOn: "JsonStructure" // 指定分隔点
                                );

                    // 将字典中的值转换为列表
                    powerStationInfos = lookup.Values.ToList();


                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }


            int role = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("Role")?.Value);
            if (role == 1) {
                
            } else {
                int userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value);
                var psList = this.GetBindPowerStationListByUserId(userId);
                powerStationInfos = powerStationInfos.FindAll(s => psList.Contains(s.Id));
            }
            return powerStationInfos;
        }

        public List<EnergyStorageCabinetInfo> GetAllEnergyStorageCabinetArray() {
            List<EnergyStorageCabinetInfo> ret = new List<EnergyStorageCabinetInfo>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT id,power_station_id,json_structure,name,power_station_id FROM energy_storage_cabinet_array";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                EnergyStorageCabinetInfo esci = new EnergyStorageCabinetInfo();
                                if (reader.IsDBNull(reader.GetOrdinal("power_station_id"))) {
                                    esci.IsSelected = false;
                                    esci.PowerStationId = null;
                                } else {
                                    esci.IsSelected = true;
                                    esci.PowerStationId = reader.GetInt32(reader.GetOrdinal("power_station_id"));
                                }
                                esci.CabinetName = reader.GetString(reader.GetOrdinal("name"));
                                if (reader.IsDBNull(reader.GetOrdinal("json_structure"))) {
                                    esci.rootDataFromMqtt = null;
                                } else {
                                    esci.rootDataFromMqtt = MQTTHelper.ConvertRootInfoToObject(reader.GetString(reader.GetOrdinal("json_structure")));
                                }
                                esci.CabinetId = reader.GetInt32(reader.GetOrdinal("id"));
                                ret.Add(esci);

                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ret;
        }


        public PowerStationInfo GetEnergyStorageCabinetArrayById(int PowerStationId) {
            PowerStationInfo ret = new PowerStationInfo() {
                Id = PowerStationId
            };
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT json_structure FROM energy_storage_cabinet_array WHERE power_station_id=@id";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@id", PowerStationId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                ret.EnergyStorageCabinetRootDataList.Add(
                                    new EnergyStorageCabinetInfo() {
                                        IsSelected = false,
                                        rootDataFromMqtt = MQTTHelper.ConvertRootInfoToObject(reader.GetString("json_structure"))
                                    });
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ret;
        }

        public List<string> GetAllStationImages(int PowerStationId) {
            List<string> allImages = new List<string>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT  power_station_id,image_path FROM  power_station_images WHERE power_station_id=@id";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@id", PowerStationId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                allImages.Add(reader.GetString("image_path"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {

            }
            return allImages;


        }

        public List<int> GetBindPowerStationListByUserId(int UserId) {
            List<int> ret = new List<int>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT power_station_id FROM power_station_bind_user WHERE user_id = @UserId";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@UserId", UserId);
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                ret.Add(reader.GetInt32("power_station_id"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return ret;
        }
        public List<int> GetBindUserListByPowerStationId(int PowerStationId) {
            List<int> ret = new List<int>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT user_id FROM power_station_bind_user WHERE power_station_id = @PowerStationId";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@PowerStationId", PowerStationId);
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                ret.Add(reader.GetInt32("user_id"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {

            }
            return ret;
        }
        public List<string> GetAllStationInstallImages(int PowerStationId) {
            List<string> allImages = new List<string>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT  power_station_id,image_path FROM  power_station_install_images WHERE power_station_id=@id";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@id", PowerStationId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                allImages.Add(reader.GetString("image_path"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {

            }
            return allImages;
        }

        public bool UpdateStationInfo(PowerStationInfo powerStationInfo) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string sql_main = "UPDATE " +
                            "power_station " +
                            "SET name = @Name, owner = @Owner, phone = @Phone, installed_power = @InstalledPower, " +
                            "installed_capacity = @InstalledCapacity, start_time = @StartTime," +
                            " country = @Country, state = @State, city = @City, region = @Region, location_details = @LocationDetails, " +
                            "longitude = @Longitude, latitude = @Latitude, transformer_capacity = @TransformerCapacity, " +
                            "transformer_info = @TransformerInfo, network_info = @NetworkInfo, installer = @Installer " +
                            "WHERE id=" + powerStationInfo.Id;
                        conn.Execute(sql_main, powerStationInfo, transaction);
                        if (powerStationInfo.StationImages.Count > 0) {
                            conn.Execute("DELETE FROM power_station_images WHERE power_station_id=" + powerStationInfo.Id);
                        }
                        if (powerStationInfo.StationInstallImages.Count > 0) {
                            conn.Execute("DELETE FROM power_station_install_images WHERE power_station_id=" + powerStationInfo.Id);
                        }
                        InsertStationImages(powerStationInfo, conn, transaction);
                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        private static void InsertStationImages(PowerStationInfo powerStationInfo, MySqlConnection conn, MySqlTransaction transaction) {
            if (powerStationInfo.StationImagesFilePath != null && powerStationInfo.StationImagesFilePath.Count > 0) {
                StringBuilder sql_images = new StringBuilder("INSERT INTO power_station_images (power_station_id,image_path) VALUES ");
                var parameters = new List<MySqlParameter>();
                for (int i = 0; i < powerStationInfo.StationImagesFilePath.Count; i++) {
                    var paramId = $"@PowerStationId{i}";
                    var paramImage = $"@ImagePath{i}";
                    sql_images.Append($"({paramId}, {paramImage}),");
                    parameters.Add(new MySqlParameter(paramId, powerStationInfo.Id));
                    parameters.Add(new MySqlParameter(paramImage, powerStationInfo.StationImagesFilePath[i]));

                }
                sql_images.Length--;  // 移除最后一个逗号
                var command = new MySqlCommand(sql_images.ToString(), conn, transaction);
                command.Parameters.AddRange(parameters.ToArray());
                command.ExecuteNonQuery();
            }
            if (powerStationInfo.StationInstallImagesFilePath != null && powerStationInfo.StationInstallImagesFilePath.Count > 0) {
                StringBuilder sql_images = new StringBuilder("INSERT INTO power_station_install_images (power_station_id,image_path) VALUES ");
                var parameters = new List<MySqlParameter>();
                for (int i = 0; i < powerStationInfo.StationInstallImagesFilePath.Count; i++) {
                    var paramId = $"@PowerStationId{i}";
                    var paramImage = $"@ImagePath{i}";
                    sql_images.Append($"({paramId}, {paramImage}),");
                    parameters.Add(new MySqlParameter(paramId, powerStationInfo.Id));
                    parameters.Add(new MySqlParameter(paramImage, powerStationInfo.StationInstallImagesFilePath[i]));

                }
                sql_images.Length--;  // 移除最后一个逗号
                var command = new MySqlCommand(sql_images.ToString(), conn, transaction);
                command.Parameters.AddRange(parameters.ToArray());
                command.ExecuteNonQuery();
            }
        }

        public List<PowerStationInfo> GetAllPowerStationInfoByCabinetName(string cabinetName) {
            List<PowerStationInfo> PowerStationInfos = this.GetAllPowerStationInfos();
            PowerStationInfos = PowerStationInfos.FindAll(s => s.EnergyStorageCabinetRootDataList.Count > 0);
            PowerStationInfo selectedPS = PowerStationInfos[0];
            EnergyStorageCabinetInfo selectedCabinet = selectedPS.EnergyStorageCabinetRootDataList[0];

            if (!string.IsNullOrEmpty(cabinetName)) {
                selectedPS = PowerStationInfos.FirstOrDefault(s => s.EnergyStorageCabinetRootDataList.Any(m => m.rootDataFromMqtt.structure.name == cabinetName));
                selectedCabinet = selectedPS.EnergyStorageCabinetRootDataList.FirstOrDefault(s => s.rootDataFromMqtt.structure.name == cabinetName);
            }
            selectedPS.IsSelected = true;
            selectedCabinet.IsSelected = true;
            return PowerStationInfos;
        }

        public RootDataFromMqtt GetDataSourceCabinet(List<PowerStationInfo> powerStationInfos) {
            return powerStationInfos.SelectMany(s => s.EnergyStorageCabinetRootDataList).FirstOrDefault(m => m.IsSelected)?.rootDataFromMqtt;
        }

        public bool BindPowerStationToUser(int PowerStationId, List<int> UserIds) {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string sql = "DELETE FROM power_station_bind_user WHERE power_station_id=@PowerStationId";
                        // 使用 MySqlCommand 执行更新操作
                        using (var cmd = new MySqlCommand(sql, conn, transaction)) {
                            cmd.Parameters.AddWithValue("@PowerStationId", PowerStationId);

                            // 执行更新
                            cmd.ExecuteNonQuery();
                        }
                        if (UserIds.Count > 0) {
                            string insertSql = "INSERT INTO power_station_bind_user (power_station_id, user_id) VALUES (@PowerStationId, @UserId)";
                            var insertParams = UserIds.Select(userId => new { PowerStationId, UserId = userId }).ToList();

                            conn.Execute(insertSql, insertParams, transaction);
                        }

                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }
        public bool BindCabinetToPowerStation(int PowerStationId, List<int> CabinetIds) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string cabinetIdsStr = string.Join(",", CabinetIds);
                        string sql = "UPDATE energy_storage_cabinet_array SET power_station_id = NULL WHERE power_station_id=@PowerStationId";
                        // 使用 MySqlCommand 执行更新操作
                        using (var cmd = new MySqlCommand(sql, conn, transaction)) {
                            cmd.Parameters.AddWithValue("@PowerStationId", PowerStationId);

                            // 执行更新
                            cmd.ExecuteNonQuery();
                        }
                        if (CabinetIds.Count > 0) {
                            sql = @"
                    UPDATE energy_storage_cabinet_array 
                    SET power_station_id = @PowerStationId
                    WHERE id IN (" + cabinetIdsStr + ")";

                            // 使用 MySqlCommand 执行更新操作
                            using (var cmd = new MySqlCommand(sql, conn, transaction)) {
                                cmd.Parameters.AddWithValue("@PowerStationId", PowerStationId);

                                // 执行更新
                                cmd.ExecuteNonQuery();
                            }
                        }


                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
