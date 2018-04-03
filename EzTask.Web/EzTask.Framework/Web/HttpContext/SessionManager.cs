using EzTask.Framework.Values;
using Microsoft.AspNetCore.Http;

namespace EzTask.Framework.Web.HttpContext
{
    public class SessionManager
    {

        public void Set(EzTaskKey key, string value)
        {
            EzTaskContext.Current.Session.SetString(key.ToString(), value);
        }

        public string Get(EzTaskKey key)
        {
            try
            {
                return EzTaskContext.Current.Session.GetString(key.ToString());
            }
            catch
            {
                return null;
            }
        }

        public void SetObject(EzTaskKey key, object value)
        {
            EzTaskContext.Current.Session.SetObjectAsJson(key.ToString(), value);
        }

        public T GetObject<T>(EzTaskKey key)
        {
            try
            {
                return EzTaskContext.Current.Session.GetObjectFromJson<T>(key.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        public void Suspend(EzTaskKey key)
        {
            EzTaskContext.Current.Session.Remove(key.ToString());
        }
    }
}
