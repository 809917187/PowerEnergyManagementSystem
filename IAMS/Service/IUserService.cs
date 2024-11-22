using IAMS.Models.User;

namespace IAMS.Service {
    public interface IUserService {
        public List<UserInfo> GetAllUsers();
        public UserInfo GetUserInfoByEmailAndPassword(string email,string password);
    }
}
