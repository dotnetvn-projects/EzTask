using EzTask.Framework.FrameworkObjects;
using Microsoft.AspNetCore.Http;

namespace EzTask.Framework.Web.HttpContext
{
    public class SessionManager
    {
        /// <summary>
        /// Set session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(AppKey key, string value)
        {
            Context.Current.Session.SetString(key.ToString(), value);
        }

        /// <summary>
        /// Get session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(AppKey key)
        {
            try
            {
                return Context.Current.Session.GetString(key.ToString());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set an object to session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetObject(AppKey key, object value)
        {
            Context.Current.Session.SetObjectAsJson(key.ToString(), value);
        }

        /// <summary>
        /// Get session and convert to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetObject<T>(AppKey key)
        {
            try
            {
                return Context.Current.Session.GetObjectFromJson<T>(key.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Remove session
        /// </summary>
        /// <param name="key"></param>
        public void Remove(AppKey key)
        {
            Context.Current.Session.Remove(key.ToString());
        }
    }
}
