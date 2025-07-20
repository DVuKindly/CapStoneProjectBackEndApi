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

        public Dictionary<string, string> ResponseData => new(_responseData);

        // --- Add data ---
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _requestData[key] = value;
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _responseData[key] = value;
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var value) ? value : string.Empty;
        }

        // --- Tạo URL thanh toán VNPay ---
        public string CreateRequestUrl(string baseUrl, string hashSecret, bool encodeHash = false)
        {
            var queryString = new StringBuilder();
            var hashData = new StringBuilder();

            foreach (var kv in _requestData)
            {
                if (string.IsNullOrWhiteSpace(kv.Value)) continue;

                var encodedKey = WebUtility.UrlEncode(kv.Key);
                var encodedValue = WebUtility.UrlEncode(kv.Value);

                queryString.Append($"{encodedKey}={encodedValue}&");

                if (encodeHash)
                {
                 
                    if (kv.Key == "vnp_IpnUrl")
                        hashData.Append($"{encodedKey}={kv.Value}&");
                    else
                        hashData.Append($"{encodedKey}={encodedValue}&");
                }
                else
                {
                    hashData.Append($"{kv.Key}={kv.Value}&");
                }
            }



            if (queryString.Length > 0) queryString.Length--;
            if (hashData.Length > 0) hashData.Length--;

            string secureHash = Utils.HmacSHA512(hashSecret, hashData.ToString());

            return $"{baseUrl}?{queryString}&vnp_SecureHashType=SHA512&vnp_SecureHash={secureHash}";
        }



        // --- Build RawData từ _responseData để validate chữ ký ---
        public string BuildRawResponseData(bool encode = false)
        {
            var data = new StringBuilder();
            var clone = new SortedList<string, string>(_responseData, new VnPayCompare());

            clone.Remove("vnp_SecureHashType");
            clone.Remove("vnp_SecureHash");

            foreach (var kv in clone)
            {
                if (string.IsNullOrWhiteSpace(kv.Value)) continue;

                var key = encode ? WebUtility.UrlEncode(kv.Key) : kv.Key;
                var value = encode ? WebUtility.UrlEncode(kv.Value) : kv.Value;

                data.Append($"{key}={value}&");
            }

            if (data.Length > 0)
                data.Length--;

            return data.ToString();
        }

        // --- Dùng để log chuỗi raw dễ debug ---
        public string BuildRawDebug()
        {
            return BuildRawResponseData(false);
        }

        // --- Validate chữ ký VNPay gửi về ---
        public bool ValidateSignature(string inputHash, string secretKey, bool encodeHash = false)
        {
            string rawData = BuildRawResponseData(encodeHash);
            string myChecksum = Utils.HmacSHA512(secretKey, rawData);

            Console.WriteLine("🔍 RawData: " + rawData);
            Console.WriteLine("🔑 MyChecksum: " + myChecksum);
            Console.WriteLine("🔐 InputHash: " + inputHash);

            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    // --- SHA512 Hash Helper ---
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
                hash.Append(theByte.ToString("x2"));

            return hash.ToString();
        }
    }

    // --- Dùng cho sort theo chuẩn VNPay ---
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
