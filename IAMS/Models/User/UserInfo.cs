namespace IAMS.Models.User {
    public class UserInfo {
        public int Id { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleCode { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool KeepLoggedIn { get; set; }
        public bool IsDelete { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
