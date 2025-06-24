// Helpers/VnPayLibrary.cs
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PaymentService.API.Helpers
{
    public class VnPayLibrary
    {
        public const string VERSION = "2.1.0";
        private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new(new VnPayCompare());
        public Dictionary<string, string> ResponseData => new Dictionary<string, string>(_responseData);


        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _requestData[key] = value;
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _responseData[key] = value;
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string BuildRawDebug()
        {
            var clone = new SortedList<string, string>(_responseData, new VnPayCompare());
            clone.Remove("vnp_SecureHashType");
            clone.Remove("vnp_SecureHash");

            return string.Join("&", clone.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret, bool encodeHash = true)
        {
            var queryString = new StringBuilder();
            var hashData = new StringBuilder();

            foreach (var kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    var encodedKey = WebUtility.UrlEncode(kv.Key);
                    var encodedValue = WebUtility.UrlEncode(kv.Value);

                    queryString.Append($"{encodedKey}={encodedValue}&");

                    // Cho phép tùy chọn encode khi hash
                    if (encodeHash)
                        hashData.Append($"{encodedKey}={encodedValue}&");
                    else
                        hashData.Append($"{kv.Key}={kv.Value}&");
                }
            }

            if (queryString.Length > 0) queryString.Length--;
            if (hashData.Length > 0) hashData.Length--;

            string secureHash = Utils.HmacSHA512(hashSecret, hashData.ToString());

            return $"{baseUrl}?{queryString}&vnp_SecureHashType=SHA512&vnp_SecureHash={secureHash}";
        }

        public bool ValidateSignature(string inputHash, string secretKey, bool encodeHash = false) // <-- sửa thành true
        {
            string rawData = BuildRawResponseData(encodeHash); // dùng bản encode
            string myChecksum = Utils.HmacSHA512(secretKey, rawData);

            Console.WriteLine("🔍 RawData: " + rawData);
            Console.WriteLine("🔑 MyChecksum: " + myChecksum);
            Console.WriteLine("🔐 InputHash: " + inputHash);

            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }



        public string BuildRawResponseData(bool encode = false)
        {
            var data = new StringBuilder();
            var clone = new SortedList<string, string>(_responseData, new VnPayCompare());

            clone.Remove("vnp_SecureHashType");
            clone.Remove("vnp_SecureHash");

            foreach (var kv in clone)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    var key = encode ? WebUtility.UrlEncode(kv.Key) : kv.Key;
                    var value = encode ? WebUtility.UrlEncode(kv.Value) : kv.Value;
                    data.Append($"{key}={value}&");
                }
            }

            if (data.Length > 0)
                data.Length--;

            return data.ToString();
        }

    }

    public static class Utils
    {
        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);

            using var hmac = new HMACSHA512(keyBytes);
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var comparer = CompareInfo.GetCompareInfo("en-US");
            return comparer.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
