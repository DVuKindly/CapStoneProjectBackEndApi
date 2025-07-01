using System.Text.RegularExpressions;

namespace AuthService.API.Helpers
{
    public static class StringValidator
    {
        private static readonly Regex ValidUsernameRegex = new("^[a-zA-Z0-9_]{6,20}$");

        public static bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) &&
                   username.Length >= 6 &&
                   username.Length <= 20 &&
                   ValidUsernameRegex.IsMatch(username);
        }

        public static bool IsNumericOnly(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }


        public static bool HasWhitespace(string input)
        {
            return input.Any(char.IsWhiteSpace);
        }

        public static bool IsValidPassword(string password)
        {
            return password.Length >= 6 && password.Length <= 20 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsLetter) &&
                   !HasWhitespace(password);
        }
    }

}
