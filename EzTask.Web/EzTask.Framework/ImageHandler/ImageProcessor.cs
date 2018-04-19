using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace EzTask.Framework.ImageHandler
{
    public class ImageProcessor
    {
        private int _quality = 80;

        /// <summary>
        /// Set quality
        /// </summary>
        /// <param name="quality"></param>
        public void SetQuality(int quality = 80)
        {
            if (_quality != quality)
            {
                _quality = quality;
            }
            else
            {
                _quality = quality;
            }
        }

        /// <summary>
        /// Compress image but keep original size
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public async Task<byte[]> Compress(byte[] input)
        {
            var data = await Task.Factory.StartNew(() =>
            {
                using (var resizer = new ImageResizer())
                {
                    return resizer.Resize(input, quality: _quality);
                }
            });
            return data;
        }

        /// <summary>
        ///    Compress image and resize
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public async Task<byte[]> CreateNewSize(byte[] input,
            int width,
            int height)
        {
            var data = await Task.Factory.StartNew(() =>
            {
                using (var resizer = new ImageResizer())
                {
                    return resizer.Resize(input, width, height, _quality);
                }
            });
            return data;
        }

        /// <summary>
        ///     Compress image and resize for mobile
        /// </summary>
        /// <param name="input"></param>
        public async Task<byte[]> CreateMobileSize(byte[] input)
        {
            var data = await Task.Factory.StartNew(() =>
            {
                using (var resizer = new ImageResizer())
                {
                    return resizer.Resize(input, 500, 500, quality: _quality);
                }
            });
            return data;
        }
    }
}