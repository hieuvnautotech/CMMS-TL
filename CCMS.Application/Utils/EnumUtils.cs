using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
namespace CCMS.Application.Utils
{
    public class EnumEntity
    {
       
        public string Describe { set; get; }

       
        public string Name { set; get; }

        public int Value { set; get; }
    }

    public static class EnumUtils
    {
        public static string GetDescription(this System.Enum value)
        {
            return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
        public static string GetDescription(this object value)
        {
            return value.GetType().GetMember(value.ToString() ?? string.Empty).FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }

        public static List<EnumEntity> EnumToList(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum.");
            var arr = System.Enum.GetNames(type);
            return arr.Select(sl =>
            {
                var item = System.Enum.Parse(type, sl);
                return new EnumEntity
                {
                    Name = item.ToString(),
                    Describe = item.GetDescription(),
                    Value = item.GetHashCode()
                };
            }).ToList();
        }

        public static List<T> EnumToList<T>(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum.");
            var arr = System.Enum.GetNames(type);
            return arr.Select(name => (T)System.Enum.Parse(type, name)).ToList();
        }
    }
}
