using System.Collections.Generic;
using System.Linq;
using Furion.JsonSerialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CCMS.Application.Core
{
    
    public static class JsonUtil
    {
        
        public static T ToObject<T>(this string json)
        {
            json = json.Replace("&nbsp;", "");
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
        
        public static object ToObject(this string Json)
        {
            return string.IsNullOrEmpty(Json) ? null : JsonConvert.DeserializeObject(Json);
        }
        
        public static string ToJsonString(this object obj)
        {
            return obj == null ? string.Empty : JsonConvert.SerializeObject(obj);
        }

       
        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

        
        public static T ToObject<T>(this IDictionary<string, object> dictionary)
        {
            return dictionary.ToJsonString().ToObject<T>();
        }
       
        public static string ArrayToString(dynamic data, string Str)
        {
            string resStr = Str;
            foreach (var item in data)
            {
                if (resStr != "")
                {
                    resStr += ",";
                }

                if (item is string)
                {
                    resStr += item;
                }
                else
                {
                    resStr += item.Value;

                }
            }
            return resStr;
        }
      
        public static bool IsArrayIntersection<T>(List<T> list1, List<T> list2)
        {
            List<T> t = list1.Distinct().ToList();

            var exceptArr = t.Except(list2).ToList();

            if (exceptArr.Count < t.Count)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}