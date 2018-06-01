using EzTask.Framework.Data;
using Microsoft.AspNetCore.Http;
using System;

namespace EzTask.Web.Framework.HttpContext
{
    public class CookiesManager
    {
        /// <summary>
        /// Set cookies
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public void Set(AppKey key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Context.Current.Response.Cookies.Append(key.ToString(), value, option);
        }

        /// <summary>
        /// Get cookies
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(AppKey key)
        {
            try
            {
                return Context.Current.Request.Cookies[key.ToString()];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set an object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public void SetObject(AppKey key, object value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Context.Current.Response.SetObjectAsJson(key.ToString(), value, option);
        }

        /// <summary>
        /// Get cookie and convert to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetObject<T>(AppKey key)
        {
            try
            {
                return Context.Current.Request.GetObjectFromJson<T>(key.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Remove cookie
        /// </summary>
        /// <param name="key"></param>
        public void Remove(AppKey key)
        {
            Context.Current.Response.Cookies.Delete(key.ToString());
        }
    }
}
