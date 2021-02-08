using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kovai.Serverless360.Bam
{
    internal static class Extensions
    {
        internal static async Task<T> ReadAsJsonAsync<T>(this HttpContent content) where T : class
        {
            try
            {
                var contentStr = await content.ReadAsStringAsync();
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(contentStr)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return serializer.ReadObject(stream) as T;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
        internal static void AddOrReplace(this HttpRequestHeaders headers, string name, string value)
        {
            if (headers.Contains(name))
                headers.Remove(name);
            headers.Add(name, value);
        }
        internal static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        internal static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static T DeSerialize<T>(this string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static string Serialize<T>(this T instance) where T : class
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}