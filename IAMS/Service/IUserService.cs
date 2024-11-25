using IAMS.Models.User;

namespace IAMS.Service {
    public interface IUserService {
        public List<UserInfo> GetAllUsers();
        public UserInfo GetUserInfoByEmailAndPassword(string email,string password);
        public bool AddUser(UserInfo userInfo);
        public bool UpdateUser(UserInfo userInfo);
        public bool UpdatePassword(string oldPassword,string newPassword,int userId);
    }
}
