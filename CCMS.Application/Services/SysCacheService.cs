using CCMS.Application.cache;
using CCMS.Application.Const;
using CCMS.Application.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services
{
   
    public interface ISysCacheService
    {
        bool Del(string key);
        Task<bool> DelAsync(string key);
    
  
        bool Set(string key, object value);
        Task<bool> SetAsync(string key, object value);
        string Get(string key);
        Task<string> GetAsync(string key);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
 
        Task<List<string>> GetAllPermission();
        Task<List<string>> GetPermission(long userId);
       Task SetAllPermission(List<string> permissions);
        Task SetPermission(long userId, List<string> permissions);
    }

    [ApiDescriptionSettings(Name = "Cache", Order = 100)]
    public class SysCacheService : ISysCacheService, ISingleton
    {
        private readonly ICache _cache;
        private readonly CacheOptions _cacheOptions;

        public SysCacheService(IOptions<CacheOptions> cacheOptions, Func<string, ISingleton, object> resolveNamed)
        {
            _cacheOptions = cacheOptions.Value;
            _cache = resolveNamed(_cacheOptions.CacheType.ToString(), default) as ICache;
        }
        public async Task<List<string>> GetAllPermission()
        {
            var cacheKey = CommonConst.CACHE_KEY_ALLPERMISSION;
            return await _cache.GetAsync<List<string>>(cacheKey);
        }
        public async Task<List<string>> GetPermission(long userId)
        {
            var cacheKey = CommonConst.CACHE_KEY_PERMISSION + $"{userId}";
            return await _cache.GetAsync<List<string>>(cacheKey);
        }

        public bool Del(string key)
        {
            _cache.Del(key);
            return true;
        }


        public Task<bool> DelAsync(string key)
        {
            _cache.DelAsync(key);
            return Task.FromResult(true);
        }

       
        public Task<bool> DelByPatternAsync(string key)
        {
            _cache.DelByPatternAsync(key);
            return Task.FromResult(true);
        }

      
        
        public bool Set(string key, object value)
        {
            return _cache.Set(key, value);
        }

       
        public async Task<bool> SetAsync(string key, object value)
        {
            return await _cache.SetAsync(key, value);
        }


        public string Get(string key)
        {
            return _cache.Get(key);
        }
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }


        public async Task<string> GetAsync(string key)
        {
            return await _cache.GetAsync(key);
        }

      
      
        public Task<T> GetAsync<T>(string key)
        {
            return _cache.GetAsync<T>(key);
        }

      

        public Task<bool> ExistsAsync(string key)
        {
            return _cache.ExistsAsync(key);
        }

        public async Task SetAllPermission(List<string> permissions)
        {
            var cacheKey = CommonConst.CACHE_KEY_ALLPERMISSION;
            await _cache.SetAsync(cacheKey, permissions);
        }

        public async Task SetPermission(long userId, List<string> permissions)
        {
            var cacheKey = CommonConst.CACHE_KEY_PERMISSION + $"{userId}";
            await _cache.SetAsync(cacheKey, permissions);
        }
    }
}
