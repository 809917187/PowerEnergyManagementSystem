using IAMS.Common;
using IAMS.Models.User;
using MySql.Data.MySqlClient;

namespace IAMS.Service {
    public class UserService : IUserService {
        private string _connectionString;
        public UserService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("ems");
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
                        "WHERE user.role_code=role.role_code AND user.email=@email AND is_delete=0";



                    // 创建 MySQL 命令
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@email", email);
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
                                    RoleCode = reader.GetInt32("role_code"),
                                    RoleName = reader.GetString("role_name"),
                                    CreateTime = reader.GetDateTime("create_time")
                                };
                            }
                        }

                    }

                    if (user != null && Utility.IsPasswordCorrect(user.Password, password)) {
                        return user;
                    } else {
                        return null;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");

            }
            return null;
        }


        public List<UserInfo> GetAllUsers() {
            List<UserInfo> users = new List<UserInfo>();

            try {
                // 创建 MySQL 连接
                using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
                    connection.Open();

                    // 定义 SQL 查询
                    string query = "SELECT " +
                        "user.id, user.email,user.password,user.name,user.phone_number,user.role_code,user.create_time,role.role_name,user.is_delete " +
                        "FROM user,role " +
                        "WHERE user.role_code=role.role_code ";

                    // 创建 MySQL 命令
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        // 执行查询并读取结果
                        using (MySqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                // 读取每一行数据
                                users.Add(new UserInfo() {
                                    Id = reader.GetInt32("id"),
                                    Email = reader.GetString("email"),
                                    Password = reader.GetString("password"),
                                    Name = reader.GetString("name"),
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("phone_number")) ? null : reader.GetString("phone_number"),
                                    RoleCode = reader.GetInt32("id"),
                                    RoleName = reader.GetString("role_name"),
                                    CreateTime = reader.GetDateTime("create_time"),
                                    IsDelete = reader.GetBoolean("is_delete"),
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

        public bool AddUser(UserInfo userInfo) {
            try {
                using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                    conn.Open();
                    string sql = "INSERT INTO " +
                        "user (email,password,name,phone_number,role_code) " +
                        "VALUES(@email,@password,@name,@phone_number,@role_code)";
                    using (MySqlCommand command = new MySqlCommand(sql, conn)) {
                        command.Parameters.AddWithValue("@email", userInfo.Email);
                        command.Parameters.AddWithValue("@password", Utility.GetEncryptPassword(userInfo.Password));
                        command.Parameters.AddWithValue("@name", userInfo.Name);
                        command.Parameters.AddWithValue("@phone_number", userInfo.PhoneNumber);
                        command.Parameters.AddWithValue("@role_code", userInfo.RoleCode);

                        int row = command.ExecuteNonQuery();
                        return row > 0;
                    }

                }
            } catch (Exception ex) {
                return false;
            }
        }



        public bool UpdateUser(UserInfo userInfo) {
            try {
                using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                    conn.Open();
                    string sql = "UPDATE user SET " +
                        "name=@name,password=@password,phone_number=@phone_number,role_code=@role_code" +
                        " WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@name", userInfo.Name);
                        cmd.Parameters.AddWithValue("@phone_number", userInfo.PhoneNumber);
                        cmd.Parameters.AddWithValue("@role_code", userInfo.RoleCode);
                        cmd.Parameters.AddWithValue("@password", Utility.GetEncryptPassword(userInfo.Password));
                        cmd.Parameters.AddWithValue("@id", userInfo.Id);

                        int row = cmd.ExecuteNonQuery();
                        return row > 0;
                    }
                }
            } catch (Exception ex) {
                return false;
            }
        }

        public bool IsPasswordCorrect(int userId, string password) {
            try {
                using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                    conn.Open();

                    string sql = "SELECT password FROM user WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@id", userId);
                        using (MySqlDataReader dr = cmd.ExecuteReader()) {
                            if (dr.Read()) {
                                return Utility.IsPasswordCorrect(dr.GetString(0), password);
                            } else {
                                return false;
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                return false;
            }
        }

        public bool UpdatePassword(string oldPassword, string newPassword, int userId) {
            try {
                if (this.IsPasswordCorrect(userId, oldPassword)) {
                    using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                        conn.Open();

                        string sql = "UPDATE user SET password=@new_password WHERE id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                            cmd.Parameters.AddWithValue("@new_password", Utility.GetEncryptPassword(newPassword));
                            cmd.Parameters.AddWithValue("@id", userId);
                            int row = cmd.ExecuteNonQuery();
                            return row > 0;
                        }
                    }
                } else {
                    return false;
                }

            } catch (Exception ex) {
                return false;
            }
        }

        public bool DeleteUser(int userId) {
            try {
                using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                    conn.Open();
                    string sql = "UPDATE user SET is_delete=1 WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@id", userId);
                        int row = cmd.ExecuteNonQuery();
                        return row > 0;
                    }
                }
            } catch (Exception er) {
                return false;
            }
        }
    }
}
