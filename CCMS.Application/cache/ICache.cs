using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.Application.cache
{
    
    public interface ICache
    {
         
        long Del(params string[] key);
 
        Task<long> DelAsync(params string[] key);

        
        Task<long> DelByPatternAsync(string pattern);

       
        bool Exists(string key);

        
        Task<bool> ExistsAsync(string key);

        
        string Get(string key);

      
        T Get<T>(string key);

       
        Task<string> GetAsync(string key);
 
        Task<T> GetAsync<T>(string key);

       
        bool Set(string key, object value);

        
        bool Set(string key, object value, TimeSpan expire);

       
        Task<bool> SetAsync(string key, object value);

        
        Task<bool> SetAsync(string key, object value, TimeSpan expire);
 
        List<string> GetAllKeys();
    }
}
