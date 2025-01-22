using Dapper;
using IAMS.Models.PriceTemplate;
using MySql.Data.MySqlClient;
using System.Text;

namespace IAMS.Service {
    public class TemplateService : ITemplateService {
        private string _connectionString;
        public TemplateService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("gq");
        }

        public bool AddTemplate(PriceTemplateInfo priceTemplate) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string sql_main = "INSERT INTO price_template (name,tag,creater_id) VALUES (@Name,@Tag,@CreaterId) ;" +
                            "SELECT LAST_INSERT_ID();";
                        using (MySqlCommand cmd = new MySqlCommand(sql_main, conn)) {
                            cmd.Parameters.AddWithValue("@Name", priceTemplate.Name);
                            cmd.Parameters.AddWithValue("@Tag", priceTemplate.Tag);
                            cmd.Parameters.AddWithValue("@CreaterId", priceTemplate.CreaterId);
                            priceTemplate.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        List<MySqlParameter> parameters;
                        StringBuilder sqlBuilder;
                        InsertTimeFrameInfo(priceTemplate, conn, transaction);
                        InsertBuyAndSalseInfo(priceTemplate, conn, transaction);

                        transaction.Commit();
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        private static void InsertBuyAndSalseInfo(PriceTemplateInfo priceTemplate, MySqlConnection conn, MySqlTransaction transaction) {
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO price_template_bug_salse (template_id, time_frame_type_code, buy_price,salse_price) VALUES ");
            List<int> types = priceTemplate.TimeFrameTypeCode2Name.Keys.ToList();
            for (int i = 0; i < types.Count; i++) {
                sqlBuilder.Append($"(@template_id{i}, @time_frame_type_code{i},@buy_price{i},@salse_price{i})");
                if (i < types.Count - 1) {
                    sqlBuilder.Append(", ");
                }
                parameters.Add(new MySqlParameter($"@template_id{i}", priceTemplate.Id));
                parameters.Add(new MySqlParameter($"@time_frame_type_code{i}", types[i]));
                parameters.Add(new MySqlParameter($"@buy_price{i}", priceTemplate.TimeFrame2BuyPrice[types[i]]));
                parameters.Add(new MySqlParameter($"@salse_price{i}", priceTemplate.TimeFrame2SalePrice[types[i]]));
            }
            using (MySqlCommand cmd = new MySqlCommand(sqlBuilder.ToString(), conn)) {
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertTimeFrameInfo(PriceTemplateInfo priceTemplate, MySqlConnection conn, MySqlTransaction transaction) {
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO price_template_frame_info (template_id, start_time,end_time,time_frame_type) VALUES ");
            for (int i = 0; i < priceTemplate.timeFrameInfos.Count; i++) {
                sqlBuilder.Append($"(@template_id{i}, @start_time{i},@end_time{i},@time_frame_type{i})");
                if (i < priceTemplate.timeFrameInfos.Count - 1) {
                    sqlBuilder.Append(", ");
                }
                parameters.Add(new MySqlParameter($"@template_id{i}", priceTemplate.Id));
                parameters.Add(new MySqlParameter($"@start_time{i}", priceTemplate.timeFrameInfos[i].StartTime.TotalMinutes));
                parameters.Add(new MySqlParameter($"@end_time{i}", priceTemplate.timeFrameInfos[i].EndTime.TotalMinutes));
                parameters.Add(new MySqlParameter($"@time_frame_type{i}", priceTemplate.timeFrameInfos[i].TimeFrameType));
            }
            using (MySqlCommand cmd = new MySqlCommand(sqlBuilder.ToString(), conn)) {
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }

        public bool DeleteTemplateInfo(int templateId) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Transaction = transaction;

                        // 执行删除操作
                        cmd.CommandText = "DELETE FROM price_template WHERE id=@id";
                        cmd.Parameters.AddWithValue("@id", templateId);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM price_template_frame_info WHERE template_id=@id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM price_template_bug_salse WHERE template_id=@id";
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

        public List<PriceTemplateInfo> GetAllPriceTemplateInfos() {
            List<PriceTemplateInfo> ret = new List<PriceTemplateInfo>();

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "price_template.id as Id,price_template.name as Name,tag as Tag,price_template.create_time as CreateTime," +
                        "user.name as CreaterName FROM price_template " +
                        "LEFT JOIN " +
                        "user " +
                        "ON " +
                        "price_template.creater_id=user.id;";
                    ret = connection.Query<PriceTemplateInfo>(query).ToList();
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }

        public List<TimeFrameInfo> GetTimeFrameInfoById(int templateId) {
            PriceTemplateInfo priceTemplateInfo = new PriceTemplateInfo();
            List<TimeFrameInfo> ret = new List<TimeFrameInfo>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT  start_time,end_time,time_frame_type FROM price_template_frame_info WHERE template_id=@id";

                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@id", templateId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                TimeFrameInfo timeFrameInfo = new TimeFrameInfo() {
                                    StartTime = TimeSpan.FromMinutes(reader.GetInt32("start_time")),
                                    EndTime = TimeSpan.FromMinutes(reader.GetInt32("end_time")),
                                    TimeFrameType = reader.GetInt32("time_frame_type")
                                };
                                timeFrameInfo.StartTimeStr = timeFrameInfo.StartTime.ToString(@"hh\:mm");
                                timeFrameInfo.EndTimeStr = timeFrameInfo.EndTime.ToString(@"hh\:mm");
                                timeFrameInfo.TimeFrameTypeName = priceTemplateInfo.TimeFrameTypeCode2Name[timeFrameInfo.TimeFrameType];
                                // 读取每一行数据
                                ret.Add(timeFrameInfo);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ret;
        }

        public PriceTemplateInfo GetPriceTemplateInfoById(int templateId) {
            PriceTemplateInfo ret = new PriceTemplateInfo();
            ret.Id = templateId;
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "price_template.id as Id,price_template.name as Name,tag as Tag,price_template.create_time as CreateTime," +
                        "user.name as CreaterName FROM price_template " +
                        "LEFT JOIN " +
                        "user " +
                        "ON " +
                        "price_template.creater_id=user.id " +
                        "WHERE price_template.id=@id;";
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@id", templateId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                ret.Name = reader.GetString("Name");
                                ret.Tag = reader.GetString("tag");
                                ret.CreateTime = reader.GetDateTime("CreateTime");
                                ret.CreaterName = reader.GetString("CreaterName");
                            }
                        }
                    }
                    ret.timeFrameInfos = this.GetTimeFrameInfoById(templateId);
                    ret.TimeFrame2BuyPrice = this.GetTimeFrame2BuyPrice(templateId);
                    ret.TimeFrame2SalePrice = this.GetTimeFrame2SalePrice(templateId);
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }

        // 定义 SQL 查询                             
        string query_buy_salse = "SELECT time_frame_type_code,buy_price,salse_price FROM price_template_bug_salse WHERE template_id=@id ORDER BY time_frame_type_code";
        public Dictionary<int, decimal> GetTimeFrame2BuyPrice(int templateId) {
            Dictionary<int, decimal> ret = new Dictionary<int, decimal>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_buy_salse, connection)) {
                        command.Parameters.AddWithValue("@id", templateId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                ret.Add(reader.GetInt32("time_frame_type_code"), reader.GetDecimal("buy_price"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ret;
        }

        public Dictionary<int, decimal> GetTimeFrame2SalePrice(int templateId) {
            Dictionary<int, decimal> ret = new Dictionary<int, decimal>();
            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_buy_salse, connection)) {
                        command.Parameters.AddWithValue("@id", templateId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                ret.Add(reader.GetInt32("time_frame_type_code"), reader.GetDecimal("salse_price"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ret;
        }
        public PriceTemplateInfo GetTemplateByPowerStationId(int powerstationId) {
            PriceTemplateInfo ret = new PriceTemplateInfo();

            try {
                string sql = "SELECT price_template_id FROM power_station_map_price_template WHERE power_station_id = @id";
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@id", powerstationId);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                ret = this.GetPriceTemplateInfoById(reader.GetInt32("price_template_id"));
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ret;
        }
        public bool UpdatePriceTemplate(PriceTemplateInfo priceTemplate) {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    try {
                        string sql_main = "UPDATE " +
                            "price_template " +
                            "SET name = @Name, tag = @Tag " +
                            "WHERE id=@Id";
                        using (MySqlCommand cmd = new MySqlCommand(sql_main, conn, transaction)) {
                            cmd.Parameters.AddWithValue("@Name", priceTemplate.Name);
                            cmd.Parameters.AddWithValue("@Tag", priceTemplate.Tag);
                            cmd.Parameters.AddWithValue("@Id", priceTemplate.Id);
                            cmd.ExecuteNonQuery();

                            string sql_delete_time_frame = "" +
                                "DELETE FROM price_template_frame_info WHERE template_id=@Id;" +
                                "DELETE FROM price_template_bug_salse WHERE template_id=@Id;";
                            cmd.Parameters.Clear();
                            cmd.CommandText = sql_delete_time_frame;
                            cmd.Parameters.AddWithValue("@Id", priceTemplate.Id);
                            cmd.ExecuteNonQuery();

                            InsertTimeFrameInfo(priceTemplate, conn, transaction);
                            InsertBuyAndSalseInfo(priceTemplate, conn, transaction);
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
