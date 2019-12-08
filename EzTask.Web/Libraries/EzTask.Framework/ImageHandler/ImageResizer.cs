using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace EzTask.Framework.ImageHandler
{
    internal class ImageResizer : IDisposable
    {
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        ///     Resize image
        /// </summary>
        /// <param name="input"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public byte[] Resize(byte[] input, int w = 0, int h = 0, int quality = 100)
        {
            return ScaleImage(input, w, h, quality);
        }

        /// <summary>
        ///     Scales an image proportionally.  Returns a bitmap.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        private byte[] ScaleImage(byte[] data,
            int maxWidth, int maxHeight, int quality)
        {
            var ms = new MemoryStream(data);
            var image = Image.FromStream(ms);
            ms.Dispose();

            if (maxWidth == 0 && maxHeight == 0)
            {
                maxWidth = image.Width;
                maxHeight = image.Height;
            }

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            using (var stream = new MemoryStream())
            {
                var newImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                var bmp = new Bitmap(newImage);
                newImage.Dispose();
                image.Dispose();

                var encoder = ImageCodecInfo.GetImageEncoders()
                    .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                var encParams = new EncoderParameters(1)
                {
                    Param = { [0] = new EncoderParameter(Encoder.Quality, quality) }
                };

                bmp.Save(stream, encoder, encParams);
                bmp.Dispose();
                return stream.ToArray();
            }
        }
    }
}