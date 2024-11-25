using Microsoft.AspNetCore.Identity;

namespace IAMS.Common {
    public class Utility {
        private static PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        public static string GetEncryptPassword(string str) {
            return passwordHasher.HashPassword(null, str);
        }

        public static bool IsPasswordCorrect(string hashedPassword,string password) {
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
