using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RedisProxy
{
    public class RedisSidecarProxy : IRedisSidecarProxy
    {
        private readonly HttpClient _http;
        public RedisSidecarProxy(HttpClient http)
        {
            _http = http;
        }

        public async Task SetUserProfileCache(UserProfileDto user)
        {
            await _http.PostAsJsonAsync("api/cache/user", user);
        }

        public async Task RemoveUserProfileCache(string userId)
        {
            await _http.DeleteAsync($"api/cache/user/{userId}");
        }
    }

}
