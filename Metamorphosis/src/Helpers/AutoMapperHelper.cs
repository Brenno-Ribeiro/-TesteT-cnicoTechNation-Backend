using AutoMapper;
using Metamorphosis.App.Models;
using System.Linq;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace Metamorphosis.App.Helpers
{
    internal class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<string[], MyCdn>().ConvertUsing(src => ConvertArrayToObject<MyCdn>(src));
            CreateMap<string[], Source>().ConvertUsing(src => ConvertArrayToObject<Source>(src));
        }

        private T ConvertArrayToObject<T>(string[] src) where T : new()
        {
            var dest = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i < src.Length && i < properties.Length; i++)
            {
                if (src[i].Contains("HTTP"))
                {
                    src[i].Replace(" HTTP", "");
                }

                properties[i].SetValue(dest, src[i]);
            }

            return dest;
        }
    }
}