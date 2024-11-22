using IAMS.Models.User;
using MySql.Data.MySqlClient;

namespace IAMS.Service {
    public class UserService : IUserService {
        private string _connectionString;
        public UserService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("gq");
        }

        public UserInfo GetUserInfoByEmailAndPassword(string email, string password) {
            UserInfo? user = null;

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "user.id, user.email,user.password,user.name,user.phone_number,user.role_code,user.create_time,role.role_name " +
                        "FROM user,role " +
                        "WHERE user.role_code=role.role_code AND user.email=@email AND user.password=@password";

                    // 创建 MySQL 命令
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                user = new UserInfo {
                                    Id = reader.GetInt32("id"),
                                    Email = reader.GetString("email"),
                                    Password = reader.GetString("password"),
                                    Name = reader.GetString("name"),
                                    PhoneNumber = reader.GetString("phone_number"),
                                    RoleCode = reader.GetInt32("id"),
                                    RoleName = reader.GetString("role_name"),
                                    CreateTime = reader.GetDateTime("create_time")
                                };
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return user;
        }


        public List<UserInfo> GetAllUsers() {
            List<UserInfo> users = new List<UserInfo>();

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "user.id, user.email,user.password,user.name,user.phone_number,user.role_code,user.create_time,role.role_name " +
                        "FROM user,role " +
                        "WHERE user.role_code=role.role_code";

                    // 创建 MySQL 命令
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                users.Add(new UserInfo {
                                    Id = reader.GetInt32("id"),
                                    Email = reader.GetString("email"),
                                    Password = reader.GetString("password"),
                                    Name=reader.GetString("name"),
                                    PhoneNumber = reader.GetString("phone_number"),
                                    RoleCode = reader.GetInt32("id"),
                                    RoleName = reader.GetString("role_name"),
                                    CreateTime = reader.GetDateTime("create_time")
                                });
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return users;
        }
    }
}
