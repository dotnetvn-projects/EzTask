using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EzTask.Web.Framework.WebContext
{
    public static class HtppContextExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsJson(this HttpResponse response, string key, object value, CookieOptions options)
        {
            response.Cookies.Append(key,JsonConvert.SerializeObject(value), options);
        }

        public static T GetObjectFromJson<T>(this HttpRequest request, string key)
        {
            var value = request.Cookies[key];

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
