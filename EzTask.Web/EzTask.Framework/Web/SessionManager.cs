using Microsoft.AspNetCore.Http;

namespace EzTask.Framework.Web
{
    public class SessionManager
    {
        private EzTaskHttpContext _ezTaskHttp;
        public SessionManager(IHttpContextAccessor httpContext)
        {
            _ezTaskHttp = new EzTaskHttpContext(httpContext);
        }
        
        public void Set(string key, string value)
        {
            _ezTaskHttp.Context.HttpContext.Session.SetString(key, value);
        }

        public string Get(string key)
        {
            try
            {
                return _ezTaskHttp.Context.HttpContext.Session.GetString(key);
            }
            catch
            {
                return null;
            }
        }

        public void SetObject(string key, object value)
        {
            _ezTaskHttp.Context.HttpContext.Session.SetObjectAsJson(key, value);
        }

        public object GetObject<T>(string key)
        {
            try
            {
                return _ezTaskHttp.Context.HttpContext.Session.GetObjectFromJson<T>(key);
            }
            catch
            {
                return null;
            }
        }

        public void Suspend(string key)
        {
            _ezTaskHttp.Context.HttpContext.Session.Remove(key);
        }
    }
}
