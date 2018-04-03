using EzTask.Framework.Values;
using Microsoft.AspNetCore.Http;

namespace EzTask.Framework.Web.HttpContext
{
    public class SessionManager
    {
        private EzTaskHttpContext _ezTaskHttp;
        public SessionManager(IHttpContextAccessor httpContext)
        {
            _ezTaskHttp = new EzTaskHttpContext(httpContext);
        }
        
        public void Set(EzTaskKey key, string value)
        {
            _ezTaskHttp.Context.HttpContext.Session.SetString(key.ToString(), value);
        }

        public string Get(EzTaskKey key)
        {
            try
            {
                return _ezTaskHttp.Context.HttpContext.Session.GetString(key.ToString());
            }
            catch
            {
                return null;
            }
        }

        public void SetObject(EzTaskKey key, object value)
        {
            _ezTaskHttp.Context.HttpContext.Session.SetObjectAsJson(key.ToString(), value);
        }

        public T GetObject<T>(EzTaskKey key)
        {
            try
            {
                return _ezTaskHttp.Context.HttpContext.Session.GetObjectFromJson<T>(key.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        public void Suspend(EzTaskKey key)
        {
            _ezTaskHttp.Context.HttpContext.Session.Remove(key.ToString());
        }
    }
}
