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

        public static DateTime GetStartOfMonth(DateTime date) {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime GetEndOfMonth(DateTime date) {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static DateTime GetMinDate() {
            return new DateTime(2025, 1, 1);
        }
        public static DateTime GetMaxDate() {
            return new DateTime(2100, 1, 1);
        }
    }
}
