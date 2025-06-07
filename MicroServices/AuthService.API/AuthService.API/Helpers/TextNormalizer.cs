using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace AuthService.API.Helpers
{
    public static class TextNormalizer
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Bỏ dấu tiếng Việt
            string normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            normalized = builder.ToString().Normalize(NormalizationForm.FormC);

            // Bỏ khoảng trắng thừa, chuyển về chữ thường
            normalized = Regex.Replace(normalized, @"\s+", " ").Trim().ToLower();

            return normalized;
        }
        public static bool IsSameLocation(string? loc1, string? loc2)
        {
            var n1 = Normalize(loc1 ?? "");
            var n2 = Normalize(loc2 ?? "");
            return string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase);
        }

    }
}
