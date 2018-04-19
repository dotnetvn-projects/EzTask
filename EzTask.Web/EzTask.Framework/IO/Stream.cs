using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Framework.IO
{
    public class Stream
    {
        /// <summary>
        /// Convert stream to byte data
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<byte[]> ConvertStreamToBytes(System.IO.Stream source)
        {
            using (var ms = new MemoryStream())
            {
                await source.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Convert byte data to Image object
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<Image> ConvertStreamToImage(byte[] input)
        {
            var result = await Task.Factory.StartNew(() =>
              {
                  using (var ms = new MemoryStream())
                  {
                      var image = Image.FromStream(ms, false, false);
                      return image;
                  }
              });
            return result;
        }
    }
}
