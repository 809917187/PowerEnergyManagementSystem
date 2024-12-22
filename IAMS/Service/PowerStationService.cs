using Dapper;
using IAMS.Models.PowerStation;
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

            return powerStationInfo;
        }

        public List<PowerStationInfo> GetAllPowerStationInfos() {
            List<PowerStationInfo> powerStationInfos = new List<PowerStationInfo>();

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
                        "power_station;";
                    powerStationInfos = connection.Query<PowerStationInfo>(query).ToList();
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return powerStationInfos;
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
    }
}
